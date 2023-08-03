// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.Pem.PemHeader
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.IO.Pem
{
    public class PemHeader
    {
        private string name;
        private string val;

        public PemHeader( string name, string val )
        {
            this.name = name;
            this.val = val;
        }

        public virtual string Name => this.name;

        public virtual string Value => this.val;

        public override int GetHashCode() => this.GetHashCode( this.name ) + (31 * this.GetHashCode( this.val ));

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            if (!(obj is PemHeader))
                return false;
            PemHeader pemHeader = (PemHeader)obj;
            return Equals( name, pemHeader.name ) && Equals( val, pemHeader.val );
        }

        private int GetHashCode( string s ) => s == null ? 1 : s.GetHashCode();
    }
}
