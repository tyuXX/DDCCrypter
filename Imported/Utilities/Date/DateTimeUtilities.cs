// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Date.DateTimeUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.Date
{
    public class DateTimeUtilities
    {
        public static readonly DateTime UnixEpoch = new( 1970, 1, 1 );

        private DateTimeUtilities()
        {
        }

        public static long DateTimeToUnixMs( DateTime dateTime )
        {
            if (dateTime.CompareTo( (object)UnixEpoch ) < 0)
                throw new ArgumentException( "DateTime value may not be before the epoch", nameof( dateTime ) );
            return (dateTime.Ticks - UnixEpoch.Ticks) / 10000L;
        }

        public static DateTime UnixMsToDateTime( long unixMs ) => new( (unixMs * 10000L) + UnixEpoch.Ticks );

        public static long CurrentUnixMs() => DateTimeToUnixMs( DateTime.UtcNow );
    }
}
