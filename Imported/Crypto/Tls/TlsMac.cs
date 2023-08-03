// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsMac
    {
        protected readonly TlsContext context;
        protected readonly byte[] secret;
        protected readonly IMac mac;
        protected readonly int digestBlockSize;
        protected readonly int digestOverhead;
        protected readonly int macLength;

        public TlsMac( TlsContext context, IDigest digest, byte[] key, int keyOff, int keyLen )
        {
            this.context = context;
            KeyParameter parameters = new KeyParameter( key, keyOff, keyLen );
            this.secret = Arrays.Clone( parameters.GetKey() );
            if (digest is LongDigest)
            {
                this.digestBlockSize = 128;
                this.digestOverhead = 16;
            }
            else
            {
                this.digestBlockSize = 64;
                this.digestOverhead = 8;
            }
            if (TlsUtilities.IsSsl( context ))
            {
                this.mac = new Ssl3Mac( digest );
                if (digest.GetDigestSize() == 20)
                    this.digestOverhead = 4;
            }
            else
                this.mac = new HMac( digest );
            this.mac.Init( parameters );
            this.macLength = this.mac.GetMacSize();
            if (!context.SecurityParameters.truncatedHMac)
                return;
            this.macLength = System.Math.Min( this.macLength, 10 );
        }

        public virtual byte[] MacSecret => this.secret;

        public virtual int Size => this.macLength;

        public virtual byte[] CalculateMac(
          long seqNo,
          byte type,
          byte[] message,
          int offset,
          int length )
        {
            ProtocolVersion serverVersion = this.context.ServerVersion;
            bool isSsl = serverVersion.IsSsl;
            byte[] numArray = new byte[isSsl ? 11 : 13];
            TlsUtilities.WriteUint64( seqNo, numArray, 0 );
            TlsUtilities.WriteUint8( type, numArray, 8 );
            if (!isSsl)
                TlsUtilities.WriteVersion( serverVersion, numArray, 9 );
            TlsUtilities.WriteUint16( length, numArray, numArray.Length - 2 );
            this.mac.BlockUpdate( numArray, 0, numArray.Length );
            this.mac.BlockUpdate( message, offset, length );
            return this.Truncate( MacUtilities.DoFinal( this.mac ) );
        }

        public virtual byte[] CalculateMacConstantTime(
          long seqNo,
          byte type,
          byte[] message,
          int offset,
          int length,
          int fullLength,
          byte[] dummyData )
        {
            byte[] mac = this.CalculateMac( seqNo, type, message, offset, length );
            int num1 = TlsUtilities.IsSsl( this.context ) ? 11 : 13;
            int num2 = this.GetDigestBlockCount( num1 + fullLength ) - this.GetDigestBlockCount( num1 + length );
            while (--num2 >= 0)
                this.mac.BlockUpdate( dummyData, 0, this.digestBlockSize );
            this.mac.Update( dummyData[0] );
            this.mac.Reset();
            return mac;
        }

        protected virtual int GetDigestBlockCount( int inputLength ) => (inputLength + this.digestOverhead) / this.digestBlockSize;

        protected virtual byte[] Truncate( byte[] bs ) => bs.Length <= this.macLength ? bs : Arrays.CopyOf( bs, this.macLength );
    }
}
