// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.SignerInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class SignerInfo : Asn1Encodable
    {
        private DerInteger version;
        private IssuerAndSerialNumber issuerAndSerialNumber;
        private AlgorithmIdentifier digAlgorithm;
        private Asn1Set authenticatedAttributes;
        private AlgorithmIdentifier digEncryptionAlgorithm;
        private Asn1OctetString encryptedDigest;
        private Asn1Set unauthenticatedAttributes;

        public static SignerInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case SignerInfo _:
                    return (SignerInfo)obj;
                case Asn1Sequence _:
                    return new SignerInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public SignerInfo(
          DerInteger version,
          IssuerAndSerialNumber issuerAndSerialNumber,
          AlgorithmIdentifier digAlgorithm,
          Asn1Set authenticatedAttributes,
          AlgorithmIdentifier digEncryptionAlgorithm,
          Asn1OctetString encryptedDigest,
          Asn1Set unauthenticatedAttributes )
        {
            this.version = version;
            this.issuerAndSerialNumber = issuerAndSerialNumber;
            this.digAlgorithm = digAlgorithm;
            this.authenticatedAttributes = authenticatedAttributes;
            this.digEncryptionAlgorithm = digEncryptionAlgorithm;
            this.encryptedDigest = encryptedDigest;
            this.unauthenticatedAttributes = unauthenticatedAttributes;
        }

        public SignerInfo( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.version = (DerInteger)enumerator.Current;
            enumerator.MoveNext();
            this.issuerAndSerialNumber = IssuerAndSerialNumber.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.digAlgorithm = AlgorithmIdentifier.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            object current = enumerator.Current;
            if (current is Asn1TaggedObject)
            {
                this.authenticatedAttributes = Asn1Set.GetInstance( (Asn1TaggedObject)current, false );
                enumerator.MoveNext();
                this.digEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( enumerator.Current );
            }
            else
            {
                this.authenticatedAttributes = null;
                this.digEncryptionAlgorithm = AlgorithmIdentifier.GetInstance( current );
            }
            enumerator.MoveNext();
            this.encryptedDigest = Asn1OctetString.GetInstance( enumerator.Current );
            if (enumerator.MoveNext())
                this.unauthenticatedAttributes = Asn1Set.GetInstance( (Asn1TaggedObject)enumerator.Current, false );
            else
                this.unauthenticatedAttributes = null;
        }

        public DerInteger Version => this.version;

        public IssuerAndSerialNumber IssuerAndSerialNumber => this.issuerAndSerialNumber;

        public Asn1Set AuthenticatedAttributes => this.authenticatedAttributes;

        public AlgorithmIdentifier DigestAlgorithm => this.digAlgorithm;

        public Asn1OctetString EncryptedDigest => this.encryptedDigest;

        public AlgorithmIdentifier DigestEncryptionAlgorithm => this.digEncryptionAlgorithm;

        public Asn1Set UnauthenticatedAttributes => this.unauthenticatedAttributes;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         version,
         issuerAndSerialNumber,
         digAlgorithm
            } );
            if (this.authenticatedAttributes != null)
                v.Add( new DerTaggedObject( false, 0, authenticatedAttributes ) );
            v.Add( digEncryptionAlgorithm, encryptedDigest );
            if (this.unauthenticatedAttributes != null)
                v.Add( new DerTaggedObject( false, 1, unauthenticatedAttributes ) );
            return new DerSequence( v );
        }
    }
}
