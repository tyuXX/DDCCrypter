// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSignatureGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSignatureGenerator
    {
        private static readonly SignatureSubpacket[] EmptySignatureSubpackets = new SignatureSubpacket[0];
        private PublicKeyAlgorithmTag keyAlgorithm;
        private HashAlgorithmTag hashAlgorithm;
        private PgpPrivateKey privKey;
        private ISigner sig;
        private IDigest dig;
        private int signatureType;
        private byte lastb;
        private SignatureSubpacket[] unhashed = EmptySignatureSubpackets;
        private SignatureSubpacket[] hashed = EmptySignatureSubpackets;

        public PgpSignatureGenerator( PublicKeyAlgorithmTag keyAlgorithm, HashAlgorithmTag hashAlgorithm )
        {
            this.keyAlgorithm = keyAlgorithm;
            this.hashAlgorithm = hashAlgorithm;
            this.dig = DigestUtilities.GetDigest( PgpUtilities.GetDigestName( hashAlgorithm ) );
            this.sig = SignerUtilities.GetSigner( PgpUtilities.GetSignatureName( keyAlgorithm, hashAlgorithm ) );
        }

        public void InitSign( int sigType, PgpPrivateKey key ) => this.InitSign( sigType, key, null );

        public void InitSign( int sigType, PgpPrivateKey key, SecureRandom random )
        {
            this.privKey = key;
            this.signatureType = sigType;
            try
            {
                ICipherParameters parameters = key.Key;
                if (random != null)
                    parameters = new ParametersWithRandom( key.Key, random );
                this.sig.Init( true, parameters );
            }
            catch (InvalidKeyException ex)
            {
                throw new PgpException( "invalid key.", ex );
            }
            this.dig.Reset();
            this.lastb = 0;
        }

        public void Update( byte b )
        {
            if (this.signatureType == 1)
                this.doCanonicalUpdateByte( b );
            else
                this.doUpdateByte( b );
        }

        private void doCanonicalUpdateByte( byte b )
        {
            switch (b)
            {
                case 10:
                    if (this.lastb != 13)
                    {
                        this.doUpdateCRLF();
                        break;
                    }
                    break;
                case 13:
                    this.doUpdateCRLF();
                    break;
                default:
                    this.doUpdateByte( b );
                    break;
            }
            this.lastb = b;
        }

        private void doUpdateCRLF()
        {
            this.doUpdateByte( 13 );
            this.doUpdateByte( 10 );
        }

        private void doUpdateByte( byte b )
        {
            this.sig.Update( b );
            this.dig.Update( b );
        }

        public void Update( params byte[] b ) => this.Update( b, 0, b.Length );

        public void Update( byte[] b, int off, int len )
        {
            if (this.signatureType == 1)
            {
                int num = off + len;
                for (int index = off; index != num; ++index)
                    this.doCanonicalUpdateByte( b[index] );
            }
            else
            {
                this.sig.BlockUpdate( b, off, len );
                this.dig.BlockUpdate( b, off, len );
            }
        }

        public void SetHashedSubpackets( PgpSignatureSubpacketVector hashedPackets ) => this.hashed = hashedPackets == null ? EmptySignatureSubpackets : hashedPackets.ToSubpacketArray();

        public void SetUnhashedSubpackets( PgpSignatureSubpacketVector unhashedPackets ) => this.unhashed = unhashedPackets == null ? EmptySignatureSubpackets : unhashedPackets.ToSubpacketArray();

        public PgpOnePassSignature GenerateOnePassVersion( bool isNested ) => new PgpOnePassSignature( new OnePassSignaturePacket( this.signatureType, this.hashAlgorithm, this.keyAlgorithm, this.privKey.KeyId, isNested ) );

        public PgpSignature Generate()
        {
            SignatureSubpacket[] signatureSubpacketArray1 = this.hashed;
            SignatureSubpacket[] signatureSubpacketArray2 = this.unhashed;
            if (!this.packetPresent( this.hashed, SignatureSubpacketTag.CreationTime ))
                signatureSubpacketArray1 = this.insertSubpacket( signatureSubpacketArray1, new SignatureCreationTime( false, DateTime.UtcNow ) );
            if (!this.packetPresent( this.hashed, SignatureSubpacketTag.IssuerKeyId ) && !this.packetPresent( this.unhashed, SignatureSubpacketTag.IssuerKeyId ))
                signatureSubpacketArray2 = this.insertSubpacket( signatureSubpacketArray2, new IssuerKeyId( false, this.privKey.KeyId ) );
            int num = 4;
            byte[] array1;
            try
            {
                MemoryStream os = new MemoryStream();
                for (int index = 0; index != signatureSubpacketArray1.Length; ++index)
                    signatureSubpacketArray1[index].Encode( os );
                byte[] array2 = os.ToArray();
                MemoryStream memoryStream = new MemoryStream( array2.Length + 6 );
                memoryStream.WriteByte( (byte)num );
                memoryStream.WriteByte( (byte)this.signatureType );
                memoryStream.WriteByte( (byte)this.keyAlgorithm );
                memoryStream.WriteByte( (byte)this.hashAlgorithm );
                memoryStream.WriteByte( (byte)(array2.Length >> 8) );
                memoryStream.WriteByte( (byte)array2.Length );
                memoryStream.Write( array2, 0, array2.Length );
                array1 = memoryStream.ToArray();
            }
            catch (IOException ex)
            {
                throw new PgpException( "exception encoding hashed data.", ex );
            }
            this.sig.BlockUpdate( array1, 0, array1.Length );
            this.dig.BlockUpdate( array1, 0, array1.Length );
            byte[] input = new byte[6]
            {
        (byte) num,
        byte.MaxValue,
        (byte) (array1.Length >> 24),
        (byte) (array1.Length >> 16),
        (byte) (array1.Length >> 8),
        (byte) array1.Length
            };
            this.sig.BlockUpdate( input, 0, input.Length );
            this.dig.BlockUpdate( input, 0, input.Length );
            byte[] signature1 = this.sig.GenerateSignature();
            byte[] numArray = DigestUtilities.DoFinal( this.dig );
            byte[] fingerprint = new byte[2]
            {
        numArray[0],
        numArray[1]
            };
            MPInteger[] signature2 = this.keyAlgorithm == PublicKeyAlgorithmTag.RsaSign || this.keyAlgorithm == PublicKeyAlgorithmTag.RsaGeneral ? PgpUtilities.RsaSigToMpi( signature1 ) : PgpUtilities.DsaSigToMpi( signature1 );
            return new PgpSignature( new SignaturePacket( this.signatureType, this.privKey.KeyId, this.keyAlgorithm, this.hashAlgorithm, signatureSubpacketArray1, signatureSubpacketArray2, fingerprint, signature2 ) );
        }

        public PgpSignature GenerateCertification( string id, PgpPublicKey pubKey )
        {
            this.UpdateWithPublicKey( pubKey );
            this.UpdateWithIdData( 180, Strings.ToUtf8ByteArray( id ) );
            return this.Generate();
        }

        public PgpSignature GenerateCertification(
          PgpUserAttributeSubpacketVector userAttributes,
          PgpPublicKey pubKey )
        {
            this.UpdateWithPublicKey( pubKey );
            try
            {
                MemoryStream os = new MemoryStream();
                foreach (UserAttributeSubpacket subpacket in userAttributes.ToSubpacketArray())
                    subpacket.Encode( os );
                this.UpdateWithIdData( 209, os.ToArray() );
            }
            catch (IOException ex)
            {
                throw new PgpException( "cannot encode subpacket array", ex );
            }
            return this.Generate();
        }

        public PgpSignature GenerateCertification( PgpPublicKey masterKey, PgpPublicKey pubKey )
        {
            this.UpdateWithPublicKey( masterKey );
            this.UpdateWithPublicKey( pubKey );
            return this.Generate();
        }

        public PgpSignature GenerateCertification( PgpPublicKey pubKey )
        {
            this.UpdateWithPublicKey( pubKey );
            return this.Generate();
        }

        private byte[] GetEncodedPublicKey( PgpPublicKey pubKey )
        {
            try
            {
                return pubKey.publicPk.GetEncodedContents();
            }
            catch (IOException ex)
            {
                throw new PgpException( "exception preparing key.", ex );
            }
        }

        private bool packetPresent( SignatureSubpacket[] packets, SignatureSubpacketTag type )
        {
            for (int index = 0; index != packets.Length; ++index)
            {
                if (packets[index].SubpacketType == type)
                    return true;
            }
            return false;
        }

        private SignatureSubpacket[] insertSubpacket(
          SignatureSubpacket[] packets,
          SignatureSubpacket subpacket )
        {
            SignatureSubpacket[] signatureSubpacketArray = new SignatureSubpacket[packets.Length + 1];
            signatureSubpacketArray[0] = subpacket;
            packets.CopyTo( signatureSubpacketArray, 1 );
            return signatureSubpacketArray;
        }

        private void UpdateWithIdData( int header, byte[] idBytes )
        {
            this.Update( (byte)header, (byte)(idBytes.Length >> 24), (byte)(idBytes.Length >> 16), (byte)(idBytes.Length >> 8), (byte)idBytes.Length );
            this.Update( idBytes );
        }

        private void UpdateWithPublicKey( PgpPublicKey key )
        {
            byte[] encodedPublicKey = this.GetEncodedPublicKey( key );
            this.Update( 153, (byte)(encodedPublicKey.Length >> 8), (byte)encodedPublicKey.Length );
            this.Update( encodedPublicKey );
        }
    }
}
