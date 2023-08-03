// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CertRepMessage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CertRepMessage : Asn1Encodable
    {
        private readonly Asn1Sequence caPubs;
        private readonly Asn1Sequence response;

        private CertRepMessage( Asn1Sequence seq )
        {
            int index = 0;
            if (seq.Count > 1)
                this.caPubs = Asn1Sequence.GetInstance( (Asn1TaggedObject)seq[index++], true );
            this.response = Asn1Sequence.GetInstance( seq[index] );
        }

        public static CertRepMessage GetInstance( object obj )
        {
            switch (obj)
            {
                case CertRepMessage _:
                    return (CertRepMessage)obj;
                case Asn1Sequence _:
                    return new CertRepMessage( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertRepMessage( CmpCertificate[] caPubs, CertResponse[] response )
        {
            if (response == null)
                throw new ArgumentNullException( nameof( response ) );
            if (caPubs != null)
                this.caPubs = new DerSequence( caPubs );
            this.response = new DerSequence( response );
        }

        public virtual CmpCertificate[] GetCAPubs()
        {
            if (this.caPubs == null)
                return null;
            CmpCertificate[] caPubs = new CmpCertificate[this.caPubs.Count];
            for (int index = 0; index != caPubs.Length; ++index)
                caPubs[index] = CmpCertificate.GetInstance( this.caPubs[index] );
            return caPubs;
        }

        public virtual CertResponse[] GetResponse()
        {
            CertResponse[] response = new CertResponse[this.response.Count];
            for (int index = 0; index != response.Length; ++index)
                response[index] = CertResponse.GetInstance( this.response[index] );
            return response;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.caPubs != null)
                v.Add( new DerTaggedObject( true, 1, caPubs ) );
            v.Add( response );
            return new DerSequence( v );
        }
    }
}
