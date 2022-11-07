using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commodore64.Vic
{
    public class VicIiSprite
    {
        private int VIC_II_BASE_ADDRESS = 0xD000;

        public int X;
        public byte Y;

        public static VicIiSprite GetSprite(byte[] registers, byte index)
        {
            var s = new VicIiSprite();

            var offset = index * 2;

            s.X = (registers[offset] | (((registers[0x10] >> index) & 0x01) << 9));
            s.Y = registers[offset + 1];

            return s;
        }
    }
}
