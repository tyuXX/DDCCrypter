// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SignatureSubpacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class SignatureSubpacket
    {
        private readonly SignatureSubpacketTag type;
        private readonly bool critical;
        private readonly bool isLongLength;
        internal byte[] data;

        protected internal SignatureSubpacket(
          SignatureSubpacketTag type,
          bool critical,
          bool isLongLength,
          byte[] data )
        {
            this.type = type;
            this.critical = critical;
            this.isLongLength = isLongLength;
            this.data = data;
        }

        public SignatureSubpacketTag SubpacketType => this.type;

        public bool IsCritical() => this.critical;

        public bool IsLongLength() => this.isLongLength;

        public byte[] GetData() => (byte[])this.data.Clone();

        public void Encode( Stream os )
        {
            int num1 = this.data.Length + 1;
            if (this.isLongLength)
            {
                os.WriteByte( byte.MaxValue );
                os.WriteByte( (byte)(num1 >> 24) );
                os.WriteByte( (byte)(num1 >> 16) );
                os.WriteByte( (byte)(num1 >> 8) );
                os.WriteByte( (byte)num1 );
            }
            else if (num1 < 192)
                os.WriteByte( (byte)num1 );
            else if (num1 <= 8383)
            {
                int num2 = num1 - 192;
                os.WriteByte( (byte)(((num2 >> 8) & byte.MaxValue) + 192) );
                os.WriteByte( (byte)num2 );
            }
            else
            {
                os.WriteByte( byte.MaxValue );
                os.WriteByte( (byte)(num1 >> 24) );
                os.WriteByte( (byte)(num1 >> 16) );
                os.WriteByte( (byte)(num1 >> 8) );
                os.WriteByte( (byte)num1 );
            }
            if (this.critical)
                os.WriteByte( (byte)((SignatureSubpacketTag)128 | this.type) );
            else
                os.WriteByte( (byte)this.type );
            os.Write( this.data, 0, this.data.Length );
        }
    }
}
