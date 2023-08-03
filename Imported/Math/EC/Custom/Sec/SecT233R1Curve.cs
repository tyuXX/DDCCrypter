// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT233R1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT233R1Curve : AbstractF2mCurve
    {
        private const int SecT233R1_DEFAULT_COORDS = 6;
        protected readonly SecT233R1Point m_infinity;

        public SecT233R1Curve()
          : base( 233, 74, 0, 0 )
        {
            this.m_infinity = new SecT233R1Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.One );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "0066647EDE6C332C7F8C0923BB58213B333B20E9CE4281FE115F7D8F90AD" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "01000000000000000000000000000013E974E72F8A6922031D2603CFE0D7" ) );
            this.m_cofactor = BigInteger.Two;
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT233R1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => 233;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT233FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT233R1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT233R1Point( this, x, y, zs, withCompression );
        }

        public override bool IsKoblitz => false;

        public virtual int M => 233;

        public virtual bool IsTrinomial => true;

        public virtual int K1 => 74;

        public virtual int K2 => 0;

        public virtual int K3 => 0;
    }
}
