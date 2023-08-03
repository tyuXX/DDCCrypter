// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.PublicKeyEncSessionPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class PublicKeyEncSessionPacket : ContainedPacket
    {
        private int version;
        private long keyId;
        private PublicKeyAlgorithmTag algorithm;
        private byte[][] data;

        internal PublicKeyEncSessionPacket( BcpgInputStream bcpgIn )
        {
            this.version = bcpgIn.ReadByte();
            this.keyId |= (long)bcpgIn.ReadByte() << 56;
            this.keyId |= (long)bcpgIn.ReadByte() << 48;
            this.keyId |= (long)bcpgIn.ReadByte() << 40;
            this.keyId |= (long)bcpgIn.ReadByte() << 32;
            this.keyId |= (long)bcpgIn.ReadByte() << 24;
            this.keyId |= (long)bcpgIn.ReadByte() << 16;
            this.keyId |= (long)bcpgIn.ReadByte() << 8;
            this.keyId |= (uint)bcpgIn.ReadByte();
            this.algorithm = (PublicKeyAlgorithmTag)bcpgIn.ReadByte();
            switch (this.algorithm)
            {
                case PublicKeyAlgorithmTag.RsaGeneral:
                case PublicKeyAlgorithmTag.RsaEncrypt:
                    this.data = new byte[1][]
                    {
            new MPInteger(bcpgIn).GetEncoded()
                    };
                    break;
                case PublicKeyAlgorithmTag.ElGamalEncrypt:
                case PublicKeyAlgorithmTag.ElGamalGeneral:
                    MPInteger mpInteger1 = new( bcpgIn );
                    MPInteger mpInteger2 = new( bcpgIn );
                    this.data = new byte[2][]
                    {
            mpInteger1.GetEncoded(),
            mpInteger2.GetEncoded()
                    };
                    break;
                case PublicKeyAlgorithmTag.EC:
                    this.data = new byte[1][]
                    {
            Streams.ReadAll( bcpgIn)
                    };
                    break;
                default:
                    throw new IOException( "unknown PGP public key algorithm encountered" );
            }
        }

        public PublicKeyEncSessionPacket( long keyId, PublicKeyAlgorithmTag algorithm, byte[][] data )
        {
            this.version = 3;
            this.keyId = keyId;
            this.algorithm = algorithm;
            this.data = new byte[data.Length][];
            for (int index = 0; index < data.Length; ++index)
                this.data[index] = Arrays.Clone( data[index] );
        }

        public int Version => this.version;

        public long KeyId => this.keyId;

        public PublicKeyAlgorithmTag Algorithm => this.algorithm;

        public byte[][] GetEncSessionKey() => this.data;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            MemoryStream outStr = new();
            BcpgOutputStream s = new( outStr );
            s.WriteByte( (byte)this.version );
            s.WriteLong( this.keyId );
            s.WriteByte( (byte)this.algorithm );
            for (int index = 0; index < this.data.Length; ++index)
                s.Write( this.data[index] );
            Platform.Dispose( s );
            bcpgOut.WritePacket( PacketTag.PublicKeyEncryptedSession, outStr.ToArray(), true );
        }
    }
}
