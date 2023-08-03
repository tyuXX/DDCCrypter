// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.AsymmetricKeyEntry
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Pkcs
{
    public class AsymmetricKeyEntry : Pkcs12Entry
    {
        private readonly AsymmetricKeyParameter key;

        public AsymmetricKeyEntry( AsymmetricKeyParameter key )
          : base( Platform.CreateHashtable() )
        {
            this.key = key;
        }

        [Obsolete]
        public AsymmetricKeyEntry( AsymmetricKeyParameter key, Hashtable attributes )
          : base( attributes )
        {
            this.key = key;
        }

        public AsymmetricKeyEntry( AsymmetricKeyParameter key, IDictionary attributes )
          : base( attributes )
        {
            this.key = key;
        }

        public AsymmetricKeyParameter Key => this.key;

        public override bool Equals( object obj ) => obj is AsymmetricKeyEntry asymmetricKeyEntry && this.key.Equals( asymmetricKeyEntry.key );

        public override int GetHashCode() => ~this.key.GetHashCode();
    }
}
