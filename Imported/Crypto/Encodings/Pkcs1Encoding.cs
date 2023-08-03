// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Encodings.Pkcs1Encoding
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Encodings
{
    public class Pkcs1Encoding : IAsymmetricBlockCipher
    {
        public const string StrictLengthEnabledProperty = "Org.BouncyCastle.Pkcs1.Strict";
        private const int HeaderLength = 10;
        private static readonly bool[] strictLengthEnabled;
        private SecureRandom random;
        private IAsymmetricBlockCipher engine;
        private bool forEncryption;
        private bool forPrivateKey;
        private bool useStrictLength;
        private int pLen = -1;
        private byte[] fallback = null;

        public static bool StrictLengthEnabled
        {
            get => strictLengthEnabled[0];
            set => strictLengthEnabled[0] = value;
        }

        static Pkcs1Encoding()
        {
            string environmentVariable = Platform.GetEnvironmentVariable( "Org.BouncyCastle.Pkcs1.Strict" );
            strictLengthEnabled = new bool[1]
      {
        environmentVariable == null || environmentVariable.Equals("true")
      };
        }

        public Pkcs1Encoding( IAsymmetricBlockCipher cipher )
        {
            this.engine = cipher;
            this.useStrictLength = StrictLengthEnabled;
        }

        public Pkcs1Encoding( IAsymmetricBlockCipher cipher, int pLen )
        {
            this.engine = cipher;
            this.useStrictLength = StrictLengthEnabled;
            this.pLen = pLen;
        }

        public Pkcs1Encoding( IAsymmetricBlockCipher cipher, byte[] fallback )
        {
            this.engine = cipher;
            this.useStrictLength = StrictLengthEnabled;
            this.fallback = fallback;
            this.pLen = fallback.Length;
        }

        public IAsymmetricBlockCipher GetUnderlyingCipher() => this.engine;

        public string AlgorithmName => this.engine.AlgorithmName + "/PKCS1Padding";

        public void Init( bool forEncryption, ICipherParameters parameters )
        {
            AsymmetricKeyParameter asymmetricKeyParameter;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                this.random = parametersWithRandom.Random;
                asymmetricKeyParameter = (AsymmetricKeyParameter)parametersWithRandom.Parameters;
            }
            else
            {
                this.random = new SecureRandom();
                asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
            }
            this.engine.Init( forEncryption, parameters );
            this.forPrivateKey = asymmetricKeyParameter.IsPrivate;
            this.forEncryption = forEncryption;
        }

        public int GetInputBlockSize()
        {
            int inputBlockSize = this.engine.GetInputBlockSize();
            return !this.forEncryption ? inputBlockSize : inputBlockSize - 10;
        }

        public int GetOutputBlockSize()
        {
            int outputBlockSize = this.engine.GetOutputBlockSize();
            return !this.forEncryption ? outputBlockSize - 10 : outputBlockSize;
        }

        public byte[] ProcessBlock( byte[] input, int inOff, int length ) => !this.forEncryption ? this.DecodeBlock( input, inOff, length ) : this.EncodeBlock( input, inOff, length );

        private byte[] EncodeBlock( byte[] input, int inOff, int inLen )
        {
            if (inLen > this.GetInputBlockSize())
                throw new ArgumentException( "input data too large", nameof( inLen ) );
            byte[] numArray = new byte[this.engine.GetInputBlockSize()];
            if (this.forPrivateKey)
            {
                numArray[0] = 1;
                for (int index = 1; index != numArray.Length - inLen - 1; ++index)
                    numArray[index] = byte.MaxValue;
            }
            else
            {
                this.random.NextBytes( numArray );
                numArray[0] = 2;
                for (int index = 1; index != numArray.Length - inLen - 1; ++index)
                {
                    while (numArray[index] == 0)
                        numArray[index] = (byte)this.random.NextInt();
                }
            }
            numArray[numArray.Length - inLen - 1] = 0;
            Array.Copy( input, inOff, numArray, numArray.Length - inLen, inLen );
            return this.engine.ProcessBlock( numArray, 0, numArray.Length );
        }

        private static int CheckPkcs1Encoding( byte[] encoded, int pLen )
        {
            int num1 = 0 | (encoded[0] ^ 2);
            int num2 = encoded.Length - (pLen + 1);
            for (int index = 1; index < num2; ++index)
            {
                int num3 = encoded[index];
                int num4 = num3 | (num3 >> 1);
                int num5 = num4 | (num4 >> 2);
                int num6 = num5 | (num5 >> 4);
                num1 |= (num6 & 1) - 1;
            }
            int num7 = num1 | encoded[encoded.Length - (pLen + 1)];
            int num8 = num7 | (num7 >> 1);
            int num9 = num8 | (num8 >> 2);
            return ~(((num9 | (num9 >> 4)) & 1) - 1);
        }

        private byte[] DecodeBlockOrRandom( byte[] input, int inOff, int inLen )
        {
            if (!this.forPrivateKey)
                throw new InvalidCipherTextException( "sorry, this method is only for decryption, not for signing" );
            byte[] encoded = this.engine.ProcessBlock( input, inOff, inLen );
            byte[] buffer;
            if (this.fallback == null)
            {
                buffer = new byte[this.pLen];
                this.random.NextBytes( buffer );
            }
            else
                buffer = this.fallback;
            if (encoded.Length < this.GetOutputBlockSize())
                throw new InvalidCipherTextException( "block truncated" );
            if (this.useStrictLength && encoded.Length != this.engine.GetOutputBlockSize())
                throw new InvalidCipherTextException( "block incorrect size" );
            int num = CheckPkcs1Encoding( encoded, this.pLen );
            byte[] numArray = new byte[this.pLen];
            for (int index = 0; index < this.pLen; ++index)
                numArray[index] = (byte)((encoded[index + (encoded.Length - this.pLen)] & ~num) | (buffer[index] & num));
            return numArray;
        }

        private byte[] DecodeBlock( byte[] input, int inOff, int inLen )
        {
            if (this.pLen != -1)
                return this.DecodeBlockOrRandom( input, inOff, inLen );
            byte[] sourceArray = this.engine.ProcessBlock( input, inOff, inLen );
            byte num1 = sourceArray.Length >= this.GetOutputBlockSize() ? sourceArray[0] : throw new InvalidCipherTextException( "block truncated" );
            switch (num1)
            {
                case 1:
                case 2:
                    if (this.useStrictLength && sourceArray.Length != this.engine.GetOutputBlockSize())
                        throw new InvalidCipherTextException( "block incorrect size" );
                    int index;
                    for (index = 1; index != sourceArray.Length; ++index)
                    {
                        byte num2 = sourceArray[index];
                        if (num2 != 0)
                        {
                            if (num1 == 1 && num2 != byte.MaxValue)
                                throw new InvalidCipherTextException( "block padding incorrect" );
                        }
                        else
                            break;
                    }
                    int sourceIndex = index + 1;
                    if (sourceIndex > sourceArray.Length || sourceIndex < 10)
                        throw new InvalidCipherTextException( "no data in block" );
                    byte[] destinationArray = new byte[sourceArray.Length - sourceIndex];
                    Array.Copy( sourceArray, sourceIndex, destinationArray, 0, destinationArray.Length );
                    return destinationArray;
                default:
                    throw new InvalidCipherTextException( "unknown block type" );
            }
        }
    }
}
