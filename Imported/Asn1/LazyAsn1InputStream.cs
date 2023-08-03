// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.LazyAsn1InputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class LazyAsn1InputStream : Asn1InputStream
    {
        public LazyAsn1InputStream( byte[] input )
          : base( input )
        {
        }

        public LazyAsn1InputStream( Stream inputStream )
          : base( inputStream )
        {
        }

        internal override DerSequence CreateDerSequence( DefiniteLengthInputStream dIn ) => new LazyDerSequence( dIn.ToArray() );

        internal override DerSet CreateDerSet( DefiniteLengthInputStream dIn ) => new LazyDerSet( dIn.ToArray() );
    }
}
