// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Nat384
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Nat384
    {
        public static void Mul( uint[] x, uint[] y, uint[] zz )
        {
            Nat192.Mul( x, y, zz );
            Nat192.Mul( x, 6, y, 6, zz, 12 );
            uint eachOther = Nat192.AddToEachOther( zz, 6, zz, 12 );
            uint cIn = eachOther + Nat192.AddTo( zz, 0, zz, 6, 0U );
            uint num1 = eachOther + Nat192.AddTo( zz, 18, zz, 12, cIn );
            uint[] numArray1 = Nat192.Create();
            uint[] numArray2 = Nat192.Create();
            bool flag = Nat192.Diff( x, 6, x, 0, numArray1, 0 ) != Nat192.Diff( y, 6, y, 0, numArray2, 0 );
            uint[] ext = Nat192.CreateExt();
            Nat192.Mul( numArray1, numArray2, ext );
            int num2 = (int)Nat.AddWordAt( 24, num1 + (flag ? Nat.AddTo( 12, ext, 0, zz, 6 ) : (uint)Nat.SubFrom( 12, ext, 0, zz, 6 )), zz, 18 );
        }

        public static void Square( uint[] x, uint[] zz )
        {
            Nat192.Square( x, zz );
            Nat192.Square( x, 6, zz, 12 );
            uint eachOther = Nat192.AddToEachOther( zz, 6, zz, 12 );
            uint cIn = eachOther + Nat192.AddTo( zz, 0, zz, 6, 0U );
            uint num1 = eachOther + Nat192.AddTo( zz, 18, zz, 12, cIn );
            uint[] numArray = Nat192.Create();
            Nat192.Diff( x, 6, x, 0, numArray, 0 );
            uint[] ext = Nat192.CreateExt();
            Nat192.Square( numArray, ext );
            int num2 = (int)Nat.AddWordAt( 24, num1 + (uint)Nat.SubFrom( 12, ext, 0, zz, 6 ), zz, 18 );
        }
    }
}
