// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Pkcs5S2ParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Pkcs5S2ParametersGenerator : PbeParametersGenerator
    {
        private readonly IMac hMac;
        private readonly byte[] state;

        public Pkcs5S2ParametersGenerator()
          : this( new Sha1Digest() )
        {
        }

        public Pkcs5S2ParametersGenerator( IDigest digest )
        {
            this.hMac = new HMac( digest );
            this.state = new byte[this.hMac.GetMacSize()];
        }

        private void F( byte[] S, int c, byte[] iBuf, byte[] outBytes, int outOff )
        {
            if (c == 0)
                throw new ArgumentException( "iteration count must be at least 1." );
            if (S != null)
                this.hMac.BlockUpdate( S, 0, S.Length );
            this.hMac.BlockUpdate( iBuf, 0, iBuf.Length );
            this.hMac.DoFinal( this.state, 0 );
            Array.Copy( state, 0, outBytes, outOff, this.state.Length );
            for (int index1 = 1; index1 < c; ++index1)
            {
                this.hMac.BlockUpdate( this.state, 0, this.state.Length );
                this.hMac.DoFinal( this.state, 0 );
                for (int index2 = 0; index2 < this.state.Length; ++index2)
                {
                    byte[] numArray;
                    IntPtr index3;
                    (numArray = outBytes)[(int)(index3 = (IntPtr)(outOff + index2))] = (byte)(numArray[(int)index3] ^ (uint)this.state[index2]);
                }
            }
        }

        private byte[] GenerateDerivedKey( int dkLen )
        {
            int macSize = this.hMac.GetMacSize();
            int num1 = (dkLen + macSize - 1) / macSize;
            byte[] iBuf = new byte[4];
            byte[] outBytes = new byte[num1 * macSize];
            int outOff = 0;
            this.hMac.Init( new KeyParameter( this.mPassword ) );
            for (int index1 = 1; index1 <= num1; ++index1)
            {
                int num2 = 3;
                byte[] numArray;
                IntPtr index2;
                while (((numArray = iBuf)[(int)(index2 = (IntPtr)num2)] = (byte)(numArray[(int)index2] + 1U)) == 0)
                    --num2;
                this.F( this.mSalt, this.mIterationCount, iBuf, outBytes, outOff );
                outOff += macSize;
            }
            return outBytes;
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
