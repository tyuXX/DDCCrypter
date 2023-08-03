// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP384R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP384R1Curve : AbstractFpCurve
    {
        private const int SecP384R1_DEFAULT_COORDS = 2;
        public static readonly BigInteger q = new BigInteger( 1, Hex.Decode( "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFF" ) );
        protected readonly SecP384R1Point m_infinity;

        public SecP384R1Curve()
          : base( q )
        {
            this.m_infinity = new SecP384R1Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFC" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "B3312FA7E23EE7E4988E056BE3F82D19181D9C6EFE8141120314088F5013875AC656398D8A2ED19D2A85C8EDD3EC2AEF" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973" ) );
            this.m_cofactor = BigInteger.One;
            this.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => new SecP384R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 2;

        public virtual BigInteger Q => q;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => q.BitLength;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecP384R1FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecP384R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecP384R1Point( this, x, y, zs, withCompression );
        }
    }
}
