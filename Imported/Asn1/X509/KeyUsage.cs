// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.KeyUsage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class KeyUsage : DerBitString
    {
        public const int DigitalSignature = 128;
        public const int NonRepudiation = 64;
        public const int KeyEncipherment = 32;
        public const int DataEncipherment = 16;
        public const int KeyAgreement = 8;
        public const int KeyCertSign = 4;
        public const int CrlSign = 2;
        public const int EncipherOnly = 1;
        public const int DecipherOnly = 32768;

        public static KeyUsage GetInstance( object obj )
        {
            switch (obj)
            {
                case KeyUsage _:
                    return (KeyUsage)obj;
                case X509Extension _:
                    return GetInstance( X509Extension.ConvertValueToObject( (X509Extension)obj ) );
                default:
                    return new KeyUsage( DerBitString.GetInstance( obj ) );
            }
        }

        public KeyUsage( int usage )
          : base( usage )
        {
        }

        private KeyUsage( DerBitString usage )
          : base( usage.GetBytes(), usage.PadBits )
        {
        }

        public override string ToString()
        {
            byte[] bytes = this.GetBytes();
            return bytes.Length == 1 ? "KeyUsage: 0x" + (bytes[0] & byte.MaxValue).ToString( "X" ) : "KeyUsage: 0x" + (((bytes[1] & byte.MaxValue) << 8) | (bytes[0] & byte.MaxValue)).ToString( "X" );
        }
    }
}
