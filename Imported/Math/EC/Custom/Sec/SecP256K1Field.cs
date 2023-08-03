// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Custom.Sec.SecP256K1Field
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    internal class SecP256K1Field
    {
        private const uint P7 = 4294967295;
        private const uint PExt15 = 4294967295;
        private const uint PInv33 = 977;
        internal static readonly uint[] P = new uint[8]
        {
      4294966319U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        internal static readonly uint[] PExt = new uint[16]
        {
      954529U,
      1954U,
      1U,
      0U,
      0U,
      0U,
      0U,
      0U,
      4294965342U,
      4294967293U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[10]
        {
      4294012767U,
      4294965341U,
      4294967294U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      1953U,
      2U
        };

        public static void Add( uint[] x, uint[] y, uint[] z )
        {
            if (Nat256.Add( x, y, z ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 8, 977U, z );
        }

        public static void AddExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if ((Nat.Add( 16, xx, yy, zz ) == 0U && (zz[15] != uint.MaxValue || !Nat.Gte( 16, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 16, zz, PExtInv.Length );
        }

        public static void AddOne( uint[] x, uint[] z )
        {
            if (Nat.Inc( 8, x, z ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 8, 977U, z );
        }

        public static uint[] FromBigInteger( BigInteger x )
        {
            uint[] numArray = Nat256.FromBigInteger( x );
            if (numArray[7] == uint.MaxValue && Nat256.Gte( numArray, P ))
                Nat256.SubFrom( P, numArray );
            return numArray;
        }

        public static void Half( uint[] x, uint[] z )
        {
            if (((int)x[0] & 1) == 0)
            {
                int num1 = (int)Nat.ShiftDownBit( 8, x, 0U, z );
            }
            else
            {
                uint c = Nat256.Add( x, P, z );
                int num2 = (int)Nat.ShiftDownBit( 8, z, c );
            }
        }

        public static void Multiply( uint[] x, uint[] y, uint[] z )
        {
            uint[] ext = Nat256.CreateExt();
            Nat256.Mul( x, y, ext );
            Reduce( ext, z );
        }

        public static void MultiplyAddToExt( uint[] x, uint[] y, uint[] zz )
        {
            if ((Nat256.MulAddTo( x, y, zz ) == 0U && (zz[15] != uint.MaxValue || !Nat.Gte( 16, zz, PExt ))) || Nat.AddTo( PExtInv.Length, PExtInv, zz ) == 0U)
                return;
            int num = (int)Nat.IncAt( 16, zz, PExtInv.Length );
        }

        public static void Negate( uint[] x, uint[] z )
        {
            if (Nat256.IsZero( x ))
                Nat256.Zero( z );
            else
                Nat256.Sub( P, x, z );
        }

        public static void Reduce( uint[] xx, uint[] z )
        {
            if (Nat256.Mul33DWordAdd( 977U, Nat256.Mul33Add( 977U, xx, 8, xx, 0, z, 0 ), z, 0 ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 8, 977U, z );
        }

        public static void Reduce32( uint x, uint[] z )
        {
            if ((x == 0U || Nat256.Mul33WordAdd( 977U, x, z, 0 ) == 0U) && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 8, 977U, z );
        }

        public static void Square( uint[] x, uint[] z )
        {
            uint[] ext = Nat256.CreateExt();
            Nat256.Square( x, ext );
            Reduce( ext, z );
        }

        public static void SquareN( uint[] x, int n, uint[] z )
        {
            uint[] ext = Nat256.CreateExt();
            Nat256.Square( x, ext );
            Reduce( ext, z );
            while (--n > 0)
            {
                Nat256.Square( z, ext );
                Reduce( ext, z );
            }
        }

        public static void Subtract( uint[] x, uint[] y, uint[] z )
        {
            if (Nat256.Sub( x, y, z ) == 0)
                return;
            Nat.Sub33From( 8, 977U, z );
        }

        public static void SubtractExt( uint[] xx, uint[] yy, uint[] zz )
        {
            if (Nat.Sub( 16, xx, yy, zz ) == 0 || Nat.SubFrom( PExtInv.Length, PExtInv, zz ) == 0)
                return;
            Nat.DecAt( 16, zz, PExtInv.Length );
        }

        public static void Twice( uint[] x, uint[] z )
        {
            if (Nat.ShiftUpBit( 8, x, 0U, z ) == 0U && (z[7] != uint.MaxValue || !Nat256.Gte( z, P )))
                return;
            int num = (int)Nat.Add33To( 8, 977U, z );
        }
    }
}
