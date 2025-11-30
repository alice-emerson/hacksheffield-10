use driver_v1::remote::{RemoteEvent, launch_receiver};
use evdev::{AbsInfo, AbsoluteAxisCode, AttributeSet, EventType, InputEvent, KeyCode, KeyEvent, UinputAbsSetup};

#[tokio::main]
pub async fn main() {
    let mut manager = launch_receiver().await;

    let mut i = 0;
    
    while let Some(mut remote) = manager.get_remote().await {
        i += 1;
        let name = format!("remote{i}");
        
        let mut buttons = AttributeSet::new();
        buttons.insert(KeyCode::BTN_TOUCH);
        buttons.insert(KeyCode::BTN_TOOL_PEN);

        let mut dev2 = evdev::uinput::VirtualDevice::builder().unwrap()
            .name(&name)
            .with_keys(&buttons).unwrap()
            .with_absolute_axis(&UinputAbsSetup::new(AbsoluteAxisCode::ABS_X, AbsInfo::new(0, 100, 1962, 0, 0, 100))).unwrap()
            .with_absolute_axis(&UinputAbsSetup::new(AbsoluteAxisCode::ABS_Y, AbsInfo::new(0, 0, 1335, 0, 0, 100))).unwrap()
            .with_absolute_axis(&UinputAbsSetup::new(AbsoluteAxisCode::ABS_PRESSURE, AbsInfo::new(0, 0, 512, 0, 0, 0))).unwrap()
            .build().unwrap();


        while let Some(evt) = remote.recv_event().await {
            match evt {
                // RemoteEvent::KeyDown(key) => todo!(),
                // RemoteEvent::KeyUp(key) => todo!(),
                RemoteEvent::Tablet(packet) => {
                    println!("{packet:?}");
                    let active = packet.x() != 4095 || packet.y() != 4095;
                    if active {
                        dev2.emit(&[
                            *KeyEvent::new(KeyCode::BTN_TOUCH, if packet.p() > 8 {1} else {0}),
                            *KeyEvent::new(KeyCode::BTN_TOOL_PEN, 1),
                            InputEvent::new(EventType::ABSOLUTE.0, AbsoluteAxisCode::ABS_X.0, packet.x() as i32),
                            InputEvent::new(EventType::ABSOLUTE.0, AbsoluteAxisCode::ABS_Y.0, 1420 - packet.y() as i32),
                            InputEvent::new(EventType::ABSOLUTE.0, AbsoluteAxisCode::ABS_PRESSURE.0, packet.p() as i32),
                        ]).unwrap();
                    } else {
                        dev2.emit(&[
                            *KeyEvent::new(KeyCode::BTN_TOUCH, 0),
                            *KeyEvent::new(KeyCode::BTN_TOOL_PEN, 0),
                        ]).unwrap();
                    }
                    // device.send(Digi::Touch, if packet.p() > 8 { 1 } else { 0 }).unwrap();
                    // device.send(Digi::Pen, if active { 1 } else { 0 }).unwrap();
                    // if active {
                    //     device.send(Position::X, packet.x() as i32).unwrap();
                    //     device.send(Position::Y, 1500 - packet.y() as i32).unwrap();
                    //     device.send(absolute::Digi::Pressure, packet.p() as i32).unwrap();
                    // }
                    // device.send(Mouse::Left, if packet.p() > 100 { 1 } else { 0 }).unwrap();
                    // device.synchronize().unwrap();
                },
                // RemoteEvent::Other(_) => todo!(),
                evt => println!("Event: {evt:?}")
            }
        }
    }
}