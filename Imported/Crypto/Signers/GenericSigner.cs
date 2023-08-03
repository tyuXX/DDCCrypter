// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.GenericSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class GenericSigner : ISigner
    {
        private readonly IAsymmetricBlockCipher engine;
        private readonly IDigest digest;
        private bool forSigning;

        public GenericSigner( IAsymmetricBlockCipher engine, IDigest digest )
        {
            this.engine = engine;
            this.digest = digest;
        }

        public virtual string AlgorithmName => "Generic(" + this.engine.AlgorithmName + "/" + this.digest.AlgorithmName + ")";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            this.forSigning = forSigning;
            AsymmetricKeyParameter asymmetricKeyParameter = !(parameters is ParametersWithRandom) ? (AsymmetricKeyParameter)parameters : (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
            if (forSigning && !asymmetricKeyParameter.IsPrivate)
                throw new InvalidKeyException( "Signing requires private key." );
            if (!forSigning && asymmetricKeyParameter.IsPrivate)
                throw new InvalidKeyException( "Verification requires public key." );
            this.Reset();
            this.engine.Init( forSigning, parameters );
        }

        public virtual void Update( byte input ) => this.digest.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int length ) => this.digest.BlockUpdate( input, inOff, length );

        public virtual byte[] GenerateSignature()
        {
            if (!this.forSigning)
                throw new InvalidOperationException( "GenericSigner not initialised for signature generation." );
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            return this.engine.ProcessBlock( numArray, 0, numArray.Length );
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            if (this.forSigning)
                throw new InvalidOperationException( "GenericSigner not initialised for verification" );
            byte[] numArray1 = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray1, 0 );
            try
            {
                byte[] numArray2 = this.engine.ProcessBlock( signature, 0, signature.Length );
                if (numArray2.Length < numArray1.Length)
                {
                    byte[] destinationArray = new byte[numArray1.Length];
                    Array.Copy( numArray2, 0, destinationArray, destinationArray.Length - numArray2.Length, numArray2.Length );
                    numArray2 = destinationArray;
                }
                return Arrays.ConstantTimeAreEqual( numArray2, numArray1 );
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual void Reset() => this.digest.Reset();
    }
}
