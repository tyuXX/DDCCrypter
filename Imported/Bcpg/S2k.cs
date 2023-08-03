// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.S2k
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class S2k : BcpgObject
    {
        private const int ExpBias = 6;
        public const int Simple = 0;
        public const int Salted = 1;
        public const int SaltedAndIterated = 3;
        public const int GnuDummyS2K = 101;
        public const int GnuProtectionModeNoPrivateKey = 1;
        public const int GnuProtectionModeDivertToCard = 2;
        internal int type;
        internal HashAlgorithmTag algorithm;
        internal byte[] iv;
        internal int itCount = -1;
        internal int protectionMode = -1;

        internal S2k( Stream inStr )
        {
            this.type = inStr.ReadByte();
            this.algorithm = (HashAlgorithmTag)inStr.ReadByte();
            if (this.type != 101)
            {
                if (this.type == 0)
                    return;
                this.iv = new byte[8];
                if (Streams.ReadFully( inStr, this.iv, 0, this.iv.Length ) < this.iv.Length)
                    throw new EndOfStreamException();
                if (this.type != 3)
                    return;
                this.itCount = inStr.ReadByte();
            }
            else
            {
                inStr.ReadByte();
                inStr.ReadByte();
                inStr.ReadByte();
                this.protectionMode = inStr.ReadByte();
            }
        }

        public S2k( HashAlgorithmTag algorithm )
        {
            this.type = 0;
            this.algorithm = algorithm;
        }

        public S2k( HashAlgorithmTag algorithm, byte[] iv )
        {
            this.type = 1;
            this.algorithm = algorithm;
            this.iv = iv;
        }

        public S2k( HashAlgorithmTag algorithm, byte[] iv, int itCount )
        {
            this.type = 3;
            this.algorithm = algorithm;
            this.iv = iv;
            this.itCount = itCount;
        }

        public virtual int Type => this.type;

        public virtual HashAlgorithmTag HashAlgorithm => this.algorithm;

        public virtual byte[] GetIV() => Arrays.Clone( this.iv );

        [Obsolete( "Use 'IterationCount' property instead" )]
        public long GetIterationCount() => this.IterationCount;

        public virtual long IterationCount => (16 + (this.itCount & 15)) << ((this.itCount >> 4) + 6);

        public virtual int ProtectionMode => this.protectionMode;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            bcpgOut.WriteByte( (byte)this.type );
            bcpgOut.WriteByte( (byte)this.algorithm );
            if (this.type != 101)
            {
                if (this.type != 0)
                    bcpgOut.Write( this.iv );
                if (this.type != 3)
                    return;
                bcpgOut.WriteByte( (byte)this.itCount );
            }
            else
            {
                bcpgOut.WriteByte( 71 );
                bcpgOut.WriteByte( 78 );
                bcpgOut.WriteByte( 85 );
                bcpgOut.WriteByte( (byte)this.protectionMode );
            }
        }
    }
}
