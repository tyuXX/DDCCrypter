// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP128R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP128R1Curve : AbstractFpCurve
    {
        private const int SecP128R1_DEFAULT_COORDS = 2;
        public static readonly BigInteger q = new( 1, Hex.Decode( "FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFF" ) );
        protected readonly SecP128R1Point m_infinity;

        public SecP128R1Curve()
          : base( q )
        {
            this.m_infinity = new SecP128R1Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFC" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "E87579C11079F43DD824993C2CEE5ED3" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "FFFFFFFE0000000075A30D1B9038A115" ) );
            this.m_cofactor = BigInteger.One;
            this.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => new SecP128R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 2;

        public virtual BigInteger Q => q;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => q.BitLength;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecP128R1FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecP128R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecP128R1Point( this, x, y, zs, withCompression );
        }
    }
}
