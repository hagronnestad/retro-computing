using Memory;
using System;
using System.IO;

namespace Commodore64.Cartridge.FileFormats.Raw
{
    /// <summary>
    /// Cartridge implementation for raw 8/16K cartridges
    /// </summary>
    public class CartridgeRaw : ICartridge
    {
        private byte[] _rom;

        public event EventHandler<MemoryReadEventArgs<byte>> OnRead;
        public event EventHandler<MemoryWriteEventArgs<byte>> OnWrite;

        public byte this[int i]
        {
            get => Read(i);
            set => Write(i, value);
        }

        public string Id => "raw";
        public bool ControlLineExRom => false;
        public bool ControlLineGame => true;
        public string Name => "Raw Cartridge";
        public bool IsReadOnly => true;


        public byte Read(int address)
        {
            return _rom[address - 0x8000];
        }

        public void Write(int address, byte value)
        {
            throw new NotImplementedException();
        }

        public static CartridgeRaw FromFile(string path)
        {
            return new CartridgeRaw() {
                _rom = File.ReadAllBytes(path)
            };
        }
    }
}
