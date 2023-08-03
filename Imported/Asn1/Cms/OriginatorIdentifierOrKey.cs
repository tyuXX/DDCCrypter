// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.OriginatorIdentifierOrKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class OriginatorIdentifierOrKey : Asn1Encodable, IAsn1Choice
    {
        private Asn1Encodable id;

        public OriginatorIdentifierOrKey( IssuerAndSerialNumber id ) => this.id = id;

        [Obsolete( "Use version taking a 'SubjectKeyIdentifier'" )]
        public OriginatorIdentifierOrKey( Asn1OctetString id )
          : this( new SubjectKeyIdentifier( id ) )
        {
        }

        public OriginatorIdentifierOrKey( SubjectKeyIdentifier id ) => this.id = new DerTaggedObject( false, 0, id );

        public OriginatorIdentifierOrKey( OriginatorPublicKey id ) => this.id = new DerTaggedObject( false, 1, id );

        [Obsolete( "Use more specific version" )]
        public OriginatorIdentifierOrKey( Asn1Object id ) => this.id = id;

        private OriginatorIdentifierOrKey( Asn1TaggedObject id ) => this.id = id;

        public static OriginatorIdentifierOrKey GetInstance( Asn1TaggedObject o, bool explicitly )
        {
            if (!explicitly)
                throw new ArgumentException( "Can't implicitly tag OriginatorIdentifierOrKey" );
            return GetInstance( o.GetObject() );
        }

        public static OriginatorIdentifierOrKey GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case OriginatorIdentifierOrKey _:
                    return (OriginatorIdentifierOrKey)o;
                case IssuerAndSerialNumber _:
                    return new OriginatorIdentifierOrKey( (IssuerAndSerialNumber)o );
                case SubjectKeyIdentifier _:
                    return new OriginatorIdentifierOrKey( (SubjectKeyIdentifier)o );
                case OriginatorPublicKey _:
                    return new OriginatorIdentifierOrKey( (OriginatorPublicKey)o );
                case Asn1TaggedObject _:
                    return new OriginatorIdentifierOrKey( (Asn1TaggedObject)o );
                default:
                    throw new ArgumentException( "Invalid OriginatorIdentifierOrKey: " + Platform.GetTypeName( o ) );
            }
        }

        public Asn1Encodable ID => this.id;

        public IssuerAndSerialNumber IssuerAndSerialNumber => this.id is IssuerAndSerialNumber ? (IssuerAndSerialNumber)this.id : null;

        public SubjectKeyIdentifier SubjectKeyIdentifier => this.id is Asn1TaggedObject && ((Asn1TaggedObject)this.id).TagNo == 0 ? SubjectKeyIdentifier.GetInstance( (Asn1TaggedObject)this.id, false ) : null;

        [Obsolete( "Use 'OriginatorPublicKey' property" )]
        public OriginatorPublicKey OriginatorKey => this.OriginatorPublicKey;

        public OriginatorPublicKey OriginatorPublicKey => this.id is Asn1TaggedObject && ((Asn1TaggedObject)this.id).TagNo == 1 ? OriginatorPublicKey.GetInstance( (Asn1TaggedObject)this.id, false ) : null;

        public override Asn1Object ToAsn1Object() => this.id.ToAsn1Object();
    }
}
