// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.BufferedAeadBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto
{
    public class BufferedAeadBlockCipher : BufferedCipherBase
    {
        private readonly IAeadBlockCipher cipher;

        public BufferedAeadBlockCipher( IAeadBlockCipher cipher ) => this.cipher = cipher != null ? cipher : throw new ArgumentNullException( nameof( cipher ) );

        public override string AlgorithmName => this.cipher.AlgorithmName;

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.cipher.Init( forEncryption, parameters );
        }

        public override int GetBlockSize() => this.cipher.GetBlockSize();

        public override int GetUpdateOutputSize( int length ) => this.cipher.GetUpdateOutputSize( length );

        public override int GetOutputSize( int length ) => this.cipher.GetOutputSize( length );

        public override int ProcessByte( byte input, byte[] output, int outOff ) => this.cipher.ProcessByte( input, output, outOff );

        public override byte[] ProcessByte( byte input )
        {
            int updateOutputSize = this.GetUpdateOutputSize( 1 );
            byte[] numArray = updateOutputSize > 0 ? new byte[updateOutputSize] : null;
            int length = this.ProcessByte( input, numArray, 0 );
            if (updateOutputSize > 0 && length < updateOutputSize)
            {
                byte[] destinationArray = new byte[length];
                Array.Copy( numArray, 0, destinationArray, 0, length );
                numArray = destinationArray;
            }
            return numArray;
        }

        public override byte[] ProcessBytes( byte[] input, int inOff, int length )
        {
            if (input == null)
                throw new ArgumentNullException( nameof( input ) );
            if (length < 1)
                return null;
            int updateOutputSize = this.GetUpdateOutputSize( length );
            byte[] numArray = updateOutputSize > 0 ? new byte[updateOutputSize] : null;
            int length1 = this.ProcessBytes( input, inOff, length, numArray, 0 );
            if (updateOutputSize > 0 && length1 < updateOutputSize)
            {
                byte[] destinationArray = new byte[length1];
                Array.Copy( numArray, 0, destinationArray, 0, length1 );
                numArray = destinationArray;
            }
            return numArray;
        }

        public override int ProcessBytes(
          byte[] input,
          int inOff,
          int length,
          byte[] output,
          int outOff )
        {
            return this.cipher.ProcessBytes( input, inOff, length, output, outOff );
        }

        public override byte[] DoFinal()
        {
            byte[] numArray = new byte[this.GetOutputSize( 0 )];
            int length = this.DoFinal( numArray, 0 );
            if (length < numArray.Length)
            {
                byte[] destinationArray = new byte[length];
                Array.Copy( numArray, 0, destinationArray, 0, length );
                numArray = destinationArray;
            }
            return numArray;
        }

        public override byte[] DoFinal( byte[] input, int inOff, int inLen )
        {
            if (input == null)
                throw new ArgumentNullException( nameof( input ) );
            byte[] numArray = new byte[this.GetOutputSize( inLen )];
            int outOff = inLen > 0 ? this.ProcessBytes( input, inOff, inLen, numArray, 0 ) : 0;
            int length = outOff + this.DoFinal( numArray, outOff );
            if (length < numArray.Length)
            {
                byte[] destinationArray = new byte[length];
                Array.Copy( numArray, 0, destinationArray, 0, length );
                numArray = destinationArray;
            }
            return numArray;
        }

        public override int DoFinal( byte[] output, int outOff ) => this.cipher.DoFinal( output, outOff );

        public override void Reset() => this.cipher.Reset();
    }
}
