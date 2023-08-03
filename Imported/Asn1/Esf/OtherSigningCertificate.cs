// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OtherSigningCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OtherSigningCertificate : Asn1Encodable
    {
        private readonly Asn1Sequence certs;
        private readonly Asn1Sequence policies;

        public static OtherSigningCertificate GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OtherSigningCertificate _:
                    return (OtherSigningCertificate)obj;
                case Asn1Sequence _:
                    return new OtherSigningCertificate( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'OtherSigningCertificate' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private OtherSigningCertificate( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.certs = seq.Count >= 1 && seq.Count <= 2 ? Asn1Sequence.GetInstance( seq[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.policies = Asn1Sequence.GetInstance( seq[1].ToAsn1Object() );
        }

        public OtherSigningCertificate( params OtherCertID[] certs )
          : this( certs, null )
        {
        }

        public OtherSigningCertificate( OtherCertID[] certs, params PolicyInformation[] policies )
        {
            this.certs = certs != null ? (Asn1Sequence)new DerSequence( certs ) : throw new ArgumentNullException( nameof( certs ) );
            if (policies == null)
                return;
            this.policies = new DerSequence( policies );
        }

        public OtherSigningCertificate( IEnumerable certs )
          : this( certs, null )
        {
        }

        public OtherSigningCertificate( IEnumerable certs, IEnumerable policies )
        {
            if (certs == null)
                throw new ArgumentNullException( nameof( certs ) );
            this.certs = CollectionUtilities.CheckElementsAreOfType( certs, typeof( OtherCertID ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( certs ) ) : throw new ArgumentException( "Must contain only 'OtherCertID' objects", nameof( certs ) );
            if (policies == null)
                return;
            this.policies = CollectionUtilities.CheckElementsAreOfType( policies, typeof( PolicyInformation ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( policies ) ) : throw new ArgumentException( "Must contain only 'PolicyInformation' objects", nameof( policies ) );
        }

        public OtherCertID[] GetCerts()
        {
            OtherCertID[] certs = new OtherCertID[this.certs.Count];
            for (int index = 0; index < this.certs.Count; ++index)
                certs[index] = OtherCertID.GetInstance( this.certs[index].ToAsn1Object() );
            return certs;
        }

        public PolicyInformation[] GetPolicies()
        {
            if (this.policies == null)
                return null;
            PolicyInformation[] policies = new PolicyInformation[this.policies.Count];
            for (int index = 0; index < this.policies.Count; ++index)
                policies[index] = PolicyInformation.GetInstance( this.policies[index].ToAsn1Object() );
            return policies;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         certs
            } );
            if (this.policies != null)
                v.Add( policies );
            return new DerSequence( v );
        }
    }
}
