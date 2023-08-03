// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.RevAnnContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class RevAnnContent : Asn1Encodable
    {
        private readonly PkiStatusEncodable status;
        private readonly CertId certId;
        private readonly DerGeneralizedTime willBeRevokedAt;
        private readonly DerGeneralizedTime badSinceDate;
        private readonly X509Extensions crlDetails;

        private RevAnnContent( Asn1Sequence seq )
        {
            this.status = PkiStatusEncodable.GetInstance( seq[0] );
            this.certId = CertId.GetInstance( seq[1] );
            this.willBeRevokedAt = DerGeneralizedTime.GetInstance( seq[2] );
            this.badSinceDate = DerGeneralizedTime.GetInstance( seq[3] );
            if (seq.Count <= 4)
                return;
            this.crlDetails = X509Extensions.GetInstance( seq[4] );
        }

        public static RevAnnContent GetInstance( object obj )
        {
            switch (obj)
            {
                case RevAnnContent _:
                    return (RevAnnContent)obj;
                case Asn1Sequence _:
                    return new RevAnnContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual PkiStatusEncodable Status => this.status;

        public virtual CertId CertID => this.certId;

        public virtual DerGeneralizedTime WillBeRevokedAt => this.willBeRevokedAt;

        public virtual DerGeneralizedTime BadSinceDate => this.badSinceDate;

        public virtual X509Extensions CrlDetails => this.crlDetails;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[4]
            {
         status,
         certId,
         willBeRevokedAt,
         badSinceDate
            } );
            v.AddOptional( crlDetails );
            return new DerSequence( v );
        }
    }
}
