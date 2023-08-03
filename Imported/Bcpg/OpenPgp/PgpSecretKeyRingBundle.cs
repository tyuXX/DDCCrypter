// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSecretKeyRingBundle
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSecretKeyRingBundle
    {
        private readonly IDictionary secretRings;
        private readonly IList order;

        private PgpSecretKeyRingBundle( IDictionary secretRings, IList order )
        {
            this.secretRings = secretRings;
            this.order = order;
        }

        public PgpSecretKeyRingBundle( byte[] encoding )
          : this( new MemoryStream( encoding, false ) )
        {
        }

        public PgpSecretKeyRingBundle( Stream inputStream )
          : this( new PgpObjectFactory( inputStream ).AllPgpObjects() )
        {
        }

        public PgpSecretKeyRingBundle( IEnumerable e )
        {
            this.secretRings = Platform.CreateHashtable();
            this.order = Platform.CreateArrayList();
            foreach (object obj in e)
            {
                long key = obj is PgpSecretKeyRing pgpSecretKeyRing ? pgpSecretKeyRing.GetPublicKey().KeyId : throw new PgpException( Platform.GetTypeName( obj ) + " found where PgpSecretKeyRing expected" );
                this.secretRings.Add( key, pgpSecretKeyRing );
                this.order.Add( key );
            }
        }

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.order.Count;

        public int Count => this.order.Count;

        public IEnumerable GetKeyRings() => new EnumerableProxy( secretRings.Values );

        public IEnumerable GetKeyRings( string userId ) => this.GetKeyRings( userId, false, false );

        public IEnumerable GetKeyRings( string userId, bool matchPartial ) => this.GetKeyRings( userId, matchPartial, false );

        public IEnumerable GetKeyRings( string userId, bool matchPartial, bool ignoreCase )
        {
            IList arrayList = Platform.CreateArrayList();
            if (ignoreCase)
                userId = Platform.ToUpperInvariant( userId );
            foreach (PgpSecretKeyRing keyRing in this.GetKeyRings())
            {
                foreach (string userId1 in keyRing.GetSecretKey().UserIds)
                {
                    string str = userId1;
                    if (ignoreCase)
                        str = Platform.ToUpperInvariant( str );
                    if (matchPartial)
                    {
                        if (Platform.IndexOf( str, userId ) > -1)
                            arrayList.Add( keyRing );
                    }
                    else if (str.Equals( userId ))
                        arrayList.Add( keyRing );
                }
            }
            return new EnumerableProxy( arrayList );
        }

        public PgpSecretKey GetSecretKey( long keyId )
        {
            foreach (PgpSecretKeyRing keyRing in this.GetKeyRings())
            {
                PgpSecretKey secretKey = keyRing.GetSecretKey( keyId );
                if (secretKey != null)
                    return secretKey;
            }
            return null;
        }

        public PgpSecretKeyRing GetSecretKeyRing( long keyId )
        {
            long key = keyId;
            if (this.secretRings.Contains( key ))
                return (PgpSecretKeyRing)this.secretRings[key];
            foreach (PgpSecretKeyRing keyRing in this.GetKeyRings())
            {
                if (keyRing.GetSecretKey( keyId ) != null)
                    return keyRing;
            }
            return null;
        }

        public bool Contains( long keyID ) => this.GetSecretKey( keyID ) != null;

        public byte[] GetEncoded()
        {
            MemoryStream outStr = new();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public void Encode( Stream outStr )
        {
            BcpgOutputStream outStr1 = BcpgOutputStream.Wrap( outStr );
            foreach (long key in (IEnumerable)this.order)
                ((PgpSecretKeyRing)this.secretRings[key]).Encode( outStr1 );
        }

        public static PgpSecretKeyRingBundle AddSecretKeyRing(
          PgpSecretKeyRingBundle bundle,
          PgpSecretKeyRing secretKeyRing )
        {
            long keyId = secretKeyRing.GetPublicKey().KeyId;
            if (bundle.secretRings.Contains( keyId ))
                throw new ArgumentException( "Collection already contains a key with a keyId for the passed in ring." );
            IDictionary hashtable = Platform.CreateHashtable( bundle.secretRings );
            IList arrayList = Platform.CreateArrayList( bundle.order );
            hashtable[keyId] = secretKeyRing;
            arrayList.Add( keyId );
            return new PgpSecretKeyRingBundle( hashtable, arrayList );
        }

        public static PgpSecretKeyRingBundle RemoveSecretKeyRing(
          PgpSecretKeyRingBundle bundle,
          PgpSecretKeyRing secretKeyRing )
        {
            long keyId = secretKeyRing.GetPublicKey().KeyId;
            if (!bundle.secretRings.Contains( keyId ))
                throw new ArgumentException( "Collection does not contain a key with a keyId for the passed in ring." );
            IDictionary hashtable = Platform.CreateHashtable( bundle.secretRings );
            IList arrayList = Platform.CreateArrayList( bundle.order );
            hashtable.Remove( keyId );
            arrayList.Remove( keyId );
            return new PgpSecretKeyRingBundle( hashtable, arrayList );
        }
    }
}
