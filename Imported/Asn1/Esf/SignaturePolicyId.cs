// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.SignaturePolicyId
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class SignaturePolicyId : Asn1Encodable
    {
        private readonly DerObjectIdentifier sigPolicyIdentifier;
        private readonly OtherHashAlgAndValue sigPolicyHash;
        private readonly Asn1Sequence sigPolicyQualifiers;

        public static SignaturePolicyId GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SignaturePolicyId _:
                    return (SignaturePolicyId)obj;
                case Asn1Sequence _:
                    return new SignaturePolicyId( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'SignaturePolicyId' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private SignaturePolicyId( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.sigPolicyIdentifier = seq.Count >= 2 && seq.Count <= 3 ? (DerObjectIdentifier)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.sigPolicyHash = OtherHashAlgAndValue.GetInstance( seq[1].ToAsn1Object() );
            if (seq.Count <= 2)
                return;
            this.sigPolicyQualifiers = (Asn1Sequence)seq[2].ToAsn1Object();
        }

        public SignaturePolicyId(
          DerObjectIdentifier sigPolicyIdentifier,
          OtherHashAlgAndValue sigPolicyHash )
          : this( sigPolicyIdentifier, sigPolicyHash, null )
        {
        }

        public SignaturePolicyId(
          DerObjectIdentifier sigPolicyIdentifier,
          OtherHashAlgAndValue sigPolicyHash,
          params SigPolicyQualifierInfo[] sigPolicyQualifiers )
        {
            if (sigPolicyIdentifier == null)
                throw new ArgumentNullException( nameof( sigPolicyIdentifier ) );
            if (sigPolicyHash == null)
                throw new ArgumentNullException( nameof( sigPolicyHash ) );
            this.sigPolicyIdentifier = sigPolicyIdentifier;
            this.sigPolicyHash = sigPolicyHash;
            if (sigPolicyQualifiers == null)
                return;
            this.sigPolicyQualifiers = new DerSequence( sigPolicyQualifiers );
        }

        public SignaturePolicyId(
          DerObjectIdentifier sigPolicyIdentifier,
          OtherHashAlgAndValue sigPolicyHash,
          IEnumerable sigPolicyQualifiers )
        {
            if (sigPolicyIdentifier == null)
                throw new ArgumentNullException( nameof( sigPolicyIdentifier ) );
            if (sigPolicyHash == null)
                throw new ArgumentNullException( nameof( sigPolicyHash ) );
            this.sigPolicyIdentifier = sigPolicyIdentifier;
            this.sigPolicyHash = sigPolicyHash;
            if (sigPolicyQualifiers == null)
                return;
            this.sigPolicyQualifiers = CollectionUtilities.CheckElementsAreOfType( sigPolicyQualifiers, typeof( SigPolicyQualifierInfo ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( sigPolicyQualifiers ) ) : throw new ArgumentException( "Must contain only 'SigPolicyQualifierInfo' objects", nameof( sigPolicyQualifiers ) );
        }

        public DerObjectIdentifier SigPolicyIdentifier => this.sigPolicyIdentifier;

        public OtherHashAlgAndValue SigPolicyHash => this.sigPolicyHash;

        public SigPolicyQualifierInfo[] GetSigPolicyQualifiers()
        {
            if (this.sigPolicyQualifiers == null)
                return null;
            SigPolicyQualifierInfo[] policyQualifiers = new SigPolicyQualifierInfo[this.sigPolicyQualifiers.Count];
            for (int index = 0; index < this.sigPolicyQualifiers.Count; ++index)
                policyQualifiers[index] = SigPolicyQualifierInfo.GetInstance( this.sigPolicyQualifiers[index] );
            return policyQualifiers;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         sigPolicyIdentifier,
         this.sigPolicyHash.ToAsn1Object()
            } );
            if (this.sigPolicyQualifiers != null)
                v.Add( this.sigPolicyQualifiers.ToAsn1Object() );
            return new DerSequence( v );
        }
    }
}
