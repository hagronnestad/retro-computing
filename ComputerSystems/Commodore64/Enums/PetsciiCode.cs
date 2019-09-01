using Commodore64.Attributes;

namespace Commodore64.Enums {
    public enum PetsciiCode {
        // Control chars
        [Petscii(AsciiCode = 0, PetsciiCode = 147, KeyCombination = "SHIFT + CLR/HOME", Description = "Clears screen")] ShiftClrHome = 147,
        [Petscii(AsciiCode = 0, PetsciiCode = 19, KeyCombination = "CLR/HOME or CTRL + S", Description = "Place cursor in top left corner")] ClrHome = 19,
        [Petscii(AsciiCode = 0, PetsciiCode = 29, KeyCombination = "CRSR ⇐ ⇒", Description = "Cursor one step right")] CrSrLeftRight = 29,
        [Petscii(AsciiCode = 0, PetsciiCode = 157, KeyCombination = "SHIFT + CRSR ⇐ ⇒ ", Description = "Cursor one step to the left")] ShiftCrSrLeftRight = 157,
        [Petscii(AsciiCode = 0, PetsciiCode = 17, KeyCombination = "CRSR ⇑ ⇓ or CTRL + Q", Description = "Cursor one position down")] CrSrUpDown = 17,
        [Petscii(AsciiCode = 0, PetsciiCode = 145, KeyCombination = "SHIFT + CRSR ⇑ ⇓", Description = "Cursor one position up")] ShiftCrSrUpDown = 145,
        [Petscii(AsciiCode = 0, PetsciiCode = 144, KeyCombination = "CTRL + 1", Description = "Character color black")] Ctrl1 = 144,
        [Petscii(AsciiCode = 0, PetsciiCode = 5, KeyCombination = "	CTRL + 2 or CTRL + E", Description = "Character color white")] Ctrl2 = 5,
        [Petscii(AsciiCode = 0, PetsciiCode = 28, KeyCombination = "CTRL + 3", Description = "Character color red")] Ctrl3 = 28,
        [Petscii(AsciiCode = 0, PetsciiCode = 159, KeyCombination = "CTRL + 4", Description = "Character color cyan")] Ctrl4 = 159,
        [Petscii(AsciiCode = 0, PetsciiCode = 156, KeyCombination = "CTRL + 5", Description = "Character color purple")] Ctrl5 = 156,
        [Petscii(AsciiCode = 0, PetsciiCode = 30, KeyCombination = "CTRL + 6", Description = "Character color green")] Ctrl6 = 30,
        [Petscii(AsciiCode = 0, PetsciiCode = 31, KeyCombination = "CTRL + 7", Description = "Character color blue")] Ctrl7 = 31,
        [Petscii(AsciiCode = 0, PetsciiCode = 158, KeyCombination = "CTRL + 8", Description = "Character color yellow")] Ctrl8 = 158,
        [Petscii(AsciiCode = 0, PetsciiCode = 18, KeyCombination = "CTRL + 9 or CTRL + R", Description = "Reverse ON")] Ctrl9 = 18,
        [Petscii(AsciiCode = 0, PetsciiCode = 146, KeyCombination = "CTRL + 0", Description = "Reverse OFF")] Ctrl0 = 146,
        [Petscii(AsciiCode = 0, PetsciiCode = 129, KeyCombination = "C= + 1", Description = "Character color orange")] Commodore1 = 129,
        [Petscii(AsciiCode = 0, PetsciiCode = 149, KeyCombination = "C= + 2", Description = "Character color brown")] Commodore2 = 149,
        [Petscii(AsciiCode = 0, PetsciiCode = 150, KeyCombination = "C= + 3", Description = "Character color pink")] Commodore3 = 150,
        [Petscii(AsciiCode = 0, PetsciiCode = 151, KeyCombination = "C= + 4", Description = "Character color grey 1 / dark grey")] Commodore4 = 151,
        [Petscii(AsciiCode = 0, PetsciiCode = 152, KeyCombination = "C= + 5", Description = "Character color grey 2")] Commodore5 = 152,
        [Petscii(AsciiCode = 0, PetsciiCode = 153, KeyCombination = "C= + 6", Description = "Character color light green")] Commodore6 = 153,
        [Petscii(AsciiCode = 0, PetsciiCode = 154, KeyCombination = "C= + 7", Description = "Character color light blue")] Commodore7 = 154,
        [Petscii(AsciiCode = 0, PetsciiCode = 155, KeyCombination = "C= + 8", Description = "Character color grey 3 / light grey")] Commodore8 = 155,
        [Petscii(AsciiCode = 0, PetsciiCode = 20, KeyCombination = "INST/DEL or CTRL + T", Description = "Erases character left of cursor.")] InstDel = 20,
        [Petscii(AsciiCode = 0, PetsciiCode = 148, KeyCombination = "SHIFT + INST/DEL", Description = "Move characters to the right to insert character.")] ShiftInstDel = 148,
        [Petscii(AsciiCode = 0, PetsciiCode = 3, KeyCombination = "RUN/STOP * or CTRL + C", Description = "Uset by GET query if RUN/STOP has been deactivated")] RunStop = 3,
        [Petscii(AsciiCode = 0, PetsciiCode = 8, KeyCombination = "CTRL + H", Description = "Deactivates case change SHIFT + C=")] CtrlH = 8,
        [Petscii(AsciiCode = 0, PetsciiCode = 9, KeyCombination = "CTRL + I", Description = "Activates case change SHIFT + C=")] CtrlI = 9,
        [Petscii(AsciiCode = 0, PetsciiCode = 13, KeyCombination = "RETURN or CTRL + M", Description = "Return to enter data")] Return = 13,
        [Petscii(AsciiCode = 0, PetsciiCode = 32, KeyCombination = "SPACE", Description = "Empty character")] Space = 32,
        [Petscii(AsciiCode = 0, PetsciiCode = 141, KeyCombination = "SHIFT + RETURN", Description = "New row")] ShiftReturn = 141
    }
}

