// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ByteQueue
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ByteQueue
    {
        private const int DefaultCapacity = 1024;
        private byte[] databuf;
        private int skipped = 0;
        private int available = 0;

        public static int NextTwoPow( int i )
        {
            i |= i >> 1;
            i |= i >> 2;
            i |= i >> 4;
            i |= i >> 8;
            i |= i >> 16;
            return i + 1;
        }

        public ByteQueue()
          : this( 1024 )
        {
        }

        public ByteQueue( int capacity ) => this.databuf = new byte[capacity];

        public void Read( byte[] buf, int offset, int len, int skip )
        {
            if (buf.Length - offset < len)
                throw new ArgumentException( "Buffer size of " + buf.Length + " is too small for a read of " + len + " bytes" );
            if (this.available - skip < len)
                throw new InvalidOperationException( "Not enough data to read" );
            Array.Copy( databuf, this.skipped + skip, buf, offset, len );
        }

        public void AddData( byte[] data, int offset, int len )
        {
            if (this.skipped + this.available + len > this.databuf.Length)
            {
                int length = NextTwoPow( this.available + len );
                if (length > this.databuf.Length)
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy( databuf, this.skipped, destinationArray, 0, this.available );
                    this.databuf = destinationArray;
                }
                else
                    Array.Copy( databuf, this.skipped, databuf, 0, this.available );
                this.skipped = 0;
            }
            Array.Copy( data, offset, databuf, this.skipped + this.available, len );
            this.available += len;
        }

        public void RemoveData( int i )
        {
            if (i > this.available)
                throw new InvalidOperationException( "Cannot remove " + i + " bytes, only got " + available );
            this.available -= i;
            this.skipped += i;
        }

        public void RemoveData( byte[] buf, int off, int len, int skip )
        {
            this.Read( buf, off, len, skip );
            this.RemoveData( skip + len );
        }

        public byte[] RemoveData( int len, int skip )
        {
            byte[] buf = new byte[len];
            this.RemoveData( buf, 0, len, skip );
            return buf;
        }

        public int Available => this.available;
    }
}
