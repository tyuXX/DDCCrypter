// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.Ssl3Mac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class Ssl3Mac : IMac
    {
        private const byte IPAD_BYTE = 54;
        private const byte OPAD_BYTE = 92;
        internal static readonly byte[] IPAD = GenPad( 54, 48 );
        internal static readonly byte[] OPAD = GenPad( 92, 48 );
        private readonly IDigest digest;
        private readonly int padLength;
        private byte[] secret;

        public Ssl3Mac( IDigest digest )
        {
            this.digest = digest;
            if (digest.GetDigestSize() == 20)
                this.padLength = 40;
            else
                this.padLength = 48;
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "/SSL3MAC";

        public virtual void Init( ICipherParameters parameters )
        {
            this.secret = Arrays.Clone( ((KeyParameter)parameters).GetKey() );
            this.Reset();
        }

        public virtual int GetMacSize() => this.digest.GetDigestSize();

        public virtual void Update( byte input ) => this.digest.Update( input );

        public virtual void BlockUpdate( byte[] input, int inOff, int len ) => this.digest.BlockUpdate( input, inOff, len );

        public virtual int DoFinal( byte[] output, int outOff )
        {
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            this.digest.DoFinal( numArray, 0 );
            this.digest.BlockUpdate( this.secret, 0, this.secret.Length );
            this.digest.BlockUpdate( OPAD, 0, this.padLength );
            this.digest.BlockUpdate( numArray, 0, numArray.Length );
            int num = this.digest.DoFinal( output, outOff );
            this.Reset();
            return num;
        }

        public virtual void Reset()
        {
            this.digest.Reset();
            this.digest.BlockUpdate( this.secret, 0, this.secret.Length );
            this.digest.BlockUpdate( IPAD, 0, this.padLength );
        }

        private static byte[] GenPad( byte b, int count )
        {
            byte[] buf = new byte[count];
            Arrays.Fill( buf, b );
            return buf;
        }
    }
}
