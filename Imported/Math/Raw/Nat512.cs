// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat512
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat512
    {
        public static void Mul( uint[] x, uint[] y, uint[] zz )
        {
            Nat256.Mul( x, y, zz );
            Nat256.Mul( x, 8, y, 8, zz, 16 );
            uint eachOther = Nat256.AddToEachOther( zz, 8, zz, 16 );
            uint cIn = eachOther + Nat256.AddTo( zz, 0, zz, 8, 0U );
            uint num1 = eachOther + Nat256.AddTo( zz, 24, zz, 16, cIn );
            uint[] numArray1 = Nat256.Create();
            uint[] numArray2 = Nat256.Create();
            bool flag = Nat256.Diff( x, 8, x, 0, numArray1, 0 ) != Nat256.Diff( y, 8, y, 0, numArray2, 0 );
            uint[] ext = Nat256.CreateExt();
            Nat256.Mul( numArray1, numArray2, ext );
            int num2 = (int)Nat.AddWordAt( 32, num1 + (flag ? Nat.AddTo( 16, ext, 0, zz, 8 ) : (uint)Nat.SubFrom( 16, ext, 0, zz, 8 )), zz, 24 );
        }

        public static void Square( uint[] x, uint[] zz )
        {
            Nat256.Square( x, zz );
            Nat256.Square( x, 8, zz, 16 );
            uint eachOther = Nat256.AddToEachOther( zz, 8, zz, 16 );
            uint cIn = eachOther + Nat256.AddTo( zz, 0, zz, 8, 0U );
            uint num1 = eachOther + Nat256.AddTo( zz, 24, zz, 16, cIn );
            uint[] numArray = Nat256.Create();
            Nat256.Diff( x, 8, x, 0, numArray, 0 );
            uint[] ext = Nat256.CreateExt();
            Nat256.Square( numArray, ext );
            int num2 = (int)Nat.AddWordAt( 32, num1 + (uint)Nat.SubFrom( 16, ext, 0, zz, 8 ), zz, 24 );
        }
    }
}
