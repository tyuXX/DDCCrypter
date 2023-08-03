// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Microsoft.MicrosoftObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Microsoft
{
    public abstract class MicrosoftObjectIdentifiers
    {
        public static readonly DerObjectIdentifier Microsoft = new( "1.3.6.1.4.1.311" );
        public static readonly DerObjectIdentifier MicrosoftCertTemplateV1 = Microsoft.Branch( "20.2" );
        public static readonly DerObjectIdentifier MicrosoftCAVersion = Microsoft.Branch( "21.1" );
        public static readonly DerObjectIdentifier MicrosoftPrevCACertHash = Microsoft.Branch( "21.2" );
        public static readonly DerObjectIdentifier MicrosoftCrlNextPublish = Microsoft.Branch( "21.4" );
        public static readonly DerObjectIdentifier MicrosoftCertTemplateV2 = Microsoft.Branch( "21.7" );
        public static readonly DerObjectIdentifier MicrosoftAppPolicies = Microsoft.Branch( "21.10" );
    }
}
