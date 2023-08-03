// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.DesEdeWrapEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class DesEdeWrapEngine : IWrapper
    {
        private CbcBlockCipher engine;
        private KeyParameter param;
        private ParametersWithIV paramPlusIV;
        private byte[] iv;
        private bool forWrapping;
        private static readonly byte[] IV2 = new byte[8]
        {
       74,
       221,
       162,
       44,
       121,
       232,
       33,
       5
        };
        private readonly IDigest sha1 = new Sha1Digest();
        private readonly byte[] digest = new byte[20];

        public virtual void Init( bool forWrapping, ICipherParameters parameters )
        {
            this.forWrapping = forWrapping;
            this.engine = new CbcBlockCipher( new DesEdeEngine() );
            SecureRandom secureRandom;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                parameters = parametersWithRandom.Parameters;
                secureRandom = parametersWithRandom.Random;
            }
            else
                secureRandom = new SecureRandom();
            switch (parameters)
            {
                case KeyParameter _:
                    this.param = (KeyParameter)parameters;
                    if (!this.forWrapping)
                        break;
                    this.iv = new byte[8];
                    secureRandom.NextBytes( this.iv );
                    this.paramPlusIV = new ParametersWithIV( param, this.iv );
                    break;
                case ParametersWithIV _:
                    if (!forWrapping)
                        throw new ArgumentException( "You should not supply an IV for unwrapping" );
                    this.paramPlusIV = (ParametersWithIV)parameters;
                    this.iv = this.paramPlusIV.GetIV();
                    this.param = (KeyParameter)this.paramPlusIV.Parameters;
                    if (this.iv.Length == 8)
                        break;
                    throw new ArgumentException( "IV is not 8 octets", nameof( parameters ) );
            }
        }

        public virtual string AlgorithmName => "DESede";

        public virtual byte[] Wrap( byte[] input, int inOff, int length )
        {
            if (!this.forWrapping)
                throw new InvalidOperationException( "Not initialized for wrapping" );
            byte[] numArray1 = new byte[length];
            Array.Copy( input, inOff, numArray1, 0, length );
            byte[] cmsKeyChecksum = this.CalculateCmsKeyChecksum( numArray1 );
            byte[] numArray2 = new byte[numArray1.Length + cmsKeyChecksum.Length];
            Array.Copy( numArray1, 0, numArray2, 0, numArray1.Length );
            Array.Copy( cmsKeyChecksum, 0, numArray2, numArray1.Length, cmsKeyChecksum.Length );
            int blockSize = this.engine.GetBlockSize();
            if (numArray2.Length % blockSize != 0)
                throw new InvalidOperationException( "Not multiple of block length" );
            this.engine.Init( true, paramPlusIV );
            byte[] numArray3 = new byte[numArray2.Length];
            for (int index = 0; index != numArray2.Length; index += blockSize)
                this.engine.ProcessBlock( numArray2, index, numArray3, index );
            byte[] numArray4 = new byte[this.iv.Length + numArray3.Length];
            Array.Copy( iv, 0, numArray4, 0, this.iv.Length );
            Array.Copy( numArray3, 0, numArray4, this.iv.Length, numArray3.Length );
            byte[] numArray5 = reverse( numArray4 );
            this.engine.Init( true, new ParametersWithIV( param, IV2 ) );
            for (int index = 0; index != numArray5.Length; index += blockSize)
                this.engine.ProcessBlock( numArray5, index, numArray5, index );
            return numArray5;
        }

        public virtual byte[] Unwrap( byte[] input, int inOff, int length )
        {
            if (this.forWrapping)
                throw new InvalidOperationException( "Not set for unwrapping" );
            if (input == null)
                throw new InvalidCipherTextException( "Null pointer as ciphertext" );
            int blockSize = this.engine.GetBlockSize();
            if (length % blockSize != 0)
                throw new InvalidCipherTextException( "Ciphertext not multiple of " + blockSize );
            this.engine.Init( false, new ParametersWithIV( param, IV2 ) );
            byte[] numArray1 = new byte[length];
            for (int outOff = 0; outOff != numArray1.Length; outOff += blockSize)
                this.engine.ProcessBlock( input, inOff + outOff, numArray1, outOff );
            byte[] sourceArray = reverse( numArray1 );
            this.iv = new byte[8];
            byte[] numArray2 = new byte[sourceArray.Length - 8];
            Array.Copy( sourceArray, 0, iv, 0, 8 );
            Array.Copy( sourceArray, 8, numArray2, 0, sourceArray.Length - 8 );
            this.paramPlusIV = new ParametersWithIV( param, this.iv );
            this.engine.Init( false, paramPlusIV );
            byte[] numArray3 = new byte[numArray2.Length];
            for (int index = 0; index != numArray3.Length; index += blockSize)
                this.engine.ProcessBlock( numArray2, index, numArray3, index );
            byte[] numArray4 = new byte[numArray3.Length - 8];
            byte[] numArray5 = new byte[8];
            Array.Copy( numArray3, 0, numArray4, 0, numArray3.Length - 8 );
            Array.Copy( numArray3, numArray3.Length - 8, numArray5, 0, 8 );
            return this.CheckCmsKeyChecksum( numArray4, numArray5 ) ? numArray4 : throw new InvalidCipherTextException( "Checksum inside ciphertext is corrupted" );
        }

        private byte[] CalculateCmsKeyChecksum( byte[] key )
        {
            this.sha1.BlockUpdate( key, 0, key.Length );
            this.sha1.DoFinal( this.digest, 0 );
            byte[] destinationArray = new byte[8];
            Array.Copy( digest, 0, destinationArray, 0, 8 );
            return destinationArray;
        }

        private bool CheckCmsKeyChecksum( byte[] key, byte[] checksum ) => Arrays.ConstantTimeAreEqual( this.CalculateCmsKeyChecksum( key ), checksum );

        private static byte[] reverse( byte[] bs )
        {
            byte[] numArray = new byte[bs.Length];
            for (int index = 0; index < bs.Length; ++index)
                numArray[index] = bs[bs.Length - (index + 1)];
            return numArray;
        }
    }
}
