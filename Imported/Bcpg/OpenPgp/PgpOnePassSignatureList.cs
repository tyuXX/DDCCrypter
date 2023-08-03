// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpOnePassSignatureList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpOnePassSignatureList : PgpObject
    {
        private readonly PgpOnePassSignature[] sigs;

        public PgpOnePassSignatureList( PgpOnePassSignature[] sigs ) => this.sigs = (PgpOnePassSignature[])sigs.Clone();

        public PgpOnePassSignatureList( PgpOnePassSignature sig ) => this.sigs = new PgpOnePassSignature[1]
        {
      sig
        };

        public PgpOnePassSignature this[int index] => this.sigs[index];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public PgpOnePassSignature Get( int index ) => this[index];

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.sigs.Length;

        public int Count => this.sigs.Length;

        public bool IsEmpty => this.sigs.Length == 0;
    }
}
