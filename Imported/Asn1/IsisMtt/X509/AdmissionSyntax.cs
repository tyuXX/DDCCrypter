// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.AdmissionSyntax
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class AdmissionSyntax : Asn1Encodable
    {
        private readonly GeneralName admissionAuthority;
        private readonly Asn1Sequence contentsOfAdmissions;

        public static AdmissionSyntax GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case AdmissionSyntax _:
                    return (AdmissionSyntax)obj;
                case Asn1Sequence _:
                    return new AdmissionSyntax( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private AdmissionSyntax( Asn1Sequence seq )
        {
            switch (seq.Count)
            {
                case 1:
                    this.contentsOfAdmissions = Asn1Sequence.GetInstance( seq[0] );
                    break;
                case 2:
                    this.admissionAuthority = GeneralName.GetInstance( seq[0] );
                    this.contentsOfAdmissions = Asn1Sequence.GetInstance( seq[1] );
                    break;
                default:
                    throw new ArgumentException( "Bad sequence size: " + seq.Count );
            }
        }

        public AdmissionSyntax( GeneralName admissionAuthority, Asn1Sequence contentsOfAdmissions )
        {
            this.admissionAuthority = admissionAuthority;
            this.contentsOfAdmissions = contentsOfAdmissions;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.admissionAuthority != null)
                v.Add( admissionAuthority );
            v.Add( contentsOfAdmissions );
            return new DerSequence( v );
        }

        public virtual GeneralName AdmissionAuthority => this.admissionAuthority;

        public virtual Admissions[] GetContentsOfAdmissions()
        {
            Admissions[] contentsOfAdmissions = new Admissions[this.contentsOfAdmissions.Count];
            for (int index = 0; index < this.contentsOfAdmissions.Count; ++index)
                contentsOfAdmissions[index] = Admissions.GetInstance( this.contentsOfAdmissions[index] );
            return contentsOfAdmissions;
        }
    }
}
