// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.AbstractF2mCurve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Abc;
using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class AbstractF2mCurve : ECCurve
    {
        private BigInteger[] si = null;

        public static BigInteger Inverse( int m, int[] ks, BigInteger x ) => new LongArray( x ).ModInverse( m, ks ).ToBigInteger();

        private static IFiniteField BuildField( int m, int k1, int k2, int k3 )
        {
            if (k1 == 0)
                throw new ArgumentException( "k1 must be > 0" );
            if (k2 == 0)
            {
                if (k3 != 0)
                    throw new ArgumentException( "k3 must be 0 if k2 == 0" );
                return FiniteFields.GetBinaryExtensionField( new int[3]
                {
          0,
          k1,
          m
                } );
            }
            if (k2 <= k1)
                throw new ArgumentException( "k2 must be > k1" );
            if (k3 <= k2)
                throw new ArgumentException( "k3 must be > k2" );
            return FiniteFields.GetBinaryExtensionField( new int[5]
            {
        0,
        k1,
        k2,
        k3,
        m
            } );
        }

        protected AbstractF2mCurve( int m, int k1, int k2, int k3 )
          : base( BuildField( m, k1, k2, k3 ) )
        {
        }

        public override bool IsValidFieldElement( BigInteger x ) => x != null && x.SignValue >= 0 && x.BitLength <= this.FieldSize;

        [Obsolete( "Per-point compression property will be removed" )]
        public override ECPoint CreatePoint( BigInteger x, BigInteger y, bool withCompression )
        {
            ECFieldElement ecFieldElement = this.FromBigInteger( x );
            ECFieldElement y1 = this.FromBigInteger( y );
            switch (this.CoordinateSystem)
            {
                case 5:
                case 6:
                    if (ecFieldElement.IsZero)
                    {
                        if (!y1.Square().Equals( this.B ))
                            throw new ArgumentException();
                        break;
                    }
                    y1 = y1.Divide( ecFieldElement ).Add( ecFieldElement );
                    break;
            }
            return this.CreateRawPoint( ecFieldElement, y1, withCompression );
        }

        protected override ECPoint DecompressPoint( int yTilde, BigInteger X1 )
        {
            ECFieldElement ecFieldElement1 = this.FromBigInteger( X1 );
            ECFieldElement y = null;
            if (ecFieldElement1.IsZero)
            {
                y = this.B.Sqrt();
            }
            else
            {
                ECFieldElement ecFieldElement2 = this.SolveQuadradicEquation( ecFieldElement1.Square().Invert().Multiply( this.B ).Add( this.A ).Add( ecFieldElement1 ) );
                if (ecFieldElement2 != null)
                {
                    if (ecFieldElement2.TestBitZero() != (yTilde == 1))
                        ecFieldElement2 = ecFieldElement2.AddOne();
                    switch (this.CoordinateSystem)
                    {
                        case 5:
                        case 6:
                            y = ecFieldElement2.Add( ecFieldElement1 );
                            break;
                        default:
                            y = ecFieldElement2.Multiply( ecFieldElement1 );
                            break;
                    }
                }
            }
            return y != null ? this.CreateRawPoint( ecFieldElement1, y, true ) : throw new ArgumentException( "Invalid point compression" );
        }

        private ECFieldElement SolveQuadradicEquation( ECFieldElement beta )
        {
            if (beta.IsZero)
                return beta;
            ECFieldElement ecFieldElement1 = this.FromBigInteger( BigInteger.Zero );
            int fieldSize = this.FieldSize;
            ECFieldElement b1;
            do
            {
                ECFieldElement b2 = this.FromBigInteger( BigInteger.Arbitrary( fieldSize ) );
                b1 = ecFieldElement1;
                ECFieldElement ecFieldElement2 = beta;
                for (int index = 1; index < fieldSize; ++index)
                {
                    ECFieldElement ecFieldElement3 = ecFieldElement2.Square();
                    b1 = b1.Square().Add( ecFieldElement3.Multiply( b2 ) );
                    ecFieldElement2 = ecFieldElement3.Add( beta );
                }
                if (!ecFieldElement2.IsZero)
                    return null;
            }
            while (b1.Square().Add( b1 ).IsZero);
            return b1;
        }

        internal virtual BigInteger[] GetSi()
        {
            if (this.si == null)
            {
                lock (this)
                {
                    if (this.si == null)
                        this.si = Tnaf.GetSi( this );
                }
            }
            return this.si;
        }

        public virtual bool IsKoblitz
        {
            get
            {
                if (this.m_order == null || this.m_cofactor == null || !this.m_b.IsOne)
                    return false;
                return this.m_a.IsZero || this.m_a.IsOne;
            }
        }
    }
}
