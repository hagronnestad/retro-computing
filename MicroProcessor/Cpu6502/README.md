# MOS Technology 6502 8-bit microprocessor

> *"The **6502** and variations of it is used in many of the old home video game consoles and computers, such as the Atari 2600, Atari 8-bit family, Apple II, Nintendo Entertainment System, Commodore 64, Atari Lynx, BBC Micro and others."*

I've always wanted to write my own implementation of this legendary chip and that's why this project exists. The main goal was to get the **6502**-implementation working and being able to run **Commodore 64 BASIC** on it.

I implemented the 6502 in about a weeks worth of late nights, including testing and bug fixes. I tried to use original and reverse engineered documentation rather than looking at other software implementations so that I would learn as much as possible about every part of the chip. I did however look up logic from different software implementations when I had specific bugs that I couldn't figure out after spending a lot of time debugging.

## Features / Roadmap
- [x] C#.NET implementation of the **MOS Technology 6502** 8-bit microprocessor
- [x] All official/legal instructions tested by running `nestest.nes` and comparing to the golden log from `Nintendulator`
  - https://wiki.nesdev.com/w/index.php/Emulator_tests#CPU_Tests
  - http://www.qmtpro.com/~nes/misc/nestest.txt
  - http://www.qmtpro.com/~nes/misc/nestest.log
- [ ] Cycle perfect emulation
- [ ] Decimal mode