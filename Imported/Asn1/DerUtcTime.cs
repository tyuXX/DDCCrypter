// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerUtcTime
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Globalization;

namespace Org.BouncyCastle.Asn1
{
    public class DerUtcTime : Asn1Object
    {
        private readonly string time;

        public static DerUtcTime GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerUtcTime _:
                    return (DerUtcTime)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerUtcTime GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerUtcTime ? GetInstance( asn1Object ) : new DerUtcTime( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerUtcTime( string time )
        {
            this.time = time != null ? time : throw new ArgumentNullException( nameof( time ) );
            try
            {
                this.ToDateTime();
            }
            catch (FormatException ex)
            {
                throw new ArgumentException( "invalid date string: " + ex.Message );
            }
        }

        public DerUtcTime( DateTime time ) => this.time = time.ToString( "yyMMddHHmmss", CultureInfo.InvariantCulture ) + "Z";

        internal DerUtcTime( byte[] bytes ) => this.time = Strings.FromAsciiByteArray( bytes );

        public DateTime ToDateTime() => this.ParseDateString( this.TimeString, "yyMMddHHmmss'GMT'zzz" );

        public DateTime ToAdjustedDateTime() => this.ParseDateString( this.AdjustedTimeString, "yyyyMMddHHmmss'GMT'zzz" );

        private DateTime ParseDateString( string dateStr, string formatStr ) => DateTime.ParseExact( dateStr, formatStr, DateTimeFormatInfo.InvariantInfo ).ToUniversalTime();

        public string TimeString
        {
            get
            {
                if (this.time.IndexOf( '-' ) < 0 && this.time.IndexOf( '+' ) < 0)
                    return this.time.Length == 11 ? this.time.Substring( 0, 10 ) + "00GMT+00:00" : this.time.Substring( 0, 12 ) + "GMT+00:00";
                int num = this.time.IndexOf( '-' );
                if (num < 0)
                    num = this.time.IndexOf( '+' );
                string time = this.time;
                if (num == this.time.Length - 3)
                    time += "00";
                return num == 10 ? time.Substring( 0, 10 ) + "00GMT" + time.Substring( 10, 3 ) + ":" + time.Substring( 13, 2 ) : time.Substring( 0, 12 ) + "GMT" + time.Substring( 12, 3 ) + ":" + time.Substring( 15, 2 );
            }
        }

        [Obsolete( "Use 'AdjustedTimeString' property instead" )]
        public string AdjustedTime => this.AdjustedTimeString;

        public string AdjustedTimeString
        {
            get
            {
                string timeString = this.TimeString;
                return (timeString[0] < '5' ? "20" : "19") + timeString;
            }
        }

        private byte[] GetOctets() => Strings.ToAsciiByteArray( this.time );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 23, this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerUtcTime derUtcTime && this.time.Equals( derUtcTime.time );

        protected override int Asn1GetHashCode() => this.time.GetHashCode();

        public override string ToString() => this.time;
    }
}
