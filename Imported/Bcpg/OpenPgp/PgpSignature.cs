// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSignature
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Date;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSignature
    {
        public const int BinaryDocument = 0;
        public const int CanonicalTextDocument = 1;
        public const int StandAlone = 2;
        public const int DefaultCertification = 16;
        public const int NoCertification = 17;
        public const int CasualCertification = 18;
        public const int PositiveCertification = 19;
        public const int SubkeyBinding = 24;
        public const int PrimaryKeyBinding = 25;
        public const int DirectKey = 31;
        public const int KeyRevocation = 32;
        public const int SubkeyRevocation = 40;
        public const int CertificationRevocation = 48;
        public const int Timestamp = 64;
        private readonly SignaturePacket sigPck;
        private readonly int signatureType;
        private readonly TrustPacket trustPck;
        private ISigner sig;
        private byte lastb;

        internal PgpSignature( BcpgInputStream bcpgInput )
          : this( (SignaturePacket)bcpgInput.ReadPacket() )
        {
        }

        internal PgpSignature( SignaturePacket sigPacket )
          : this( sigPacket, null )
        {
        }

        internal PgpSignature( SignaturePacket sigPacket, TrustPacket trustPacket )
        {
            this.sigPck = sigPacket != null ? sigPacket : throw new ArgumentNullException( nameof( sigPacket ) );
            this.signatureType = this.sigPck.SignatureType;
            this.trustPck = trustPacket;
        }

        private void GetSig() => this.sig = SignerUtilities.GetSigner( PgpUtilities.GetSignatureName( this.sigPck.KeyAlgorithm, this.sigPck.HashAlgorithm ) );

        public int Version => this.sigPck.Version;

        public PublicKeyAlgorithmTag KeyAlgorithm => this.sigPck.KeyAlgorithm;

        public HashAlgorithmTag HashAlgorithm => this.sigPck.HashAlgorithm;

        public void InitVerify( PgpPublicKey pubKey )
        {
            this.lastb = 0;
            if (this.sig == null)
                this.GetSig();
            try
            {
                this.sig.Init( false, pubKey.GetKey() );
            }
            catch (InvalidKeyException ex)
            {
                throw new PgpException( "invalid key.", ex );
            }
        }

        public void Update( byte b )
        {
            if (this.signatureType == 1)
                this.doCanonicalUpdateByte( b );
            else
                this.sig.Update( b );
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
                    this.sig.Update( b );
                    break;
            }
            this.lastb = b;
        }

        private void doUpdateCRLF()
        {
            this.sig.Update( 13 );
            this.sig.Update( 10 );
        }

        public void Update( params byte[] bytes ) => this.Update( bytes, 0, bytes.Length );

        public void Update( byte[] bytes, int off, int length )
        {
            if (this.signatureType == 1)
            {
                int num = off + length;
                for (int index = off; index != num; ++index)
                    this.doCanonicalUpdateByte( bytes[index] );
            }
            else
                this.sig.BlockUpdate( bytes, off, length );
        }

        public bool Verify()
        {
            byte[] signatureTrailer = this.GetSignatureTrailer();
            this.sig.BlockUpdate( signatureTrailer, 0, signatureTrailer.Length );
            return this.sig.VerifySignature( this.GetSignature() );
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

        public bool VerifyCertification(
          PgpUserAttributeSubpacketVector userAttributes,
          PgpPublicKey key )
        {
            this.UpdateWithPublicKey( key );
            try
            {
                MemoryStream os = new();
                foreach (UserAttributeSubpacket subpacket in userAttributes.ToSubpacketArray())
                    subpacket.Encode( os );
                this.UpdateWithIdData( 209, os.ToArray() );
            }
            catch (IOException ex)
            {
                throw new PgpException( "cannot encode subpacket array", ex );
            }
            this.Update( this.sigPck.GetSignatureTrailer() );
            return this.sig.VerifySignature( this.GetSignature() );
        }

        public bool VerifyCertification( string id, PgpPublicKey key )
        {
            this.UpdateWithPublicKey( key );
            this.UpdateWithIdData( 180, Strings.ToUtf8ByteArray( id ) );
            this.Update( this.sigPck.GetSignatureTrailer() );
            return this.sig.VerifySignature( this.GetSignature() );
        }

        public bool VerifyCertification( PgpPublicKey masterKey, PgpPublicKey pubKey )
        {
            this.UpdateWithPublicKey( masterKey );
            this.UpdateWithPublicKey( pubKey );
            this.Update( this.sigPck.GetSignatureTrailer() );
            return this.sig.VerifySignature( this.GetSignature() );
        }

        public bool VerifyCertification( PgpPublicKey pubKey )
        {
            if (this.SignatureType != 32 && this.SignatureType != 40)
                throw new InvalidOperationException( "signature is not a key signature" );
            this.UpdateWithPublicKey( pubKey );
            this.Update( this.sigPck.GetSignatureTrailer() );
            return this.sig.VerifySignature( this.GetSignature() );
        }

        public int SignatureType => this.sigPck.SignatureType;

        public long KeyId => this.sigPck.KeyId;

        [Obsolete( "Use 'CreationTime' property instead" )]
        public DateTime GetCreationTime() => this.CreationTime;

        public DateTime CreationTime => DateTimeUtilities.UnixMsToDateTime( this.sigPck.CreationTime );

        public byte[] GetSignatureTrailer() => this.sigPck.GetSignatureTrailer();

        public bool HasSubpackets => this.sigPck.GetHashedSubPackets() != null || this.sigPck.GetUnhashedSubPackets() != null;

        public PgpSignatureSubpacketVector GetHashedSubPackets() => this.createSubpacketVector( this.sigPck.GetHashedSubPackets() );

        public PgpSignatureSubpacketVector GetUnhashedSubPackets() => this.createSubpacketVector( this.sigPck.GetUnhashedSubPackets() );

        private PgpSignatureSubpacketVector createSubpacketVector( SignatureSubpacket[] pcks ) => pcks != null ? new PgpSignatureSubpacketVector( pcks ) : null;

        public byte[] GetSignature()
        {
            MPInteger[] signature1 = this.sigPck.GetSignature();
            byte[] signature2;
            if (signature1 != null)
            {
                if (signature1.Length == 1)
                {
                    signature2 = signature1[0].Value.ToByteArrayUnsigned();
                }
                else
                {
                    try
                    {
                        signature2 = new DerSequence( new Asn1Encodable[2]
                        {
               new DerInteger(signature1[0].Value),
               new DerInteger(signature1[1].Value)
                        } ).GetEncoded();
                    }
                    catch (IOException ex)
                    {
                        throw new PgpException( "exception encoding DSA sig.", ex );
                    }
                }
            }
            else
                signature2 = this.sigPck.GetSignatureBytes();
            return signature2;
        }

        public byte[] GetEncoded()
        {
            MemoryStream outStream = new();
            this.Encode( outStream );
            return outStream.ToArray();
        }

        public void Encode( Stream outStream )
        {
            BcpgOutputStream bcpgOutputStream = BcpgOutputStream.Wrap( outStream );
            bcpgOutputStream.WritePacket( sigPck );
            if (this.trustPck == null)
                return;
            bcpgOutputStream.WritePacket( trustPck );
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
    }
}
