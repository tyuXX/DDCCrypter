// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Pkcs12ParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Pkcs12ParametersGenerator : PbeParametersGenerator
    {
        public const int KeyMaterial = 1;
        public const int IVMaterial = 2;
        public const int MacMaterial = 3;
        private readonly IDigest digest;
        private readonly int u;
        private readonly int v;

        public Pkcs12ParametersGenerator( IDigest digest )
        {
            this.digest = digest;
            this.u = digest.GetDigestSize();
            this.v = digest.GetByteLength();
        }

        private void Adjust( byte[] a, int aOff, byte[] b )
        {
            int num1 = (b[b.Length - 1] & byte.MaxValue) + (a[aOff + b.Length - 1] & byte.MaxValue) + 1;
            a[aOff + b.Length - 1] = (byte)num1;
            int num2 = num1 >>> 8;
            for (int index = b.Length - 2; index >= 0; --index)
            {
                int num3 = num2 + (b[index] & byte.MaxValue) + (a[aOff + index] & byte.MaxValue);
                a[aOff + index] = (byte)num3;
                num2 = num3 >>> 8;
            }
        }

        private byte[] GenerateDerivedKey( int idByte, int n )
        {
            byte[] input = new byte[this.v];
            byte[] destinationArray = new byte[n];
            for (int index = 0; index != input.Length; ++index)
                input[index] = (byte)idByte;
            byte[] sourceArray1;
            if (this.mSalt != null && this.mSalt.Length != 0)
            {
                sourceArray1 = new byte[this.v * ((this.mSalt.Length + this.v - 1) / this.v)];
                for (int index = 0; index != sourceArray1.Length; ++index)
                    sourceArray1[index] = this.mSalt[index % this.mSalt.Length];
            }
            else
                sourceArray1 = new byte[0];
            byte[] sourceArray2;
            if (this.mPassword != null && this.mPassword.Length != 0)
            {
                sourceArray2 = new byte[this.v * ((this.mPassword.Length + this.v - 1) / this.v)];
                for (int index = 0; index != sourceArray2.Length; ++index)
                    sourceArray2[index] = this.mPassword[index % this.mPassword.Length];
            }
            else
                sourceArray2 = new byte[0];
            byte[] numArray1 = new byte[sourceArray1.Length + sourceArray2.Length];
            Array.Copy( sourceArray1, 0, numArray1, 0, sourceArray1.Length );
            Array.Copy( sourceArray2, 0, numArray1, sourceArray1.Length, sourceArray2.Length );
            byte[] b = new byte[this.v];
            int num = (n + this.u - 1) / this.u;
            byte[] numArray2 = new byte[this.u];
            for (int index1 = 1; index1 <= num; ++index1)
            {
                this.digest.BlockUpdate( input, 0, input.Length );
                this.digest.BlockUpdate( numArray1, 0, numArray1.Length );
                this.digest.DoFinal( numArray2, 0 );
                for (int index2 = 1; index2 != this.mIterationCount; ++index2)
                {
                    this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
                    this.digest.DoFinal( numArray2, 0 );
                }
                for (int index3 = 0; index3 != b.Length; ++index3)
                    b[index3] = numArray2[index3 % numArray2.Length];
                for (int index4 = 0; index4 != numArray1.Length / this.v; ++index4)
                    this.Adjust( numArray1, index4 * this.v, b );
                if (index1 == num)
                    Array.Copy( numArray2, 0, destinationArray, (index1 - 1) * this.u, destinationArray.Length - ((index1 - 1) * this.u) );
                else
                    Array.Copy( numArray2, 0, destinationArray, (index1 - 1) * this.u, numArray2.Length );
            }
            return destinationArray;
        }

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public override ICipherParameters GenerateDerivedParameters( int keySize )
        {
            keySize /= 8;
            return new KeyParameter( this.GenerateDerivedKey( 1, keySize ), 0, keySize );
        }

        public override ICipherParameters GenerateDerivedParameters( string algorithm, int keySize )
        {
            keySize /= 8;
            byte[] derivedKey = this.GenerateDerivedKey( 1, keySize );
            return ParameterUtilities.CreateKeyParameter( algorithm, derivedKey, 0, keySize );
        }

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public override ICipherParameters GenerateDerivedParameters( int keySize, int ivSize )
        {
            keySize /= 8;
            ivSize /= 8;
            byte[] derivedKey1 = this.GenerateDerivedKey( 1, keySize );
            byte[] derivedKey2 = this.GenerateDerivedKey( 2, ivSize );
            return new ParametersWithIV( new KeyParameter( derivedKey1, 0, keySize ), derivedKey2, 0, ivSize );
        }

        public override ICipherParameters GenerateDerivedParameters(
          string algorithm,
          int keySize,
          int ivSize )
        {
            keySize /= 8;
            ivSize /= 8;
            byte[] derivedKey = this.GenerateDerivedKey( 1, keySize );
            return new ParametersWithIV( ParameterUtilities.CreateKeyParameter( algorithm, derivedKey, 0, keySize ), this.GenerateDerivedKey( 2, ivSize ), 0, ivSize );
        }

        public override ICipherParameters GenerateDerivedMacParameters( int keySize )
        {
            keySize /= 8;
            return new KeyParameter( this.GenerateDerivedKey( 3, keySize ), 0, keySize );
        }
    }
}
