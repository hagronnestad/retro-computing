using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Commodore64.Cartridge.FileFormats.Crt {

    public class CrtHeader {

        public string Identifier { get; set; }
        public UInt32 Length { get; set; }
        public UInt16 Version { get; set; }
        public CrtHardwareType HardwareType { get; set; }
        public byte ExRomLine { get; set; }
        public byte GameLine { get; set; }
        public string Name { get; set; }

        public static CrtHeader FromBytes(byte[] data) {

            if (data.Length < 0x40) throw new ArgumentOutOfRangeException(nameof(data), "Length is less than minimum .CRT header length.");

            var h = new CrtHeader();

            h.Identifier = Encoding.ASCII.GetString(data, 0, 0x10).Trim();

            if (h.Identifier.ToUpper() != "C64 CARTRIDGE") throw new FileFormatException(".CRT header identifier is not correct.");

            h.Length = BitConverter.ToUInt32(data.Skip(0x10).Take(sizeof(UInt32)).Reverse().ToArray(), 0);
            h.Version = BitConverter.ToUInt16(data.Skip(0x14).Take(sizeof(UInt16)).Reverse().ToArray(), 0);
            h.HardwareType = (CrtHardwareType) BitConverter.ToUInt16(data.Skip(0x16).Take(sizeof(UInt16)).Reverse().ToArray(), 0);
            h.ExRomLine = data[0x18];
            h.GameLine = data[0x19];
            h.Name = Encoding.ASCII.GetString(data, 0x20, 0x20).Trim('\0', ' ');


            return h;
        }
    }
}
