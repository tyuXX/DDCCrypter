// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.PolicyQualifierID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public sealed class PolicyQualifierID : DerObjectIdentifier
    {
        private const string IdQt = "1.3.6.1.5.5.7.2";
        public static readonly PolicyQualifierID IdQtCps = new( "1.3.6.1.5.5.7.2.1" );
        public static readonly PolicyQualifierID IdQtUnotice = new( "1.3.6.1.5.5.7.2.2" );

        private PolicyQualifierID( string id )
          : base( id )
        {
        }
    }
}
