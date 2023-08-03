// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.SignerIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class SignerIdentifier : Asn1Encodable, IAsn1Choice
    {
        private Asn1Encodable id;

        public SignerIdentifier( IssuerAndSerialNumber id ) => this.id = id;

        public SignerIdentifier( Asn1OctetString id ) => this.id = new DerTaggedObject( false, 0, id );

        public SignerIdentifier( Asn1Object id ) => this.id = id;

        public static SignerIdentifier GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case SignerIdentifier _:
                    return (SignerIdentifier)o;
                case IssuerAndSerialNumber _:
                    return new SignerIdentifier( (IssuerAndSerialNumber)o );
                case Asn1OctetString _:
                    return new SignerIdentifier( (Asn1OctetString)o );
                case Asn1Object _:
                    return new SignerIdentifier( (Asn1Object)o );
                default:
                    throw new ArgumentException( "Illegal object in SignerIdentifier: " + Platform.GetTypeName( o ) );
            }
        }

        public bool IsTagged => this.id is Asn1TaggedObject;

        public Asn1Encodable ID => this.id is Asn1TaggedObject ? Asn1OctetString.GetInstance( (Asn1TaggedObject)this.id, false ) : this.id;

        public override Asn1Object ToAsn1Object() => this.id.ToAsn1Object();
    }
}
