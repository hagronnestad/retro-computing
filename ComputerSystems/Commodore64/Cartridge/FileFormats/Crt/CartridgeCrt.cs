using Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commodore64.Cartridge.FileFormats.Crt
{

    public class CartridgeCrt : ICartridge
    {

        public CrtHeader Header { get; set; }
        public List<CrtChip> Chips { get; set; } = new List<CrtChip>();

        public static CartridgeCrt FromBytes(byte[] data)
        {
            var crt = new CartridgeCrt
            {
                Header = CrtHeader.FromBytes(data)
            };

            var offset = 0x40;

            while (offset < data.Length)
            {
                var chip = CrtChip.FromBytes(data.Skip(offset).ToArray());
                crt.Chips.Add(chip);
                offset += (int)chip.ChipLength;
            }

            return crt;
        }

        public static CartridgeCrt FromFile(string path)
        {
            return FromBytes(File.ReadAllBytes(path));
        }


        // ICartridge implementation

        public event EventHandler<MemoryReadEventArgs<byte>> OnRead;
        public event EventHandler<MemoryWriteEventArgs<byte>> OnWrite;

        public byte this[int i]
        {
            get => Read(i);
            set => Write(i, value);
        }

        public string Id => Header?.Identifier;
        public bool ControlLineExRom => Header?.ExRomLine == 1;
        public bool ControlLineGame => Header?.GameLine == 1;
        public string Name => Header.Name;
        public bool IsReadOnly => true;

        public byte Read(int address)
        {
            var chip = Chips.FirstOrDefault(x => address >= x.Address && address <= (x.Address + x.Length));
            return chip.Data[address - chip.Address];
        }

        public void Write(int address, byte value)
        {
            throw new NotImplementedException();
        }

    }

}
