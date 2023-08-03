// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SignaturePacket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Date;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class SignaturePacket : ContainedPacket
    {
        private int version;
        private int signatureType;
        private long creationTime;
        private long keyId;
        private PublicKeyAlgorithmTag keyAlgorithm;
        private HashAlgorithmTag hashAlgorithm;
        private MPInteger[] signature;
        private byte[] fingerprint;
        private SignatureSubpacket[] hashedData;
        private SignatureSubpacket[] unhashedData;
        private byte[] signatureEncoding;

        internal SignaturePacket( BcpgInputStream bcpgIn )
        {
            this.version = bcpgIn.ReadByte();
            if (this.version == 3 || this.version == 2)
            {
                bcpgIn.ReadByte();
                this.signatureType = bcpgIn.ReadByte();
                this.creationTime = (((long)bcpgIn.ReadByte() << 24) | ((long)bcpgIn.ReadByte() << 16) | ((long)bcpgIn.ReadByte() << 8) | (uint)bcpgIn.ReadByte()) * 1000L;
                this.keyId |= (long)bcpgIn.ReadByte() << 56;
                this.keyId |= (long)bcpgIn.ReadByte() << 48;
                this.keyId |= (long)bcpgIn.ReadByte() << 40;
                this.keyId |= (long)bcpgIn.ReadByte() << 32;
                this.keyId |= (long)bcpgIn.ReadByte() << 24;
                this.keyId |= (long)bcpgIn.ReadByte() << 16;
                this.keyId |= (long)bcpgIn.ReadByte() << 8;
                this.keyId |= (uint)bcpgIn.ReadByte();
                this.keyAlgorithm = (PublicKeyAlgorithmTag)bcpgIn.ReadByte();
                this.hashAlgorithm = (HashAlgorithmTag)bcpgIn.ReadByte();
            }
            else
            {
                if (this.version != 4)
                    throw new Exception( "unsupported version: " + version );
                this.signatureType = bcpgIn.ReadByte();
                this.keyAlgorithm = (PublicKeyAlgorithmTag)bcpgIn.ReadByte();
                this.hashAlgorithm = (HashAlgorithmTag)bcpgIn.ReadByte();
                byte[] buffer1 = new byte[(bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte()];
                bcpgIn.ReadFully( buffer1 );
                SignatureSubpacketsParser subpacketsParser1 = new( new MemoryStream( buffer1, false ) );
                IList arrayList = Platform.CreateArrayList();
                SignatureSubpacket signatureSubpacket1;
                while ((signatureSubpacket1 = subpacketsParser1.ReadPacket()) != null)
                    arrayList.Add( signatureSubpacket1 );
                this.hashedData = new SignatureSubpacket[arrayList.Count];
                for (int index = 0; index != this.hashedData.Length; ++index)
                {
                    SignatureSubpacket signatureSubpacket2 = (SignatureSubpacket)arrayList[index];
                    switch (signatureSubpacket2)
                    {
                        case IssuerKeyId _:
                            this.keyId = ((IssuerKeyId)signatureSubpacket2).KeyId;
                            break;
                        case SignatureCreationTime _:
                            this.creationTime = DateTimeUtilities.DateTimeToUnixMs( ((SignatureCreationTime)signatureSubpacket2).GetTime() );
                            break;
                    }
                    this.hashedData[index] = signatureSubpacket2;
                }
                byte[] buffer2 = new byte[(bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte()];
                bcpgIn.ReadFully( buffer2 );
                SignatureSubpacketsParser subpacketsParser2 = new( new MemoryStream( buffer2, false ) );
                arrayList.Clear();
                SignatureSubpacket signatureSubpacket3;
                while ((signatureSubpacket3 = subpacketsParser2.ReadPacket()) != null)
                    arrayList.Add( signatureSubpacket3 );
                this.unhashedData = new SignatureSubpacket[arrayList.Count];
                for (int index = 0; index != this.unhashedData.Length; ++index)
                {
                    SignatureSubpacket signatureSubpacket4 = (SignatureSubpacket)arrayList[index];
                    if (signatureSubpacket4 is IssuerKeyId)
                        this.keyId = ((IssuerKeyId)signatureSubpacket4).KeyId;
                    this.unhashedData[index] = signatureSubpacket4;
                }
            }
            this.fingerprint = new byte[2];
            bcpgIn.ReadFully( this.fingerprint );
            switch (this.keyAlgorithm)
            {
                case PublicKeyAlgorithmTag.RsaGeneral:
                case PublicKeyAlgorithmTag.RsaSign:
                    this.signature = new MPInteger[1]
                    {
            new MPInteger(bcpgIn)
                    };
                    break;
                case PublicKeyAlgorithmTag.ElGamalEncrypt:
                case PublicKeyAlgorithmTag.ElGamalGeneral:
                    this.signature = new MPInteger[3]
                    {
            new MPInteger(bcpgIn),
            new MPInteger(bcpgIn),
            new MPInteger(bcpgIn)
                    };
                    break;
                case PublicKeyAlgorithmTag.Dsa:
                    this.signature = new MPInteger[2]
                    {
            new MPInteger(bcpgIn),
            new MPInteger(bcpgIn)
                    };
                    break;
                case PublicKeyAlgorithmTag.ECDsa:
                    this.signature = new MPInteger[2]
                    {
            new MPInteger(bcpgIn),
            new MPInteger(bcpgIn)
                    };
                    break;
                default:
                    if (this.keyAlgorithm < PublicKeyAlgorithmTag.Experimental_1 || this.keyAlgorithm > PublicKeyAlgorithmTag.Experimental_11)
                        throw new IOException( "unknown signature key algorithm: " + keyAlgorithm );
                    this.signature = null;
                    MemoryStream memoryStream = new();
                    int num;
                    while ((num = bcpgIn.ReadByte()) >= 0)
                        memoryStream.WriteByte( (byte)num );
                    this.signatureEncoding = memoryStream.ToArray();
                    break;
            }
        }

        public SignaturePacket(
          int signatureType,
          long keyId,
          PublicKeyAlgorithmTag keyAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          SignatureSubpacket[] hashedData,
          SignatureSubpacket[] unhashedData,
          byte[] fingerprint,
          MPInteger[] signature )
          : this( 4, signatureType, keyId, keyAlgorithm, hashAlgorithm, hashedData, unhashedData, fingerprint, signature )
        {
        }

        public SignaturePacket(
          int version,
          int signatureType,
          long keyId,
          PublicKeyAlgorithmTag keyAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          long creationTime,
          byte[] fingerprint,
          MPInteger[] signature )
          : this( version, signatureType, keyId, keyAlgorithm, hashAlgorithm, null, null, fingerprint, signature )
        {
            this.creationTime = creationTime;
        }

        public SignaturePacket(
          int version,
          int signatureType,
          long keyId,
          PublicKeyAlgorithmTag keyAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          SignatureSubpacket[] hashedData,
          SignatureSubpacket[] unhashedData,
          byte[] fingerprint,
          MPInteger[] signature )
        {
            this.version = version;
            this.signatureType = signatureType;
            this.keyId = keyId;
            this.keyAlgorithm = keyAlgorithm;
            this.hashAlgorithm = hashAlgorithm;
            this.hashedData = hashedData;
            this.unhashedData = unhashedData;
            this.fingerprint = fingerprint;
            this.signature = signature;
            if (hashedData == null)
                return;
            this.setCreationTime();
        }

        public int Version => this.version;

        public int SignatureType => this.signatureType;

        public long KeyId => this.keyId;

        public byte[] GetSignatureTrailer()
        {
            byte[] signatureTrailer;
            if (this.version == 3)
            {
                signatureTrailer = new byte[5];
                long num = this.creationTime / 1000L;
                signatureTrailer[0] = (byte)this.signatureType;
                signatureTrailer[1] = (byte)(num >> 24);
                signatureTrailer[2] = (byte)(num >> 16);
                signatureTrailer[3] = (byte)(num >> 8);
                signatureTrailer[4] = (byte)num;
            }
            else
            {
                MemoryStream memoryStream = new();
                memoryStream.WriteByte( (byte)this.Version );
                memoryStream.WriteByte( (byte)this.SignatureType );
                memoryStream.WriteByte( (byte)this.KeyAlgorithm );
                memoryStream.WriteByte( (byte)this.HashAlgorithm );
                MemoryStream os = new();
                SignatureSubpacket[] hashedSubPackets = this.GetHashedSubPackets();
                for (int index = 0; index != hashedSubPackets.Length; ++index)
                    hashedSubPackets[index].Encode( os );
                byte[] array1 = os.ToArray();
                memoryStream.WriteByte( (byte)(array1.Length >> 8) );
                memoryStream.WriteByte( (byte)array1.Length );
                memoryStream.Write( array1, 0, array1.Length );
                byte[] array2 = memoryStream.ToArray();
                memoryStream.WriteByte( (byte)this.Version );
                memoryStream.WriteByte( byte.MaxValue );
                memoryStream.WriteByte( (byte)(array2.Length >> 24) );
                memoryStream.WriteByte( (byte)(array2.Length >> 16) );
                memoryStream.WriteByte( (byte)(array2.Length >> 8) );
                memoryStream.WriteByte( (byte)array2.Length );
                signatureTrailer = memoryStream.ToArray();
            }
            return signatureTrailer;
        }

        public PublicKeyAlgorithmTag KeyAlgorithm => this.keyAlgorithm;

        public HashAlgorithmTag HashAlgorithm => this.hashAlgorithm;

        public MPInteger[] GetSignature() => this.signature;

        public byte[] GetSignatureBytes()
        {
            if (this.signatureEncoding != null)
                return (byte[])this.signatureEncoding.Clone();
            MemoryStream outStr = new();
            BcpgOutputStream bcpgOutputStream = new( outStr );
            foreach (MPInteger mpInteger in this.signature)
            {
                try
                {
                    bcpgOutputStream.WriteObject( mpInteger );
                }
                catch (IOException ex)
                {
                    throw new Exception( "internal error: " + ex );
                }
            }
            return outStr.ToArray();
        }

        public SignatureSubpacket[] GetHashedSubPackets() => this.hashedData;

        public SignatureSubpacket[] GetUnhashedSubPackets() => this.unhashedData;

        public long CreationTime => this.creationTime;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            MemoryStream outStr = new();
            BcpgOutputStream pOut = new( outStr );
            pOut.WriteByte( (byte)this.version );
            if (this.version == 3 || this.version == 2)
            {
                pOut.Write( 5, (byte)this.signatureType );
                pOut.WriteInt( (int)(this.creationTime / 1000L) );
                pOut.WriteLong( this.keyId );
                pOut.Write( (byte)this.keyAlgorithm, (byte)this.hashAlgorithm );
            }
            else
            {
                if (this.version != 4)
                    throw new IOException( "unknown version: " + version );
                pOut.Write( (byte)this.signatureType, (byte)this.keyAlgorithm, (byte)this.hashAlgorithm );
                EncodeLengthAndData( pOut, GetEncodedSubpackets( this.hashedData ) );
                EncodeLengthAndData( pOut, GetEncodedSubpackets( this.unhashedData ) );
            }
            pOut.Write( this.fingerprint );
            if (this.signature != null)
                pOut.WriteObjects( signature );
            else
                pOut.Write( this.signatureEncoding );
            bcpgOut.WritePacket( PacketTag.Signature, outStr.ToArray(), true );
        }

        private static void EncodeLengthAndData( BcpgOutputStream pOut, byte[] data )
        {
            pOut.WriteShort( (short)data.Length );
            pOut.Write( data );
        }

        private static byte[] GetEncodedSubpackets( SignatureSubpacket[] ps )
        {
            MemoryStream os = new();
            foreach (SignatureSubpacket p in ps)
                p.Encode( os );
            return os.ToArray();
        }

        private void setCreationTime()
        {
            foreach (SignatureSubpacket signatureSubpacket in this.hashedData)
            {
                if (signatureSubpacket is SignatureCreationTime)
                {
                    this.creationTime = DateTimeUtilities.DateTimeToUnixMs( ((SignatureCreationTime)signatureSubpacket).GetTime() );
                    break;
                }
            }
        }
    }
}
