// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.Gost3410DigestSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class Gost3410DigestSigner : ISigner
    {
        private readonly IDigest digest;
        private readonly IDsa dsaSigner;
        private bool forSigning;

        public Gost3410DigestSigner( IDsa signer, IDigest digest )
        {
            this.dsaSigner = signer;
            this.digest = digest;
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "with" + this.dsaSigner.AlgorithmName;

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            this.forSigning = forSigning;
            AsymmetricKeyParameter asymmetricKeyParameter = !(parameters is ParametersWithRandom) ? (AsymmetricKeyParameter)parameters : (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
            if (forSigning && !asymmetricKeyParameter.IsPrivate)
                throw new InvalidKeyException( "Signing Requires Private Key." );
            if (!forSigning && asymmetricKeyParameter.IsPrivate)
                throw new InvalidKeyException( "Verification Requires Public Key." );
            this.Reset();
            this.dsaSigner.Init( forSigning, parameters );
        }

        public virtual void Update( byte input ) => this.digest.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int length ) => this.digest.BlockUpdate( input, inOff, length );

        public virtual byte[] GenerateSignature()
        {
            if (!this.forSigning)
                throw new InvalidOperationException( "GOST3410DigestSigner not initialised for signature generation." );
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            try
            {
                BigInteger[] signature1 = this.dsaSigner.GenerateSignature( numArray );
                byte[] signature2 = new byte[64];
                byte[] byteArrayUnsigned1 = signature1[0].ToByteArrayUnsigned();
                byte[] byteArrayUnsigned2 = signature1[1].ToByteArrayUnsigned();
                byteArrayUnsigned2.CopyTo( signature2, 32 - byteArrayUnsigned2.Length );
                byteArrayUnsigned1.CopyTo( signature2, 64 - byteArrayUnsigned1.Length );
                return signature2;
            }
            catch (Exception ex)
            {
                throw new SignatureException( ex.Message, ex );
            }
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            if (this.forSigning)
                throw new InvalidOperationException( "DSADigestSigner not initialised for verification" );
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            BigInteger r;
            BigInteger s;
            try
            {
                r = new BigInteger( 1, signature, 32, 32 );
                s = new BigInteger( 1, signature, 0, 32 );
            }
            catch (Exception ex)
            {
                throw new SignatureException( "error decoding signature bytes.", ex );
            }
            return this.dsaSigner.VerifySignature( numArray, r, s );
        }

        public virtual void Reset() => this.digest.Reset();
    }
}
