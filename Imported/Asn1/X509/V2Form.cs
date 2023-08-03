// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.V2Form
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public class V2Form : Asn1Encodable
    {
        internal GeneralNames issuerName;
        internal IssuerSerial baseCertificateID;
        internal ObjectDigestInfo objectDigestInfo;

        public static V2Form GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static V2Form GetInstance( object obj )
        {
            if (obj is V2Form)
                return (V2Form)obj;
            return obj != null ? new V2Form( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public V2Form( GeneralNames issuerName )
          : this( issuerName, null, null )
        {
        }

        public V2Form( GeneralNames issuerName, IssuerSerial baseCertificateID )
          : this( issuerName, baseCertificateID, null )
        {
        }

        public V2Form( GeneralNames issuerName, ObjectDigestInfo objectDigestInfo )
          : this( issuerName, null, objectDigestInfo )
        {
        }

        public V2Form(
          GeneralNames issuerName,
          IssuerSerial baseCertificateID,
          ObjectDigestInfo objectDigestInfo )
        {
            this.issuerName = issuerName;
            this.baseCertificateID = baseCertificateID;
            this.objectDigestInfo = objectDigestInfo;
        }

        private V2Form( Asn1Sequence seq )
        {
            if (seq.Count > 3)
                throw new ArgumentException( "Bad sequence size: " + seq.Count );
            int num = 0;
            if (!(seq[0] is Asn1TaggedObject))
            {
                ++num;
                this.issuerName = GeneralNames.GetInstance( seq[0] );
            }
            for (int index = num; index != seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                if (instance.TagNo == 0)
                    this.baseCertificateID = IssuerSerial.GetInstance( instance, false );
                else
                    this.objectDigestInfo = instance.TagNo == 1 ? ObjectDigestInfo.GetInstance( instance, false ) : throw new ArgumentException( "Bad tag number: " + instance.TagNo );
            }
        }

        public GeneralNames IssuerName => this.issuerName;

        public IssuerSerial BaseCertificateID => this.baseCertificateID;

        public ObjectDigestInfo ObjectDigestInfo => this.objectDigestInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.issuerName != null)
                v.Add( issuerName );
            if (this.baseCertificateID != null)
                v.Add( new DerTaggedObject( false, 0, baseCertificateID ) );
            if (this.objectDigestInfo != null)
                v.Add( new DerTaggedObject( false, 1, objectDigestInfo ) );
            return new DerSequence( v );
        }
    }
}
