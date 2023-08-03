// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.RsassaPssParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class RsassaPssParameters : Asn1Encodable
    {
        private AlgorithmIdentifier hashAlgorithm;
        private AlgorithmIdentifier maskGenAlgorithm;
        private DerInteger saltLength;
        private DerInteger trailerField;
        public static readonly AlgorithmIdentifier DefaultHashAlgorithm = new( OiwObjectIdentifiers.IdSha1, DerNull.Instance );
        public static readonly AlgorithmIdentifier DefaultMaskGenFunction = new( PkcsObjectIdentifiers.IdMgf1, DefaultHashAlgorithm );
        public static readonly DerInteger DefaultSaltLength = new( 20 );
        public static readonly DerInteger DefaultTrailerField = new( 1 );

        public static RsassaPssParameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RsassaPssParameters _:
                    return (RsassaPssParameters)obj;
                case Asn1Sequence _:
                    return new RsassaPssParameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public RsassaPssParameters()
        {
            this.hashAlgorithm = DefaultHashAlgorithm;
            this.maskGenAlgorithm = DefaultMaskGenFunction;
            this.saltLength = DefaultSaltLength;
            this.trailerField = DefaultTrailerField;
        }

        public RsassaPssParameters(
          AlgorithmIdentifier hashAlgorithm,
          AlgorithmIdentifier maskGenAlgorithm,
          DerInteger saltLength,
          DerInteger trailerField )
        {
            this.hashAlgorithm = hashAlgorithm;
            this.maskGenAlgorithm = maskGenAlgorithm;
            this.saltLength = saltLength;
            this.trailerField = trailerField;
        }

        public RsassaPssParameters( Asn1Sequence seq )
        {
            this.hashAlgorithm = DefaultHashAlgorithm;
            this.maskGenAlgorithm = DefaultMaskGenFunction;
            this.saltLength = DefaultSaltLength;
            this.trailerField = DefaultTrailerField;
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
                        this.saltLength = DerInteger.GetInstance( asn1TaggedObject, true );
                        break;
                    case 3:
                        this.trailerField = DerInteger.GetInstance( asn1TaggedObject, true );
                        break;
                    default:
                        throw new ArgumentException( "unknown tag" );
                }
            }
        }

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public AlgorithmIdentifier MaskGenAlgorithm => this.maskGenAlgorithm;

        public DerInteger SaltLength => this.saltLength;

        public DerInteger TrailerField => this.trailerField;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (!this.hashAlgorithm.Equals( DefaultHashAlgorithm ))
                v.Add( new DerTaggedObject( true, 0, hashAlgorithm ) );
            if (!this.maskGenAlgorithm.Equals( DefaultMaskGenFunction ))
                v.Add( new DerTaggedObject( true, 1, maskGenAlgorithm ) );
            if (!this.saltLength.Equals( DefaultSaltLength ))
                v.Add( new DerTaggedObject( true, 2, saltLength ) );
            if (!this.trailerField.Equals( DefaultTrailerField ))
                v.Add( new DerTaggedObject( true, 3, trailerField ) );
            return new DerSequence( v );
        }
    }
}
