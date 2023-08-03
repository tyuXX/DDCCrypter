// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.EmptyEnumerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public sealed class EmptyEnumerator : IEnumerator
    {
        public static readonly IEnumerator Instance = new EmptyEnumerator();

        private EmptyEnumerator()
        {
        }

        public bool MoveNext() => false;

        public void Reset()
        {
        }

        public object Current => throw new InvalidOperationException( "No elements" );
    }
}
