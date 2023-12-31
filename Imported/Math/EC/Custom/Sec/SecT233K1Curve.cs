﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecT233K1Curve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecT233K1Curve : AbstractF2mCurve
    {
        private const int SecT233K1_DEFAULT_COORDS = 6;
        protected readonly SecT233K1Point m_infinity;

        public SecT233K1Curve()
          : base( 233, 74, 0, 0 )
        {
            this.m_infinity = new SecT233K1Point( this, null, null );
            this.m_a = this.FromBigInteger( BigInteger.Zero );
            this.m_b = this.FromBigInteger( BigInteger.One );
            this.m_order = new BigInteger( 1, Hex.Decode( "8000000000000000000000000000069D5BB915BCD46EFB1AD5F173ABDF" ) );
            this.m_cofactor = BigInteger.ValueOf( 4L );
            this.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => new SecT233K1Curve();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 6;

        protected override ECMultiplier CreateDefaultMultiplier() => new WTauNafMultiplier();

        public override int FieldSize => 233;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new SecT233FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new SecT233K1Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new SecT233K1Point( this, x, y, zs, withCompression );
        }

        public override ECPoint Infinity => m_infinity;

        public override bool IsKoblitz => true;

        public virtual int M => 233;

        public virtual bool IsTrinomial => true;

        public virtual int K1 => 74;

        public virtual int K2 => 0;

        public virtual int K3 => 0;
    }
}
