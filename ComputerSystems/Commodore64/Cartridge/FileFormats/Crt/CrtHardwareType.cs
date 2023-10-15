using System;

namespace Commodore64.Cartridge.FileFormats.Crt
{

    public enum CrtHardwareType : UInt16
    {
        NORMAL_CARTRIDGE = 0,
        ACTION_REPLAY = 1,
        KCS_POWER_CARTRIDGE = 2,
        FINAL_CARTRIDGE_III = 3,
        SIMONS_BASIC = 4,
        OCEAN_TYPE_1_256_AND_128_KB = 5,
        EXPERT_CARTRIDGE = 6,
        FUN_PLAY = 7,
        SUPER_GAMES = 8,
        ATOMIC_POWER = 9,
        EPYX_FASTLOAD = 10,
        WESTERMANN = 11,
        REX = 12,
        FINAL_CARTRIDGE_I = 13,
        MAGIC_FORMEL = 14,
        C64_GAME_SYSTEM = 15,
        WARPSPEED = 16,
        DINAMIC = 17,
        ZAXXON = 18,
        MAGIC_DESK, _DOMARK, _HES_AUSTRALIA = 19,
        SUPER_SNAPSHOT_5 = 20,
        COMAL_80 = 21,
    }

}
