// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.Iso9796d2PssSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class Iso9796d2PssSigner : ISignerWithRecovery, ISigner
    {
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerImplicit = 188;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerRipeMD160 = 12748;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerRipeMD128 = 13004;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerSha1 = 13260;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerSha256 = 13516;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerSha512 = 13772;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerSha384 = 14028;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TrailerWhirlpool = 14284;
        private IDigest digest;
        private IAsymmetricBlockCipher cipher;
        private SecureRandom random;
        private byte[] standardSalt;
        private int hLen;
        private int trailer;
        private int keyBits;
        private byte[] block;
        private byte[] mBuf;
        private int messageLength;
        private readonly int saltLength;
        private bool fullMessage;
        private byte[] recoveredMessage;
        private byte[] preSig;
        private byte[] preBlock;
        private int preMStart;
        private int preTLength;

        public byte[] GetRecoveredMessage() => this.recoveredMessage;

        public Iso9796d2PssSigner(
          IAsymmetricBlockCipher cipher,
          IDigest digest,
          int saltLength,
          bool isImplicit )
        {
            this.cipher = cipher;
            this.digest = digest;
            this.hLen = digest.GetDigestSize();
            this.saltLength = saltLength;
            if (isImplicit)
                this.trailer = 188;
            else
                this.trailer = !IsoTrailers.NoTrailerAvailable( digest ) ? IsoTrailers.GetTrailer( digest ) : throw new ArgumentException( "no valid trailer", nameof( digest ) );
        }

        public Iso9796d2PssSigner( IAsymmetricBlockCipher cipher, IDigest digest, int saltLength )
          : this( cipher, digest, saltLength, false )
        {
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "withISO9796-2S2";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            RsaKeyParameters parameters1;
            switch (parameters)
            {
                case ParametersWithRandom _:
                    ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                    parameters1 = (RsaKeyParameters)parametersWithRandom.Parameters;
                    if (forSigning)
                    {
                        this.random = parametersWithRandom.Random;
                        break;
                    }
                    break;
                case ParametersWithSalt _:
                    if (!forSigning)
                        throw new ArgumentException( "ParametersWithSalt only valid for signing", nameof( parameters ) );
                    ParametersWithSalt parametersWithSalt = (ParametersWithSalt)parameters;
                    parameters1 = (RsaKeyParameters)parametersWithSalt.Parameters;
                    this.standardSalt = parametersWithSalt.GetSalt();
                    if (this.standardSalt.Length != this.saltLength)
                        throw new ArgumentException( "Fixed salt is of wrong length" );
                    break;
                default:
                    parameters1 = (RsaKeyParameters)parameters;
                    if (forSigning)
                    {
                        this.random = new SecureRandom();
                        break;
                    }
                    break;
            }
            this.cipher.Init( forSigning, parameters1 );
            this.keyBits = parameters1.Modulus.BitLength;
            this.block = new byte[(this.keyBits + 7) / 8];
            this.mBuf = this.trailer != 188 ? new byte[this.block.Length - this.digest.GetDigestSize() - this.saltLength - 1 - 2] : new byte[this.block.Length - this.digest.GetDigestSize() - this.saltLength - 1 - 1];
            this.Reset();
        }

        private bool IsSameAs( byte[] a, byte[] b )
        {
            if (this.messageLength != b.Length)
                return false;
            bool flag = true;
            for (int index = 0; index != b.Length; ++index)
            {
                if (a[index] != b[index])
                    flag = false;
            }
            return flag;
        }

        private void ClearBlock( byte[] block ) => Array.Clear( block, 0, block.Length );

        public virtual void UpdateWithRecoveredMessage( byte[] signature )
        {
            byte[] numArray1 = this.cipher.ProcessBlock( signature, 0, signature.Length );
            if (numArray1.Length < (this.keyBits + 7) / 8)
            {
                byte[] destinationArray = new byte[(this.keyBits + 7) / 8];
                Array.Copy( numArray1, 0, destinationArray, destinationArray.Length - numArray1.Length, numArray1.Length );
                this.ClearBlock( numArray1 );
                numArray1 = destinationArray;
            }
            int num1;
            if (((numArray1[numArray1.Length - 1] & byte.MaxValue) ^ 188) == 0)
            {
                num1 = 1;
            }
            else
            {
                int num2 = ((numArray1[numArray1.Length - 2] & byte.MaxValue) << 8) | (numArray1[numArray1.Length - 1] & byte.MaxValue);
                if (IsoTrailers.NoTrailerAvailable( this.digest ))
                    throw new ArgumentException( "unrecognised hash in signature" );
                if (num2 != IsoTrailers.GetTrailer( this.digest ))
                    throw new InvalidOperationException( "signer initialised with wrong digest for trailer " + num2 );
                num1 = 2;
            }
            this.digest.DoFinal( new byte[this.hLen], 0 );
            byte[] numArray2 = this.MaskGeneratorFunction1( numArray1, numArray1.Length - this.hLen - num1, this.hLen, numArray1.Length - this.hLen - num1 );
            for (int index1 = 0; index1 != numArray2.Length; ++index1)
            {
                byte[] numArray3;
                IntPtr index2;
                (numArray3 = numArray1)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray3[(int)index2] ^ (uint)numArray2[index1]);
            }
            byte[] numArray4;
            (numArray4 = numArray1)[0] = (byte)(numArray4[0] & (uint)sbyte.MaxValue);
            int sourceIndex = 0;
            do
                ;
            while (sourceIndex < numArray1.Length && numArray1[sourceIndex++] != 1);
            if (sourceIndex >= numArray1.Length)
                this.ClearBlock( numArray1 );
            this.fullMessage = sourceIndex > 1;
            this.recoveredMessage = new byte[numArray2.Length - sourceIndex - this.saltLength];
            Array.Copy( numArray1, sourceIndex, recoveredMessage, 0, this.recoveredMessage.Length );
            this.recoveredMessage.CopyTo( mBuf, 0 );
            this.preSig = signature;
            this.preBlock = numArray1;
            this.preMStart = sourceIndex;
            this.preTLength = num1;
        }

        public virtual void Update( byte input )
        {
            if (this.preSig == null && this.messageLength < this.mBuf.Length)
                this.mBuf[this.messageLength++] = input;
            else
                this.digest.Update( input );
        }

        public virtual void BlockUpdate( byte[] input, int inOff, int length )
        {
            if (this.preSig == null)
            {
                for (; length > 0 && this.messageLength < this.mBuf.Length; --length)
                {
                    this.Update( input[inOff] );
                    ++inOff;
                }
            }
            if (length <= 0)
                return;
            this.digest.BlockUpdate( input, inOff, length );
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.messageLength = 0;
            if (this.mBuf != null)
                this.ClearBlock( this.mBuf );
            if (this.recoveredMessage != null)
            {
                this.ClearBlock( this.recoveredMessage );
                this.recoveredMessage = null;
            }
            this.fullMessage = false;
            if (this.preSig == null)
                return;
            this.preSig = null;
            this.ClearBlock( this.preBlock );
            this.preBlock = null;
        }

        public virtual byte[] GenerateSignature()
        {
            byte[] numArray1 = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray1, 0 );
            byte[] numArray2 = new byte[8];
            this.LtoOSP( this.messageLength * 8, numArray2 );
            this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
            this.digest.BlockUpdate( this.mBuf, 0, this.messageLength );
            this.digest.BlockUpdate( numArray1, 0, numArray1.Length );
            byte[] numArray3;
            if (this.standardSalt != null)
            {
                numArray3 = this.standardSalt;
            }
            else
            {
                numArray3 = new byte[this.saltLength];
                this.random.NextBytes( numArray3 );
            }
            this.digest.BlockUpdate( numArray3, 0, numArray3.Length );
            byte[] numArray4 = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray4, 0 );
            int num = 2;
            if (this.trailer == 188)
                num = 1;
            int index1 = this.block.Length - this.messageLength - numArray3.Length - this.hLen - num - 1;
            this.block[index1] = 1;
            Array.Copy( mBuf, 0, block, index1 + 1, this.messageLength );
            Array.Copy( numArray3, 0, block, index1 + 1 + this.messageLength, numArray3.Length );
            byte[] numArray5 = this.MaskGeneratorFunction1( numArray4, 0, numArray4.Length, this.block.Length - this.hLen - num );
            for (int index2 = 0; index2 != numArray5.Length; ++index2)
            {
                byte[] block;
                IntPtr index3;
                (block = this.block)[(int)(index3 = (IntPtr)index2)] = (byte)(block[(int)index3] ^ (uint)numArray5[index2]);
            }
            Array.Copy( numArray4, 0, block, this.block.Length - this.hLen - num, this.hLen );
            if (this.trailer == 188)
            {
                this.block[this.block.Length - 1] = 188;
            }
            else
            {
                this.block[this.block.Length - 2] = (byte)(this.trailer >>> 8);
                this.block[this.block.Length - 1] = (byte)this.trailer;
            }
            byte[] block1;
            (block1 = this.block)[0] = (byte)(block1[0] & (uint)sbyte.MaxValue);
            byte[] signature = this.cipher.ProcessBlock( this.block, 0, this.block.Length );
            this.ClearBlock( this.mBuf );
            this.ClearBlock( this.block );
            this.messageLength = 0;
            return signature;
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            byte[] numArray1 = new byte[this.hLen];
            this.digest.DoFinal( numArray1, 0 );
            if (this.preSig == null)
            {
                try
                {
                    this.UpdateWithRecoveredMessage( signature );
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else if (!Arrays.AreEqual( this.preSig, signature ))
                throw new InvalidOperationException( "UpdateWithRecoveredMessage called on different signature" );
            byte[] preBlock = this.preBlock;
            int preMstart = this.preMStart;
            int preTlength = this.preTLength;
            this.preSig = null;
            this.preBlock = null;
            byte[] numArray2 = new byte[8];
            this.LtoOSP( this.recoveredMessage.Length * 8, numArray2 );
            this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
            if (this.recoveredMessage.Length != 0)
                this.digest.BlockUpdate( this.recoveredMessage, 0, this.recoveredMessage.Length );
            this.digest.BlockUpdate( numArray1, 0, numArray1.Length );
            if (this.standardSalt != null)
                this.digest.BlockUpdate( this.standardSalt, 0, this.standardSalt.Length );
            else
                this.digest.BlockUpdate( preBlock, preMstart + this.recoveredMessage.Length, this.saltLength );
            byte[] numArray3 = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray3, 0 );
            int num = preBlock.Length - preTlength - numArray3.Length;
            bool flag = true;
            for (int index = 0; index != numArray3.Length; ++index)
            {
                if (numArray3[index] != preBlock[num + index])
                    flag = false;
            }
            this.ClearBlock( preBlock );
            this.ClearBlock( numArray3 );
            if (!flag)
            {
                this.fullMessage = false;
                this.ClearBlock( this.recoveredMessage );
                return false;
            }
            if (this.messageLength != 0)
            {
                if (!this.IsSameAs( this.mBuf, this.recoveredMessage ))
                {
                    this.ClearBlock( this.mBuf );
                    return false;
                }
                this.messageLength = 0;
            }
            this.ClearBlock( this.mBuf );
            return true;
        }

        public virtual bool HasFullMessage() => this.fullMessage;

        private void ItoOSP( int i, byte[] sp )
        {
            sp[0] = (byte)(i >>> 24);
            sp[1] = (byte)(i >>> 16);
            sp[2] = (byte)(i >>> 8);
            sp[3] = (byte)i;
        }

        private void LtoOSP( long l, byte[] sp )
        {
            sp[0] = (byte)(l >>> 56);
            sp[1] = (byte)(l >>> 48);
            sp[2] = (byte)(l >>> 40);
            sp[3] = (byte)(l >>> 32);
            sp[4] = (byte)(l >>> 24);
            sp[5] = (byte)(l >>> 16);
            sp[6] = (byte)(l >>> 8);
            sp[7] = (byte)l;
        }

        private byte[] MaskGeneratorFunction1( byte[] Z, int zOff, int zLen, int length )
        {
            byte[] destinationArray = new byte[length];
            byte[] numArray1 = new byte[this.hLen];
            byte[] numArray2 = new byte[4];
            int i = 0;
            this.digest.Reset();
            do
            {
                this.ItoOSP( i, numArray2 );
                this.digest.BlockUpdate( Z, zOff, zLen );
                this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
                this.digest.DoFinal( numArray1, 0 );
                Array.Copy( numArray1, 0, destinationArray, i * this.hLen, this.hLen );
            }
            while (++i < length / this.hLen);
            if (i * this.hLen < length)
            {
                this.ItoOSP( i, numArray2 );
                this.digest.BlockUpdate( Z, zOff, zLen );
                this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
                this.digest.DoFinal( numArray1, 0 );
                Array.Copy( numArray1, 0, destinationArray, i * this.hLen, destinationArray.Length - (i * this.hLen) );
            }
            return destinationArray;
        }
    }
}
