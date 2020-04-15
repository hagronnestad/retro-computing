using Hardware.Memory;

namespace Commodore64.Cartridge {
    public interface ICartridge : IMemory<byte> {

        string Id { get; }
        bool ControlLineExRom { get; }
        bool ControlLineGame { get; }
        string Name { get; }

    }
}
