// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OcspListID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OcspListID : Asn1Encodable
    {
        private readonly Asn1Sequence ocspResponses;

        public static OcspListID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OcspListID _:
                    return (OcspListID)obj;
                case Asn1Sequence _:
                    return new OcspListID( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'OcspListID' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private OcspListID( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.ocspResponses = seq.Count == 1 ? (Asn1Sequence)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            foreach (Asn1Encodable ocspResponse in this.ocspResponses)
                OcspResponsesID.GetInstance( ocspResponse.ToAsn1Object() );
        }

        public OcspListID( params OcspResponsesID[] ocspResponses ) => this.ocspResponses = ocspResponses != null ? (Asn1Sequence)new DerSequence( ocspResponses ) : throw new ArgumentNullException( nameof( ocspResponses ) );

        public OcspListID( IEnumerable ocspResponses )
        {
            if (ocspResponses == null)
                throw new ArgumentNullException( nameof( ocspResponses ) );
            this.ocspResponses = CollectionUtilities.CheckElementsAreOfType( ocspResponses, typeof( OcspResponsesID ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( ocspResponses ) ) : throw new ArgumentException( "Must contain only 'OcspResponsesID' objects", nameof( ocspResponses ) );
        }

        public OcspResponsesID[] GetOcspResponses()
        {
            OcspResponsesID[] ocspResponses = new OcspResponsesID[this.ocspResponses.Count];
            for (int index = 0; index < this.ocspResponses.Count; ++index)
                ocspResponses[index] = OcspResponsesID.GetInstance( this.ocspResponses[index].ToAsn1Object() );
            return ocspResponses;
        }

        public override Asn1Object ToAsn1Object() => new DerSequence( ocspResponses );
    }
}
