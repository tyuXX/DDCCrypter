// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AuthorityKeyIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AuthorityKeyIdentifier : Asn1Encodable
    {
        internal readonly Asn1OctetString keyidentifier;
        internal readonly GeneralNames certissuer;
        internal readonly DerInteger certserno;

        public static AuthorityKeyIdentifier GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static AuthorityKeyIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case AuthorityKeyIdentifier _:
                    return (AuthorityKeyIdentifier)obj;
                case Asn1Sequence _:
                    return new AuthorityKeyIdentifier( (Asn1Sequence)obj );
                case X509Extension _:
                    return GetInstance( X509Extension.ConvertValueToObject( (X509Extension)obj ) );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        protected internal AuthorityKeyIdentifier( Asn1Sequence seq )
        {
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.keyidentifier = Asn1OctetString.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 1:
                        this.certissuer = GeneralNames.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 2:
                        this.certserno = DerInteger.GetInstance( asn1TaggedObject, false );
                        continue;
                    default:
                        throw new ArgumentException( "illegal tag" );
                }
            }
        }

        public AuthorityKeyIdentifier( SubjectPublicKeyInfo spki )
        {
            IDigest digest = new Sha1Digest();
            byte[] numArray = new byte[digest.GetDigestSize()];
            byte[] bytes = spki.PublicKeyData.GetBytes();
            digest.BlockUpdate( bytes, 0, bytes.Length );
            digest.DoFinal( numArray, 0 );
            this.keyidentifier = new DerOctetString( numArray );
        }

        public AuthorityKeyIdentifier(
          SubjectPublicKeyInfo spki,
          GeneralNames name,
          BigInteger serialNumber )
        {
            IDigest digest = new Sha1Digest();
            byte[] numArray = new byte[digest.GetDigestSize()];
            byte[] bytes = spki.PublicKeyData.GetBytes();
            digest.BlockUpdate( bytes, 0, bytes.Length );
            digest.DoFinal( numArray, 0 );
            this.keyidentifier = new DerOctetString( numArray );
            this.certissuer = name;
            this.certserno = new DerInteger( serialNumber );
        }

        public AuthorityKeyIdentifier( GeneralNames name, BigInteger serialNumber )
        {
            this.keyidentifier = null;
            this.certissuer = GeneralNames.GetInstance( name.ToAsn1Object() );
            this.certserno = new DerInteger( serialNumber );
        }

        public AuthorityKeyIdentifier( byte[] keyIdentifier )
        {
            this.keyidentifier = new DerOctetString( keyIdentifier );
            this.certissuer = null;
            this.certserno = null;
        }

        public AuthorityKeyIdentifier( byte[] keyIdentifier, GeneralNames name, BigInteger serialNumber )
        {
            this.keyidentifier = new DerOctetString( keyIdentifier );
            this.certissuer = GeneralNames.GetInstance( name.ToAsn1Object() );
            this.certserno = new DerInteger( serialNumber );
        }

        public byte[] GetKeyIdentifier() => this.keyidentifier != null ? this.keyidentifier.GetOctets() : null;

        public GeneralNames AuthorityCertIssuer => this.certissuer;

        public BigInteger AuthorityCertSerialNumber => this.certserno != null ? this.certserno.Value : null;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.keyidentifier != null)
                v.Add( new DerTaggedObject( false, 0, keyidentifier ) );
            if (this.certissuer != null)
                v.Add( new DerTaggedObject( false, 1, certissuer ) );
            if (this.certserno != null)
                v.Add( new DerTaggedObject( false, 2, certserno ) );
            return new DerSequence( v );
        }

        public override string ToString() => "AuthorityKeyIdentifier: KeyID(" + this.keyidentifier.GetOctets() + ")";
    }
}
