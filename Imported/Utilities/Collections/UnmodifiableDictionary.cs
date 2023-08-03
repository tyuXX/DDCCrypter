// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.UnmodifiableDictionary
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public abstract class UnmodifiableDictionary : IDictionary, ICollection, IEnumerable
    {
        public virtual void Add( object k, object v ) => throw new NotSupportedException();

        public virtual void Clear() => throw new NotSupportedException();

        public abstract bool Contains( object k );

        public abstract void CopyTo( Array array, int index );

        public abstract int Count { get; }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public abstract IDictionaryEnumerator GetEnumerator();

        public virtual void Remove( object k ) => throw new NotSupportedException();

        public abstract bool IsFixedSize { get; }

        public virtual bool IsReadOnly => true;

        public abstract bool IsSynchronized { get; }

        public abstract object SyncRoot { get; }

        public abstract ICollection Keys { get; }

        public abstract ICollection Values { get; }

        public virtual object this[object k]
        {
            get => this.GetValue( k );
            set => throw new NotSupportedException();
        }

        protected abstract object GetValue( object k );
    }
}
