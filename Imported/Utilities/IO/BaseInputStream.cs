// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.BaseInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
    public abstract class BaseInputStream : Stream
    {
        private bool closed;

        public override sealed bool CanRead => !this.closed;

        public override sealed bool CanSeek => false;

        public override sealed bool CanWrite => false;

        public override void Close()
        {
            this.closed = true;
            base.Close();
        }

        public override sealed void Flush()
        {
        }

        public override sealed long Length => throw new NotSupportedException();

        public override sealed long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            int num1 = offset;
            try
            {
                int num2;
                for (int index = offset + count; num1 < index; buffer[num1++] = (byte)num2)
                {
                    num2 = this.ReadByte();
                    if (num2 == -1)
                        break;
                }
            }
            catch (IOException ex)
            {
                if (num1 == offset)
                    throw;
            }
            return num1 - offset;
        }

        public override sealed long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

        public override sealed void SetLength( long value ) => throw new NotSupportedException();

        public override sealed void Write( byte[] buffer, int offset, int count ) => throw new NotSupportedException();
    }
}
