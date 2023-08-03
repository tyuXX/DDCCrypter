// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.CertBag
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class CertBag : Asn1Encodable
    {
        private readonly DerObjectIdentifier certID;
        private readonly Asn1Object certValue;

        public CertBag( Asn1Sequence seq )
        {
            this.certID = seq.Count == 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.certValue = Asn1TaggedObject.GetInstance( seq[1] ).GetObject();
        }

        public CertBag( DerObjectIdentifier certID, Asn1Object certValue )
        {
            this.certID = certID;
            this.certValue = certValue;
        }

        public DerObjectIdentifier CertID => this.certID;

        public Asn1Object CertValue => this.certValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       certID,
       new DerTaggedObject(0,  certValue)
        } );
    }
}
