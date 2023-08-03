// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPublicKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpPublicKey
    {
        private static readonly int[] MasterKeyCertificationTypes = new int[4]
        {
      19,
      18,
      17,
      16
        };
        private long keyId;
        private byte[] fingerprint;
        private int keyStrength;
        internal PublicKeyPacket publicPk;
        internal TrustPacket trustPk;
        internal IList keySigs = Platform.CreateArrayList();
        internal IList ids = Platform.CreateArrayList();
        internal IList idTrusts = Platform.CreateArrayList();
        internal IList idSigs = Platform.CreateArrayList();
        internal IList subSigs;

        public static byte[] CalculateFingerprint( PublicKeyPacket publicPk )
        {
            IBcpgKey key = publicPk.Key;
            IDigest digest;
            if (publicPk.Version <= 3)
            {
                RsaPublicBcpgKey rsaPublicBcpgKey = (RsaPublicBcpgKey)key;
                try
                {
                    digest = DigestUtilities.GetDigest( "MD5" );
                    UpdateDigest( digest, rsaPublicBcpgKey.Modulus );
                    UpdateDigest( digest, rsaPublicBcpgKey.PublicExponent );
                }
                catch (Exception ex)
                {
                    throw new PgpException( "can't encode key components: " + ex.Message, ex );
                }
            }
            else
            {
                try
                {
                    byte[] encodedContents = publicPk.GetEncodedContents();
                    digest = DigestUtilities.GetDigest( "SHA1" );
                    digest.Update( 153 );
                    digest.Update( (byte)(encodedContents.Length >> 8) );
                    digest.Update( (byte)encodedContents.Length );
                    digest.BlockUpdate( encodedContents, 0, encodedContents.Length );
                }
                catch (Exception ex)
                {
                    throw new PgpException( "can't encode key components: " + ex.Message, ex );
                }
            }
            return DigestUtilities.DoFinal( digest );
        }

        private static void UpdateDigest( IDigest d, BigInteger b )
        {
            byte[] byteArrayUnsigned = b.ToByteArrayUnsigned();
            d.BlockUpdate( byteArrayUnsigned, 0, byteArrayUnsigned.Length );
        }

        private void Init()
        {
            IBcpgKey key = this.publicPk.Key;
            this.fingerprint = CalculateFingerprint( this.publicPk );
            if (this.publicPk.Version <= 3)
            {
                RsaPublicBcpgKey rsaPublicBcpgKey = (RsaPublicBcpgKey)key;
                this.keyId = rsaPublicBcpgKey.Modulus.LongValue;
                this.keyStrength = rsaPublicBcpgKey.Modulus.BitLength;
            }
            else
            {
                this.keyId = ((long)this.fingerprint[this.fingerprint.Length - 8] << 56) | ((long)this.fingerprint[this.fingerprint.Length - 7] << 48) | ((long)this.fingerprint[this.fingerprint.Length - 6] << 40) | ((long)this.fingerprint[this.fingerprint.Length - 5] << 32) | ((long)this.fingerprint[this.fingerprint.Length - 4] << 24) | ((long)this.fingerprint[this.fingerprint.Length - 3] << 16) | ((long)this.fingerprint[this.fingerprint.Length - 2] << 8) | this.fingerprint[this.fingerprint.Length - 1];
                switch (key)
                {
                    case RsaPublicBcpgKey _:
                        this.keyStrength = ((RsaPublicBcpgKey)key).Modulus.BitLength;
                        break;
                    case DsaPublicBcpgKey _:
                        this.keyStrength = ((DsaPublicBcpgKey)key).P.BitLength;
                        break;
                    case ElGamalPublicBcpgKey _:
                        this.keyStrength = ((ElGamalPublicBcpgKey)key).P.BitLength;
                        break;
                    case ECPublicBcpgKey _:
                        this.keyStrength = ECKeyPairGenerator.FindECCurveByOid( ((ECPublicBcpgKey)key).CurveOid ).Curve.FieldSize;
                        break;
                }
            }
        }

        public PgpPublicKey(
          PublicKeyAlgorithmTag algorithm,
          AsymmetricKeyParameter pubKey,
          DateTime time )
        {
            if (pubKey.IsPrivate)
                throw new ArgumentException( "Expected a public key", nameof( pubKey ) );
            IBcpgKey key;
            switch (pubKey)
            {
                case RsaKeyParameters _:
                    RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)pubKey;
                    key = new RsaPublicBcpgKey( rsaKeyParameters.Modulus, rsaKeyParameters.Exponent );
                    break;
                case DsaPublicKeyParameters _:
                    DsaPublicKeyParameters publicKeyParameters1 = (DsaPublicKeyParameters)pubKey;
                    DsaParameters parameters1 = publicKeyParameters1.Parameters;
                    key = new DsaPublicBcpgKey( parameters1.P, parameters1.Q, parameters1.G, publicKeyParameters1.Y );
                    break;
                case ECPublicKeyParameters _:
                    ECPublicKeyParameters publicKeyParameters2 = (ECPublicKeyParameters)pubKey;
                    if (algorithm == PublicKeyAlgorithmTag.EC)
                    {
                        key = new ECDHPublicBcpgKey( publicKeyParameters2.PublicKeyParamSet, publicKeyParameters2.Q, HashAlgorithmTag.Sha256, SymmetricKeyAlgorithmTag.Aes128 );
                        break;
                    }
                    if (algorithm != PublicKeyAlgorithmTag.ECDsa)
                        throw new PgpException( "unknown EC algorithm" );
                    key = new ECDsaPublicBcpgKey( publicKeyParameters2.PublicKeyParamSet, publicKeyParameters2.Q );
                    break;
                case ElGamalPublicKeyParameters _:
                    ElGamalPublicKeyParameters publicKeyParameters3 = (ElGamalPublicKeyParameters)pubKey;
                    ElGamalParameters parameters2 = publicKeyParameters3.Parameters;
                    key = new ElGamalPublicBcpgKey( parameters2.P, parameters2.G, publicKeyParameters3.Y );
                    break;
                default:
                    throw new PgpException( "unknown key class" );
            }
            this.publicPk = new PublicKeyPacket( algorithm, time, key );
            this.ids = Platform.CreateArrayList();
            this.idSigs = Platform.CreateArrayList();
            try
            {
                this.Init();
            }
            catch (IOException ex)
            {
                throw new PgpException( "exception calculating keyId", ex );
            }
        }

        public PgpPublicKey( PublicKeyPacket publicPk )
          : this( publicPk, Platform.CreateArrayList(), Platform.CreateArrayList() )
        {
        }

        internal PgpPublicKey( PublicKeyPacket publicPk, TrustPacket trustPk, IList sigs )
        {
            this.publicPk = publicPk;
            this.trustPk = trustPk;
            this.subSigs = sigs;
            this.Init();
        }

        internal PgpPublicKey( PgpPublicKey key, TrustPacket trust, IList subSigs )
        {
            this.publicPk = key.publicPk;
            this.trustPk = trust;
            this.subSigs = subSigs;
            this.fingerprint = key.fingerprint;
            this.keyId = key.keyId;
            this.keyStrength = key.keyStrength;
        }

        internal PgpPublicKey( PgpPublicKey pubKey )
        {
            this.publicPk = pubKey.publicPk;
            this.keySigs = Platform.CreateArrayList( pubKey.keySigs );
            this.ids = Platform.CreateArrayList( pubKey.ids );
            this.idTrusts = Platform.CreateArrayList( pubKey.idTrusts );
            this.idSigs = Platform.CreateArrayList( pubKey.idSigs.Count );
            for (int index = 0; index != pubKey.idSigs.Count; ++index)
                this.idSigs.Add( Platform.CreateArrayList( (ICollection)pubKey.idSigs[index] ) );
            if (pubKey.subSigs != null)
            {
                this.subSigs = Platform.CreateArrayList( pubKey.subSigs.Count );
                for (int index = 0; index != pubKey.subSigs.Count; ++index)
                    this.subSigs.Add( pubKey.subSigs[index] );
            }
            this.fingerprint = pubKey.fingerprint;
            this.keyId = pubKey.keyId;
            this.keyStrength = pubKey.keyStrength;
        }

        internal PgpPublicKey(
          PublicKeyPacket publicPk,
          TrustPacket trustPk,
          IList keySigs,
          IList ids,
          IList idTrusts,
          IList idSigs )
        {
            this.publicPk = publicPk;
            this.trustPk = trustPk;
            this.keySigs = keySigs;
            this.ids = ids;
            this.idTrusts = idTrusts;
            this.idSigs = idSigs;
            this.Init();
        }

        internal PgpPublicKey( PublicKeyPacket publicPk, IList ids, IList idSigs )
        {
            this.publicPk = publicPk;
            this.ids = ids;
            this.idSigs = idSigs;
            this.Init();
        }

        public int Version => this.publicPk.Version;

        public DateTime CreationTime => this.publicPk.GetTime();

        [Obsolete( "Use 'GetValidSeconds' instead" )]
        public int ValidDays
        {
            get
            {
                if (this.publicPk.Version <= 3)
                    return this.publicPk.ValidDays;
                long validSeconds = this.GetValidSeconds();
                return validSeconds <= 0L ? 0 : System.Math.Max( 1, (int)(validSeconds / 86400L) );
            }
        }

        public byte[] GetTrustData() => this.trustPk == null ? null : Arrays.Clone( this.trustPk.GetLevelAndTrustAmount() );

        public long GetValidSeconds()
        {
            if (this.publicPk.Version <= 3)
                return publicPk.ValidDays * 86400L;
            if (this.IsMasterKey)
            {
                for (int index = 0; index != MasterKeyCertificationTypes.Length; ++index)
                {
                    long expirationTimeFromSig = this.GetExpirationTimeFromSig( true, MasterKeyCertificationTypes[index] );
                    if (expirationTimeFromSig >= 0L)
                        return expirationTimeFromSig;
                }
            }
            else
            {
                long expirationTimeFromSig = this.GetExpirationTimeFromSig( false, 24 );
                if (expirationTimeFromSig >= 0L)
                    return expirationTimeFromSig;
            }
            return 0;
        }

        private long GetExpirationTimeFromSig( bool selfSigned, int signatureType )
        {
            foreach (PgpSignature pgpSignature in this.GetSignaturesOfType( signatureType ))
            {
                if (!selfSigned || pgpSignature.KeyId == this.KeyId)
                {
                    PgpSignatureSubpacketVector hashedSubPackets = pgpSignature.GetHashedSubPackets();
                    return hashedSubPackets != null ? hashedSubPackets.GetKeyExpirationTime() : 0L;
                }
            }
            return -1;
        }

        public long KeyId => this.keyId;

        public byte[] GetFingerprint() => (byte[])this.fingerprint.Clone();

        public bool IsEncryptionKey
        {
            get
            {
                switch (this.publicPk.Algorithm)
                {
                    case PublicKeyAlgorithmTag.RsaGeneral:
                    case PublicKeyAlgorithmTag.RsaEncrypt:
                    case PublicKeyAlgorithmTag.ElGamalEncrypt:
                    case PublicKeyAlgorithmTag.EC:
                    case PublicKeyAlgorithmTag.ElGamalGeneral:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsMasterKey => this.subSigs == null;

        public PublicKeyAlgorithmTag Algorithm => this.publicPk.Algorithm;

        public int BitStrength => this.keyStrength;

        public AsymmetricKeyParameter GetKey()
        {
            try
            {
                switch (this.publicPk.Algorithm)
                {
                    case PublicKeyAlgorithmTag.RsaGeneral:
                    case PublicKeyAlgorithmTag.RsaEncrypt:
                    case PublicKeyAlgorithmTag.RsaSign:
                        RsaPublicBcpgKey key1 = (RsaPublicBcpgKey)this.publicPk.Key;
                        return new RsaKeyParameters( false, key1.Modulus, key1.PublicExponent );
                    case PublicKeyAlgorithmTag.ElGamalEncrypt:
                    case PublicKeyAlgorithmTag.ElGamalGeneral:
                        ElGamalPublicBcpgKey key2 = (ElGamalPublicBcpgKey)this.publicPk.Key;
                        return new ElGamalPublicKeyParameters( key2.Y, new ElGamalParameters( key2.P, key2.G ) );
                    case PublicKeyAlgorithmTag.Dsa:
                        DsaPublicBcpgKey key3 = (DsaPublicBcpgKey)this.publicPk.Key;
                        return new DsaPublicKeyParameters( key3.Y, new DsaParameters( key3.P, key3.Q, key3.G ) );
                    case PublicKeyAlgorithmTag.EC:
                        return this.GetECKey( "ECDH" );
                    case PublicKeyAlgorithmTag.ECDsa:
                        return this.GetECKey( "ECDSA" );
                    default:
                        throw new PgpException( "unknown public key algorithm encountered" );
                }
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "exception constructing public key", ex );
            }
        }

        private ECPublicKeyParameters GetECKey( string algorithm )
        {
            ECPublicBcpgKey key = (ECPublicBcpgKey)this.publicPk.Key;
            ECPoint q = ECKeyPairGenerator.FindECCurveByOid( key.CurveOid ).Curve.DecodePoint( BigIntegers.AsUnsignedByteArray( key.EncodedPoint ) );
            return new ECPublicKeyParameters( algorithm, q, key.CurveOid );
        }

        public IEnumerable GetUserIds()
        {
            IList arrayList = Platform.CreateArrayList();
            foreach (object id in (IEnumerable)this.ids)
            {
                if (id is string)
                    arrayList.Add( id );
            }
            return new EnumerableProxy( arrayList );
        }

        public IEnumerable GetUserAttributes()
        {
            IList arrayList = Platform.CreateArrayList();
            foreach (object id in (IEnumerable)this.ids)
            {
                if (id is PgpUserAttributeSubpacketVector)
                    arrayList.Add( id );
            }
            return new EnumerableProxy( arrayList );
        }

        public IEnumerable GetSignaturesForId( string id )
        {
            if (id == null)
                throw new ArgumentNullException( nameof( id ) );
            for (int index = 0; index != this.ids.Count; ++index)
            {
                if (id.Equals( this.ids[index] ))
                    return new EnumerableProxy( (IEnumerable)this.idSigs[index] );
            }
            return null;
        }

        public IEnumerable GetSignaturesForUserAttribute( PgpUserAttributeSubpacketVector userAttributes )
        {
            for (int index = 0; index != this.ids.Count; ++index)
            {
                if (userAttributes.Equals( this.ids[index] ))
                    return new EnumerableProxy( (IEnumerable)this.idSigs[index] );
            }
            return null;
        }

        public IEnumerable GetSignaturesOfType( int signatureType )
        {
            IList arrayList = Platform.CreateArrayList();
            foreach (PgpSignature signature in this.GetSignatures())
            {
                if (signature.SignatureType == signatureType)
                    arrayList.Add( signature );
            }
            return new EnumerableProxy( arrayList );
        }

        public IEnumerable GetSignatures()
        {
            IList list = this.subSigs;
            if (list == null)
            {
                list = Platform.CreateArrayList( keySigs );
                foreach (ICollection idSig in (IEnumerable)this.idSigs)
                    CollectionUtilities.AddRange( list, idSig );
            }
            return new EnumerableProxy( list );
        }

        public IEnumerable GetKeySignatures() => new EnumerableProxy( this.subSigs ?? Platform.CreateArrayList( keySigs ) );

        public PublicKeyPacket PublicKeyPacket => this.publicPk;

        public byte[] GetEncoded()
        {
            MemoryStream outStr = new();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public void Encode( Stream outStr )
        {
            BcpgOutputStream outStream = BcpgOutputStream.Wrap( outStr );
            outStream.WritePacket( publicPk );
            if (this.trustPk != null)
                outStream.WritePacket( trustPk );
            if (this.subSigs == null)
            {
                foreach (PgpSignature keySig in (IEnumerable)this.keySigs)
                    keySig.Encode( outStream );
                for (int index = 0; index != this.ids.Count; ++index)
                {
                    if (this.ids[index] is string)
                    {
                        string id = (string)this.ids[index];
                        outStream.WritePacket( new UserIdPacket( id ) );
                    }
                    else
                    {
                        PgpUserAttributeSubpacketVector id = (PgpUserAttributeSubpacketVector)this.ids[index];
                        outStream.WritePacket( new UserAttributePacket( id.ToSubpacketArray() ) );
                    }
                    if (this.idTrusts[index] != null)
                        outStream.WritePacket( (ContainedPacket)this.idTrusts[index] );
                    foreach (PgpSignature pgpSignature in (IEnumerable)this.idSigs[index])
                        pgpSignature.Encode( outStream );
                }
            }
            else
            {
                foreach (PgpSignature subSig in (IEnumerable)this.subSigs)
                    subSig.Encode( outStream );
            }
        }

        public bool IsRevoked()
        {
            int num = 0;
            bool flag = false;
            if (this.IsMasterKey)
            {
                while (!flag && num < this.keySigs.Count)
                {
                    if (((PgpSignature)this.keySigs[num++]).SignatureType == 32)
                        flag = true;
                }
            }
            else
            {
                while (!flag && num < this.subSigs.Count)
                {
                    if (((PgpSignature)this.subSigs[num++]).SignatureType == 40)
                        flag = true;
                }
            }
            return flag;
        }

        public static PgpPublicKey AddCertification(
          PgpPublicKey key,
          string id,
          PgpSignature certification )
        {
            return AddCert( key, id, certification );
        }

        public static PgpPublicKey AddCertification(
          PgpPublicKey key,
          PgpUserAttributeSubpacketVector userAttributes,
          PgpSignature certification )
        {
            return AddCert( key, userAttributes, certification );
        }

        private static PgpPublicKey AddCert( PgpPublicKey key, object id, PgpSignature certification )
        {
            PgpPublicKey pgpPublicKey = new( key );
            IList list = null;
            for (int index = 0; index != pgpPublicKey.ids.Count; ++index)
            {
                if (id.Equals( pgpPublicKey.ids[index] ))
                    list = (IList)pgpPublicKey.idSigs[index];
            }
            if (list != null)
            {
                list.Add( certification );
            }
            else
            {
                IList arrayList = Platform.CreateArrayList();
                arrayList.Add( certification );
                pgpPublicKey.ids.Add( id );
                pgpPublicKey.idTrusts.Add( null );
                pgpPublicKey.idSigs.Add( arrayList );
            }
            return pgpPublicKey;
        }

        public static PgpPublicKey RemoveCertification(
          PgpPublicKey key,
          PgpUserAttributeSubpacketVector userAttributes )
        {
            return RemoveCert( key, userAttributes );
        }

        public static PgpPublicKey RemoveCertification( PgpPublicKey key, string id ) => RemoveCert( key, id );

        private static PgpPublicKey RemoveCert( PgpPublicKey key, object id )
        {
            PgpPublicKey pgpPublicKey = new( key );
            bool flag = false;
            for (int index = 0; index < pgpPublicKey.ids.Count; ++index)
            {
                if (id.Equals( pgpPublicKey.ids[index] ))
                {
                    flag = true;
                    pgpPublicKey.ids.RemoveAt( index );
                    pgpPublicKey.idTrusts.RemoveAt( index );
                    pgpPublicKey.idSigs.RemoveAt( index );
                }
            }
            return !flag ? null : pgpPublicKey;
        }

        public static PgpPublicKey RemoveCertification(
          PgpPublicKey key,
          string id,
          PgpSignature certification )
        {
            return RemoveCert( key, id, certification );
        }

        public static PgpPublicKey RemoveCertification(
          PgpPublicKey key,
          PgpUserAttributeSubpacketVector userAttributes,
          PgpSignature certification )
        {
            return RemoveCert( key, userAttributes, certification );
        }

        private static PgpPublicKey RemoveCert( PgpPublicKey key, object id, PgpSignature certification )
        {
            PgpPublicKey pgpPublicKey = new( key );
            bool flag = false;
            for (int index = 0; index < pgpPublicKey.ids.Count; ++index)
            {
                if (id.Equals( pgpPublicKey.ids[index] ))
                {
                    IList idSig = (IList)pgpPublicKey.idSigs[index];
                    flag = idSig.Contains( certification );
                    if (flag)
                        idSig.Remove( certification );
                }
            }
            return !flag ? null : pgpPublicKey;
        }

        public static PgpPublicKey AddCertification( PgpPublicKey key, PgpSignature certification )
        {
            if (key.IsMasterKey)
            {
                if (certification.SignatureType == 40)
                    throw new ArgumentException( "signature type incorrect for master key revocation." );
            }
            else if (certification.SignatureType == 32)
                throw new ArgumentException( "signature type incorrect for sub-key revocation." );
            PgpPublicKey pgpPublicKey = new( key );
            if (pgpPublicKey.subSigs != null)
                pgpPublicKey.subSigs.Add( certification );
            else
                pgpPublicKey.keySigs.Add( certification );
            return pgpPublicKey;
        }

        public static PgpPublicKey RemoveCertification( PgpPublicKey key, PgpSignature certification )
        {
            PgpPublicKey key1 = new( key );
            IList list = key1.subSigs != null ? key1.subSigs : key1.keySigs;
            int index = list.IndexOf( certification );
            bool flag = index >= 0;
            if (flag)
            {
                list.RemoveAt( index );
            }
            else
            {
                foreach (string userId in key.GetUserIds())
                {
                    foreach (object obj in key.GetSignaturesForId( userId ))
                    {
                        if (certification == obj)
                        {
                            flag = true;
                            key1 = RemoveCertification( key1, userId, certification );
                        }
                    }
                }
                if (!flag)
                {
                    foreach (PgpUserAttributeSubpacketVector userAttribute in key.GetUserAttributes())
                    {
                        foreach (object obj in key.GetSignaturesForUserAttribute( userAttribute ))
                        {
                            if (certification == obj)
                                key1 = RemoveCertification( key1, userAttribute, certification );
                        }
                    }
                }
            }
            return key1;
        }
    }
}
