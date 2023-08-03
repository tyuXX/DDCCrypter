// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509Extensions
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class X509Extensions : Asn1Encodable
    {
        public static readonly DerObjectIdentifier SubjectDirectoryAttributes = new( "2.5.29.9" );
        public static readonly DerObjectIdentifier SubjectKeyIdentifier = new( "2.5.29.14" );
        public static readonly DerObjectIdentifier KeyUsage = new( "2.5.29.15" );
        public static readonly DerObjectIdentifier PrivateKeyUsagePeriod = new( "2.5.29.16" );
        public static readonly DerObjectIdentifier SubjectAlternativeName = new( "2.5.29.17" );
        public static readonly DerObjectIdentifier IssuerAlternativeName = new( "2.5.29.18" );
        public static readonly DerObjectIdentifier BasicConstraints = new( "2.5.29.19" );
        public static readonly DerObjectIdentifier CrlNumber = new( "2.5.29.20" );
        public static readonly DerObjectIdentifier ReasonCode = new( "2.5.29.21" );
        public static readonly DerObjectIdentifier InstructionCode = new( "2.5.29.23" );
        public static readonly DerObjectIdentifier InvalidityDate = new( "2.5.29.24" );
        public static readonly DerObjectIdentifier DeltaCrlIndicator = new( "2.5.29.27" );
        public static readonly DerObjectIdentifier IssuingDistributionPoint = new( "2.5.29.28" );
        public static readonly DerObjectIdentifier CertificateIssuer = new( "2.5.29.29" );
        public static readonly DerObjectIdentifier NameConstraints = new( "2.5.29.30" );
        public static readonly DerObjectIdentifier CrlDistributionPoints = new( "2.5.29.31" );
        public static readonly DerObjectIdentifier CertificatePolicies = new( "2.5.29.32" );
        public static readonly DerObjectIdentifier PolicyMappings = new( "2.5.29.33" );
        public static readonly DerObjectIdentifier AuthorityKeyIdentifier = new( "2.5.29.35" );
        public static readonly DerObjectIdentifier PolicyConstraints = new( "2.5.29.36" );
        public static readonly DerObjectIdentifier ExtendedKeyUsage = new( "2.5.29.37" );
        public static readonly DerObjectIdentifier FreshestCrl = new( "2.5.29.46" );
        public static readonly DerObjectIdentifier InhibitAnyPolicy = new( "2.5.29.54" );
        public static readonly DerObjectIdentifier AuthorityInfoAccess = new( "1.3.6.1.5.5.7.1.1" );
        public static readonly DerObjectIdentifier SubjectInfoAccess = new( "1.3.6.1.5.5.7.1.11" );
        public static readonly DerObjectIdentifier LogoType = new( "1.3.6.1.5.5.7.1.12" );
        public static readonly DerObjectIdentifier BiometricInfo = new( "1.3.6.1.5.5.7.1.2" );
        public static readonly DerObjectIdentifier QCStatements = new( "1.3.6.1.5.5.7.1.3" );
        public static readonly DerObjectIdentifier AuditIdentity = new( "1.3.6.1.5.5.7.1.4" );
        public static readonly DerObjectIdentifier NoRevAvail = new( "2.5.29.56" );
        public static readonly DerObjectIdentifier TargetInformation = new( "2.5.29.55" );
        private readonly IDictionary extensions = Platform.CreateHashtable();
        private readonly IList ordering;

        public static X509Extensions GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static X509Extensions GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case X509Extensions _:
                    return (X509Extensions)obj;
                case Asn1Sequence _:
                    return new X509Extensions( (Asn1Sequence)obj );
                case Asn1TaggedObject _:
                    return GetInstance( ((Asn1TaggedObject)obj).GetObject() );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private X509Extensions( Asn1Sequence seq )
        {
            this.ordering = Platform.CreateArrayList();
            foreach (Asn1Encodable asn1Encodable in seq)
            {
                Asn1Sequence instance1 = Asn1Sequence.GetInstance( asn1Encodable.ToAsn1Object() );
                DerObjectIdentifier key = instance1.Count >= 2 && instance1.Count <= 3 ? DerObjectIdentifier.GetInstance( instance1[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + instance1.Count );
                bool critical = instance1.Count == 3 && DerBoolean.GetInstance( instance1[1].ToAsn1Object() ).IsTrue;
                Asn1OctetString instance2 = Asn1OctetString.GetInstance( instance1[instance1.Count - 1].ToAsn1Object() );
                this.extensions.Add( key, new X509Extension( critical, instance2 ) );
                this.ordering.Add( key );
            }
        }

        public X509Extensions( IDictionary extensions )
          : this( null, extensions )
        {
        }

        public X509Extensions( IList ordering, IDictionary extensions )
        {
            this.ordering = ordering != null ? Platform.CreateArrayList( ordering ) : Platform.CreateArrayList( extensions.Keys );
            foreach (DerObjectIdentifier key in (IEnumerable)this.ordering)
                this.extensions.Add( key, (X509Extension)extensions[key] );
        }

        public X509Extensions( IList oids, IList values )
        {
            this.ordering = Platform.CreateArrayList( oids );
            int num = 0;
            foreach (DerObjectIdentifier key in (IEnumerable)this.ordering)
                this.extensions.Add( key, (X509Extension)values[num++] );
        }

        [Obsolete]
        public X509Extensions( Hashtable extensions )
          : this( null, extensions )
        {
        }

        [Obsolete]
        public X509Extensions( ArrayList ordering, Hashtable extensions )
        {
            this.ordering = ordering != null ? Platform.CreateArrayList( ordering ) : Platform.CreateArrayList( extensions.Keys );
            foreach (DerObjectIdentifier key in (IEnumerable)this.ordering)
                this.extensions.Add( key, (X509Extension)extensions[key] );
        }

        [Obsolete]
        public X509Extensions( ArrayList oids, ArrayList values )
        {
            this.ordering = Platform.CreateArrayList( oids );
            int num = 0;
            foreach (DerObjectIdentifier key in (IEnumerable)this.ordering)
                this.extensions.Add( key, (X509Extension)values[num++] );
        }

        [Obsolete( "Use ExtensionOids IEnumerable property" )]
        public IEnumerator Oids() => this.ExtensionOids.GetEnumerator();

        public IEnumerable ExtensionOids => new EnumerableProxy( ordering );

        public X509Extension GetExtension( DerObjectIdentifier oid ) => (X509Extension)this.extensions[oid];

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v1 = new( new Asn1Encodable[0] );
            foreach (DerObjectIdentifier key in (IEnumerable)this.ordering)
            {
                X509Extension extension = (X509Extension)this.extensions[key];
                Asn1EncodableVector v2 = new( new Asn1Encodable[1]
                {
           key
                } );
                if (extension.IsCritical)
                    v2.Add( DerBoolean.True );
                v2.Add( extension.Value );
                v1.Add( new DerSequence( v2 ) );
            }
            return new DerSequence( v1 );
        }

        public bool Equivalent( X509Extensions other )
        {
            if (this.extensions.Count != other.extensions.Count)
                return false;
            foreach (DerObjectIdentifier key in (IEnumerable)this.extensions.Keys)
            {
                if (!this.extensions[key].Equals( other.extensions[key] ))
                    return false;
            }
            return true;
        }

        public DerObjectIdentifier[] GetExtensionOids() => ToOidArray( this.ordering );

        public DerObjectIdentifier[] GetNonCriticalExtensionOids() => this.GetExtensionOids( false );

        public DerObjectIdentifier[] GetCriticalExtensionOids() => this.GetExtensionOids( true );

        private DerObjectIdentifier[] GetExtensionOids( bool isCritical )
        {
            IList arrayList = Platform.CreateArrayList();
            foreach (DerObjectIdentifier key in (IEnumerable)this.ordering)
            {
                if (((X509Extension)this.extensions[key]).IsCritical == isCritical)
                    arrayList.Add( key );
            }
            return ToOidArray( arrayList );
        }

        private static DerObjectIdentifier[] ToOidArray( IList oids )
        {
            DerObjectIdentifier[] oidArray = new DerObjectIdentifier[oids.Count];
            oids.CopyTo( oidArray, 0 );
            return oidArray;
        }
    }
}
