// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Mod
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Mod
    {
        private static readonly SecureRandom RandomSource = new();

        public static void Invert( uint[] p, uint[] x, uint[] z )
        {
            int length = p.Length;
            if (Nat.IsZero( length, x ))
                throw new ArgumentException( "cannot be 0", nameof( x ) );
            if (Nat.IsOne( length, x ))
            {
                Array.Copy( x, 0, z, 0, length );
            }
            else
            {
                uint[] numArray1 = Nat.Copy( length, x );
                uint[] numArray2 = Nat.Create( length );
                numArray2[0] = 1U;
                int xc1 = 0;
                if (((int)numArray1[0] & 1) == 0)
                    InversionStep( p, numArray1, length, numArray2, ref xc1 );
                if (Nat.IsOne( length, numArray1 ))
                {
                    InversionResult( p, xc1, numArray2, z );
                }
                else
                {
                    uint[] numArray3 = Nat.Copy( length, p );
                    uint[] numArray4 = Nat.Create( length );
                    int xc2 = 0;
                    int uLen = length;
                    while (true)
                    {
                        while (numArray1[uLen - 1] != 0U || numArray3[uLen - 1] != 0U)
                        {
                            if (Nat.Gte( length, numArray1, numArray3 ))
                            {
                                Nat.SubFrom( length, numArray3, numArray1 );
                                xc1 += Nat.SubFrom( length, numArray4, numArray2 ) - xc2;
                                InversionStep( p, numArray1, uLen, numArray2, ref xc1 );
                                if (Nat.IsOne( length, numArray1 ))
                                {
                                    InversionResult( p, xc1, numArray2, z );
                                    return;
                                }
                            }
                            else
                            {
                                Nat.SubFrom( length, numArray1, numArray3 );
                                xc2 += Nat.SubFrom( length, numArray2, numArray4 ) - xc1;
                                InversionStep( p, numArray3, uLen, numArray4, ref xc2 );
                                if (Nat.IsOne( length, numArray3 ))
                                {
                                    InversionResult( p, xc2, numArray4, z );
                                    return;
                                }
                            }
                        }
                        --uLen;
                    }
                }
            }
        }

        public static uint[] Random( uint[] p )
        {
            int length = p.Length;
            uint[] numArray1 = Nat.Create( length );
            uint num1 = p[length - 1];
            uint num2 = num1 | (num1 >> 1);
            uint num3 = num2 | (num2 >> 2);
            uint num4 = num3 | (num3 >> 4);
            uint num5 = num4 | (num4 >> 8);
            uint num6 = num5 | (num5 >> 16);
            do
            {
                byte[] numArray2 = new byte[length << 2];
                RandomSource.NextBytes( numArray2 );
                Pack.BE_To_UInt32( numArray2, 0, numArray1 );
                numArray1[length - 1] &= num6;
            }
            while (Nat.Gte( length, numArray1, p ));
            return numArray1;
        }

        public static void Add( uint[] p, uint[] x, uint[] y, uint[] z )
        {
            int length = p.Length;
            if (Nat.Add( length, x, y, z ) == 0U)
                return;
            Nat.SubFrom( length, p, z );
        }

        public static void Subtract( uint[] p, uint[] x, uint[] y, uint[] z )
        {
            int length = p.Length;
            if (Nat.Sub( length, x, y, z ) == 0)
                return;
            int num = (int)Nat.AddTo( length, p, z );
        }

        private static void InversionResult( uint[] p, int ac, uint[] a, uint[] z )
        {
            if (ac < 0)
            {
                int num = (int)Nat.Add( p.Length, a, p, z );
            }
            else
                Array.Copy( a, 0, z, 0, p.Length );
        }

        private static void InversionStep( uint[] p, uint[] u, int uLen, uint[] x, ref int xc )
        {
            int length = p.Length;
            int num1 = 0;
            while (u[0] == 0U)
            {
                int num2 = (int)Nat.ShiftDownWord( uLen, u, 0U );
                num1 += 32;
            }
            int trailingZeroes = GetTrailingZeroes( u[0] );
            if (trailingZeroes > 0)
            {
                int num3 = (int)Nat.ShiftDownBits( uLen, u, trailingZeroes, 0U );
                num1 += trailingZeroes;
            }
            for (int index = 0; index < num1; ++index)
            {
                if (((int)x[0] & 1) != 0)
                {
                    if (xc < 0)
                        xc += (int)Nat.AddTo( length, p, x );
                    else
                        xc += Nat.SubFrom( length, p, x );
                }
                int num4 = (int)Nat.ShiftDownBit( length, x, (uint)xc );
            }
        }

        private static int GetTrailingZeroes( uint x )
        {
            int trailingZeroes = 0;
            while (((int)x & 1) == 0)
            {
                x >>= 1;
                ++trailingZeroes;
            }
            return trailingZeroes;
        }
    }
}
