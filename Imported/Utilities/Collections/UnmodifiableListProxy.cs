// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.UnmodifiableListProxy
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public class UnmodifiableListProxy : UnmodifiableList
    {
        private readonly IList l;

        public UnmodifiableListProxy( IList l ) => this.l = l;

        public override bool Contains( object o ) => this.l.Contains( o );

        public override void CopyTo( Array array, int index ) => this.l.CopyTo( array, index );

        public override int Count => this.l.Count;

        public override IEnumerator GetEnumerator() => this.l.GetEnumerator();

        public override int IndexOf( object o ) => this.l.IndexOf( o );

        public override bool IsFixedSize => this.l.IsFixedSize;

        public override bool IsSynchronized => this.l.IsSynchronized;

        public override object SyncRoot => this.l.SyncRoot;

        protected override object GetValue( int i ) => this.l[i];
    }
}
