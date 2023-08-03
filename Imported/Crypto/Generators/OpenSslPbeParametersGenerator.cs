// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.OpenSslPbeParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class OpenSslPbeParametersGenerator : PbeParametersGenerator
    {
        private readonly IDigest digest = new MD5Digest();

        public override void Init( byte[] password, byte[] salt, int iterationCount ) => base.Init( password, salt, 1 );

        public virtual void Init( byte[] password, byte[] salt ) => base.Init( password, salt, 1 );

        private byte[] GenerateDerivedKey( int bytesNeeded )
        {
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            byte[] destinationArray = new byte[bytesNeeded];
            int destinationIndex = 0;
            while (true)
            {
                this.digest.BlockUpdate( this.mPassword, 0, this.mPassword.Length );
                this.digest.BlockUpdate( this.mSalt, 0, this.mSalt.Length );
                this.digest.DoFinal( numArray, 0 );
                int length = bytesNeeded > numArray.Length ? numArray.Length : bytesNeeded;
                Array.Copy( numArray, 0, destinationArray, destinationIndex, length );
                destinationIndex += length;
                bytesNeeded -= length;
                if (bytesNeeded != 0)
                {
                    this.digest.Reset();
                    this.digest.BlockUpdate( numArray, 0, numArray.Length );
                }
                else
                    break;
            }
            return destinationArray;
        }

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public override ICipherParameters GenerateDerivedParameters( int keySize ) => this.GenerateDerivedMacParameters( keySize );

        public override ICipherParameters GenerateDerivedParameters( string algorithm, int keySize )
        {
            keySize /= 8;
            byte[] derivedKey = this.GenerateDerivedKey( keySize );
            return ParameterUtilities.CreateKeyParameter( algorithm, derivedKey, 0, keySize );
        }

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public override ICipherParameters GenerateDerivedParameters( int keySize, int ivSize )
        {
            keySize /= 8;
            ivSize /= 8;
            byte[] derivedKey = this.GenerateDerivedKey( keySize + ivSize );
            return new ParametersWithIV( new KeyParameter( derivedKey, 0, keySize ), derivedKey, keySize, ivSize );
        }

        public override ICipherParameters GenerateDerivedParameters(
          string algorithm,
          int keySize,
          int ivSize )
        {
            keySize /= 8;
            ivSize /= 8;
            byte[] derivedKey = this.GenerateDerivedKey( keySize + ivSize );
            return new ParametersWithIV( ParameterUtilities.CreateKeyParameter( algorithm, derivedKey, 0, keySize ), derivedKey, keySize, ivSize );
        }

        public override ICipherParameters GenerateDerivedMacParameters( int keySize )
        {
            keySize /= 8;
            return new KeyParameter( this.GenerateDerivedKey( keySize ), 0, keySize );
        }
    }
}
