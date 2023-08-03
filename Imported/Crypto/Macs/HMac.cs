// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.HMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class HMac : IMac
    {
        private const byte IPAD = 54;
        private const byte OPAD = 92;
        private readonly IDigest digest;
        private readonly int digestSize;
        private readonly int blockLength;
        private IMemoable ipadState;
        private IMemoable opadState;
        private readonly byte[] inputPad;
        private readonly byte[] outputBuf;

        public HMac( IDigest digest )
        {
            this.digest = digest;
            this.digestSize = digest.GetDigestSize();
            this.blockLength = digest.GetByteLength();
            this.inputPad = new byte[this.blockLength];
            this.outputBuf = new byte[this.blockLength + this.digestSize];
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "/HMAC";

        public virtual IDigest GetUnderlyingDigest() => this.digest;

        public virtual void Init( ICipherParameters parameters )
        {
            this.digest.Reset();
            byte[] key = ((KeyParameter)parameters).GetKey();
            int num = key.Length;
            if (num > this.blockLength)
            {
                this.digest.BlockUpdate( key, 0, num );
                this.digest.DoFinal( this.inputPad, 0 );
                num = this.digestSize;
            }
            else
                Array.Copy( key, 0, inputPad, 0, num );
            Array.Clear( inputPad, num, this.blockLength - num );
            Array.Copy( inputPad, 0, outputBuf, 0, this.blockLength );
            XorPad( this.inputPad, this.blockLength, 54 );
            XorPad( this.outputBuf, this.blockLength, 92 );
            if (this.digest is IMemoable)
            {
                this.opadState = ((IMemoable)this.digest).Copy();
                ((IDigest)this.opadState).BlockUpdate( this.outputBuf, 0, this.blockLength );
            }
            this.digest.BlockUpdate( this.inputPad, 0, this.inputPad.Length );
            if (!(this.digest is IMemoable))
                return;
            this.ipadState = ((IMemoable)this.digest).Copy();
        }

        public virtual int GetMacSize() => this.digestSize;

        public virtual void Update( byte input ) => this.digest.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int len ) => this.digest.BlockUpdate( input, inOff, len );

        public virtual int DoFinal( byte[] output, int outOff )
        {
            this.digest.DoFinal( this.outputBuf, this.blockLength );
            if (this.opadState != null)
            {
                ((IMemoable)this.digest).Reset( this.opadState );
                this.digest.BlockUpdate( this.outputBuf, this.blockLength, this.digest.GetDigestSize() );
            }
            else
                this.digest.BlockUpdate( this.outputBuf, 0, this.outputBuf.Length );
            int num = this.digest.DoFinal( output, outOff );
            Array.Clear( outputBuf, this.blockLength, this.digestSize );
            if (this.ipadState != null)
                ((IMemoable)this.digest).Reset( this.ipadState );
            else
                this.digest.BlockUpdate( this.inputPad, 0, this.inputPad.Length );
            return num;
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.digest.BlockUpdate( this.inputPad, 0, this.inputPad.Length );
        }

        private static void XorPad( byte[] pad, int len, byte n )
        {
            for (int index1 = 0; index1 < len; ++index1)
            {
                byte[] numArray;
                IntPtr index2;
                (numArray = pad)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray[(int)index2] ^ (uint)n);
            }
        }
    }
}
