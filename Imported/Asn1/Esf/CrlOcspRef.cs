// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CrlOcspRef
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CrlOcspRef : Asn1Encodable
    {
        private readonly CrlListID crlids;
        private readonly OcspListID ocspids;
        private readonly OtherRevRefs otherRev;

        public static CrlOcspRef GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CrlOcspRef _:
                    return (CrlOcspRef)obj;
                case Asn1Sequence _:
                    return new CrlOcspRef( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CrlOcspRef' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CrlOcspRef( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                Asn1Object asn1Object = asn1TaggedObject.GetObject();
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.crlids = CrlListID.GetInstance( asn1Object );
                        continue;
                    case 1:
                        this.ocspids = OcspListID.GetInstance( asn1Object );
                        continue;
                    case 2:
                        this.otherRev = OtherRevRefs.GetInstance( asn1Object );
                        continue;
                    default:
                        throw new ArgumentException( "Illegal tag in CrlOcspRef", nameof( seq ) );
                }
            }
        }

        public CrlOcspRef( CrlListID crlids, OcspListID ocspids, OtherRevRefs otherRev )
        {
            this.crlids = crlids;
            this.ocspids = ocspids;
            this.otherRev = otherRev;
        }

        public CrlListID CrlIDs => this.crlids;

        public OcspListID OcspIDs => this.ocspids;

        public OtherRevRefs OtherRev => this.otherRev;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.crlids != null)
                v.Add( new DerTaggedObject( true, 0, this.crlids.ToAsn1Object() ) );
            if (this.ocspids != null)
                v.Add( new DerTaggedObject( true, 1, this.ocspids.ToAsn1Object() ) );
            if (this.otherRev != null)
                v.Add( new DerTaggedObject( true, 2, this.otherRev.ToAsn1Object() ) );
            return new DerSequence( v );
        }
    }
}
