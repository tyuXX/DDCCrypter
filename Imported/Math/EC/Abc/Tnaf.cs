// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Abc.Tnaf
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Abc
{
    internal class Tnaf
    {
        public const sbyte Width = 4;
        public const sbyte Pow2Width = 16;
        private static readonly BigInteger MinusOne = BigInteger.One.Negate();
        private static readonly BigInteger MinusTwo = BigInteger.Two.Negate();
        private static readonly BigInteger MinusThree = BigInteger.Three.Negate();
        private static readonly BigInteger Four = BigInteger.ValueOf( 4L );
        public static readonly ZTauElement[] Alpha0 = new ZTauElement[9]
        {
      null,
      new ZTauElement(BigInteger.One, BigInteger.Zero),
      null,
      new ZTauElement(MinusThree, MinusOne),
      null,
      new ZTauElement(MinusOne, MinusOne),
      null,
      new ZTauElement(BigInteger.One, MinusOne),
      null
        };
        public static readonly sbyte[][] Alpha0Tnaf = new sbyte[8][]
        {
      null,
      new sbyte[1]{  1 },
      null,
      new sbyte[3]{  -1,  0,  1 },
      null,
      new sbyte[3]{  1,  0,  1 },
      null,
      new sbyte[4]{ -1, 0, 0, 1 }
        };
        public static readonly ZTauElement[] Alpha1 = new ZTauElement[9]
        {
      null,
      new ZTauElement(BigInteger.One, BigInteger.Zero),
      null,
      new ZTauElement(MinusThree, BigInteger.One),
      null,
      new ZTauElement(MinusOne, BigInteger.One),
      null,
      new ZTauElement(BigInteger.One, BigInteger.One),
      null
        };
        public static readonly sbyte[][] Alpha1Tnaf = new sbyte[8][]
        {
      null,
      new sbyte[1]{  1 },
      null,
      new sbyte[3]{  -1,  0,  1 },
      null,
      new sbyte[3]{  1,  0,  1 },
      null,
      new sbyte[4]{ -1, 0, 0, -1 }
        };

        public static BigInteger Norm( sbyte mu, ZTauElement lambda )
        {
            BigInteger bigInteger1 = lambda.u.Multiply( lambda.u );
            BigInteger n = lambda.u.Multiply( lambda.v );
            BigInteger bigInteger2 = lambda.v.Multiply( lambda.v ).ShiftLeft( 1 );
            if (mu == 1)
                return bigInteger1.Add( n ).Add( bigInteger2 );
            if (mu == -1)
                return bigInteger1.Subtract( n ).Add( bigInteger2 );
            throw new ArgumentException( "mu must be 1 or -1" );
        }

        public static SimpleBigDecimal Norm( sbyte mu, SimpleBigDecimal u, SimpleBigDecimal v )
        {
            SimpleBigDecimal simpleBigDecimal = u.Multiply( u );
            SimpleBigDecimal b1 = u.Multiply( v );
            SimpleBigDecimal b2 = v.Multiply( v ).ShiftLeft( 1 );
            if (mu == 1)
                return simpleBigDecimal.Add( b1 ).Add( b2 );
            if (mu == -1)
                return simpleBigDecimal.Subtract( b1 ).Add( b2 );
            throw new ArgumentException( "mu must be 1 or -1" );
        }

        public static ZTauElement Round( SimpleBigDecimal lambda0, SimpleBigDecimal lambda1, sbyte mu )
        {
            int scale = lambda0.Scale;
            if (lambda1.Scale != scale)
                throw new ArgumentException( "lambda0 and lambda1 do not have same scale" );
            if (mu != 1 && mu != -1)
                throw new ArgumentException( "mu must be 1 or -1" );
            BigInteger b1 = lambda0.Round();
            BigInteger b2 = lambda1.Round();
            SimpleBigDecimal b3 = lambda0.Subtract( b1 );
            SimpleBigDecimal b4 = lambda1.Subtract( b2 );
            SimpleBigDecimal simpleBigDecimal1 = b3.Add( b3 );
            SimpleBigDecimal simpleBigDecimal2 = mu != 1 ? simpleBigDecimal1.Subtract( b4 ) : simpleBigDecimal1.Add( b4 );
            SimpleBigDecimal b5 = b4.Add( b4 ).Add( b4 );
            SimpleBigDecimal b6 = b5.Add( b4 );
            SimpleBigDecimal simpleBigDecimal3;
            SimpleBigDecimal simpleBigDecimal4;
            if (mu == 1)
            {
                simpleBigDecimal3 = b3.Subtract( b5 );
                simpleBigDecimal4 = b3.Add( b6 );
            }
            else
            {
                simpleBigDecimal3 = b3.Add( b5 );
                simpleBigDecimal4 = b3.Subtract( b6 );
            }
            sbyte num1 = 0;
            sbyte num2 = 0;
            if (simpleBigDecimal2.CompareTo( BigInteger.One ) >= 0)
            {
                if (simpleBigDecimal3.CompareTo( MinusOne ) < 0)
                    num2 = mu;
                else
                    num1 = 1;
            }
            else if (simpleBigDecimal4.CompareTo( BigInteger.Two ) >= 0)
                num2 = mu;
            if (simpleBigDecimal2.CompareTo( MinusOne ) < 0)
            {
                if (simpleBigDecimal3.CompareTo( BigInteger.One ) >= 0)
                    num2 = (sbyte)-mu;
                else
                    num1 = -1;
            }
            else if (simpleBigDecimal4.CompareTo( MinusTwo ) < 0)
                num2 = (sbyte)-mu;
            return new ZTauElement( b1.Add( BigInteger.ValueOf( num1 ) ), b2.Add( BigInteger.ValueOf( num2 ) ) );
        }

        public static SimpleBigDecimal ApproximateDivisionByN(
          BigInteger k,
          BigInteger s,
          BigInteger vm,
          sbyte a,
          int m,
          int c )
        {
            int num = ((m + 5) / 2) + c;
            BigInteger val1 = k.ShiftRight( m - num - 2 + a );
            BigInteger bigInteger1 = s.Multiply( val1 );
            BigInteger val2 = bigInteger1.ShiftRight( m );
            BigInteger bigInteger2 = vm.Multiply( val2 );
            BigInteger bigInteger3 = bigInteger1.Add( bigInteger2 );
            BigInteger bigInt = bigInteger3.ShiftRight( num - c );
            if (bigInteger3.TestBit( num - c - 1 ))
                bigInt = bigInt.Add( BigInteger.One );
            return new SimpleBigDecimal( bigInt, c );
        }

        public static sbyte[] TauAdicNaf( sbyte mu, ZTauElement lambda )
        {
            if (mu != 1 && mu != -1)
                throw new ArgumentException( "mu must be 1 or -1" );
            int bitLength = Norm( mu, lambda ).BitLength;
            sbyte[] sourceArray = new sbyte[bitLength > 30 ? bitLength + 4 : 34];
            int index = 0;
            int num = 0;
            BigInteger bigInteger1 = lambda.u;
            BigInteger bigInteger2 = lambda.v;
            while (!bigInteger1.Equals( BigInteger.Zero ) || !bigInteger2.Equals( BigInteger.Zero ))
            {
                if (bigInteger1.TestBit( 0 ))
                {
                    sourceArray[index] = (sbyte)BigInteger.Two.Subtract( bigInteger1.Subtract( bigInteger2.ShiftLeft( 1 ) ).Mod( Four ) ).IntValue;
                    bigInteger1 = sourceArray[index] != 1 ? bigInteger1.Add( BigInteger.One ) : bigInteger1.ClearBit( 0 );
                    num = index;
                }
                else
                    sourceArray[index] = 0;
                BigInteger bigInteger3 = bigInteger1;
                BigInteger n = bigInteger1.ShiftRight( 1 );
                bigInteger1 = mu != 1 ? bigInteger2.Subtract( n ) : bigInteger2.Add( n );
                bigInteger2 = bigInteger3.ShiftRight( 1 ).Negate();
                ++index;
            }
            int length = num + 1;
            sbyte[] destinationArray = new sbyte[length];
            Array.Copy( sourceArray, 0, destinationArray, 0, length );
            return destinationArray;
        }

        public static AbstractF2mPoint Tau( AbstractF2mPoint p ) => p.Tau();

        public static sbyte GetMu( AbstractF2mCurve curve )
        {
            BigInteger bigInteger = curve.A.ToBigInteger();
            if (bigInteger.SignValue == 0)
                return -1;
            if (bigInteger.Equals( BigInteger.One ))
                return 1;
            throw new ArgumentException( "No Koblitz curve (ABC), TNAF multiplication not possible" );
        }

        public static sbyte GetMu( ECFieldElement curveA ) => curveA.IsZero ? (sbyte)-1 : (sbyte)1;

        public static sbyte GetMu( int curveA ) => curveA == 0 ? (sbyte)-1 : (sbyte)1;

        public static BigInteger[] GetLucas( sbyte mu, int k, bool doV )
        {
            if (mu != 1 && mu != -1)
                throw new ArgumentException( "mu must be 1 or -1" );
            BigInteger bigInteger1;
            BigInteger bigInteger2;
            if (doV)
            {
                bigInteger1 = BigInteger.Two;
                bigInteger2 = BigInteger.ValueOf( mu );
            }
            else
            {
                bigInteger1 = BigInteger.Zero;
                bigInteger2 = BigInteger.One;
            }
            for (int index = 1; index < k; ++index)
            {
                BigInteger bigInteger3 = (mu != 1 ? bigInteger2.Negate() : bigInteger2).Subtract( bigInteger1.ShiftLeft( 1 ) );
                bigInteger1 = bigInteger2;
                bigInteger2 = bigInteger3;
            }
            return new BigInteger[2] { bigInteger1, bigInteger2 };
        }

        public static BigInteger GetTw( sbyte mu, int w )
        {
            if (w == 4)
                return mu == 1 ? BigInteger.ValueOf( 6L ) : BigInteger.ValueOf( 10L );
            BigInteger[] lucas = GetLucas( mu, w, false );
            BigInteger m = BigInteger.Zero.SetBit( w );
            BigInteger val = lucas[1].ModInverse( m );
            return BigInteger.Two.Multiply( lucas[0] ).Multiply( val ).Mod( m );
        }

        public static BigInteger[] GetSi( AbstractF2mCurve curve )
        {
            int num = curve.IsKoblitz ? curve.FieldSize : throw new ArgumentException( "si is defined for Koblitz curves only" );
            int intValue = curve.A.ToBigInteger().IntValue;
            sbyte mu = GetMu( intValue );
            int shiftsForCofactor = GetShiftsForCofactor( curve.Cofactor );
            int k = num + 3 - intValue;
            BigInteger[] lucas = GetLucas( mu, k, false );
            if (mu == 1)
            {
                lucas[0] = lucas[0].Negate();
                lucas[1] = lucas[1].Negate();
            }
            return new BigInteger[2]
            {
        BigInteger.One.Add(lucas[1]).ShiftRight(shiftsForCofactor),
        BigInteger.One.Add(lucas[0]).ShiftRight(shiftsForCofactor).Negate()
            };
        }

        public static BigInteger[] GetSi( int fieldSize, int curveA, BigInteger cofactor )
        {
            sbyte mu = GetMu( curveA );
            int shiftsForCofactor = GetShiftsForCofactor( cofactor );
            int k = fieldSize + 3 - curveA;
            BigInteger[] lucas = GetLucas( mu, k, false );
            if (mu == 1)
            {
                lucas[0] = lucas[0].Negate();
                lucas[1] = lucas[1].Negate();
            }
            return new BigInteger[2]
            {
        BigInteger.One.Add(lucas[1]).ShiftRight(shiftsForCofactor),
        BigInteger.One.Add(lucas[0]).ShiftRight(shiftsForCofactor).Negate()
            };
        }

        protected static int GetShiftsForCofactor( BigInteger h )
        {
            if (h != null && h.BitLength < 4)
            {
                switch (h.IntValue)
                {
                    case 2:
                        return 1;
                    case 4:
                        return 2;
                }
            }
            throw new ArgumentException( "h (Cofactor) must be 2 or 4" );
        }

        public static ZTauElement PartModReduction(
          BigInteger k,
          int m,
          sbyte a,
          BigInteger[] s,
          sbyte mu,
          sbyte c )
        {
            BigInteger bigInteger = mu != 1 ? s[0].Subtract( s[1] ) : s[0].Add( s[1] );
            BigInteger luca = GetLucas( mu, m, true )[1];
            ZTauElement ztauElement = Round( ApproximateDivisionByN( k, s[0], luca, a, m, c ), ApproximateDivisionByN( k, s[1], luca, a, m, c ), mu );
            return new ZTauElement( k.Subtract( bigInteger.Multiply( ztauElement.u ) ).Subtract( BigInteger.ValueOf( 2L ).Multiply( s[1] ).Multiply( ztauElement.v ) ), s[1].Multiply( ztauElement.u ).Subtract( s[0].Multiply( ztauElement.v ) ) );
        }

        public static AbstractF2mPoint MultiplyRTnaf( AbstractF2mPoint p, BigInteger k )
        {
            AbstractF2mCurve curve = (AbstractF2mCurve)p.Curve;
            int fieldSize = curve.FieldSize;
            int intValue = curve.A.ToBigInteger().IntValue;
            sbyte mu = GetMu( intValue );
            BigInteger[] si = curve.GetSi();
            ZTauElement lambda = PartModReduction( k, fieldSize, (sbyte)intValue, si, mu, 10 );
            return MultiplyTnaf( p, lambda );
        }

        public static AbstractF2mPoint MultiplyTnaf( AbstractF2mPoint p, ZTauElement lambda )
        {
            sbyte[] u = TauAdicNaf( GetMu( p.Curve.A ), lambda );
            return MultiplyFromTnaf( p, u );
        }

        public static AbstractF2mPoint MultiplyFromTnaf( AbstractF2mPoint p, sbyte[] u )
        {
            AbstractF2mPoint abstractF2mPoint1 = (AbstractF2mPoint)p.Curve.Infinity;
            AbstractF2mPoint abstractF2mPoint2 = (AbstractF2mPoint)p.Negate();
            int pow = 0;
            for (int index = u.Length - 1; index >= 0; --index)
            {
                ++pow;
                sbyte num = u[index];
                if (num != 0)
                {
                    AbstractF2mPoint abstractF2mPoint3 = abstractF2mPoint1.TauPow( pow );
                    pow = 0;
                    ECPoint b = num > 0 ? p : (ECPoint)abstractF2mPoint2;
                    abstractF2mPoint1 = (AbstractF2mPoint)abstractF2mPoint3.Add( b );
                }
            }
            if (pow > 0)
                abstractF2mPoint1 = abstractF2mPoint1.TauPow( pow );
            return abstractF2mPoint1;
        }

        public static sbyte[] TauAdicWNaf(
          sbyte mu,
          ZTauElement lambda,
          sbyte width,
          BigInteger pow2w,
          BigInteger tw,
          ZTauElement[] alpha )
        {
            if (mu != 1 && mu != -1)
                throw new ArgumentException( "mu must be 1 or -1" );
            int bitLength = Norm( mu, lambda ).BitLength;
            sbyte[] numArray = new sbyte[bitLength > 30 ? bitLength + 4 + width : 34 + width];
            BigInteger bigInteger1 = pow2w.ShiftRight( 1 );
            BigInteger bigInteger2 = lambda.u;
            BigInteger bigInteger3 = lambda.v;
            int index1 = 0;
            while (!bigInteger2.Equals( BigInteger.Zero ) || !bigInteger3.Equals( BigInteger.Zero ))
            {
                if (bigInteger2.TestBit( 0 ))
                {
                    BigInteger bigInteger4 = bigInteger2.Add( bigInteger3.Multiply( tw ) ).Mod( pow2w );
                    sbyte index2 = bigInteger4.CompareTo( bigInteger1 ) < 0 ? (sbyte)bigInteger4.IntValue : (sbyte)bigInteger4.Subtract( pow2w ).IntValue;
                    numArray[index1] = index2;
                    bool flag = true;
                    if (index2 < 0)
                    {
                        flag = false;
                        index2 = (sbyte)-index2;
                    }
                    if (flag)
                    {
                        bigInteger2 = bigInteger2.Subtract( alpha[index2].u );
                        bigInteger3 = bigInteger3.Subtract( alpha[index2].v );
                    }
                    else
                    {
                        bigInteger2 = bigInteger2.Add( alpha[index2].u );
                        bigInteger3 = bigInteger3.Add( alpha[index2].v );
                    }
                }
                else
                    numArray[index1] = 0;
                BigInteger bigInteger5 = bigInteger2;
                bigInteger2 = mu != 1 ? bigInteger3.Subtract( bigInteger2.ShiftRight( 1 ) ) : bigInteger3.Add( bigInteger2.ShiftRight( 1 ) );
                bigInteger3 = bigInteger5.ShiftRight( 1 ).Negate();
                ++index1;
            }
            return numArray;
        }

        public static AbstractF2mPoint[] GetPreComp( AbstractF2mPoint p, sbyte a )
        {
            sbyte[][] numArray = a == 0 ? Alpha0Tnaf : Alpha1Tnaf;
            AbstractF2mPoint[] points = new AbstractF2mPoint[(int)(IntPtr)(uint)(numArray.Length + 1 >>> 1)];
            points[0] = p;
            uint length = (uint)numArray.Length;
            for (uint index = 3; index < length; index += 2U)
                points[(int)(IntPtr)(index >> 1)] = MultiplyFromTnaf( p, numArray[(int)(IntPtr)index] );
            p.Curve.NormalizeAll( points );
            return points;
        }
    }
}
