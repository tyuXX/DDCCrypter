// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.Signature
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class Signature : Asn1Encodable
    {
        internal AlgorithmIdentifier signatureAlgorithm;
        internal DerBitString signatureValue;
        internal Asn1Sequence certs;

        public static Signature GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static Signature GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Signature _:
                    return (Signature)obj;
                case Asn1Sequence _:
                    return new Signature( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Signature( AlgorithmIdentifier signatureAlgorithm, DerBitString signatureValue )
          : this( signatureAlgorithm, signatureValue, null )
        {
        }

        public Signature(
          AlgorithmIdentifier signatureAlgorithm,
          DerBitString signatureValue,
          Asn1Sequence certs )
        {
            if (signatureAlgorithm == null)
                throw new ArgumentException( nameof( signatureAlgorithm ) );
            if (signatureValue == null)
                throw new ArgumentException( nameof( signatureValue ) );
            this.signatureAlgorithm = signatureAlgorithm;
            this.signatureValue = signatureValue;
            this.certs = certs;
        }

        private Signature( Asn1Sequence seq )
        {
            this.signatureAlgorithm = AlgorithmIdentifier.GetInstance( seq[0] );
            this.signatureValue = (DerBitString)seq[1];
            if (seq.Count != 3)
                return;
            this.certs = Asn1Sequence.GetInstance( (Asn1TaggedObject)seq[2], true );
        }

        public AlgorithmIdentifier SignatureAlgorithm => this.signatureAlgorithm;

        public DerBitString SignatureValue => this.signatureValue;

        public byte[] GetSignatureOctets() => this.signatureValue.GetOctets();

        public Asn1Sequence Certs => this.certs;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         signatureAlgorithm,
         signatureValue
            } );
            if (this.certs != null)
                v.Add( new DerTaggedObject( true, 0, certs ) );
            return new DerSequence( v );
        }
    }
}
