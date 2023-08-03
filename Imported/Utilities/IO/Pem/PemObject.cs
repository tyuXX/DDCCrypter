// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.Pem.PemObject
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
    public class PemObject : PemObjectGenerator
    {
        private string type;
        private IList headers;
        private byte[] content;

        public PemObject( string type, byte[] content )
          : this( type, Platform.CreateArrayList(), content )
        {
        }

        public PemObject( string type, IList headers, byte[] content )
        {
            this.type = type;
            this.headers = Platform.CreateArrayList( headers );
            this.content = content;
        }

        public string Type => this.type;

        public IList Headers => this.headers;

        public byte[] Content => this.content;

        public PemObject Generate() => this;
    }
}
