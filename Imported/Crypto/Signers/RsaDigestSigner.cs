// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.RsaDigestSigner
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class RsaDigestSigner : ISigner
    {
        private readonly IAsymmetricBlockCipher rsaEngine = new Pkcs1Encoding( new RsaBlindedEngine() );
        private readonly AlgorithmIdentifier algId;
        private readonly IDigest digest;
        private bool forSigning;
        private static readonly IDictionary oidMap = Platform.CreateHashtable();

        static RsaDigestSigner()
        {
            oidMap["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
            oidMap["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
            oidMap["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
            oidMap["SHA-1"] = X509ObjectIdentifiers.IdSha1;
            oidMap["SHA-224"] = NistObjectIdentifiers.IdSha224;
            oidMap["SHA-256"] = NistObjectIdentifiers.IdSha256;
            oidMap["SHA-384"] = NistObjectIdentifiers.IdSha384;
            oidMap["SHA-512"] = NistObjectIdentifiers.IdSha512;
            oidMap["MD2"] = PkcsObjectIdentifiers.MD2;
            oidMap["MD4"] = PkcsObjectIdentifiers.MD4;
            oidMap["MD5"] = PkcsObjectIdentifiers.MD5;
        }

        public RsaDigestSigner( IDigest digest )
          : this( digest, (DerObjectIdentifier)oidMap[digest.AlgorithmName] )
        {
        }

        public RsaDigestSigner( IDigest digest, DerObjectIdentifier digestOid )
          : this( digest, new AlgorithmIdentifier( digestOid, DerNull.Instance ) )
        {
        }

        public RsaDigestSigner( IDigest digest, AlgorithmIdentifier algId )
        {
            this.digest = digest;
            this.algId = algId;
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "withRSA";

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            this.forSigning = forSigning;
            AsymmetricKeyParameter asymmetricKeyParameter = !(parameters is ParametersWithRandom) ? (AsymmetricKeyParameter)parameters : (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
            if (forSigning && !asymmetricKeyParameter.IsPrivate)
                throw new InvalidKeyException( "Signing requires private key." );
            if (!forSigning && asymmetricKeyParameter.IsPrivate)
                throw new InvalidKeyException( "Verification requires public key." );
            this.Reset();
            this.rsaEngine.Init( forSigning, parameters );
        }

        public virtual void Update( byte input ) => this.digest.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int length ) => this.digest.BlockUpdate( input, inOff, length );

        public virtual byte[] GenerateSignature()
        {
            if (!this.forSigning)
                throw new InvalidOperationException( "RsaDigestSigner not initialised for signature generation." );
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            byte[] inBuf = this.DerEncode( numArray );
            return this.rsaEngine.ProcessBlock( inBuf, 0, inBuf.Length );
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            if (this.forSigning)
                throw new InvalidOperationException( "RsaDigestSigner not initialised for verification" );
            byte[] numArray1 = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray1, 0 );
            byte[] a;
            byte[] b;
            try
            {
                a = this.rsaEngine.ProcessBlock( signature, 0, signature.Length );
                b = this.DerEncode( numArray1 );
            }
            catch (Exception ex)
            {
                return false;
            }
            if (a.Length == b.Length)
                return Arrays.ConstantTimeAreEqual( a, b );
            if (a.Length != b.Length - 2)
                return false;
            int num1 = a.Length - numArray1.Length - 2;
            int num2 = b.Length - numArray1.Length - 2;
            byte[] numArray2;
            (numArray2 = b)[1] = (byte)(numArray2[1] - 2U);
            byte[] numArray3;
            (numArray3 = b)[3] = (byte)(numArray3[3] - 2U);
            int num3 = 0;
            for (int index = 0; index < numArray1.Length; ++index)
                num3 |= a[num1 + index] ^ b[num2 + index];
            for (int index = 0; index < num1; ++index)
                num3 |= a[index] ^ b[index];
            return num3 == 0;
        }

        public virtual void Reset() => this.digest.Reset();

        private byte[] DerEncode( byte[] hash ) => this.algId == null ? hash : new DigestInfo( this.algId, hash ).GetDerEncoded();
    }
}
