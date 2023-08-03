// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509Name
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1.X509
{
    public class X509Name : Asn1Encodable
    {
        public static readonly DerObjectIdentifier C = new( "2.5.4.6" );
        public static readonly DerObjectIdentifier O = new( "2.5.4.10" );
        public static readonly DerObjectIdentifier OU = new( "2.5.4.11" );
        public static readonly DerObjectIdentifier T = new( "2.5.4.12" );
        public static readonly DerObjectIdentifier CN = new( "2.5.4.3" );
        public static readonly DerObjectIdentifier Street = new( "2.5.4.9" );
        public static readonly DerObjectIdentifier SerialNumber = new( "2.5.4.5" );
        public static readonly DerObjectIdentifier L = new( "2.5.4.7" );
        public static readonly DerObjectIdentifier ST = new( "2.5.4.8" );
        public static readonly DerObjectIdentifier Surname = new( "2.5.4.4" );
        public static readonly DerObjectIdentifier GivenName = new( "2.5.4.42" );
        public static readonly DerObjectIdentifier Initials = new( "2.5.4.43" );
        public static readonly DerObjectIdentifier Generation = new( "2.5.4.44" );
        public static readonly DerObjectIdentifier UniqueIdentifier = new( "2.5.4.45" );
        public static readonly DerObjectIdentifier BusinessCategory = new( "2.5.4.15" );
        public static readonly DerObjectIdentifier PostalCode = new( "2.5.4.17" );
        public static readonly DerObjectIdentifier DnQualifier = new( "2.5.4.46" );
        public static readonly DerObjectIdentifier Pseudonym = new( "2.5.4.65" );
        public static readonly DerObjectIdentifier DateOfBirth = new( "1.3.6.1.5.5.7.9.1" );
        public static readonly DerObjectIdentifier PlaceOfBirth = new( "1.3.6.1.5.5.7.9.2" );
        public static readonly DerObjectIdentifier Gender = new( "1.3.6.1.5.5.7.9.3" );
        public static readonly DerObjectIdentifier CountryOfCitizenship = new( "1.3.6.1.5.5.7.9.4" );
        public static readonly DerObjectIdentifier CountryOfResidence = new( "1.3.6.1.5.5.7.9.5" );
        public static readonly DerObjectIdentifier NameAtBirth = new( "1.3.36.8.3.14" );
        public static readonly DerObjectIdentifier PostalAddress = new( "2.5.4.16" );
        public static readonly DerObjectIdentifier DmdName = new( "2.5.4.54" );
        public static readonly DerObjectIdentifier TelephoneNumber = X509ObjectIdentifiers.id_at_telephoneNumber;
        public static readonly DerObjectIdentifier Name = X509ObjectIdentifiers.id_at_name;
        public static readonly DerObjectIdentifier EmailAddress = PkcsObjectIdentifiers.Pkcs9AtEmailAddress;
        public static readonly DerObjectIdentifier UnstructuredName = PkcsObjectIdentifiers.Pkcs9AtUnstructuredName;
        public static readonly DerObjectIdentifier UnstructuredAddress = PkcsObjectIdentifiers.Pkcs9AtUnstructuredAddress;
        public static readonly DerObjectIdentifier E = EmailAddress;
        public static readonly DerObjectIdentifier DC = new( "0.9.2342.19200300.100.1.25" );
        public static readonly DerObjectIdentifier UID = new( "0.9.2342.19200300.100.1.1" );
        private static readonly bool[] defaultReverse = new bool[1];
        public static readonly Hashtable DefaultSymbols = new();
        public static readonly Hashtable RFC2253Symbols = new();
        public static readonly Hashtable RFC1779Symbols = new();
        public static readonly Hashtable DefaultLookup = new();
        private readonly IList ordering = Platform.CreateArrayList();
        private readonly X509NameEntryConverter converter;
        private IList values = Platform.CreateArrayList();
        private IList added = Platform.CreateArrayList();
        private Asn1Sequence seq;

        public static bool DefaultReverse
        {
            get => defaultReverse[0];
            set => defaultReverse[0] = value;
        }

        static X509Name()
        {
            DefaultSymbols.Add( C, nameof( C ) );
            DefaultSymbols.Add( O, nameof( O ) );
            DefaultSymbols.Add( T, nameof( T ) );
            DefaultSymbols.Add( OU, nameof( OU ) );
            DefaultSymbols.Add( CN, nameof( CN ) );
            DefaultSymbols.Add( L, nameof( L ) );
            DefaultSymbols.Add( ST, nameof( ST ) );
            DefaultSymbols.Add( SerialNumber, "SERIALNUMBER" );
            DefaultSymbols.Add( EmailAddress, nameof( E ) );
            DefaultSymbols.Add( DC, nameof( DC ) );
            DefaultSymbols.Add( UID, nameof( UID ) );
            DefaultSymbols.Add( Street, "STREET" );
            DefaultSymbols.Add( Surname, "SURNAME" );
            DefaultSymbols.Add( GivenName, "GIVENNAME" );
            DefaultSymbols.Add( Initials, "INITIALS" );
            DefaultSymbols.Add( Generation, "GENERATION" );
            DefaultSymbols.Add( UnstructuredAddress, "unstructuredAddress" );
            DefaultSymbols.Add( UnstructuredName, "unstructuredName" );
            DefaultSymbols.Add( UniqueIdentifier, nameof( UniqueIdentifier ) );
            DefaultSymbols.Add( DnQualifier, "DN" );
            DefaultSymbols.Add( Pseudonym, nameof( Pseudonym ) );
            DefaultSymbols.Add( PostalAddress, nameof( PostalAddress ) );
            DefaultSymbols.Add( NameAtBirth, nameof( NameAtBirth ) );
            DefaultSymbols.Add( CountryOfCitizenship, nameof( CountryOfCitizenship ) );
            DefaultSymbols.Add( CountryOfResidence, nameof( CountryOfResidence ) );
            DefaultSymbols.Add( Gender, nameof( Gender ) );
            DefaultSymbols.Add( PlaceOfBirth, nameof( PlaceOfBirth ) );
            DefaultSymbols.Add( DateOfBirth, nameof( DateOfBirth ) );
            DefaultSymbols.Add( PostalCode, nameof( PostalCode ) );
            DefaultSymbols.Add( BusinessCategory, nameof( BusinessCategory ) );
            DefaultSymbols.Add( TelephoneNumber, nameof( TelephoneNumber ) );
            RFC2253Symbols.Add( C, nameof( C ) );
            RFC2253Symbols.Add( O, nameof( O ) );
            RFC2253Symbols.Add( OU, nameof( OU ) );
            RFC2253Symbols.Add( CN, nameof( CN ) );
            RFC2253Symbols.Add( L, nameof( L ) );
            RFC2253Symbols.Add( ST, nameof( ST ) );
            RFC2253Symbols.Add( Street, "STREET" );
            RFC2253Symbols.Add( DC, nameof( DC ) );
            RFC2253Symbols.Add( UID, nameof( UID ) );
            RFC1779Symbols.Add( C, nameof( C ) );
            RFC1779Symbols.Add( O, nameof( O ) );
            RFC1779Symbols.Add( OU, nameof( OU ) );
            RFC1779Symbols.Add( CN, nameof( CN ) );
            RFC1779Symbols.Add( L, nameof( L ) );
            RFC1779Symbols.Add( ST, nameof( ST ) );
            RFC1779Symbols.Add( Street, "STREET" );
            DefaultLookup.Add( "c", C );
            DefaultLookup.Add( "o", O );
            DefaultLookup.Add( "t", T );
            DefaultLookup.Add( "ou", OU );
            DefaultLookup.Add( "cn", CN );
            DefaultLookup.Add( "l", L );
            DefaultLookup.Add( "st", ST );
            DefaultLookup.Add( "serialnumber", SerialNumber );
            DefaultLookup.Add( "street", Street );
            DefaultLookup.Add( "emailaddress", E );
            DefaultLookup.Add( "dc", DC );
            DefaultLookup.Add( "e", E );
            DefaultLookup.Add( "uid", UID );
            DefaultLookup.Add( "surname", Surname );
            DefaultLookup.Add( "givenname", GivenName );
            DefaultLookup.Add( "initials", Initials );
            DefaultLookup.Add( "generation", Generation );
            DefaultLookup.Add( "unstructuredaddress", UnstructuredAddress );
            DefaultLookup.Add( "unstructuredname", UnstructuredName );
            DefaultLookup.Add( "uniqueidentifier", UniqueIdentifier );
            DefaultLookup.Add( "dn", DnQualifier );
            DefaultLookup.Add( "pseudonym", Pseudonym );
            DefaultLookup.Add( "postaladdress", PostalAddress );
            DefaultLookup.Add( "nameofbirth", NameAtBirth );
            DefaultLookup.Add( "countryofcitizenship", CountryOfCitizenship );
            DefaultLookup.Add( "countryofresidence", CountryOfResidence );
            DefaultLookup.Add( "gender", Gender );
            DefaultLookup.Add( "placeofbirth", PlaceOfBirth );
            DefaultLookup.Add( "dateofbirth", DateOfBirth );
            DefaultLookup.Add( "postalcode", PostalCode );
            DefaultLookup.Add( "businesscategory", BusinessCategory );
            DefaultLookup.Add( "telephonenumber", TelephoneNumber );
        }

        public static X509Name GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static X509Name GetInstance( object obj )
        {
            switch (obj)
            {
                case X509Name _:
                    return (X509Name)obj;
                case null:
                    throw new ArgumentException( "null object in factory", nameof( obj ) );
                default:
                    return new X509Name( Asn1Sequence.GetInstance( obj ) );
            }
        }

        protected X509Name()
        {
        }

        protected X509Name( Asn1Sequence seq )
        {
            this.seq = seq;
            foreach (Asn1Encodable asn1Encodable in seq)
            {
                Asn1Set instance1 = Asn1Set.GetInstance( asn1Encodable.ToAsn1Object() );
                for (int index = 0; index < instance1.Count; ++index)
                {
                    Asn1Sequence instance2 = Asn1Sequence.GetInstance( instance1[index].ToAsn1Object() );
                    if (instance2.Count != 2)
                        throw new ArgumentException( "badly sized pair" );
                    this.ordering.Add( DerObjectIdentifier.GetInstance( instance2[0].ToAsn1Object() ) );
                    Asn1Object asn1Object = instance2[1].ToAsn1Object();
                    if (asn1Object is IAsn1String && !(asn1Object is DerUniversalString))
                    {
                        string source = ((IAsn1String)asn1Object).GetString();
                        if (Platform.StartsWith( source, "#" ))
                            source = "\\" + source;
                        this.values.Add( source );
                    }
                    else
                        this.values.Add( "#" + Hex.ToHexString( asn1Object.GetEncoded() ) );
                    this.added.Add( index != 0 );
                }
            }
        }

        public X509Name( IList ordering, IDictionary attributes )
          : this( ordering, attributes, new X509DefaultEntryConverter() )
        {
        }

        public X509Name( IList ordering, IDictionary attributes, X509NameEntryConverter converter )
        {
            this.converter = converter;
            foreach (DerObjectIdentifier key in (IEnumerable)ordering)
            {
                object attribute = attributes[key];
                if (attribute == null)
                    throw new ArgumentException( "No attribute for object id - " + key + " - passed to distinguished name" );
                this.ordering.Add( key );
                this.added.Add( false );
                this.values.Add( attribute );
            }
        }

        public X509Name( IList oids, IList values )
          : this( oids, values, new X509DefaultEntryConverter() )
        {
        }

        public X509Name( IList oids, IList values, X509NameEntryConverter converter )
        {
            this.converter = converter;
            if (oids.Count != values.Count)
                throw new ArgumentException( "'oids' must be same length as 'values'." );
            for (int index = 0; index < oids.Count; ++index)
            {
                this.ordering.Add( oids[index] );
                this.values.Add( values[index] );
                this.added.Add( false );
            }
        }

        public X509Name( string dirName )
          : this( DefaultReverse, DefaultLookup, dirName )
        {
        }

        public X509Name( string dirName, X509NameEntryConverter converter )
          : this( DefaultReverse, DefaultLookup, dirName, converter )
        {
        }

        public X509Name( bool reverse, string dirName )
          : this( reverse, DefaultLookup, dirName )
        {
        }

        public X509Name( bool reverse, string dirName, X509NameEntryConverter converter )
          : this( reverse, DefaultLookup, dirName, converter )
        {
        }

        public X509Name( bool reverse, IDictionary lookUp, string dirName )
          : this( reverse, lookUp, dirName, new X509DefaultEntryConverter() )
        {
        }

        private DerObjectIdentifier DecodeOid( string name, IDictionary lookUp )
        {
            if (Platform.StartsWith( Platform.ToUpperInvariant( name ), "OID." ))
                return new DerObjectIdentifier( name.Substring( 4 ) );
            if (name[0] >= '0' && name[0] <= '9')
                return new DerObjectIdentifier( name );
            return (DerObjectIdentifier)lookUp[Platform.ToLowerInvariant( name )] ?? throw new ArgumentException( "Unknown object id - " + name + " - passed to distinguished name" );
        }

        public X509Name(
          bool reverse,
          IDictionary lookUp,
          string dirName,
          X509NameEntryConverter converter )
        {
            this.converter = converter;
            X509NameTokenizer x509NameTokenizer1 = new( dirName );
            while (x509NameTokenizer1.HasMoreTokens())
            {
                string str1 = x509NameTokenizer1.NextToken();
                int length1 = str1.IndexOf( '=' );
                string name1 = length1 != -1 ? str1.Substring( 0, length1 ) : throw new ArgumentException( "badly formated directory string" );
                string oid = str1.Substring( length1 + 1 );
                DerObjectIdentifier objectIdentifier = this.DecodeOid( name1, lookUp );
                if (oid.IndexOf( '+' ) > 0)
                {
                    X509NameTokenizer x509NameTokenizer2 = new( oid, '+' );
                    string str2 = x509NameTokenizer2.NextToken();
                    this.ordering.Add( objectIdentifier );
                    this.values.Add( str2 );
                    this.added.Add( false );
                    while (x509NameTokenizer2.HasMoreTokens())
                    {
                        string str3 = x509NameTokenizer2.NextToken();
                        int length2 = str3.IndexOf( '=' );
                        string name2 = str3.Substring( 0, length2 );
                        string str4 = str3.Substring( length2 + 1 );
                        this.ordering.Add( this.DecodeOid( name2, lookUp ) );
                        this.values.Add( str4 );
                        this.added.Add( true );
                    }
                }
                else
                {
                    this.ordering.Add( objectIdentifier );
                    this.values.Add( oid );
                    this.added.Add( false );
                }
            }
            if (!reverse)
                return;
            IList arrayList1 = Platform.CreateArrayList();
            IList arrayList2 = Platform.CreateArrayList();
            IList arrayList3 = Platform.CreateArrayList();
            int num = 1;
            for (int index1 = 0; index1 < this.ordering.Count; ++index1)
            {
                if (!(bool)this.added[index1])
                    num = 0;
                int index2 = num++;
                arrayList1.Insert( index2, this.ordering[index1] );
                arrayList2.Insert( index2, this.values[index1] );
                arrayList3.Insert( index2, this.added[index1] );
            }
            this.ordering = arrayList1;
            this.values = arrayList2;
            this.added = arrayList3;
        }

        public IList GetOidList() => Platform.CreateArrayList( ordering );

        public IList GetValueList() => this.GetValueList( null );

        public IList GetValueList( DerObjectIdentifier oid )
        {
            IList arrayList = Platform.CreateArrayList();
            for (int index = 0; index != this.values.Count; ++index)
            {
                if (oid == null || oid.Equals( this.ordering[index] ))
                {
                    string source = (string)this.values[index];
                    if (Platform.StartsWith( source, "\\#" ))
                        source = source.Substring( 1 );
                    arrayList.Add( source );
                }
            }
            return arrayList;
        }

        public override Asn1Object ToAsn1Object()
        {
            if (this.seq == null)
            {
                Asn1EncodableVector v1 = new( new Asn1Encodable[0] );
                Asn1EncodableVector v2 = new( new Asn1Encodable[0] );
                DerObjectIdentifier objectIdentifier = null;
                for (int index = 0; index != this.ordering.Count; ++index)
                {
                    DerObjectIdentifier oid = (DerObjectIdentifier)this.ordering[index];
                    string str = (string)this.values[index];
                    if (objectIdentifier != null && !(bool)this.added[index])
                    {
                        v1.Add( new DerSet( v2 ) );
                        v2 = new Asn1EncodableVector( new Asn1Encodable[0] );
                    }
                    v2.Add( new DerSequence( new Asn1Encodable[2]
                    {
             oid,
             this.converter.GetConvertedValue(oid, str)
                    } ) );
                    objectIdentifier = oid;
                }
                v1.Add( new DerSet( v2 ) );
                this.seq = new DerSequence( v1 );
            }
            return seq;
        }

        public bool Equivalent( X509Name other, bool inOrder )
        {
            if (!inOrder)
                return this.Equivalent( other );
            if (other == null)
                return false;
            if (other == this)
                return true;
            int count = this.ordering.Count;
            if (count != other.ordering.Count)
                return false;
            for (int index = 0; index < count; ++index)
            {
                if (!((Asn1Encodable)this.ordering[index]).Equals( (DerObjectIdentifier)other.ordering[index] ) || !equivalentStrings( (string)this.values[index], (string)other.values[index] ))
                    return false;
            }
            return true;
        }

        public bool Equivalent( X509Name other )
        {
            if (other == null)
                return false;
            if (other == this)
                return true;
            int count = this.ordering.Count;
            if (count != other.ordering.Count)
                return false;
            bool[] flagArray = new bool[count];
            int num1;
            int num2;
            int num3;
            if (this.ordering[0].Equals( other.ordering[0] ))
            {
                num1 = 0;
                num2 = count;
                num3 = 1;
            }
            else
            {
                num1 = count - 1;
                num2 = -1;
                num3 = -1;
            }
            for (int index1 = num1; index1 != num2; index1 += num3)
            {
                bool flag = false;
                DerObjectIdentifier objectIdentifier1 = (DerObjectIdentifier)this.ordering[index1];
                string s1 = (string)this.values[index1];
                for (int index2 = 0; index2 < count; ++index2)
                {
                    if (!flagArray[index2])
                    {
                        DerObjectIdentifier objectIdentifier2 = (DerObjectIdentifier)other.ordering[index2];
                        if (objectIdentifier1.Equals( objectIdentifier2 ))
                        {
                            string s2 = (string)other.values[index2];
                            if (equivalentStrings( s1, s2 ))
                            {
                                flagArray[index2] = true;
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                if (!flag)
                    return false;
            }
            return true;
        }

        private static bool equivalentStrings( string s1, string s2 )
        {
            string str1 = canonicalize( s1 );
            string str2 = canonicalize( s2 );
            return str1.Equals( str2 ) || stripInternalSpaces( str1 ).Equals( stripInternalSpaces( str2 ) );
        }

        private static string canonicalize( string s )
        {
            string str = Platform.ToLowerInvariant( s ).Trim();
            if (Platform.StartsWith( str, "#" ))
            {
                Asn1Object asn1Object = decodeObject( str );
                if (asn1Object is IAsn1String)
                    str = Platform.ToLowerInvariant( ((IAsn1String)asn1Object).GetString() ).Trim();
            }
            return str;
        }

        private static Asn1Object decodeObject( string v )
        {
            try
            {
                return Asn1Object.FromByteArray( Hex.Decode( v.Substring( 1 ) ) );
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException( "unknown encoding in name: " + ex.Message, ex );
            }
        }

        private static string stripInternalSpaces( string str )
        {
            StringBuilder stringBuilder = new();
            if (str.Length != 0)
            {
                char ch1 = str[0];
                stringBuilder.Append( ch1 );
                for (int index = 1; index < str.Length; ++index)
                {
                    char ch2 = str[index];
                    if (ch1 != ' ' || ch2 != ' ')
                        stringBuilder.Append( ch2 );
                    ch1 = ch2;
                }
            }
            return stringBuilder.ToString();
        }

        private void AppendValue(
          StringBuilder buf,
          IDictionary oidSymbols,
          DerObjectIdentifier oid,
          string val )
        {
            string oidSymbol = (string)oidSymbols[oid];
            if (oidSymbol != null)
                buf.Append( oidSymbol );
            else
                buf.Append( oid.Id );
            buf.Append( '=' );
            int length1 = buf.Length;
            buf.Append( val );
            int length2 = buf.Length;
            if (Platform.StartsWith( val, "\\#" ))
                length1 += 2;
            for (; length1 != length2; ++length1)
            {
                if (buf[length1] == ',' || buf[length1] == '"' || buf[length1] == '\\' || buf[length1] == '+' || buf[length1] == '=' || buf[length1] == '<' || buf[length1] == '>' || buf[length1] == ';')
                {
                    buf.Insert( length1++, "\\" );
                    ++length2;
                }
            }
        }

        public string ToString( bool reverse, IDictionary oidSymbols )
        {
            ArrayList arrayList = new();
            StringBuilder buf = null;
            for (int index = 0; index < this.ordering.Count; ++index)
            {
                if ((bool)this.added[index])
                {
                    buf.Append( '+' );
                    this.AppendValue( buf, oidSymbols, (DerObjectIdentifier)this.ordering[index], (string)this.values[index] );
                }
                else
                {
                    buf = new StringBuilder();
                    this.AppendValue( buf, oidSymbols, (DerObjectIdentifier)this.ordering[index], (string)this.values[index] );
                    arrayList.Add( buf );
                }
            }
            if (reverse)
                arrayList.Reverse();
            StringBuilder stringBuilder = new();
            if (arrayList.Count > 0)
            {
                stringBuilder.Append( arrayList[0].ToString() );
                for (int index = 1; index < arrayList.Count; ++index)
                {
                    stringBuilder.Append( ',' );
                    stringBuilder.Append( arrayList[index].ToString() );
                }
            }
            return stringBuilder.ToString();
        }

        public override string ToString() => this.ToString( DefaultReverse, DefaultSymbols );
    }
}
