// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT283R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT283R1Curve : AbstractF2mCurve
    {
        private const int SecT283R1_DEFAULT_COORDS = 6;
        protected readonly SecT283R1Point m_infinity;

        public SecT283R1Curve()
          : base( 283, 5, 7, 12 )
        {
            this.m_infinity = new SecT283R1Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.One );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "027B680AC8B8596DA5A4AF8A19A0303FCA97FD7645309FA2A581485AF6263E313B79A2F5" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEF90399660FC938A90165B042A7CEFADB307" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT283R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 283;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT283FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT283R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT283R1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 283;

        public virtual bool IsTrinomial => false;

        public virtual int K1 => 5;

        public virtual int K2 => 7;

        public virtual int K3 => 12;
    }
}
