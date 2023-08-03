// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Encodings.OaepEncoding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Encodings
{
    public class OaepEncoding : IAsymmetricBlockCipher
    {
        private byte[] defHash;
        private IDigest hash;
        private IDigest mgf1Hash;
        private IAsymmetricBlockCipher engine;
        private SecureRandom random;
        private bool forEncryption;

        public OaepEncoding( IAsymmetricBlockCipher cipher )
          : this( cipher, new Sha1Digest(), null )
        {
        }

        public OaepEncoding( IAsymmetricBlockCipher cipher, IDigest hash )
          : this( cipher, hash, null )
        {
        }

        public OaepEncoding( IAsymmetricBlockCipher cipher, IDigest hash, byte[] encodingParams )
          : this( cipher, hash, hash, encodingParams )
        {
        }

        public OaepEncoding(
          IAsymmetricBlockCipher cipher,
          IDigest hash,
          IDigest mgf1Hash,
          byte[] encodingParams )
        {
            this.engine = cipher;
            this.hash = hash;
            this.mgf1Hash = mgf1Hash;
            this.defHash = new byte[hash.GetDigestSize()];
            if (encodingParams != null)
                hash.BlockUpdate( encodingParams, 0, encodingParams.Length );
            hash.DoFinal( this.defHash, 0 );
        }

        public IAsymmetricBlockCipher GetUnderlyingCipher() => this.engine;

        public string AlgorithmName => this.engine.AlgorithmName + "/OAEPPadding";

        public void Init( bool forEncryption, ICipherParameters param )
        {
            this.random = !(param is ParametersWithRandom) ? new SecureRandom() : ((ParametersWithRandom)param).Random;
            this.engine.Init( forEncryption, param );
            this.forEncryption = forEncryption;
        }

        public int GetInputBlockSize()
        {
            int inputBlockSize = this.engine.GetInputBlockSize();
            return this.forEncryption ? inputBlockSize - 1 - (2 * this.defHash.Length) : inputBlockSize;
        }

        public int GetOutputBlockSize()
        {
            int outputBlockSize = this.engine.GetOutputBlockSize();
            return this.forEncryption ? outputBlockSize : outputBlockSize - 1 - (2 * this.defHash.Length);
        }

        public byte[] ProcessBlock( byte[] inBytes, int inOff, int inLen ) => this.forEncryption ? this.EncodeBlock( inBytes, inOff, inLen ) : this.DecodeBlock( inBytes, inOff, inLen );

        private byte[] EncodeBlock( byte[] inBytes, int inOff, int inLen )
        {
            byte[] numArray1 = new byte[this.GetInputBlockSize() + 1 + (2 * this.defHash.Length)];
            Array.Copy( inBytes, inOff, numArray1, numArray1.Length - inLen, inLen );
            numArray1[numArray1.Length - inLen - 1] = 1;
            Array.Copy( defHash, 0, numArray1, this.defHash.Length, this.defHash.Length );
            byte[] nextBytes = SecureRandom.GetNextBytes( this.random, this.defHash.Length );
            byte[] numArray2 = this.maskGeneratorFunction1( nextBytes, 0, nextBytes.Length, numArray1.Length - this.defHash.Length );
            for (int length = this.defHash.Length; length != numArray1.Length; ++length)
            {
                byte[] numArray3;
                IntPtr index;
                (numArray3 = numArray1)[(int)(index = (IntPtr)length)] = (byte)(numArray3[(int)index] ^ (uint)numArray2[length - this.defHash.Length]);
            }
            Array.Copy( nextBytes, 0, numArray1, 0, this.defHash.Length );
            byte[] numArray4 = this.maskGeneratorFunction1( numArray1, this.defHash.Length, numArray1.Length - this.defHash.Length, this.defHash.Length );
            for (int index1 = 0; index1 != this.defHash.Length; ++index1)
            {
                byte[] numArray5;
                IntPtr index2;
                (numArray5 = numArray1)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray5[(int)index2] ^ (uint)numArray4[index1]);
            }
            return this.engine.ProcessBlock( numArray1, 0, numArray1.Length );
        }

        private byte[] DecodeBlock( byte[] inBytes, int inOff, int inLen )
        {
            byte[] sourceArray = this.engine.ProcessBlock( inBytes, inOff, inLen );
            byte[] numArray1;
            if (sourceArray.Length < this.engine.GetOutputBlockSize())
            {
                numArray1 = new byte[this.engine.GetOutputBlockSize()];
                Array.Copy( sourceArray, 0, numArray1, numArray1.Length - sourceArray.Length, sourceArray.Length );
            }
            else
                numArray1 = sourceArray;
            if (numArray1.Length < (2 * this.defHash.Length) + 1)
                throw new InvalidCipherTextException( "data too short" );
            byte[] numArray2 = this.maskGeneratorFunction1( numArray1, this.defHash.Length, numArray1.Length - this.defHash.Length, this.defHash.Length );
            for (int index1 = 0; index1 != this.defHash.Length; ++index1)
            {
                byte[] numArray3;
                IntPtr index2;
                (numArray3 = numArray1)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray3[(int)index2] ^ (uint)numArray2[index1]);
            }
            byte[] numArray4 = this.maskGeneratorFunction1( numArray1, 0, this.defHash.Length, numArray1.Length - this.defHash.Length );
            for (int length = this.defHash.Length; length != numArray1.Length; ++length)
            {
                byte[] numArray5;
                IntPtr index;
                (numArray5 = numArray1)[(int)(index = (IntPtr)length)] = (byte)(numArray5[(int)index] ^ (uint)numArray4[length - this.defHash.Length]);
            }
            int num = 0;
            for (int index = 0; index < this.defHash.Length; ++index)
                num |= (byte)(this.defHash[index] ^ (uint)numArray1[this.defHash.Length + index]);
            if (num != 0)
                throw new InvalidCipherTextException( "data hash wrong" );
            int index3 = 2 * this.defHash.Length;
            while (index3 != numArray1.Length && numArray1[index3] == 0)
                ++index3;
            if (index3 >= numArray1.Length - 1 || numArray1[index3] != 1)
                throw new InvalidCipherTextException( "data start wrong " + index3 );
            int sourceIndex = index3 + 1;
            byte[] destinationArray = new byte[numArray1.Length - sourceIndex];
            Array.Copy( numArray1, sourceIndex, destinationArray, 0, destinationArray.Length );
            return destinationArray;
        }

        private void ItoOSP( int i, byte[] sp )
        {
            sp[0] = (byte)(i >>> 24);
            sp[1] = (byte)(i >>> 16);
            sp[2] = (byte)(i >>> 8);
            sp[3] = (byte)i;
        }

        private byte[] maskGeneratorFunction1( byte[] Z, int zOff, int zLen, int length )
        {
            byte[] destinationArray = new byte[length];
            byte[] numArray1 = new byte[this.mgf1Hash.GetDigestSize()];
            byte[] numArray2 = new byte[4];
            int i = 0;
            this.hash.Reset();
            do
            {
                this.ItoOSP( i, numArray2 );
                this.mgf1Hash.BlockUpdate( Z, zOff, zLen );
                this.mgf1Hash.BlockUpdate( numArray2, 0, numArray2.Length );
                this.mgf1Hash.DoFinal( numArray1, 0 );
                Array.Copy( numArray1, 0, destinationArray, i * numArray1.Length, numArray1.Length );
            }
            while (++i < length / numArray1.Length);
            if (i * numArray1.Length < length)
            {
                this.ItoOSP( i, numArray2 );
                this.mgf1Hash.BlockUpdate( Z, zOff, zLen );
                this.mgf1Hash.BlockUpdate( numArray2, 0, numArray2.Length );
                this.mgf1Hash.DoFinal( numArray1, 0 );
                Array.Copy( numArray1, 0, destinationArray, i * numArray1.Length, destinationArray.Length - (i * numArray1.Length) );
            }
            return destinationArray;
        }
    }
}
