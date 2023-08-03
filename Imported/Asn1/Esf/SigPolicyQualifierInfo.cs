// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.SigPolicyQualifierInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class SigPolicyQualifierInfo : Asn1Encodable
    {
        private readonly DerObjectIdentifier sigPolicyQualifierId;
        private readonly Asn1Object sigQualifier;

        public static SigPolicyQualifierInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SigPolicyQualifierInfo _:
                    return (SigPolicyQualifierInfo)obj;
                case Asn1Sequence _:
                    return new SigPolicyQualifierInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'SigPolicyQualifierInfo' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private SigPolicyQualifierInfo( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.sigPolicyQualifierId = seq.Count == 2 ? (DerObjectIdentifier)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.sigQualifier = seq[1].ToAsn1Object();
        }

        public SigPolicyQualifierInfo(
          DerObjectIdentifier sigPolicyQualifierId,
          Asn1Encodable sigQualifier )
        {
            this.sigPolicyQualifierId = sigPolicyQualifierId;
            this.sigQualifier = sigQualifier.ToAsn1Object();
        }

        public DerObjectIdentifier SigPolicyQualifierId => this.sigPolicyQualifierId;

        public Asn1Object SigQualifier => this.sigQualifier;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       sigPolicyQualifierId,
       sigQualifier
        } );
    }
}
