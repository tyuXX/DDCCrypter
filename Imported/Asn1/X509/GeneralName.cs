// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.GeneralName
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.Globalization;
using System.IO;

namespace Org.BouncyCastle.Asn1.X509
{
    public class GeneralName : Asn1Encodable, IAsn1Choice
    {
        public const int OtherName = 0;
        public const int Rfc822Name = 1;
        public const int DnsName = 2;
        public const int X400Address = 3;
        public const int DirectoryName = 4;
        public const int EdiPartyName = 5;
        public const int UniformResourceIdentifier = 6;
        public const int IPAddress = 7;
        public const int RegisteredID = 8;
        internal readonly Asn1Encodable obj;
        internal readonly int tag;

        public GeneralName( X509Name directoryName )
        {
            this.obj = directoryName;
            this.tag = 4;
        }

        public GeneralName( Asn1Object name, int tag )
        {
            this.obj = name;
            this.tag = tag;
        }

        public GeneralName( int tag, Asn1Encodable name )
        {
            this.obj = name;
            this.tag = tag;
        }

        public GeneralName( int tag, string name )
        {
            this.tag = tag;
            switch (tag)
            {
                case 1:
                case 2:
                case 6:
                    this.obj = new DerIA5String( name );
                    break;
                case 4:
                    this.obj = new X509Name( name );
                    break;
                case 7:
                    this.obj = new DerOctetString( this.toGeneralNameEncoding( name ) ?? throw new ArgumentException( "IP Address is invalid", nameof( name ) ) );
                    break;
                case 8:
                    this.obj = new DerObjectIdentifier( name );
                    break;
                default:
                    throw new ArgumentException( "can't process string for tag: " + tag, nameof( tag ) );
            }
        }

        public static GeneralName GetInstance( object obj )
        {
            if (obj == null || obj is GeneralName)
                return (GeneralName)obj;
            if (obj is Asn1TaggedObject)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
                int tagNo = asn1TaggedObject.TagNo;
                switch (tagNo)
                {
                    case 0:
                        return new GeneralName( tagNo, Asn1Sequence.GetInstance( asn1TaggedObject, false ) );
                    case 1:
                        return new GeneralName( tagNo, DerIA5String.GetInstance( asn1TaggedObject, false ) );
                    case 2:
                        return new GeneralName( tagNo, DerIA5String.GetInstance( asn1TaggedObject, false ) );
                    case 3:
                        throw new ArgumentException( "unknown tag: " + tagNo );
                    case 4:
                        return new GeneralName( tagNo, X509Name.GetInstance( asn1TaggedObject, true ) );
                    case 5:
                        return new GeneralName( tagNo, Asn1Sequence.GetInstance( asn1TaggedObject, false ) );
                    case 6:
                        return new GeneralName( tagNo, DerIA5String.GetInstance( asn1TaggedObject, false ) );
                    case 7:
                        return new GeneralName( tagNo, Asn1OctetString.GetInstance( asn1TaggedObject, false ) );
                    case 8:
                        return new GeneralName( tagNo, DerObjectIdentifier.GetInstance( asn1TaggedObject, false ) );
                }
            }
            if (!(obj is byte[]))
                throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            try
            {
                return GetInstance( Asn1Object.FromByteArray( (byte[])obj ) );
            }
            catch (IOException ex)
            {
                throw new ArgumentException( "unable to parse encoded general name" );
            }
        }

        public static GeneralName GetInstance( Asn1TaggedObject tagObj, bool explicitly ) => GetInstance( Asn1TaggedObject.GetInstance( tagObj, true ) );

        public int TagNo => this.tag;

