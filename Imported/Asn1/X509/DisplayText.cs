// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.DisplayText
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class DisplayText : Asn1Encodable, IAsn1Choice
    {
        public const int ContentTypeIA5String = 0;
        public const int ContentTypeBmpString = 1;
        public const int ContentTypeUtf8String = 2;
        public const int ContentTypeVisibleString = 3;
        public const int DisplayTextMaximumSize = 200;
        internal readonly int contentType;
        internal readonly IAsn1String contents;

        public DisplayText( int type, string text )
        {
            if (text.Length > 200)
                text = text.Substring( 0, 200 );
            this.contentType = type;
            switch (type)
            {
                case 0:
                    this.contents = new DerIA5String( text );
                    break;
                case 1:
                    this.contents = new DerBmpString( text );
                    break;
                case 2:
                    this.contents = new DerUtf8String( text );
                    break;
                case 3:
                    this.contents = new DerVisibleString( text );
                    break;
                default:
                    this.contents = new DerUtf8String( text );
                    break;
            }
        }

        public DisplayText( string text )
        {
            if (text.Length > 200)
                text = text.Substring( 0, 200 );
            this.contentType = 2;
            this.contents = new DerUtf8String( text );
        }

        public DisplayText( IAsn1String contents ) => this.contents = contents;

        public static DisplayText GetInstance( object obj )
        {
            switch (obj)
            {
                case IAsn1String _:
                    return new DisplayText( (IAsn1String)obj );
                case DisplayText _:
                    return (DisplayText)obj;
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public override Asn1Object ToAsn1Object() => (Asn1Object)this.contents;

        public string GetString() => this.contents.GetString();
    }
}
