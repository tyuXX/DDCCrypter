// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT113R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT113R1Curve : AbstractF2mCurve
    {
        private const int SecT113R1_DEFAULT_COORDS = 6;
        protected readonly SecT113R1Point m_infinity;

        public SecT113R1Curve()
          : base( 113, 9, 0, 0 )
        {
            this.m_infinity = new SecT113R1Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "003088250CA6E7C7FE649CE85820F7" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "00E8BEE4D3E2260744188BE0E9C723" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "0100000000000000D9CCEC8A39E56F" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT113R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 113;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT113FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT113R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT113R1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 113;

        public virtual bool IsTrinomial => true;

        public virtual int K1 => 9;

        public virtual int K2 => 0;

        public virtual int K3 => 0;
    }
}
