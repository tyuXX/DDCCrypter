// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Icao.IcaoObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Icao
{
    public abstract class IcaoObjectIdentifiers
    {
        public static readonly DerObjectIdentifier IdIcao = new( "2.23.136" );
        public static readonly DerObjectIdentifier IdIcaoMrtd = IdIcao.Branch( "1" );
        public static readonly DerObjectIdentifier IdIcaoMrtdSecurity = IdIcaoMrtd.Branch( "1" );
        public static readonly DerObjectIdentifier IdIcaoLdsSecurityObject = IdIcaoMrtdSecurity.Branch( "1" );
        public static readonly DerObjectIdentifier IdIcaoCscaMasterList = IdIcaoMrtdSecurity.Branch( "2" );
        public static readonly DerObjectIdentifier IdIcaoCscaMasterListSigningKey = IdIcaoMrtdSecurity.Branch( "3" );
        public static readonly DerObjectIdentifier IdIcaoDocumentTypeList = IdIcaoMrtdSecurity.Branch( "4" );
        public static readonly DerObjectIdentifier IdIcaoAAProtocolObject = IdIcaoMrtdSecurity.Branch( "5" );
        public static readonly DerObjectIdentifier IdIcaoExtensions = IdIcaoMrtdSecurity.Branch( "6" );
        public static readonly DerObjectIdentifier IdIcaoExtensionsNamechangekeyrollover = IdIcaoExtensions.Branch( "1" );
    }
}
