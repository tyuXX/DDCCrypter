// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.Admissions
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class Admissions : Asn1Encodable
    {
        private readonly GeneralName admissionAuthority;
        private readonly NamingAuthority namingAuthority;
        private readonly Asn1Sequence professionInfos;

        public static Admissions GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Admissions _:
                    return (Admissions)obj;
                case Asn1Sequence _:
                    return new Admissions( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private Admissions( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.Count <= 3 ? seq.GetEnumerator() : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            enumerator.MoveNext();
            Asn1Encodable current = (Asn1Encodable)enumerator.Current;
            if (current is Asn1TaggedObject)
            {
                switch (((Asn1TaggedObject)current).TagNo)
                {
                    case 0:
                        this.admissionAuthority = GeneralName.GetInstance( (Asn1TaggedObject)current, true );
                        break;
                    case 1:
                        this.namingAuthority = NamingAuthority.GetInstance( (Asn1TaggedObject)current, true );
                        break;
                    default:
                        throw new ArgumentException( "Bad tag number: " + ((Asn1TaggedObject)current).TagNo );
                }
                enumerator.MoveNext();
                current = (Asn1Encodable)enumerator.Current;
            }
            if (current is Asn1TaggedObject)
            {
                this.namingAuthority = ((Asn1TaggedObject)current).TagNo == 1 ? NamingAuthority.GetInstance( (Asn1TaggedObject)current, true ) : throw new ArgumentException( "Bad tag number: " + ((Asn1TaggedObject)current).TagNo );
                enumerator.MoveNext();
                current = (Asn1Encodable)enumerator.Current;
            }
            this.professionInfos = Asn1Sequence.GetInstance( current );
            if (enumerator.MoveNext())
                throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( enumerator.Current ) );
        }

        public Admissions(
          GeneralName admissionAuthority,
          NamingAuthority namingAuthority,
          ProfessionInfo[] professionInfos )
        {
            this.admissionAuthority = admissionAuthority;
            this.namingAuthority = namingAuthority;
            this.professionInfos = new DerSequence( professionInfos );
        }

        public virtual GeneralName AdmissionAuthority => this.admissionAuthority;

        public virtual NamingAuthority NamingAuthority => this.namingAuthority;

        public ProfessionInfo[] GetProfessionInfos()
        {
            ProfessionInfo[] professionInfos = new ProfessionInfo[this.professionInfos.Count];
            int num = 0;
            foreach (Asn1Encodable professionInfo in this.professionInfos)
                professionInfos[num++] = ProfessionInfo.GetInstance( professionInfo );
            return professionInfos;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.admissionAuthority != null)
                v.Add( new DerTaggedObject( true, 0, admissionAuthority ) );
            if (this.namingAuthority != null)
                v.Add( new DerTaggedObject( true, 1, namingAuthority ) );
            v.Add( professionInfos );
            return new DerSequence( v );
        }
    }
}
