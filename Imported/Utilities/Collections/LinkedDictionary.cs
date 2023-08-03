// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.LinkedDictionary
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public class LinkedDictionary : IDictionary, ICollection, IEnumerable
    {
        internal readonly IDictionary hash = Platform.CreateHashtable();
        internal readonly IList keys = Platform.CreateArrayList();

        public virtual void Add( object k, object v )
        {
            this.hash.Add( k, v );
            this.keys.Add( k );
        }

        public virtual void Clear()
        {
            this.hash.Clear();
            this.keys.Clear();
        }

        public virtual bool Contains( object k ) => this.hash.Contains( k );

        public virtual void CopyTo( Array array, int index )
        {
            foreach (object key in (IEnumerable)this.keys)
                array.SetValue( this.hash[key], index++ );
        }

        public virtual int Count => this.hash.Count;

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public virtual IDictionaryEnumerator GetEnumerator() => new LinkedDictionaryEnumerator( this );

        public virtual void Remove( object k )
        {
            this.hash.Remove( k );
            this.keys.Remove( k );
        }

        public virtual bool IsFixedSize => false;

        public virtual bool IsReadOnly => false;

        public virtual bool IsSynchronized => false;

        public virtual object SyncRoot => false;

        public virtual ICollection Keys => Platform.CreateArrayList( keys );

        public virtual ICollection Values
        {
            get
            {
                IList arrayList = Platform.CreateArrayList( this.keys.Count );
                foreach (object key in (IEnumerable)this.keys)
                    arrayList.Add( this.hash[key] );
                return arrayList;
            }
        }

        public virtual object this[object k]
        {
            get => this.hash[k];
            set
            {
                if (!this.hash.Contains( k ))
                    this.keys.Add( k );
                this.hash[k] = value;
            }
        }
    }
}
