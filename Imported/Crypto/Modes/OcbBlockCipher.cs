// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.OcbBlockCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Modes
{
    public class OcbBlockCipher : IAeadBlockCipher
    {
        private const int BLOCK_SIZE = 16;
        private readonly IBlockCipher hashCipher;
        private readonly IBlockCipher mainCipher;
        private bool forEncryption;
        private int macSize;
        private byte[] initialAssociatedText;
        private IList L;
        private byte[] L_Asterisk;
        private byte[] L_Dollar;
        private byte[] KtopInput = null;
        private byte[] Stretch = new byte[24];
        private byte[] OffsetMAIN_0 = new byte[16];
        private byte[] hashBlock;
        private byte[] mainBlock;
        private int hashBlockPos;
        private int mainBlockPos;
        private long hashBlockCount;
        private long mainBlockCount;
        private byte[] OffsetHASH;
        private byte[] Sum;
        private byte[] OffsetMAIN = new byte[16];
        private byte[] Checksum;
        private byte[] macBlock;

        public OcbBlockCipher( IBlockCipher hashCipher, IBlockCipher mainCipher )
        {
            if (hashCipher == null)
                throw new ArgumentNullException( nameof( hashCipher ) );
            if (hashCipher.GetBlockSize() != 16)
                throw new ArgumentException( "must have a block size of " + 16, nameof( hashCipher ) );
            if (mainCipher == null)
                throw new ArgumentNullException( nameof( mainCipher ) );
            if (mainCipher.GetBlockSize() != 16)
                throw new ArgumentException( "must have a block size of " + 16, nameof( mainCipher ) );
            if (!hashCipher.AlgorithmName.Equals( mainCipher.AlgorithmName ))
                throw new ArgumentException( "'hashCipher' and 'mainCipher' must be the same algorithm" );
            this.hashCipher = hashCipher;
            this.mainCipher = mainCipher;
        }

        public virtual IBlockCipher GetUnderlyingCipher() => this.mainCipher;

        public virtual string AlgorithmName => this.mainCipher.AlgorithmName + "/OCB";

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            bool forEncryption1 = this.forEncryption;
            this.forEncryption = forEncryption;
            this.macBlock = null;
            byte[] N;
            KeyParameter parameters1;
            switch (parameters)
            {
                case AeadParameters _:
                    AeadParameters aeadParameters = (AeadParameters)parameters;
                    N = aeadParameters.GetNonce();
                    this.initialAssociatedText = aeadParameters.GetAssociatedText();
                    int macSize = aeadParameters.MacSize;
                    if (macSize < 64 || macSize > 128 || macSize % 8 != 0)
                        throw new ArgumentException( "Invalid value for MAC size: " + macSize );
                    this.macSize = macSize / 8;
                    parameters1 = aeadParameters.Key;
                    break;
                case ParametersWithIV _:
                    ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                    N = parametersWithIv.GetIV();
                    this.initialAssociatedText = null;
                    this.macSize = 16;
                    parameters1 = (KeyParameter)parametersWithIv.Parameters;
                    break;
                default:
                    throw new ArgumentException( "invalid parameters passed to OCB" );
            }
            this.hashBlock = new byte[16];
            this.mainBlock = new byte[forEncryption ? 16 : 16 + this.macSize];
            if (N == null)
                N = new byte[0];
            if (N.Length > 15)
                throw new ArgumentException( "IV must be no more than 15 bytes" );
            if (parameters1 != null)
            {
                this.hashCipher.Init( true, parameters1 );
                this.mainCipher.Init( forEncryption, parameters1 );
                this.KtopInput = null;
            }
            else if (forEncryption1 != forEncryption)
                throw new ArgumentException( "cannot change encrypting state without providing key." );
            this.L_Asterisk = new byte[16];
            this.hashCipher.ProcessBlock( this.L_Asterisk, 0, this.L_Asterisk, 0 );
            this.L_Dollar = OCB_double( this.L_Asterisk );
            this.L = Platform.CreateArrayList();
            this.L.Add( OCB_double( this.L_Dollar ) );
            int num1 = this.ProcessNonce( N );
            int num2 = num1 % 8;
            int sourceIndex = num1 / 8;
            if (num2 == 0)
            {
                Array.Copy( Stretch, sourceIndex, OffsetMAIN_0, 0, 16 );
            }
            else
            {
                for (int index = 0; index < 16; ++index)
                {
                    uint num3 = this.Stretch[sourceIndex];
                    uint num4 = this.Stretch[++sourceIndex];
                    this.OffsetMAIN_0[index] = (byte)((num3 << num2) | (num4 >> (8 - num2)));
                }
            }
            this.hashBlockPos = 0;
            this.mainBlockPos = 0;
            this.hashBlockCount = 0L;
            this.mainBlockCount = 0L;
            this.OffsetHASH = new byte[16];
            this.Sum = new byte[16];
            Array.Copy( OffsetMAIN_0, 0, OffsetMAIN, 0, 16 );
            this.Checksum = new byte[16];
            if (this.initialAssociatedText == null)
                return;
            this.ProcessAadBytes( this.initialAssociatedText, 0, this.initialAssociatedText.Length );
        }

        protected virtual int ProcessNonce( byte[] N )
        {
            byte[] numArray1 = new byte[16];
            Array.Copy( N, 0, numArray1, numArray1.Length - N.Length, N.Length );
            numArray1[0] = (byte)(this.macSize << 4);
            byte[] numArray2;
            IntPtr index1;
            (numArray2 = numArray1)[(int)(index1 = (IntPtr)(15 - N.Length))] = (byte)(numArray2[(int)index1] | 1U);
            int num = numArray1[15] & 63;
            byte[] numArray3;
            (numArray3 = numArray1)[15] = (byte)(numArray3[15] & 192U);
            if (this.KtopInput == null || !Arrays.AreEqual( numArray1, this.KtopInput ))
            {
                byte[] numArray4 = new byte[16];
                this.KtopInput = numArray1;
                this.hashCipher.ProcessBlock( this.KtopInput, 0, numArray4, 0 );
                Array.Copy( numArray4, 0, Stretch, 0, 16 );
                for (int index2 = 0; index2 < 8; ++index2)
                    this.Stretch[16 + index2] = (byte)(numArray4[index2] ^ (uint)numArray4[index2 + 1]);
            }
            return num;
        }

        public virtual int GetBlockSize() => 16;

        public virtual byte[] GetMac() => Arrays.Clone( this.macBlock );

        public virtual int GetOutputSize( int len )
        {
            int num = len + this.mainBlockPos;
            if (this.forEncryption)
                return num + this.macSize;
            return num >= this.macSize ? num - this.macSize : 0;
        }

        public virtual int GetUpdateOutputSize( int len )
        {
            int num = len + this.mainBlockPos;
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
            this.hashBlock[this.hashBlockPos] = input;
            if (++this.hashBlockPos != this.hashBlock.Length)
                return;
            this.ProcessHashBlock();
        }

        public virtual void ProcessAadBytes( byte[] input, int off, int len )
        {
            for (int index = 0; index < len; ++index)
            {
                this.hashBlock[this.hashBlockPos] = input[off + index];
                if (++this.hashBlockPos == this.hashBlock.Length)
                    this.ProcessHashBlock();
            }
        }

        public virtual int ProcessByte( byte input, byte[] output, int outOff )
        {
            this.mainBlock[this.mainBlockPos] = input;
            if (++this.mainBlockPos != this.mainBlock.Length)
                return 0;
            this.ProcessMainBlock( output, outOff );
            return 16;
        }

        public virtual int ProcessBytes( byte[] input, int inOff, int len, byte[] output, int outOff )
        {
            int num = 0;
            for (int index = 0; index < len; ++index)
            {
                this.mainBlock[this.mainBlockPos] = input[inOff + index];
                if (++this.mainBlockPos == this.mainBlock.Length)
                {
                    this.ProcessMainBlock( output, outOff + num );
                    num += 16;
                }
            }
            return num;
        }

        public virtual int DoFinal( byte[] output, int outOff )
        {
            byte[] numArray1 = null;
            if (!this.forEncryption)
            {
                if (this.mainBlockPos < this.macSize)
                    throw new InvalidCipherTextException( "data too short" );
                this.mainBlockPos -= this.macSize;
                numArray1 = new byte[this.macSize];
                Array.Copy( mainBlock, this.mainBlockPos, numArray1, 0, this.macSize );
            }
            if (this.hashBlockPos > 0)
            {
                OCB_extend( this.hashBlock, this.hashBlockPos );
                this.UpdateHASH( this.L_Asterisk );
            }
            if (this.mainBlockPos > 0)
            {
                if (this.forEncryption)
                {
                    OCB_extend( this.mainBlock, this.mainBlockPos );
                    Xor( this.Checksum, this.mainBlock );
                }
                Xor( this.OffsetMAIN, this.L_Asterisk );
                byte[] numArray2 = new byte[16];
                this.hashCipher.ProcessBlock( this.OffsetMAIN, 0, numArray2, 0 );
                Xor( this.mainBlock, numArray2 );
                Check.OutputLength( output, outOff, this.mainBlockPos, "Output buffer too short" );
                Array.Copy( mainBlock, 0, output, outOff, this.mainBlockPos );
                if (!this.forEncryption)
                {
                    OCB_extend( this.mainBlock, this.mainBlockPos );
                    Xor( this.Checksum, this.mainBlock );
                }
            }
            Xor( this.Checksum, this.OffsetMAIN );
            Xor( this.Checksum, this.L_Dollar );
            this.hashCipher.ProcessBlock( this.Checksum, 0, this.Checksum, 0 );
            Xor( this.Checksum, this.Sum );
            this.macBlock = new byte[this.macSize];
            Array.Copy( Checksum, 0, macBlock, 0, this.macSize );
            int mainBlockPos = this.mainBlockPos;
            if (this.forEncryption)
            {
                Check.OutputLength( output, outOff, mainBlockPos + this.macSize, "Output buffer too short" );
                Array.Copy( macBlock, 0, output, outOff + mainBlockPos, this.macSize );
                mainBlockPos += this.macSize;
            }
            else if (!Arrays.ConstantTimeAreEqual( this.macBlock, numArray1 ))
                throw new InvalidCipherTextException( "mac check in OCB failed" );
            this.Reset( false );
            return mainBlockPos;
        }

        public virtual void Reset() => this.Reset( true );

        protected virtual void Clear( byte[] bs )
        {
            if (bs == null)
                return;
            Array.Clear( bs, 0, bs.Length );
        }

        protected virtual byte[] GetLSub( int n )
        {
            while (n >= this.L.Count)
                this.L.Add( OCB_double( (byte[])this.L[this.L.Count - 1] ) );
            return (byte[])this.L[n];
        }

        protected virtual void ProcessHashBlock()
        {
            this.UpdateHASH( this.GetLSub( OCB_ntz( ++this.hashBlockCount ) ) );
            this.hashBlockPos = 0;
        }

        protected virtual void ProcessMainBlock( byte[] output, int outOff )
        {
            Check.DataLength( output, outOff, 16, "Output buffer too short" );
            if (this.forEncryption)
            {
                Xor( this.Checksum, this.mainBlock );
                this.mainBlockPos = 0;
            }
            Xor( this.OffsetMAIN, this.GetLSub( OCB_ntz( ++this.mainBlockCount ) ) );
            Xor( this.mainBlock, this.OffsetMAIN );
            this.mainCipher.ProcessBlock( this.mainBlock, 0, this.mainBlock, 0 );
            Xor( this.mainBlock, this.OffsetMAIN );
            Array.Copy( mainBlock, 0, output, outOff, 16 );
            if (this.forEncryption)
                return;
            Xor( this.Checksum, this.mainBlock );
            Array.Copy( mainBlock, 16, mainBlock, 0, this.macSize );
            this.mainBlockPos = this.macSize;
        }

        protected virtual void Reset( bool clearMac )
        {
            this.hashCipher.Reset();
            this.mainCipher.Reset();
            this.Clear( this.hashBlock );
            this.Clear( this.mainBlock );
            this.hashBlockPos = 0;
            this.mainBlockPos = 0;
            this.hashBlockCount = 0L;
            this.mainBlockCount = 0L;
            this.Clear( this.OffsetHASH );
            this.Clear( this.Sum );
            Array.Copy( OffsetMAIN_0, 0, OffsetMAIN, 0, 16 );
            this.Clear( this.Checksum );
            if (clearMac)
                this.macBlock = null;
            if (this.initialAssociatedText == null)
                return;
            this.ProcessAadBytes( this.initialAssociatedText, 0, this.initialAssociatedText.Length );
        }

        protected virtual void UpdateHASH( byte[] LSub )
        {
            Xor( this.OffsetHASH, LSub );
            Xor( this.hashBlock, this.OffsetHASH );
            this.hashCipher.ProcessBlock( this.hashBlock, 0, this.hashBlock, 0 );
            Xor( this.Sum, this.hashBlock );
        }

        protected static byte[] OCB_double( byte[] block )
        {
            byte[] output = new byte[16];
            int num = ShiftLeft( block, output );
            byte[] numArray;
            (numArray = output)[15] = (byte)(numArray[15] ^ (uint)(byte)(135 >> ((1 - num) << 3)));
            return output;
        }

        protected static void OCB_extend( byte[] block, int pos )
        {
            block[pos] = 128;
            while (++pos < 16)
                block[pos] = 0;
        }

        protected static int OCB_ntz( long x )
        {
            if (x == 0L)
                return 64;
            int num = 0;
            for (ulong index = (ulong)x; ((long)index & 1L) == 0L; index >>= 1)
                ++num;
            return num;
        }

        protected static int ShiftLeft( byte[] block, byte[] output )
        {
            int index = 16;
            uint num1 = 0;
            while (--index >= 0)
            {
                uint num2 = block[index];
                output[index] = (byte)((num2 << 1) | num1);
                num1 = (num2 >> 7) & 1U;
            }
            return (int)num1;
        }

        protected static void Xor( byte[] block, byte[] val )
        {
            for (int index1 = 15; index1 >= 0; --index1)
            {
                byte[] numArray;
                IntPtr index2;
                (numArray = block)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray[(int)index2] ^ (uint)val[index1]);
            }
        }
    }
}
