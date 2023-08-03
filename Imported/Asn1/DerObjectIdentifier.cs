// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerObjectIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;
using System.Text;

namespace Org.BouncyCastle.Asn1
{
    public class DerObjectIdentifier : Asn1Object
    {
        private const long LONG_LIMIT = 72057594037927808;
        private readonly string identifier;
        private byte[] body = null;
        private static readonly DerObjectIdentifier[] cache = new DerObjectIdentifier[1024];

        public static DerObjectIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerObjectIdentifier _:
                    return (DerObjectIdentifier)obj;
                case byte[] _:
                    return FromOctetString( (byte[])obj );
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static DerObjectIdentifier GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( obj.GetObject() );

        public DerObjectIdentifier( string identifier )
        {
            if (identifier == null)
                throw new ArgumentNullException( nameof( identifier ) );
            this.identifier = IsValidIdentifier( identifier ) ? identifier : throw new FormatException( "string " + identifier + " not an OID" );
        }

        internal DerObjectIdentifier( DerObjectIdentifier oid, string branchID )
        {
            if (!IsValidBranchID( branchID, 0 ))
                throw new ArgumentException( "string " + branchID + " not a valid OID branch", nameof( branchID ) );
            this.identifier = oid.Id + "." + branchID;
        }

        public string Id => this.identifier;

        public virtual DerObjectIdentifier Branch( string branchID ) => new DerObjectIdentifier( this, branchID );

        public virtual bool On( DerObjectIdentifier stem )
        {
            string id1 = this.Id;
            string id2 = stem.Id;
            return id1.Length > id2.Length && id1[id2.Length] == '.' && Platform.StartsWith( id1, id2 );
        }

        internal DerObjectIdentifier( byte[] bytes )
        {
            this.identifier = MakeOidStringFromBytes( bytes );
            this.body = Arrays.Clone( bytes );
        }

        private void WriteField( Stream outputStream, long fieldValue )
        {
            byte[] buffer = new byte[9];
            int offset = 8;
            buffer[offset] = (byte)((ulong)fieldValue & (ulong)sbyte.MaxValue);
            while (fieldValue >= 128L)
            {
                fieldValue >>= 7;
                buffer[--offset] = (byte)((ulong)(fieldValue & sbyte.MaxValue) | 128UL);
            }
            outputStream.Write( buffer, offset, 9 - offset );
        }

        private void WriteField( Stream outputStream, BigInteger fieldValue )
        {
            int length = (fieldValue.BitLength + 6) / 7;
            if (length == 0)
            {
                outputStream.WriteByte( 0 );
            }
            else
            {
                BigInteger bigInteger = fieldValue;
                byte[] buffer = new byte[length];
                for (int index = length - 1; index >= 0; --index)
                {
                    buffer[index] = (byte)((bigInteger.IntValue & sbyte.MaxValue) | 128);
                    bigInteger = bigInteger.ShiftRight( 7 );
                }
                byte[] numArray;
                IntPtr index1;
                (numArray = buffer)[(int)(index1 = (IntPtr)(length - 1))] = (byte)(numArray[(int)index1] & (uint)sbyte.MaxValue);
                outputStream.Write( buffer, 0, buffer.Length );
            }
        }

        private void DoOutput( MemoryStream bOut )
        {
            OidTokenizer oidTokenizer = new OidTokenizer( this.identifier );
            int num = int.Parse( oidTokenizer.NextToken() ) * 40;
            string s1 = oidTokenizer.NextToken();
            if (s1.Length <= 18)
                this.WriteField( bOut, num + long.Parse( s1 ) );
            else
                this.WriteField( bOut, new BigInteger( s1 ).Add( BigInteger.ValueOf( num ) ) );
            while (oidTokenizer.HasMoreTokens)
            {
                string s2 = oidTokenizer.NextToken();
                if (s2.Length <= 18)
                    this.WriteField( bOut, long.Parse( s2 ) );
                else
                    this.WriteField( bOut, new BigInteger( s2 ) );
            }
        }

        internal byte[] GetBody()
        {
            lock (this)
            {
                if (this.body == null)
                {
                    MemoryStream bOut = new MemoryStream();
                    this.DoOutput( bOut );
                    this.body = bOut.ToArray();
                }
            }
            return this.body;
        }

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 6, this.GetBody() );

        protected override int Asn1GetHashCode() => this.identifier.GetHashCode();

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerObjectIdentifier objectIdentifier && this.identifier.Equals( objectIdentifier.identifier );

        public override string ToString() => this.identifier;

        private static bool IsValidBranchID( string branchID, int start )
        {
            bool flag = false;
            int length = branchID.Length;
            while (--length >= start)
            {
                char ch = branchID[length];
                if ('0' <= ch && ch <= '9')
                {
                    flag = true;
                }
                else
                {
                    if (ch != '.' || !flag)
                        return false;
                    flag = false;
                }
            }
            return flag;
        }

        private static bool IsValidIdentifier( string identifier )
        {
            if (identifier.Length < 3 || identifier[1] != '.')
                return false;
            char ch = identifier[0];
            return ch >= '0' && ch <= '2' && IsValidBranchID( identifier, 2 );
        }

        private static string MakeOidStringFromBytes( byte[] bytes )
        {
            StringBuilder stringBuilder = new StringBuilder();
            long num1 = 0;
            BigInteger bigInteger1 = null;
            bool flag = true;
            for (int index = 0; index != bytes.Length; ++index)
            {
                int num2 = bytes[index];
                if (num1 <= 72057594037927808L)
                {
                    long num3 = num1 + (num2 & sbyte.MaxValue);
                    if ((num2 & 128) == 0)
                    {
                        if (flag)
                        {
                            if (num3 < 40L)
                                stringBuilder.Append( '0' );
                            else if (num3 < 80L)
                            {
                                stringBuilder.Append( '1' );
                                num3 -= 40L;
                            }
                            else
                            {
                                stringBuilder.Append( '2' );
                                num3 -= 80L;
                            }
                            flag = false;
                        }
                        stringBuilder.Append( '.' );
                        stringBuilder.Append( num3 );
                        num1 = 0L;
                    }
                    else
                        num1 = num3 << 7;
                }
                else
                {
                    if (bigInteger1 == null)
                        bigInteger1 = BigInteger.ValueOf( num1 );
                    BigInteger bigInteger2 = bigInteger1.Or( BigInteger.ValueOf( num2 & sbyte.MaxValue ) );
                    if ((num2 & 128) == 0)
                    {
                        if (flag)
                        {
                            stringBuilder.Append( '2' );
                            bigInteger2 = bigInteger2.Subtract( BigInteger.ValueOf( 80L ) );
                            flag = false;
                        }
                        stringBuilder.Append( '.' );
                        stringBuilder.Append( bigInteger2 );
                        bigInteger1 = null;
                        num1 = 0L;
                    }
                    else
                        bigInteger1 = bigInteger2.ShiftLeft( 7 );
                }
            }
            return stringBuilder.ToString();
        }

        internal static DerObjectIdentifier FromOctetString( byte[] enc )
        {
            int index = Arrays.GetHashCode( enc ) & 1023;
            lock (cache)
            {
                DerObjectIdentifier objectIdentifier = cache[index];
                return objectIdentifier != null && Arrays.AreEqual( enc, objectIdentifier.GetBody() ) ? objectIdentifier : (cache[index] = new DerObjectIdentifier( enc ));
            }
        }
    }
}
