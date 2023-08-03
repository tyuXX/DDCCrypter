// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPublicKeyRing
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpPublicKeyRing : PgpKeyRing
    {
        private readonly IList keys;

        public PgpPublicKeyRing( byte[] encoding )
          : this( new MemoryStream( encoding, false ) )
        {
        }

        internal PgpPublicKeyRing( IList pubKeys ) => this.keys = pubKeys;

        public PgpPublicKeyRing( Stream inputStream )
        {
            this.keys = Platform.CreateArrayList();
            BcpgInputStream bcpgInput = BcpgInputStream.Wrap( inputStream );
            PacketTag packetTag = bcpgInput.NextPacketTag();
            switch (packetTag)
            {
                case PacketTag.PublicKey:
                case PacketTag.PublicSubkey:
                    PublicKeyPacket publicPk = (PublicKeyPacket)bcpgInput.ReadPacket();
                    TrustPacket trustPk = ReadOptionalTrustPacket( bcpgInput );
                    IList keySigs = ReadSignaturesAndTrust( bcpgInput );
                    IList ids;
                    IList idTrusts;
                    IList idSigs;
                    ReadUserIDs( bcpgInput, out ids, out idTrusts, out idSigs );
                    this.keys.Add( new PgpPublicKey( publicPk, trustPk, keySigs, ids, idTrusts, idSigs ) );
                    while (bcpgInput.NextPacketTag() == PacketTag.PublicSubkey)
                        this.keys.Add( ReadSubkey( bcpgInput ) );
                    break;
                default:
                    throw new IOException( "public key ring doesn't start with public key tag: tag 0x" + ((int)packetTag).ToString( "X" ) );
            }
        }

        public virtual PgpPublicKey GetPublicKey() => (PgpPublicKey)this.keys[0];

        public virtual PgpPublicKey GetPublicKey( long keyId )
        {
            foreach (PgpPublicKey key in (IEnumerable)this.keys)
            {
                if (keyId == key.KeyId)
                    return key;
            }
            return null;
        }

        public virtual IEnumerable GetPublicKeys() => new EnumerableProxy( keys );

        public virtual byte[] GetEncoded()
        {
            MemoryStream outStr = new MemoryStream();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public virtual void Encode( Stream outStr )
        {
            if (outStr == null)
                throw new ArgumentNullException( nameof( outStr ) );
            foreach (PgpPublicKey key in (IEnumerable)this.keys)
                key.Encode( outStr );
        }

        public static PgpPublicKeyRing InsertPublicKey( PgpPublicKeyRing pubRing, PgpPublicKey pubKey )
        {
            IList arrayList = Platform.CreateArrayList( pubRing.keys );
            bool flag1 = false;
            bool flag2 = false;
            for (int index = 0; index != arrayList.Count; ++index)
            {
                PgpPublicKey pgpPublicKey = (PgpPublicKey)arrayList[index];
                if (pgpPublicKey.KeyId == pubKey.KeyId)
                {
                    flag1 = true;
                    arrayList[index] = pubKey;
                }
                if (pgpPublicKey.IsMasterKey)
                    flag2 = true;
            }
            if (!flag1)
            {
                if (pubKey.IsMasterKey)
                {
                    if (flag2)
                        throw new ArgumentException( "cannot add a master key to a ring that already has one" );
                    arrayList.Insert( 0, pubKey );
                }
                else
                    arrayList.Add( pubKey );
            }
            return new PgpPublicKeyRing( arrayList );
        }

        public static PgpPublicKeyRing RemovePublicKey( PgpPublicKeyRing pubRing, PgpPublicKey pubKey )
        {
            IList arrayList = Platform.CreateArrayList( pubRing.keys );
            bool flag = false;
            for (int index = 0; index < arrayList.Count; ++index)
            {
                if (((PgpPublicKey)arrayList[index]).KeyId == pubKey.KeyId)
                {
                    flag = true;
                    arrayList.RemoveAt( index );
                }
            }
            return !flag ? null : new PgpPublicKeyRing( arrayList );
        }

        internal static PgpPublicKey ReadSubkey( BcpgInputStream bcpgInput ) => new PgpPublicKey( (PublicKeyPacket)bcpgInput.ReadPacket(), ReadOptionalTrustPacket( bcpgInput ), ReadSignaturesAndTrust( bcpgInput ) );
    }
}
