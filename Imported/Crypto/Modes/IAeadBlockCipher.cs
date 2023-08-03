// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.IAeadBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Modes
{
    public interface IAeadBlockCipher
    {
        string AlgorithmName { get; }

        IBlockCipher GetUnderlyingCipher();

        void Init( bool forEncryption, ICipherParameters parameters );

        int GetBlockSize();

        void ProcessAadByte( byte input );

        void ProcessAadBytes( byte[] inBytes, int inOff, int len );

        int ProcessByte( byte input, byte[] outBytes, int outOff );

        int ProcessBytes( byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff );

        int DoFinal( byte[] outBytes, int outOff );

        byte[] GetMac();

        int GetUpdateOutputSize( int len );

        int GetOutputSize( int len );

        void Reset();
    }
}
