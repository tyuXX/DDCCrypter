// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.RecipientInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class RecipientInfo : Asn1Encodable, IAsn1Choice
    {
        internal Asn1Encodable info;

        public RecipientInfo( KeyTransRecipientInfo info ) => this.info = info;

        public RecipientInfo( KeyAgreeRecipientInfo info ) => this.info = new DerTaggedObject( false, 1, info );

        public RecipientInfo( KekRecipientInfo info ) => this.info = new DerTaggedObject( false, 2, info );

        public RecipientInfo( PasswordRecipientInfo info ) => this.info = new DerTaggedObject( false, 3, info );

        public RecipientInfo( OtherRecipientInfo info ) => this.info = new DerTaggedObject( false, 4, info );

        public RecipientInfo( Asn1Object info ) => this.info = info;

        public static RecipientInfo GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case RecipientInfo _:
                    return (RecipientInfo)o;
                case Asn1Sequence _:
                    return new RecipientInfo( (Asn1Object)o );
                case Asn1TaggedObject _:
                    return new RecipientInfo( (Asn1Object)o );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( o ) );
            }
        }

        public DerInteger Version
        {
            get
            {
                if (!(this.info is Asn1TaggedObject))
                    return KeyTransRecipientInfo.GetInstance( this.info ).Version;
                Asn1TaggedObject info = (Asn1TaggedObject)this.info;
                switch (info.TagNo)
                {
                    case 1:
                        return KeyAgreeRecipientInfo.GetInstance( info, false ).Version;
                    case 2:
                        return this.GetKekInfo( info ).Version;
                    case 3:
                        return PasswordRecipientInfo.GetInstance( info, false ).Version;
                    case 4:
                        return new DerInteger( 0 );
                    default:
                        throw new InvalidOperationException( "unknown tag" );
                }
            }
        }

        public bool IsTagged => this.info is Asn1TaggedObject;

        public Asn1Encodable Info
        {
            get
            {
                if (!(this.info is Asn1TaggedObject))
                    return KeyTransRecipientInfo.GetInstance( this.info );
                Asn1TaggedObject info = (Asn1TaggedObject)this.info;
                switch (info.TagNo)
                {
                    case 1:
                        return KeyAgreeRecipientInfo.GetInstance( info, false );
                    case 2:
                        return this.GetKekInfo( info );
                    case 3:
                        return PasswordRecipientInfo.GetInstance( info, false );
                    case 4:
                        return OtherRecipientInfo.GetInstance( info, false );
                    default:
                        throw new InvalidOperationException( "unknown tag" );
                }
            }
        }

        private KekRecipientInfo GetKekInfo( Asn1TaggedObject o ) => KekRecipientInfo.GetInstance( o, o.IsExplicit() );

        public override Asn1Object ToAsn1Object() => this.info.ToAsn1Object();
    }
}
