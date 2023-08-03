// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.OcspRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class OcspRequest : Asn1Encodable
    {
        private readonly TbsRequest tbsRequest;
        private readonly Signature optionalSignature;

        public static OcspRequest GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static OcspRequest GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OcspRequest _:
                    return (OcspRequest)obj;
                case Asn1Sequence _:
                    return new OcspRequest( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public OcspRequest( TbsRequest tbsRequest, Signature optionalSignature )
        {
            this.tbsRequest = tbsRequest != null ? tbsRequest : throw new ArgumentNullException( nameof( tbsRequest ) );
            this.optionalSignature = optionalSignature;
        }

        private OcspRequest( Asn1Sequence seq )
        {
            this.tbsRequest = TbsRequest.GetInstance( seq[0] );
            if (seq.Count != 2)
                return;
            this.optionalSignature = Signature.GetInstance( (Asn1TaggedObject)seq[1], true );
        }

        public TbsRequest TbsRequest => this.tbsRequest;

        public Signature OptionalSignature => this.optionalSignature;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         tbsRequest
            } );
            if (this.optionalSignature != null)
                v.Add( new DerTaggedObject( true, 0, optionalSignature ) );
            return new DerSequence( v );
        }
    }
}
