// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT131R2Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT131R2Curve : AbstractF2mCurve
    {
        private const int SecT131R2_DEFAULT_COORDS = 6;
        protected readonly SecT131R2Point m_infinity;

        public SecT131R2Curve()
          : base( 131, 2, 3, 8 )
        {
            this.m_infinity = new SecT131R2Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "03E5A88919D7CAFCBF415F07C2176573B2" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "04B8266A46C55657AC734CE38F018F2192" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "0400000000000000016954A233049BA98F" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT131R2Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override int FieldSize => 131;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT131FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT131R2Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT131R2Point( this, x, y, zs, withCompression );
        }

        public override ECPoint Infinity => m_infinity;

        public override bool IsKoblitz => false;

        public virtual int M => 131;

        public virtual bool IsTrinomial => false;

        public virtual int K1 => 2;

        public virtual int K2 => 3;

        public virtual int K3 => 8;
    }
}
