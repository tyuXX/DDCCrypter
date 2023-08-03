// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.PublicKeyPacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Date;
using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class PublicKeyPacket : ContainedPacket
    {
        private int version;
        private long time;
        private int validDays;
        private PublicKeyAlgorithmTag algorithm;
        private IBcpgKey key;

        internal PublicKeyPacket( BcpgInputStream bcpgIn )
        {
            this.version = bcpgIn.ReadByte();
            this.time = (uint)((bcpgIn.ReadByte() << 24) | (bcpgIn.ReadByte() << 16) | (bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte());
            if (this.version <= 3)
                this.validDays = (bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte();
            this.algorithm = (PublicKeyAlgorithmTag)bcpgIn.ReadByte();
            switch (this.algorithm)
            {
                case PublicKeyAlgorithmTag.RsaGeneral:
                case PublicKeyAlgorithmTag.RsaEncrypt:
                case PublicKeyAlgorithmTag.RsaSign:
                    this.key = new RsaPublicBcpgKey( bcpgIn );
                    break;
                case PublicKeyAlgorithmTag.ElGamalEncrypt:
                case PublicKeyAlgorithmTag.ElGamalGeneral:
                    this.key = new ElGamalPublicBcpgKey( bcpgIn );
                    break;
                case PublicKeyAlgorithmTag.Dsa:
                    this.key = new DsaPublicBcpgKey( bcpgIn );
                    break;
                case PublicKeyAlgorithmTag.EC:
                    this.key = new ECDHPublicBcpgKey( bcpgIn );
                    break;
                case PublicKeyAlgorithmTag.ECDsa:
                    this.key = new ECDsaPublicBcpgKey( bcpgIn );
                    break;
                default:
                    throw new IOException( "unknown PGP public key algorithm encountered" );
            }
        }

        public PublicKeyPacket( PublicKeyAlgorithmTag algorithm, DateTime time, IBcpgKey key )
        {
            this.version = 4;
            this.time = DateTimeUtilities.DateTimeToUnixMs( time ) / 1000L;
            this.algorithm = algorithm;
            this.key = key;
        }

        public virtual int Version => this.version;

        public virtual PublicKeyAlgorithmTag Algorithm => this.algorithm;

        public virtual int ValidDays => this.validDays;

        public virtual DateTime GetTime() => DateTimeUtilities.UnixMsToDateTime( this.time * 1000L );

        public virtual IBcpgKey Key => this.key;

        public virtual byte[] GetEncodedContents()
        {
            MemoryStream outStr = new MemoryStream();
            BcpgOutputStream bcpgOutputStream = new BcpgOutputStream( outStr );
            bcpgOutputStream.WriteByte( (byte)this.version );
            bcpgOutputStream.WriteInt( (int)this.time );
            if (this.version <= 3)
                bcpgOutputStream.WriteShort( (short)this.validDays );
            bcpgOutputStream.WriteByte( (byte)this.algorithm );
            bcpgOutputStream.WriteObject( (BcpgObject)this.key );
            return outStr.ToArray();
        }

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WritePacket( PacketTag.PublicKey, this.GetEncodedContents(), true );
    }
}
