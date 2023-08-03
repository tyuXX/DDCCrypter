// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.PrivateKeyUsagePeriod
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class PrivateKeyUsagePeriod : Asn1Encodable
    {
        private DerGeneralizedTime _notBefore;
        private DerGeneralizedTime _notAfter;

        public static PrivateKeyUsagePeriod GetInstance( object obj )
        {
            switch (obj)
            {
                case PrivateKeyUsagePeriod _:
                    return (PrivateKeyUsagePeriod)obj;
                case Asn1Sequence _:
                    return new PrivateKeyUsagePeriod( (Asn1Sequence)obj );
                case X509Extension _:
                    return GetInstance( X509Extension.ConvertValueToObject( (X509Extension)obj ) );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private PrivateKeyUsagePeriod( Asn1Sequence seq )
        {
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                if (asn1TaggedObject.TagNo == 0)
                    this._notBefore = DerGeneralizedTime.GetInstance( asn1TaggedObject, false );
                else if (asn1TaggedObject.TagNo == 1)
                    this._notAfter = DerGeneralizedTime.GetInstance( asn1TaggedObject, false );
            }
        }

        public DerGeneralizedTime NotBefore => this._notBefore;

        public DerGeneralizedTime NotAfter => this._notAfter;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this._notBefore != null)
                v.Add( new DerTaggedObject( false, 0, _notBefore ) );
            if (this._notAfter != null)
                v.Add( new DerTaggedObject( false, 1, _notAfter ) );
            return new DerSequence( v );
        }
    }
}
