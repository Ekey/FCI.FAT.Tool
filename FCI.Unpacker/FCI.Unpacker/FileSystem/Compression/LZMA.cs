using System;
using System.IO;
using System.Linq;
using SevenZip.Compression.LZMA;

namespace FCI.Unpacker
{
    class LZMA
    {
        public static Byte[] iDecompress(Byte[] lpSrcBuffer)
        {
            lpSrcBuffer = lpSrcBuffer.Skip(1).ToArray();

            using (MemoryStream TDstMemoryStream = new MemoryStream())
            {
                using (MemoryStream TSrcMemoryStream = new MemoryStream(lpSrcBuffer))
                {
                    Decoder LZMADecoder = new Decoder();

                    var lpProperties = TSrcMemoryStream.ReadBytes(5);
                    Int64 dwUncompressedSize = TSrcMemoryStream.ReadInt64();

                    LZMADecoder.SetDecoderProperties(lpProperties);
                    LZMADecoder.Code(TSrcMemoryStream, TDstMemoryStream, TSrcMemoryStream.Length, dwUncompressedSize, null);
                }

                return TDstMemoryStream.ToArray();
            }
        }
    }
}
