// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.ISigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto
{
    public interface ISigner
    {
        string AlgorithmName { get; }

        void Init( bool forSigning, ICipherParameters parameters );

        void Update( byte input );

        void BlockUpdate( byte[] input, int inOff, int length );

        byte[] GenerateSignature();

        bool VerifySignature( byte[] signature );

        void Reset();
    }
}
