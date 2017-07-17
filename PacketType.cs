using System;
using System.Text;

namespace NetworkLibrary.PacketType
{
    public abstract class PrimitiveType
    {
        public abstract void ToBytes(byte[] buff, ref int pointer);
        public abstract void ToType(byte[] buff, ref int pointer);
        public abstract int GetSize();
    }

    public class PByte : PrimitiveType
    {
        public byte n;

        public PByte()
        {
            n = 0;
        }

        public PByte(byte ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 1)
                buff[pointer] = n;
            pointer += 1;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 1)
                n = buff[pointer];
            pointer += 1;
        }

        public override int GetSize()
        {
            return 1;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PSByte : PrimitiveType
    {
        public sbyte n;

        public PSByte()
        {
            n = 0;
        }

        public PSByte(sbyte ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 1)
                buff[pointer] = (byte)n;
            pointer += 1;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 1)
                n = (sbyte)buff[pointer];
            pointer += 1;
        }

        public override int GetSize()
        {
            return 1;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PShort : PrimitiveType
    {
        public short n;

        public PShort()
        {
            n = 0;
        }

        public PShort(short ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 2)
                Util.ShortToByteArr(n, buff, pointer);
            pointer += 2;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 2)
                n = Util.ByteArrToShort(buff, pointer);
            pointer += 2;
        }

        public override int GetSize()
        {
            return 2;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PInteger : PrimitiveType
    {
        public int n;

        public PInteger()
        {
            n = 0;
        }

        public PInteger(int ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
                Util.IntToByteArr(n, buff, pointer);
            pointer += 4;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
                n = Util.ByteArrToInt(buff, pointer);
            pointer += 4;
        }

        public override int GetSize()
        {
            return 4;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PUInteger : PrimitiveType
    {
        public uint n;

        public PUInteger()
        {
            n = 0;
        }

        public PUInteger(uint ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
                Util.UIntToByteArr(n, buff, pointer);
            pointer += 4;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
                n = Util.ByteArrToUInt(buff, pointer);
            pointer += 4;
        }

        public override int GetSize()
        {
            return 4;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PFloat : PrimitiveType
    {
        public float f;

        public PFloat()
        {
            f = 0.0f;
        }

        public PFloat(float fi)
        {
            f = fi;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
                Util.FloatToByteArr(f, buff, pointer);
            pointer += 4;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
                f = Util.ByteArrToFloat(buff, pointer);
            pointer += 4;
        }

        public override int GetSize()
        {
            return 4;
        }

        public override string ToString()
        {
            return f.ToString();
        }
    }

    public class PLong : PrimitiveType
    {
        public long n;

        public PLong()
        {
            n = 0;
        }

        public PLong(long ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 8)
                Util.LongToByteArr(n, buff, pointer);
            pointer += 8;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 8)
                n = Util.ByteArrToLong(buff, pointer);
            pointer += 8;
        }

        public override int GetSize()
        {
            return 8;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PULong : PrimitiveType
    {
        public ulong n;

        public PULong()
        {
            n = 0;
        }

        public PULong(ulong ni)
        {
            n = ni;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 8)
                Util.ULongToByteArr(n, buff, pointer);
            pointer += 8;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 8)
                n = Util.ByteArrToULong(buff, pointer);
            pointer += 8;
        }

        public override int GetSize()
        {
            return 8;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }

    public class PString : PrimitiveType
    {
        public string str;

        public PString()
        {
            str = "";
        }

        public PString(string stri)
        {
            if (stri != null)
                str = stri;
            else
                str = "";
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (str == null)
            {
                if (buff.Length >= pointer + 1)
                    buff[pointer] = (byte)0;
                pointer += 1;
            }
            else
            {
                int len = Util.GetUnicodeLength(str);
                if (buff.Length >= pointer + 1 + len)
                {
                    buff[pointer] = (byte)len;
                    Util.StringToUnicode(str, buff, pointer + 1);
                }
                pointer += 1;
                pointer += len;
            }
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 1)
            {
                int len = buff[pointer];
                if (buff.Length >= pointer + 1 + len)
                    str = Util.UnicodeToString(buff, pointer + 1, len);
                pointer += 1;
                pointer += len;
            }
        }

        public override int GetSize()
        {
            if (str == null)
                return 1;
            else
                return Util.GetUnicodeLength(str) + 1;
        }

        public override string ToString()
        {
            return str;
        }
    }

    public class PLongString : PrimitiveType
    {
        public string str;

        public PLongString()
        {
            str = "";
        }

        public PLongString(string stri)
        {
            if (stri != null)
                str = stri;
            else
                str = "";
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (str == null)
            {
                if (buff.Length >= pointer + 2)
                    Util.ShortToByteArr((short)0, buff, pointer);
                pointer += 2;
            }
            else
            {
                int len = Util.GetUnicodeLength(str);
                if (buff.Length >= pointer + 2 + len)
                {
                    Util.ShortToByteArr((short)len, buff, pointer);
                    Util.StringToUnicode(str, buff, pointer + 2);
                }
                pointer += 2;
                pointer += len;
            }
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 1)
            {
                int len = (int)Util.ByteArrToShort(buff, pointer);
                if (buff.Length >= pointer + 1 + len)
                    str = Util.UnicodeToString(buff, pointer + 2, len);
                pointer += 2;
                pointer += len;
            }
        }

        public override int GetSize()
        {
            if (str == null)
                return 2;
            else
                return Util.GetUnicodeLength(str) + 2;
        }

        public override string ToString()
        {
            return str;
        }
    }

    public class PByteArray : PrimitiveType
    {
        public byte[] bytes;

        public PByteArray()
        {
            bytes = null;
        }

        public PByteArray(byte[] buff)
        {
            bytes = buff;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            int len = 0;
            if (bytes != null)
                len = bytes.Length;
            if (buff.Length >= pointer + 4)
                Util.IntToByteArr(len, buff, pointer);
            if (bytes != null && buff.Length >= pointer + 4 + len)
                Buffer.BlockCopy(bytes, 0, buff, pointer + 4, len);
            pointer += 4;
            pointer += len;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
            {
                int len = Util.ByteArrToInt(buff, pointer);
                if (len > 0 && buff.Length >= pointer + 4 + len)
                {
                    bytes = new byte[len];
                    Buffer.BlockCopy(buff, pointer + 4, bytes, 0, len);
                }
                else
                {
                    bytes = null;
                }
                pointer += 4;
                pointer += len;
            }
        }

        public override int GetSize()
        {
            if (bytes != null)
                return bytes.Length + 4;
            else
                return 4;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; ++i)
            {
                sb.Append(bytes[i].ToString());
                if (i == bytes.Length - 1)
                    sb.Append(',');
            }
            return sb.ToString();
        }
    }

    public class PSByteArray : PrimitiveType
    {
        public sbyte[] sbytes;

        public PSByteArray()
        {
            sbytes = null;
        }

        public PSByteArray(sbyte[] buff)
        {
            sbytes = buff;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            int len = 0;
            if (sbytes != null)
                len = sbytes.Length;
            if (buff.Length >= pointer + 4)
                Util.IntToByteArr(len, buff, pointer);
            if (sbytes != null && buff.Length >= pointer + 4 + len)
                Buffer.BlockCopy(sbytes, 0, buff, pointer + 4, len);
            pointer += 4;
            pointer += len;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 4)
            {
                int len = Util.ByteArrToInt(buff, pointer);
                if (len > 0 && buff.Length >= pointer + 4 + len)
                {
                    sbytes = new sbyte[len];
                    Buffer.BlockCopy(buff, pointer + 4, sbytes, 0, len);
                }
                else
                {
                    sbytes = null;
                }
                pointer += 4;
                pointer += len;
            }
        }

        public override int GetSize()
        {
            if (sbytes != null)
                return sbytes.Length + 4;
            else
                return 4;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sbytes.Length; ++i)
            {
                sb.Append(sbytes[i].ToString());
                if (i == sbytes.Length - 1)
                    sb.Append(',');
            }
            return sb.ToString();
        }
    }

    public class PGuid : PrimitiveType
    {
        public Guid guid;

        public PGuid()
        {
            guid = Guid.Empty;
        }

        public PGuid(Guid guidi)
        {
            guid = guidi;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 16)
                Buffer.BlockCopy(guid.ToByteArray(), 0, buff, pointer, 16);
            pointer += 16;
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            byte[] bytes = new byte[16];
            if (buff.Length >= pointer + 16)
                Buffer.BlockCopy(buff, pointer, bytes, 0, 16);
            guid = new Guid(bytes);
            pointer += 16;
        }

        public override int GetSize()
        {
            return 16;
        }

        public override string ToString()
        {
            return guid.ToString();
        }
    }

    public class PArray<T> : PrimitiveType where T : PrimitiveType, new()
    {
        public T[] array;

        public PArray()
        {
            array = null;
        }

        public PArray(T[] arrayi)
        {
            array = arrayi;
        }

        public override void ToBytes(byte[] buff, ref int pointer)
        {
            if (array == null)
            {
                if (buff.Length >= pointer + 2)
                {
                    buff[pointer] = 0;
                    buff[pointer + 1] = 0;
                }
                pointer += 2;
            }
            else
            {
                if (buff.Length >= pointer + 2)
                    Util.ShortToByteArr((short)array.Length, buff, pointer);
                pointer += 2;
                for (int i = 0; i < array.Length; ++i)
                {
                    array[i].ToBytes(buff, ref pointer);
                }
            }
        }

        public override void ToType(byte[] buff, ref int pointer)
        {
            if (buff.Length >= pointer + 2)
            {
                int len = Util.ByteArrToShort(buff, pointer);
                pointer += 2;
                if (len > 0 && buff.Length >= pointer + len)
                {
                    array = new T[len];
                    for (int i = 0; i < len; ++i)
                    {
                        array[i] = new T();
                        array[i].ToType(buff, ref pointer);
                    }
                }
                else
                {
                    array = null;
                }
            }
        }

        public override int GetSize()
        {
            if (array == null)
            {
                return 2;
            }
            else
            {
                int size = 2;
                for (int i = 0; i < array.Length; ++i)
                {
                    size += array[i].GetSize();
                }
                return size;
            }
        }

        public override string ToString()
        {
            if (array == null) return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; ++i)
            {
                sb.Append(array[i].ToString());
                if (i != array.Length - 1)
                    sb.Append(',');
            }
            return sb.ToString();
        }
    }
}
