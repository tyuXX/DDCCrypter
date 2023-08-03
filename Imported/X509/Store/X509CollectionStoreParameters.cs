// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509CollectionStoreParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.X509.Store
{
    public class X509CollectionStoreParameters : IX509StoreParameters
    {
        private readonly IList collection;

        public X509CollectionStoreParameters( ICollection collection ) => this.collection = collection != null ? Platform.CreateArrayList( collection ) : throw new ArgumentNullException( nameof( collection ) );

        public ICollection GetCollection() => Platform.CreateArrayList( collection );

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( "X509CollectionStoreParameters: [\n" );
            stringBuilder.Append( "  collection: " + collection + "\n" );
            stringBuilder.Append( "]" );
            return stringBuilder.ToString();
        }
    }
}
