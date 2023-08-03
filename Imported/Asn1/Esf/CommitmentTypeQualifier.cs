// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CommitmentTypeQualifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CommitmentTypeQualifier : Asn1Encodable
    {
        private readonly DerObjectIdentifier commitmentTypeIdentifier;
        private readonly Asn1Object qualifier;

        public CommitmentTypeQualifier( DerObjectIdentifier commitmentTypeIdentifier )
          : this( commitmentTypeIdentifier, null )
        {
        }

        public CommitmentTypeQualifier(
          DerObjectIdentifier commitmentTypeIdentifier,
          Asn1Encodable qualifier )
        {
            this.commitmentTypeIdentifier = commitmentTypeIdentifier != null ? commitmentTypeIdentifier : throw new ArgumentNullException( nameof( commitmentTypeIdentifier ) );
            if (qualifier == null)
                return;
            this.qualifier = qualifier.ToAsn1Object();
        }

        public CommitmentTypeQualifier( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.commitmentTypeIdentifier = seq.Count >= 1 && seq.Count <= 2 ? (DerObjectIdentifier)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.qualifier = seq[1].ToAsn1Object();
        }

        public static CommitmentTypeQualifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CommitmentTypeQualifier _:
                    return (CommitmentTypeQualifier)obj;
                case Asn1Sequence _:
                    return new CommitmentTypeQualifier( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CommitmentTypeQualifier' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public DerObjectIdentifier CommitmentTypeIdentifier => this.commitmentTypeIdentifier;

        public Asn1Object Qualifier => this.qualifier;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         commitmentTypeIdentifier
            } );
            if (this.qualifier != null)
                v.Add( qualifier );
            return new DerSequence( v );
        }
    }
}
