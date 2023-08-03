// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.MonetaryLimit
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class MonetaryLimit : Asn1Encodable
    {
        private readonly DerPrintableString currency;
        private readonly DerInteger amount;
        private readonly DerInteger exponent;

        public static MonetaryLimit GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case MonetaryLimit _:
                    return (MonetaryLimit)obj;
                case Asn1Sequence _:
                    return new MonetaryLimit( Asn1Sequence.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private MonetaryLimit( Asn1Sequence seq )
        {
            this.currency = seq.Count == 3 ? DerPrintableString.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.amount = DerInteger.GetInstance( seq[1] );
            this.exponent = DerInteger.GetInstance( seq[2] );
        }

        public MonetaryLimit( string currency, int amount, int exponent )
        {
            this.currency = new DerPrintableString( currency, true );
            this.amount = new DerInteger( amount );
            this.exponent = new DerInteger( exponent );
        }

        public virtual string Currency => this.currency.GetString();

        public virtual BigInteger Amount => this.amount.Value;

        public virtual BigInteger Exponent => this.exponent.Value;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       currency,
       amount,
       exponent
        } );
    }
}
