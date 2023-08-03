// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Encoders.HexTranslator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Utilities.Encoders
{
    public class HexTranslator : ITranslator
    {
        private static readonly byte[] hexTable = new byte[16]
        {
       48,
       49,
       50,
       51,
       52,
       53,
       54,
       55,
       56,
       57,
       97,
       98,
       99,
       100,
       101,
       102
        };

        public int GetEncodedBlockSize() => 2;

        public int Encode( byte[] input, int inOff, int length, byte[] outBytes, int outOff )
        {
            int num1 = 0;
            int num2 = 0;
            while (num1 < length)
            {
                outBytes[outOff + num2] = hexTable[(input[inOff] >> 4) & 15];
                outBytes[outOff + num2 + 1] = hexTable[input[inOff] & 15];
                ++inOff;
                ++num1;
                num2 += 2;
            }
            return length * 2;
        }

        public int GetDecodedBlockSize() => 1;

        public int Decode( byte[] input, int inOff, int length, byte[] outBytes, int outOff )
        {
            int num1 = length / 2;
            for (int index1 = 0; index1 < num1; ++index1)
            {
                byte num2 = input[inOff + (index1 * 2)];
                byte num3 = input[inOff + (index1 * 2) + 1];
                outBytes[outOff] = num2 >= 97 ? (byte)((num2 - 97 + 10) << 4) : (byte)((num2 - 48) << 4);
                if (num3 < 97)
                {
                    byte[] numArray;
                    IntPtr index2;
                    (numArray = outBytes)[(int)(index2 = (IntPtr)outOff)] = (byte)(numArray[(int)index2] + (uint)(byte)(num3 - 48U));
                }
                else
                {
                    byte[] numArray;
                    IntPtr index3;
                    (numArray = outBytes)[(int)(index3 = (IntPtr)outOff)] = (byte)(numArray[(int)index3] + (uint)(byte)(num3 - 97 + 10));
                }
                ++outOff;
            }
            return num1;
        }
    }
}
