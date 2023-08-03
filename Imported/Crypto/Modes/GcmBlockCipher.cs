// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.GcmBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes.Gcm;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class GcmBlockCipher : IAeadBlockCipher
    {
        private const int BlockSize = 16;
        private readonly IBlockCipher cipher;
        private readonly IGcmMultiplier multiplier;
        private IGcmExponentiator exp;
        private bool forEncryption;
        private int macSize;
        private byte[] nonce;
        private byte[] initialAssociatedText;
        private byte[] H;
        private byte[] J0;
        private byte[] bufBlock;
        private byte[] macBlock;
        private byte[] S;
        private byte[] S_at;
        private byte[] S_atPre;
        private byte[] counter;
        private int bufOff;
        private ulong totalLength;
        private byte[] atBlock;
        private int atBlockPos;
        private ulong atLength;
        private ulong atLengthPre;

        public GcmBlockCipher( IBlockCipher c )
          : this( c, null )
        {
        }

        public GcmBlockCipher( IBlockCipher c, IGcmMultiplier m )
        {
            if (c.GetBlockSize() != 16)
                throw new ArgumentException( "cipher required with a block size of " + 16 + "." );
            if (m == null)
                m = new Tables8kGcmMultiplier();
            this.cipher = c;
            this.multiplier = m;
        }

        public virtual string AlgorithmName => this.cipher.AlgorithmName + "/GCM";

        public IBlockCipher GetUnderlyingCipher() => this.cipher;

        public virtual int GetBlockSize() => 16;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.forEncryption = forEncryption;
            this.macBlock = null;
            KeyParameter parameters1;
            switch (parameters)
            {
                case AeadParameters _:
                    AeadParameters aeadParameters = (AeadParameters)parameters;
                    this.nonce = aeadParameters.GetNonce();
                    this.initialAssociatedText = aeadParameters.GetAssociatedText();
                    int macSize = aeadParameters.MacSize;
                    if (macSize < 32 || macSize > 128 || macSize % 8 != 0)
                        throw new ArgumentException( "Invalid value for MAC size: " + macSize );
                    this.macSize = macSize / 8;
                    parameters1 = aeadParameters.Key;
                    break;
                case ParametersWithIV _:
                    ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                    this.nonce = parametersWithIv.GetIV();
                    this.initialAssociatedText = null;
                    this.macSize = 16;
                    parameters1 = (KeyParameter)parametersWithIv.Parameters;
                    break;
                default:
                    throw new ArgumentException( "invalid parameters passed to GCM" );
            }
            this.bufBlock = new byte[forEncryption ? 16 : 16 + this.macSize];
            if (this.nonce == null || this.nonce.Length < 1)
                throw new ArgumentException( "IV must be at least 1 byte" );
            if (parameters1 != null)
            {
                this.cipher.Init( true, parameters1 );
                this.H = new byte[16];
                this.cipher.ProcessBlock( this.H, 0, this.H, 0 );
                this.multiplier.Init( this.H );
                this.exp = null;
            }
            else if (this.H == null)
                throw new ArgumentException( "Key must be specified in initial init" );
            this.J0 = new byte[16];
            if (this.nonce.Length == 12)
            {
                Array.Copy( nonce, 0, J0, 0, this.nonce.Length );
                this.J0[15] = 1;
            }
            else
            {
                this.gHASH( this.J0, this.nonce, this.nonce.Length );
                byte[] numArray = new byte[16];
                Pack.UInt64_To_BE( (ulong)this.nonce.Length * 8UL, numArray, 8 );
                this.gHASHBlock( this.J0, numArray );
            }
            this.S = new byte[16];
            this.S_at = new byte[16];
            this.S_atPre = new byte[16];
            this.atBlock = new byte[16];
            this.atBlockPos = 0;
            this.atLength = 0UL;
            this.atLengthPre = 0UL;
            this.counter = Arrays.Clone( this.J0 );
            this.bufOff = 0;
            this.totalLength = 0UL;
            if (this.initialAssociatedText == null)
                return;
            this.ProcessAadBytes( this.initialAssociatedText, 0, this.initialAssociatedText.Length );
        }

        public virtual byte[] GetMac() => Arrays.Clone( this.macBlock );

        public virtual int GetOutputSize( int len )
        {
            int num = len + this.bufOff;
            if (this.forEncryption)
                return num + this.macSize;
            return num >= this.macSize ? num - this.macSize : 0;
        }

        public virtual int GetUpdateOutputSize( int len )
        {
            int num = len + this.bufOff;
            if (!this.forEncryption)
            {
                if (num < this.macSize)
                    return 0;
                num -= this.macSize;
            }
            return num - (num % 16);
        }

        public virtual void ProcessAadByte( byte input )
        {
            this.atBlock[this.atBlockPos] = input;
            if (++this.atBlockPos != 16)
                return;
            this.gHASHBlock( this.S_at, this.atBlock );
            this.atBlockPos = 0;
            this.atLength += 16UL;
        }

        public virtual void ProcessAadBytes( byte[] inBytes, int inOff, int len )
        {
            for (int index = 0; index < len; ++index)
            {
                this.atBlock[this.atBlockPos] = inBytes[inOff + index];
                if (++this.atBlockPos == 16)
                {
                    this.gHASHBlock( this.S_at, this.atBlock );
                    this.atBlockPos = 0;
                    this.atLength += 16UL;
                }
            }
        }

        private void InitCipher()
        {
            if (this.atLength > 0UL)
            {
                Array.Copy( S_at, 0, S_atPre, 0, 16 );
                this.atLengthPre = this.atLength;
            }
            if (this.atBlockPos > 0)
            {
                this.gHASHPartial( this.S_atPre, this.atBlock, 0, this.atBlockPos );
                this.atLengthPre += (uint)this.atBlockPos;
            }
            if (this.atLengthPre <= 0UL)
                return;
            Array.Copy( S_atPre, 0, S, 0, 16 );
        }

        public virtual int ProcessByte( byte input, byte[] output, int outOff )
        {
            this.bufBlock[this.bufOff] = input;
            if (++this.bufOff != this.bufBlock.Length)
                return 0;
            this.OutputBlock( output, outOff );
            return 16;
        }

        public virtual int ProcessBytes( byte[] input, int inOff, int len, byte[] output, int outOff )
        {
            if (input.Length < inOff + len)
                throw new DataLengthException( "Input buffer too short" );
            int num = 0;
            for (int index = 0; index < len; ++index)
            {
                this.bufBlock[this.bufOff] = input[inOff + index];
                if (++this.bufOff == this.bufBlock.Length)
                {
                    this.OutputBlock( output, outOff + num );
                    num += 16;
                }
            }
            return num;
        }

        private void OutputBlock( byte[] output, int offset )
        {
            Check.OutputLength( output, offset, 16, "Output buffer too short" );
            if (this.totalLength == 0UL)
                this.InitCipher();
            this.gCTRBlock( this.bufBlock, output, offset );
            if (this.forEncryption)
            {
                this.bufOff = 0;
            }
            else
            {
                Array.Copy( bufBlock, 16, bufBlock, 0, this.macSize );
                this.bufOff = this.macSize;
            }
        }

        public int DoFinal( byte[] output, int outOff )
        {
            if (this.totalLength == 0UL)
                this.InitCipher();
            int bufOff = this.bufOff;
            if (this.forEncryption)
            {
                Check.OutputLength( output, outOff, bufOff + this.macSize, "Output buffer too short" );
            }
            else
            {
                if (bufOff < this.macSize)
                    throw new InvalidCipherTextException( "data too short" );
                bufOff -= this.macSize;
                Check.OutputLength( output, outOff, bufOff, "Output buffer too short" );
            }
            if (bufOff > 0)
                this.gCTRPartial( this.bufBlock, 0, bufOff, output, outOff );
            this.atLength += (uint)this.atBlockPos;
            if (this.atLength > this.atLengthPre)
            {
                if (this.atBlockPos > 0)
                    this.gHASHPartial( this.S_at, this.atBlock, 0, this.atBlockPos );
                if (this.atLengthPre > 0UL)
                    GcmUtilities.Xor( this.S_at, this.S_atPre );
                long pow = (long)(((this.totalLength * 8UL) + (ulong)sbyte.MaxValue) >> 7);
                byte[] numArray = new byte[16];
                if (this.exp == null)
                {
                    this.exp = new Tables1kGcmExponentiator();
                    this.exp.Init( this.H );
                }
                this.exp.ExponentiateX( pow, numArray );
                GcmUtilities.Multiply( this.S_at, numArray );
                GcmUtilities.Xor( this.S, this.S_at );
            }
            byte[] numArray1 = new byte[16];
            Pack.UInt64_To_BE( this.atLength * 8UL, numArray1, 0 );
            Pack.UInt64_To_BE( this.totalLength * 8UL, numArray1, 8 );
            this.gHASHBlock( this.S, numArray1 );
            byte[] numArray2 = new byte[16];
            this.cipher.ProcessBlock( this.J0, 0, numArray2, 0 );
            GcmUtilities.Xor( numArray2, this.S );
            int num = bufOff;
            this.macBlock = new byte[this.macSize];
            Array.Copy( numArray2, 0, macBlock, 0, this.macSize );
            if (this.forEncryption)
            {
                Array.Copy( macBlock, 0, output, outOff + this.bufOff, this.macSize );
                num += this.macSize;
            }
            else
            {
                byte[] numArray3 = new byte[this.macSize];
                Array.Copy( bufBlock, bufOff, numArray3, 0, this.macSize );
                if (!Arrays.ConstantTimeAreEqual( this.macBlock, numArray3 ))
                    throw new InvalidCipherTextException( "mac check in GCM failed" );
            }
            this.Reset( false );
            return num;
        }

        public virtual void Reset() => this.Reset( true );

        private void Reset( bool clearMac )
        {
            this.cipher.Reset();
            this.S = new byte[16];
            this.S_at = new byte[16];
            this.S_atPre = new byte[16];
            this.atBlock = new byte[16];
            this.atBlockPos = 0;
            this.atLength = 0UL;
            this.atLengthPre = 0UL;
            this.counter = Arrays.Clone( this.J0 );
            this.bufOff = 0;
            this.totalLength = 0UL;
            if (this.bufBlock != null)
                Arrays.Fill( this.bufBlock, 0 );
            if (clearMac)
                this.macBlock = null;
            if (this.initialAssociatedText == null)
                return;
            this.ProcessAadBytes( this.initialAssociatedText, 0, this.initialAssociatedText.Length );
        }

        private void gCTRBlock( byte[] block, byte[] output, int outOff )
        {
            byte[] nextCounterBlock = this.GetNextCounterBlock();
            GcmUtilities.Xor( nextCounterBlock, block );
            Array.Copy( nextCounterBlock, 0, output, outOff, 16 );
            this.gHASHBlock( this.S, this.forEncryption ? nextCounterBlock : block );
            this.totalLength += 16UL;
        }

        private void gCTRPartial( byte[] buf, int off, int len, byte[] output, int outOff )
        {
            byte[] nextCounterBlock = this.GetNextCounterBlock();
            GcmUtilities.Xor( nextCounterBlock, buf, off, len );
            Array.Copy( nextCounterBlock, 0, output, outOff, len );
            this.gHASHPartial( this.S, this.forEncryption ? nextCounterBlock : buf, 0, len );
            this.totalLength += (uint)len;
        }

        private void gHASH( byte[] Y, byte[] b, int len )
        {
            for (int off = 0; off < len; off += 16)
            {
                int len1 = System.Math.Min( len - off, 16 );
                this.gHASHPartial( Y, b, off, len1 );
            }
        }

        private void gHASHBlock( byte[] Y, byte[] b )
        {
            GcmUtilities.Xor( Y, b );
            this.multiplier.MultiplyH( Y );
        }

        private void gHASHPartial( byte[] Y, byte[] b, int off, int len )
        {
            GcmUtilities.Xor( Y, b, off, len );
            this.multiplier.MultiplyH( Y );
        }

        private byte[] GetNextCounterBlock()
        {
            uint num1 = 1U + this.counter[15];
            this.counter[15] = (byte)num1;
            uint num2 = (num1 >> 8) + this.counter[14];
            this.counter[14] = (byte)num2;
            uint num3 = (num2 >> 8) + this.counter[13];
            this.counter[13] = (byte)num3;
            this.counter[12] = (byte)((num3 >> 8) + this.counter[12]);
            byte[] outBuf = new byte[16];
            this.cipher.ProcessBlock( this.counter, 0, outBuf, 0 );
            return outBuf;
        }
    }
}
