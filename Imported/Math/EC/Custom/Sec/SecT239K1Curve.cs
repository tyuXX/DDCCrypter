// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT239K1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT239K1Curve : AbstractF2mCurve
    {
        private const int SecT239K1_DEFAULT_COORDS = 6;
        protected readonly SecT239K1Point m_infinity;

        public SecT239K1Curve()
          : base( 239, 158, 0, 0 )
        {
            this.m_infinity = new SecT239K1Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.Zero );
            this.m_b = this.FromBigInteger( BigInteger.One );
            this.m_order = new BigInteger( 1, Hex.Decode( "2000000000000000000000000000005A79FEC67CB6E91F1C1DA800E478A5" ) );
            this.m_cofactor = BigInteger.ValueOf( 4L );
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT239K1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        protected override ECMultiplier CreateDefaultMultiplier() => new WTauNafMultiplier();

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 239;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT239FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT239K1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT239K1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => true;

        public virtual int M => 239;

        public virtual bool IsTrinomial => true;

        public virtual int K1 => 158;

        public virtual int K2 => 0;

        public virtual int K3 => 0;
    }
}
