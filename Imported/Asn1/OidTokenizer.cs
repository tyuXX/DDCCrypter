// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.OidTokenizer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class OidTokenizer
    {
        private string oid;
        private int index;

        public OidTokenizer( string oid ) => this.oid = oid;

        public bool HasMoreTokens => this.index != -1;

        public string NextToken()
        {
            if (this.index == -1)
                return null;
            int num = this.oid.IndexOf( '.', this.index );
            if (num == -1)
            {
                string str = this.oid.Substring( this.index );
                this.index = -1;
                return str;
            }
            string str1 = this.oid.Substring( this.index, num - this.index );
            this.index = num + 1;
            return str1;
        }
    }
}
