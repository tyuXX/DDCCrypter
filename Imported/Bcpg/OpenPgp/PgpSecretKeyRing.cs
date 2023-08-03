// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSecretKeyRing
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSecretKeyRing : PgpKeyRing
    {
        private readonly IList keys;
        private readonly IList extraPubKeys;

        internal PgpSecretKeyRing( IList keys )
          : this( keys, Platform.CreateArrayList() )
        {
        }

        private PgpSecretKeyRing( IList keys, IList extraPubKeys )
        {
            this.keys = keys;
            this.extraPubKeys = extraPubKeys;
        }

        public PgpSecretKeyRing( byte[] encoding )
          : this( new MemoryStream( encoding ) )
        {
        }

        public PgpSecretKeyRing( Stream inputStream )
        {
            this.keys = Platform.CreateArrayList();
            this.extraPubKeys = Platform.CreateArrayList();
            BcpgInputStream bcpgInput = BcpgInputStream.Wrap( inputStream );
            PacketTag packetTag = bcpgInput.NextPacketTag();
            switch (packetTag)
            {
                case PacketTag.SecretKey:
                case PacketTag.SecretSubkey:
                    SecretKeyPacket secret1 = (SecretKeyPacket)bcpgInput.ReadPacket();
                    while (bcpgInput.NextPacketTag() == PacketTag.Experimental2)
                        bcpgInput.ReadPacket();
                    TrustPacket trustPk1 = ReadOptionalTrustPacket( bcpgInput );
                    IList keySigs = ReadSignaturesAndTrust( bcpgInput );
                    IList ids;
                    IList idTrusts;
                    IList idSigs;
                    ReadUserIDs( bcpgInput, out ids, out idTrusts, out idSigs );
                    this.keys.Add( new PgpSecretKey( secret1, new PgpPublicKey( secret1.PublicKeyPacket, trustPk1, keySigs, ids, idTrusts, idSigs ) ) );
                    while (bcpgInput.NextPacketTag() == PacketTag.SecretSubkey || bcpgInput.NextPacketTag() == PacketTag.PublicSubkey)
                    {
                        if (bcpgInput.NextPacketTag() == PacketTag.SecretSubkey)
                        {
                            SecretSubkeyPacket secret2 = (SecretSubkeyPacket)bcpgInput.ReadPacket();
                            while (bcpgInput.NextPacketTag() == PacketTag.Experimental2)
                                bcpgInput.ReadPacket();
                            TrustPacket trustPk2 = ReadOptionalTrustPacket( bcpgInput );
                            IList sigs = ReadSignaturesAndTrust( bcpgInput );
                            this.keys.Add( new PgpSecretKey( secret2, new PgpPublicKey( secret2.PublicKeyPacket, trustPk2, sigs ) ) );
                        }
                        else
                            this.extraPubKeys.Add( new PgpPublicKey( (PublicKeyPacket)bcpgInput.ReadPacket(), ReadOptionalTrustPacket( bcpgInput ), ReadSignaturesAndTrust( bcpgInput ) ) );
                    }
                    break;
                default:
                    throw new IOException( "secret key ring doesn't start with secret key tag: tag 0x" + ((int)packetTag).ToString( "X" ) );
            }
        }

        public PgpPublicKey GetPublicKey() => ((PgpSecretKey)this.keys[0]).PublicKey;

        public PgpSecretKey GetSecretKey() => (PgpSecretKey)this.keys[0];

        public IEnumerable GetSecretKeys() => new EnumerableProxy( keys );

        public PgpSecretKey GetSecretKey( long keyId )
        {
            foreach (PgpSecretKey key in (IEnumerable)this.keys)
            {
                if (keyId == key.KeyId)
                    return key;
            }
            return null;
        }

        public IEnumerable GetExtraPublicKeys() => new EnumerableProxy( extraPubKeys );

        public byte[] GetEncoded()
        {
            MemoryStream outStr = new();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public void Encode( Stream outStr )
        {
            if (outStr == null)
                throw new ArgumentNullException( nameof( outStr ) );
            foreach (PgpSecretKey key in (IEnumerable)this.keys)
                key.Encode( outStr );
            foreach (PgpPublicKey extraPubKey in (IEnumerable)this.extraPubKeys)
                extraPubKey.Encode( outStr );
        }

        public static PgpSecretKeyRing ReplacePublicKeys(
          PgpSecretKeyRing secretRing,
          PgpPublicKeyRing publicRing )
        {
            IList arrayList = Platform.CreateArrayList( secretRing.keys.Count );
            foreach (PgpSecretKey key in (IEnumerable)secretRing.keys)
            {
                PgpPublicKey publicKey = publicRing.GetPublicKey( key.KeyId );
                arrayList.Add( PgpSecretKey.ReplacePublicKey( key, publicKey ) );
            }
            return new PgpSecretKeyRing( arrayList );
        }

        public static PgpSecretKeyRing CopyWithNewPassword(
          PgpSecretKeyRing ring,
          char[] oldPassPhrase,
          char[] newPassPhrase,
          SymmetricKeyAlgorithmTag newEncAlgorithm,
          SecureRandom rand )
        {
            IList arrayList = Platform.CreateArrayList( ring.keys.Count );
            foreach (PgpSecretKey secretKey in ring.GetSecretKeys())
            {
                if (secretKey.IsPrivateKeyEmpty)
                    arrayList.Add( secretKey );
                else
                    arrayList.Add( PgpSecretKey.CopyWithNewPassword( secretKey, oldPassPhrase, newPassPhrase, newEncAlgorithm, rand ) );
            }
            return new PgpSecretKeyRing( arrayList, ring.extraPubKeys );
        }

        public static PgpSecretKeyRing InsertSecretKey( PgpSecretKeyRing secRing, PgpSecretKey secKey )
        {
            IList arrayList = Platform.CreateArrayList( secRing.keys );
            bool flag1 = false;
            bool flag2 = false;
            for (int index = 0; index != arrayList.Count; ++index)
            {
                PgpSecretKey pgpSecretKey = (PgpSecretKey)arrayList[index];
                if (pgpSecretKey.KeyId == secKey.KeyId)
                {
                    flag1 = true;
                    arrayList[index] = secKey;
                }
                if (pgpSecretKey.IsMasterKey)
                    flag2 = true;
            }
            if (!flag1)
            {
                if (secKey.IsMasterKey)
                {
                    if (flag2)
                        throw new ArgumentException( "cannot add a master key to a ring that already has one" );
                    arrayList.Insert( 0, secKey );
                }
                else
                    arrayList.Add( secKey );
            }
            return new PgpSecretKeyRing( arrayList, secRing.extraPubKeys );
        }

        public static PgpSecretKeyRing RemoveSecretKey( PgpSecretKeyRing secRing, PgpSecretKey secKey )
        {
            IList arrayList = Platform.CreateArrayList( secRing.keys );
            bool flag = false;
            for (int index = 0; index < arrayList.Count; ++index)
            {
                if (((PgpSecretKey)arrayList[index]).KeyId == secKey.KeyId)
                {
                    flag = true;
                    arrayList.RemoveAt( index );
                }
            }
            return !flag ? null : new PgpSecretKeyRing( arrayList, secRing.extraPubKeys );
        }
    }
}
