// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.RevocationValues
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class RevocationValues : Asn1Encodable
    {
        private readonly Asn1Sequence crlVals;
        private readonly Asn1Sequence ocspVals;
        private readonly OtherRevVals otherRevVals;

        public static RevocationValues GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RevocationValues _:
                    return (RevocationValues)obj;
                default:
                    return new RevocationValues( Asn1Sequence.GetInstance( obj ) );
            }
        }

        private RevocationValues( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            if (seq.Count > 3)
                throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                Asn1Object asn1Object = asn1TaggedObject.GetObject();
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        Asn1Sequence asn1Sequence1 = (Asn1Sequence)asn1Object;
                        foreach (Asn1Encodable asn1Encodable in asn1Sequence1)
                            CertificateList.GetInstance( asn1Encodable.ToAsn1Object() );
                        this.crlVals = asn1Sequence1;
                        continue;
                    case 1:
                        Asn1Sequence asn1Sequence2 = (Asn1Sequence)asn1Object;
                        foreach (Asn1Encodable asn1Encodable in asn1Sequence2)
                            BasicOcspResponse.GetInstance( asn1Encodable.ToAsn1Object() );
                        this.ocspVals = asn1Sequence2;
                        continue;
                    case 2:
                        this.otherRevVals = OtherRevVals.GetInstance( asn1Object );
                        continue;
                    default:
                        throw new ArgumentException( "Illegal tag in RevocationValues", nameof( seq ) );
                }
            }
        }

        public RevocationValues(
          CertificateList[] crlVals,
          BasicOcspResponse[] ocspVals,
          OtherRevVals otherRevVals )
        {
            if (crlVals != null)
                this.crlVals = new DerSequence( crlVals );
            if (ocspVals != null)
                this.ocspVals = new DerSequence( ocspVals );
            this.otherRevVals = otherRevVals;
        }

        public RevocationValues( IEnumerable crlVals, IEnumerable ocspVals, OtherRevVals otherRevVals )
        {
            if (crlVals != null)
                this.crlVals = CollectionUtilities.CheckElementsAreOfType( crlVals, typeof( CertificateList ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( crlVals ) ) : throw new ArgumentException( "Must contain only 'CertificateList' objects", nameof( crlVals ) );
            if (ocspVals != null)
                this.ocspVals = CollectionUtilities.CheckElementsAreOfType( ocspVals, typeof( BasicOcspResponse ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( ocspVals ) ) : throw new ArgumentException( "Must contain only 'BasicOcspResponse' objects", nameof( ocspVals ) );
            this.otherRevVals = otherRevVals;
        }

        public CertificateList[] GetCrlVals()
        {
            CertificateList[] crlVals = new CertificateList[this.crlVals.Count];
            for (int index = 0; index < this.crlVals.Count; ++index)
                crlVals[index] = CertificateList.GetInstance( this.crlVals[index].ToAsn1Object() );
            return crlVals;
        }

        public BasicOcspResponse[] GetOcspVals()
        {
            BasicOcspResponse[] ocspVals = new BasicOcspResponse[this.ocspVals.Count];
            for (int index = 0; index < this.ocspVals.Count; ++index)
                ocspVals[index] = BasicOcspResponse.GetInstance( this.ocspVals[index].ToAsn1Object() );
            return ocspVals;
        }

        public OtherRevVals OtherRevVals => this.otherRevVals;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.crlVals != null)
                v.Add( new DerTaggedObject( true, 0, crlVals ) );
            if (this.ocspVals != null)
                v.Add( new DerTaggedObject( true, 1, ocspVals ) );
            if (this.otherRevVals != null)
                v.Add( new DerTaggedObject( true, 2, this.otherRevVals.ToAsn1Object() ) );
            return new DerSequence( v );
        }
    }
}
