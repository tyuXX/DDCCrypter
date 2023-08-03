// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Djb.Curve25519
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    internal class Curve25519 : AbstractFpCurve
    {
        private const int Curve25519_DEFAULT_COORDS = 4;
        public static readonly BigInteger q = Nat256.ToBigInteger( Curve25519Field.P );
        protected readonly Curve25519Point m_infinity;

        public Curve25519()
          : base( q )
        {
            this.m_infinity = new Curve25519Point( this, null, null );
            this.m_a = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "2AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA984914A144" ) ) );
            this.m_b = this.FromBigInteger( new BigInteger( 1, Hex.Decode( "7B425ED097B425ED097B425ED097B425ED097B425ED097B4260B5E9C7710C864" ) ) );
            this.m_order = new BigInteger( 1, Hex.Decode( "1000000000000000000000000000000014DEF9DEA2F79CD65812631A5CF5D3ED" ) );
            this.m_cofactor = BigInteger.ValueOf( 8L );
            this.m_coord = 4;
        }

        protected override ECCurve CloneCurve() => new Curve25519();

        public override bool SupportsCoordinateSystem( int coord ) => coord == 4;

        public virtual BigInteger Q => q;

        public override ECPoint Infinity => m_infinity;

        public override int FieldSize => q.BitLength;

        public override ECFieldElement FromBigInteger( BigInteger x ) => new Curve25519FieldElement( x );

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
        {
            return new Curve25519Point( this, x, y, withCompression );
        }

        protected internal override ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            return new Curve25519Point( this, x, y, zs, withCompression );
        }
    }
}
