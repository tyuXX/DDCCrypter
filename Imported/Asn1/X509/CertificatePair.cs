// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CertificatePair
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class CertificatePair : Asn1Encodable
    {
        private X509CertificateStructure forward;
        private X509CertificateStructure reverse;

        public static CertificatePair GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CertificatePair _:
                    return (CertificatePair)obj;
                case Asn1Sequence _:
                    return new CertificatePair( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CertificatePair( Asn1Sequence seq )
        {
            if (seq.Count != 1 && seq.Count != 2)
                throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            foreach (object obj in seq)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( obj );
                if (instance.TagNo == 0)
                    this.forward = X509CertificateStructure.GetInstance( instance, true );
                else
                    this.reverse = instance.TagNo == 1 ? X509CertificateStructure.GetInstance( instance, true ) : throw new ArgumentException( "Bad tag number: " + instance.TagNo );
            }
        }

        public CertificatePair( X509CertificateStructure forward, X509CertificateStructure reverse )
        {
            this.forward = forward;
            this.reverse = reverse;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.forward != null)
                v.Add( new DerTaggedObject( 0, forward ) );
            if (this.reverse != null)
                v.Add( new DerTaggedObject( 1, reverse ) );
            return new DerSequence( v );
        }

        public X509CertificateStructure Forward => this.forward;

        public X509CertificateStructure Reverse => this.reverse;
    }
}
