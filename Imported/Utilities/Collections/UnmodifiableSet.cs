// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.UnmodifiableSet
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public abstract class UnmodifiableSet : ISet, ICollection, IEnumerable
    {
        public virtual void Add( object o ) => throw new NotSupportedException();

        public virtual void AddAll( IEnumerable e ) => throw new NotSupportedException();

        public virtual void Clear() => throw new NotSupportedException();

        public abstract bool Contains( object o );

        public abstract void CopyTo( Array array, int index );

        public abstract int Count { get; }

        public abstract IEnumerator GetEnumerator();

        public abstract bool IsEmpty { get; }

        public abstract bool IsFixedSize { get; }

        public virtual bool IsReadOnly => true;

        public abstract bool IsSynchronized { get; }

        public abstract object SyncRoot { get; }

        public virtual void Remove( object o ) => throw new NotSupportedException();

        public virtual void RemoveAll( IEnumerable e ) => throw new NotSupportedException();
    }
}
