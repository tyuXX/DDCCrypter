// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.ProfessionInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class ProfessionInfo : Asn1Encodable
    {
        public static readonly DerObjectIdentifier Rechtsanwltin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".1" );
        public static readonly DerObjectIdentifier Rechtsanwalt = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".2" );
        public static readonly DerObjectIdentifier Rechtsbeistand = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".3" );
        public static readonly DerObjectIdentifier Steuerberaterin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".4" );
        public static readonly DerObjectIdentifier Steuerberater = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".5" );
        public static readonly DerObjectIdentifier Steuerbevollmchtigte = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".6" );
        public static readonly DerObjectIdentifier Steuerbevollmchtigter = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".7" );
        public static readonly DerObjectIdentifier Notarin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".8" );
        public static readonly DerObjectIdentifier Notar = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".9" );
        public static readonly DerObjectIdentifier Notarvertreterin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".10" );
        public static readonly DerObjectIdentifier Notarvertreter = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".11" );
        public static readonly DerObjectIdentifier Notariatsverwalterin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".12" );
        public static readonly DerObjectIdentifier Notariatsverwalter = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".13" );
        public static readonly DerObjectIdentifier Wirtschaftsprferin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".14" );
        public static readonly DerObjectIdentifier Wirtschaftsprfer = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".15" );
        public static readonly DerObjectIdentifier VereidigteBuchprferin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".16" );
        public static readonly DerObjectIdentifier VereidigterBuchprfer = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".17" );
        public static readonly DerObjectIdentifier Patentanwltin = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".18" );
        public static readonly DerObjectIdentifier Patentanwalt = new DerObjectIdentifier( NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() + ".19" );
        private readonly NamingAuthority namingAuthority;
        private readonly Asn1Sequence professionItems;
        private readonly Asn1Sequence professionOids;
        private readonly string registrationNumber;
        private readonly Asn1OctetString addProfessionInfo;

        public static ProfessionInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ProfessionInfo _:
                    return (ProfessionInfo)obj;
                case Asn1Sequence _:
                    return new ProfessionInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private ProfessionInfo( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.Count <= 5 ? seq.GetEnumerator() : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            enumerator.MoveNext();
            Asn1Encodable current1 = (Asn1Encodable)enumerator.Current;
            if (current1 is Asn1TaggedObject)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)current1;
                this.namingAuthority = asn1TaggedObject.TagNo == 0 ? NamingAuthority.GetInstance( asn1TaggedObject, true ) : throw new ArgumentException( "Bad tag number: " + asn1TaggedObject.TagNo );
                enumerator.MoveNext();
                current1 = (Asn1Encodable)enumerator.Current;
            }
            this.professionItems = Asn1Sequence.GetInstance( current1 );
            if (enumerator.MoveNext())
            {
                Asn1Encodable current2 = (Asn1Encodable)enumerator.Current;
                switch (current2)
                {
                    case Asn1Sequence _:
                        this.professionOids = Asn1Sequence.GetInstance( current2 );
                        break;
                    case DerPrintableString _:
                        this.registrationNumber = DerPrintableString.GetInstance( current2 ).GetString();
                        break;
                    case Asn1OctetString _:
                        this.addProfessionInfo = Asn1OctetString.GetInstance( current2 );
                        break;
                    default:
                        throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( current2 ) );
                }
            }
            if (enumerator.MoveNext())
            {
                Asn1Encodable current3 = (Asn1Encodable)enumerator.Current;
                switch (current3)
                {
                    case DerPrintableString _:
                        this.registrationNumber = DerPrintableString.GetInstance( current3 ).GetString();
                        break;
                    case DerOctetString _:
                        this.addProfessionInfo = (Asn1OctetString)current3;
                        break;
                    default:
                        throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( current3 ) );
                }
            }
            if (!enumerator.MoveNext())
                return;
            Asn1Encodable current4 = (Asn1Encodable)enumerator.Current;
            this.addProfessionInfo = current4 is DerOctetString ? (Asn1OctetString)current4 : throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( current4 ) );
        }

        public ProfessionInfo(
          NamingAuthority namingAuthority,
          DirectoryString[] professionItems,
          DerObjectIdentifier[] professionOids,
          string registrationNumber,
          Asn1OctetString addProfessionInfo )
        {
            this.namingAuthority = namingAuthority;
            this.professionItems = new DerSequence( professionItems );
            if (professionOids != null)
                this.professionOids = new DerSequence( professionOids );
            this.registrationNumber = registrationNumber;
            this.addProfessionInfo = addProfessionInfo;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.namingAuthority != null)
                v.Add( new DerTaggedObject( true, 0, namingAuthority ) );
            v.Add( professionItems );
            if (this.professionOids != null)
                v.Add( professionOids );
            if (this.registrationNumber != null)
                v.Add( new DerPrintableString( this.registrationNumber, true ) );
            if (this.addProfessionInfo != null)
                v.Add( addProfessionInfo );
            return new DerSequence( v );
        }

        public virtual Asn1OctetString AddProfessionInfo => this.addProfessionInfo;

        public virtual NamingAuthority NamingAuthority => this.namingAuthority;

        public virtual DirectoryString[] GetProfessionItems()
        {
            DirectoryString[] professionItems = new DirectoryString[this.professionItems.Count];
            for (int index = 0; index < this.professionItems.Count; ++index)
                professionItems[index] = DirectoryString.GetInstance( this.professionItems[index] );
            return professionItems;
        }

        public virtual DerObjectIdentifier[] GetProfessionOids()
        {
            if (this.professionOids == null)
                return new DerObjectIdentifier[0];
            DerObjectIdentifier[] professionOids = new DerObjectIdentifier[this.professionOids.Count];
            for (int index = 0; index < this.professionOids.Count; ++index)
                professionOids[index] = DerObjectIdentifier.GetInstance( this.professionOids[index] );
            return professionOids;
        }

        public virtual string RegistrationNumber => this.registrationNumber;
    }
}
