using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commodore64.Cia.Enums {
    /// <summary>
    /// CIA2 REGISTER ADDRESSES 0xDD00-0xDD0F
    /// </summary>
    public enum Register : byte {
        R_0x00_PORT_A = 0x00,
        R_0x01_PORT_B = 0x01,
        R_0x02_PORT_A_DATA_DIRECTION = 0x02,
        R_0x03_PORT_B_DATA_DIRECTION = 0x03,
    }
}
