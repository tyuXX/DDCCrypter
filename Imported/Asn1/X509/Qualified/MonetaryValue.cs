// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.MonetaryValue
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public class MonetaryValue : Asn1Encodable
    {
        internal Iso4217CurrencyCode currency;
        internal DerInteger amount;
        internal DerInteger exponent;

        public static MonetaryValue GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case MonetaryValue _:
                    return (MonetaryValue)obj;
                case Asn1Sequence _:
                    return new MonetaryValue( Asn1Sequence.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private MonetaryValue( Asn1Sequence seq )
        {
            this.currency = seq.Count == 3 ? Iso4217CurrencyCode.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.amount = DerInteger.GetInstance( seq[1] );
            this.exponent = DerInteger.GetInstance( seq[2] );
        }

        public MonetaryValue( Iso4217CurrencyCode currency, int amount, int exponent )
        {
            this.currency = currency;
            this.amount = new DerInteger( amount );
            this.exponent = new DerInteger( exponent );
        }

        public Iso4217CurrencyCode Currency => this.currency;

        public BigInteger Amount => this.amount.Value;

        public BigInteger Exponent => this.exponent.Value;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       currency,
       amount,
       exponent
        } );
    }
}
