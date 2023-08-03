// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.CcmBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class CcmBlockCipher : IAeadBlockCipher
    {
        private static readonly int BlockSize = 16;
        private readonly IBlockCipher cipher;
        private readonly byte[] macBlock;
        private bool forEncryption;
        private byte[] nonce;
        private byte[] initialAssociatedText;
        private int macSize;
        private ICipherParameters keyParam;
        private readonly MemoryStream associatedText = new();
        private readonly MemoryStream data = new();

        public CcmBlockCipher( IBlockCipher cipher )
        {
            this.cipher = cipher;
            this.macBlock = new byte[BlockSize];
            if (cipher.GetBlockSize() != BlockSize)
                throw new ArgumentException( "cipher required with a block size of " + BlockSize + "." );
        }

        public virtual IBlockCipher GetUnderlyingCipher() => this.cipher;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.forEncryption = forEncryption;
            ICipherParameters cipherParameters;
            switch (parameters)
            {
                case AeadParameters _:
                    AeadParameters aeadParameters = (AeadParameters)parameters;
                    this.nonce = aeadParameters.GetNonce();
                    this.initialAssociatedText = aeadParameters.GetAssociatedText();
                    this.macSize = aeadParameters.MacSize / 8;
                    cipherParameters = aeadParameters.Key;
                    break;
                case ParametersWithIV _:
                    ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                    this.nonce = parametersWithIv.GetIV();
                    this.initialAssociatedText = null;
                    this.macSize = this.macBlock.Length / 2;
                    cipherParameters = parametersWithIv.Parameters;
                    break;
                default:
                    throw new ArgumentException( "invalid parameters passed to CCM" );
            }
            if (cipherParameters != null)
                this.keyParam = cipherParameters;
            if (this.nonce == null || this.nonce.Length < 7 || this.nonce.Length > 13)
                throw new ArgumentException( "nonce must have length from 7 to 13 octets" );
            this.Reset();
        }

        public virtual string AlgorithmName => this.cipher.AlgorithmName + "/CCM";

        public virtual int GetBlockSize() => this.cipher.GetBlockSize();

        public virtual void ProcessAadByte( byte input ) => this.associatedText.WriteByte( input );

        public virtual void ProcessAadBytes( byte[] inBytes, int inOff, int len ) => this.associatedText.Write( inBytes, inOff, len );

        public virtual int ProcessByte( byte input, byte[] outBytes, int outOff )
        {
            this.data.WriteByte( input );
            return 0;
        }

        public virtual int ProcessBytes(
          byte[] inBytes,
          int inOff,
          int inLen,
          byte[] outBytes,
          int outOff )
        {
            Check.DataLength( inBytes, inOff, inLen, "Input buffer too short" );
            this.data.Write( inBytes, inOff, inLen );
            return 0;
        }

        public virtual int DoFinal( byte[] outBytes, int outOff )
        {
            int num = this.ProcessPacket( this.data.GetBuffer(), 0, (int)this.data.Position, outBytes, outOff );
            this.Reset();
            return num;
        }

        public virtual void Reset()
        {
            this.cipher.Reset();
            this.associatedText.SetLength( 0L );
            this.data.SetLength( 0L );
        }

        public virtual byte[] GetMac() => Arrays.CopyOfRange( this.macBlock, 0, this.macSize );

        public virtual int GetUpdateOutputSize( int len ) => 0;

        public virtual int GetOutputSize( int len )
        {
            int num = (int)this.data.Length + len;
            if (this.forEncryption)
                return num + this.macSize;
            return num >= this.macSize ? num - this.macSize : 0;
        }

        public virtual byte[] ProcessPacket( byte[] input, int inOff, int inLen )
        {
            byte[] output;
            if (this.forEncryption)
                output = new byte[inLen + this.macSize];
            else
                output = inLen >= this.macSize ? new byte[inLen - this.macSize] : throw new InvalidCipherTextException( "data too short" );
            this.ProcessPacket( input, inOff, inLen, output, 0 );
            return output;
        }

        public virtual int ProcessPacket(
          byte[] input,
          int inOff,
          int inLen,
          byte[] output,
          int outOff )
        {
            if (this.keyParam == null)
                throw new InvalidOperationException( "CCM cipher unitialized." );
            int num1 = 15 - this.nonce.Length;
            if (num1 < 4)
            {
                int num2 = 1 << (8 * num1);
                if (inLen >= num2)
                    throw new InvalidOperationException( "CCM packet too large for choice of q." );
            }
            byte[] iv = new byte[BlockSize];
            iv[0] = (byte)((num1 - 1) & 7);
            this.nonce.CopyTo( iv, 1 );
            IBlockCipher blockCipher = new SicBlockCipher( this.cipher );
            blockCipher.Init( this.forEncryption, new ParametersWithIV( this.keyParam, iv ) );
            int num3 = inOff;
            int num4 = outOff;
            int num5;
            if (this.forEncryption)
            {
                num5 = inLen + this.macSize;
                Check.OutputLength( output, outOff, num5, "Output buffer too short." );
                this.CalculateMac( input, inOff, inLen, this.macBlock );
                byte[] numArray1 = new byte[BlockSize];
                blockCipher.ProcessBlock( this.macBlock, 0, numArray1, 0 );
                for (; num3 < inOff + inLen - BlockSize; num3 += BlockSize)
                {
                    blockCipher.ProcessBlock( input, num3, output, num4 );
                    num4 += BlockSize;
                }
                byte[] numArray2 = new byte[BlockSize];
                Array.Copy( input, num3, numArray2, 0, inLen + inOff - num3 );
                blockCipher.ProcessBlock( numArray2, 0, numArray2, 0 );
                Array.Copy( numArray2, 0, output, num4, inLen + inOff - num3 );
                Array.Copy( numArray1, 0, output, outOff + inLen, this.macSize );
            }
            else
            {
                if (inLen < this.macSize)
                    throw new InvalidCipherTextException( "data too short" );
                num5 = inLen - this.macSize;
                Check.OutputLength( output, outOff, num5, "Output buffer too short." );
                Array.Copy( input, inOff + num5, macBlock, 0, this.macSize );
                blockCipher.ProcessBlock( this.macBlock, 0, this.macBlock, 0 );
                for (int macSize = this.macSize; macSize != this.macBlock.Length; ++macSize)
                    this.macBlock[macSize] = 0;
                for (; num3 < inOff + num5 - BlockSize; num3 += BlockSize)
                {
                    blockCipher.ProcessBlock( input, num3, output, num4 );
                    num4 += BlockSize;
                }
                byte[] numArray3 = new byte[BlockSize];
                Array.Copy( input, num3, numArray3, 0, num5 - (num3 - inOff) );
                blockCipher.ProcessBlock( numArray3, 0, numArray3, 0 );
                Array.Copy( numArray3, 0, output, num4, num5 - (num3 - inOff) );
                byte[] numArray4 = new byte[BlockSize];
                this.CalculateMac( output, outOff, num5, numArray4 );
                if (!Arrays.ConstantTimeAreEqual( this.macBlock, numArray4 ))
                    throw new InvalidCipherTextException( "mac check in CCM failed" );
            }
            return num5;
        }

        private int CalculateMac( byte[] data, int dataOff, int dataLen, byte[] macBlock )
        {
            IMac mac = new CbcBlockCipherMac( this.cipher, this.macSize * 8 );
            mac.Init( this.keyParam );
            byte[] numArray1 = new byte[16];
            if (this.HasAssociatedText())
            {
                byte[] numArray2;
                (numArray2 = numArray1)[0] = (byte)(numArray2[0] | 64U);
            }
            byte[] numArray3;
            (numArray3 = numArray1)[0] = (byte)(numArray3[0] | (uint)(byte)((((mac.GetMacSize() - 2) / 2) & 7) << 3));
            byte[] numArray4;
            (numArray4 = numArray1)[0] = (byte)(numArray4[0] | (uint)(byte)((15 - this.nonce.Length - 1) & 7));
            Array.Copy( nonce, 0, numArray1, 1, this.nonce.Length );
            int num1 = dataLen;
            int num2 = 1;
            while (num1 > 0)
            {
                numArray1[numArray1.Length - num2] = (byte)(num1 & byte.MaxValue);
                num1 >>= 8;
                ++num2;
            }
            mac.BlockUpdate( numArray1, 0, numArray1.Length );
            if (this.HasAssociatedText())
            {
                int associatedTextLength = this.GetAssociatedTextLength();
                int num3;
                if (associatedTextLength < 65280)
                {
                    mac.Update( (byte)(associatedTextLength >> 8) );
                    mac.Update( (byte)associatedTextLength );
                    num3 = 2;
                }
                else
                {
                    mac.Update( byte.MaxValue );
                    mac.Update( 254 );
                    mac.Update( (byte)(associatedTextLength >> 24) );
                    mac.Update( (byte)(associatedTextLength >> 16) );
                    mac.Update( (byte)(associatedTextLength >> 8) );
                    mac.Update( (byte)associatedTextLength );
                    num3 = 6;
                }
                if (this.initialAssociatedText != null)
                    mac.BlockUpdate( this.initialAssociatedText, 0, this.initialAssociatedText.Length );
                if (this.associatedText.Position > 0L)
                {
                    byte[] buffer = this.associatedText.GetBuffer();
                    int position = (int)this.associatedText.Position;
                    mac.BlockUpdate( buffer, 0, position );
                }
                int num4 = (num3 + associatedTextLength) % 16;
                if (num4 != 0)
                {
                    for (int index = num4; index < 16; ++index)
                        mac.Update( 0 );
                }
            }
            mac.BlockUpdate( data, dataOff, dataLen );
            return mac.DoFinal( macBlock, 0 );
        }

        private int GetAssociatedTextLength() => (int)this.associatedText.Length + (this.initialAssociatedText == null ? 0 : this.initialAssociatedText.Length);

        private bool HasAssociatedText() => this.GetAssociatedTextLength() > 0;
    }
}
