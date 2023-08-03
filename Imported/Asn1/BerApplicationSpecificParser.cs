// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerApplicationSpecificParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class BerApplicationSpecificParser : IAsn1ApplicationSpecificParser, IAsn1Convertible
    {
        private readonly int tag;
        private readonly Asn1StreamParser parser;

        internal BerApplicationSpecificParser( int tag, Asn1StreamParser parser )
        {
            this.tag = tag;
            this.parser = parser;
        }

        public IAsn1Convertible ReadObject() => this.parser.ReadObject();

        public Asn1Object ToAsn1Object() => new BerApplicationSpecific( this.tag, this.parser.ReadVector() );
    }
}
