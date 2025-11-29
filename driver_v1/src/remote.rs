use tokio::{sync::mpsc, task::JoinHandle};
use wiimote_rs::prelude::*;

pub enum RemoteEvent {
    KeyDown(Key),
    KeyUp(Key),
    Tablet(TabletUpdate)
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

pub struct RemoteHandle {
    events: mpsc::UnboundedReceiver<RemoteEvent>,
}

pub struct ManagerHandle {
    remotes: mpsc::UnboundedReceiver<RemoteHandle>,
    task: JoinHandle<()>,
}

pub async fn launch_receiver() -> ManagerHandle {
    let (send, recv) = mpsc::unbounded_channel();

    let task = tokio::task::spawn_blocking(move || {
        let send = send;
    });

    ManagerHandle { remotes: recv, task }
}

fn run_manager(tx_remotes: mpsc::UnboundedSender<RemoteHandle>) {
    let manager = WiimoteManager::get_instance();

    let (recv, seen) = {
        let manager = manager.lock().unwrap();
        let recv = manager.new_devices_receiver();
        let seen = manager.seen_devices();
        (recv, seen)
    };
}