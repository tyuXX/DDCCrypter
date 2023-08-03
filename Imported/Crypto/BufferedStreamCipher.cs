// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.BufferedStreamCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto
{
    public class BufferedStreamCipher : BufferedCipherBase
    {
        private readonly IStreamCipher cipher;

        public BufferedStreamCipher( IStreamCipher cipher ) => this.cipher = cipher != null ? cipher : throw new ArgumentNullException( nameof( cipher ) );

        public override string AlgorithmName => this.cipher.AlgorithmName;

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.cipher.Init( forEncryption, parameters );
        }

        public override int GetBlockSize() => 0;

        public override int GetOutputSize( int inputLen ) => inputLen;

        public override int GetUpdateOutputSize( int inputLen ) => inputLen;

        public override byte[] ProcessByte( byte input ) => new byte[1]
        {
      this.cipher.ReturnByte(input)
        };

        public override int ProcessByte( byte input, byte[] output, int outOff )
        {
            if (outOff >= output.Length)
                throw new DataLengthException( "output buffer too short" );
            output[outOff] = this.cipher.ReturnByte( input );
            return 1;
        }

        public override byte[] ProcessBytes( byte[] input, int inOff, int length )
        {
            if (length < 1)
                return null;
            byte[] output = new byte[length];
            this.cipher.ProcessBytes( input, inOff, length, output, 0 );
            return output;
        }

        public override int ProcessBytes(
          byte[] input,
          int inOff,
          int length,
          byte[] output,
          int outOff )
        {
            if (length < 1)
                return 0;
            if (length > 0)
                this.cipher.ProcessBytes( input, inOff, length, output, outOff );
            return length;
        }

        public override byte[] DoFinal()
        {
            this.Reset();
            return EmptyBuffer;
        }

        public override byte[] DoFinal( byte[] input, int inOff, int length )
        {
            if (length < 1)
                return EmptyBuffer;
            byte[] numArray = this.ProcessBytes( input, inOff, length );
            this.Reset();
            return numArray;
        }

        public override void Reset() => this.cipher.Reset();
    }
}
