using System.Diagnostics;
using System.Text;

namespace ConvertLib
{
	public interface IFileToHex
	{
        /// <summary>
        /// Returns pretty formatted binary converted line:
        /// </summary>
        /// <param name="offset">16 byte offset start</param>
        /// <param name="prettyFormat">0010: 73 61 6D 70  6C 65 20 61  73 63 69 69  20 73 74 72  sample ascii str</param>
        /// <returns>True if enumeration should continue</returns>
        bool BinaryData(long offset, string prettyFormat);

        /// <summary>
        /// Returns raw formatted binary converted line:
        /// </summary>
        /// <param name="offset">0010</param>
        /// <param name="strBinary">73616D706C6520617363696920737472</param>
        /// <param name="strAscii">sample ascii str</param>
        /// <returns>True if enumeration should continue</returns>
        bool BinaryData(long offset, string strBinary, string strAscii);

        void Error(string errMsg);
	}

	public class FileToHex
	{
        private string strFile;
        private const int maxCharsToRead = 16;

        public FileToHex(string file)
		{
            if (File.Exists(file) == false)
            {
                throw new FileNotFoundException(string.Format("File was not found: {0}", file));
            }

            strFile = file;
		}

        public void EnumContent(long startOffset, IFileToHex iCallback, long numChars = -1, bool pretty = false)
        {
            try
            {
                using (FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read))
                {
                    if (numChars == -1)
                    {
                        numChars = fs.Length;
                    }

                    string offsetErr = string.Format("Could not seek to specified offset: {0}", startOffset);

                    long lPos = fs.Seek(startOffset, SeekOrigin.Begin);
                    Debug.Assert(lPos == startOffset, offsetErr);

                    if (lPos != startOffset)
                    {
                        throw new IOException(offsetErr);
                    }

                    BinaryReader br = new BinaryReader(fs);
                    bool shouldContinue = true;

                    while(numChars > 0 && shouldContinue == true)
                    {
                        int readChars = int.Min(Convert.ToInt32(numChars), maxCharsToRead);

                        byte[] bytes = br.ReadBytes(readChars);
                        int bytesRead = bytes.Count();

                        numChars -= bytesRead;

                        string str = BaseConvert.ByteArrayToHex(bytes);

                        if (pretty == false)
                        {
                            shouldContinue = iCallback.BinaryData(
                                startOffset, str,
                                ToAscii(bytes)
                                );
                        }
                        else
                        {
                            shouldContinue = iCallback.BinaryData(startOffset, ToPretty(startOffset, str, ToAscii(bytes)));
                        }

                        startOffset += bytesRead;
                    }
                }
            }
            catch(IOException ex)
            {
                iCallback.Error(ex.Message);
            }
        }

        private static string ToPretty(long startOffset, string strBinary, string strAscii)
        {
            strBinary = PadBinaryData(strBinary);
            return string.Format("{0:D8}: {1}{2}", BaseConvert.ToHex(startOffset).PadLeft(8, '0'), strBinary, strAscii);
        }

        private static string ToAscii(byte[] bytes)
        {
            StringBuilder sb = new();

            foreach(byte b in bytes)
            {
                // Ensure byte is printable
                if (b >= 32 && b <= 126)
                {
                    sb.Append((char)b);
                }
                else
                {
                    sb.Append('.');
                }
            }

            return sb.ToString();
        }

        private static string PadBinaryData(string strBinary)
        {
            const int calculatedLength = 52;

            StringBuilder sb = new();
            int count = strBinary.Count();
            int spaceIndex = 0;

            for (int idx = 0; idx < count; idx += 2)
            {
                sb.Append(strBinary[idx]);
                sb.Append(strBinary[idx+1]);

                if (++spaceIndex % 4 == 0)
                {
                    sb.Append("  ");
                }
                else
                {
                    sb.Append(" ");
                }
            }

            // End of file might not terminated exactly on the expected
            // boundary so insert spaces so that printable Ascii characters
            // that will be added show up nicely aligned.

            int len = sb.ToString().Length;

            if (len < calculatedLength)
            {
                len = calculatedLength - len;

                while (len-- > 0)
                {
                    sb.Append(" ");
                }
            }

            return (sb.ToString());
        }
    }
}

