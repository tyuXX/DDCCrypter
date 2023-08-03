// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.SigI.PersonalData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509.SigI
{
    public class PersonalData : Asn1Encodable
    {
        private readonly NameOrPseudonym nameOrPseudonym;
        private readonly BigInteger nameDistinguisher;
        private readonly DerGeneralizedTime dateOfBirth;
        private readonly DirectoryString placeOfBirth;
        private readonly string gender;
        private readonly DirectoryString postalAddress;

        public static PersonalData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case PersonalData _:
                    return (PersonalData)obj;
                case Asn1Sequence _:
                    return new PersonalData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private PersonalData( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.Count >= 1 ? seq.GetEnumerator() : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            enumerator.MoveNext();
            this.nameOrPseudonym = NameOrPseudonym.GetInstance( enumerator.Current );
            while (enumerator.MoveNext())
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( enumerator.Current );
                switch (instance.TagNo)
                {
                    case 0:
                        this.nameDistinguisher = DerInteger.GetInstance( instance, false ).Value;
                        continue;
                    case 1:
                        this.dateOfBirth = DerGeneralizedTime.GetInstance( instance, false );
                        continue;
                    case 2:
                        this.placeOfBirth = DirectoryString.GetInstance( instance, true );
                        continue;
                    case 3:
                        this.gender = DerPrintableString.GetInstance( instance, false ).GetString();
                        continue;
                    case 4:
                        this.postalAddress = DirectoryString.GetInstance( instance, true );
                        continue;
                    default:
                        throw new ArgumentException( "Bad tag number: " + instance.TagNo );
                }
            }
        }

        public PersonalData(
          NameOrPseudonym nameOrPseudonym,
          BigInteger nameDistinguisher,
          DerGeneralizedTime dateOfBirth,
          DirectoryString placeOfBirth,
          string gender,
          DirectoryString postalAddress )
        {
            this.nameOrPseudonym = nameOrPseudonym;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
            this.nameDistinguisher = nameDistinguisher;
            this.postalAddress = postalAddress;
            this.placeOfBirth = placeOfBirth;
        }

        public NameOrPseudonym NameOrPseudonym => this.nameOrPseudonym;

        public BigInteger NameDistinguisher => this.nameDistinguisher;

        public DerGeneralizedTime DateOfBirth => this.dateOfBirth;

        public DirectoryString PlaceOfBirth => this.placeOfBirth;

        public string Gender => this.gender;

        public DirectoryString PostalAddress => this.postalAddress;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] )
            {
                nameOrPseudonym
            };
            if (this.nameDistinguisher != null)
                v.Add( new DerTaggedObject( false, 0, new DerInteger( this.nameDistinguisher ) ) );
            if (this.dateOfBirth != null)
                v.Add( new DerTaggedObject( false, 1, dateOfBirth ) );
            if (this.placeOfBirth != null)
                v.Add( new DerTaggedObject( true, 2, placeOfBirth ) );
            if (this.gender != null)
                v.Add( new DerTaggedObject( false, 3, new DerPrintableString( this.gender, true ) ) );
            if (this.postalAddress != null)
                v.Add( new DerTaggedObject( true, 4, postalAddress ) );
            return new DerSequence( v );
        }
    }
}
