// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CompleteCertificateRefs
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CompleteCertificateRefs : Asn1Encodable
    {
        private readonly Asn1Sequence otherCertIDs;

        public static CompleteCertificateRefs GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CompleteCertificateRefs _:
                    return (CompleteCertificateRefs)obj;
                case Asn1Sequence _:
                    return new CompleteCertificateRefs( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CompleteCertificateRefs' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CompleteCertificateRefs( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            foreach (Asn1Encodable asn1Encodable in seq)
                OtherCertID.GetInstance( asn1Encodable.ToAsn1Object() );
            this.otherCertIDs = seq;
        }

        public CompleteCertificateRefs( params OtherCertID[] otherCertIDs ) => this.otherCertIDs = otherCertIDs != null ? (Asn1Sequence)new DerSequence( otherCertIDs ) : throw new ArgumentNullException( nameof( otherCertIDs ) );

        public CompleteCertificateRefs( IEnumerable otherCertIDs )
        {
            if (otherCertIDs == null)
                throw new ArgumentNullException( nameof( otherCertIDs ) );
            this.otherCertIDs = CollectionUtilities.CheckElementsAreOfType( otherCertIDs, typeof( OtherCertID ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( otherCertIDs ) ) : throw new ArgumentException( "Must contain only 'OtherCertID' objects", nameof( otherCertIDs ) );
        }

        public OtherCertID[] GetOtherCertIDs()
        {
            OtherCertID[] otherCertIds = new OtherCertID[this.otherCertIDs.Count];
            for (int index = 0; index < this.otherCertIDs.Count; ++index)
                otherCertIds[index] = OtherCertID.GetInstance( this.otherCertIDs[index].ToAsn1Object() );
            return otherCertIds;
        }

        public override Asn1Object ToAsn1Object() => otherCertIDs;
    }
}
