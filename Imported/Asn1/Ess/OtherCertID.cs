// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.OtherCertID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ess
{
    [Obsolete( "Use version in Asn1.Esf instead" )]
    public class OtherCertID : Asn1Encodable
    {
        private Asn1Encodable otherCertHash;
        private IssuerSerial issuerSerial;

        public static OtherCertID GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case OtherCertID _:
                    return (OtherCertID)o;
                case Asn1Sequence _:
                    return new OtherCertID( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in 'OtherCertID' factory : " + Platform.GetTypeName( o ) + "." );
            }
        }

        public OtherCertID( Asn1Sequence seq )
        {
            if (seq.Count < 1 || seq.Count > 2)
                throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.otherCertHash = !(seq[0].ToAsn1Object() is Asn1OctetString) ? DigestInfo.GetInstance( seq[0] ) : (Asn1Encodable)Asn1OctetString.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            this.issuerSerial = IssuerSerial.GetInstance( Asn1Sequence.GetInstance( seq[1] ) );
        }

        public OtherCertID( AlgorithmIdentifier algId, byte[] digest ) => this.otherCertHash = new DigestInfo( algId, digest );

        public OtherCertID( AlgorithmIdentifier algId, byte[] digest, IssuerSerial issuerSerial )
        {
            this.otherCertHash = new DigestInfo( algId, digest );
            this.issuerSerial = issuerSerial;
        }

        public AlgorithmIdentifier AlgorithmHash => this.otherCertHash.ToAsn1Object() is Asn1OctetString ? new AlgorithmIdentifier( OiwObjectIdentifiers.IdSha1 ) : DigestInfo.GetInstance( otherCertHash ).AlgorithmID;

        public byte[] GetCertHash() => this.otherCertHash.ToAsn1Object() is Asn1OctetString ? ((Asn1OctetString)this.otherCertHash.ToAsn1Object()).GetOctets() : DigestInfo.GetInstance( otherCertHash ).GetDigest();

        public IssuerSerial IssuerSerial => this.issuerSerial;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
        this.otherCertHash
            } );
            if (this.issuerSerial != null)
                v.Add( issuerSerial );
            return new DerSequence( v );
        }
    }
}
