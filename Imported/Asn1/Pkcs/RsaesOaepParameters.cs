// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.RsaesOaepParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class RsaesOaepParameters : Asn1Encodable
    {
        private AlgorithmIdentifier hashAlgorithm;
        private AlgorithmIdentifier maskGenAlgorithm;
        private AlgorithmIdentifier pSourceAlgorithm;
        public static readonly AlgorithmIdentifier DefaultHashAlgorithm = new( OiwObjectIdentifiers.IdSha1, DerNull.Instance );
        public static readonly AlgorithmIdentifier DefaultMaskGenFunction = new( PkcsObjectIdentifiers.IdMgf1, DefaultHashAlgorithm );
        public static readonly AlgorithmIdentifier DefaultPSourceAlgorithm = new( PkcsObjectIdentifiers.IdPSpecified, new DerOctetString( new byte[0] ) );

        public static RsaesOaepParameters GetInstance( object obj )
        {
            switch (obj)
            {
                case RsaesOaepParameters _:
                    return (RsaesOaepParameters)obj;
                case Asn1Sequence _:
                    return new RsaesOaepParameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public RsaesOaepParameters()
        {
            this.hashAlgorithm = DefaultHashAlgorithm;
            this.maskGenAlgorithm = DefaultMaskGenFunction;
            this.pSourceAlgorithm = DefaultPSourceAlgorithm;
        }

        public RsaesOaepParameters(
          AlgorithmIdentifier hashAlgorithm,
          AlgorithmIdentifier maskGenAlgorithm,
          AlgorithmIdentifier pSourceAlgorithm )
        {
            this.hashAlgorithm = hashAlgorithm;
            this.maskGenAlgorithm = maskGenAlgorithm;
            this.pSourceAlgorithm = pSourceAlgorithm;
        }

        public RsaesOaepParameters( Asn1Sequence seq )
        {
            this.hashAlgorithm = DefaultHashAlgorithm;
            this.maskGenAlgorithm = DefaultMaskGenFunction;
            this.pSourceAlgorithm = DefaultPSourceAlgorithm;
            for (int index = 0; index != seq.Count; ++index)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index];
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.hashAlgorithm = AlgorithmIdentifier.GetInstance( asn1TaggedObject, true );
                        break;
                    case 1:
                        this.maskGenAlgorithm = AlgorithmIdentifier.GetInstance( asn1TaggedObject, true );
                        break;
                    case 2:
                        this.pSourceAlgorithm = AlgorithmIdentifier.GetInstance( asn1TaggedObject, true );
                        break;
                    default:
                        throw new ArgumentException( "unknown tag" );
                }
            }
        }

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public AlgorithmIdentifier MaskGenAlgorithm => this.maskGenAlgorithm;

        public AlgorithmIdentifier PSourceAlgorithm => this.pSourceAlgorithm;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (!this.hashAlgorithm.Equals( DefaultHashAlgorithm ))
                v.Add( new DerTaggedObject( true, 0, hashAlgorithm ) );
            if (!this.maskGenAlgorithm.Equals( DefaultMaskGenFunction ))
                v.Add( new DerTaggedObject( true, 1, maskGenAlgorithm ) );
            if (!this.pSourceAlgorithm.Equals( DefaultPSourceAlgorithm ))
                v.Add( new DerTaggedObject( true, 2, pSourceAlgorithm ) );
            return new DerSequence( v );
        }
    }
}
