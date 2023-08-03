// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.KeyRecRepContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class KeyRecRepContent : Asn1Encodable
    {
        private readonly PkiStatusInfo status;
        private readonly CmpCertificate newSigCert;
        private readonly Asn1Sequence caCerts;
        private readonly Asn1Sequence keyPairHist;

        private KeyRecRepContent( Asn1Sequence seq )
        {
            this.status = PkiStatusInfo.GetInstance( seq[0] );
            for (int index = 1; index < seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                switch (instance.TagNo)
                {
                    case 0:
                        this.newSigCert = CmpCertificate.GetInstance( instance.GetObject() );
                        break;
                    case 1:
                        this.caCerts = Asn1Sequence.GetInstance( instance.GetObject() );
                        break;
                    case 2:
                        this.keyPairHist = Asn1Sequence.GetInstance( instance.GetObject() );
                        break;
                    default:
                        throw new ArgumentException( "unknown tag number: " + instance.TagNo, nameof( seq ) );
                }
            }
        }

        public static KeyRecRepContent GetInstance( object obj )
        {
            switch (obj)
            {
                case KeyRecRepContent _:
                    return (KeyRecRepContent)obj;
                case Asn1Sequence _:
                    return new KeyRecRepContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual PkiStatusInfo Status => this.status;

        public virtual CmpCertificate NewSigCert => this.newSigCert;

        public virtual CmpCertificate[] GetCACerts()
        {
            if (this.caCerts == null)
                return null;
            CmpCertificate[] caCerts = new CmpCertificate[this.caCerts.Count];
            for (int index = 0; index != caCerts.Length; ++index)
                caCerts[index] = CmpCertificate.GetInstance( this.caCerts[index] );
            return caCerts;
        }

        public virtual CertifiedKeyPair[] GetKeyPairHist()
        {
            if (this.keyPairHist == null)
                return null;
            CertifiedKeyPair[] keyPairHist = new CertifiedKeyPair[this.keyPairHist.Count];
            for (int index = 0; index != keyPairHist.Length; ++index)
                keyPairHist[index] = CertifiedKeyPair.GetInstance( this.keyPairHist[index] );
            return keyPairHist;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         status
            } );
            this.AddOptional( v, 0, newSigCert );
            this.AddOptional( v, 1, caCerts );
            this.AddOptional( v, 2, keyPairHist );
            return new DerSequence( v );
        }

        private void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( true, tagNo, obj ) );
        }
    }
}
