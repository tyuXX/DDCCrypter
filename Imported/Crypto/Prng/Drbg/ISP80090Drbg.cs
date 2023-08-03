// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.Drbg.ISP80090Drbg
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Prng.Drbg
{
    public interface ISP80090Drbg
    {
        int BlockSize { get; }

        int Generate( byte[] output, byte[] additionalInput, bool predictionResistant );

        void Reseed( byte[] additionalInput );
    }
}
