// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509KeyUsage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.X509
{
    public class X509KeyUsage : Asn1Encodable
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
        private readonly int usage;

        public X509KeyUsage( int usage ) => this.usage = usage;

        public override Asn1Object ToAsn1Object() => new KeyUsage( this.usage );
    }
}
