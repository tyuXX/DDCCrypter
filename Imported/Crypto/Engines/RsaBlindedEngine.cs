// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RsaBlindedEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RsaBlindedEngine : IAsymmetricBlockCipher
    {
        private readonly RsaCoreEngine core = new();
        private RsaKeyParameters key;
        private SecureRandom random;

        public virtual string AlgorithmName => "RSA";

        public virtual void Init( bool forEncryption, ICipherParameters param )
        {
            this.core.Init( forEncryption, param );
            if (param is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
                this.key = (RsaKeyParameters)parametersWithRandom.Parameters;
                this.random = parametersWithRandom.Random;
            }
            else
            {
                this.key = (RsaKeyParameters)param;
                this.random = new SecureRandom();
            }
        }

        public virtual int GetInputBlockSize() => this.core.GetInputBlockSize();

        public virtual int GetOutputBlockSize() => this.core.GetOutputBlockSize();

        public virtual byte[] ProcessBlock( byte[] inBuf, int inOff, int inLen )
        {
            if (this.key == null)
                throw new InvalidOperationException( "RSA engine not initialised" );
            BigInteger bigInteger = this.core.ConvertInput( inBuf, inOff, inLen );
            BigInteger result;
            if (this.key is RsaPrivateCrtKeyParameters)
            {
                RsaPrivateCrtKeyParameters key = (RsaPrivateCrtKeyParameters)this.key;
                BigInteger publicExponent = key.PublicExponent;
                if (publicExponent != null)
                {
                    BigInteger modulus = key.Modulus;
                    BigInteger randomInRange = BigIntegers.CreateRandomInRange( BigInteger.One, modulus.Subtract( BigInteger.One ), this.random );
                    result = this.core.ProcessBlock( randomInRange.ModPow( publicExponent, modulus ).Multiply( bigInteger ).Mod( modulus ) ).Multiply( randomInRange.ModInverse( modulus ) ).Mod( modulus );
                    if (!bigInteger.Equals( result.ModPow( publicExponent, modulus ) ))
                        throw new InvalidOperationException( "RSA engine faulty decryption/signing detected" );
                }
                else
                    result = this.core.ProcessBlock( bigInteger );
            }
            else
                result = this.core.ProcessBlock( bigInteger );
            return this.core.ConvertOutput( result );
        }
    }
}