        public Asn1Encodable Name => this.obj;

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append( this.tag );
            stringBuilder.Append( ": " );
            switch (this.tag)
            {
                case 1:
                case 2:
                case 6:
                    stringBuilder.Append( DerIA5String.GetInstance( obj ).GetString() );
                    break;
                case 4:
                    stringBuilder.Append( X509Name.GetInstance( obj ).ToString() );
                    break;
                default:
                    stringBuilder.Append( this.obj.ToString() );
                    break;
            }
            return stringBuilder.ToString();
        }

        private byte[] toGeneralNameEncoding( string ip )
        {
            if (BouncyCastle.Utilities.Net.IPAddress.IsValidIPv6WithNetmask( ip ) || BouncyCastle.Utilities.Net.IPAddress.IsValidIPv6( ip ))
            {
                int length = ip.IndexOf( '/' );
                if (length < 0)
                {
                    byte[] addr = new byte[16];
                    this.copyInts( this.parseIPv6( ip ), addr, 0 );
                    return addr;
                }
                byte[] addr1 = new byte[32];
                this.copyInts( this.parseIPv6( ip.Substring( 0, length ) ), addr1, 0 );
                string str = ip.Substring( length + 1 );
                this.copyInts( str.IndexOf( ':' ) <= 0 ? this.parseMask( str ) : this.parseIPv6( str ), addr1, 16 );
                return addr1;
            }
            if (!BouncyCastle.Utilities.Net.IPAddress.IsValidIPv4WithNetmask( ip ) && !BouncyCastle.Utilities.Net.IPAddress.IsValidIPv4( ip ))
                return null;
            int length1 = ip.IndexOf( '/' );
            if (length1 < 0)
            {
                byte[] addr = new byte[4];
                this.parseIPv4( ip, addr, 0 );
                return addr;
            }
            byte[] addr2 = new byte[8];
            this.parseIPv4( ip.Substring( 0, length1 ), addr2, 0 );
            string str1 = ip.Substring( length1 + 1 );
            if (str1.IndexOf( '.' ) > 0)
                this.parseIPv4( str1, addr2, 4 );
            else
                this.parseIPv4Mask( str1, addr2, 4 );
            return addr2;
        }

        private void parseIPv4Mask( string mask, byte[] addr, int offset )
        {
            int num = int.Parse( mask );
            for (int index1 = 0; index1 != num; ++index1)
            {
                byte[] numArray;
                IntPtr index2;
                (numArray = addr)[(int)(index2 = (IntPtr)((index1 / 8) + offset))] = (byte)(numArray[(int)index2] | (uint)(byte)(1 << (index1 % 8)));
            }
        }

        private void parseIPv4( string ip, byte[] addr, int offset )
        {
            string str = ip;
            char[] chArray = new char[2] { '.', '/' };
            foreach (string s in str.Split( chArray ))
                addr[offset++] = (byte)int.Parse( s );
        }

        private int[] parseMask( string mask )
        {
            int[] mask1 = new int[8];
            int num = int.Parse( mask );
            for (int index = 0; index != num; ++index)
                mask1[index / 16] |= 1 << (index % 16);
            return mask1;
        }

        private void copyInts( int[] parsedIp, byte[] addr, int offSet )
        {
            for (int index = 0; index != parsedIp.Length; ++index)
            {
                addr[(index * 2) + offSet] = (byte)(parsedIp[index] >> 8);
                addr[(index * 2) + 1 + offSet] = (byte)parsedIp[index];
            }
        }

        private int[] parseIPv6( string ip )
        {
            if (Platform.StartsWith( ip, "::" ))
                ip = ip.Substring( 1 );
            else if (Platform.EndsWith( ip, "::" ))
                ip = ip.Substring( 0, ip.Length - 1 );
            IEnumerator enumerator = ip.Split( ':' ).GetEnumerator();
            int num1 = 0;
            int[] ipv6 = new int[8];
            int sourceIndex = -1;
            while (enumerator.MoveNext())
            {
                string current = (string)enumerator.Current;
                if (current.Length == 0)
                {
                    sourceIndex = num1;
                    ipv6[num1++] = 0;
                }
                else if (current.IndexOf( '.' ) < 0)
                {
                    ipv6[num1++] = int.Parse( current, NumberStyles.AllowHexSpecifier );
                }
                else
                {
                    string[] strArray = current.Split( '.' );
                    int[] numArray1 = ipv6;
                    int index1 = num1;
                    int num2 = index1 + 1;
                    int num3 = (int.Parse( strArray[0] ) << 8) | int.Parse( strArray[1] );
                    numArray1[index1] = num3;
                    int[] numArray2 = ipv6;
                    int index2 = num2;
                    num1 = index2 + 1;
                    int num4 = (int.Parse( strArray[2] ) << 8) | int.Parse( strArray[3] );
                    numArray2[index2] = num4;
                }
            }
            if (num1 != ipv6.Length)
            {
                Array.Copy( ipv6, sourceIndex, ipv6, ipv6.Length - (num1 - sourceIndex), num1 - sourceIndex );
                for (int index = sourceIndex; index != ipv6.Length - (num1 - sourceIndex); ++index)
                    ipv6[index] = 0;
            }
            return ipv6;
        }

        public override Asn1Object ToAsn1Object() => new DerTaggedObject( this.tag == 4, this.tag, this.obj );
    }
}
