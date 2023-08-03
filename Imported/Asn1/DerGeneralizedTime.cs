// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerGeneralizedTime
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Globalization;

namespace Org.BouncyCastle.Asn1
{
    public class DerGeneralizedTime : Asn1Object
    {
        private readonly string time;

        public static DerGeneralizedTime GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerGeneralizedTime _:
                    return (DerGeneralizedTime)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static DerGeneralizedTime GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerGeneralizedTime ? GetInstance( asn1Object ) : new DerGeneralizedTime( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerGeneralizedTime( string time )
        {
            this.time = time;
            try
            {
                this.ToDateTime();
            }
            catch (FormatException ex)
            {
                throw new ArgumentException( "invalid date string: " + ex.Message );
            }
        }

        public DerGeneralizedTime( DateTime time ) => this.time = time.ToString( "yyyyMMddHHmmss\\Z" );

        internal DerGeneralizedTime( byte[] bytes ) => this.time = Strings.FromAsciiByteArray( bytes );

        public string TimeString => this.time;

        public string GetTime()
        {
            if (this.time[this.time.Length - 1] == 'Z')
                return this.time.Substring( 0, this.time.Length - 1 ) + "GMT+00:00";
            int num1 = this.time.Length - 5;
            switch (this.time[num1])
            {
                case '+':
                case '-':
                    return this.time.Substring( 0, num1 ) + "GMT" + this.time.Substring( num1, 3 ) + ":" + this.time.Substring( num1 + 3 );
                default:
                    int num2 = this.time.Length - 3;
                    switch (this.time[num2])
                    {
                        case '+':
                        case '-':
                            return this.time.Substring( 0, num2 ) + "GMT" + this.time.Substring( num2 ) + ":00";
                        default:
                            return this.time + this.CalculateGmtOffset();
                    }
            }
        }

        private string CalculateGmtOffset()
        {
            char ch = '+';
            TimeSpan timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset( this.ToDateTime() );
            if (timeSpan.CompareTo( (object)TimeSpan.Zero ) < 0)
            {
                ch = '-';
                timeSpan = timeSpan.Duration();
            }
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            return "GMT" + ch + Convert( hours ) + ":" + Convert( minutes );
        }

        private static string Convert( int time ) => time < 10 ? "0" + time : time.ToString();

        public DateTime ToDateTime()
        {
            string time = this.time;
            bool makeUniversal = false;
            string format;
            if (Platform.EndsWith( time, "Z" ))
                format = !this.HasFractionalSeconds ? "yyyyMMddHHmmss\\Z" : "yyyyMMddHHmmss." + this.FString( time.Length - time.IndexOf( '.' ) - 2 ) + "\\Z";
            else if (this.time.IndexOf( '-' ) > 0 || this.time.IndexOf( '+' ) > 0)
            {
                time = this.GetTime();
                makeUniversal = true;
                format = !this.HasFractionalSeconds ? "yyyyMMddHHmmss'GMT'zzz" : "yyyyMMddHHmmss." + this.FString( Platform.IndexOf( time, "GMT" ) - 1 - time.IndexOf( '.' ) ) + "'GMT'zzz";
            }
            else
                format = !this.HasFractionalSeconds ? "yyyyMMddHHmmss" : "yyyyMMddHHmmss." + this.FString( time.Length - 1 - time.IndexOf( '.' ) );
            return this.ParseDateString( time, format, makeUniversal );
        }

        private string FString( int count )
        {
            StringBuilder stringBuilder = new();
            for (int index = 0; index < count; ++index)
                stringBuilder.Append( 'f' );
            return stringBuilder.ToString();
        }

        private DateTime ParseDateString( string s, string format, bool makeUniversal )
        {
            DateTimeStyles style = DateTimeStyles.None;
            if (Platform.EndsWith( format, "Z" ))
            {
                try
                {
                    style = (DateTimeStyles)Enums.GetEnumValue( typeof( DateTimeStyles ), "AssumeUniversal" );
                }
                catch (Exception ex)
                {
                }
                style |= DateTimeStyles.AdjustToUniversal;
            }
            DateTime exact = DateTime.ParseExact( s, format, DateTimeFormatInfo.InvariantInfo, style );
            return !makeUniversal ? exact : exact.ToUniversalTime();
        }

        private bool HasFractionalSeconds => this.time.IndexOf( '.' ) == 14;

        private byte[] GetOctets() => Strings.ToAsciiByteArray( this.time );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 24, this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerGeneralizedTime derGeneralizedTime && this.time.Equals( derGeneralizedTime.time );

        protected override int Asn1GetHashCode() => this.time.GetHashCode();
    }
}
