// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.IDsaKCalculator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
    public interface IDsaKCalculator
    {
        bool IsDeterministic { get; }

        void Init( BigInteger n, SecureRandom random );

        void Init( BigInteger n, BigInteger d, byte[] message );

        BigInteger NextK();
    }
}
