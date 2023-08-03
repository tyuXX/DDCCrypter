// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Raw.Interleave
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.Raw
{
    internal abstract class Interleave
    {
        private const ulong M32 = 1431655765;
        private const ulong M64 = 6148914691236517205;

        internal static uint Expand8to16( uint x )
        {
            x &= byte.MaxValue;
            x = (uint)(((int)x | ((int)x << 4)) & 3855);
            x = (uint)(((int)x | ((int)x << 2)) & 13107);
            x = (uint)(((int)x | ((int)x << 1)) & 21845);
            return x;
        }

        internal static uint Expand16to32( uint x )
        {
            x &= ushort.MaxValue;
            x = (uint)(((int)x | ((int)x << 8)) & 16711935);
            x = (uint)(((int)x | ((int)x << 4)) & 252645135);
            x = (uint)(((int)x | ((int)x << 2)) & 858993459);
            x = (uint)(((int)x | ((int)x << 1)) & 1431655765);
            return x;
        }

        internal static ulong Expand32to64( uint x )
        {
            uint num1 = (uint)(((int)x ^ (int)(x >> 8)) & 65280);
            x ^= num1 ^ (num1 << 8);
            uint num2 = (uint)(((int)x ^ (int)(x >> 4)) & 15728880);
            x ^= num2 ^ (num2 << 4);
            uint num3 = (uint)(((int)x ^ (int)(x >> 2)) & 202116108);
            x ^= num3 ^ (num3 << 2);
            uint num4 = (uint)(((int)x ^ (int)(x >> 1)) & 572662306);
            x ^= num4 ^ (num4 << 1);
            return (ulong)((((x >> 1) & 1431655765L) << 32) | (x & 1431655765L));
        }

        internal static void Expand64To128( ulong x, ulong[] z, int zOff )
        {
            ulong num1 = (x ^ (x >> 16)) & 4294901760UL;
            x ^= num1 ^ (num1 << 16);
            ulong num2 = (ulong)(((long)x ^ (long)(x >> 8)) & 280375465148160L);
            x ^= num2 ^ (num2 << 8);
            ulong num3 = (ulong)(((long)x ^ (long)(x >> 4)) & 67555025218437360L);
            x ^= num3 ^ (num3 << 4);
            ulong num4 = (ulong)(((long)x ^ (long)(x >> 2)) & 868082074056920076L);
            x ^= num4 ^ (num4 << 2);
            ulong num5 = (ulong)(((long)x ^ (long)(x >> 1)) & 2459565876494606882L);
            x ^= num5 ^ (num5 << 1);
            z[zOff] = x & 6148914691236517205UL;
            z[zOff + 1] = (x >> 1) & 6148914691236517205UL;
        }

        internal static ulong Unshuffle( ulong x )
        {
            ulong num1 = (ulong)(((long)x ^ (long)(x >> 1)) & 2459565876494606882L);
            x ^= num1 ^ (num1 << 1);
            ulong num2 = (ulong)(((long)x ^ (long)(x >> 2)) & 868082074056920076L);
            x ^= num2 ^ (num2 << 2);
            ulong num3 = (ulong)(((long)x ^ (long)(x >> 4)) & 67555025218437360L);
            x ^= num3 ^ (num3 << 4);
            ulong num4 = (ulong)(((long)x ^ (long)(x >> 8)) & 280375465148160L);
            x ^= num4 ^ (num4 << 8);
            ulong num5 = (x ^ (x >> 16)) & 4294901760UL;
            x ^= num5 ^ (num5 << 16);
            return x;
        }
    }
}
