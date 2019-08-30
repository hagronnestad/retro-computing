# 6502

### This is my implementation of the **MOS Technology 6502** 8-bit microprocessor.

> *"The **6502** and variations of it is used in many of the old home video game consoles and computers, such as the Atari 2600, Atari 8-bit family, Apple II, Nintendo Entertainment System, Commodore 64, Atari Lynx, BBC Micro and others."*

‼ ***This project is intended for educational use only!*** ‼

I implemented the 6502 in about a weeks worth of late nights, including testing and bug fixes. I tried to use original and reverse engineered documentation rather than looking at other software implementations so that I would learn as much as possible about every part of the chip. I did however look up logic from different software implementations when I had specific bugs that I couldn't figure out after spending a lot of time debugging.


## In this repository

### MOS Technology 6502
- C#.NET implementation of the **MOS Technology 6502** 8-bit microprocessor
- Simple debugger with memory view/watches

### Commodore 64 emulator
- [x] Loads the **KERNAL**, **CHARACTER** and **BASIC** ROM
- [x] Boots into **COMMODORE 64 BASIC V2**
- [x] Simple character buffer view (with limited input possibilities)
- [x] Able to input BASIC program and run
- [ ] Proper input/keyboard mapping
- [ ] Real character mode output in color using the character ROM
- [ ] VIC-II emulation? (probably never)
- [ ] SID emulation? (hopefully)

### Screenshots

![](Gifs/01-simple-character-buffer-output.gif)

***Preview of a simple C64 emulator running **COMMODORE 64 BASIC V2** and a simple BASIC program.***
