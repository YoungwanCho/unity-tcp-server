using System;
using System.Net;
using System.Net.Sockets;
using NetworkLibrary.PacketType;

namespace NetworkLibrary
{
    public class Packet : IDisposable
    {
        public PInteger PacketType { get { return this._packetType; } }
        public PInteger EmptySpace { get { return this._emptySpace; } }
        public PShort TotalSize { get { return this._totalSize; } set { this._totalSize = value; } }

        protected PShort _totalSize = null;
        protected PInteger _packetType = null;
        protected PInteger _emptySpace = null;

        protected PrimitiveType[] _field;

        private bool disposed = false;

        public Packet(int pakcetType, int fieldCount)
        {
            this._packetType = new PInteger(pakcetType);
            this._emptySpace = new PInteger(0);
            this._field = new PrimitiveType[fieldCount];
            this._totalSize = new PShort(0);
        }

        ~Packet()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {

            }
            _field = null;
        }

        public byte[] ToBytes()
        {
            byte[] buff = new byte[this.GetSize()];
            int offset = 0;
            _totalSize.ToBytes(buff, ref offset);
            _packetType.ToBytes(buff, ref offset);
            _emptySpace.ToBytes(buff, ref offset);

            for (int i = 0; i < _field.Length; i++)
            {
                _field[i].ToBytes(buff, ref offset);
            }

            return buff;
        }

        public void ToType(byte[] buff)
        {
            int offset = 0;
            _totalSize  = new PShort(Util.ByteArrToShort(buff, offset));
            _packetType = new PInteger(Util.ByteArrToInt(buff, offset += 2));
            _emptySpace = new PInteger(Util.ByteArrToInt(buff, offset += 4));

            int pointer = offset += 4;

            if (_field != null)
            {
                for (int i = 0; i < _field.Length; i++)
                {
                    _field[i].ToType(buff, ref pointer);
                    Console.WriteLine(string.Format("_field[{0}] : {1}", i, _field[i].ToString()));
                }
            }
        }

        public int GetSize()
        {
            int result = 0;

            result += 2;//_totalSize.GetSize();
            result += _packetType.GetSize();
            result += _emptySpace.GetSize();

            for (int i = 0; i < _field.Length; i++)
            {
                result += _field[i].GetSize();
                //UnityEngine.Debug.Log(_field[i].GetSize());
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("totalSize : {0}, PacketType : {1}, EmptySize {2}{3}", _totalSize.n, _packetType.n, _emptySpace.n, Environment.NewLine);
        }

    }
}
