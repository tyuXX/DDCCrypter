// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.DesEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class DesEngine : IBlockCipher
    {
        internal const int BLOCK_SIZE = 8;
        private int[] workingKey;
        private static readonly short[] bytebit = new short[8]
        {
       128,
       64,
       32,
       16,
       8,
       4,
       2,
       1
        };
        private static readonly int[] bigbyte = new int[24]
        {
      8388608,
      4194304,
      2097152,
      1048576,
      524288,
      262144,
      131072,
      65536,
      32768,
      16384,
      8192,
      4096,
      2048,
      1024,
      512,
      256,
      128,
      64,
      32,
      16,
      8,
      4,
      2,
      1
        };
        private static readonly byte[] pc1 = new byte[56]
        {
       56,
       48,
       40,
       32,
       24,
       16,
       8,
       0,
       57,
       49,
       41,
       33,
       25,
       17,
       9,
       1,
       58,
       50,
       42,
       34,
       26,
       18,
       10,
       2,
       59,
       51,
       43,
       35,
       62,
       54,
       46,
       38,
       30,
       22,
       14,
       6,
       61,
       53,
       45,
       37,
       29,
       21,
       13,
       5,
       60,
       52,
       44,
       36,
       28,
       20,
       12,
       4,
       27,
       19,
       11,
       3
        };
        private static readonly byte[] totrot = new byte[16]
        {
       1,
       2,
       4,
       6,
       8,
       10,
       12,
       14,
       15,
       17,
       19,
       21,
       23,
       25,
       27,
       28
        };
        private static readonly byte[] pc2 = new byte[48]
        {
       13,
       16,
       10,
       23,
       0,
       4,
       2,
       27,
       14,
       5,
       20,
       9,
       22,
       18,
       11,
       3,
       25,
       7,
       15,
       6,
       26,
       19,
       12,
       1,
       40,
       51,
       30,
       36,
       46,
       54,
       29,
       39,
       50,
       44,
       32,
       47,
       43,
       48,
       38,
       55,
       33,
       52,
       45,
       41,
       49,
       35,
       28,
       31
        };
        private static readonly uint[] SP1 = new uint[64]
        {
      16843776U,
      0U,
      65536U,
      16843780U,
      16842756U,
      66564U,
      4U,
      65536U,
      1024U,
      16843776U,
      16843780U,
      1024U,
      16778244U,
      16842756U,
      16777216U,
      4U,
      1028U,
      16778240U,
      16778240U,
      66560U,
      66560U,
      16842752U,
      16842752U,
      16778244U,
      65540U,
      16777220U,
      16777220U,
      65540U,
      0U,
      1028U,
      66564U,
      16777216U,
      65536U,
      16843780U,
      4U,
      16842752U,
      16843776U,
      16777216U,
      16777216U,
      1024U,
      16842756U,
      65536U,
      66560U,
      16777220U,
      1024U,
      4U,
      16778244U,
      66564U,
      16843780U,
      65540U,
      16842752U,
      16778244U,
      16777220U,
      1028U,
      66564U,
      16843776U,
      1028U,
      16778240U,
      16778240U,
      0U,
      65540U,
      66560U,
      0U,
      16842756U
        };
        private static readonly uint[] SP2 = new uint[64]
        {
      2148565024U,
      2147516416U,
      32768U,
      1081376U,
      1048576U,
      32U,
      2148532256U,
      2147516448U,
      2147483680U,
      2148565024U,
      2148564992U,
      2147483648U,
      2147516416U,
      1048576U,
      32U,
      2148532256U,
      1081344U,
      1048608U,
      2147516448U,
      0U,
      2147483648U,
      32768U,
      1081376U,
      2148532224U,
      1048608U,
      2147483680U,
      0U,
      1081344U,
      32800U,
      2148564992U,
      2148532224U,
      32800U,
      0U,
      1081376U,
      2148532256U,
      1048576U,
      2147516448U,
      2148532224U,
      2148564992U,
      32768U,
      2148532224U,
      2147516416U,
      32U,
      2148565024U,
      1081376U,
      32U,
      32768U,
      2147483648U,
      32800U,
      2148564992U,
      1048576U,
      2147483680U,
      1048608U,
      2147516448U,
      2147483680U,
      1048608U,
      1081344U,
      0U,
      2147516416U,
      32800U,
      2147483648U,
      2148532256U,
      2148565024U,
      1081344U
        };
        private static readonly uint[] SP3 = new uint[64]
        {
      520U,
      134349312U,
      0U,
      134348808U,
      134218240U,
      0U,
      131592U,
      134218240U,
      131080U,
      134217736U,
      134217736U,
      131072U,
      134349320U,
      131080U,
      134348800U,
      520U,
      134217728U,
      8U,
      134349312U,
      512U,
      131584U,
      134348800U,
      134348808U,
      131592U,
      134218248U,
      131584U,
      131072U,
      134218248U,
      8U,
      134349320U,
      512U,
      134217728U,
      134349312U,
      134217728U,
      131080U,
      520U,
      131072U,
      134349312U,
      134218240U,
      0U,
      512U,
      131080U,
      134349320U,
      134218240U,
      134217736U,
      512U,
      0U,
      134348808U,
      134218248U,
      131072U,
      134217728U,
      134349320U,
      8U,
      131592U,
      131584U,
      134217736U,
      134348800U,
      134218248U,
      520U,
      134348800U,
      131592U,
      8U,
      134348808U,
      131584U
        };
        private static readonly uint[] SP4 = new uint[64]
        {
      8396801U,
      8321U,
      8321U,
      128U,
      8396928U,
      8388737U,
      8388609U,
      8193U,
      0U,
      8396800U,
      8396800U,
      8396929U,
      129U,
      0U,
      8388736U,
      8388609U,
      1U,
      8192U,
      8388608U,
      8396801U,
      128U,
      8388608U,
      8193U,
      8320U,
      8388737U,
      1U,
      8320U,
      8388736U,
      8192U,
      8396928U,
      8396929U,
      129U,
      8388736U,
      8388609U,
      8396800U,
      8396929U,
      129U,
      0U,
      0U,
      8396800U,
      8320U,
      8388736U,
      8388737U,
      1U,
      8396801U,
      8321U,
      8321U,
      128U,
      8396929U,
      129U,
      1U,
      8192U,
      8388609U,
      8193U,
      8396928U,
      8388737U,
      8193U,
      8320U,
      8388608U,
      8396801U,
      128U,
      8388608U,
      8192U,
      8396928U
        };
        private static readonly uint[] SP5 = new uint[64]
        {
      256U,
      34078976U,
      34078720U,
      1107296512U,
      524288U,
      256U,
      1073741824U,
      34078720U,
      1074266368U,
      524288U,
      33554688U,
      1074266368U,
      1107296512U,
      1107820544U,
      524544U,
      1073741824U,
      33554432U,
      1074266112U,
      1074266112U,
      0U,
      1073742080U,
      1107820800U,
      1107820800U,
      33554688U,
      1107820544U,
      1073742080U,
      0U,
      1107296256U,
      34078976U,
      33554432U,
      1107296256U,
      524544U,
      524288U,
      1107296512U,
      256U,
      33554432U,
      1073741824U,
      34078720U,
      1107296512U,
      1074266368U,
      33554688U,
      1073741824U,
      1107820544U,
      34078976U,
      1074266368U,
      256U,
      33554432U,
      1107820544U,
      1107820800U,
      524544U,
      1107296256U,
      1107820800U,
      34078720U,
      0U,
      1074266112U,
      1107296256U,
      524544U,
      33554688U,
      1073742080U,
      524288U,
      0U,
      1074266112U,
      34078976U,
      1073742080U
        };
        private static readonly uint[] SP6 = new uint[64]
        {
      536870928U,
      541065216U,
      16384U,
      541081616U,
      541065216U,
      16U,
      541081616U,
      4194304U,
      536887296U,
      4210704U,
      4194304U,
      536870928U,
      4194320U,
      536887296U,
      536870912U,
      16400U,
      0U,
      4194320U,
      536887312U,
      16384U,
      4210688U,
      536887312U,
      16U,
      541065232U,
      541065232U,
      0U,
      4210704U,
      541081600U,
      16400U,
      4210688U,
      541081600U,
      536870912U,
      536887296U,
      16U,
      541065232U,
      4210688U,
      541081616U,
      4194304U,
      16400U,
      536870928U,
      4194304U,
      536887296U,
      536870912U,
      16400U,
      536870928U,
      541081616U,
      4210688U,
      541065216U,
      4210704U,
      541081600U,
      0U,
      541065232U,
      16U,
      16384U,
      541065216U,
      4210704U,
      16384U,
      4194320U,
      536887312U,
      0U,
      541081600U,
      536870912U,
      4194320U,
      536887312U
        };
        private static readonly uint[] SP7 = new uint[64]
        {
      2097152U,
      69206018U,
      67110914U,
      0U,
      2048U,
      67110914U,
      2099202U,
      69208064U,
      69208066U,
      2097152U,
      0U,
      67108866U,
      2U,
      67108864U,
      69206018U,
      2050U,
      67110912U,
      2099202U,
      2097154U,
      67110912U,
      67108866U,
      69206016U,
      69208064U,
      2097154U,
      69206016U,
      2048U,
      2050U,
      69208066U,
      2099200U,
      2U,
      67108864U,
      2099200U,
      67108864U,
      2099200U,
      2097152U,
      67110914U,
      67110914U,
      69206018U,
      69206018U,
      2U,
      2097154U,
      67108864U,
      67110912U,
      2097152U,
      69208064U,
      2050U,
      2099202U,
      69208064U,
      2050U,
      67108866U,
      69208066U,
      69206016U,
      2099200U,
      0U,
      2U,
      69208066U,
      0U,
      2099202U,
      69206016U,
      2048U,
      67108866U,
      67110912U,
      2048U,
      2097154U
        };
        private static readonly uint[] SP8 = new uint[64]
        {
      268439616U,
      4096U,
      262144U,
      268701760U,
      268435456U,
      268439616U,
      64U,
      268435456U,
      262208U,
      268697600U,
      268701760U,
      266240U,
      268701696U,
      266304U,
      4096U,
      64U,
      268697600U,
      268435520U,
      268439552U,
      4160U,
      266240U,
      262208U,
      268697664U,
      268701696U,
      4160U,
      0U,
      0U,
      268697664U,
      268435520U,
      268439552U,
      266304U,
      262144U,
      266304U,
      262144U,
      268701696U,
      4096U,
      64U,
      268697664U,
      4096U,
      266304U,
      268439552U,
      64U,
      268435520U,
      268697600U,
      268697664U,
      268435456U,
      262144U,
      268439616U,
      0U,
      268701760U,
      262208U,
      268435520U,
      268697600U,
      268439552U,
      268439616U,
      0U,
      268701760U,
      266240U,
      266240U,
      4160U,
      4160U,
      262208U,
      268435456U,
      268701696U
        };

        public virtual int[] GetWorkingKey() => this.workingKey;

        public virtual void Init( bool forEncryption, ICipherParameters parameters ) => this.workingKey = parameters is KeyParameter ? GenerateWorkingKey( forEncryption, ((KeyParameter)parameters).GetKey() ) : throw new ArgumentException( "invalid parameter passed to DES init - " + Platform.GetTypeName( parameters ) );

        public virtual string AlgorithmName => "DES";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 8;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.workingKey == null)
                throw new InvalidOperationException( "DES engine not initialised" );
            Check.DataLength( input, inOff, 8, "input buffer too short" );
            Check.OutputLength( output, outOff, 8, "output buffer too short" );
            DesFunc( this.workingKey, input, inOff, output, outOff );
            return 8;
        }

        public virtual void Reset()
        {
        }

        protected static int[] GenerateWorkingKey( bool encrypting, byte[] key )
        {
            int[] workingKey = new int[32];
            bool[] flagArray1 = new bool[56];
            bool[] flagArray2 = new bool[56];
            for (int index = 0; index < 56; ++index)
            {
                int num = pc1[index];
                flagArray1[index] = (key[(int)(IntPtr)(uint)(num >>> 3)] & bytebit[num & 7]) != 0;
            }
            for (int index1 = 0; index1 < 16; ++index1)
            {
                int index2 = !encrypting ? (15 - index1) << 1 : index1 << 1;
                int index3 = index2 + 1;
                workingKey[index2] = workingKey[index3] = 0;
                for (int index4 = 0; index4 < 28; ++index4)
                {
                    int index5 = index4 + totrot[index1];
                    flagArray2[index4] = index5 >= 28 ? flagArray1[index5 - 28] : flagArray1[index5];
                }
                for (int index6 = 28; index6 < 56; ++index6)
                {
                    int index7 = index6 + totrot[index1];
                    flagArray2[index6] = index7 >= 56 ? flagArray1[index7 - 28] : flagArray1[index7];
                }
                for (int index8 = 0; index8 < 24; ++index8)
                {
                    if (flagArray2[pc2[index8]])
                        workingKey[index2] |= bigbyte[index8];
                    if (flagArray2[pc2[index8 + 24]])
                        workingKey[index3] |= bigbyte[index8];
                }
            }
            for (int index = 0; index != 32; index += 2)
            {
                int num1 = workingKey[index];
                int num2 = workingKey[index + 1];
                workingKey[index] = ((num1 & 16515072) << 6) | ((num1 & 4032) << 10) | (num2 & 16515072) >>> 10 | (num2 & 4032) >>> 6;
                workingKey[index + 1] = ((num1 & 258048) << 12) | ((num1 & 63) << 16) | (num2 & 258048) >>> 4 | (num2 & 63);
            }
            return workingKey;
        }

        internal static void DesFunc(
          int[] wKey,
          byte[] input,
          int inOff,
          byte[] outBytes,
          int outOff )
        {
            uint uint32_1 = Pack.BE_To_UInt32( input, inOff );
            uint uint32_2 = Pack.BE_To_UInt32( input, inOff + 4 );
            uint num1 = (uint)(((int)(uint32_1 >> 4) ^ (int)uint32_2) & 252645135);
            uint num2 = uint32_2 ^ num1;
            uint num3 = uint32_1 ^ (num1 << 4);
            uint num4 = (uint)(((int)(num3 >> 16) ^ (int)num2) & ushort.MaxValue);
            uint num5 = num2 ^ num4;
            uint num6 = num3 ^ (num4 << 16);
            uint num7 = (uint)(((int)(num5 >> 2) ^ (int)num6) & 858993459);
            uint num8 = num6 ^ num7;
            uint num9 = num5 ^ (num7 << 2);
            uint num10 = (uint)(((int)(num9 >> 8) ^ (int)num8) & 16711935);
            uint num11 = num8 ^ num10;
            uint num12 = num9 ^ (num10 << 8);
            uint num13 = (num12 << 1) | (num12 >> 31);
            uint num14 = (uint)(((int)num11 ^ (int)num13) & -1431655766);
            uint num15 = num11 ^ num14;
            uint num16 = num13 ^ num14;
            uint num17 = (num15 << 1) | (num15 >> 31);
            for (int index = 0; index < 8; ++index)
            {
                uint num18 = ((num16 << 28) | (num16 >> 4)) ^ (uint)wKey[index * 4];
                uint num19 = SP7[(int)(IntPtr)(num18 & 63U)] | SP5[(int)(IntPtr)((num18 >> 8) & 63U)] | SP3[(int)(IntPtr)((num18 >> 16) & 63U)] | SP1[(int)(IntPtr)((num18 >> 24) & 63U)];
                uint num20 = num16 ^ (uint)wKey[(index * 4) + 1];
                uint num21 = num19 | SP8[(int)(IntPtr)(num20 & 63U)] | SP6[(int)(IntPtr)((num20 >> 8) & 63U)] | SP4[(int)(IntPtr)((num20 >> 16) & 63U)] | SP2[(int)(IntPtr)((num20 >> 24) & 63U)];
                num17 ^= num21;
                uint num22 = ((num17 << 28) | (num17 >> 4)) ^ (uint)wKey[(index * 4) + 2];
                uint num23 = SP7[(int)(IntPtr)(num22 & 63U)] | SP5[(int)(IntPtr)((num22 >> 8) & 63U)] | SP3[(int)(IntPtr)((num22 >> 16) & 63U)] | SP1[(int)(IntPtr)((num22 >> 24) & 63U)];
                uint num24 = num17 ^ (uint)wKey[(index * 4) + 3];
                uint num25 = num23 | SP8[(int)(IntPtr)(num24 & 63U)] | SP6[(int)(IntPtr)((num24 >> 8) & 63U)] | SP4[(int)(IntPtr)((num24 >> 16) & 63U)] | SP2[(int)(IntPtr)((num24 >> 24) & 63U)];
                num16 ^= num25;
            }
            uint num26 = (num16 << 31) | (num16 >> 1);
            uint num27 = (uint)(((int)num17 ^ (int)num26) & -1431655766);
            uint num28 = num17 ^ num27;
            uint num29 = num26 ^ num27;
            uint num30 = (num28 << 31) | (num28 >> 1);
            uint num31 = (uint)(((int)(num30 >> 8) ^ (int)num29) & 16711935);
            uint num32 = num29 ^ num31;
            uint num33 = num30 ^ (num31 << 8);
            uint num34 = (uint)(((int)(num33 >> 2) ^ (int)num32) & 858993459);
            uint num35 = num32 ^ num34;
            uint num36 = num33 ^ (num34 << 2);
            uint num37 = (uint)(((int)(num35 >> 16) ^ (int)num36) & ushort.MaxValue);
            uint num38 = num36 ^ num37;
            uint num39 = num35 ^ (num37 << 16);
            uint num40 = (uint)(((int)(num39 >> 4) ^ (int)num38) & 252645135);
            uint n = num38 ^ num40;
            Pack.UInt32_To_BE( num39 ^ (num40 << 4), outBytes, outOff );
            Pack.UInt32_To_BE( n, outBytes, outOff + 4 );
        }
    }
}
