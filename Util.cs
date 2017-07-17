using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkLibrary
{
    public class Util
    {
        public static byte[] IntToByteArr(int n)
        {
            byte[] buff = new byte[4];
            buff[0] = (byte)(n >> 24);
            buff[1] = (byte)(n >> 16);
            buff[2] = (byte)(n >> 8);
            buff[3] = (byte)n;
            return buff;
        }

        public static void IntToByteArr(int n, byte[] dest, int offset)
        {
            dest[offset] = (byte)(n >> 24);
            dest[offset + 1] = (byte)(n >> 16);
            dest[offset + 2] = (byte)(n >> 8);
            dest[offset + 3] = (byte)n;
        }

        public static void UIntToByteArr(uint n, byte[] dest, int offset)
        {
            dest[offset] = (byte)(n >> 24);
            dest[offset + 1] = (byte)(n >> 16);
            dest[offset + 2] = (byte)(n >> 8);
            dest[offset + 3] = (byte)n;
        }

        public static byte[] FloatToByteArr(float f)
        {
            return BitConverter.GetBytes(f);
        }

        public static void FloatToByteArr(float f, byte[] dest, int offset)
        {
            byte[] buff = BitConverter.GetBytes(f);
            dest[offset] = buff[0];
            dest[offset + 1] = buff[1];
            dest[offset + 2] = buff[2];
            dest[offset + 3] = buff[3];
        }

        public static byte[] LongToByteArr(long n)
        {
            return BitConverter.GetBytes(n);
        }

        public static void LongToByteArr(long n, byte[] dest, int offset)
        {
            byte[] buff = BitConverter.GetBytes(n);
            Buffer.BlockCopy(buff, 0, dest, offset, buff.Length);
        }

        public static byte[] ULongToByteArr(ulong un)
        {
            byte[] buff = new byte[8];
            buff[0] = (byte)(un >> 56);
            buff[1] = (byte)(un >> 48);
            buff[2] = (byte)(un >> 40);
            buff[3] = (byte)(un >> 32);
            buff[4] = (byte)(un >> 24);
            buff[5] = (byte)(un >> 16);
            buff[6] = (byte)(un >> 8);
            buff[7] = (byte)un;
            return buff;
        }

        public static void ULongToByteArr(ulong un, byte[] dest, int offset)
        {
            dest[offset] = (byte)(un >> 56);
            dest[offset + 1] = (byte)(un >> 48);
            dest[offset + 2] = (byte)(un >> 40);
            dest[offset + 3] = (byte)(un >> 32);
            dest[offset + 4] = (byte)(un >> 24);
            dest[offset + 5] = (byte)(un >> 16);
            dest[offset + 6] = (byte)(un >> 8);
            dest[offset + 7] = (byte)un;
        }

        public static byte[] ShortToByteArr(short n)
        {
            byte[] buff = new byte[2];
            buff[0] = (byte)(n >> 8);
            buff[1] = (byte)n;
            return buff;
        }

        public static void ShortToByteArr(short n, byte[] dest, int offset)
        {
            dest[offset] = (byte)(n >> 8);
            dest[offset + 1] = (byte)n;
        }

        public static int ByteArrToInt(byte[] buff, int offset)
        {
            int result = 0;
            result = result | (buff[offset] << 24);
            result = result | (buff[offset + 1] << 16);
            result = result | (buff[offset + 2] << 8);
            result = result | (buff[offset + 3]);
            return result;
        }

        public static uint ByteArrToUInt(byte[] buff, int offset)
        {
            uint result = 0;
            result = result | ((uint)buff[offset] << 24);
            result = result | ((uint)buff[offset + 1] << 16);
            result = result | ((uint)buff[offset + 2] << 8);
            result = result | ((uint)buff[offset + 3]);
            return result;
        }

        public static float ByteArrToFloat(byte[] buff, int offset)
        {
            return BitConverter.ToSingle(buff, offset);
        }

        public static long ByteArrToLong(byte[] buff, int offset)
        {
            return BitConverter.ToInt64(buff, offset);
        }

        public static ulong ByteArrToULong(byte[] buff, int offset)
        {
            int temp = 0;
            temp = temp | (buff[offset] << 24);
            temp = temp | (buff[offset + 1] << 16);
            temp = temp | (buff[offset + 2] << 8);
            temp = temp | (buff[offset + 3]);
            ulong result = (ulong)temp << 32;
            temp = 0;
            temp = temp | (buff[offset + 4] << 24);
            temp = temp | (buff[offset + 5] << 16);
            temp = temp | (buff[offset + 6] << 8);
            temp = temp | (buff[offset + 7]);
            result = result | ((uint)temp);
            return result;
        }

        public static short ByteArrToShort(byte[] buff, int offset)
        {
            int result = 0;
            result = result | (buff[offset] << 8);
            result = result | (buff[offset + 1]);
            return (short)result;
        }

        public static byte[] StringToUTF8(string str)
        {
            byte[] buff = new byte[30];
            System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, buff, 0);
            return buff;
        }

        public static byte[] StringToUnicode(string str)
        {
            byte[] buff = new byte[30];
            System.Text.Encoding.Unicode.GetBytes(str, 0, str.Length, buff, 0);
            return buff;
        }

        public static void StringToUnicode(string str, byte[] buff, int offset)
        {
            if (str == null)
                return;
            System.Text.Encoding.Unicode.GetBytes(str, 0, str.Length, buff, offset);
        }

        public static int GetUnicodeLength(string str)
        {
            if (str == null)
                return 0;
            return System.Text.Encoding.Unicode.GetBytes(str).Length;
        }

        public static string UTF8ToString(byte[] buff)
        {
            return Encoding.UTF8.GetString(buff);
        }

        public static string UnicodeToString(byte[] buff)
        {
            int len = buff.Length / 2;
            int cutOffset = len;
            for (int i = len - 1; i >= 0; i--)
            {
                if (buff[i * 2] != 0 || buff[i * 2 + 1] != 0)
                {
                    cutOffset = i + 1;
                    break;
                }
            }
            return Encoding.Unicode.GetString(buff, 0, cutOffset * 2);
        }

        public static string UnicodeToString(byte[] buff, int offset, int len)
        {
            if (len == 0)
                return "";
            return Encoding.Unicode.GetString(buff, offset, len);
        }

        public static bool CheckEMail(string emailStr)
        {
            char[] seperator = new char[1];
            seperator[0] = '@';
            string[] splitStr = emailStr.Split(seperator);
            if (splitStr.Length != 2 || splitStr[0] == "" || splitStr[1] == "")
            {
                return false;
            }
            for (int i = 0; i < splitStr[0].Length; i++)
            {
                char ch = splitStr[0][i];
                if (ch < '0' || (ch > '9' && ch < 'A') || (ch > 'Z' && ch < 'a') || ch > 'z')
                {
                    return false;
                }
            }
            seperator[0] = '.';
            splitStr = splitStr[1].Split(seperator);
            if (splitStr.Length != 2 || splitStr[0] == "" || splitStr[1] == "")
            {
                return false;
            }
            splitStr[0] = splitStr[0] + splitStr[1];
            for (int i = 0; i < splitStr[0].Length; i++)
            {
                char ch = splitStr[0][i];
                if (ch < '0' || (ch > '9' && ch < 'A') || (ch > 'Z' && ch < 'a') || ch > 'z')
                {
                    if (ch != '-')
                        return false;
                }
            }
            return true;
        }
    }
}
