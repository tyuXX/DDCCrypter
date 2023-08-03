// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.Pkcs7Padding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public class Pkcs7Padding : IBlockCipherPadding
    {
        public void Init( SecureRandom random )
        {
        }

        public string PaddingName => "PKCS7";

        public int AddPadding( byte[] input, int inOff )
        {
            byte num = (byte)(input.Length - inOff);
            for (; inOff < input.Length; ++inOff)
                input[inOff] = num;
            return num;
        }

        public int PadCount( byte[] input )
        {
            byte num1 = input[input.Length - 1];
            int num2 = num1;
            if (num2 < 1 || num2 > input.Length)
                throw new InvalidCipherTextException( "pad block corrupted" );
            for (int index = 2; index <= num2; ++index)
            {
                if (input[input.Length - index] != num1)
                    throw new InvalidCipherTextException( "pad block corrupted" );
            }
            return num2;
        }
    }
}
