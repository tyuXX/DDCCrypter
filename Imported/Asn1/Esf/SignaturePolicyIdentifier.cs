// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.SignaturePolicyIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class SignaturePolicyIdentifier : Asn1Encodable, IAsn1Choice
    {
        private readonly SignaturePolicyId sigPolicy;

        public static SignaturePolicyIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SignaturePolicyIdentifier _:
                    return (SignaturePolicyIdentifier)obj;
                case SignaturePolicyId _:
                    return new SignaturePolicyIdentifier( (SignaturePolicyId)obj );
                case Asn1Null _:
                    return new SignaturePolicyIdentifier();
                default:
                    throw new ArgumentException( "Unknown object in 'SignaturePolicyIdentifier' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public SignaturePolicyIdentifier() => this.sigPolicy = null;

        public SignaturePolicyIdentifier( SignaturePolicyId signaturePolicyId ) => this.sigPolicy = signaturePolicyId != null ? signaturePolicyId : throw new ArgumentNullException( nameof( signaturePolicyId ) );

        public SignaturePolicyId SignaturePolicyId => this.sigPolicy;

        public override Asn1Object ToAsn1Object() => this.sigPolicy != null ? this.sigPolicy.ToAsn1Object() : DerNull.Instance;
    }
}
