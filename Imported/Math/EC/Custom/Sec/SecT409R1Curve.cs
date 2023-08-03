// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT409R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT409R1Curve : AbstractF2mCurve
    {
        private const int SecT409R1_DEFAULT_COORDS = 6;
        protected readonly SecT409R1Point m_infinity;

        public SecT409R1Curve()
          : base( 409, 87, 0, 0 )
        {
            this.m_infinity = new SecT409R1Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.One );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "0021A5C2C8EE9FEB5C4B9A753B7B476B7FD6422EF1F3DD674761FA99D6AC27C8A9A197B272822F6CD57A55AA4F50AE317B13545F" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "010000000000000000000000000000000000000000000000000001E2AAD6A612F33307BE5FA47C3C9E052F838164CD37D9A21173" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT409R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 409;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT409FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT409R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT409R1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 409;

        public virtual bool IsTrinomial => true;

        public virtual int K1 => 87;

        public virtual int K2 => 0;

        public virtual int K3 => 0;
    }
}
