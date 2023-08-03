// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerOctetStringParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
    {
        private readonly Asn1StreamParser _parser;

        internal BerOctetStringParser( Asn1StreamParser parser ) => this._parser = parser;

        public Stream GetOctetStream() => new ConstructedOctetStream( this._parser );

        public Asn1Object ToAsn1Object()
        {
            try
            {
                return new BerOctetString( Streams.ReadAll( this.GetOctetStream() ) );
            }
            catch (IOException ex)
            {
                throw new Asn1ParsingException( "IOException converting stream to byte array: " + ex.Message, ex );
            }
        }
    }
}
