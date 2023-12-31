﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP192K1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP192K1Field
    {
        private const uint P5 = 4294967295;
        private const uint PExt11 = 4294967295;
        private const uint PInv33 = 4553;
        internal static readonly uint[] P = new uint[6]
        {
      4294962743U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[12]
        {
      20729809U,
      9106U,
      1U,
      0U,
      0U,
      0U,
      4294958190U,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[8]
        {
      4274237487U,
      4294958189U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      9105U,
      2U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat192.Add( x, y, z ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 6, 4553U, z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if ((Nat.Add( 12, xx, yy, zz ) == 0U && (zz[11] != uint.MaxValue || !Nat.Gte( 12, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 12, zz, PExtInv.Length );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 6, x, z ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 6, 4553U, z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat192.FromBigInteger( x );
            if (numArray[5] == uint.MaxValue && Nat192.Gte( numArray, P ))
                Nat192.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 6, x, 0U, z );
            }
            else
            {
                uint c = Nat192.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 6, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat192.CreateExt();
            Nat192.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            if ((Nat192.MulAddTo( x, y, zz ) == 0U && (zz[11] != uint.MaxValue || !Nat.Gte( 12, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 12, zz, PExtInv.Length );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat192.IsZero( x ))
                Nat192.Zero( z );
            else
                Nat192.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            if (Nat192.Mul33DWordAdd( 4553U, Nat192.Mul33Add( 4553U, xx, 6, xx, 0, z, 0 ), z, 0 ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 6, 4553U, z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            if ((x == 0U || Nat192.Mul33WordAdd( 4553U, x, z, 0 ) == 0U) && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 6, 4553U, z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat192.CreateExt();
            Nat192.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat192.CreateExt();
            Nat192.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat192.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat192.Sub( x, y, z ) == 0)
                return;
            Nat.Sub33From( 6, 4553U, z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 12, xx, yy, zz ) == 0 || Nat.SubFrom( PExtInv.Length, PExtInv, zz ) == 0)
                return;
            Nat.DecAt( 12, zz, PExtInv.Length );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 6, x, 0U, z ) == 0U && (z[5] != uint.MaxValue || !Nat192.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 6, 4553U, z );
        }
    }
}
