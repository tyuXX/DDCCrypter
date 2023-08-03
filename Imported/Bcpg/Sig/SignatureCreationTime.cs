// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.SignatureCreationTime
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Date;
using System;

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class SignatureCreationTime : SignatureSubpacket
    {
        protected static byte[] TimeToBytes( DateTime time )
        {
            long num = DateTimeUtilities.DateTimeToUnixMs( time ) / 1000L;
            return new byte[4]
            {
        (byte) (num >> 24),
        (byte) (num >> 16),
        (byte) (num >> 8),
        (byte) num
            };
        }

        public SignatureCreationTime( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.CreationTime, critical, isLongLength, data )
        {
        }

        public SignatureCreationTime( bool critical, DateTime date )
          : base( SignatureSubpacketTag.CreationTime, critical, false, TimeToBytes( date ) )
        {
        }

        public DateTime GetTime() => DateTimeUtilities.UnixMsToDateTime( ((uint)((this.data[0] << 24) | (this.data[1] << 16) | (this.data[2] << 8)) | this.data[3]) * 1000L );
    }
}
