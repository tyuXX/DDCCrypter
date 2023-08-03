// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.Pkcs5Scheme2PbeKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Cms
{
    public class Pkcs5Scheme2PbeKey : CmsPbeKey
    {
        [Obsolete( "Use version taking 'char[]' instead" )]
        public Pkcs5Scheme2PbeKey( string password, byte[] salt, int iterationCount )
          : this( password.ToCharArray(), salt, iterationCount )
        {
        }

        [Obsolete( "Use version taking 'char[]' instead" )]
        public Pkcs5Scheme2PbeKey( string password, AlgorithmIdentifier keyDerivationAlgorithm )
          : this( password.ToCharArray(), keyDerivationAlgorithm )
        {
        }

        public Pkcs5Scheme2PbeKey( char[] password, byte[] salt, int iterationCount )
          : base( password, salt, iterationCount )
        {
        }

        public Pkcs5Scheme2PbeKey( char[] password, AlgorithmIdentifier keyDerivationAlgorithm )
          : base( password, keyDerivationAlgorithm )
        {
        }

        internal override KeyParameter GetEncoded( string algorithmOid )
        {
            Pkcs5S2ParametersGenerator parametersGenerator = new Pkcs5S2ParametersGenerator();
            parametersGenerator.Init( PbeParametersGenerator.Pkcs5PasswordToBytes( this.password ), this.salt, this.iterationCount );
            return (KeyParameter)parametersGenerator.GenerateDerivedParameters( algorithmOid, CmsEnvelopedHelper.Instance.GetKeySize( algorithmOid ) );
        }
    }
}
