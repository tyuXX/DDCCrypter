// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.RecipientIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class RecipientIdentifier : Asn1Encodable, IAsn1Choice
    {
        private Asn1Encodable id;

        public RecipientIdentifier( IssuerAndSerialNumber id ) => this.id = id;

        public RecipientIdentifier( Asn1OctetString id ) => this.id = new DerTaggedObject( false, 0, id );

        public RecipientIdentifier( Asn1Object id ) => this.id = id;

        public static RecipientIdentifier GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case RecipientIdentifier _:
                    return (RecipientIdentifier)o;
                case IssuerAndSerialNumber _:
                    return new RecipientIdentifier( (IssuerAndSerialNumber)o );
                case Asn1OctetString _:
                    return new RecipientIdentifier( (Asn1OctetString)o );
                case Asn1Object _:
                    return new RecipientIdentifier( (Asn1Object)o );
                default:
                    throw new ArgumentException( "Illegal object in RecipientIdentifier: " + Platform.GetTypeName( o ) );
            }
        }

        public bool IsTagged => this.id is Asn1TaggedObject;

        public Asn1Encodable ID => this.id is Asn1TaggedObject ? Asn1OctetString.GetInstance( (Asn1TaggedObject)this.id, false ) : (Asn1Encodable)IssuerAndSerialNumber.GetInstance( id );

        public override Asn1Object ToAsn1Object() => this.id.ToAsn1Object();
    }
}
