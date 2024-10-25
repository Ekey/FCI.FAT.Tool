using System;

namespace FCI.Unpacker
{
    class FatEntry
    {
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwUncompressedSize { get; set; }
        public UInt32 dwNameHash { get; set; }
    }
}
