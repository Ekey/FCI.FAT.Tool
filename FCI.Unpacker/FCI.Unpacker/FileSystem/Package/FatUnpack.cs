using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FCI.Unpacker
{
    class FatUnpack
    {
        private static Boolean bOldFatFormat = false;
        private static List<FatEntry> m_EntryTable = new List<FatEntry>();

        public static void iDoIt(String m_FatFile, String m_DstFolder)
        {
            FatHashList.iLoadProject();

            using (FileStream TFatStream = File.OpenRead(m_FatFile))
            {
                var m_Header = new FatHeader();

                m_Header.dwMagic = TFatStream.ReadUInt32();
                m_Header.dwTotalFiles = TFatStream.ReadInt32();

                if (m_Header.dwMagic != 0x32544144)
                {
                    throw new Exception("[ERROR]: Invalid magic of FAT file!");
                }

                Int64 dwFatSize = TFatStream.Length;
                if ((m_Header.dwTotalFiles * 12) + 8 == dwFatSize)
                {
                    bOldFatFormat = true;
                }
                else
                {
                    bOldFatFormat = false;
                }


                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new FatEntry();

                    m_Entry.dwOffset = TFatStream.ReadUInt32();
                    if (!bOldFatFormat)
                    {
                        m_Entry.dwCompressedSize = TFatStream.ReadInt32();
                    }
                    m_Entry.dwUncompressedSize = TFatStream.ReadInt32();
                    m_Entry.dwNameHash = TFatStream.ReadUInt32();

                    m_EntryTable.Add(m_Entry);
                }

                TFatStream.Dispose();
            }

            String m_DatFile = Path.GetDirectoryName(m_FatFile) + @"\"+ Path.GetFileNameWithoutExtension(m_FatFile) + ".dat";
            using (FileStream TDatStream = File.OpenRead(m_DatFile))
            {
                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = FatHashList.iGetNameFromHashList(m_Entry.dwNameHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TDatStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    if (m_Entry.dwCompressedSize == m_Entry.dwUncompressedSize || m_Entry.dwCompressedSize == 0)
                    {
                        var lpBuffer = TDatStream.ReadBytes(m_Entry.dwUncompressedSize);
                        
                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                    else
                    {
                        var lpScrBuffer = TDatStream.ReadBytes(m_Entry.dwCompressedSize);
                        File.WriteAllBytes(m_FullPath, lpScrBuffer);
                        var lpDstBuffer = LZMA.iDecompress(lpScrBuffer);
                        
                        File.WriteAllBytes(m_FullPath, lpDstBuffer);
                    }
                }

                TDatStream.Dispose();
            }
        }
    }
}
