// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.DsaDigestSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class DsaDigestSigner : ISigner
    {
        private readonly IDigest digest;
        private readonly IDsa dsaSigner;
        private bool forSigning;

        public DsaDigestSigner( IDsa signer, IDigest digest )
        {
            this.digest = digest;
            this.dsaSigner = signer;
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
                throw new InvalidOperationException( "DSADigestSigner not initialised for signature generation." );
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            BigInteger[] signature = this.dsaSigner.GenerateSignature( numArray );
            return this.DerEncode( signature[0], signature[1] );
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            if (this.forSigning)
                throw new InvalidOperationException( "DSADigestSigner not initialised for verification" );
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            try
            {
                BigInteger[] bigIntegerArray = this.DerDecode( signature );
                return this.dsaSigner.VerifySignature( numArray, bigIntegerArray[0], bigIntegerArray[1] );
            }
            catch (IOException ex)
            {
                return false;
            }
        }

        public virtual void Reset() => this.digest.Reset();

        private byte[] DerEncode( BigInteger r, BigInteger s ) => new DerSequence( new Asn1Encodable[2]
        {
       new DerInteger(r),
       new DerInteger(s)
        } ).GetDerEncoded();

        private BigInteger[] DerDecode( byte[] encoding )
        {
            Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray( encoding );
            return new BigInteger[2]
            {
        ((DerInteger) asn1Sequence[0]).Value,
        ((DerInteger) asn1Sequence[1]).Value
            };
        }
    }
}
