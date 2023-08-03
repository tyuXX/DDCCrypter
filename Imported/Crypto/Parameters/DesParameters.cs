// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DesParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DesParameters : KeyParameter
    {
        public const int DesKeyLength = 8;
        private const int N_DES_WEAK_KEYS = 16;
        private static readonly byte[] DES_weak_keys = new byte[128]
        {
       1,
       1,
       1,
       1,
       1,
       1,
       1,
       1,
       31,
       31,
       31,
       31,
       14,
       14,
       14,
       14,
       224,
       224,
       224,
       224,
       241,
       241,
       241,
       241,
       254,
       254,
       254,
       254,
       254,
       254,
       254,
       254,
       1,
       254,
       1,
       254,
       1,
       254,
       1,
       254,
       31,
       224,
       31,
       224,
       14,
       241,
       14,
       241,
       1,
       224,
       1,
       224,
       1,
       241,
       1,
       241,
       31,
       254,
       31,
       254,
       14,
       254,
       14,
       254,
       1,
       31,
       1,
       31,
       1,
       14,
       1,
       14,
       224,
       254,
       224,
       254,
       241,
       254,
       241,
       254,
       254,
       1,
       254,
       1,
       254,
       1,
       254,
       1,
       224,
       31,
       224,
       31,
       241,
       14,
       241,
       14,
       224,
       1,
       224,
       1,
       241,
       1,
       241,
       1,
       254,
       31,
       254,
       31,
       254,
       14,
       254,
       14,
       31,
       1,
       31,
       1,
       14,
       1,
       14,
       1,
       254,
       224,
       254,
       224,
       254,
       241,
       254,
       241
        };

        public DesParameters( byte[] key )
          : base( key )
        {
            if (IsWeakKey( key ))
                throw new ArgumentException( "attempt to create weak DES key" );
        }

        public DesParameters( byte[] key, int keyOff, int keyLen )
          : base( key, keyOff, keyLen )
        {
            if (IsWeakKey( key, keyOff ))
                throw new ArgumentException( "attempt to create weak DES key" );
        }

        public static bool IsWeakKey( byte[] key, int offset )
        {
            if (key.Length - offset < 8)
                throw new ArgumentException( "key material too short." );
            for (int index1 = 0; index1 < 16; ++index1)
            {
                bool flag = false;
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    if (key[index2 + offset] != DES_weak_keys[(index1 * 8) + index2])
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return true;
            }
            return false;
        }

        public static bool IsWeakKey( byte[] key ) => IsWeakKey( key, 0 );

        public static byte SetOddParity( byte b )
        {
            uint num1 = b ^ 1U;
            uint num2 = num1 ^ (num1 >> 4);
            uint num3 = num2 ^ (num2 >> 2);
            uint num4 = (num3 ^ (num3 >> 1)) & 1U;
            return (byte)(b ^ num4);
        }

        public static void SetOddParity( byte[] bytes )
        {
            for (int index = 0; index < bytes.Length; ++index)
                bytes[index] = SetOddParity( bytes[index] );
        }

        public static void SetOddParity( byte[] bytes, int off, int len )
        {
            for (int index = 0; index < len; ++index)
                bytes[off + index] = SetOddParity( bytes[off + index] );
        }
    }
}
