// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.HashSet
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public class HashSet : ISet, ICollection, IEnumerable
    {
        private readonly IDictionary impl = Platform.CreateHashtable();

        public HashSet()
        {
        }

        public HashSet( IEnumerable s )
        {
            foreach (object o in s)
                this.Add( o );
        }

        public virtual void Add( object o ) => this.impl[o] = null;

        public virtual void AddAll( IEnumerable e )
        {
            foreach (object o in e)
                this.Add( o );
        }

        public virtual void Clear() => this.impl.Clear();

        public virtual bool Contains( object o ) => this.impl.Contains( o );

        public virtual void CopyTo( Array array, int index ) => this.impl.Keys.CopyTo( array, index );

        public virtual int Count => this.impl.Count;

        public virtual IEnumerator GetEnumerator() => this.impl.Keys.GetEnumerator();

        public virtual bool IsEmpty => this.impl.Count == 0;

        public virtual bool IsFixedSize => this.impl.IsFixedSize;

        public virtual bool IsReadOnly => this.impl.IsReadOnly;

        public virtual bool IsSynchronized => this.impl.IsSynchronized;

        public virtual void Remove( object o ) => this.impl.Remove( o );

        public virtual void RemoveAll( IEnumerable e )
        {
            foreach (object o in e)
                this.Remove( o );
        }

        public virtual object SyncRoot => this.impl.SyncRoot;
    }
}
