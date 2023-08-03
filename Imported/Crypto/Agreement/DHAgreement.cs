// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.DHAgreement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Agreement
{
    public class DHAgreement
    {
        private DHPrivateKeyParameters key;
        private DHParameters dhParams;
        private BigInteger privateValue;
        private SecureRandom random;

        public void Init( ICipherParameters parameters )
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
            this.key = asymmetricKeyParameter is DHPrivateKeyParameters ? (DHPrivateKeyParameters)asymmetricKeyParameter : throw new ArgumentException( "DHEngine expects DHPrivateKeyParameters" );
            this.dhParams = this.key.Parameters;
        }

        public BigInteger CalculateMessage()
        {
            DHKeyPairGenerator keyPairGenerator = new();
            keyPairGenerator.Init( new DHKeyGenerationParameters( this.random, this.dhParams ) );
            AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
            this.privateValue = ((DHPrivateKeyParameters)keyPair.Private).X;
            return ((DHPublicKeyParameters)keyPair.Public).Y;
        }

        public BigInteger CalculateAgreement( DHPublicKeyParameters pub, BigInteger message )
        {
            if (pub == null)
                throw new ArgumentNullException( nameof( pub ) );
            if (message == null)
                throw new ArgumentNullException( nameof( message ) );
            if (!pub.Parameters.Equals( dhParams ))
                throw new ArgumentException( "Diffie-Hellman public key has wrong parameters." );
            BigInteger p = this.dhParams.P;
            return message.ModPow( this.key.X, p ).Multiply( pub.Y.ModPow( this.privateValue, p ) ).Mod( p );
        }
    }
}
