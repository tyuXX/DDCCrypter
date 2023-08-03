// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Iana.IanaObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Iana
{
    public abstract class IanaObjectIdentifiers
    {
        public static readonly DerObjectIdentifier IsakmpOakley = new DerObjectIdentifier( "1.3.6.1.5.5.8.1" );
        public static readonly DerObjectIdentifier HmacMD5 = new DerObjectIdentifier( IsakmpOakley.ToString() + ".1" );
        public static readonly DerObjectIdentifier HmacSha1 = new DerObjectIdentifier( IsakmpOakley.ToString() + ".2" );
        public static readonly DerObjectIdentifier HmacTiger = new DerObjectIdentifier( IsakmpOakley.ToString() + ".3" );
        public static readonly DerObjectIdentifier HmacRipeMD160 = new DerObjectIdentifier( IsakmpOakley.ToString() + ".4" );
    }
}
