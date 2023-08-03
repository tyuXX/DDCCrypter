// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.ReasonFlags
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class ReasonFlags : DerBitString
    {
        public const int Unused = 128;
        public const int KeyCompromise = 64;
        public const int CACompromise = 32;
        public const int AffiliationChanged = 16;
        public const int Superseded = 8;
        public const int CessationOfOperation = 4;
        public const int CertificateHold = 2;
        public const int PrivilegeWithdrawn = 1;
        public const int AACompromise = 32768;

        public ReasonFlags( int reasons )
          : base( reasons )
        {
        }

        public ReasonFlags( DerBitString reasons )
          : base( reasons.GetBytes(), reasons.PadBits )
        {
        }
    }
}
