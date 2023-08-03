// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.DHValidationParms
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X9
{
    public class DHValidationParms : Asn1Encodable
    {
        private readonly DerBitString seed;
        private readonly DerInteger pgenCounter;

        public static DHValidationParms GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static DHValidationParms GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DHDomainParameters _:
                    return (DHValidationParms)obj;
                case Asn1Sequence _:
                    return new DHValidationParms( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid DHValidationParms: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public DHValidationParms( DerBitString seed, DerInteger pgenCounter )
        {
            if (seed == null)
                throw new ArgumentNullException( nameof( seed ) );
            if (pgenCounter == null)
                throw new ArgumentNullException( nameof( pgenCounter ) );
            this.seed = seed;
            this.pgenCounter = pgenCounter;
        }

        private DHValidationParms( Asn1Sequence seq )
        {
            this.seed = seq.Count == 2 ? DerBitString.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.pgenCounter = DerInteger.GetInstance( seq[1] );
        }

        public DerBitString Seed => this.seed;

        public DerInteger PgenCounter => this.pgenCounter;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       seed,
       pgenCounter
        } );
    }
}
