// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.X923Padding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public class X923Padding : IBlockCipherPadding
    {
        private SecureRandom random;

        public void Init( SecureRandom random ) => this.random = random;

        public string PaddingName => "X9.23";

        public int AddPadding( byte[] input, int inOff )
        {
            byte num = (byte)(input.Length - inOff);
            for (; inOff < input.Length - 1; ++inOff)
                input[inOff] = this.random != null ? (byte)this.random.NextInt() : (byte)0;
            input[inOff] = num;
            return num;
        }

        public int PadCount( byte[] input )
        {
            int num = input[input.Length - 1] & byte.MaxValue;
            if (num > input.Length)
                throw new InvalidCipherTextException( "pad block corrupted" );
            return num;
        }
    }
}
