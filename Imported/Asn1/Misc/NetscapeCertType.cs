// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Misc.NetscapeCertType
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Misc
{
    public class NetscapeCertType : DerBitString
    {
        public const int SslClient = 128;
        public const int SslServer = 64;
        public const int Smime = 32;
        public const int ObjectSigning = 16;
        public const int Reserved = 8;
        public const int SslCA = 4;
        public const int SmimeCA = 2;
        public const int ObjectSigningCA = 1;

        public NetscapeCertType( int usage )
          : base( usage )
        {
        }

        public NetscapeCertType( DerBitString usage )
          : base( usage.GetBytes(), usage.PadBits )
        {
        }

        public override string ToString() => "NetscapeCertType: 0x" + (this.GetBytes()[0] & byte.MaxValue).ToString( "X" );
    }
}
