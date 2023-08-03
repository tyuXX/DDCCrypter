// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.ProcurationSyntax
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class ProcurationSyntax : Asn1Encodable
    {
        private readonly string country;
        private readonly DirectoryString typeOfSubstitution;
        private readonly GeneralName thirdPerson;
        private readonly IssuerSerial certRef;

        public static ProcurationSyntax GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ProcurationSyntax _:
                    return (ProcurationSyntax)obj;
                case Asn1Sequence _:
                    return new ProcurationSyntax( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private ProcurationSyntax( Asn1Sequence seq )
        {
            if (seq.Count < 1 || seq.Count > 3)
                throw new ArgumentException( "Bad sequence size: " + seq.Count );
            foreach (object obj in seq)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( obj );
                switch (instance.TagNo)
                {
                    case 1:
                        this.country = DerPrintableString.GetInstance( instance, true ).GetString();
                        continue;
                    case 2:
                        this.typeOfSubstitution = DirectoryString.GetInstance( instance, true );
                        continue;
                    case 3:
                        Asn1Object asn1Object = instance.GetObject();
                        if (asn1Object is Asn1TaggedObject)
                        {
                            this.thirdPerson = GeneralName.GetInstance( asn1Object );
                            continue;
                        }
                        this.certRef = IssuerSerial.GetInstance( asn1Object );
                        continue;
                    default:
                        throw new ArgumentException( "Bad tag number: " + instance.TagNo );
                }
            }
        }

        public ProcurationSyntax(
          string country,
          DirectoryString typeOfSubstitution,
          IssuerSerial certRef )
        {
            this.country = country;
            this.typeOfSubstitution = typeOfSubstitution;
            this.thirdPerson = null;
            this.certRef = certRef;
        }

        public ProcurationSyntax(
          string country,
          DirectoryString typeOfSubstitution,
          GeneralName thirdPerson )
        {
            this.country = country;
            this.typeOfSubstitution = typeOfSubstitution;
            this.thirdPerson = thirdPerson;
            this.certRef = null;
        }

        public virtual string Country => this.country;

        public virtual DirectoryString TypeOfSubstitution => this.typeOfSubstitution;

        public virtual GeneralName ThirdPerson => this.thirdPerson;

        public virtual IssuerSerial CertRef => this.certRef;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.country != null)
                v.Add( new DerTaggedObject( true, 1, new DerPrintableString( this.country, true ) ) );
            if (this.typeOfSubstitution != null)
                v.Add( new DerTaggedObject( true, 2, typeOfSubstitution ) );
            if (this.thirdPerson != null)
                v.Add( new DerTaggedObject( true, 3, thirdPerson ) );
            else
                v.Add( new DerTaggedObject( true, 3, certRef ) );
            return new DerSequence( v );
        }
    }
}
