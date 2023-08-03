// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Paddings.IBlockCipherPadding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Paddings
{
    public interface IBlockCipherPadding
    {
        void Init( SecureRandom random );

        string PaddingName { get; }

        int AddPadding( byte[] input, int inOff );

        int PadCount( byte[] input );
    }
}
