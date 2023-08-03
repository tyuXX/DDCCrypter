// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OcspResponsesID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OcspResponsesID : Asn1Encodable
    {
        private readonly OcspIdentifier ocspIdentifier;
        private readonly OtherHash ocspRepHash;

        public static OcspResponsesID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OcspResponsesID _:
                    return (OcspResponsesID)obj;
                case Asn1Sequence _:
                    return new OcspResponsesID( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'OcspResponsesID' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private OcspResponsesID( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.ocspIdentifier = seq.Count >= 1 && seq.Count <= 2 ? OcspIdentifier.GetInstance( seq[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.ocspRepHash = OtherHash.GetInstance( seq[1].ToAsn1Object() );
        }

        public OcspResponsesID( OcspIdentifier ocspIdentifier )
          : this( ocspIdentifier, null )
        {
        }

        public OcspResponsesID( OcspIdentifier ocspIdentifier, OtherHash ocspRepHash )
        {
            this.ocspIdentifier = ocspIdentifier != null ? ocspIdentifier : throw new ArgumentNullException( nameof( ocspIdentifier ) );
            this.ocspRepHash = ocspRepHash;
        }

        public OcspIdentifier OcspIdentifier => this.ocspIdentifier;

        public OtherHash OcspRepHash => this.ocspRepHash;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         this.ocspIdentifier.ToAsn1Object()
            } );
            if (this.ocspRepHash != null)
                v.Add( this.ocspRepHash.ToAsn1Object() );
            return new DerSequence( v );
        }
    }
}
