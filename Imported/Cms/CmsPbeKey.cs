// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsPbeKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Cms
{
    public abstract class CmsPbeKey : ICipherParameters
    {
        internal readonly char[] password;
        internal readonly byte[] salt;
        internal readonly int iterationCount;

        [Obsolete( "Use version taking 'char[]' instead" )]
        public CmsPbeKey( string password, byte[] salt, int iterationCount )
          : this( password.ToCharArray(), salt, iterationCount )
        {
        }

        [Obsolete( "Use version taking 'char[]' instead" )]
        public CmsPbeKey( string password, AlgorithmIdentifier keyDerivationAlgorithm )
          : this( password.ToCharArray(), keyDerivationAlgorithm )
        {
        }

        public CmsPbeKey( char[] password, byte[] salt, int iterationCount )
        {
            this.password = (char[])password.Clone();
            this.salt = Arrays.Clone( salt );
            this.iterationCount = iterationCount;
        }

        public CmsPbeKey( char[] password, AlgorithmIdentifier keyDerivationAlgorithm )
        {
            if (!keyDerivationAlgorithm.Algorithm.Equals( PkcsObjectIdentifiers.IdPbkdf2 ))
                throw new ArgumentException( "Unsupported key derivation algorithm: " + keyDerivationAlgorithm.Algorithm );
            Pbkdf2Params instance = Pbkdf2Params.GetInstance( keyDerivationAlgorithm.Parameters.ToAsn1Object() );
            this.password = (char[])password.Clone();
            this.salt = instance.GetSalt();
            this.iterationCount = instance.IterationCount.IntValue;
        }

        ~CmsPbeKey() => Array.Clear( password, 0, this.password.Length );

        [Obsolete( "Will be removed" )]
        public string Password => new string( this.password );

        public byte[] Salt => Arrays.Clone( this.salt );

        [Obsolete( "Use 'Salt' property instead" )]
        public byte[] GetSalt() => this.Salt;

        public int IterationCount => this.iterationCount;

        public string Algorithm => "PKCS5S2";

        public string Format => "RAW";

        public byte[] GetEncoded() => null;

        internal abstract KeyParameter GetEncoded( string algorithmOid );
    }
}
