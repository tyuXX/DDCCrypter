// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.Iso9796d2Signer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class Iso9796d2Signer : ISignerWithRecovery, ISigner
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
        private int trailer;
        private int keyBits;
        private byte[] block;
        private byte[] mBuf;
        private int messageLength;
        private bool fullMessage;
        private byte[] recoveredMessage;
        private byte[] preSig;
        private byte[] preBlock;

        public byte[] GetRecoveredMessage() => this.recoveredMessage;

        public Iso9796d2Signer( IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit )
        {
            this.cipher = cipher;
            this.digest = digest;
            if (isImplicit)
                this.trailer = 188;
            else
                this.trailer = !IsoTrailers.NoTrailerAvailable( digest ) ? IsoTrailers.GetTrailer( digest ) : throw new ArgumentException( "no valid trailer", nameof( digest ) );
        }

        public Iso9796d2Signer( IAsymmetricBlockCipher cipher, IDigest digest )
          : this( cipher, digest, false )
        {
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "withISO9796-2S1";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            RsaKeyParameters parameters1 = (RsaKeyParameters)parameters;
            this.cipher.Init( forSigning, parameters1 );
            this.keyBits = parameters1.Modulus.BitLength;
            this.block = new byte[(this.keyBits + 7) / 8];
            this.mBuf = this.trailer != 188 ? new byte[this.block.Length - this.digest.GetDigestSize() - 3] : new byte[this.block.Length - this.digest.GetDigestSize() - 2];
            this.Reset();
        }

        private bool IsSameAs( byte[] a, byte[] b )
        {
            int length;
            if (this.messageLength > this.mBuf.Length)
            {
                if (this.mBuf.Length > b.Length)
                    return false;
                length = this.mBuf.Length;
            }
            else
            {
                if (this.messageLength != b.Length)
                    return false;
                length = b.Length;
            }
            bool flag = true;
            for (int index = 0; index != length; ++index)
            {
                if (a[index] != b[index])
                    flag = false;
            }
            return flag;
        }

        private void ClearBlock( byte[] block ) => Array.Clear( block, 0, block.Length );

        public virtual void UpdateWithRecoveredMessage( byte[] signature )
        {
            byte[] sourceArray = this.cipher.ProcessBlock( signature, 0, signature.Length );
            if (((sourceArray[0] & 192) ^ 64) != 0)
                throw new InvalidCipherTextException( "malformed signature" );
            if (((sourceArray[sourceArray.Length - 1] & 15) ^ 12) != 0)
                throw new InvalidCipherTextException( "malformed signature" );
            int num1;
            if (((sourceArray[sourceArray.Length - 1] & byte.MaxValue) ^ 188) == 0)
            {
                num1 = 1;
            }
            else
            {
                int num2 = ((sourceArray[sourceArray.Length - 2] & byte.MaxValue) << 8) | (sourceArray[sourceArray.Length - 1] & byte.MaxValue);
                if (IsoTrailers.NoTrailerAvailable( this.digest ))
                    throw new ArgumentException( "unrecognised hash in signature" );
                if (num2 != IsoTrailers.GetTrailer( this.digest ))
                    throw new InvalidOperationException( "signer initialised with wrong digest for trailer " + num2 );
                num1 = 2;
            }
            int index = 0;
            while (index != sourceArray.Length && ((sourceArray[index] & 15) ^ 10) != 0)
                ++index;
            int sourceIndex = index + 1;
            int num3 = sourceArray.Length - num1 - this.digest.GetDigestSize();
            if (num3 - sourceIndex <= 0)
                throw new InvalidCipherTextException( "malformed block" );
            if ((sourceArray[0] & 32) == 0)
            {
                this.fullMessage = true;
                this.recoveredMessage = new byte[num3 - sourceIndex];
                Array.Copy( sourceArray, sourceIndex, recoveredMessage, 0, this.recoveredMessage.Length );
            }
            else
            {
                this.fullMessage = false;
                this.recoveredMessage = new byte[num3 - sourceIndex];
                Array.Copy( sourceArray, sourceIndex, recoveredMessage, 0, this.recoveredMessage.Length );
            }
            this.preSig = signature;
            this.preBlock = sourceArray;
            this.digest.BlockUpdate( this.recoveredMessage, 0, this.recoveredMessage.Length );
            this.messageLength = this.recoveredMessage.Length;
            this.recoveredMessage.CopyTo( mBuf, 0 );
        }

        public virtual void Update( byte input )
        {
            this.digest.Update( input );
            if (this.messageLength < this.mBuf.Length)
                this.mBuf[this.messageLength] = input;
            ++this.messageLength;
        }

        public virtual void BlockUpdate( byte[] input, int inOff, int length )
        {
            for (; length > 0 && this.messageLength < this.mBuf.Length; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
            this.digest.BlockUpdate( input, inOff, length );
            this.messageLength += length;
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.messageLength = 0;
            this.ClearBlock( this.mBuf );
            if (this.recoveredMessage != null)
                this.ClearBlock( this.recoveredMessage );
            this.recoveredMessage = null;
            this.fullMessage = false;
            if (this.preSig == null)
                return;
            this.preSig = null;
            this.ClearBlock( this.preBlock );
            this.preBlock = null;
        }

        public virtual byte[] GenerateSignature()
        {
            int digestSize = this.digest.GetDigestSize();
            int num1;
            int outOff;
            if (this.trailer == 188)
            {
                num1 = 8;
                outOff = this.block.Length - digestSize - 1;
                this.digest.DoFinal( this.block, outOff );
                this.block[this.block.Length - 1] = 188;
            }
            else
            {
                num1 = 16;
                outOff = this.block.Length - digestSize - 2;
                this.digest.DoFinal( this.block, outOff );
                this.block[this.block.Length - 2] = (byte)(this.trailer >>> 8);
                this.block[this.block.Length - 1] = (byte)this.trailer;
            }
            int num2 = ((digestSize + this.messageLength) * 8) + num1 + 4 - this.keyBits;
            byte num3;
            int destinationIndex;
            if (num2 > 0)
            {
                int length = this.messageLength - ((num2 + 7) / 8);
                num3 = 96;
                destinationIndex = outOff - length;
                Array.Copy( mBuf, 0, block, destinationIndex, length );
            }
            else
            {
                num3 = 64;
                destinationIndex = outOff - this.messageLength;
                Array.Copy( mBuf, 0, block, destinationIndex, this.messageLength );
            }
            if (destinationIndex - 1 > 0)
            {
                for (int index = destinationIndex - 1; index != 0; --index)
                    this.block[index] = 187;
                byte[] block1;
                IntPtr index1;
                (block1 = this.block)[(int)(index1 = (IntPtr)(destinationIndex - 1))] = (byte)(block1[(int)index1] ^ 1U);
                this.block[0] = 11;
                byte[] block2;
                (block2 = this.block)[0] = (byte)(block2[0] | (uint)num3);
            }
            else
            {
                this.block[0] = 10;
                byte[] block;
                (block = this.block)[0] = (byte)(block[0] | (uint)num3);
            }
            byte[] signature = this.cipher.ProcessBlock( this.block, 0, this.block.Length );
            this.ClearBlock( this.mBuf );
            this.ClearBlock( this.block );
            return signature;
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            byte[] numArray1;
            if (this.preSig == null)
            {
                try
                {
                    numArray1 = this.cipher.ProcessBlock( signature, 0, signature.Length );
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                if (!Arrays.AreEqual( this.preSig, signature ))
                    throw new InvalidOperationException( "updateWithRecoveredMessage called on different signature" );
                numArray1 = this.preBlock;
                this.preSig = null;
                this.preBlock = null;
            }
            if (((numArray1[0] & 192) ^ 64) != 0 || ((numArray1[numArray1.Length - 1] & 15) ^ 12) != 0)
                return this.ReturnFalse( numArray1 );
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
            int index1 = 0;
            while (index1 != numArray1.Length && ((numArray1[index1] & 15) ^ 10) != 0)
                ++index1;
            int num3 = index1 + 1;
            byte[] output = new byte[this.digest.GetDigestSize()];
            int num4 = numArray1.Length - num1 - output.Length;
            if (num4 - num3 <= 0)
                return this.ReturnFalse( numArray1 );
            if ((numArray1[0] & 32) == 0)
            {
                this.fullMessage = true;
                if (this.messageLength > num4 - num3)
                    return this.ReturnFalse( numArray1 );
                this.digest.Reset();
                this.digest.BlockUpdate( numArray1, num3, num4 - num3 );
                this.digest.DoFinal( output, 0 );
                bool flag = true;
                for (int index2 = 0; index2 != output.Length; ++index2)
                {
                    byte[] numArray2;
                    IntPtr index3;
                    (numArray2 = numArray1)[(int)(index3 = (IntPtr)(num4 + index2))] = (byte)(numArray2[(int)index3] ^ (uint)output[index2]);
                    if (numArray1[num4 + index2] != 0)
                        flag = false;
                }
                if (!flag)
                    return this.ReturnFalse( numArray1 );
                this.recoveredMessage = new byte[num4 - num3];
                Array.Copy( numArray1, num3, recoveredMessage, 0, this.recoveredMessage.Length );
            }
            else
            {
                this.fullMessage = false;
                this.digest.DoFinal( output, 0 );
                bool flag = true;
                for (int index4 = 0; index4 != output.Length; ++index4)
                {
                    byte[] numArray3;
                    IntPtr index5;
                    (numArray3 = numArray1)[(int)(index5 = (IntPtr)(num4 + index4))] = (byte)(numArray3[(int)index5] ^ (uint)output[index4]);
                    if (numArray1[num4 + index4] != 0)
                        flag = false;
                }
                if (!flag)
                    return this.ReturnFalse( numArray1 );
                this.recoveredMessage = new byte[num4 - num3];
                Array.Copy( numArray1, num3, recoveredMessage, 0, this.recoveredMessage.Length );
            }
            if (this.messageLength != 0 && !this.IsSameAs( this.mBuf, this.recoveredMessage ))
                return this.ReturnFalse( numArray1 );
            this.ClearBlock( this.mBuf );
            this.ClearBlock( numArray1 );
            return true;
        }

        private bool ReturnFalse( byte[] block )
        {
            this.ClearBlock( this.mBuf );
            this.ClearBlock( block );
            return false;
        }

        public virtual bool HasFullMessage() => this.fullMessage;
    }
}
