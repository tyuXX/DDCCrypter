// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.SignerBucket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Operators
{
    internal class SignerBucket : Stream
    {
        protected readonly ISigner signer;

        public SignerBucket( ISigner signer ) => this.signer = signer;

        public override int Read( byte[] buffer, int offset, int count ) => throw new NotImplementedException();

        public override int ReadByte() => throw new NotImplementedException();

        public override void Write( byte[] buffer, int offset, int count )
        {
            if (count <= 0)
                return;
            this.signer.BlockUpdate( buffer, offset, count );
        }

        public override void WriteByte( byte b ) => this.signer.Update( b );

        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override bool CanSeek => false;

        public override long Length => 0;

        public override long Position
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override void Flush()
        {
        }

        public override long Seek( long offset, SeekOrigin origin ) => throw new NotImplementedException();

        public override void SetLength( long length ) => throw new NotImplementedException();
    }
}
