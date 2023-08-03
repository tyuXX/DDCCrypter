// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509CollectionStore
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.X509.Store
{
    internal class X509CollectionStore : IX509Store
    {
        private ICollection _local;

        internal X509CollectionStore( ICollection collection ) => this._local = Platform.CreateArrayList( collection );

        public ICollection GetMatches( IX509Selector selector )
        {
            if (selector == null)
                return Platform.CreateArrayList( this._local );
            IList arrayList = Platform.CreateArrayList();
            foreach (object obj in (IEnumerable)this._local)
            {
                if (selector.Match( obj ))
                    arrayList.Add( obj );
            }
            return arrayList;
        }
    }
}
