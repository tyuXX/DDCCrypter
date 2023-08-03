// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RC2WrapEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RC2WrapEngine : IWrapper
    {
        private CbcBlockCipher engine;
        private ICipherParameters parameters;
        private ParametersWithIV paramPlusIV;
        private byte[] iv;
        private bool forWrapping;
        private SecureRandom sr;
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
        private IDigest sha1 = new Sha1Digest();
        private byte[] digest = new byte[20];

        public virtual void Init( bool forWrapping, ICipherParameters parameters )
        {
            this.forWrapping = forWrapping;
            this.engine = new CbcBlockCipher( new RC2Engine() );
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                this.sr = parametersWithRandom.Random;
                parameters = parametersWithRandom.Parameters;
            }
            else
                this.sr = new SecureRandom();
            if (parameters is ParametersWithIV)
            {
                if (!forWrapping)
                    throw new ArgumentException( "You should not supply an IV for unwrapping" );
                this.paramPlusIV = (ParametersWithIV)parameters;
                this.iv = this.paramPlusIV.GetIV();
                this.parameters = this.paramPlusIV.Parameters;
                if (this.iv.Length != 8)
                    throw new ArgumentException( "IV is not 8 octets" );
            }
            else
            {
                this.parameters = parameters;
                if (!this.forWrapping)
                    return;
                this.iv = new byte[8];
                this.sr.NextBytes( this.iv );
                this.paramPlusIV = new ParametersWithIV( this.parameters, this.iv );
            }
        }

        public virtual string AlgorithmName => "RC2";

        public virtual byte[] Wrap( byte[] input, int inOff, int length )
        {
            if (!this.forWrapping)
                throw new InvalidOperationException( "Not initialized for wrapping" );
            int length1 = length + 1;
            if (length1 % 8 != 0)
                length1 += 8 - (length1 % 8);
            byte[] numArray1 = new byte[length1];
            numArray1[0] = (byte)length;
            Array.Copy( input, inOff, numArray1, 1, length );
            byte[] numArray2 = new byte[numArray1.Length - length - 1];
            if (numArray2.Length > 0)
            {
                this.sr.NextBytes( numArray2 );
                Array.Copy( numArray2, 0, numArray1, length + 1, numArray2.Length );
            }
            byte[] cmsKeyChecksum = this.CalculateCmsKeyChecksum( numArray1 );
            byte[] numArray3 = new byte[numArray1.Length + cmsKeyChecksum.Length];
            Array.Copy( numArray1, 0, numArray3, 0, numArray1.Length );
            Array.Copy( cmsKeyChecksum, 0, numArray3, numArray1.Length, cmsKeyChecksum.Length );
            byte[] numArray4 = new byte[numArray3.Length];
            Array.Copy( numArray3, 0, numArray4, 0, numArray3.Length );
            int num1 = numArray3.Length / this.engine.GetBlockSize();
            if (numArray3.Length % this.engine.GetBlockSize() != 0)
                throw new InvalidOperationException( "Not multiple of block length" );
            this.engine.Init( true, paramPlusIV );
            for (int index = 0; index < num1; ++index)
            {
                int num2 = index * this.engine.GetBlockSize();
                this.engine.ProcessBlock( numArray4, num2, numArray4, num2 );
            }
            byte[] destinationArray = new byte[this.iv.Length + numArray4.Length];
            Array.Copy( iv, 0, destinationArray, 0, this.iv.Length );
            Array.Copy( numArray4, 0, destinationArray, this.iv.Length, numArray4.Length );
            byte[] numArray5 = new byte[destinationArray.Length];
            for (int index = 0; index < destinationArray.Length; ++index)
                numArray5[index] = destinationArray[destinationArray.Length - (index + 1)];
            this.engine.Init( true, new ParametersWithIV( this.parameters, IV2 ) );
            for (int index = 0; index < num1 + 1; ++index)
            {
                int num3 = index * this.engine.GetBlockSize();
                this.engine.ProcessBlock( numArray5, num3, numArray5, num3 );
            }
            return numArray5;
        }

        public virtual byte[] Unwrap( byte[] input, int inOff, int length )
        {
            if (this.forWrapping)
                throw new InvalidOperationException( "Not set for unwrapping" );
            if (input == null)
                throw new InvalidCipherTextException( "Null pointer as ciphertext" );
            if (length % this.engine.GetBlockSize() != 0)
                throw new InvalidCipherTextException( "Ciphertext not multiple of " + this.engine.GetBlockSize() );
            this.engine.Init( false, new ParametersWithIV( this.parameters, IV2 ) );
            byte[] numArray1 = new byte[length];
            Array.Copy( input, inOff, numArray1, 0, length );
            for (int index = 0; index < numArray1.Length / this.engine.GetBlockSize(); ++index)
            {
                int num = index * this.engine.GetBlockSize();
                this.engine.ProcessBlock( numArray1, num, numArray1, num );
            }
            byte[] sourceArray = new byte[numArray1.Length];
            for (int index = 0; index < numArray1.Length; ++index)
                sourceArray[index] = numArray1[numArray1.Length - (index + 1)];
            this.iv = new byte[8];
            byte[] numArray2 = new byte[sourceArray.Length - 8];
            Array.Copy( sourceArray, 0, iv, 0, 8 );
            Array.Copy( sourceArray, 8, numArray2, 0, sourceArray.Length - 8 );
            this.paramPlusIV = new ParametersWithIV( this.parameters, this.iv );
            this.engine.Init( false, paramPlusIV );
            byte[] numArray3 = new byte[numArray2.Length];
            Array.Copy( numArray2, 0, numArray3, 0, numArray2.Length );
            for (int index = 0; index < numArray3.Length / this.engine.GetBlockSize(); ++index)
            {
                int num = index * this.engine.GetBlockSize();
                this.engine.ProcessBlock( numArray3, num, numArray3, num );
            }
            byte[] numArray4 = new byte[numArray3.Length - 8];
            byte[] numArray5 = new byte[8];
            Array.Copy( numArray3, 0, numArray4, 0, numArray3.Length - 8 );
            Array.Copy( numArray3, numArray3.Length - 8, numArray5, 0, 8 );
            if (!this.CheckCmsKeyChecksum( numArray4, numArray5 ))
                throw new InvalidCipherTextException( "Checksum inside ciphertext is corrupted" );
            byte[] destinationArray = numArray4.Length - ((numArray4[0] & byte.MaxValue) + 1) <= 7 ? new byte[(numArray4[0])] : throw new InvalidCipherTextException( "too many pad bytes (" + (numArray4.Length - ((numArray4[0] & byte.MaxValue) + 1)) + ")" );
            Array.Copy( numArray4, 1, destinationArray, 0, destinationArray.Length );
            return destinationArray;
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
    }
}
