use std::fmt::{self, Debug};

#[repr(transparent)]
#[derive(Clone, Copy, PartialEq, Eq)]
pub struct UDrawPacket([u8; 6]);

impl UDrawPacket {
    pub const fn from_bytes(bytes: [u8; 6]) -> Self {
        Self(bytes)
    }

    pub const fn x(self) -> u16 {
        let Self([b0, _, b2, _, _, _]) = self;
        let lower = b0 as u16;
        let upper = (b2 & 0b0000_1111) as u16;
        (upper << 8) | lower
    }

    pub const fn y(self) -> u16 {
        let Self([_, b1, b2, _, _, _]) = self;
        let lower = b1 as u16;
        let upper = ((b2 & 0b1111_0000) >> 4) as u16;
        (upper << 8) | lower
    }

    pub const fn p(self) -> u16 {
        let Self([_, _, _, b3, _, b5]) = self;
        let lower = b3 as u16;
        let upper = ((b5 & 0b0000_0100) >> 2) as u16;
        (upper << 8) | lower
    }

    pub const fn l(self) -> bool {
        self.0[5] & 0b0000_0010 == 0
    }

    pub const fn u(self) -> bool {
        self.0[5] & 0b0000_0001 == 0
    }
}

impl Debug for UDrawPacket {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "UDrawPacket[x={}, y={}, p={}, l={}, u={}]", self.x(), self.y(), self.p(), self.l(), self.u())
    }
}