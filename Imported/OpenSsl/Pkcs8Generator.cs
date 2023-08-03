// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.OpenSsl.Pkcs8Generator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using System;

namespace Org.BouncyCastle.OpenSsl
{
    public class Pkcs8Generator : PemObjectGenerator
    {
        public static readonly string PbeSha1_RC4_128 = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4.Id;
        public static readonly string PbeSha1_RC4_40 = PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4.Id;
        public static readonly string PbeSha1_3DES = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id;
        public static readonly string PbeSha1_2DES = PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc.Id;
        public static readonly string PbeSha1_RC2_128 = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc.Id;
        public static readonly string PbeSha1_RC2_40 = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc.Id;
        private char[] password;
        private string algorithm;
        private int iterationCount;
        private AsymmetricKeyParameter privKey;
        private SecureRandom random;

        public Pkcs8Generator( AsymmetricKeyParameter privKey ) => this.privKey = privKey;

        public Pkcs8Generator( AsymmetricKeyParameter privKey, string algorithm )
        {
            this.privKey = privKey;
            this.algorithm = algorithm;
            this.iterationCount = 2048;
        }

        public SecureRandom SecureRandom
        {
            set => this.random = value;
        }

        public char[] Password
        {
            set => this.password = value;
        }

        public int IterationCount
        {
            set => this.iterationCount = value;
        }

        public PemObject Generate()
        {
            if (this.algorithm == null)
                return new PemObject( "PRIVATE KEY", PrivateKeyInfoFactory.CreatePrivateKeyInfo( this.privKey ).GetEncoded() );
            byte[] numArray = new byte[20];
            if (this.random == null)
                this.random = new SecureRandom();
            this.random.NextBytes( numArray );
            try
            {
                return new PemObject( "ENCRYPTED PRIVATE KEY", EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo( this.algorithm, this.password, numArray, this.iterationCount, this.privKey ).GetEncoded() );
            }
            catch (Exception ex)
            {
                throw new PemGenerationException( "Couldn't encrypt private key", ex );
            }
        }
    }
}
