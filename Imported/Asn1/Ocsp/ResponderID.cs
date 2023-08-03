// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.ResponderID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class ResponderID : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1Encodable id;

        public static ResponderID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ResponderID _:
                    return (ResponderID)obj;
                case DerOctetString _:
                    return new ResponderID( (Asn1OctetString)obj );
                case Asn1TaggedObject _:
                    Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
                    return asn1TaggedObject.TagNo == 1 ? new ResponderID( X509Name.GetInstance( asn1TaggedObject, true ) ) : new ResponderID( Asn1OctetString.GetInstance( asn1TaggedObject, true ) );
                default:
                    return new ResponderID( X509Name.GetInstance( obj ) );
            }
        }

        public ResponderID( Asn1OctetString id ) => this.id = id != null ? (Asn1Encodable)id : throw new ArgumentNullException( nameof( id ) );

        public ResponderID( X509Name id ) => this.id = id != null ? (Asn1Encodable)id : throw new ArgumentNullException( nameof( id ) );

        public static ResponderID GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( obj.GetObject() );

        public virtual byte[] GetKeyHash() => this.id is Asn1OctetString ? ((Asn1OctetString)this.id).GetOctets() : null;

        public virtual X509Name Name => this.id is Asn1OctetString ? null : X509Name.GetInstance( id );

        public override Asn1Object ToAsn1Object() => this.id is Asn1OctetString ? new DerTaggedObject( true, 2, this.id ) : (Asn1Object)new DerTaggedObject( true, 1, this.id );
    }
}
