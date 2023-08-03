// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Pkcs5S1ParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Pkcs5S1ParametersGenerator : PbeParametersGenerator
    {
        private readonly IDigest digest;

        public Pkcs5S1ParametersGenerator( IDigest digest ) => this.digest = digest;

        private byte[] GenerateDerivedKey()
        {
            byte[] derivedKey = new byte[this.digest.GetDigestSize()];
            this.digest.BlockUpdate( this.mPassword, 0, this.mPassword.Length );
            this.digest.BlockUpdate( this.mSalt, 0, this.mSalt.Length );
            this.digest.DoFinal( derivedKey, 0 );
            for (int index = 1; index < this.mIterationCount; ++index)
            {
                this.digest.BlockUpdate( derivedKey, 0, derivedKey.Length );
                this.digest.DoFinal( derivedKey, 0 );
            }
            return derivedKey;
        }

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public override ICipherParameters GenerateDerivedParameters( int keySize ) => this.GenerateDerivedMacParameters( keySize );

        public override ICipherParameters GenerateDerivedParameters( string algorithm, int keySize )
        {
            keySize /= 8;
            if (keySize > this.digest.GetDigestSize())
                throw new ArgumentException( "Can't Generate a derived key " + keySize + " bytes long." );
            byte[] derivedKey = this.GenerateDerivedKey();
            return ParameterUtilities.CreateKeyParameter( algorithm, derivedKey, 0, keySize );
        }

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public override ICipherParameters GenerateDerivedParameters( int keySize, int ivSize )
        {
            keySize /= 8;
            ivSize /= 8;
            if (keySize + ivSize > this.digest.GetDigestSize())
                throw new ArgumentException( "Can't Generate a derived key " + (keySize + ivSize) + " bytes long." );
            byte[] derivedKey = this.GenerateDerivedKey();
            return new ParametersWithIV( new KeyParameter( derivedKey, 0, keySize ), derivedKey, keySize, ivSize );
        }

        public override ICipherParameters GenerateDerivedParameters(
          string algorithm,
          int keySize,
          int ivSize )
        {
            keySize /= 8;
            ivSize /= 8;
            if (keySize + ivSize > this.digest.GetDigestSize())
                throw new ArgumentException( "Can't Generate a derived key " + (keySize + ivSize) + " bytes long." );
            byte[] derivedKey = this.GenerateDerivedKey();
            return new ParametersWithIV( ParameterUtilities.CreateKeyParameter( algorithm, derivedKey, 0, keySize ), derivedKey, keySize, ivSize );
        }

        public override ICipherParameters GenerateDerivedMacParameters( int keySize )
        {
            keySize /= 8;
            return keySize <= this.digest.GetDigestSize() ? (ICipherParameters)new KeyParameter( this.GenerateDerivedKey(), 0, keySize ) : throw new ArgumentException( "Can't Generate a derived key " + keySize + " bytes long." );
        }
    }
}
