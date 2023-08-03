// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.PssSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class PssSigner : ISigner
    {
        public const byte TrailerImplicit = 188;
        private readonly IDigest contentDigest1;
        private readonly IDigest contentDigest2;
        private readonly IDigest mgfDigest;
        private readonly IAsymmetricBlockCipher cipher;
        private SecureRandom random;
        private int hLen;
        private int mgfhLen;
        private int sLen;
        private bool sSet;
        private int emBits;
        private byte[] salt;
        private byte[] mDash;
        private byte[] block;
        private byte trailer;

        public static PssSigner CreateRawSigner( IAsymmetricBlockCipher cipher, IDigest digest ) => new PssSigner( cipher, new NullDigest(), digest, digest, digest.GetDigestSize(), null, 188 );

        public static PssSigner CreateRawSigner(
          IAsymmetricBlockCipher cipher,
          IDigest contentDigest,
          IDigest mgfDigest,
          int saltLen,
          byte trailer )
        {
            return new PssSigner( cipher, new NullDigest(), contentDigest, mgfDigest, saltLen, null, trailer );
        }

        public PssSigner( IAsymmetricBlockCipher cipher, IDigest digest )
          : this( cipher, digest, digest.GetDigestSize() )
        {
        }

        public PssSigner( IAsymmetricBlockCipher cipher, IDigest digest, int saltLen )
          : this( cipher, digest, saltLen, 188 )
        {
        }

        public PssSigner( IAsymmetricBlockCipher cipher, IDigest digest, byte[] salt )
          : this( cipher, digest, digest, digest, salt.Length, salt, 188 )
        {
        }

        public PssSigner(
          IAsymmetricBlockCipher cipher,
          IDigest contentDigest,
          IDigest mgfDigest,
          int saltLen )
          : this( cipher, contentDigest, mgfDigest, saltLen, 188 )
        {
        }

        public PssSigner(
          IAsymmetricBlockCipher cipher,
          IDigest contentDigest,
          IDigest mgfDigest,
          byte[] salt )
          : this( cipher, contentDigest, contentDigest, mgfDigest, salt.Length, salt, 188 )
        {
        }

        public PssSigner( IAsymmetricBlockCipher cipher, IDigest digest, int saltLen, byte trailer )
          : this( cipher, digest, digest, saltLen, 188 )
        {
        }

        public PssSigner(
          IAsymmetricBlockCipher cipher,
          IDigest contentDigest,
          IDigest mgfDigest,
          int saltLen,
          byte trailer )
          : this( cipher, contentDigest, contentDigest, mgfDigest, saltLen, null, trailer )
        {
        }

        private PssSigner(
          IAsymmetricBlockCipher cipher,
          IDigest contentDigest1,
          IDigest contentDigest2,
          IDigest mgfDigest,
          int saltLen,
          byte[] salt,
          byte trailer )
        {
            this.cipher = cipher;
            this.contentDigest1 = contentDigest1;
            this.contentDigest2 = contentDigest2;
            this.mgfDigest = mgfDigest;
            this.hLen = contentDigest2.GetDigestSize();
            this.mgfhLen = mgfDigest.GetDigestSize();
            this.sLen = saltLen;
            this.sSet = salt != null;
            this.salt = !this.sSet ? new byte[saltLen] : salt;
            this.mDash = new byte[8 + saltLen + this.hLen];
            this.trailer = trailer;
        }

        public virtual string AlgorithmName => this.mgfDigest.AlgorithmName + "withRSAandMGF1";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                parameters = parametersWithRandom.Parameters;
                this.random = parametersWithRandom.Random;
            }
            else if (forSigning)
                this.random = new SecureRandom();
            this.cipher.Init( forSigning, parameters );
            this.emBits = (!(parameters is RsaBlindingParameters) ? (RsaKeyParameters)parameters : ((RsaBlindingParameters)parameters).PublicKey).Modulus.BitLength - 1;
            if (this.emBits < (8 * this.hLen) + (8 * this.sLen) + 9)
                throw new ArgumentException( "key too small for specified hash and salt lengths" );
            this.block = new byte[(this.emBits + 7) / 8];
        }

        private void ClearBlock( byte[] block ) => Array.Clear( block, 0, block.Length );

        public virtual void Update( byte input ) => this.contentDigest1.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int length ) => this.contentDigest1.BlockUpdate( input, inOff, length );

        public virtual void Reset() => this.contentDigest1.Reset();

        public virtual byte[] GenerateSignature()
        {
            this.contentDigest1.DoFinal( this.mDash, this.mDash.Length - this.hLen - this.sLen );
            if (this.sLen != 0)
            {
                if (!this.sSet)
                    this.random.NextBytes( this.salt );
                this.salt.CopyTo( mDash, this.mDash.Length - this.sLen );
            }
            byte[] numArray1 = new byte[this.hLen];
            this.contentDigest2.BlockUpdate( this.mDash, 0, this.mDash.Length );
            this.contentDigest2.DoFinal( numArray1, 0 );
            this.block[this.block.Length - this.sLen - 1 - this.hLen - 1] = 1;
            this.salt.CopyTo( block, this.block.Length - this.sLen - this.hLen - 1 );
            byte[] numArray2 = this.MaskGeneratorFunction1( numArray1, 0, numArray1.Length, this.block.Length - this.hLen - 1 );
            for (int index1 = 0; index1 != numArray2.Length; ++index1)
            {
                byte[] block;
                IntPtr index2;
                (block = this.block)[(int)(index2 = (IntPtr)index1)] = (byte)(block[(int)index2] ^ (uint)numArray2[index1]);
            }
            byte[] block1;
            (block1 = this.block)[0] = (byte)(block1[0] & (uint)(byte)(byte.MaxValue >> ((this.block.Length * 8) - this.emBits)));
            numArray1.CopyTo( block, this.block.Length - this.hLen - 1 );
            this.block[this.block.Length - 1] = this.trailer;
            byte[] signature = this.cipher.ProcessBlock( this.block, 0, this.block.Length );
            this.ClearBlock( this.block );
            return signature;
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            this.contentDigest1.DoFinal( this.mDash, this.mDash.Length - this.hLen - this.sLen );
            byte[] numArray1 = this.cipher.ProcessBlock( signature, 0, signature.Length );
            numArray1.CopyTo( block, this.block.Length - numArray1.Length );
            if (this.block[this.block.Length - 1] != trailer)
            {
                this.ClearBlock( this.block );
                return false;
            }
            byte[] numArray2 = this.MaskGeneratorFunction1( this.block, this.block.Length - this.hLen - 1, this.hLen, this.block.Length - this.hLen - 1 );
            for (int index1 = 0; index1 != numArray2.Length; ++index1)
            {
                byte[] block;
                IntPtr index2;
                (block = this.block)[(int)(index2 = (IntPtr)index1)] = (byte)(block[(int)index2] ^ (uint)numArray2[index1]);
            }
            byte[] block1;
            (block1 = this.block)[0] = (byte)(block1[0] & (uint)(byte)(byte.MaxValue >> ((this.block.Length * 8) - this.emBits)));
            for (int index = 0; index != this.block.Length - this.hLen - this.sLen - 2; ++index)
            {
                if (this.block[index] != 0)
                {
                    this.ClearBlock( this.block );
                    return false;
                }
            }
            if (this.block[this.block.Length - this.hLen - this.sLen - 2] != 1)
            {
                this.ClearBlock( this.block );
                return false;
            }
            if (this.sSet)
                Array.Copy( salt, 0, mDash, this.mDash.Length - this.sLen, this.sLen );
            else
                Array.Copy( block, this.block.Length - this.sLen - this.hLen - 1, mDash, this.mDash.Length - this.sLen, this.sLen );
            this.contentDigest2.BlockUpdate( this.mDash, 0, this.mDash.Length );
            this.contentDigest2.DoFinal( this.mDash, this.mDash.Length - this.hLen );
            int index3 = this.block.Length - this.hLen - 1;
            for (int index4 = this.mDash.Length - this.hLen; index4 != this.mDash.Length; ++index4)
            {
                if ((this.block[index3] ^ this.mDash[index4]) != 0)
                {
                    this.ClearBlock( this.mDash );
                    this.ClearBlock( this.block );
                    return false;
                }
                ++index3;
            }
            this.ClearBlock( this.mDash );
            this.ClearBlock( this.block );
            return true;
        }

        private void ItoOSP( int i, byte[] sp )
        {
            sp[0] = (byte)(i >>> 24);
            sp[1] = (byte)(i >>> 16);
            sp[2] = (byte)(i >>> 8);
            sp[3] = (byte)i;
        }

        private byte[] MaskGeneratorFunction1( byte[] Z, int zOff, int zLen, int length )
        {
            byte[] destinationArray = new byte[length];
            byte[] numArray1 = new byte[this.mgfhLen];
            byte[] numArray2 = new byte[4];
            int i = 0;
            this.mgfDigest.Reset();
            for (; i < length / this.mgfhLen; ++i)
            {
                this.ItoOSP( i, numArray2 );
                this.mgfDigest.BlockUpdate( Z, zOff, zLen );
                this.mgfDigest.BlockUpdate( numArray2, 0, numArray2.Length );
                this.mgfDigest.DoFinal( numArray1, 0 );
                numArray1.CopyTo( destinationArray, i * this.mgfhLen );
            }
            if (i * this.mgfhLen < length)
            {
                this.ItoOSP( i, numArray2 );
                this.mgfDigest.BlockUpdate( Z, zOff, zLen );
                this.mgfDigest.BlockUpdate( numArray2, 0, numArray2.Length );
                this.mgfDigest.DoFinal( numArray1, 0 );
                Array.Copy( numArray1, 0, destinationArray, i * this.mgfhLen, destinationArray.Length - (i * this.mgfhLen) );
            }
            return destinationArray;
        }
    }
}
