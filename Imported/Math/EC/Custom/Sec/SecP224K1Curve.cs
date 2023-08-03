// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP224K1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP224K1Curve : AbstractFpCurve
    {
        private const int SECP224K1_DEFAULT_COORDS = 2;
        public static readonly BigInteger q = new BigInteger( 1, Hex.Decode( "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFE56D" ) );
        protected readonly SecP224K1Point m_infinity;

        public SecP224K1Curve()
          : base( q )
        {
            this.m_infinity = new SecP224K1Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.Zero );
            this.m_b = this.FromBigInteger( BigInteger.ValueOf( 5L ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "010000000000000000000000000001DCE8D2EC6184CAF0A971769FB1F7" ) );
            this.m_cofactor = BigInteger.One;
            this.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => new SecP224K1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 2;

        public virtual BigInteger Q => q;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => q.BitLength;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecP224K1FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecP224K1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecP224K1Point( this, x, y, zs, withCompression );
        }
    }
}
