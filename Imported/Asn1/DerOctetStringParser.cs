// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerOctetStringParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
    {
        private readonly DefiniteLengthInputStream stream;

        internal DerOctetStringParser( DefiniteLengthInputStream stream ) => this.stream = stream;

        public Stream GetOctetStream() => stream;

        public Asn1Object ToAsn1Object()
        {
            try
            {
                return new DerOctetString( this.stream.ToArray() );
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException( "IOException converting stream to byte array: " + ex.Message, ex );
            }
        }
    }
}
