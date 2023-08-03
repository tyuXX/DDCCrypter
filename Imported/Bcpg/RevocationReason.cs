// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.RevocationReason
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Bcpg
{
    public class RevocationReason : SignatureSubpacket
    {
        public RevocationReason( bool isCritical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.RevocationReason, isCritical, isLongLength, data )
        {
        }

        public RevocationReason( bool isCritical, RevocationReasonTag reason, string description )
          : base( SignatureSubpacketTag.RevocationReason, isCritical, false, CreateData( reason, description ) )
        {
        }

        private static byte[] CreateData( RevocationReasonTag reason, string description )
        {
            byte[] utf8ByteArray = Strings.ToUtf8ByteArray( description );
            byte[] destinationArray = new byte[1 + utf8ByteArray.Length];
            destinationArray[0] = (byte)reason;
            Array.Copy( utf8ByteArray, 0, destinationArray, 1, utf8ByteArray.Length );
            return destinationArray;
        }

        public virtual RevocationReasonTag GetRevocationReason() => (RevocationReasonTag)this.GetData()[0];

        public virtual string GetRevocationDescription()
        {
            byte[] data = this.GetData();
            if (data.Length == 1)
                return string.Empty;
            byte[] numArray = new byte[data.Length - 1];
            Array.Copy( data, 1, numArray, 0, numArray.Length );
            return Strings.FromUtf8ByteArray( numArray );
        }
    }
}
