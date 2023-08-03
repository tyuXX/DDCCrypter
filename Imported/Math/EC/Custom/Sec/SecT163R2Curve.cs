// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT163R2Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT163R2Curve : AbstractF2mCurve
    {
        private const int SecT163R2_DEFAULT_COORDS = 6;
        protected readonly SecT163R2Point m_infinity;

        public SecT163R2Curve()
          : base( 163, 3, 6, 7 )
        {
            this.m_infinity = new SecT163R2Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.One );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "020A601907B8C953CA1481EB10512F78744A3205FD" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "040000000000000000000292FE77E70C12A4234C33" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT163R2Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 163;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT163FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT163R2Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT163R2Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 163;

        public virtual bool IsTrinomial => false;

        public virtual int K1 => 3;

        public virtual int K2 => 6;

        public virtual int K3 => 7;
    }
}
