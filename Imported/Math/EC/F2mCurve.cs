// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.F2mCurve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Multiplier;
using System;

namespace Org.BouncyCastle.Math.EC
{
    public class F2mCurve : AbstractF2mCurve
    {
        private const int F2M_DEFAULT_COORDS = 6;
        private readonly int m;
        private readonly int k1;
        private readonly int k2;
        private readonly int k3;
        protected readonly F2mPoint m_infinity;

        public F2mCurve( int m, int k, BigInteger a, BigInteger b )
          : this( m, k, 0, 0, a, b, null, null )
        {
        }

        public F2mCurve(
          int m,
          int k,
          BigInteger a,
          BigInteger b,
          BigInteger order,
          BigInteger cofactor )
          : this( m, k, 0, 0, a, b, order, cofactor )
        {
        }

        public F2mCurve( int m, int k1, int k2, int k3, BigInteger a, BigInteger b )
          : this( m, k1, k2, k3, a, b, null, null )
        {
        }

        public F2mCurve(
          int m,
          int k1,
          int k2,
          int k3,
          BigInteger a,
          BigInteger b,
          BigInteger order,
          BigInteger cofactor )
          : base( m, k1, k2, k3 )
        {
            this.m = m;
            this.k1 = k1;
            this.k2 = k2;
            this.k3 = k3;
            this.m_order = order;
            this.m_cofactor = cofactor;
            this.m_infinity = new F2mPoint( this, null, null );
            if (k1 == 0)
                throw new ArgumentException( "k1 must be > 0" );
            if (k2 == 0)
            {
                if (k3 != 0)
                    throw new ArgumentException( "k3 must be 0 if k2 == 0" );
            }
            else
            {
                if (k2 <= k1)
                    throw new ArgumentException( "k2 must be > k1" );
                if (k3 <= k2)
                    throw new ArgumentException( "k3 must be > k2" );
            }
            this.m_a = this.FromBigInteger( a );
            this.m_b = this.FromBigInteger( b );
            this.m_coord = 6;
        }

        protected F2mCurve(
          int m,
          int k1,
          int k2,
          int k3,
          ECFieldElement a,
          ECFieldElement b,
          BigInteger order,
          BigInteger cofactor )
          : base( m, k1, k2, k3 )
        {
            this.m = m;
            this.k1 = k1;
            this.k2 = k2;
            this.k3 = k3;
            this.m_order = order;
            this.m_cofactor = cofactor;
            this.m_infinity = new F2mPoint( this, null, null );
            this.m_a = a;
            this.m_b = b;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new F2mCurve( this.m, this.k1, this.k2, this.k3, this.m_a, this.m_b, this.m_order, this.m_cofactor );

        public override bool SupportsCoordinateSystem( int coord )
        {
            switch (coord)
            {
                case 0:
                case 1:
                case 6:
                    return true;
                default:
                    return false;
            }
        }

        protected override ECMultiplier CreateDefaultMultiplier() => this.IsKoblitz ? new WTauNafMultiplier() : base.CreateDefaultMultiplier();

        public override int FieldSize => this.m;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new F2mFieldElement( this.m, this.k1, this.k2, this.k3, x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new F2mPoint( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new F2mPoint( this, x, y, zs, withCompression );
        }

        public override ECPoint Infinity => m_infinity;

        public int M => this.m;

        public bool IsTrinomial() => this.k2 == 0 && this.k3 == 0;

        public int K1 => this.k1;

        public int K2 => this.k2;

        public int K3 => this.k3;

        [Obsolete( "Use 'Order' property instead" )]
        public BigInteger N => this.m_order;

        [Obsolete( "Use 'Cofactor' property instead" )]
        public BigInteger H => this.m_cofactor;
    }
}
