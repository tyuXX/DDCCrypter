// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public interface TlsCipher
    {
        int GetPlaintextLimit( int ciphertextLimit );

        byte[] EncodePlaintext( long seqNo, byte type, byte[] plaintext, int offset, int len );

        byte[] DecodeCiphertext( long seqNo, byte type, byte[] ciphertext, int offset, int len );
    }
}
