using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Commodore64.Cartridge.FileFormats.Crt
{

    public class CrtChip
    {

        public string Identifier { get; set; }
        public UInt32 ChipLength { get; set; }
        public CrtChipType ChipType { get; set; }
        public UInt16 Bank { get; set; }
        public UInt16 Address { get; set; }
        public UInt16 Length { get; set; }

        public byte[] Data { get; set; }


        public static CrtChip FromBytes(byte[] data)
        {

            var c = new CrtChip();

            c.Identifier = Encoding.ASCII.GetString(data, 0, 0x04);

            if (c.Identifier.ToUpper() != "CHIP") throw new FileFormatException(".CRT CHIP-packet identifier is not correct.");

            c.ChipLength = BitConverter.ToUInt32(data.Skip(0x04).Take(sizeof(UInt32)).Reverse().ToArray(), 0);
            c.ChipType = (CrtChipType)BitConverter.ToUInt16(data.Skip(0x08).Take(sizeof(UInt16)).Reverse().ToArray(), 0);
            c.Bank = BitConverter.ToUInt16(data.Skip(0x0A).Take(sizeof(UInt16)).Reverse().ToArray(), 0);
            c.Address = BitConverter.ToUInt16(data.Skip(0x0C).Take(sizeof(UInt16)).Reverse().ToArray(), 0);
            c.Length = BitConverter.ToUInt16(data.Skip(0x0E).Take(sizeof(UInt16)).Reverse().ToArray(), 0);

            c.Data = data.Skip(0x10).Take(c.Length).ToArray();

            return c;
        }

    }

}
