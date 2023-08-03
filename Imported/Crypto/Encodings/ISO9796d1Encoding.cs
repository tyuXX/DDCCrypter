// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Encodings.ISO9796d1Encoding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Encodings
{
    public class ISO9796d1Encoding : IAsymmetricBlockCipher
    {
        private static readonly BigInteger Sixteen = BigInteger.ValueOf( 16L );
        private static readonly BigInteger Six = BigInteger.ValueOf( 6L );
        private static readonly byte[] shadows = new byte[16]
        {
       14,
       3,
       5,
       8,
       9,
       4,
       2,
       15,
       0,
       13,
       11,
       6,
       7,
       10,
       12,
       1
        };
        private static readonly byte[] inverse = new byte[16]
        {
       8,
       15,
       6,
       1,
       5,
       2,
       11,
       12,
       3,
       4,
       13,
       10,
       14,
       9,
       0,
       7
        };
        private readonly IAsymmetricBlockCipher engine;
        private bool forEncryption;
        private int bitSize;
        private int padBits = 0;
        private BigInteger modulus;

        public ISO9796d1Encoding( IAsymmetricBlockCipher cipher ) => this.engine = cipher;

        public string AlgorithmName => this.engine.AlgorithmName + "/ISO9796-1Padding";

        public IAsymmetricBlockCipher GetUnderlyingCipher() => this.engine;

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            RsaKeyParameters rsaKeyParameters = !(parameters is ParametersWithRandom) ? (RsaKeyParameters)parameters : (RsaKeyParameters)((ParametersWithRandom)parameters).Parameters;
            this.engine.Init( forEncryption, parameters );
            this.modulus = rsaKeyParameters.Modulus;
            this.bitSize = this.modulus.BitLength;
            this.forEncryption = forEncryption;
        }

        public int GetInputBlockSize()
        {
            int inputBlockSize = this.engine.GetInputBlockSize();
            return this.forEncryption ? (inputBlockSize + 1) / 2 : inputBlockSize;
        }

        public int GetOutputBlockSize()
        {
            int outputBlockSize = this.engine.GetOutputBlockSize();
            return this.forEncryption ? outputBlockSize : (outputBlockSize + 1) / 2;
        }

        public void SetPadBits( int padBits ) => this.padBits = padBits <= 7 ? padBits : throw new ArgumentException( "padBits > 7" );

        public int GetPadBits() => this.padBits;

        public byte[] ProcessBlock( byte[] input, int inOff, int length ) => this.forEncryption ? this.EncodeBlock( input, inOff, length ) : this.DecodeBlock( input, inOff, length );

        private byte[] EncodeBlock( byte[] input, int inOff, int inLen )
        {
            byte[] numArray1 = new byte[(this.bitSize + 7) / 8];
            int num1 = this.padBits + 1;
            int length = inLen;
            int num2 = (this.bitSize + 13) / 16;
            for (int index = 0; index < num2; index += length)
            {
                if (index > num2 - length)
                    Array.Copy( input, inOff + inLen - (num2 - index), numArray1, numArray1.Length - num2, num2 - index );
                else
                    Array.Copy( input, inOff, numArray1, numArray1.Length - (index + length), length );
            }
            for (int index = numArray1.Length - (2 * num2); index != numArray1.Length; index += 2)
            {
                byte num3 = numArray1[numArray1.Length - num2 + (index / 2)];
                numArray1[index] = (byte)(((uint)shadows[(int)(IntPtr)((num3 & (uint)byte.MaxValue) >> 4)] << 4) | shadows[num3 & 15]);
                numArray1[index + 1] = num3;
            }
            byte[] numArray2;
            IntPtr index1;
            (numArray2 = numArray1)[(int)(index1 = (IntPtr)(numArray1.Length - (2 * length)))] = (byte)(numArray2[(int)index1] ^ (uint)(byte)num1);
            numArray1[numArray1.Length - 1] = (byte)((numArray1[numArray1.Length - 1] << 4) | 6);
            int num4 = 8 - ((this.bitSize - 1) % 8);
            int inOff1 = 0;
            if (num4 != 8)
            {
                byte[] numArray3;
                (numArray3 = numArray1)[0] = (byte)(numArray3[0] & (uint)(byte)(byte.MaxValue >> num4));
                byte[] numArray4;
                (numArray4 = numArray1)[0] = (byte)(numArray4[0] | (uint)(byte)(128 >> num4));
            }
            else
            {
                numArray1[0] = 0;
                byte[] numArray5;
                (numArray5 = numArray1)[1] = (byte)(numArray5[1] | 128U);
                inOff1 = 1;
            }
            return this.engine.ProcessBlock( numArray1, inOff1, numArray1.Length - inOff1 );
        }

        private byte[] DecodeBlock( byte[] input, int inOff, int inLen )
        {
            byte[] bytes = this.engine.ProcessBlock( input, inOff, inLen );
            int num1 = 1;
            int num2 = (this.bitSize + 13) / 16;
            BigInteger n = new( 1, bytes );
            BigInteger bigInteger;
            if (n.Mod( Sixteen ).Equals( Six ))
            {
                bigInteger = n;
            }
            else
            {
                bigInteger = this.modulus.Subtract( n );
                if (!bigInteger.Mod( Sixteen ).Equals( Six ))
                    throw new InvalidCipherTextException( "resulting integer iS or (modulus - iS) is not congruent to 6 mod 16" );
            }
            byte[] byteArrayUnsigned = bigInteger.ToByteArrayUnsigned();
            if ((byteArrayUnsigned[byteArrayUnsigned.Length - 1] & 15) != 6)
                throw new InvalidCipherTextException( "invalid forcing byte in block" );
            byteArrayUnsigned[byteArrayUnsigned.Length - 1] = (byte)(((ushort)(byteArrayUnsigned[byteArrayUnsigned.Length - 1] & (uint)byte.MaxValue) >> 4) | (inverse[(byteArrayUnsigned[byteArrayUnsigned.Length - 2] & byte.MaxValue) >> 4] << 4));
            byteArrayUnsigned[0] = (byte)(((uint)shadows[(int)(IntPtr)((byteArrayUnsigned[1] & (uint)byte.MaxValue) >> 4)] << 4) | shadows[byteArrayUnsigned[1] & 15]);
            bool flag = false;
            int index1 = 0;
            for (int index2 = byteArrayUnsigned.Length - 1; index2 >= byteArrayUnsigned.Length - (2 * num2); index2 -= 2)
            {
                int num3 = (shadows[(int)(IntPtr)((byteArrayUnsigned[index2] & (uint)byte.MaxValue) >> 4)] << 4) | shadows[byteArrayUnsigned[index2] & 15];
                if (((byteArrayUnsigned[index2 - 1] ^ num3) & byte.MaxValue) != 0)
                {
                    flag = !flag ? true : throw new InvalidCipherTextException( "invalid tsums in block" );
                    num1 = (byteArrayUnsigned[index2 - 1] ^ num3) & byte.MaxValue;
                    index1 = index2 - 1;
                }
            }
            byteArrayUnsigned[index1] = 0;
            byte[] numArray = new byte[(byteArrayUnsigned.Length - index1) / 2];
            for (int index3 = 0; index3 < numArray.Length; ++index3)
                numArray[index3] = byteArrayUnsigned[(2 * index3) + index1 + 1];
            this.padBits = num1 - 1;
            return numArray;
        }
    }
}
