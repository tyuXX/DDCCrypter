// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CrlEntry
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class CrlEntry : Asn1Encodable
    {
        internal Asn1Sequence seq;
        internal DerInteger userCertificate;
        internal Time revocationDate;
        internal X509Extensions crlEntryExtensions;

        public CrlEntry( Asn1Sequence seq )
        {
            this.seq = seq.Count >= 2 && seq.Count <= 3 ? seq : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.userCertificate = DerInteger.GetInstance( seq[0] );
            this.revocationDate = Time.GetInstance( seq[1] );
        }

        public DerInteger UserCertificate => this.userCertificate;

        public Time RevocationDate => this.revocationDate;

        public X509Extensions Extensions
        {
            get
            {
                if (this.crlEntryExtensions == null && this.seq.Count == 3)
                    this.crlEntryExtensions = X509Extensions.GetInstance( this.seq[2] );
                return this.crlEntryExtensions;
            }
        }

        public override Asn1Object ToAsn1Object() => seq;
    }
}
