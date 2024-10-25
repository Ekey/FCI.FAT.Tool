using System;

namespace FCI.Unpacker
{
    class FatHeader
    {
        public UInt32 dwMagic { get; set; } // 0x32544144 (DAT2)
        public Int32 dwTotalFiles { get; set; }
    }
}
