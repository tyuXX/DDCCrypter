// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.Rfc3211WrapEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class Rfc3211WrapEngine : IWrapper
    {
        private CbcBlockCipher engine;
        private ParametersWithIV param;
        private bool forWrapping;
        private SecureRandom rand;

        public Rfc3211WrapEngine( IBlockCipher engine ) => this.engine = new CbcBlockCipher( engine );

        public virtual void Init( bool forWrapping, ICipherParameters param )
        {
            this.forWrapping = forWrapping;
            if (param is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
                this.rand = parametersWithRandom.Random;
                this.param = (ParametersWithIV)parametersWithRandom.Parameters;
            }
            else
            {
                if (forWrapping)
                    this.rand = new SecureRandom();
                this.param = (ParametersWithIV)param;
            }
        }

        public virtual string AlgorithmName => this.engine.GetUnderlyingCipher().AlgorithmName + "/RFC3211Wrap";

        public virtual byte[] Wrap( byte[] inBytes, int inOff, int inLen )
        {
            if (!this.forWrapping)
                throw new InvalidOperationException( "not set for wrapping" );
            this.engine.Init( true, param );
            int blockSize = this.engine.GetBlockSize();
            byte[] numArray = inLen + 4 >= blockSize * 2 ? new byte[(inLen + 4) % blockSize == 0 ? inLen + 4 : (((inLen + 4) / blockSize) + 1) * blockSize] : new byte[blockSize * 2];
            numArray[0] = (byte)inLen;
            numArray[1] = (byte)~inBytes[inOff];
            numArray[2] = (byte)~inBytes[inOff + 1];
            numArray[3] = (byte)~inBytes[inOff + 2];
            Array.Copy( inBytes, inOff, numArray, 4, inLen );
            this.rand.NextBytes( numArray, inLen + 4, numArray.Length - inLen - 4 );
            for (int index = 0; index < numArray.Length; index += blockSize)
                this.engine.ProcessBlock( numArray, index, numArray, index );
            for (int index = 0; index < numArray.Length; index += blockSize)
                this.engine.ProcessBlock( numArray, index, numArray, index );
            return numArray;
        }

        public virtual byte[] Unwrap( byte[] inBytes, int inOff, int inLen )
        {
            if (this.forWrapping)
                throw new InvalidOperationException( "not set for unwrapping" );
            int blockSize = this.engine.GetBlockSize();
            byte[] numArray1 = inLen >= 2 * blockSize ? new byte[inLen] : throw new InvalidCipherTextException( "input too short" );
            byte[] numArray2 = new byte[blockSize];
            Array.Copy( inBytes, inOff, numArray1, 0, inLen );
            Array.Copy( inBytes, inOff, numArray2, 0, numArray2.Length );
            this.engine.Init( false, new ParametersWithIV( this.param.Parameters, numArray2 ) );
            for (int index = blockSize; index < numArray1.Length; index += blockSize)
                this.engine.ProcessBlock( numArray1, index, numArray1, index );
            Array.Copy( numArray1, numArray1.Length - numArray2.Length, numArray2, 0, numArray2.Length );
            this.engine.Init( false, new ParametersWithIV( this.param.Parameters, numArray2 ) );
            this.engine.ProcessBlock( numArray1, 0, numArray1, 0 );
            this.engine.Init( false, param );
            for (int index = 0; index < numArray1.Length; index += blockSize)
                this.engine.ProcessBlock( numArray1, index, numArray1, index );
            byte[] destinationArray = (numArray1[0] & byte.MaxValue) <= numArray1.Length - 4 ? new byte[numArray1[0] & byte.MaxValue] : throw new InvalidCipherTextException( "wrapped key corrupted" );
            Array.Copy( numArray1, 4, destinationArray, 0, numArray1[0] );
            int num1 = 0;
            for (int index = 0; index != 3; ++index)
            {
                byte num2 = (byte)~numArray1[1 + index];
                num1 |= num2 ^ destinationArray[index];
            }
            if (num1 != 0)
                throw new InvalidCipherTextException( "wrapped key fails checksum" );
            return destinationArray;
        }
    }
}
