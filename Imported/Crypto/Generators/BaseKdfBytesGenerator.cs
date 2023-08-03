// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.BaseKdfBytesGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class BaseKdfBytesGenerator : IDerivationFunction
    {
        private int counterStart;
        private IDigest digest;
        private byte[] shared;
        private byte[] iv;

        public BaseKdfBytesGenerator( int counterStart, IDigest digest )
        {
            this.counterStart = counterStart;
            this.digest = digest;
        }

        public virtual void Init( IDerivationParameters parameters )
        {
            switch (parameters)
            {
                case KdfParameters _:
                    KdfParameters kdfParameters = (KdfParameters)parameters;
                    this.shared = kdfParameters.GetSharedSecret();
                    this.iv = kdfParameters.GetIV();
                    break;
                case Iso18033KdfParameters _:
                    this.shared = ((Iso18033KdfParameters)parameters).GetSeed();
                    this.iv = null;
                    break;
                default:
                    throw new ArgumentException( "KDF parameters required for KDF Generator" );
            }
        }

        public virtual IDigest Digest => this.digest;

        public virtual int GenerateBytes( byte[] output, int outOff, int length )
        {
            if (output.Length - length < outOff)
                throw new DataLengthException( "output buffer too small" );
            long bytes = length;
            int digestSize = this.digest.GetDigestSize();
            if (bytes > 8589934591L)
                throw new ArgumentException( "Output length too large" );
            int num = (int)((bytes + digestSize - 1L) / digestSize);
            byte[] numArray1 = new byte[this.digest.GetDigestSize()];
            byte[] numArray2 = new byte[4];
            Pack.UInt32_To_BE( (uint)this.counterStart, numArray2, 0 );
            uint n = (uint)(this.counterStart & -256);
            for (int index = 0; index < num; ++index)
            {
                this.digest.BlockUpdate( this.shared, 0, this.shared.Length );
                this.digest.BlockUpdate( numArray2, 0, 4 );
                if (this.iv != null)
                    this.digest.BlockUpdate( this.iv, 0, this.iv.Length );
                this.digest.DoFinal( numArray1, 0 );
                if (length > digestSize)
                {
                    Array.Copy( numArray1, 0, output, outOff, digestSize );
                    outOff += digestSize;
                    length -= digestSize;
                }
                else
                    Array.Copy( numArray1, 0, output, outOff, length );
                byte[] numArray3;
                if (((numArray3 = numArray2)[3] = (byte)(numArray3[3] + 1U)) == 0)
                {
                    n += 256U;
                    Pack.UInt32_To_BE( n, numArray2, 0 );
                }
            }
            this.digest.Reset();
            return (int)bytes;
        }
    }
}
