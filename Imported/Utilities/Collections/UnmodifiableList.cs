// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.UnmodifiableList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public abstract class UnmodifiableList : IList, ICollection, IEnumerable
    {
        public virtual int Add( object o ) => throw new NotSupportedException();

        public virtual void Clear() => throw new NotSupportedException();

        public abstract bool Contains( object o );

        public abstract void CopyTo( Array array, int index );

        public abstract int Count { get; }

        public abstract IEnumerator GetEnumerator();

        public abstract int IndexOf( object o );

        public virtual void Insert( int i, object o ) => throw new NotSupportedException();

        public abstract bool IsFixedSize { get; }

        public virtual bool IsReadOnly => true;

        public abstract bool IsSynchronized { get; }

        public virtual void Remove( object o ) => throw new NotSupportedException();

        public virtual void RemoveAt( int i ) => throw new NotSupportedException();

        public abstract object SyncRoot { get; }

        public virtual object this[int i]
        {
            get => this.GetValue( i );
            set => throw new NotSupportedException();
        }

        protected abstract object GetValue( int i );
    }
}
