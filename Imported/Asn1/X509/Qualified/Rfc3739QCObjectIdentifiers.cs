// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.Rfc3739QCObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public sealed class Rfc3739QCObjectIdentifiers
    {
        public static readonly DerObjectIdentifier IdQcs = new DerObjectIdentifier( "1.3.6.1.5.5.7.11" );
        public static readonly DerObjectIdentifier IdQcsPkixQCSyntaxV1 = new DerObjectIdentifier( IdQcs.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdQcsPkixQCSyntaxV2 = new DerObjectIdentifier( IdQcs.ToString() + ".2" );

        private Rfc3739QCObjectIdentifiers()
        {
        }
    }
}
