// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT163R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT163R1Curve : AbstractF2mCurve
    {
        private const int SecT163R1_DEFAULT_COORDS = 6;
        protected readonly SecT163R1Point m_infinity;

        public SecT163R1Curve()
          : base( 163, 3, 6, 7 )
        {
            this.m_infinity = new SecT163R1Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "07B6882CAAEFA84F9554FF8428BD88E246D2782AE2" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "0713612DCDDCB40AAB946BDA29CA91F73AF958AFD9" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "03FFFFFFFFFFFFFFFFFFFF48AAB689C29CA710279B" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT163R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 163;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT163FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT163R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT163R1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 163;

        public virtual bool IsTrinomial => false;

        public virtual int K1 => 3;

        public virtual int K2 => 6;

        public virtual int K3 => 7;
    }
}
