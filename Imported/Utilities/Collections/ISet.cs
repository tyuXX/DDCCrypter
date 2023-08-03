// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.ISet
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
    public interface ISet : ICollection, IEnumerable
    {
        void Add( object o );

        void AddAll( IEnumerable e );

        void Clear();

        bool Contains( object o );

        bool IsEmpty { get; }

        bool IsFixedSize { get; }

        bool IsReadOnly { get; }

        void Remove( object o );

        void RemoveAll( IEnumerable e );
    }
}
