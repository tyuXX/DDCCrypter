// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixNameConstraintValidator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    public class PkixNameConstraintValidator
    {
        private ISet excludedSubtreesDN = new HashSet();
        private ISet excludedSubtreesDNS = new HashSet();
        private ISet excludedSubtreesEmail = new HashSet();
        private ISet excludedSubtreesURI = new HashSet();
        private ISet excludedSubtreesIP = new HashSet();
        private ISet permittedSubtreesDN;
        private ISet permittedSubtreesDNS;
        private ISet permittedSubtreesEmail;
        private ISet permittedSubtreesURI;
        private ISet permittedSubtreesIP;

        private static bool WithinDNSubtree( Asn1Sequence dns, Asn1Sequence subtree )
        {
            if (subtree.Count < 1 || subtree.Count > dns.Count)
                return false;
            for (int index = subtree.Count - 1; index >= 0; --index)
            {
                if (!subtree[index].Equals( dns[index] ))
                    return false;
            }
            return true;
        }

        public void CheckPermittedDN( Asn1Sequence dns ) => this.CheckPermittedDN( this.permittedSubtreesDN, dns );

        public void CheckExcludedDN( Asn1Sequence dns ) => this.CheckExcludedDN( this.excludedSubtreesDN, dns );

        private void CheckPermittedDN( ISet permitted, Asn1Sequence dns )
        {
            if (permitted != null && (permitted.Count != 0 || dns.Count != 0))
            {
                foreach (Asn1Sequence subtree in (IEnumerable)permitted)
                {
                    if (WithinDNSubtree( dns, subtree ))
                        return;
                }
                throw new PkixNameConstraintValidatorException( "Subject distinguished name is not from a permitted subtree" );
            }
        }

        private void CheckExcludedDN( ISet excluded, Asn1Sequence dns )
        {
            if (excluded.IsEmpty)
                return;
            foreach (Asn1Sequence subtree in (IEnumerable)excluded)
            {
                if (WithinDNSubtree( dns, subtree ))
                    throw new PkixNameConstraintValidatorException( "Subject distinguished name is from an excluded subtree" );
            }
        }

        private ISet IntersectDN( ISet permitted, ISet dns )
        {
            ISet set = new HashSet();
            foreach (GeneralSubtree dn in (IEnumerable)dns)
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance( dn.Base.Name.ToAsn1Object() );
                if (permitted == null)
                {
                    if (instance != null)
                        set.Add( instance );
                }
                else
                {
                    foreach (Asn1Sequence asn1Sequence in (IEnumerable)permitted)
                    {
                        if (WithinDNSubtree( instance, asn1Sequence ))
                            set.Add( instance );
                        else if (WithinDNSubtree( asn1Sequence, instance ))
                            set.Add( asn1Sequence );
                    }
                }
            }
            return set;
        }

        private ISet UnionDN( ISet excluded, Asn1Sequence dn )
        {
            if (excluded.IsEmpty)
            {
                if (dn == null)
                    return excluded;
                excluded.Add( dn );
                return excluded;
            }
            ISet set = new HashSet();
            foreach (Asn1Sequence asn1Sequence in (IEnumerable)excluded)
            {
                if (WithinDNSubtree( dn, asn1Sequence ))
                    set.Add( asn1Sequence );
                else if (WithinDNSubtree( asn1Sequence, dn ))
                {
                    set.Add( dn );
                }
                else
                {
                    set.Add( asn1Sequence );
                    set.Add( dn );
                }
            }
            return set;
        }

        private ISet IntersectEmail( ISet permitted, ISet emails )
        {
            ISet intersect = new HashSet();
            foreach (GeneralSubtree email in (IEnumerable)emails)
            {
                string nameAsString = this.ExtractNameAsString( email.Base );
                if (permitted == null)
                {
                    if (nameAsString != null)
                        intersect.Add( nameAsString );
                }
                else
                {
                    foreach (string email2 in (IEnumerable)permitted)
                        this.intersectEmail( nameAsString, email2, intersect );
                }
            }
            return intersect;
        }

        private ISet UnionEmail( ISet excluded, string email )
        {
            if (excluded.IsEmpty)
            {
                if (email == null)
                    return excluded;
                excluded.Add( email );
                return excluded;
            }
            ISet union = new HashSet();
            foreach (string email1 in (IEnumerable)excluded)
                this.unionEmail( email1, email, union );
            return union;
        }

        private ISet IntersectIP( ISet permitted, ISet ips )
        {
            ISet set = new HashSet();
            foreach (GeneralSubtree ip in (IEnumerable)ips)
            {
                byte[] octets = Asn1OctetString.GetInstance( ip.Base.Name ).GetOctets();
                if (permitted == null)
                {
                    if (octets != null)
                        set.Add( octets );
                }
                else
                {
                    foreach (byte[] ipWithSubmask1 in (IEnumerable)permitted)
                        set.AddAll( this.IntersectIPRange( ipWithSubmask1, octets ) );
                }
            }
            return set;
        }

        private ISet UnionIP( ISet excluded, byte[] ip )
        {
            if (excluded.IsEmpty)
            {
                if (ip == null)
                    return excluded;
                excluded.Add( ip );
                return excluded;
            }
            ISet set = new HashSet();
            foreach (byte[] ipWithSubmask1 in (IEnumerable)excluded)
                set.AddAll( this.UnionIPRange( ipWithSubmask1, ip ) );
            return set;
        }

        private ISet UnionIPRange( byte[] ipWithSubmask1, byte[] ipWithSubmask2 )
        {
            ISet set = new HashSet();
            if (Arrays.AreEqual( ipWithSubmask1, ipWithSubmask2 ))
            {
                set.Add( ipWithSubmask1 );
            }
            else
            {
                set.Add( ipWithSubmask1 );
                set.Add( ipWithSubmask2 );
            }
            return set;
        }

        private ISet IntersectIPRange( byte[] ipWithSubmask1, byte[] ipWithSubmask2 )
        {
            if (ipWithSubmask1.Length != ipWithSubmask2.Length)
                return new HashSet();
            byte[][] ipsAndSubnetMasks = this.ExtractIPsAndSubnetMasks( ipWithSubmask1, ipWithSubmask2 );
            byte[] ip1 = ipsAndSubnetMasks[0];
            byte[] numArray1 = ipsAndSubnetMasks[1];
            byte[] ip2_1 = ipsAndSubnetMasks[2];
            byte[] numArray2 = ipsAndSubnetMasks[3];
            byte[][] numArray3 = this.MinMaxIPs( ip1, numArray1, ip2_1, numArray2 );
            byte[] ip2_2 = Min( numArray3[1], numArray3[3] );
            if (CompareTo( Max( numArray3[0], numArray3[2] ), ip2_2 ) == 1)
                return new HashSet();
            byte[] ip = Or( numArray3[0], numArray3[2] );
            byte[] subnetMask = Or( numArray1, numArray2 );
            ISet set = new HashSet
            {
                this.IpWithSubnetMask( ip, subnetMask )
            };
            return set;
        }

        private byte[] IpWithSubnetMask( byte[] ip, byte[] subnetMask )
        {
            int length = ip.Length;
            byte[] destinationArray = new byte[length * 2];
            Array.Copy( ip, 0, destinationArray, 0, length );
            Array.Copy( subnetMask, 0, destinationArray, length, length );
            return destinationArray;
        }

        private byte[][] ExtractIPsAndSubnetMasks( byte[] ipWithSubmask1, byte[] ipWithSubmask2 )
        {
            int length = ipWithSubmask1.Length / 2;
            byte[] destinationArray1 = new byte[length];
            byte[] destinationArray2 = new byte[length];
            Array.Copy( ipWithSubmask1, 0, destinationArray1, 0, length );
            Array.Copy( ipWithSubmask1, length, destinationArray2, 0, length );
            byte[] destinationArray3 = new byte[length];
            byte[] destinationArray4 = new byte[length];
            Array.Copy( ipWithSubmask2, 0, destinationArray3, 0, length );
            Array.Copy( ipWithSubmask2, length, destinationArray4, 0, length );
            return new byte[4][]
            {
        destinationArray1,
        destinationArray2,
        destinationArray3,
        destinationArray4
            };
        }

        private byte[][] MinMaxIPs( byte[] ip1, byte[] subnetmask1, byte[] ip2, byte[] subnetmask2 )
        {
            int length = ip1.Length;
            byte[] numArray1 = new byte[length];
            byte[] numArray2 = new byte[length];
            byte[] numArray3 = new byte[length];
            byte[] numArray4 = new byte[length];
            for (int index = 0; index < length; ++index)
            {
                numArray1[index] = (byte)(ip1[index] & (uint)subnetmask1[index]);
                numArray2[index] = (byte)((ip1[index] & (uint)subnetmask1[index]) | (uint)~subnetmask1[index]);
                numArray3[index] = (byte)(ip2[index] & (uint)subnetmask2[index]);
                numArray4[index] = (byte)((ip2[index] & (uint)subnetmask2[index]) | (uint)~subnetmask2[index]);
            }
            return new byte[4][]
            {
        numArray1,
        numArray2,
        numArray3,
        numArray4
            };
        }

        private void CheckPermittedEmail( ISet permitted, string email )
        {
            if (permitted == null)
                return;
            foreach (string constraint in (IEnumerable)permitted)
            {
                if (this.EmailIsConstrained( email, constraint ))
                    return;
            }
            if (email.Length != 0 || permitted.Count != 0)
                throw new PkixNameConstraintValidatorException( "Subject email address is not from a permitted subtree." );
        }

        private void CheckExcludedEmail( ISet excluded, string email )
        {
            if (excluded.IsEmpty)
                return;
            foreach (string constraint in (IEnumerable)excluded)
            {
                if (this.EmailIsConstrained( email, constraint ))
                    throw new PkixNameConstraintValidatorException( "Email address is from an excluded subtree." );
            }
        }

        private void CheckPermittedIP( ISet permitted, byte[] ip )
        {
            if (permitted == null)
                return;
            foreach (byte[] constraint in (IEnumerable)permitted)
            {
                if (this.IsIPConstrained( ip, constraint ))
                    return;
            }
            if (ip.Length != 0 || permitted.Count != 0)
                throw new PkixNameConstraintValidatorException( "IP is not from a permitted subtree." );
        }

        private void checkExcludedIP( ISet excluded, byte[] ip )
        {
            if (excluded.IsEmpty)
                return;
            foreach (byte[] constraint in (IEnumerable)excluded)
            {
                if (this.IsIPConstrained( ip, constraint ))
                    throw new PkixNameConstraintValidatorException( "IP is from an excluded subtree." );
            }
        }

        private bool IsIPConstrained( byte[] ip, byte[] constraint )
        {
            int length = ip.Length;
            if (length != constraint.Length / 2)
                return false;
            byte[] destinationArray = new byte[length];
            Array.Copy( constraint, length, destinationArray, 0, length );
            byte[] a = new byte[length];
            byte[] b = new byte[length];
            for (int index = 0; index < length; ++index)
            {
                a[index] = (byte)(constraint[index] & (uint)destinationArray[index]);
                b[index] = (byte)(ip[index] & (uint)destinationArray[index]);
            }
            return Arrays.AreEqual( a, b );
        }

        private bool EmailIsConstrained( string email, string constraint )
        {
            string str = email.Substring( email.IndexOf( '@' ) + 1 );
            if (constraint.IndexOf( '@' ) != -1)
            {
                if (Platform.ToUpperInvariant( email ).Equals( Platform.ToUpperInvariant( constraint ) ))
                    return true;
            }
            else if (!constraint[0].Equals( (object)'.' ))
            {
                if (Platform.ToUpperInvariant( str ).Equals( Platform.ToUpperInvariant( constraint ) ))
                    return true;
            }
            else if (this.WithinDomain( str, constraint ))
                return true;
            return false;
        }

        private bool WithinDomain( string testDomain, string domain )
        {
            string source = domain;
            if (Platform.StartsWith( source, "." ))
                source = source.Substring( 1 );
            string[] strArray1 = source.Split( '.' );
            string[] strArray2 = testDomain.Split( '.' );
            if (strArray2.Length <= strArray1.Length)
                return false;
            int num = strArray2.Length - strArray1.Length;
            for (int index = -1; index < strArray1.Length; ++index)
            {
                if (index == -1)
                {
                    if (strArray2[index + num].Equals( "" ))
                        return false;
                }
                else if (!Platform.EqualsIgnoreCase( strArray2[index + num], strArray1[index] ))
                    return false;
            }
            return true;
        }

        private void CheckPermittedDNS( ISet permitted, string dns )
        {
            if (permitted == null)
                return;
            foreach (string str in (IEnumerable)permitted)
            {
                if (this.WithinDomain( dns, str ) || Platform.ToUpperInvariant( dns ).Equals( Platform.ToUpperInvariant( str ) ))
                    return;
            }
            if (dns.Length != 0 || permitted.Count != 0)
                throw new PkixNameConstraintValidatorException( "DNS is not from a permitted subtree." );
        }

        private void checkExcludedDNS( ISet excluded, string dns )
        {
            if (excluded.IsEmpty)
                return;
            foreach (string str in (IEnumerable)excluded)
            {
                if (this.WithinDomain( dns, str ) || Platform.EqualsIgnoreCase( dns, str ))
                    throw new PkixNameConstraintValidatorException( "DNS is from an excluded subtree." );
            }
        }

        private void unionEmail( string email1, string email2, ISet union )
        {
            if (email1.IndexOf( '@' ) != -1)
            {
                string str = email1.Substring( email1.IndexOf( '@' ) + 1 );
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (Platform.EqualsIgnoreCase( email1, email2 ))
                    {
                        union.Add( email1 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (this.WithinDomain( str, email2 ))
                    {
                        union.Add( email2 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (Platform.EqualsIgnoreCase( str, email2 ))
                {
                    union.Add( email2 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (Platform.StartsWith( email1, "." ))
            {
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (this.WithinDomain( email2.Substring( email1.IndexOf( '@' ) + 1 ), email1 ))
                    {
                        union.Add( email1 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (this.WithinDomain( email1, email2 ) || Platform.EqualsIgnoreCase( email1, email2 ))
                        union.Add( email2 );
                    else if (this.WithinDomain( email2, email1 ))
                    {
                        union.Add( email1 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (this.WithinDomain( email2, email1 ))
                {
                    union.Add( email1 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (email2.IndexOf( '@' ) != -1)
            {
                if (Platform.EqualsIgnoreCase( email2.Substring( email1.IndexOf( '@' ) + 1 ), email1 ))
                {
                    union.Add( email1 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (Platform.StartsWith( email2, "." ))
            {
                if (this.WithinDomain( email1, email2 ))
                {
                    union.Add( email2 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (Platform.EqualsIgnoreCase( email1, email2 ))
            {
                union.Add( email1 );
            }
            else
            {
                union.Add( email1 );
                union.Add( email2 );
            }
        }

        private void unionURI( string email1, string email2, ISet union )
        {
            if (email1.IndexOf( '@' ) != -1)
            {
                string str = email1.Substring( email1.IndexOf( '@' ) + 1 );
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (Platform.EqualsIgnoreCase( email1, email2 ))
                    {
                        union.Add( email1 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (this.WithinDomain( str, email2 ))
                    {
                        union.Add( email2 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (Platform.EqualsIgnoreCase( str, email2 ))
                {
                    union.Add( email2 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (Platform.StartsWith( email1, "." ))
            {
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (this.WithinDomain( email2.Substring( email1.IndexOf( '@' ) + 1 ), email1 ))
                    {
                        union.Add( email1 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (this.WithinDomain( email1, email2 ) || Platform.EqualsIgnoreCase( email1, email2 ))
                        union.Add( email2 );
                    else if (this.WithinDomain( email2, email1 ))
                    {
                        union.Add( email1 );
                    }
                    else
                    {
                        union.Add( email1 );
                        union.Add( email2 );
                    }
                }
                else if (this.WithinDomain( email2, email1 ))
                {
                    union.Add( email1 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (email2.IndexOf( '@' ) != -1)
            {
                if (Platform.EqualsIgnoreCase( email2.Substring( email1.IndexOf( '@' ) + 1 ), email1 ))
                {
                    union.Add( email1 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (Platform.StartsWith( email2, "." ))
            {
                if (this.WithinDomain( email1, email2 ))
                {
                    union.Add( email2 );
                }
                else
                {
                    union.Add( email1 );
                    union.Add( email2 );
                }
            }
            else if (Platform.EqualsIgnoreCase( email1, email2 ))
            {
                union.Add( email1 );
            }
            else
            {
                union.Add( email1 );
                union.Add( email2 );
            }
        }

        private ISet intersectDNS( ISet permitted, ISet dnss )
        {
            ISet set = new HashSet();
            foreach (GeneralSubtree generalSubtree in (IEnumerable)dnss)
            {
                string nameAsString = this.ExtractNameAsString( generalSubtree.Base );
                if (permitted == null)
                {
                    if (nameAsString != null)
                        set.Add( nameAsString );
                }
                else
                {
                    foreach (string str in (IEnumerable)permitted)
                    {
                        if (this.WithinDomain( str, nameAsString ))
                            set.Add( str );
                        else if (this.WithinDomain( nameAsString, str ))
                            set.Add( nameAsString );
                    }
                }
            }
            return set;
        }

        protected ISet unionDNS( ISet excluded, string dns )
        {
            if (excluded.IsEmpty)
            {
                if (dns == null)
                    return excluded;
                excluded.Add( dns );
                return excluded;
            }
            ISet set = new HashSet();
            foreach (string str in (IEnumerable)excluded)
            {
                if (this.WithinDomain( str, dns ))
                    set.Add( dns );
                else if (this.WithinDomain( dns, str ))
                {
                    set.Add( str );
                }
                else
                {
                    set.Add( str );
                    set.Add( dns );
                }
            }
            return set;
        }

        private void intersectEmail( string email1, string email2, ISet intersect )
        {
            if (email1.IndexOf( '@' ) != -1)
            {
                string str = email1.Substring( email1.IndexOf( '@' ) + 1 );
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (!Platform.EqualsIgnoreCase( email1, email2 ))
                        return;
                    intersect.Add( email1 );
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (!this.WithinDomain( str, email2 ))
                        return;
                    intersect.Add( email1 );
                }
                else
                {
                    if (!Platform.EqualsIgnoreCase( str, email2 ))
                        return;
                    intersect.Add( email1 );
                }
            }
            else if (Platform.StartsWith( email1, "." ))
            {
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (!this.WithinDomain( email2.Substring( email1.IndexOf( '@' ) + 1 ), email1 ))
                        return;
                    intersect.Add( email2 );
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (this.WithinDomain( email1, email2 ) || Platform.EqualsIgnoreCase( email1, email2 ))
                    {
                        intersect.Add( email1 );
                    }
                    else
                    {
                        if (!this.WithinDomain( email2, email1 ))
                            return;
                        intersect.Add( email2 );
                    }
                }
                else
                {
                    if (!this.WithinDomain( email2, email1 ))
                        return;
                    intersect.Add( email2 );
                }
            }
            else if (email2.IndexOf( '@' ) != -1)
            {
                if (!Platform.EqualsIgnoreCase( email2.Substring( email2.IndexOf( '@' ) + 1 ), email1 ))
                    return;
                intersect.Add( email2 );
            }
            else if (Platform.StartsWith( email2, "." ))
            {
                if (!this.WithinDomain( email1, email2 ))
                    return;
                intersect.Add( email1 );
            }
            else
            {
                if (!Platform.EqualsIgnoreCase( email1, email2 ))
                    return;
                intersect.Add( email1 );
            }
        }

        private void checkExcludedURI( ISet excluded, string uri )
        {
            if (excluded.IsEmpty)
                return;
            foreach (string constraint in (IEnumerable)excluded)
            {
                if (this.IsUriConstrained( uri, constraint ))
                    throw new PkixNameConstraintValidatorException( "URI is from an excluded subtree." );
            }
        }

        private ISet intersectURI( ISet permitted, ISet uris )
        {
            ISet intersect = new HashSet();
            foreach (GeneralSubtree uri in (IEnumerable)uris)
            {
                string nameAsString = this.ExtractNameAsString( uri.Base );
                if (permitted == null)
                {
                    if (nameAsString != null)
                        intersect.Add( nameAsString );
                }
                else
                {
                    foreach (string email1 in (IEnumerable)permitted)
                        this.intersectURI( email1, nameAsString, intersect );
                }
            }
            return intersect;
        }

        private ISet unionURI( ISet excluded, string uri )
        {
            if (excluded.IsEmpty)
            {
                if (uri == null)
                    return excluded;
                excluded.Add( uri );
                return excluded;
            }
            ISet union = new HashSet();
            foreach (string email1 in (IEnumerable)excluded)
                this.unionURI( email1, uri, union );
            return union;
        }

        private void intersectURI( string email1, string email2, ISet intersect )
        {
            if (email1.IndexOf( '@' ) != -1)
            {
                string str = email1.Substring( email1.IndexOf( '@' ) + 1 );
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (!Platform.EqualsIgnoreCase( email1, email2 ))
                        return;
                    intersect.Add( email1 );
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (!this.WithinDomain( str, email2 ))
                        return;
                    intersect.Add( email1 );
                }
                else
                {
                    if (!Platform.EqualsIgnoreCase( str, email2 ))
                        return;
                    intersect.Add( email1 );
                }
            }
            else if (Platform.StartsWith( email1, "." ))
            {
                if (email2.IndexOf( '@' ) != -1)
                {
                    if (!this.WithinDomain( email2.Substring( email1.IndexOf( '@' ) + 1 ), email1 ))
                        return;
                    intersect.Add( email2 );
                }
                else if (Platform.StartsWith( email2, "." ))
                {
                    if (this.WithinDomain( email1, email2 ) || Platform.EqualsIgnoreCase( email1, email2 ))
                    {
                        intersect.Add( email1 );
                    }
                    else
                    {
                        if (!this.WithinDomain( email2, email1 ))
                            return;
                        intersect.Add( email2 );
                    }
                }
                else
                {
                    if (!this.WithinDomain( email2, email1 ))
                        return;
                    intersect.Add( email2 );
                }
            }
            else if (email2.IndexOf( '@' ) != -1)
            {
                if (!Platform.EqualsIgnoreCase( email2.Substring( email2.IndexOf( '@' ) + 1 ), email1 ))
                    return;
                intersect.Add( email2 );
            }
            else if (Platform.StartsWith( email2, "." ))
            {
                if (!this.WithinDomain( email1, email2 ))
                    return;
                intersect.Add( email1 );
            }
            else
            {
                if (!Platform.EqualsIgnoreCase( email1, email2 ))
                    return;
                intersect.Add( email1 );
            }
        }

        private void CheckPermittedURI( ISet permitted, string uri )
        {
            if (permitted == null)
                return;
            foreach (string constraint in (IEnumerable)permitted)
            {
                if (this.IsUriConstrained( uri, constraint ))
                    return;
            }
            if (uri.Length != 0 || permitted.Count != 0)
                throw new PkixNameConstraintValidatorException( "URI is not from a permitted subtree." );
        }

        private bool IsUriConstrained( string uri, string constraint )
        {
            string hostFromUrl = ExtractHostFromURL( uri );
            if (!Platform.StartsWith( constraint, "." ))
            {
                if (Platform.EqualsIgnoreCase( hostFromUrl, constraint ))
                    return true;
            }
            else if (this.WithinDomain( hostFromUrl, constraint ))
                return true;
            return false;
        }

        private static string ExtractHostFromURL( string url )
        {
            string source = url.Substring( url.IndexOf( ':' ) + 1 );
            int num = Platform.IndexOf( source, "//" );
            if (num != -1)
                source = source.Substring( num + 2 );
            if (source.LastIndexOf( ':' ) != -1)
                source = source.Substring( 0, source.LastIndexOf( ':' ) );
            string str = source.Substring( source.IndexOf( ':' ) + 1 );
            string hostFromUrl = str.Substring( str.IndexOf( '@' ) + 1 );
            if (hostFromUrl.IndexOf( '/' ) != -1)
                hostFromUrl = hostFromUrl.Substring( 0, hostFromUrl.IndexOf( '/' ) );
            return hostFromUrl;
        }

        public void checkPermitted( GeneralName name )
        {
            switch (name.TagNo)
            {
                case 1:
                    this.CheckPermittedEmail( this.permittedSubtreesEmail, this.ExtractNameAsString( name ) );
                    break;
                case 2:
                    this.CheckPermittedDNS( this.permittedSubtreesDNS, DerIA5String.GetInstance( name.Name ).GetString() );
                    break;
                case 4:
                    this.CheckPermittedDN( Asn1Sequence.GetInstance( name.Name.ToAsn1Object() ) );
                    break;
                case 6:
                    this.CheckPermittedURI( this.permittedSubtreesURI, DerIA5String.GetInstance( name.Name ).GetString() );
                    break;
                case 7:
                    this.CheckPermittedIP( this.permittedSubtreesIP, Asn1OctetString.GetInstance( name.Name ).GetOctets() );
                    break;
            }
        }

        public void checkExcluded( GeneralName name )
        {
            switch (name.TagNo)
            {
                case 1:
                    this.CheckExcludedEmail( this.excludedSubtreesEmail, this.ExtractNameAsString( name ) );
                    break;
                case 2:
                    this.checkExcludedDNS( this.excludedSubtreesDNS, DerIA5String.GetInstance( name.Name ).GetString() );
                    break;
                case 4:
                    this.CheckExcludedDN( Asn1Sequence.GetInstance( name.Name.ToAsn1Object() ) );
                    break;
                case 6:
                    this.checkExcludedURI( this.excludedSubtreesURI, DerIA5String.GetInstance( name.Name ).GetString() );
                    break;
                case 7:
                    this.checkExcludedIP( this.excludedSubtreesIP, Asn1OctetString.GetInstance( name.Name ).GetOctets() );
                    break;
            }
        }

        public void IntersectPermittedSubtree( Asn1Sequence permitted )
        {
            IDictionary hashtable = Platform.CreateHashtable();
            foreach (object obj in permitted)
            {
                GeneralSubtree instance = GeneralSubtree.GetInstance( obj );
                int tagNo = instance.Base.TagNo;
                if (hashtable[tagNo] == null)
                    hashtable[tagNo] = new HashSet();
                ((ISet)hashtable[tagNo]).Add( instance );
            }
            IEnumerator enumerator = hashtable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry)enumerator.Current;
                switch ((int)current.Key)
                {
                    case 1:
                        this.permittedSubtreesEmail = this.IntersectEmail( this.permittedSubtreesEmail, (ISet)current.Value );
                        continue;
                    case 2:
                        this.permittedSubtreesDNS = this.intersectDNS( this.permittedSubtreesDNS, (ISet)current.Value );
                        continue;
                    case 4:
                        this.permittedSubtreesDN = this.IntersectDN( this.permittedSubtreesDN, (ISet)current.Value );
                        continue;
                    case 6:
                        this.permittedSubtreesURI = this.intersectURI( this.permittedSubtreesURI, (ISet)current.Value );
                        continue;
                    case 7:
                        this.permittedSubtreesIP = this.IntersectIP( this.permittedSubtreesIP, (ISet)current.Value );
                        continue;
                    default:
                        continue;
                }
            }
        }

        private string ExtractNameAsString( GeneralName name ) => DerIA5String.GetInstance( name.Name ).GetString();

        public void IntersectEmptyPermittedSubtree( int nameType )
        {
            switch (nameType)
            {
                case 1:
                    this.permittedSubtreesEmail = new HashSet();
                    break;
                case 2:
                    this.permittedSubtreesDNS = new HashSet();
                    break;
                case 4:
                    this.permittedSubtreesDN = new HashSet();
                    break;
                case 6:
                    this.permittedSubtreesURI = new HashSet();
                    break;
                case 7:
                    this.permittedSubtreesIP = new HashSet();
                    break;
            }
        }

        public void AddExcludedSubtree( GeneralSubtree subtree )
        {
            GeneralName name = subtree.Base;
            switch (name.TagNo)
            {
                case 1:
                    this.excludedSubtreesEmail = this.UnionEmail( this.excludedSubtreesEmail, this.ExtractNameAsString( name ) );
                    break;
                case 2:
                    this.excludedSubtreesDNS = this.unionDNS( this.excludedSubtreesDNS, this.ExtractNameAsString( name ) );
                    break;
                case 4:
                    this.excludedSubtreesDN = this.UnionDN( this.excludedSubtreesDN, (Asn1Sequence)name.Name.ToAsn1Object() );
                    break;
                case 6:
                    this.excludedSubtreesURI = this.unionURI( this.excludedSubtreesURI, this.ExtractNameAsString( name ) );
                    break;
                case 7:
                    this.excludedSubtreesIP = this.UnionIP( this.excludedSubtreesIP, Asn1OctetString.GetInstance( name.Name ).GetOctets() );
                    break;
            }
        }

        private static byte[] Max( byte[] ip1, byte[] ip2 )
        {
            for (int index = 0; index < ip1.Length; ++index)
            {
                if ((ip1[index] & ushort.MaxValue) > (ip2[index] & ushort.MaxValue))
                    return ip1;
            }
            return ip2;
        }

        private static byte[] Min( byte[] ip1, byte[] ip2 )
        {
            for (int index = 0; index < ip1.Length; ++index)
            {
                if ((ip1[index] & ushort.MaxValue) < (ip2[index] & ushort.MaxValue))
                    return ip1;
            }
            return ip2;
        }

        private static int CompareTo( byte[] ip1, byte[] ip2 )
        {
            if (Arrays.AreEqual( ip1, ip2 ))
                return 0;
            return Arrays.AreEqual( Max( ip1, ip2 ), ip1 ) ? 1 : -1;
        }

        private static byte[] Or( byte[] ip1, byte[] ip2 )
        {
            byte[] numArray = new byte[ip1.Length];
            for (int index = 0; index < ip1.Length; ++index)
                numArray[index] = (byte)(ip1[index] | (uint)ip2[index]);
            return numArray;
        }

        [Obsolete( "Use GetHashCode instead" )]
        public int HashCode() => this.GetHashCode();

        public override int GetHashCode() => this.HashCollection( excludedSubtreesDN ) + this.HashCollection( excludedSubtreesDNS ) + this.HashCollection( excludedSubtreesEmail ) + this.HashCollection( excludedSubtreesIP ) + this.HashCollection( excludedSubtreesURI ) + this.HashCollection( permittedSubtreesDN ) + this.HashCollection( permittedSubtreesDNS ) + this.HashCollection( permittedSubtreesEmail ) + this.HashCollection( permittedSubtreesIP ) + this.HashCollection( permittedSubtreesURI );

        private int HashCollection( ICollection coll )
        {
            if (coll == null)
                return 0;
            int num = 0;
            foreach (object data in (IEnumerable)coll)
            {
                if (data is byte[])
                    num += Arrays.GetHashCode( (byte[])data );
                else
                    num += data.GetHashCode();
            }
            return num;
        }

        public override bool Equals( object o )
        {
            if (!(o is PkixNameConstraintValidator))
                return false;
            PkixNameConstraintValidator constraintValidator = (PkixNameConstraintValidator)o;
            return this.CollectionsAreEqual( constraintValidator.excludedSubtreesDN, excludedSubtreesDN ) && this.CollectionsAreEqual( constraintValidator.excludedSubtreesDNS, excludedSubtreesDNS ) && this.CollectionsAreEqual( constraintValidator.excludedSubtreesEmail, excludedSubtreesEmail ) && this.CollectionsAreEqual( constraintValidator.excludedSubtreesIP, excludedSubtreesIP ) && this.CollectionsAreEqual( constraintValidator.excludedSubtreesURI, excludedSubtreesURI ) && this.CollectionsAreEqual( constraintValidator.permittedSubtreesDN, permittedSubtreesDN ) && this.CollectionsAreEqual( constraintValidator.permittedSubtreesDNS, permittedSubtreesDNS ) && this.CollectionsAreEqual( constraintValidator.permittedSubtreesEmail, permittedSubtreesEmail ) && this.CollectionsAreEqual( constraintValidator.permittedSubtreesIP, permittedSubtreesIP ) && this.CollectionsAreEqual( constraintValidator.permittedSubtreesURI, permittedSubtreesURI );
        }

        private bool CollectionsAreEqual( ICollection coll1, ICollection coll2 )
        {
            if (coll1 == coll2)
                return true;
            if (coll1 == null || coll2 == null || coll1.Count != coll2.Count)
                return false;
            foreach (object o1 in (IEnumerable)coll1)
            {
                IEnumerator enumerator = coll2.GetEnumerator();
                bool flag = false;
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (this.SpecialEquals( o1, current ))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return false;
            }
            return true;
        }

        private bool SpecialEquals( object o1, object o2 )
        {
            if (o1 == o2)
                return true;
            if (o1 == null || o2 == null)
                return false;
            return o1 is byte[] && o2 is byte[]? Arrays.AreEqual( (byte[])o1, (byte[])o2 ) : o1.Equals( o2 );
        }

        private string StringifyIP( byte[] ip )
        {
            string str1 = "";
            for (int index = 0; index < ip.Length / 2; ++index)
                str1 = str1 + (ip[index] & byte.MaxValue) + ".";
            string str2 = str1.Substring( 0, str1.Length - 1 ) + "/";
            for (int index = ip.Length / 2; index < ip.Length; ++index)
                str2 = str2 + (ip[index] & byte.MaxValue) + ".";
            return str2.Substring( 0, str2.Length - 1 );
        }

        private string StringifyIPCollection( ISet ips )
        {
            string str = "" + "[";
            foreach (byte[] ip in (IEnumerable)ips)
                str = str + this.StringifyIP( ip ) + ",";
            if (str.Length > 1)
                str = str.Substring( 0, str.Length - 1 );
            return str + "]";
        }

        public override string ToString()
        {
            string str1 = "" + "permitted:\n";
            if (this.permittedSubtreesDN != null)
                str1 = str1 + "DN:\n" + this.permittedSubtreesDN.ToString() + "\n";
            if (this.permittedSubtreesDNS != null)
                str1 = str1 + "DNS:\n" + this.permittedSubtreesDNS.ToString() + "\n";
            if (this.permittedSubtreesEmail != null)
                str1 = str1 + "Email:\n" + this.permittedSubtreesEmail.ToString() + "\n";
            if (this.permittedSubtreesURI != null)
                str1 = str1 + "URI:\n" + this.permittedSubtreesURI.ToString() + "\n";
            if (this.permittedSubtreesIP != null)
                str1 = str1 + "IP:\n" + this.StringifyIPCollection( this.permittedSubtreesIP ) + "\n";
            string str2 = str1 + "excluded:\n";
            if (!this.excludedSubtreesDN.IsEmpty)
                str2 = str2 + "DN:\n" + this.excludedSubtreesDN.ToString() + "\n";
            if (!this.excludedSubtreesDNS.IsEmpty)
                str2 = str2 + "DNS:\n" + this.excludedSubtreesDNS.ToString() + "\n";
            if (!this.excludedSubtreesEmail.IsEmpty)
                str2 = str2 + "Email:\n" + this.excludedSubtreesEmail.ToString() + "\n";
            if (!this.excludedSubtreesURI.IsEmpty)
                str2 = str2 + "URI:\n" + this.excludedSubtreesURI.ToString() + "\n";
            if (!this.excludedSubtreesIP.IsEmpty)
                str2 = str2 + "IP:\n" + this.StringifyIPCollection( this.excludedSubtreesIP ) + "\n";
            return str2;
        }
    }
}
