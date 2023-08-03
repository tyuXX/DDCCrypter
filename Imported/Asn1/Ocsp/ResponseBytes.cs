// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.ResponseBytes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class ResponseBytes : Asn1Encodable
    {
        private readonly DerObjectIdentifier responseType;
        private readonly Asn1OctetString response;

        public static ResponseBytes GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static ResponseBytes GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ResponseBytes _:
                    return (ResponseBytes)obj;
                case Asn1Sequence _:
                    return new ResponseBytes( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public ResponseBytes( DerObjectIdentifier responseType, Asn1OctetString response )
        {
            if (responseType == null)
                throw new ArgumentNullException( nameof( responseType ) );
            if (response == null)
                throw new ArgumentNullException( nameof( response ) );
            this.responseType = responseType;
            this.response = response;
        }

        private ResponseBytes( Asn1Sequence seq )
        {
            this.responseType = seq.Count == 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.response = Asn1OctetString.GetInstance( seq[1] );
        }

        public DerObjectIdentifier ResponseType => this.responseType;

        public Asn1OctetString Response => this.response;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       responseType,
       response
        } );
    }
}
