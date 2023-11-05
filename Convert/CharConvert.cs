using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace ConvertLib;

public class BaseConvert
{
    private static char[] HexTable = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

    public static string ToHex(int num, bool padIfByte = false)
    {
        string res = "";

        // Check if num is 0 and directly return "0"
        if (num == 0)
        {
            return (padIfByte == true) ? "00" : "0";
        }

        // If num > 0, use normal technique as discussed in other post
        if (num > 0)
        {
            while (num > 0)
            {
                res = HexTable[num % 16] + res;
                num /= 16;
            }

            if (padIfByte == true && res.Length == 1)
                res = "0" + res;
        }
        // If num < 0, we need to use the elaborated trick above
        else
        {
            // Convert num to 32-bit unsigned integer
            uint n = (uint)Math.Abs(num);
            // Use the same remainder technique
            while (n > 0)
            {
                res = HexTable[(int)(n % 16)] + res;
                n /= 16;
            }

            res = "-" + res;
        }

        return res;
    }

    public static string ToHex(long num)
    {
        string res = "";
        // Check if num is 0 and directly return "0"
        if (num == 0)
        {
            return "0";
        }
        // If num > 0, use normal technique as discussed in other post
        if (num > 0)
        {
            while (num > 0)
            {
                res = HexTable[num % 16] + res;
                num /= 16;
            }
        }
        // If num < 0, we need to use the elaborated trick above
        else
        {
            // Convert num to 32-bit unsigned integer
            ulong n = (ulong)Math.Abs(num);
            // Use the same remainder technique
            while (n > 0)
            {
                res = HexTable[(int)(n % 16)] + res;
                n /= 16;
            }
            res = "-" + res;
        }

        return res;
    }

    public static string ByteArrayToHex(byte[] array)
    {
        StringBuilder sb = new();

        foreach (byte b in array)
        {
            sb.Append(BaseConvert.ToHex(Convert.ToInt32(b), true));
        }

        return (sb.ToString());
    }

    public static string StringToHex(string str)
    {
        return ByteArrayToHex(Encoding.ASCII.GetBytes(str));
    }
}
