// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.UnmodifiableDictionaryProxy
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public class UnmodifiableDictionaryProxy : UnmodifiableDictionary
    {
        private readonly IDictionary d;

        public UnmodifiableDictionaryProxy( IDictionary d ) => this.d = d;

        public override bool Contains( object k ) => this.d.Contains( k );

        public override void CopyTo( Array array, int index ) => this.d.CopyTo( array, index );

        public override int Count => this.d.Count;

        public override IDictionaryEnumerator GetEnumerator() => this.d.GetEnumerator();

        public override bool IsFixedSize => this.d.IsFixedSize;

        public override bool IsSynchronized => this.d.IsSynchronized;

        public override object SyncRoot => this.d.SyncRoot;

        public override ICollection Keys => this.d.Keys;

        public override ICollection Values => this.d.Values;

        protected override object GetValue( object k ) => this.d[k];
    }
}
