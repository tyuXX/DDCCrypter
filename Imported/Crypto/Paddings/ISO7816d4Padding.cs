// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.ISO7816d4Padding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public class ISO7816d4Padding : IBlockCipherPadding
    {
        public void Init( SecureRandom random )
        {
        }

        public string PaddingName => "ISO7816-4";

        public int AddPadding( byte[] input, int inOff )
        {
            int num = input.Length - inOff;
            input[inOff] = 128;
            for (++inOff; inOff < input.Length; ++inOff)
                input[inOff] = 0;
            return num;
        }

        public int PadCount( byte[] input )
        {
            int index = input.Length - 1;
            while (index > 0 && input[index] == 0)
                --index;
            if (input[index] != 128)
                throw new InvalidCipherTextException( "pad block corrupted" );
            return input.Length - index;
        }
    }
}
