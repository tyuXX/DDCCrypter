// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.IO.CipherStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.IO
{
    public class CipherStream : Stream
    {
        internal Stream stream;
        internal IBufferedCipher inCipher;
        internal IBufferedCipher outCipher;
        private byte[] mInBuf;
        private int mInPos;
        private bool inStreamEnded;

        public CipherStream( Stream stream, IBufferedCipher readCipher, IBufferedCipher writeCipher )
        {
            this.stream = stream;
            if (readCipher != null)
            {
                this.inCipher = readCipher;
                this.mInBuf = null;
            }
            if (writeCipher == null)
                return;
            this.outCipher = writeCipher;
        }

        public IBufferedCipher ReadCipher => this.inCipher;

        public IBufferedCipher WriteCipher => this.outCipher;

        public override int ReadByte()
        {
            if (this.inCipher == null)
                return this.stream.ReadByte();
            return (this.mInBuf == null || this.mInPos >= this.mInBuf.Length) && !this.FillInBuf() ? -1 : this.mInBuf[this.mInPos++];
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            if (this.inCipher == null)
                return this.stream.Read( buffer, offset, count );
            int num;
            int length;
            for (num = 0; num < count && ((this.mInBuf != null && this.mInPos < this.mInBuf.Length) || this.FillInBuf()); num += length)
            {
                length = System.Math.Min( count - num, this.mInBuf.Length - this.mInPos );
                Array.Copy( mInBuf, this.mInPos, buffer, offset + num, length );
                this.mInPos += length;
            }
            return num;
        }

        private bool FillInBuf()
        {
            if (this.inStreamEnded)
                return false;
            this.mInPos = 0;
            do
            {
                this.mInBuf = this.ReadAndProcessBlock();
            }
            while (!this.inStreamEnded && this.mInBuf == null);
            return this.mInBuf != null;
        }

        private byte[] ReadAndProcessBlock()
        {
            int blockSize = this.inCipher.GetBlockSize();
            byte[] numArray1 = new byte[blockSize == 0 ? 256 : blockSize];
            int num1 = 0;
            do
            {
                int num2 = this.stream.Read( numArray1, num1, numArray1.Length - num1 );
                if (num2 < 1)
                {
                    this.inStreamEnded = true;
                    break;
                }
                num1 += num2;
            }
            while (num1 < numArray1.Length);
            byte[] numArray2 = this.inStreamEnded ? this.inCipher.DoFinal( numArray1, 0, num1 ) : this.inCipher.ProcessBytes( numArray1 );
            if (numArray2 != null && numArray2.Length == 0)
                numArray2 = null;
            return numArray2;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            int num = offset + count;
            if (this.outCipher == null)
            {
                this.stream.Write( buffer, offset, count );
            }
            else
            {
                byte[] buffer1 = this.outCipher.ProcessBytes( buffer, offset, count );
                if (buffer1 == null)
                    return;
                this.stream.Write( buffer1, 0, buffer1.Length );
            }
        }

        public override void WriteByte( byte b )
        {
            if (this.outCipher == null)
            {
                this.stream.WriteByte( b );
            }
            else
            {
                byte[] buffer = this.outCipher.ProcessByte( b );
                if (buffer == null)
                    return;
                this.stream.Write( buffer, 0, buffer.Length );
            }
        }

        public override bool CanRead => this.stream.CanRead && this.inCipher != null;

        public override bool CanWrite => this.stream.CanWrite && this.outCipher != null;

        public override bool CanSeek => false;

        public override sealed long Length => throw new NotSupportedException();

        public override sealed long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Close()
        {
            if (this.outCipher != null)
            {
                byte[] buffer = this.outCipher.DoFinal();
                this.stream.Write( buffer, 0, buffer.Length );
                this.stream.Flush();
            }
            Platform.Dispose( this.stream );
            base.Close();
        }

        public override void Flush() => this.stream.Flush();

        public override sealed long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

        public override sealed void SetLength( long length ) => throw new NotSupportedException();
    }
}
