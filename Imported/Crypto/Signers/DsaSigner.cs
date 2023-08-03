// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.DsaSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class DsaSigner : IDsa
    {
        protected readonly IDsaKCalculator kCalculator;
        protected DsaKeyParameters key = null;
        protected SecureRandom random = null;

        public DsaSigner() => this.kCalculator = new RandomDsaKCalculator();

        public DsaSigner( IDsaKCalculator kCalculator ) => this.kCalculator = kCalculator;

        public virtual string AlgorithmName => "DSA";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            SecureRandom provided = null;
            if (forSigning)
            {
                if (parameters is ParametersWithRandom)
                {
                    ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                    provided = parametersWithRandom.Random;
                    parameters = parametersWithRandom.Parameters;
                }
                this.key = parameters is DsaPrivateKeyParameters ? (DsaKeyParameters)parameters : throw new InvalidKeyException( "DSA private key required for signing" );
            }
            else
                this.key = parameters is DsaPublicKeyParameters ? (DsaKeyParameters)parameters : throw new InvalidKeyException( "DSA public key required for verification" );
            this.random = this.InitSecureRandom( forSigning && !this.kCalculator.IsDeterministic, provided );
        }

        public virtual BigInteger[] GenerateSignature( byte[] message )
        {
            DsaParameters parameters = this.key.Parameters;
            BigInteger q = parameters.Q;
            BigInteger e1 = this.CalculateE( q, message );
            BigInteger x = ((DsaPrivateKeyParameters)this.key).X;
            if (this.kCalculator.IsDeterministic)
                this.kCalculator.Init( q, x, message );
            else
                this.kCalculator.Init( q, this.random );
            BigInteger e2 = this.kCalculator.NextK();
            BigInteger val = parameters.G.ModPow( e2, parameters.P ).Mod( q );
            BigInteger bigInteger = e2.ModInverse( q ).Multiply( e1.Add( x.Multiply( val ) ) ).Mod( q );
            return new BigInteger[2] { val, bigInteger };
        }

        public virtual bool VerifySignature( byte[] message, BigInteger r, BigInteger s )
        {
            DsaParameters parameters = this.key.Parameters;
            BigInteger q = parameters.Q;
            BigInteger e1 = this.CalculateE( q, message );
            if (r.SignValue <= 0 || q.CompareTo( r ) <= 0 || s.SignValue <= 0 || q.CompareTo( s ) <= 0)
                return false;
            BigInteger val = s.ModInverse( q );
            BigInteger e2 = e1.Multiply( val ).Mod( q );
            BigInteger e3 = r.Multiply( val ).Mod( q );
            BigInteger p = parameters.P;
            return parameters.G.ModPow( e2, p ).Multiply( ((DsaPublicKeyParameters)this.key).Y.ModPow( e3, p ) ).Mod( p ).Mod( q ).Equals( r );
        }

        protected virtual BigInteger CalculateE( BigInteger n, byte[] message )
        {
            int length = System.Math.Min( message.Length, n.BitLength / 8 );
            return new BigInteger( 1, message, 0, length );
        }

        protected virtual SecureRandom InitSecureRandom( bool needed, SecureRandom provided )
        {
            if (!needed)
                return null;
            return provided == null ? new SecureRandom() : provided;
        }
    }
}
