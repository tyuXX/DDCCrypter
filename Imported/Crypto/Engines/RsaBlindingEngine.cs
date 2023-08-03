// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RsaBlindingEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RsaBlindingEngine : IAsymmetricBlockCipher
    {
        private readonly RsaCoreEngine core = new();
        private RsaKeyParameters key;
        private BigInteger blindingFactor;
        private bool forEncryption;

        public virtual string AlgorithmName => "RSA";

        public virtual void Init( bool forEncryption, ICipherParameters param )
        {
            RsaBlindingParameters blindingParameters = !(param is ParametersWithRandom) ? (RsaBlindingParameters)param : (RsaBlindingParameters)((ParametersWithRandom)param).Parameters;
            this.core.Init( forEncryption, blindingParameters.PublicKey );
            this.forEncryption = forEncryption;
            this.key = blindingParameters.PublicKey;
            this.blindingFactor = blindingParameters.BlindingFactor;
        }

        public virtual int GetInputBlockSize() => this.core.GetInputBlockSize();

        public virtual int GetOutputBlockSize() => this.core.GetOutputBlockSize();

        public virtual byte[] ProcessBlock( byte[] inBuf, int inOff, int inLen )
        {
            BigInteger bigInteger = this.core.ConvertInput( inBuf, inOff, inLen );
            return this.core.ConvertOutput( !this.forEncryption ? this.UnblindMessage( bigInteger ) : this.BlindMessage( bigInteger ) );
        }

        private BigInteger BlindMessage( BigInteger msg )
        {
            BigInteger blindingFactor = this.blindingFactor;
            return msg.Multiply( blindingFactor.ModPow( this.key.Exponent, this.key.Modulus ) ).Mod( this.key.Modulus );
        }

        private BigInteger UnblindMessage( BigInteger blindedMsg )
        {
            BigInteger modulus = this.key.Modulus;
            return blindedMsg.Multiply( this.blindingFactor.ModInverse( modulus ) ).Mod( modulus );
        }
    }
}
