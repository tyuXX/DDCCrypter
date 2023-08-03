// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.ECPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Math.EC
{
    public abstract class ECPoint
    {
        protected static ECFieldElement[] EMPTY_ZS = new ECFieldElement[0];
        protected internal readonly ECCurve m_curve;
        protected internal readonly ECFieldElement m_x;
        protected internal readonly ECFieldElement m_y;
        protected internal readonly ECFieldElement[] m_zs;
        protected internal readonly bool m_withCompression;
        protected internal IDictionary m_preCompTable = null;

        protected static ECFieldElement[] GetInitialZCoords( ECCurve curve )
        {
            int coordinateSystem = curve == null ? 0 : curve.CoordinateSystem;
            switch (coordinateSystem)
            {
                case 0:
                case 5:
                    return EMPTY_ZS;
                default:
                    ECFieldElement ecFieldElement = curve.FromBigInteger( BigInteger.One );
                    switch (coordinateSystem)
                    {
                        case 1:
                        case 2:
                        case 6:
                            return new ECFieldElement[1] { ecFieldElement };
                        case 3:
                            return new ECFieldElement[3]
                            {
                ecFieldElement,
                ecFieldElement,
                ecFieldElement
                            };
                        case 4:
                            return new ECFieldElement[2]
                            {
                ecFieldElement,
                curve.A
                            };
                        default:
                            throw new ArgumentException( "unknown coordinate system" );
                    }
            }
        }

        protected ECPoint( ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression )
          : this( curve, x, y, GetInitialZCoords( curve ), withCompression )
        {
        }

        internal ECPoint(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
        {
            this.m_curve = curve;
            this.m_x = x;
            this.m_y = y;
            this.m_zs = zs;
            this.m_withCompression = withCompression;
        }

        protected internal bool SatisfiesCofactor()
        {
            BigInteger cofactor = this.Curve.Cofactor;
            return cofactor == null || cofactor.Equals( BigInteger.One ) || !ECAlgorithms.ReferenceMultiply( this, cofactor ).IsInfinity;
        }

        protected abstract bool SatisfiesCurveEquation();

        public ECPoint GetDetachedPoint() => this.Normalize().Detach();

        public virtual ECCurve Curve => this.m_curve;

        protected abstract ECPoint Detach();

        protected virtual int CurveCoordinateSystem => this.m_curve != null ? this.m_curve.CoordinateSystem : 0;

        [Obsolete( "Use AffineXCoord, or Normalize() and XCoord, instead" )]
        public virtual ECFieldElement X => this.Normalize().XCoord;

        [Obsolete( "Use AffineYCoord, or Normalize() and YCoord, instead" )]
        public virtual ECFieldElement Y => this.Normalize().YCoord;

        public virtual ECFieldElement AffineXCoord
        {
            get
            {
                this.CheckNormalized();
                return this.XCoord;
            }
        }

        public virtual ECFieldElement AffineYCoord
        {
            get
            {
                this.CheckNormalized();
                return this.YCoord;
            }
        }

        public virtual ECFieldElement XCoord => this.m_x;

        public virtual ECFieldElement YCoord => this.m_y;

        public virtual ECFieldElement GetZCoord( int index ) => index >= 0 && index < this.m_zs.Length ? this.m_zs[index] : null;

        public virtual ECFieldElement[] GetZCoords()
        {
            int length = this.m_zs.Length;
            if (length == 0)
                return this.m_zs;
            ECFieldElement[] destinationArray = new ECFieldElement[length];
            Array.Copy( m_zs, 0, destinationArray, 0, length );
            return destinationArray;
        }

        protected internal ECFieldElement RawXCoord => this.m_x;

        protected internal ECFieldElement RawYCoord => this.m_y;

        protected internal ECFieldElement[] RawZCoords => this.m_zs;

        protected virtual void CheckNormalized()
        {
            if (!this.IsNormalized())
                throw new InvalidOperationException( "point not in normal form" );
        }

        public virtual bool IsNormalized()
        {
            switch (this.CurveCoordinateSystem)
            {
                case 0:
                case 5:
                    return true;
                default:
                    if (!this.IsInfinity)
                        return this.RawZCoords[0].IsOne;
                    goto case 0;
            }
        }

        public virtual ECPoint Normalize()
        {
            if (this.IsInfinity)
                return this;
            switch (this.CurveCoordinateSystem)
            {
                case 0:
                case 5:
                    return this;
                default:
                    ECFieldElement rawZcoord = this.RawZCoords[0];
                    return rawZcoord.IsOne ? this : this.Normalize( rawZcoord.Invert() );
            }
        }

        internal virtual ECPoint Normalize( ECFieldElement zInv )
        {
            switch (this.CurveCoordinateSystem)
            {
                case 1:
                case 6:
                    return this.CreateScaledPoint( zInv, zInv );
                case 2:
                case 3:
                case 4:
                    ECFieldElement sx = zInv.Square();
                    ECFieldElement sy = sx.Multiply( zInv );
                    return this.CreateScaledPoint( sx, sy );
                default:
                    throw new InvalidOperationException( "not a projective coordinate system" );
            }
        }

        protected virtual ECPoint CreateScaledPoint( ECFieldElement sx, ECFieldElement sy ) => this.Curve.CreateRawPoint( this.RawXCoord.Multiply( sx ), this.RawYCoord.Multiply( sy ), this.IsCompressed );

        public bool IsInfinity => this.m_x == null && this.m_y == null;

        public bool IsCompressed => this.m_withCompression;

        public bool IsValid() => this.IsInfinity || this.Curve == null || (this.SatisfiesCurveEquation() && this.SatisfiesCofactor());

        public virtual ECPoint ScaleX( ECFieldElement scale ) => !this.IsInfinity ? this.Curve.CreateRawPoint( this.RawXCoord.Multiply( scale ), this.RawYCoord, this.RawZCoords, this.IsCompressed ) : this;

        public virtual ECPoint ScaleY( ECFieldElement scale ) => !this.IsInfinity ? this.Curve.CreateRawPoint( this.RawXCoord, this.RawYCoord.Multiply( scale ), this.RawZCoords, this.IsCompressed ) : this;

        public override bool Equals( object obj ) => this.Equals( obj as ECPoint );

        public virtual bool Equals( ECPoint other )
        {
            if (this == other)
                return true;
            if (other == null)
                return false;
            ECCurve curve1 = this.Curve;
            ECCurve curve2 = other.Curve;
            bool flag1 = null == curve1;
            bool flag2 = null == curve2;
            bool isInfinity1 = this.IsInfinity;
            bool isInfinity2 = other.IsInfinity;
            if (isInfinity1 || isInfinity2)
            {
                if (!isInfinity1 || !isInfinity2)
                    return false;
                return flag1 || flag2 || curve1.Equals( curve2 );
            }
            ECPoint ecPoint = this;
            ECPoint p = other;
            if (!flag1 || !flag2)
            {
                if (flag1)
                    p = p.Normalize();
                else if (flag2)
                {
                    ecPoint = ecPoint.Normalize();
                }
                else
                {
                    if (!curve1.Equals( curve2 ))
                        return false;
                    ECPoint[] points = new ECPoint[2]
                    {
            this,
            curve1.ImportPoint(p)
                    };
                    curve1.NormalizeAll( points );
                    ecPoint = points[0];
                    p = points[1];
                }
            }
            return ecPoint.XCoord.Equals( p.XCoord ) && ecPoint.YCoord.Equals( p.YCoord );
        }

        public override int GetHashCode()
        {
            ECCurve curve = this.Curve;
            int hashCode = curve == null ? 0 : ~curve.GetHashCode();
            if (!this.IsInfinity)
            {
                ECPoint ecPoint = this.Normalize();
                hashCode = hashCode ^ (ecPoint.XCoord.GetHashCode() * 17) ^ (ecPoint.YCoord.GetHashCode() * 257);
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (this.IsInfinity)
                return "INF";
            StringBuilder stringBuilder = new();
            stringBuilder.Append( '(' );
            stringBuilder.Append( RawXCoord );
            stringBuilder.Append( ',' );
            stringBuilder.Append( RawYCoord );
            for (int index = 0; index < this.m_zs.Length; ++index)
            {
                stringBuilder.Append( ',' );
                stringBuilder.Append( this.m_zs[index] );
            }
            stringBuilder.Append( ')' );
            return stringBuilder.ToString();
        }

        public virtual byte[] GetEncoded() => this.GetEncoded( this.m_withCompression );

        public abstract byte[] GetEncoded( bool compressed );

        protected internal abstract bool CompressionYTilde { get; }

        public abstract ECPoint Add( ECPoint b );

        public abstract ECPoint Subtract( ECPoint b );

        public abstract ECPoint Negate();

        public virtual ECPoint TimesPow2( int e )
        {
            if (e < 0)
                throw new ArgumentException( "cannot be negative", nameof( e ) );
            ECPoint ecPoint = this;
            while (--e >= 0)
                ecPoint = ecPoint.Twice();
            return ecPoint;
        }

        public abstract ECPoint Twice();

        public abstract ECPoint Multiply( BigInteger b );

        public virtual ECPoint TwicePlus( ECPoint b ) => this.Twice().Add( b );

        public virtual ECPoint ThreeTimes() => this.TwicePlus( this );
    }
}
