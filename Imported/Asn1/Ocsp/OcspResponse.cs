// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.OcspResponse
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class OcspResponse : Asn1Encodable
    {
        private readonly OcspResponseStatus responseStatus;
        private readonly ResponseBytes responseBytes;

        public static OcspResponse GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static OcspResponse GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OcspResponse _:
                    return (OcspResponse)obj;
                case Asn1Sequence _:
                    return new OcspResponse( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public OcspResponse( OcspResponseStatus responseStatus, ResponseBytes responseBytes )
        {
            this.responseStatus = responseStatus != null ? responseStatus : throw new ArgumentNullException( nameof( responseStatus ) );
            this.responseBytes = responseBytes;
        }

        private OcspResponse( Asn1Sequence seq )
        {
            this.responseStatus = new OcspResponseStatus( DerEnumerated.GetInstance( seq[0] ) );
            if (seq.Count != 2)
                return;
            this.responseBytes = ResponseBytes.GetInstance( (Asn1TaggedObject)seq[1], true );
        }

        public OcspResponseStatus ResponseStatus => this.responseStatus;

        public ResponseBytes ResponseBytes => this.responseBytes;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         responseStatus
            } );
            if (this.responseBytes != null)
                v.Add( new DerTaggedObject( true, 0, responseBytes ) );
            return new DerSequence( v );
        }
    }
}
