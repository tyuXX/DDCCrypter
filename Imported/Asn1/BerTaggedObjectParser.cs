// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerTaggedObjectParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerTaggedObjectParser : Asn1TaggedObjectParser, IAsn1Convertible
    {
        private bool _constructed;
        private int _tagNumber;
        private Asn1StreamParser _parser;

        [Obsolete]
        internal BerTaggedObjectParser( int baseTag, int tagNumber, Stream contentStream )
          : this( (baseTag & 32) != 0, tagNumber, new Asn1StreamParser( contentStream ) )
        {
        }

        internal BerTaggedObjectParser( bool constructed, int tagNumber, Asn1StreamParser parser )
        {
            this._constructed = constructed;
            this._tagNumber = tagNumber;
            this._parser = parser;
        }

        public bool IsConstructed => this._constructed;

        public int TagNo => this._tagNumber;

        public IAsn1Convertible GetObjectParser( int tag, bool isExplicit )
        {
            if (!isExplicit)
                return this._parser.ReadImplicit( this._constructed, tag );
            if (!this._constructed)
                throw new IOException( "Explicit tags must be constructed (see X.690 8.14.2)" );
            return this._parser.ReadObject();
        }

        public Asn1Object ToAsn1Object()
        {
            try
            {
                return this._parser.ReadTaggedObject( this._constructed, this._tagNumber );
            }
            catch (IOException ex)
            {
                throw new Asn1ParsingException( ex.Message );
            }
        }
    }
}
