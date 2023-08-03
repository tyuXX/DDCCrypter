// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpKeyRingGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpKeyRingGenerator
    {
        private IList keys = Platform.CreateArrayList();
        private string id;
        private SymmetricKeyAlgorithmTag encAlgorithm;
        private HashAlgorithmTag hashAlgorithm;
        private int certificationLevel;
        private byte[] rawPassPhrase;
        private bool useSha1;
        private PgpKeyPair masterKey;
        private PgpSignatureSubpacketVector hashedPacketVector;
        private PgpSignatureSubpacketVector unhashedPacketVector;
        private SecureRandom rand;

        [Obsolete( "Use version taking an explicit 'useSha1' parameter instead" )]
        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          char[] passPhrase,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, masterKey, id, encAlgorithm, passPhrase, false, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, masterKey, id, encAlgorithm, false, passPhrase, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          bool utf8PassPhrase,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, masterKey, id, encAlgorithm, PgpUtilities.EncodePassPhrase( passPhrase, utf8PassPhrase ), useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          byte[] rawPassPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
        {
            this.certificationLevel = certificationLevel;
            this.masterKey = masterKey;
            this.id = id;
            this.encAlgorithm = encAlgorithm;
            this.rawPassPhrase = rawPassPhrase;
            this.useSha1 = useSha1;
            this.hashedPacketVector = hashedPackets;
            this.unhashedPacketVector = unhashedPackets;
            this.rand = rand;
            this.keys.Add( new PgpSecretKey( certificationLevel, masterKey, id, encAlgorithm, rawPassPhrase, false, useSha1, hashedPackets, unhashedPackets, rand ) );
        }

        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, masterKey, id, encAlgorithm, hashAlgorithm, false, passPhrase, useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          bool utf8PassPhrase,
          char[] passPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
          : this( certificationLevel, masterKey, id, encAlgorithm, hashAlgorithm, PgpUtilities.EncodePassPhrase( passPhrase, utf8PassPhrase ), useSha1, hashedPackets, unhashedPackets, rand )
        {
        }

        public PgpKeyRingGenerator(
          int certificationLevel,
          PgpKeyPair masterKey,
          string id,
          SymmetricKeyAlgorithmTag encAlgorithm,
          HashAlgorithmTag hashAlgorithm,
          byte[] rawPassPhrase,
          bool useSha1,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          SecureRandom rand )
        {
            this.certificationLevel = certificationLevel;
            this.masterKey = masterKey;
            this.id = id;
            this.encAlgorithm = encAlgorithm;
            this.rawPassPhrase = rawPassPhrase;
            this.useSha1 = useSha1;
            this.hashedPacketVector = hashedPackets;
            this.unhashedPacketVector = unhashedPackets;
            this.rand = rand;
            this.hashAlgorithm = hashAlgorithm;
            this.keys.Add( new PgpSecretKey( certificationLevel, masterKey, id, encAlgorithm, hashAlgorithm, rawPassPhrase, false, useSha1, hashedPackets, unhashedPackets, rand ) );
        }

        public void AddSubKey( PgpKeyPair keyPair ) => this.AddSubKey( keyPair, this.hashedPacketVector, this.unhashedPacketVector );

        public void AddSubKey( PgpKeyPair keyPair, HashAlgorithmTag hashAlgorithm ) => this.AddSubKey( keyPair, this.hashedPacketVector, this.unhashedPacketVector, hashAlgorithm );

        public void AddSubKey(
          PgpKeyPair keyPair,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets )
        {
            try
            {
                PgpSignatureGenerator signatureGenerator = new PgpSignatureGenerator( this.masterKey.PublicKey.Algorithm, HashAlgorithmTag.Sha1 );
                signatureGenerator.InitSign( 24, this.masterKey.PrivateKey );
                signatureGenerator.SetHashedSubpackets( hashedPackets );
                signatureGenerator.SetUnhashedSubpackets( unhashedPackets );
                IList arrayList = Platform.CreateArrayList();
                arrayList.Add( signatureGenerator.GenerateCertification( this.masterKey.PublicKey, keyPair.PublicKey ) );
                this.keys.Add( new PgpSecretKey( keyPair.PrivateKey, new PgpPublicKey( keyPair.PublicKey, null, arrayList ), this.encAlgorithm, this.rawPassPhrase, false, this.useSha1, this.rand, false ) );
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "exception adding subkey: ", ex );
            }
        }

        public void AddSubKey(
          PgpKeyPair keyPair,
          PgpSignatureSubpacketVector hashedPackets,
          PgpSignatureSubpacketVector unhashedPackets,
          HashAlgorithmTag hashAlgorithm )
        {
            try
            {
                PgpSignatureGenerator signatureGenerator = new PgpSignatureGenerator( this.masterKey.PublicKey.Algorithm, hashAlgorithm );
                signatureGenerator.InitSign( 24, this.masterKey.PrivateKey );
                signatureGenerator.SetHashedSubpackets( hashedPackets );
                signatureGenerator.SetUnhashedSubpackets( unhashedPackets );
                IList arrayList = Platform.CreateArrayList();
                arrayList.Add( signatureGenerator.GenerateCertification( this.masterKey.PublicKey, keyPair.PublicKey ) );
                this.keys.Add( new PgpSecretKey( keyPair.PrivateKey, new PgpPublicKey( keyPair.PublicKey, null, arrayList ), this.encAlgorithm, this.rawPassPhrase, false, this.useSha1, this.rand, false ) );
            }
            catch (PgpException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PgpException( "exception adding subkey: ", ex );
            }
        }

        public PgpSecretKeyRing GenerateSecretKeyRing() => new PgpSecretKeyRing( this.keys );

        public PgpPublicKeyRing GeneratePublicKeyRing()
        {
            IList arrayList = Platform.CreateArrayList();
            IEnumerator enumerator = this.keys.GetEnumerator();
            enumerator.MoveNext();
            PgpSecretKey current = (PgpSecretKey)enumerator.Current;
            arrayList.Add( current.PublicKey );
            while (enumerator.MoveNext())
            {
                PgpPublicKey pgpPublicKey = new PgpPublicKey( ((PgpSecretKey)enumerator.Current).PublicKey );
                pgpPublicKey.publicPk = new PublicSubkeyPacket( pgpPublicKey.Algorithm, pgpPublicKey.CreationTime, pgpPublicKey.publicPk.Key );
                arrayList.Add( pgpPublicKey );
            }
            return new PgpPublicKeyRing( arrayList );
        }
    }
}
