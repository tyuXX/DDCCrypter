// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.AbstractFpCurve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class AbstractFpCurve : ECCurve
    {
        protected AbstractFpCurve( BigInteger q )
          : base( FiniteFields.GetPrimeField( q ) )
        {
        }

        public override bool IsValidFieldElement( BigInteger x ) => x != null && x.SignValue >= 0 && x.CompareTo( this.Field.Characteristic ) < 0;

        protected override ECPoint DecompressPoint( int yTilde, BigInteger X1 )
        {
            ECFieldElement ecFieldElement = this.FromBigInteger( X1 );
            ECFieldElement y = ecFieldElement.Square().Add( this.A ).Multiply( ecFieldElement ).Add( this.B ).Sqrt();
            if (y == null)
                throw new ArgumentException( "Invalid point compression" );
            if (y.TestBitZero() != (yTilde == 1))
                y = y.Negate();
            return this.CreateRawPoint( ecFieldElement, y, true );
        }
    }
}
