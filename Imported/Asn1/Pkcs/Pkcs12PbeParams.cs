// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.Pkcs12PbeParams
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class Pkcs12PbeParams : Asn1Encodable
    {
        private readonly DerInteger iterations;
        private readonly Asn1OctetString iv;

        public Pkcs12PbeParams( byte[] salt, int iterations )
        {
            this.iv = new DerOctetString( salt );
            this.iterations = new DerInteger( iterations );
        }

        private Pkcs12PbeParams( Asn1Sequence seq )
        {
            this.iv = seq.Count == 2 ? Asn1OctetString.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.iterations = DerInteger.GetInstance( seq[1] );
        }

        public static Pkcs12PbeParams GetInstance( object obj )
        {
            switch (obj)
            {
                case Pkcs12PbeParams _:
                    return (Pkcs12PbeParams)obj;
                case Asn1Sequence _:
                    return new Pkcs12PbeParams( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public BigInteger Iterations => this.iterations.Value;

        public byte[] GetIV() => this.iv.GetOctets();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       iv,
       iterations
        } );
    }
}
