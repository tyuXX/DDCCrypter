// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509NameEntryConverter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1.X509
{
    public abstract class X509NameEntryConverter
    {
        protected Asn1Object ConvertHexEncoded( string hexString, int offset ) => Asn1Object.FromByteArray( Hex.Decode( hexString.Substring( offset ) ) );

        protected bool CanBePrintable( string str ) => DerPrintableString.IsPrintableString( str );

        public abstract Asn1Object GetConvertedValue( DerObjectIdentifier oid, string value );
    }
}
