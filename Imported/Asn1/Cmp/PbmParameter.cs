// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PbmParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PbmParameter : Asn1Encodable
    {
        private Asn1OctetString salt;
        private AlgorithmIdentifier owf;
        private DerInteger iterationCount;
        private AlgorithmIdentifier mac;

        private PbmParameter( Asn1Sequence seq )
        {
            this.salt = Asn1OctetString.GetInstance( seq[0] );
            this.owf = AlgorithmIdentifier.GetInstance( seq[1] );
            this.iterationCount = DerInteger.GetInstance( seq[2] );
            this.mac = AlgorithmIdentifier.GetInstance( seq[3] );
        }

        public static PbmParameter GetInstance( object obj )
        {
            switch (obj)
            {
                case PbmParameter _:
                    return (PbmParameter)obj;
                case Asn1Sequence _:
                    return new PbmParameter( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public PbmParameter(
          byte[] salt,
          AlgorithmIdentifier owf,
          int iterationCount,
          AlgorithmIdentifier mac )
          : this( new DerOctetString( salt ), owf, new DerInteger( iterationCount ), mac )
        {
        }

        public PbmParameter(
          Asn1OctetString salt,
          AlgorithmIdentifier owf,
          DerInteger iterationCount,
          AlgorithmIdentifier mac )
        {
            this.salt = salt;
            this.owf = owf;
            this.iterationCount = iterationCount;
            this.mac = mac;
        }

        public virtual Asn1OctetString Salt => this.salt;

        public virtual AlgorithmIdentifier Owf => this.owf;

        public virtual DerInteger IterationCount => this.iterationCount;

        public virtual AlgorithmIdentifier Mac => this.mac;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[4]
        {
       salt,
       owf,
       iterationCount,
       mac
        } );
    }
}
