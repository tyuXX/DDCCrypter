// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.UnmodifiableSetProxy
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public class UnmodifiableSetProxy : UnmodifiableSet
    {
        private readonly ISet s;

        public UnmodifiableSetProxy( ISet s ) => this.s = s;

        public override bool Contains( object o ) => this.s.Contains( o );

        public override void CopyTo( Array array, int index ) => this.s.CopyTo( array, index );

        public override int Count => this.s.Count;

        public override IEnumerator GetEnumerator() => this.s.GetEnumerator();

        public override bool IsEmpty => this.s.IsEmpty;

        public override bool IsFixedSize => this.s.IsFixedSize;

        public override bool IsSynchronized => this.s.IsSynchronized;

        public override object SyncRoot => this.s.SyncRoot;
    }
}
