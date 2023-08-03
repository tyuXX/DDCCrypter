// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.PbeParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class PbeParameter : Asn1Encodable
    {
        private readonly Asn1OctetString salt;
        private readonly DerInteger iterationCount;

        public static PbeParameter GetInstance( object obj )
        {
            switch (obj)
            {
                case PbeParameter _:
                case null:
                    return (PbeParameter)obj;
                case Asn1Sequence _:
                    return new PbeParameter( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private PbeParameter( Asn1Sequence seq )
        {
            this.salt = seq.Count == 2 ? Asn1OctetString.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.iterationCount = DerInteger.GetInstance( seq[1] );
        }

        public PbeParameter( byte[] salt, int iterationCount )
        {
            this.salt = new DerOctetString( salt );
            this.iterationCount = new DerInteger( iterationCount );
        }

        public byte[] GetSalt() => this.salt.GetOctets();

        public BigInteger IterationCount => this.iterationCount.Value;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       salt,
       iterationCount
        } );
    }
}
