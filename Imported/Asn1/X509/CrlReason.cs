// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CrlReason
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class CrlReason : DerEnumerated
    {
        public const int Unspecified = 0;
        public const int KeyCompromise = 1;
        public const int CACompromise = 2;
        public const int AffiliationChanged = 3;
        public const int Superseded = 4;
        public const int CessationOfOperation = 5;
        public const int CertificateHold = 6;
        public const int RemoveFromCrl = 8;
        public const int PrivilegeWithdrawn = 9;
        public const int AACompromise = 10;
        private static readonly string[] ReasonString = new string[11]
        {
      nameof (Unspecified),
      nameof (KeyCompromise),
      nameof (CACompromise),
      nameof (AffiliationChanged),
      nameof (Superseded),
      nameof (CessationOfOperation),
      nameof (CertificateHold),
      "Unknown",
      nameof (RemoveFromCrl),
      nameof (PrivilegeWithdrawn),
      nameof (AACompromise)
        };

        public CrlReason( int reason )
          : base( reason )
        {
        }

        public CrlReason( DerEnumerated reason )
          : base( reason.Value.IntValue )
        {
        }

        public override string ToString()
        {
            int intValue = this.Value.IntValue;
            string str;
            switch (intValue)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    str = ReasonString[intValue];
                    break;
                default:
                    str = "Invalid";
                    break;
            }
            return "CrlReason: " + str;
        }
    }
}
