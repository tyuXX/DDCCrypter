// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CertifiedKeyPair
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CertifiedKeyPair : Asn1Encodable
    {
        private readonly CertOrEncCert certOrEncCert;
        private readonly EncryptedValue privateKey;
        private readonly PkiPublicationInfo publicationInfo;

        private CertifiedKeyPair( Asn1Sequence seq )
        {
            this.certOrEncCert = CertOrEncCert.GetInstance( seq[0] );
            if (seq.Count < 2)
                return;
            if (seq.Count == 2)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[1] );
                if (instance.TagNo == 0)
                    this.privateKey = EncryptedValue.GetInstance( instance.GetObject() );
                else
                    this.publicationInfo = PkiPublicationInfo.GetInstance( instance.GetObject() );
            }
            else
            {
                this.privateKey = EncryptedValue.GetInstance( Asn1TaggedObject.GetInstance( seq[1] ) );
                this.publicationInfo = PkiPublicationInfo.GetInstance( Asn1TaggedObject.GetInstance( seq[2] ) );
            }
        }

        public static CertifiedKeyPair GetInstance( object obj )
        {
            switch (obj)
            {
                case CertifiedKeyPair _:
                    return (CertifiedKeyPair)obj;
                case Asn1Sequence _:
                    return new CertifiedKeyPair( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertifiedKeyPair( CertOrEncCert certOrEncCert )
          : this( certOrEncCert, null, null )
        {
        }

        public CertifiedKeyPair(
          CertOrEncCert certOrEncCert,
          EncryptedValue privateKey,
          PkiPublicationInfo publicationInfo )
        {
            this.certOrEncCert = certOrEncCert != null ? certOrEncCert : throw new ArgumentNullException( nameof( certOrEncCert ) );
            this.privateKey = privateKey;
            this.publicationInfo = publicationInfo;
        }

        public virtual CertOrEncCert CertOrEncCert => this.certOrEncCert;

        public virtual EncryptedValue PrivateKey => this.privateKey;

        public virtual PkiPublicationInfo PublicationInfo => this.publicationInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         certOrEncCert
            } );
            if (this.privateKey != null)
                v.Add( new DerTaggedObject( true, 0, privateKey ) );
            if (this.publicationInfo != null)
                v.Add( new DerTaggedObject( true, 1, publicationInfo ) );
            return new DerSequence( v );
        }
    }
}
