// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.SubsequentMessage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class SubsequentMessage : DerInteger
    {
        public static readonly SubsequentMessage encrCert = new( 0 );
        public static readonly SubsequentMessage challengeResp = new( 1 );

        private SubsequentMessage( int value )
          : base( value )
        {
        }

        public static SubsequentMessage ValueOf( int value )
        {
            if (value == 0)
                return encrCert;
            if (value == 1)
                return challengeResp;
            throw new ArgumentException( "unknown value: " + value, nameof( value ) );
        }
    }
}
