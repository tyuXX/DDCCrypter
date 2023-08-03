// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.Rfc3394WrapEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class Rfc3394WrapEngine : IWrapper
    {
        private readonly IBlockCipher engine;
        private KeyParameter param;
        private bool forWrapping;
        private byte[] iv = new byte[8]
        {
       166,
       166,
       166,
       166,
       166,
       166,
       166,
       166
        };

        public Rfc3394WrapEngine( IBlockCipher engine ) => this.engine = engine;

        public virtual void Init( bool forWrapping, ICipherParameters parameters )
        {
            this.forWrapping = forWrapping;
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            if (parameters is KeyParameter)
            {
                this.param = (KeyParameter)parameters;
            }
            else
            {
                if (!(parameters is ParametersWithIV))
                    return;
                ParametersWithIV parametersWithIv = (ParametersWithIV)parameters;
                byte[] iv = parametersWithIv.GetIV();
                this.iv = iv.Length == 8 ? iv : throw new ArgumentException( "IV length not equal to 8", nameof( parameters ) );
                this.param = (KeyParameter)parametersWithIv.Parameters;
            }
        }

        public virtual string AlgorithmName => this.engine.AlgorithmName;

        public virtual byte[] Wrap( byte[] input, int inOff, int inLen )
        {
            if (!this.forWrapping)
                throw new InvalidOperationException( "not set for wrapping" );
            int num1 = inLen / 8;
            if (num1 * 8 != inLen)
                throw new DataLengthException( "wrap data must be a multiple of 8 bytes" );
            byte[] numArray1 = new byte[inLen + this.iv.Length];
            byte[] numArray2 = new byte[8 + this.iv.Length];
            Array.Copy( iv, 0, numArray1, 0, this.iv.Length );
            Array.Copy( input, inOff, numArray1, this.iv.Length, inLen );
            this.engine.Init( true, param );
            for (int index1 = 0; index1 != 6; ++index1)
            {
                for (int index2 = 1; index2 <= num1; ++index2)
                {
                    Array.Copy( numArray1, 0, numArray2, 0, this.iv.Length );
                    Array.Copy( numArray1, 8 * index2, numArray2, this.iv.Length, 8 );
                    this.engine.ProcessBlock( numArray2, 0, numArray2, 0 );
                    int num2 = (num1 * index1) + index2;
                    int num3 = 1;
                    while (num2 != 0)
                    {
                        byte num4 = (byte)num2;
                        byte[] numArray3;
                        IntPtr index3;
                        (numArray3 = numArray2)[(int)(index3 = (IntPtr)(this.iv.Length - num3))] = (byte)(numArray3[(int)index3] ^ (uint)num4);
                        num2 >>>= 8;
                        ++num3;
                    }
                    Array.Copy( numArray2, 0, numArray1, 0, 8 );
                    Array.Copy( numArray2, 8, numArray1, 8 * index2, 8 );
                }
            }
            return numArray1;
        }

        public virtual byte[] Unwrap( byte[] input, int inOff, int inLen )
        {
            if (this.forWrapping)
                throw new InvalidOperationException( "not set for unwrapping" );
            int num1 = inLen / 8;
            if (num1 * 8 != inLen)
                throw new InvalidCipherTextException( "unwrap data must be a multiple of 8 bytes" );
            byte[] numArray1 = new byte[inLen - this.iv.Length];
            byte[] numArray2 = new byte[this.iv.Length];
            byte[] numArray3 = new byte[8 + this.iv.Length];
            Array.Copy( input, inOff, numArray2, 0, this.iv.Length );
            Array.Copy( input, inOff + this.iv.Length, numArray1, 0, inLen - this.iv.Length );
            this.engine.Init( false, param );
            int num2 = num1 - 1;
            for (int index1 = 5; index1 >= 0; --index1)
            {
                for (int index2 = num2; index2 >= 1; --index2)
                {
                    Array.Copy( numArray2, 0, numArray3, 0, this.iv.Length );
                    Array.Copy( numArray1, 8 * (index2 - 1), numArray3, this.iv.Length, 8 );
                    int num3 = (num2 * index1) + index2;
                    int num4 = 1;
                    while (num3 != 0)
                    {
                        byte num5 = (byte)num3;
                        byte[] numArray4;
                        IntPtr index3;
                        (numArray4 = numArray3)[(int)(index3 = (IntPtr)(this.iv.Length - num4))] = (byte)(numArray4[(int)index3] ^ (uint)num5);
                        num3 >>>= 8;
                        ++num4;
                    }
                    this.engine.ProcessBlock( numArray3, 0, numArray3, 0 );
                    Array.Copy( numArray3, 0, numArray2, 0, 8 );
                    Array.Copy( numArray3, 8, numArray1, 8 * (index2 - 1), 8 );
                }
            }
            if (!Arrays.ConstantTimeAreEqual( numArray2, this.iv ))
                throw new InvalidCipherTextException( "checksum failed" );
            return numArray1;
        }
    }
}
