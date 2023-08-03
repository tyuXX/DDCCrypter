// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.ContentIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ess
{
    public class ContentIdentifier : Asn1Encodable
    {
        private Asn1OctetString value;

        public static ContentIdentifier GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case ContentIdentifier _:
                    return (ContentIdentifier)o;
                case Asn1OctetString _:
                    return new ContentIdentifier( (Asn1OctetString)o );
                default:
                    throw new ArgumentException( "unknown object in 'ContentIdentifier' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        public ContentIdentifier( Asn1OctetString value ) => this.value = value;

        public ContentIdentifier( byte[] value )
          : this( new DerOctetString( value ) )
        {
        }

        public Asn1OctetString Value => this.value;

        public override Asn1Object ToAsn1Object() => value;
    }
}
