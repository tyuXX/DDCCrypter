// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.BufferedAsymmetricBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto
{
    public class BufferedAsymmetricBlockCipher : BufferedCipherBase
    {
        private readonly IAsymmetricBlockCipher cipher;
        private byte[] buffer;
        private int bufOff;

        public BufferedAsymmetricBlockCipher( IAsymmetricBlockCipher cipher ) => this.cipher = cipher;

        internal int GetBufferPosition() => this.bufOff;

        public override string AlgorithmName => this.cipher.AlgorithmName;

        public override int GetBlockSize() => this.cipher.GetInputBlockSize();

        public override int GetOutputSize( int length ) => this.cipher.GetOutputBlockSize();

        public override int GetUpdateOutputSize( int length ) => 0;

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.Reset();
            this.cipher.Init( forEncryption, parameters );
            this.buffer = new byte[this.cipher.GetInputBlockSize() + (forEncryption ? 1 : 0)];
            this.bufOff = 0;
        }

        public override byte[] ProcessByte( byte input )
        {
            if (this.bufOff >= this.buffer.Length)
                throw new DataLengthException( "attempt to process message to long for cipher" );
            this.buffer[this.bufOff++] = input;
            return null;
        }

        public override byte[] ProcessBytes( byte[] input, int inOff, int length )
        {
            if (length < 1)
                return null;
            if (input == null)
                throw new ArgumentNullException( nameof( input ) );
            if (this.bufOff + length > this.buffer.Length)
                throw new DataLengthException( "attempt to process message to long for cipher" );
            Array.Copy( input, inOff, buffer, this.bufOff, length );
            this.bufOff += length;
            return null;
        }

        public override byte[] DoFinal()
        {
            byte[] numArray = this.bufOff > 0 ? this.cipher.ProcessBlock( this.buffer, 0, this.bufOff ) : EmptyBuffer;
            this.Reset();
            return numArray;
        }

        public override byte[] DoFinal( byte[] input, int inOff, int length )
        {
            this.ProcessBytes( input, inOff, length );
            return this.DoFinal();
        }

        public override void Reset()
        {
            if (this.buffer == null)
                return;
            Array.Clear( buffer, 0, this.buffer.Length );
            this.bufOff = 0;
        }
    }
}
