// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.ResponseData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class ResponseData : Asn1Encodable
    {
        private static readonly DerInteger V1 = new( 0 );
        private readonly bool versionPresent;
        private readonly DerInteger version;
        private readonly ResponderID responderID;
        private readonly DerGeneralizedTime producedAt;
        private readonly Asn1Sequence responses;
        private readonly X509Extensions responseExtensions;

        public static ResponseData GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static ResponseData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ResponseData _:
                    return (ResponseData)obj;
                case Asn1Sequence _:
                    return new ResponseData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public ResponseData(
          DerInteger version,
          ResponderID responderID,
          DerGeneralizedTime producedAt,
          Asn1Sequence responses,
          X509Extensions responseExtensions )
        {
            this.version = version;
            this.responderID = responderID;
            this.producedAt = producedAt;
            this.responses = responses;
            this.responseExtensions = responseExtensions;
        }

        public ResponseData(
          ResponderID responderID,
          DerGeneralizedTime producedAt,
          Asn1Sequence responses,
          X509Extensions responseExtensions )
          : this( V1, responderID, producedAt, responses, responseExtensions )
        {
        }

        private ResponseData( Asn1Sequence seq )
        {
            int num1 = 0;
            Asn1Encodable asn1Encodable = seq[0];
            if (asn1Encodable is Asn1TaggedObject)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
                if (asn1TaggedObject.TagNo == 0)
                {
                    this.versionPresent = true;
                    this.version = DerInteger.GetInstance( asn1TaggedObject, true );
                    ++num1;
                }
                else
                    this.version = V1;
            }
            else
                this.version = V1;
            Asn1Sequence asn1Sequence1 = seq;
            int index1 = num1;
            int num2 = index1 + 1;
            this.responderID = ResponderID.GetInstance( asn1Sequence1[index1] );
            Asn1Sequence asn1Sequence2 = seq;
            int index2 = num2;
            int num3 = index2 + 1;
            this.producedAt = (DerGeneralizedTime)asn1Sequence2[index2];
            Asn1Sequence asn1Sequence3 = seq;
            int index3 = num3;
            int index4 = index3 + 1;
            this.responses = (Asn1Sequence)asn1Sequence3[index3];
            if (seq.Count <= index4)
                return;
            this.responseExtensions = X509Extensions.GetInstance( (Asn1TaggedObject)seq[index4], true );
        }

        public DerInteger Version => this.version;

        public ResponderID ResponderID => this.responderID;

        public DerGeneralizedTime ProducedAt => this.producedAt;

        public Asn1Sequence Responses => this.responses;

        public X509Extensions ResponseExtensions => this.responseExtensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.versionPresent || !this.version.Equals( V1 ))
                v.Add( new DerTaggedObject( true, 0, version ) );
            v.Add( responderID, producedAt, responses );
            if (this.responseExtensions != null)
                v.Add( new DerTaggedObject( true, 1, responseExtensions ) );
            return new DerSequence( v );
        }
    }
}
