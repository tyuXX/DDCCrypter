// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPublicKeyRingBundle
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
    public class PgpPublicKeyRingBundle
    {
        private readonly IDictionary pubRings;
        private readonly IList order;

        private PgpPublicKeyRingBundle( IDictionary pubRings, IList order )
        {
            this.pubRings = pubRings;
            this.order = order;
        }

        public PgpPublicKeyRingBundle( byte[] encoding )
          : this( new MemoryStream( encoding, false ) )
        {
        }

        public PgpPublicKeyRingBundle( Stream inputStream )
          : this( new PgpObjectFactory( inputStream ).AllPgpObjects() )
        {
        }

        public PgpPublicKeyRingBundle( IEnumerable e )
        {
            this.pubRings = Platform.CreateHashtable();
            this.order = Platform.CreateArrayList();
            foreach (object obj in e)
            {
                long key = obj is PgpPublicKeyRing pgpPublicKeyRing ? pgpPublicKeyRing.GetPublicKey().KeyId : throw new PgpException( Platform.GetTypeName( obj ) + " found where PgpPublicKeyRing expected" );
                this.pubRings.Add( key, pgpPublicKeyRing );
                this.order.Add( key );
            }
        }

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.order.Count;

        public int Count => this.order.Count;

        public IEnumerable GetKeyRings() => new EnumerableProxy( pubRings.Values );

        public IEnumerable GetKeyRings( string userId ) => this.GetKeyRings( userId, false, false );

        public IEnumerable GetKeyRings( string userId, bool matchPartial ) => this.GetKeyRings( userId, matchPartial, false );

        public IEnumerable GetKeyRings( string userId, bool matchPartial, bool ignoreCase )
        {
            IList arrayList = Platform.CreateArrayList();
            if (ignoreCase)
                userId = Platform.ToUpperInvariant( userId );
            foreach (PgpPublicKeyRing keyRing in this.GetKeyRings())
            {
                foreach (string userId1 in keyRing.GetPublicKey().GetUserIds())
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

        public PgpPublicKey GetPublicKey( long keyId )
        {
            foreach (PgpPublicKeyRing keyRing in this.GetKeyRings())
            {
                PgpPublicKey publicKey = keyRing.GetPublicKey( keyId );
                if (publicKey != null)
                    return publicKey;
            }
            return null;
        }

        public PgpPublicKeyRing GetPublicKeyRing( long keyId )
        {
            if (this.pubRings.Contains( keyId ))
                return (PgpPublicKeyRing)this.pubRings[keyId];
            foreach (PgpPublicKeyRing keyRing in this.GetKeyRings())
            {
                if (keyRing.GetPublicKey( keyId ) != null)
                    return keyRing;
            }
            return null;
        }

        public bool Contains( long keyID ) => this.GetPublicKey( keyID ) != null;

        public byte[] GetEncoded()
        {
            MemoryStream outStr = new MemoryStream();
            this.Encode( outStr );
            return outStr.ToArray();
        }

        public void Encode( Stream outStr )
        {
            BcpgOutputStream outStr1 = BcpgOutputStream.Wrap( outStr );
            foreach (long key in (IEnumerable)this.order)
                ((PgpPublicKeyRing)this.pubRings[key]).Encode( outStr1 );
        }

        public static PgpPublicKeyRingBundle AddPublicKeyRing(
          PgpPublicKeyRingBundle bundle,
          PgpPublicKeyRing publicKeyRing )
        {
            long keyId = publicKeyRing.GetPublicKey().KeyId;
            if (bundle.pubRings.Contains( keyId ))
                throw new ArgumentException( "Bundle already contains a key with a keyId for the passed in ring." );
            IDictionary hashtable = Platform.CreateHashtable( bundle.pubRings );
            IList arrayList = Platform.CreateArrayList( bundle.order );
            hashtable[keyId] = publicKeyRing;
            arrayList.Add( keyId );
            return new PgpPublicKeyRingBundle( hashtable, arrayList );
        }

        public static PgpPublicKeyRingBundle RemovePublicKeyRing(
          PgpPublicKeyRingBundle bundle,
          PgpPublicKeyRing publicKeyRing )
        {
            long keyId = publicKeyRing.GetPublicKey().KeyId;
            if (!bundle.pubRings.Contains( keyId ))
                throw new ArgumentException( "Bundle does not contain a key with a keyId for the passed in ring." );
            IDictionary hashtable = Platform.CreateHashtable( bundle.pubRings );
            IList arrayList = Platform.CreateArrayList( bundle.order );
            hashtable.Remove( keyId );
            arrayList.Remove( keyId );
            return new PgpPublicKeyRingBundle( hashtable, arrayList );
        }
    }
}
