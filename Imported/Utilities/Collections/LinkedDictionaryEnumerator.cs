// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.LinkedDictionaryEnumerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    internal class LinkedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
    {
        private readonly LinkedDictionary parent;
        private int pos = -1;

        internal LinkedDictionaryEnumerator( LinkedDictionary parent ) => this.parent = parent;

        public virtual object Current => Entry;

        public virtual DictionaryEntry Entry
        {
            get
            {
                object currentKey = this.CurrentKey;
                return new DictionaryEntry( currentKey, this.parent.hash[currentKey] );
            }
        }

        public virtual object Key => this.CurrentKey;

        public virtual bool MoveNext() => this.pos < this.parent.keys.Count && ++this.pos < this.parent.keys.Count;

        public virtual void Reset() => this.pos = -1;

        public virtual object Value => this.parent.hash[this.CurrentKey];

        private object CurrentKey
        {
            get
            {
                if (this.pos < 0 || this.pos >= this.parent.keys.Count)
                    throw new InvalidOperationException();
                return this.parent.keys[this.pos];
            }
        }
    }
}
