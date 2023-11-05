using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ConvertLib;
using Utils;

namespace Cli
{
    public class Viewer : IFileToHex
    {
        public bool BinaryData(long offset, string prettyFormat)
        {
            Console.WriteLine(prettyFormat);

            return (true);
        }

        public bool BinaryData(long offset, string strBinary, string strAscii)
        {
            Console.WriteLine(string.Format("Offset: {0}", BaseConvert.ToHex(offset).PadLeft(4, '0')));
            Console.WriteLine(string.Format("Hex: {0}", strBinary));
            Console.WriteLine(string.Format("Ascii: {0}", strAscii));

            return (true);
        }

        public void Error(string errMsg)
        {
            Console.WriteLine(errMsg);
        }
    }

    public class HexAsciiViewer
	{
        static void Main(string[] args)
        {
            CmdLine cmdLine = new CmdLine(args);
            string file = cmdLine.GetString("file");

            if (file == string.Empty)
            {
                DisplaySyntax();
                return;
            }

            try
            {
                FileToHex binConvert = new FileToHex(file);

                binConvert.EnumContent(0, new Viewer(), -1, true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void DisplaySyntax()
        {
            Console.WriteLine("HexAsciiViewer - Displays a file in binary hexadecimal format");
            Console.WriteLine("Syntax: HexAsciiViewer -file=\"<file>\"");
        }
    }
}
