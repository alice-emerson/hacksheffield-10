use std::{sync::Arc, time::{Duration, Instant}};

use log::warn;
use tokio::{sync::mpsc, task::JoinHandle};
use wiimote_rs::{input::InputReport, output::{Addressing, OutputReport}, prelude::*};

pub enum RemoteEvent {
    KeyDown(Key),
    KeyUp(Key),
    Tablet(TabletUpdate),
    Other(String),
}

pub struct TabletUpdate {
    pub x: f64,
    pub y: f64,
    pub p: f64,
}

pub struct CallibrationData {
    pub x_min: u16,
    pub x_max: u16,
    pub y_min: u16,
    pub y_max: u16,
}

pub enum Key {
    PenL,
    PenU,
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

#[derive(Debug)]
pub struct ManagerHandle {
    remotes: mpsc::UnboundedReceiver<RemoteHandle>,
    task: JoinHandle<()>,
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

const READ_UDRAW: OutputReport = OutputReport::ReadMemory(Addressing::control_registers(0xA40000, 6));

fn run_remote(remote: Arc<std::sync::Mutex<WiimoteDevice>>) -> RemoteHandle {
    println!("Remote connected!");
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
                    InputReport::StatusInformation(status_data) => println!("STA {status_data:?}"),
                    InputReport::ReadMemory(memory_data) => println!("MEM {memory_data:?}"),
                    InputReport::Acknowledge(acknowledge_data) => println!("ACK {acknowledge_data:?}"),
                    InputReport::DataReport(id, wiimote_data) => println!("DAT {id} {wiimote_data:?}"),
                }
            }
        }
    });

    RemoteHandle { events: recv, task }
}