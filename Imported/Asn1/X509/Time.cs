// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Time
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Globalization;

namespace Org.BouncyCastle.Asn1.X509
{
    public class Time : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1Object time;

        public static Time GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( obj.GetObject() );

        public Time( Asn1Object time )
        {
            switch (time)
            {
                case null:
                    throw new ArgumentNullException( nameof( time ) );
                case DerUtcTime _:
                case DerGeneralizedTime _:
                    this.time = time;
                    break;
                default:
                    throw new ArgumentException( "unknown object passed to Time" );
            }
        }

        public Time( DateTime date )
        {
            string time = date.ToString( "yyyyMMddHHmmss", CultureInfo.InvariantCulture ) + "Z";
            int num = int.Parse( time.Substring( 0, 4 ) );
            if (num < 1950 || num > 2049)
                this.time = new DerGeneralizedTime( time );
            else
                this.time = new DerUtcTime( time.Substring( 2 ) );
        }

        public static Time GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Time _:
                    return (Time)obj;
                case DerUtcTime _:
                    return new Time( (Asn1Object)obj );
                case DerGeneralizedTime _:
                    return new Time( (Asn1Object)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public string GetTime() => this.time is DerUtcTime ? ((DerUtcTime)this.time).AdjustedTimeString : ((DerGeneralizedTime)this.time).GetTime();

        public DateTime ToDateTime()
        {
            try
            {
                return this.time is DerUtcTime ? ((DerUtcTime)this.time).ToAdjustedDateTime() : ((DerGeneralizedTime)this.time).ToDateTime();
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException( "invalid date string: " + ex.Message );
            }
        }

        public override Asn1Object ToAsn1Object() => this.time;

        public override string ToString() => this.GetTime();
    }
}
