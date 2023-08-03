// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT193R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT193R1Curve : AbstractF2mCurve
    {
        private const int SecT193R1_DEFAULT_COORDS = 6;
        protected readonly SecT193R1Point m_infinity;

        public SecT193R1Curve()
          : base( 193, 15, 0, 0 )
        {
            this.m_infinity = new SecT193R1Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "0017858FEB7A98975169E171F77B4087DE098AC8A911DF7B01" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "00FDFB49BFE6C3A89FACADAA7A1E5BBC7CC1C2E5D831478814" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "01000000000000000000000000C7F34A778F443ACC920EBA49" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT193R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 193;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT193FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT193R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT193R1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 193;

        public virtual bool IsTrinomial => true;

        public virtual int K1 => 15;

        public virtual int K2 => 0;

        public virtual int K3 => 0;
    }
}
