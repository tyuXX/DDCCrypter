// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.ZeroBytePadding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public class ZeroBytePadding : IBlockCipherPadding
    {
        public string PaddingName => nameof( ZeroBytePadding );

        public void Init( SecureRandom random )
        {
        }

        public int AddPadding( byte[] input, int inOff )
        {
            int num = input.Length - inOff;
            for (; inOff < input.Length; ++inOff)
                input[inOff] = 0;
            return num;
        }

        public int PadCount( byte[] input )
        {
            int length = input.Length;
            while (length > 0 && input[length - 1] == 0)
                --length;
            return input.Length - length;
        }
    }
}
