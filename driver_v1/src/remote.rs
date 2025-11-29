use std::{sync::Arc, time::{Duration, Instant}};

use log::warn;
use tokio::{sync::mpsc, task::JoinHandle};
use wiimote_rs::{input::InputReport, output::{Addressing, OutputReport}, prelude::*};

mod udraw;

pub use udraw::UDrawPacket;

#[derive(Clone, Debug)]
pub enum RemoteEvent {
    KeyDown(Key),
    KeyUp(Key),
    Tablet(UDrawPacket),
    Other(String),
}

pub struct CallibrationData {
    pub x_min: u16,
    pub x_max: u16,
    pub y_min: u16,
    pub y_max: u16,
}

#[derive(Clone, Copy, Debug)]
pub enum Key {
    DPadUp,
    DPadDown,
    DPadLeft,
    DPadRight,
    BtnPower,
    BtnA,
    BtnB,
    BtnMinus,
    BtnPlus,
    Btn1,
    Btn2,
    BtnHome,
}

#[derive(Debug)]
pub struct RemoteHandle {
    events: mpsc::UnboundedReceiver<RemoteEvent>,
    task: JoinHandle<WiimoteResult<()>>,
}

impl RemoteHandle {
    pub async fn recv_event(&mut self) -> Option<RemoteEvent> {
        self.events.recv().await
    }
}

#[derive(Debug)]
pub struct ManagerHandle {
    remotes: mpsc::UnboundedReceiver<RemoteHandle>,
    task: JoinHandle<()>,
}

impl ManagerHandle {
    pub async fn get_remote(&mut self) -> Option<RemoteHandle> {
        self.remotes.recv().await
    }
}

pub async fn launch_receiver() -> ManagerHandle {
    let (send, recv) = mpsc::unbounded_channel();

    let task = tokio::task::spawn_blocking(move || {
        run_manager(send);
    });

    ManagerHandle { remotes: recv, task }
}

fn run_manager(tx_remotes: mpsc::UnboundedSender<RemoteHandle>) {
    let manager = WiimoteManager::get_instance();

    let remotes = {
        let manager = manager.lock().expect("Failed to open device manager");
        let seen = manager.seen_devices();
        let recv = manager.new_devices_receiver();
        seen.into_iter().chain(recv)
    };

    for remote in remotes {
        match tx_remotes.send(run_remote(remote)) {
            Ok(()) => (),
            Err(e) => {
                warn!("Failed to send error: {e}")
            }
        }
    }
}

const READ_TIMEOUT: usize = 10;
const POLL_INTERVAL: Duration = Duration::from_millis(25);

const READ_UDRAW: OutputReport = OutputReport::ReadMemory(Addressing::control_registers(0xA4AD00, 6));

fn run_remote(remote: Arc<std::sync::Mutex<WiimoteDevice>>) -> RemoteHandle {
    let (send, recv) = mpsc::unbounded_channel();

    let task = tokio::task::spawn_blocking(move || {
        let mut last_poll = Instant::now();
        loop {
            let remote = remote.lock().expect("Failed to lock remote");
            let now = Instant::now();
            if now - last_poll > POLL_INTERVAL {
                last_poll = now;
                remote.write(&READ_UDRAW)?;
            }
            if let Ok(evt) = remote.read_timeout(READ_TIMEOUT) {
                match evt {
                    InputReport::StatusInformation(status_data) => eprintln!("STA {status_data:?}"),
                    InputReport::ReadMemory(memory_data) => {
                        if memory_data.address_offset() == 0xAD00 {
                            // uDraw
                            let [b0, b1, b2, b3, b4, b5, ..] = memory_data.data;
                            let packet = UDrawPacket::from_bytes([b0, b1, b2, b3, b4, b5]);
                            match send.send(RemoteEvent::Tablet(packet)) {
                                Ok(()) => {}
                                Err(e) => warn!("Send failed: {e}"),
                            }
                        } else {
                            eprintln!("MEM {memory_data:?}");
                        }
                    },
                    InputReport::Acknowledge(acknowledge_data) => eprintln!("ACK {acknowledge_data:?}"),
                    InputReport::DataReport(id, wiimote_data) => eprintln!("DAT {id} {wiimote_data:?}"),
                }
            }
        }
    });

    RemoteHandle { events: recv, task }
}