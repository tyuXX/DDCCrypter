// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerExternalParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class DerExternalParser : Asn1Encodable
    {
        private readonly Asn1StreamParser _parser;

        public DerExternalParser( Asn1StreamParser parser ) => this._parser = parser;

        public IAsn1Convertible ReadObject() => this._parser.ReadObject();

        public override Asn1Object ToAsn1Object() => new DerExternal( this._parser.ReadVector() );
    }
}
