// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.StreamBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto
{
    public class StreamBlockCipher : IStreamCipher
    {
        private readonly IBlockCipher cipher;
        private readonly byte[] oneByte = new byte[1];

        public StreamBlockCipher( IBlockCipher cipher )
        {
            if (cipher == null)
                throw new ArgumentNullException( nameof( cipher ) );
            this.cipher = cipher.GetBlockSize() == 1 ? cipher : throw new ArgumentException( "block cipher block size != 1.", nameof( cipher ) );
        }

        public void Init( bool forEncryption, ICipherParameters parameters ) => this.cipher.Init( forEncryption, parameters );

        public string AlgorithmName => this.cipher.AlgorithmName;

        public byte ReturnByte( byte input )
        {
            this.oneByte[0] = input;
            this.cipher.ProcessBlock( this.oneByte, 0, this.oneByte, 0 );
            return this.oneByte[0];
        }

        public void ProcessBytes( byte[] input, int inOff, int length, byte[] output, int outOff )
        {
            if (outOff + length > output.Length)
                throw new DataLengthException( "output buffer too small in ProcessBytes()" );
            for (int index = 0; index != length; ++index)
                this.cipher.ProcessBlock( input, inOff + index, output, outOff + index );
        }

        public void Reset() => this.cipher.Reset();
    }
}
