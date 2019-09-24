# Commodore 64 Emulator

One of the main goals of this project was to be able to run the **C64 KERNAL-** and **BASIC-ROM** on the **6502**. **BASIC** was one of my first introductions to programming and seeing that **`READY.`** prompt on my own C64 emulator so many years later was extremely pleasing. 😁

![](../../Screenshots/11-basic-prompt.png)

## Features / Roadmap

- [x] Loads the **KERNAL-**, **CHARACTER-** and **BASIC-ROM**
- [x] Boots into **COMMODORE 64 BASIC V2**
- [x] Simple character buffer view (with limited input possibilities)
- [x] Able to input BASIC program and run
- [x] Proper input/keyboard mapping
- [x] Real character mode output by using the CHARACTER GENERATOR ROM
- [x] Character mode output with char color data
- [ ] Full VIC-II emulation
- [ ] SID emulation



## Progress Screenshots
*Most recent screenshots first*.


***06 - I'm currently working on the UI. I added a tool bar with some commands and moved the fps-counter into a status bar. In this screenshot I have opened the `color_bars.prg` program and executed it.***

![](../../Screenshots/06-color-bars.png)


***05 - Preview of the second version of the C64 emulator running **COMMODORE 64 BASIC V2** and a simple BASIC program. This video output is using the CHARACTER ROM which is why it looks more like the real thing. Keyboard mapping is also improved in this version. And memory mapping is working as expected too, as we can tell from the `38911 BASIC BYTES FREE`-message!***

![](../../Gifs/03-character-rom-output.gif)


***04 - BASIC Program to print color bars on screen.***

![](../../Screenshots/03-foreground-color-bars-basic.png)


***03 - Output of BASIC program above.***

![](../../Screenshots/04-foreground-color-bars.png)


***02 - Preview of setting another background color by using the `POKE 53281,2` command.***

![](../../Screenshots/05-background-color.png)


***01 - Preview of the first version of the C64 emulator running **COMMODORE 64 BASIC V2** and a simple BASIC program. This video output isn't using the CHARACTER ROM.***

![](../../Gifs/01-simple-character-buffer-output.gif)