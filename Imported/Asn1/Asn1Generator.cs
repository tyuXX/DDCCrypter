// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1Generator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1Generator
    {
        private Stream _out;

        protected Asn1Generator( Stream outStream ) => this._out = outStream;

        protected Stream Out => this._out;

        public abstract void AddObject( Asn1Encodable obj );

        public abstract Stream GetRawOutputStream();

        public abstract void Close();
    }
}
