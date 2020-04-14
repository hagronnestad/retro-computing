using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commodore64.Cartridge.FileFormats.Crt {
    public class CrtFile {
        public CrtHeader Header { get; set; }
        public List<CrtChip> Chips { get; set; } = new List<CrtChip>();


        public static CrtFile FromBytes(byte[] data) {
            var crt = new CrtFile();

            crt.Header = CrtHeader.FromBytes(data);

            // TODO: Add support for multiple CHIP-sections
            crt.Chips.Add(CrtChip.FromBytes(data.Skip(0x40).ToArray()));

            return crt;
        }

        public static CrtFile FromFile(string path) {
            return FromBytes(File.ReadAllBytes(path));
        }
    }
}
