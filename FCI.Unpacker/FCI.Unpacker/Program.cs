using System;
using System.IO;

namespace FCI.Unpacker
{
    class Program
    {
        public static String m_Title = "Far Cry Instincts FAT/DAT Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    FCI.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of FAT/DAT file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    FCI.Unpacker E:\\Games\\Far Cry Instincts\\FarCryEd.fat D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_File = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_File))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_File + " <- does not exist");
                return;
            }

            if (Path.GetExtension(m_File) == ".dat")
                m_File = m_File.Replace(".dat", ".fat");

            FatUnpack.iDoIt(m_File, m_Output);
        }
    }
}
