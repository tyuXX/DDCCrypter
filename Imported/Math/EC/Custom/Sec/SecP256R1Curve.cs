// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP256R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP256R1Curve : AbstractFpCurve
    {
        private const int SecP256R1_DEFAULT_COORDS = 2;
        public static readonly BigInteger q = new BigInteger( 1, Hex.Decode( "FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF" ) );
        protected readonly SecP256R1Point m_infinity;

        public SecP256R1Curve()
          : base( q )
        {
            this.m_infinity = new SecP256R1Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "5AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551" ) );
            this.m_cofactor = BigInteger.One;
            this.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => new SecP256R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 2;

        public virtual BigInteger Q => q;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => q.BitLength;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecP256R1FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecP256R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecP256R1Point( this, x, y, zs, withCompression );
        }
    }
}
