// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Holder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class Holder : Asn1Encodable
    {
        internal readonly IssuerSerial baseCertificateID;
        internal readonly GeneralNames entityName;
        internal readonly ObjectDigestInfo objectDigestInfo;
        private readonly int version;

        public static Holder GetInstance( object obj )
        {
            switch (obj)
            {
                case Holder _:
                    return (Holder)obj;
                case Asn1Sequence _:
                    return new Holder( (Asn1Sequence)obj );
                case Asn1TaggedObject _:
                    return new Holder( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Holder( Asn1TaggedObject tagObj )
        {
            switch (tagObj.TagNo)
            {
                case 0:
                    this.baseCertificateID = IssuerSerial.GetInstance( tagObj, false );
                    break;
                case 1:
                    this.entityName = GeneralNames.GetInstance( tagObj, false );
                    break;
                default:
                    throw new ArgumentException( "unknown tag in Holder" );
            }
            this.version = 0;
        }

        private Holder( Asn1Sequence seq )
        {
            if (seq.Count > 3)
                throw new ArgumentException( "Bad sequence size: " + seq.Count );
            for (int index = 0; index != seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                switch (instance.TagNo)
                {
                    case 0:
                        this.baseCertificateID = IssuerSerial.GetInstance( instance, false );
                        break;
                    case 1:
                        this.entityName = GeneralNames.GetInstance( instance, false );
                        break;
                    case 2:
                        this.objectDigestInfo = ObjectDigestInfo.GetInstance( instance, false );
                        break;
                    default:
                        throw new ArgumentException( "unknown tag in Holder" );
                }
            }
            this.version = 1;
        }

        public Holder( IssuerSerial baseCertificateID )
          : this( baseCertificateID, 1 )
        {
        }

        public Holder( IssuerSerial baseCertificateID, int version )
        {
            this.baseCertificateID = baseCertificateID;
            this.version = version;
        }

        public int Version => this.version;

        public Holder( GeneralNames entityName )
          : this( entityName, 1 )
        {
        }

        public Holder( GeneralNames entityName, int version )
        {
            this.entityName = entityName;
            this.version = version;
        }

        public Holder( ObjectDigestInfo objectDigestInfo )
        {
            this.objectDigestInfo = objectDigestInfo;
            this.version = 1;
        }

        public IssuerSerial BaseCertificateID => this.baseCertificateID;

        public GeneralNames EntityName => this.entityName;

        public ObjectDigestInfo ObjectDigestInfo => this.objectDigestInfo;

        public override Asn1Object ToAsn1Object()
        {
            if (this.version == 1)
            {
                Asn1EncodableVector v = new( new Asn1Encodable[0] );
                if (this.baseCertificateID != null)
                    v.Add( new DerTaggedObject( false, 0, baseCertificateID ) );
                if (this.entityName != null)
                    v.Add( new DerTaggedObject( false, 1, entityName ) );
                if (this.objectDigestInfo != null)
                    v.Add( new DerTaggedObject( false, 2, objectDigestInfo ) );
                return new DerSequence( v );
            }
            return this.entityName != null ? new DerTaggedObject( false, 1, entityName ) : (Asn1Object)new DerTaggedObject( false, 0, baseCertificateID );
        }
    }
}
