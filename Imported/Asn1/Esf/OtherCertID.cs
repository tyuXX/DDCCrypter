// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OtherCertID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OtherCertID : Asn1Encodable
    {
        private readonly OtherHash otherCertHash;
        private readonly IssuerSerial issuerSerial;

        public static OtherCertID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OtherCertID _:
                    return (OtherCertID)obj;
                case Asn1Sequence _:
                    return new OtherCertID( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'OtherCertID' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private OtherCertID( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.otherCertHash = seq.Count >= 1 && seq.Count <= 2 ? OtherHash.GetInstance( seq[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.issuerSerial = IssuerSerial.GetInstance( seq[1].ToAsn1Object() );
        }

        public OtherCertID( OtherHash otherCertHash )
          : this( otherCertHash, null )
        {
        }

        public OtherCertID( OtherHash otherCertHash, IssuerSerial issuerSerial )
        {
            this.otherCertHash = otherCertHash != null ? otherCertHash : throw new ArgumentNullException( nameof( otherCertHash ) );
            this.issuerSerial = issuerSerial;
        }

        public OtherHash OtherCertHash => this.otherCertHash;

        public IssuerSerial IssuerSerial => this.issuerSerial;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         this.otherCertHash.ToAsn1Object()
            } );
            if (this.issuerSerial != null)
                v.Add( this.issuerSerial.ToAsn1Object() );
            return new DerSequence( v );
        }
    }
}
