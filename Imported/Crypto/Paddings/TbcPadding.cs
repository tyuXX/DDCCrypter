// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.TbcPadding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public class TbcPadding : IBlockCipherPadding
    {
        public string PaddingName => "TBC";

        public virtual void Init( SecureRandom random )
        {
        }

        public virtual int AddPadding( byte[] input, int inOff )
        {
            int num1 = input.Length - inOff;
            byte num2 = inOff <= 0 ? ((input[input.Length - 1] & 1) == 0 ? byte.MaxValue : (byte)0) : ((input[inOff - 1] & 1) == 0 ? byte.MaxValue : (byte)0);
            for (; inOff < input.Length; ++inOff)
                input[inOff] = num2;
            return num1;
        }

        public virtual int PadCount( byte[] input )
        {
            byte num1 = input[input.Length - 1];
            int num2 = input.Length - 1;
            while (num2 > 0 && input[num2 - 1] == num1)
                --num2;
            return input.Length - num2;
        }
    }
}
