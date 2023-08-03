// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.LazyDerSequence
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Asn1
{
    internal class LazyDerSequence : DerSequence
    {
        private byte[] encoded;

        internal LazyDerSequence( byte[] encoded ) => this.encoded = encoded;

        private void Parse()
        {
            lock (this)
            {
                if (this.encoded == null)
                    return;
                Asn1InputStream asn1InputStream = new LazyAsn1InputStream( this.encoded );
                Asn1Object asn1Object;
                while ((asn1Object = asn1InputStream.ReadObject()) != null)
                    this.AddObject( asn1Object );
                this.encoded = null;
            }
        }

        public override Asn1Encodable this[int index]
        {
            get
            {
                this.Parse();
                return base[index];
            }
        }

        public override IEnumerator GetEnumerator()
        {
            this.Parse();
            return base.GetEnumerator();
        }

        public override int Count
        {
            get
            {
                this.Parse();
                return base.Count;
            }
        }

        internal override void Encode( DerOutputStream derOut )
        {
            lock (this)
            {
                if (this.encoded == null)
                    base.Encode( derOut );
                else
                    derOut.WriteEncoded( 48, this.encoded );
            }
        }
    }
}
