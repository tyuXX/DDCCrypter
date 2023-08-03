// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509StoreFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.X509.Store
{
    public sealed class X509StoreFactory
    {
        private X509StoreFactory()
        {
        }

        public static IX509Store Create( string type, IX509StoreParameters parameters )
        {
            string[] strArray = type != null ? Platform.ToUpperInvariant( type ).Split( '/' ) : throw new ArgumentNullException( nameof( type ) );
            if (strArray.Length < 2)
                throw new ArgumentException( nameof( type ) );
            if (strArray[1] != "COLLECTION")
                throw new NoSuchStoreException( "X.509 store type '" + type + "' not available." );
            ICollection collection = ((X509CollectionStoreParameters)parameters).GetCollection();
            switch (strArray[0])
            {
                case "ATTRIBUTECERTIFICATE":
                    checkCorrectType( collection, typeof( IX509AttributeCertificate ) );
                    break;
                case "CERTIFICATE":
                    checkCorrectType( collection, typeof( X509Certificate ) );
                    break;
                case "CERTIFICATEPAIR":
                    checkCorrectType( collection, typeof( X509CertificatePair ) );
                    break;
                case "CRL":
                    checkCorrectType( collection, typeof( X509Crl ) );
                    break;
                default:
                    throw new NoSuchStoreException( "X.509 store type '" + type + "' not available." );
            }
            return new X509CollectionStore( collection );
        }

        private static void checkCorrectType( ICollection coll, Type t )
        {
            foreach (object o in (IEnumerable)coll)
            {
                if (!t.IsInstanceOfType( o ))
                    throw new InvalidCastException( "Can't cast object to type: " + t.FullName );
            }
        }
    }
}
