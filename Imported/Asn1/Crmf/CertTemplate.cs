// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CertTemplate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class CertTemplate : Asn1Encodable
    {
        private readonly Asn1Sequence seq;
        private readonly DerInteger version;
        private readonly DerInteger serialNumber;
        private readonly AlgorithmIdentifier signingAlg;
        private readonly X509Name issuer;
        private readonly OptionalValidity validity;
        private readonly X509Name subject;
        private readonly SubjectPublicKeyInfo publicKey;
        private readonly DerBitString issuerUID;
        private readonly DerBitString subjectUID;
        private readonly X509Extensions extensions;

        private CertTemplate( Asn1Sequence seq )
        {
            this.seq = seq;
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.version = DerInteger.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 1:
                        this.serialNumber = DerInteger.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 2:
                        this.signingAlg = AlgorithmIdentifier.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 3:
                        this.issuer = X509Name.GetInstance( asn1TaggedObject, true );
                        continue;
                    case 4:
                        this.validity = OptionalValidity.GetInstance( Asn1Sequence.GetInstance( asn1TaggedObject, false ) );
                        continue;
                    case 5:
                        this.subject = X509Name.GetInstance( asn1TaggedObject, true );
                        continue;
                    case 6:
                        this.publicKey = SubjectPublicKeyInfo.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 7:
                        this.issuerUID = DerBitString.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 8:
                        this.subjectUID = DerBitString.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 9:
                        this.extensions = X509Extensions.GetInstance( asn1TaggedObject, false );
                        continue;
                    default:
                        throw new ArgumentException( "unknown tag: " + asn1TaggedObject.TagNo, nameof( seq ) );
                }
            }
        }

        public static CertTemplate GetInstance( object obj )
        {
            if (obj is CertTemplate)
                return (CertTemplate)obj;
            return obj != null ? new CertTemplate( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public virtual int Version => this.version.Value.IntValue;

        public virtual DerInteger SerialNumber => this.serialNumber;

        public virtual AlgorithmIdentifier SigningAlg => this.signingAlg;

        public virtual X509Name Issuer => this.issuer;

        public virtual OptionalValidity Validity => this.validity;

        public virtual X509Name Subject => this.subject;

        public virtual SubjectPublicKeyInfo PublicKey => this.publicKey;

        public virtual DerBitString IssuerUID => this.issuerUID;

        public virtual DerBitString SubjectUID => this.subjectUID;

        public virtual X509Extensions Extensions => this.extensions;

        public override Asn1Object ToAsn1Object() => seq;
    }
}
