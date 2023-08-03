// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.ECCurve
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Endo;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Math.Field;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class ECCurve
    {
        public const int COORD_AFFINE = 0;
        public const int COORD_HOMOGENEOUS = 1;
        public const int COORD_JACOBIAN = 2;
        public const int COORD_JACOBIAN_CHUDNOVSKY = 3;
        public const int COORD_JACOBIAN_MODIFIED = 4;
        public const int COORD_LAMBDA_AFFINE = 5;
        public const int COORD_LAMBDA_PROJECTIVE = 6;
        public const int COORD_SKEWED = 7;
        protected readonly IFiniteField m_field;
        protected ECFieldElement m_a;
        protected ECFieldElement m_b;
        protected BigInteger m_order;
        protected BigInteger m_cofactor;
        protected int m_coord = 0;
        protected ECEndomorphism m_endomorphism = null;
        protected ECMultiplier m_multiplier = null;

        public static int[] GetAllCoordinateSystems() => new int[8]
        {
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7
        };

        protected ECCurve( IFiniteField field ) => this.m_field = field;

        public abstract int FieldSize { get; }

        public abstract ECFieldElement FromBigInteger( BigInteger x );

        public abstract bool IsValidFieldElement( BigInteger x );

        public virtual ECCurve.Config Configure() => new( this, this.m_coord, this.m_endomorphism, this.m_multiplier );

        public virtual ECPoint ValidatePoint( BigInteger x, BigInteger y )
        {
            ECPoint point = this.CreatePoint( x, y );
            return point.IsValid() ? point : throw new ArgumentException( "Invalid point coordinates" );
        }

        [Obsolete( "Per-point compression property will be removed" )]
        public virtual ECPoint ValidatePoint( BigInteger x, BigInteger y, bool withCompression )
        {
            ECPoint point = this.CreatePoint( x, y, withCompression );
            return point.IsValid() ? point : throw new ArgumentException( "Invalid point coordinates" );
        }

        public virtual ECPoint CreatePoint( BigInteger x, BigInteger y ) => this.CreatePoint( x, y, false );

        [Obsolete( "Per-point compression property will be removed" )]
        public virtual ECPoint CreatePoint( BigInteger x, BigInteger y, bool withCompression ) => this.CreateRawPoint( this.FromBigInteger( x ), this.FromBigInteger( y ), withCompression );

        protected abstract ECCurve CloneCurve();

        protected internal abstract ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression );

        protected internal abstract ECPoint CreateRawPoint(
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression );

        protected virtual ECMultiplier CreateDefaultMultiplier() => this.m_endomorphism is GlvEndomorphism endomorphism ? new GlvMultiplier( this, endomorphism ) : (ECMultiplier)new WNafL2RMultiplier();

        public virtual bool SupportsCoordinateSystem( int coord ) => coord == 0;

        public virtual PreCompInfo GetPreCompInfo( ECPoint point, string name )
        {
            this.CheckPoint( point );
            lock (point)
            {
                IDictionary preCompTable = point.m_preCompTable;
                return preCompTable == null ? null : (PreCompInfo)preCompTable[name];
            }
        }

        public virtual void SetPreCompInfo( ECPoint point, string name, PreCompInfo preCompInfo )
        {
            this.CheckPoint( point );
            lock (point)
            {
                IDictionary dictionary = point.m_preCompTable;
                if (dictionary == null)
                    point.m_preCompTable = dictionary = Platform.CreateHashtable( 4 );
                dictionary[name] = preCompInfo;
            }
        }

        public virtual ECPoint ImportPoint( ECPoint p )
        {
            if (this == p.Curve)
                return p;
            if (p.IsInfinity)
                return this.Infinity;
            p = p.Normalize();
            return this.ValidatePoint( p.XCoord.ToBigInteger(), p.YCoord.ToBigInteger(), p.IsCompressed );
        }

        public virtual void NormalizeAll( ECPoint[] points ) => this.NormalizeAll( points, 0, points.Length, null );

        public virtual void NormalizeAll( ECPoint[] points, int off, int len, ECFieldElement iso )
        {
            this.CheckPoints( points, off, len );
            switch (this.CoordinateSystem)
            {
                case 0:
                case 5:
                    if (iso == null)
                        break;
                    throw new ArgumentException( "not valid for affine coordinates", nameof( iso ) );
                default:
                    ECFieldElement[] zs = new ECFieldElement[len];
                    int[] numArray = new int[len];
                    int len1 = 0;
                    for (int index = 0; index < len; ++index)
                    {
                        ECPoint point = points[off + index];
                        if (point != null && (iso != null || !point.IsNormalized()))
                        {
                            zs[len1] = point.GetZCoord( 0 );
                            numArray[len1++] = off + index;
                        }
                    }
                    if (len1 == 0)
                        break;
                    ECAlgorithms.MontgomeryTrick( zs, 0, len1, iso );
                    for (int index1 = 0; index1 < len1; ++index1)
                    {
                        int index2 = numArray[index1];
                        points[index2] = points[index2].Normalize( zs[index1] );
                    }
                    break;
            }
        }

        public abstract ECPoint Infinity { get; }

        public virtual IFiniteField Field => this.m_field;

        public virtual ECFieldElement A => this.m_a;

        public virtual ECFieldElement B => this.m_b;

        public virtual BigInteger Order => this.m_order;

        public virtual BigInteger Cofactor => this.m_cofactor;

        public virtual int CoordinateSystem => this.m_coord;

        protected virtual void CheckPoint( ECPoint point )
        {
            if (point == null || this != point.Curve)
                throw new ArgumentException( "must be non-null and on this curve", nameof( point ) );
        }

        protected virtual void CheckPoints( ECPoint[] points ) => this.CheckPoints( points, 0, points.Length );

        protected virtual void CheckPoints( ECPoint[] points, int off, int len )
        {
            if (points == null)
                throw new ArgumentNullException( nameof( points ) );
            if (off < 0 || len < 0 || off > points.Length - len)
                throw new ArgumentException( "invalid range specified", nameof( points ) );
            for (int index = 0; index < len; ++index)
            {
                ECPoint point = points[off + index];
                if (point != null && this != point.Curve)
                    throw new ArgumentException( "entries must be null or on this curve", nameof( points ) );
            }
        }

        public virtual bool Equals( ECCurve other )
        {
            if (this == other)
                return true;
            return other != null && this.Field.Equals( other.Field ) && this.A.ToBigInteger().Equals( other.A.ToBigInteger() ) && this.B.ToBigInteger().Equals( other.B.ToBigInteger() );
        }

        public override bool Equals( object obj ) => this.Equals( obj as ECCurve );

        public override int GetHashCode() => this.Field.GetHashCode() ^ Integers.RotateLeft( this.A.ToBigInteger().GetHashCode(), 8 ) ^ Integers.RotateLeft( this.B.ToBigInteger().GetHashCode(), 16 );

        protected abstract ECPoint DecompressPoint( int yTilde, BigInteger X1 );

        public virtual ECEndomorphism GetEndomorphism() => this.m_endomorphism;

        public virtual ECMultiplier GetMultiplier()
        {
            lock (this)
            {
                if (this.m_multiplier == null)
                    this.m_multiplier = this.CreateDefaultMultiplier();
                return this.m_multiplier;
            }
        }

        public virtual ECPoint DecodePoint( byte[] encoded )
        {
            int length = (this.FieldSize + 7) / 8;
            byte num = encoded[0];
            ECPoint ecPoint;
            switch (num)
            {
                case 0:
                    if (encoded.Length != 1)
                        throw new ArgumentException( "Incorrect length for infinity encoding", nameof( encoded ) );
                    ecPoint = this.Infinity;
                    break;
                case 2:
                case 3:
                    if (encoded.Length != length + 1)
                        throw new ArgumentException( "Incorrect length for compressed encoding", nameof( encoded ) );
                    ecPoint = this.DecompressPoint( num & 1, new BigInteger( 1, encoded, 1, length ) );
                    if (!ecPoint.SatisfiesCofactor())
                        throw new ArgumentException( "Invalid point" );
                    break;
                case 4:
                    if (encoded.Length != (2 * length) + 1)
                        throw new ArgumentException( "Incorrect length for uncompressed encoding", nameof( encoded ) );
                    ecPoint = this.ValidatePoint( new BigInteger( 1, encoded, 1, length ), new BigInteger( 1, encoded, 1 + length, length ) );
                    break;
                case 6:
                case 7:
                    if (encoded.Length != (2 * length) + 1)
                        throw new ArgumentException( "Incorrect length for hybrid encoding", nameof( encoded ) );
                    BigInteger x = new( 1, encoded, 1, length );
                    BigInteger y = new( 1, encoded, 1 + length, length );
                    ecPoint = y.TestBit( 0 ) == (num == 7) ? this.ValidatePoint( x, y ) : throw new ArgumentException( "Inconsistent Y coordinate in hybrid encoding", nameof( encoded ) );
                    break;
                default:
                    throw new FormatException( "Invalid point encoding " + num );
            }
            if (num != 0 && ecPoint.IsInfinity)
                throw new ArgumentException( "Invalid infinity encoding", nameof( encoded ) );
            return ecPoint;
        }

        public class Config
        {
            protected ECCurve outer;
            protected int coord;
            protected ECEndomorphism endomorphism;
            protected ECMultiplier multiplier;

            internal Config(
              ECCurve outer,
              int coord,
              ECEndomorphism endomorphism,
              ECMultiplier multiplier )
            {
                this.outer = outer;
                this.coord = coord;
                this.endomorphism = endomorphism;
                this.multiplier = multiplier;
            }

            public ECCurve.Config SetCoordinateSystem( int coord )
            {
                this.coord = coord;
                return this;
            }

            public ECCurve.Config SetEndomorphism( ECEndomorphism endomorphism )
            {
                this.endomorphism = endomorphism;
                return this;
            }

            public ECCurve.Config SetMultiplier( ECMultiplier multiplier )
            {
                this.multiplier = multiplier;
                return this;
            }

            public ECCurve Create()
            {
                ECCurve ecCurve = this.outer.SupportsCoordinateSystem( this.coord ) ? this.outer.CloneCurve() : throw new InvalidOperationException( "unsupported coordinate system" );
                if (ecCurve == this.outer)
                    throw new InvalidOperationException( "implementation returned current curve" );
                ecCurve.m_coord = this.coord;
                ecCurve.m_endomorphism = this.endomorphism;
                ecCurve.m_multiplier = this.multiplier;
                return ecCurve;
            }
        }
    }
}
