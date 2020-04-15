using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Commodore64.Cartridge.FileFormats.Crt {
    public class CrtFile {
        public CrtHeader Header { get; set; }
        public List<CrtChip> Chips { get; set; } = new List<CrtChip>();


        public static CrtFile FromBytes(byte[] data) {
            var crt = new CrtFile {
                Header = CrtHeader.FromBytes(data)
            };

            var offset = 0x40;

            while (offset < data.Length) {
                var chip = CrtChip.FromBytes(data.Skip(offset).ToArray());
                crt.Chips.Add(chip);
                offset += (int) chip.ChipLength;
            }

            return crt;
        }

        public static CrtFile FromFile(string path) {
            return FromBytes(File.ReadAllBytes(path));
        }
    }
}
