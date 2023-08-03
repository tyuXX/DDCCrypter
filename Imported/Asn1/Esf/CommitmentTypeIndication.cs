// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CommitmentTypeIndication
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CommitmentTypeIndication : Asn1Encodable
    {
        private readonly DerObjectIdentifier commitmentTypeId;
        private readonly Asn1Sequence commitmentTypeQualifier;

        public static CommitmentTypeIndication GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CommitmentTypeIndication _:
                    return (CommitmentTypeIndication)obj;
                case Asn1Sequence _:
                    return new CommitmentTypeIndication( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CommitmentTypeIndication' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CommitmentTypeIndication( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.commitmentTypeId = seq.Count >= 1 && seq.Count <= 2 ? (DerObjectIdentifier)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.commitmentTypeQualifier = (Asn1Sequence)seq[1].ToAsn1Object();
        }

        public CommitmentTypeIndication( DerObjectIdentifier commitmentTypeId )
          : this( commitmentTypeId, null )
        {
        }

        public CommitmentTypeIndication(
          DerObjectIdentifier commitmentTypeId,
          Asn1Sequence commitmentTypeQualifier )
        {
            this.commitmentTypeId = commitmentTypeId != null ? commitmentTypeId : throw new ArgumentNullException( nameof( commitmentTypeId ) );
            if (commitmentTypeQualifier == null)
                return;
            this.commitmentTypeQualifier = commitmentTypeQualifier;
        }

        public DerObjectIdentifier CommitmentTypeID => this.commitmentTypeId;

        public Asn1Sequence CommitmentTypeQualifier => this.commitmentTypeQualifier;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         commitmentTypeId
            } );
            if (this.commitmentTypeQualifier != null)
                v.Add( commitmentTypeQualifier );
            return new DerSequence( v );
        }
    }
}
