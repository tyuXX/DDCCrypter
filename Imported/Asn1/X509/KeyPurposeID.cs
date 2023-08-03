﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.KeyPurposeID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509
{
    public sealed class KeyPurposeID : DerObjectIdentifier
    {
        private const string IdKP = "1.3.6.1.5.5.7.3";
        public static readonly KeyPurposeID AnyExtendedKeyUsage = new KeyPurposeID( X509Extensions.ExtendedKeyUsage.Id + ".0" );
        public static readonly KeyPurposeID IdKPServerAuth = new KeyPurposeID( "1.3.6.1.5.5.7.3.1" );
        public static readonly KeyPurposeID IdKPClientAuth = new KeyPurposeID( "1.3.6.1.5.5.7.3.2" );
        public static readonly KeyPurposeID IdKPCodeSigning = new KeyPurposeID( "1.3.6.1.5.5.7.3.3" );
        public static readonly KeyPurposeID IdKPEmailProtection = new KeyPurposeID( "1.3.6.1.5.5.7.3.4" );
        public static readonly KeyPurposeID IdKPIpsecEndSystem = new KeyPurposeID( "1.3.6.1.5.5.7.3.5" );
        public static readonly KeyPurposeID IdKPIpsecTunnel = new KeyPurposeID( "1.3.6.1.5.5.7.3.6" );
        public static readonly KeyPurposeID IdKPIpsecUser = new KeyPurposeID( "1.3.6.1.5.5.7.3.7" );
        public static readonly KeyPurposeID IdKPTimeStamping = new KeyPurposeID( "1.3.6.1.5.5.7.3.8" );
        public static readonly KeyPurposeID IdKPOcspSigning = new KeyPurposeID( "1.3.6.1.5.5.7.3.9" );
        public static readonly KeyPurposeID IdKPSmartCardLogon = new KeyPurposeID( "1.3.6.1.4.1.311.20.2.2" );

        private KeyPurposeID( string id )
          : base( id )
        {
        }
    }
}
