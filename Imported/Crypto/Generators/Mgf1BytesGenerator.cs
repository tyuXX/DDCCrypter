// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Mgf1BytesGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Mgf1BytesGenerator : IDerivationFunction
    {
        private IDigest digest;
        private byte[] seed;
        private int hLen;

        public Mgf1BytesGenerator( IDigest digest )
        {
            this.digest = digest;
            this.hLen = digest.GetDigestSize();
        }

        public void Init( IDerivationParameters parameters ) => this.seed = typeof( MgfParameters ).IsInstanceOfType( parameters ) ? ((MgfParameters)parameters).GetSeed() : throw new ArgumentException( "MGF parameters required for MGF1Generator" );

        public IDigest Digest => this.digest;

        private void ItoOSP( int i, byte[] sp )
        {
            sp[0] = (byte)(i >>> 24);
            sp[1] = (byte)(i >>> 16);
            sp[2] = (byte)(i >>> 8);
            sp[3] = (byte)i;
        }

        public int GenerateBytes( byte[] output, int outOff, int length )
        {
            if (output.Length - length < outOff)
                throw new DataLengthException( "output buffer too small" );
            byte[] numArray1 = new byte[this.hLen];
            byte[] numArray2 = new byte[4];
            int i = 0;
            this.digest.Reset();
            if (length > this.hLen)
            {
                do
                {
                    this.ItoOSP( i, numArray2 );
                    this.digest.BlockUpdate( this.seed, 0, this.seed.Length );
                    this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
                    this.digest.DoFinal( numArray1, 0 );
                    Array.Copy( numArray1, 0, output, outOff + (i * this.hLen), this.hLen );
                }
                while (++i < length / this.hLen);
            }
            if (i * this.hLen < length)
            {
                this.ItoOSP( i, numArray2 );
                this.digest.BlockUpdate( this.seed, 0, this.seed.Length );
                this.digest.BlockUpdate( numArray2, 0, numArray2.Length );
                this.digest.DoFinal( numArray1, 0 );
                Array.Copy( numArray1, 0, output, outOff + (i * this.hLen), length - (i * this.hLen) );
            }
            return length;
        }
    }
}
