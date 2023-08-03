// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.EssCertID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ess
{
    public class EssCertID : Asn1Encodable
    {
        private Asn1OctetString certHash;
        private IssuerSerial issuerSerial;

        public static EssCertID GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case EssCertID _:
                    return (EssCertID)o;
                case Asn1Sequence _:
                    return new EssCertID( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in 'EssCertID' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        public EssCertID( Asn1Sequence seq )
        {
            this.certHash = seq.Count >= 1 && seq.Count <= 2 ? Asn1OctetString.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            if (seq.Count <= 1)
                return;
            this.issuerSerial = IssuerSerial.GetInstance( seq[1] );
        }

        public EssCertID( byte[] hash ) => this.certHash = new DerOctetString( hash );

        public EssCertID( byte[] hash, IssuerSerial issuerSerial )
        {
            this.certHash = new DerOctetString( hash );
            this.issuerSerial = issuerSerial;
        }

        public byte[] GetCertHash() => this.certHash.GetOctets();

        public IssuerSerial IssuerSerial => this.issuerSerial;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         certHash
            } );
            if (this.issuerSerial != null)
                v.Add( issuerSerial );
            return new DerSequence( v );
        }
    }
}
