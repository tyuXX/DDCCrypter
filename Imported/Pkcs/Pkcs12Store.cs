// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.Pkcs12Store
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Pkcs
{
    public class Pkcs12Store
    {
        private const int MinIterations = 1024;
        private const int SaltSize = 20;
        private readonly Pkcs12Store.IgnoresCaseHashtable keys = new();
        private readonly IDictionary localIds = Platform.CreateHashtable();
        private readonly Pkcs12Store.IgnoresCaseHashtable certs = new();
        private readonly IDictionary chainCerts = Platform.CreateHashtable();
        private readonly IDictionary keyCerts = Platform.CreateHashtable();
        private readonly DerObjectIdentifier keyAlgorithm;
        private readonly DerObjectIdentifier certAlgorithm;
        private readonly bool useDerEncoding;
        private AsymmetricKeyEntry unmarkedKeyEntry = null;

        private static SubjectKeyIdentifier CreateSubjectKeyID( AsymmetricKeyParameter pubKey ) => new( SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( pubKey ) );

        internal Pkcs12Store(
          DerObjectIdentifier keyAlgorithm,
          DerObjectIdentifier certAlgorithm,
          bool useDerEncoding )
        {
            this.keyAlgorithm = keyAlgorithm;
            this.certAlgorithm = certAlgorithm;
            this.useDerEncoding = useDerEncoding;
        }

        public Pkcs12Store()
          : this( PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc, PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc, false )
        {
        }

        public Pkcs12Store( Stream input, char[] password )
          : this()
        {
            this.Load( input, password );
        }

        protected virtual void LoadKeyBag( PrivateKeyInfo privKeyInfo, Asn1Set bagAttributes )
        {
            AsymmetricKeyParameter key = PrivateKeyFactory.CreateKey( privKeyInfo );
            IDictionary hashtable = Platform.CreateHashtable();
            AsymmetricKeyEntry asymmetricKeyEntry = new( key, hashtable );
            string str = null;
            Asn1OctetString asn1OctetString = null;
            if (bagAttributes != null)
            {
                foreach (Asn1Sequence bagAttribute in bagAttributes)
                {
                    DerObjectIdentifier instance1 = DerObjectIdentifier.GetInstance( bagAttribute[0] );
                    Asn1Set instance2 = Asn1Set.GetInstance( bagAttribute[1] );
                    if (instance2.Count > 0)
                    {
                        Asn1Encodable asn1Encodable = instance2[0];
                        if (hashtable.Contains( instance1.Id ))
                        {
                            if (!hashtable[instance1.Id].Equals( asn1Encodable ))
                                throw new IOException( "attempt to add existing attribute with different value" );
                        }
                        else
                            hashtable.Add( instance1.Id, asn1Encodable );
                        if (instance1.Equals( PkcsObjectIdentifiers.Pkcs9AtFriendlyName ))
                        {
                            str = ((DerStringBase)asn1Encodable).GetString();
                            this.keys[str] = asymmetricKeyEntry;
                        }
                        else if (instance1.Equals( PkcsObjectIdentifiers.Pkcs9AtLocalKeyID ))
                            asn1OctetString = (Asn1OctetString)asn1Encodable;
                    }
                }
            }
            if (asn1OctetString != null)
            {
                string hexString = Hex.ToHexString( asn1OctetString.GetOctets() );
                if (str == null)
                    this.keys[hexString] = asymmetricKeyEntry;
                else
                    this.localIds[str] = hexString;
            }
            else
                this.unmarkedKeyEntry = asymmetricKeyEntry;
        }

        protected virtual void LoadPkcs8ShroudedKeyBag(
          EncryptedPrivateKeyInfo encPrivKeyInfo,
          Asn1Set bagAttributes,
          char[] password,
          bool wrongPkcs12Zero )
        {
            if (password == null)
                return;
            this.LoadKeyBag( PrivateKeyInfoFactory.CreatePrivateKeyInfo( password, wrongPkcs12Zero, encPrivKeyInfo ), bagAttributes );
        }

        public void Load( Stream input, char[] password )
        {
            Pfx pfx = input != null ? new Pfx( (Asn1Sequence)Asn1Object.FromStream( input ) ) : throw new ArgumentNullException( nameof( input ) );
            ContentInfo authSafe = pfx.AuthSafe;
            bool wrongPkcs12Zero = false;
            if (password != null && pfx.MacData != null)
            {
                MacData macData = pfx.MacData;
                DigestInfo mac = macData.Mac;
                AlgorithmIdentifier algorithmId = mac.AlgorithmID;
                byte[] salt = macData.GetSalt();
                int intValue = macData.IterationCount.IntValue;
                byte[] octets = ((Asn1OctetString)authSafe.Content).GetOctets();
                byte[] pbeMac = CalculatePbeMac( algorithmId.Algorithm, salt, intValue, password, false, octets );
                byte[] digest = mac.GetDigest();
                if (!Arrays.ConstantTimeAreEqual( pbeMac, digest ))
                {
                    if (password.Length > 0)
                        throw new IOException( "PKCS12 key store MAC invalid - wrong password or corrupted file." );
                    if (!Arrays.ConstantTimeAreEqual( CalculatePbeMac( algorithmId.Algorithm, salt, intValue, password, true, octets ), digest ))
                        throw new IOException( "PKCS12 key store MAC invalid - wrong password or corrupted file." );
                    wrongPkcs12Zero = true;
                }
            }
            this.keys.Clear();
            this.localIds.Clear();
            this.unmarkedKeyEntry = null;
            IList arrayList = Platform.CreateArrayList();
            if (authSafe.ContentType.Equals( PkcsObjectIdentifiers.Data ))
            {
                foreach (ContentInfo contentInfo in new AuthenticatedSafe( (Asn1Sequence)Asn1Object.FromByteArray( ((Asn1OctetString)authSafe.Content).GetOctets() ) ).GetContentInfo())
                {
                    DerObjectIdentifier contentType = contentInfo.ContentType;
                    byte[] data = null;
                    if (contentType.Equals( PkcsObjectIdentifiers.Data ))
                        data = ((Asn1OctetString)contentInfo.Content).GetOctets();
                    else if (contentType.Equals( PkcsObjectIdentifiers.EncryptedData ) && password != null)
                    {
                        EncryptedData instance = EncryptedData.GetInstance( contentInfo.Content );
                        data = CryptPbeData( false, instance.EncryptionAlgorithm, password, wrongPkcs12Zero, instance.Content.GetOctets() );
                    }
                    if (data != null)
                    {
                        foreach (Asn1Sequence fromByte in (Asn1Sequence)Asn1Object.FromByteArray( data ))
                        {
                            SafeBag safeBag = new( fromByte );
                            if (safeBag.BagID.Equals( PkcsObjectIdentifiers.CertBag ))
                                arrayList.Add( safeBag );
                            else if (safeBag.BagID.Equals( PkcsObjectIdentifiers.Pkcs8ShroudedKeyBag ))
                                this.LoadPkcs8ShroudedKeyBag( EncryptedPrivateKeyInfo.GetInstance( safeBag.BagValue ), safeBag.BagAttributes, password, wrongPkcs12Zero );
                            else if (safeBag.BagID.Equals( PkcsObjectIdentifiers.KeyBag ))
                                this.LoadKeyBag( PrivateKeyInfo.GetInstance( safeBag.BagValue ), safeBag.BagAttributes );
                        }
                    }
                }
            }
            this.certs.Clear();
            this.chainCerts.Clear();
            this.keyCerts.Clear();
            foreach (SafeBag safeBag in (IEnumerable)arrayList)
            {
                X509Certificate cert = new X509CertificateParser().ReadCertificate( ((Asn1OctetString)new CertBag( (Asn1Sequence)safeBag.BagValue ).CertValue).GetOctets() );
                IDictionary hashtable = Platform.CreateHashtable();
                Asn1OctetString asn1OctetString = null;
                string alias = null;
                if (safeBag.BagAttributes != null)
                {
                    foreach (Asn1Sequence bagAttribute in safeBag.BagAttributes)
                    {
                        DerObjectIdentifier instance1 = DerObjectIdentifier.GetInstance( bagAttribute[0] );
                        Asn1Set instance2 = Asn1Set.GetInstance( bagAttribute[1] );
                        if (instance2.Count > 0)
                        {
                            Asn1Encodable asn1Encodable = instance2[0];
                            if (hashtable.Contains( instance1.Id ))
                            {
                                if (!hashtable[instance1.Id].Equals( asn1Encodable ))
                                    throw new IOException( "attempt to add existing attribute with different value" );
                            }
                            else
                                hashtable.Add( instance1.Id, asn1Encodable );
                            if (instance1.Equals( PkcsObjectIdentifiers.Pkcs9AtFriendlyName ))
                                alias = ((DerStringBase)asn1Encodable).GetString();
                            else if (instance1.Equals( PkcsObjectIdentifiers.Pkcs9AtLocalKeyID ))
                                asn1OctetString = (Asn1OctetString)asn1Encodable;
                        }
                    }
                }
                Pkcs12Store.CertId key = new( cert.GetPublicKey() );
                X509CertificateEntry certificateEntry = new( cert, hashtable );
                this.chainCerts[key] = certificateEntry;
                if (this.unmarkedKeyEntry != null)
                {
                    if (this.keyCerts.Count == 0)
                    {
                        string hexString = Hex.ToHexString( key.Id );
                        this.keyCerts[hexString] = certificateEntry;
                        this.keys[hexString] = unmarkedKeyEntry;
                    }
                }
                else
                {
                    if (asn1OctetString != null)
                        this.keyCerts[Hex.ToHexString( asn1OctetString.GetOctets() )] = certificateEntry;
                    if (alias != null)
                        this.certs[alias] = certificateEntry;
                }
            }
        }

        public AsymmetricKeyEntry GetKey( string alias ) => alias != null ? (AsymmetricKeyEntry)this.keys[alias] : throw new ArgumentNullException( nameof( alias ) );

        public bool IsCertificateEntry( string alias )
        {
            if (alias == null)
                throw new ArgumentNullException( nameof( alias ) );
            return this.certs[alias] != null && this.keys[alias] == null;
        }

        public bool IsKeyEntry( string alias )
        {
            if (alias == null)
                throw new ArgumentNullException( nameof( alias ) );
            return this.keys[alias] != null;
        }

        private IDictionary GetAliasesTable()
        {
            IDictionary hashtable = Platform.CreateHashtable();
            foreach (string key in (IEnumerable)this.certs.Keys)
                hashtable[key] = "cert";
            foreach (string key in (IEnumerable)this.keys.Keys)
            {
                if (hashtable[key] == null)
                    hashtable[key] = "key";
            }
            return hashtable;
        }

        public IEnumerable Aliases => new EnumerableProxy( GetAliasesTable().Keys );

        public bool ContainsAlias( string alias ) => this.certs[alias] != null || this.keys[alias] != null;

        public X509CertificateEntry GetCertificate( string alias )
        {
            X509CertificateEntry certificate = alias != null ? (X509CertificateEntry)this.certs[alias] : throw new ArgumentNullException( nameof( alias ) );
            if (certificate == null)
            {
                string localId = (string)this.localIds[alias];
                certificate = localId == null ? (X509CertificateEntry)this.keyCerts[alias] : (X509CertificateEntry)this.keyCerts[localId];
            }
            return certificate;
        }

        public string GetCertificateAlias( X509Certificate cert )
        {
            if (cert == null)
                throw new ArgumentNullException( nameof( cert ) );
            foreach (DictionaryEntry cert1 in this.certs)
            {
                if (((X509CertificateEntry)cert1.Value).Certificate.Equals( cert ))
                    return (string)cert1.Key;
            }
            foreach (DictionaryEntry keyCert in this.keyCerts)
            {
                if (((X509CertificateEntry)keyCert.Value).Certificate.Equals( cert ))
                    return (string)keyCert.Key;
            }
            return null;
        }

        public X509CertificateEntry[] GetCertificateChain( string alias )
        {
            if (alias == null)
                throw new ArgumentNullException( nameof( alias ) );
            if (!this.IsKeyEntry( alias ))
                return null;
            X509CertificateEntry certificateEntry1 = this.GetCertificate( alias );
            if (certificateEntry1 == null)
                return null;
            IList arrayList = Platform.CreateArrayList();
            X509CertificateEntry certificateEntry2;
            for (; certificateEntry1 != null; certificateEntry1 = certificateEntry2 == certificateEntry1 ? null : certificateEntry2)
            {
                X509Certificate certificate1 = certificateEntry1.Certificate;
                certificateEntry2 = null;
                Asn1OctetString extensionValue = certificate1.GetExtensionValue( X509Extensions.AuthorityKeyIdentifier );
                if (extensionValue != null)
                {
                    AuthorityKeyIdentifier instance = AuthorityKeyIdentifier.GetInstance( Asn1Object.FromByteArray( extensionValue.GetOctets() ) );
                    if (instance.GetKeyIdentifier() != null)
                        certificateEntry2 = (X509CertificateEntry)this.chainCerts[new Pkcs12Store.CertId( instance.GetKeyIdentifier() )];
                }
                if (certificateEntry2 == null)
                {
                    X509Name issuerDn = certificate1.IssuerDN;
                    X509Name subjectDn = certificate1.SubjectDN;
                    if (!issuerDn.Equivalent( subjectDn ))
                    {
                        foreach (Pkcs12Store.CertId key in (IEnumerable)this.chainCerts.Keys)
                        {
                            X509CertificateEntry chainCert = (X509CertificateEntry)this.chainCerts[key];
                            X509Certificate certificate2 = chainCert.Certificate;
                            if (certificate2.SubjectDN.Equivalent( issuerDn ))
                            {
                                try
                                {
                                    certificate1.Verify( certificate2.GetPublicKey() );
                                    certificateEntry2 = chainCert;
                                    break;
                                }
                                catch (InvalidKeyException ex)
                                {
                                }
                            }
                        }
                    }
                }
                arrayList.Add( certificateEntry1 );
            }
            X509CertificateEntry[] certificateChain = new X509CertificateEntry[arrayList.Count];
            for (int index = 0; index < arrayList.Count; ++index)
                certificateChain[index] = (X509CertificateEntry)arrayList[index];
            return certificateChain;
        }

        public void SetCertificateEntry( string alias, X509CertificateEntry certEntry )
        {
            if (alias == null)
                throw new ArgumentNullException( nameof( alias ) );
            if (certEntry == null)
                throw new ArgumentNullException( nameof( certEntry ) );
            if (this.keys[alias] != null)
                throw new ArgumentException( "There is a key entry with the name " + alias + "." );
            this.certs[alias] = certEntry;
            this.chainCerts[new Pkcs12Store.CertId( certEntry.Certificate.GetPublicKey() )] = certEntry;
        }

        public void SetKeyEntry(
          string alias,
          AsymmetricKeyEntry keyEntry,
          X509CertificateEntry[] chain )
        {
            if (alias == null)
                throw new ArgumentNullException( nameof( alias ) );
            if (keyEntry == null)
                throw new ArgumentNullException( nameof( keyEntry ) );
            if (keyEntry.Key.IsPrivate && chain == null)
                throw new ArgumentException( "No certificate chain for private key" );
            if (this.keys[alias] != null)
                this.DeleteEntry( alias );
            this.keys[alias] = keyEntry;
            this.certs[alias] = chain[0];
            for (int index = 0; index != chain.Length; ++index)
                this.chainCerts[new Pkcs12Store.CertId( chain[index].Certificate.GetPublicKey() )] = chain[index];
        }

        public void DeleteEntry( string alias )
        {
            AsymmetricKeyEntry asymmetricKeyEntry = alias != null ? (AsymmetricKeyEntry)this.keys[alias] : throw new ArgumentNullException( nameof( alias ) );
            if (asymmetricKeyEntry != null)
                this.keys.Remove( alias );
            X509CertificateEntry certificateEntry = (X509CertificateEntry)this.certs[alias];
            if (certificateEntry != null)
            {
                this.certs.Remove( alias );
                this.chainCerts.Remove( new Pkcs12Store.CertId( certificateEntry.Certificate.GetPublicKey() ) );
            }
            if (asymmetricKeyEntry != null)
            {
                string localId = (string)this.localIds[alias];
                if (localId != null)
                {
                    this.localIds.Remove( alias );
                    certificateEntry = (X509CertificateEntry)this.keyCerts[localId];
                }
                if (certificateEntry != null)
                {
                    this.keyCerts.Remove( localId );
                    this.chainCerts.Remove( new Pkcs12Store.CertId( certificateEntry.Certificate.GetPublicKey() ) );
                }
            }
            if (certificateEntry == null && asymmetricKeyEntry == null)
                throw new ArgumentException( "no such entry as " + alias );
        }

        public bool IsEntryOfType( string alias, Type entryType )
        {
            if (entryType == typeof( X509CertificateEntry ))
                return this.IsCertificateEntry( alias );
            return entryType == typeof( AsymmetricKeyEntry ) && this.IsKeyEntry( alias ) && this.GetCertificate( alias ) != null;
        }

        [Obsolete( "Use 'Count' property instead" )]
        public int Size() => this.Count;

        public int Count => this.GetAliasesTable().Count;

        public void Save( Stream stream, char[] password, SecureRandom random )
        {
            if (stream == null)
                throw new ArgumentNullException( nameof( stream ) );
            if (random == null)
                throw new ArgumentNullException( nameof( random ) );
            Asn1EncodableVector v1 = new( new Asn1Encodable[0] );
            foreach (string key1 in (IEnumerable)this.keys.Keys)
            {
                byte[] numArray = new byte[20];
                random.NextBytes( numArray );
                AsymmetricKeyEntry key2 = (AsymmetricKeyEntry)this.keys[key1];
                DerObjectIdentifier oid;
                Asn1Encodable asn1Encodable1;
                if (password == null)
                {
                    oid = PkcsObjectIdentifiers.KeyBag;
                    asn1Encodable1 = PrivateKeyInfoFactory.CreatePrivateKeyInfo( key2.Key );
                }
                else
                {
                    oid = PkcsObjectIdentifiers.Pkcs8ShroudedKeyBag;
                    asn1Encodable1 = EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo( this.keyAlgorithm, password, numArray, 1024, key2.Key );
                }
                Asn1EncodableVector v2 = new( new Asn1Encodable[0] );
                foreach (string bagAttributeKey in key2.BagAttributeKeys)
                {
                    Asn1Encodable asn1Encodable2 = key2[bagAttributeKey];
                    if (!bagAttributeKey.Equals( PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id ))
                        v2.Add( new DerSequence( new Asn1Encodable[2]
                        {
               new DerObjectIdentifier(bagAttributeKey),
               new DerSet(asn1Encodable2)
                        } ) );
                }
                v2.Add( new DerSequence( new Asn1Encodable[2]
                {
           PkcsObjectIdentifiers.Pkcs9AtFriendlyName,
           new DerSet( new DerBmpString(key1))
                } ) );
                if (key2[PkcsObjectIdentifiers.Pkcs9AtLocalKeyID] == null)
                {
                    SubjectKeyIdentifier subjectKeyId = CreateSubjectKeyID( this.GetCertificate( key1 ).Certificate.GetPublicKey() );
                    v2.Add( new DerSequence( new Asn1Encodable[2]
                    {
             PkcsObjectIdentifiers.Pkcs9AtLocalKeyID,
             new DerSet( subjectKeyId)
                    } ) );
                }
                v1.Add( new SafeBag( oid, asn1Encodable1.ToAsn1Object(), new DerSet( v2 ) ) );
            }
            byte[] derEncoded1 = new DerSequence( v1 ).GetDerEncoded();
            ContentInfo contentInfo1 = new( PkcsObjectIdentifiers.Data, new BerOctetString( derEncoded1 ) );
            byte[] numArray1 = new byte[20];
            random.NextBytes( numArray1 );
            Asn1EncodableVector v3 = new( new Asn1Encodable[0] );
            AlgorithmIdentifier algorithmIdentifier = new( this.certAlgorithm, new Pkcs12PbeParams( numArray1, 1024 ).ToAsn1Object() );
            ISet set = new HashSet();
            foreach (string key in (IEnumerable)this.keys.Keys)
            {
                X509CertificateEntry certificate = this.GetCertificate( key );
                CertBag certBag = new( PkcsObjectIdentifiers.X509Certificate, new DerOctetString( certificate.Certificate.GetEncoded() ) );
                Asn1EncodableVector v4 = new( new Asn1Encodable[0] );
                foreach (string bagAttributeKey in certificate.BagAttributeKeys)
                {
                    Asn1Encodable asn1Encodable = certificate[bagAttributeKey];
                    if (!bagAttributeKey.Equals( PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id ))
                        v4.Add( new DerSequence( new Asn1Encodable[2]
                        {
               new DerObjectIdentifier(bagAttributeKey),
               new DerSet(asn1Encodable)
                        } ) );
                }
                v4.Add( new DerSequence( new Asn1Encodable[2]
                {
           PkcsObjectIdentifiers.Pkcs9AtFriendlyName,
           new DerSet( new DerBmpString(key))
                } ) );
                if (certificate[PkcsObjectIdentifiers.Pkcs9AtLocalKeyID] == null)
                {
                    SubjectKeyIdentifier subjectKeyId = CreateSubjectKeyID( certificate.Certificate.GetPublicKey() );
                    v4.Add( new DerSequence( new Asn1Encodable[2]
                    {
             PkcsObjectIdentifiers.Pkcs9AtLocalKeyID,
             new DerSet( subjectKeyId)
                    } ) );
                }
                v3.Add( new SafeBag( PkcsObjectIdentifiers.CertBag, certBag.ToAsn1Object(), new DerSet( v4 ) ) );
                set.Add( certificate.Certificate );
            }
            foreach (string key in (IEnumerable)this.certs.Keys)
            {
                X509CertificateEntry cert = (X509CertificateEntry)this.certs[key];
                if (this.keys[key] == null)
                {
                    CertBag certBag = new( PkcsObjectIdentifiers.X509Certificate, new DerOctetString( cert.Certificate.GetEncoded() ) );
                    Asn1EncodableVector v5 = new( new Asn1Encodable[0] );
                    foreach (string bagAttributeKey in cert.BagAttributeKeys)
                    {
                        if (!bagAttributeKey.Equals( PkcsObjectIdentifiers.Pkcs9AtLocalKeyID.Id ))
                        {
                            Asn1Encodable asn1Encodable = cert[bagAttributeKey];
                            if (!bagAttributeKey.Equals( PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id ))
                                v5.Add( new DerSequence( new Asn1Encodable[2]
                                {
                   new DerObjectIdentifier(bagAttributeKey),
                   new DerSet(asn1Encodable)
                                } ) );
                        }
                    }
                    v5.Add( new DerSequence( new Asn1Encodable[2]
                    {
             PkcsObjectIdentifiers.Pkcs9AtFriendlyName,
             new DerSet( new DerBmpString(key))
                    } ) );
                    v3.Add( new SafeBag( PkcsObjectIdentifiers.CertBag, certBag.ToAsn1Object(), new DerSet( v5 ) ) );
                    set.Add( cert.Certificate );
                }
            }
            foreach (Pkcs12Store.CertId key in (IEnumerable)this.chainCerts.Keys)
            {
                X509CertificateEntry chainCert = (X509CertificateEntry)this.chainCerts[key];
                if (!set.Contains( chainCert.Certificate ))
                {
                    CertBag certBag = new( PkcsObjectIdentifiers.X509Certificate, new DerOctetString( chainCert.Certificate.GetEncoded() ) );
                    Asn1EncodableVector v6 = new( new Asn1Encodable[0] );
                    foreach (string bagAttributeKey in chainCert.BagAttributeKeys)
                    {
                        if (!bagAttributeKey.Equals( PkcsObjectIdentifiers.Pkcs9AtLocalKeyID.Id ))
                            v6.Add( new DerSequence( new Asn1Encodable[2]
                            {
                 new DerObjectIdentifier(bagAttributeKey),
                 new DerSet(chainCert[bagAttributeKey])
                            } ) );
                    }
                    v3.Add( new SafeBag( PkcsObjectIdentifiers.CertBag, certBag.ToAsn1Object(), new DerSet( v6 ) ) );
                }
            }
            byte[] derEncoded2 = new DerSequence( v3 ).GetDerEncoded();
            ContentInfo contentInfo2;
            if (password == null)
            {
                contentInfo2 = new ContentInfo( PkcsObjectIdentifiers.Data, new BerOctetString( derEncoded2 ) );
            }
            else
            {
                byte[] str = CryptPbeData( true, algorithmIdentifier, password, false, derEncoded2 );
                EncryptedData encryptedData = new( PkcsObjectIdentifiers.Data, algorithmIdentifier, new BerOctetString( str ) );
                contentInfo2 = new ContentInfo( PkcsObjectIdentifiers.EncryptedData, encryptedData.ToAsn1Object() );
            }
            byte[] encoded = new AuthenticatedSafe( new ContentInfo[2]
            {
        contentInfo1,
        contentInfo2
            } ).GetEncoded( this.useDerEncoding ? "DER" : "BER" );
            ContentInfo contentInfo3 = new( PkcsObjectIdentifiers.Data, new BerOctetString( encoded ) );
            MacData macData = null;
            if (password != null)
            {
                byte[] numArray2 = new byte[20];
                random.NextBytes( numArray2 );
                byte[] pbeMac = CalculatePbeMac( OiwObjectIdentifiers.IdSha1, numArray2, 1024, password, false, encoded );
                macData = new MacData( new DigestInfo( new AlgorithmIdentifier( OiwObjectIdentifiers.IdSha1, DerNull.Instance ), pbeMac ), numArray2, 1024 );
            }
            Pfx pfx = new( contentInfo3, macData );
            (!this.useDerEncoding ? new BerOutputStream( stream ) : new DerOutputStream( stream )).WriteObject( pfx );
        }

        internal static byte[] CalculatePbeMac(
          DerObjectIdentifier oid,
          byte[] salt,
          int itCount,
          char[] password,
          bool wrongPkcs12Zero,
          byte[] data )
        {
            Asn1Encodable algorithmParameters = PbeUtilities.GenerateAlgorithmParameters( oid, salt, itCount );
            ICipherParameters cipherParameters = PbeUtilities.GenerateCipherParameters( oid, password, wrongPkcs12Zero, algorithmParameters );
            IMac engine = (IMac)PbeUtilities.CreateEngine( oid );
            engine.Init( cipherParameters );
            return MacUtilities.DoFinal( engine, data );
        }

        private static byte[] CryptPbeData(
          bool forEncryption,
          AlgorithmIdentifier algId,
          char[] password,
          bool wrongPkcs12Zero,
          byte[] data )
        {
            if (!(PbeUtilities.CreateEngine( algId.Algorithm ) is IBufferedCipher engine))
                throw new Exception( "Unknown encryption algorithm: " + algId.Algorithm );
            Pkcs12PbeParams instance = Pkcs12PbeParams.GetInstance( algId.Parameters );
            ICipherParameters cipherParameters = PbeUtilities.GenerateCipherParameters( algId.Algorithm, password, wrongPkcs12Zero, instance );
            engine.Init( forEncryption, cipherParameters );
            return engine.DoFinal( data );
        }

        internal class CertId
        {
            private readonly byte[] id;

            internal CertId( AsymmetricKeyParameter pubKey ) => this.id = CreateSubjectKeyID( pubKey ).GetKeyIdentifier();

            internal CertId( byte[] id ) => this.id = id;

            internal byte[] Id => this.id;

            public override int GetHashCode() => Arrays.GetHashCode( this.id );

            public override bool Equals( object obj )
            {
                if (obj == this)
                    return true;
                return obj is Pkcs12Store.CertId certId && Arrays.AreEqual( this.id, certId.id );
            }
        }

        private class IgnoresCaseHashtable : IEnumerable
        {
            private readonly IDictionary orig = Platform.CreateHashtable();
            private readonly IDictionary keys = Platform.CreateHashtable();

            public void Clear()
            {
                this.orig.Clear();
                this.keys.Clear();
            }

            public IEnumerator GetEnumerator() => this.orig.GetEnumerator();

            public ICollection Keys => this.orig.Keys;

            public object Remove( string alias )
            {
                string upperInvariant = Platform.ToUpperInvariant( alias );
                string key = (string)this.keys[upperInvariant];
                if (key == null)
                    return null;
                this.keys.Remove( upperInvariant );
                object obj = this.orig[key];
                this.orig.Remove( key );
                return obj;
            }

            public object this[string alias]
            {
                get
                {
                    string key = (string)this.keys[Platform.ToUpperInvariant( alias )];
                    return key == null ? null : this.orig[key];
                }
                set
                {
                    string upperInvariant = Platform.ToUpperInvariant( alias );
                    string key = (string)this.keys[upperInvariant];
                    if (key != null)
                        this.orig.Remove( key );
                    this.keys[upperInvariant] = alias;
                    this.orig[alias] = value;
                }
            }

            public ICollection Values => this.orig.Values;
        }
    }
}
