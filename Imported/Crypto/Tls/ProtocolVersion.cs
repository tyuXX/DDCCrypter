// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ProtocolVersion
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public sealed class ProtocolVersion
    {
        public static readonly ProtocolVersion SSLv3 = new( 768, "SSL 3.0" );
        public static readonly ProtocolVersion TLSv10 = new( 769, "TLS 1.0" );
        public static readonly ProtocolVersion TLSv11 = new( 770, "TLS 1.1" );
        public static readonly ProtocolVersion TLSv12 = new( 771, "TLS 1.2" );
        public static readonly ProtocolVersion DTLSv10 = new( 65279, "DTLS 1.0" );
        public static readonly ProtocolVersion DTLSv12 = new( 65277, "DTLS 1.2" );
        private readonly int version;
        private readonly string name;

        private ProtocolVersion( int v, string name )
        {
            this.version = v & ushort.MaxValue;
            this.name = name;
        }

        public int FullVersion => this.version;

        public int MajorVersion => this.version >> 8;

        public int MinorVersion => this.version & byte.MaxValue;

        public bool IsDtls => this.MajorVersion == 254;

        public bool IsSsl => this == SSLv3;

        public bool IsTls => this.MajorVersion == 3;

        public ProtocolVersion GetEquivalentTLSVersion()
        {
            if (!this.IsDtls)
                return this;
            return this == DTLSv10 ? TLSv11 : TLSv12;
        }

        public bool IsEqualOrEarlierVersionOf( ProtocolVersion version )
        {
            if (this.MajorVersion != version.MajorVersion)
                return false;
            int num = version.MinorVersion - this.MinorVersion;
            return !this.IsDtls ? num >= 0 : num <= 0;
        }

        public bool IsLaterVersionOf( ProtocolVersion version )
        {
            if (this.MajorVersion != version.MajorVersion)
                return false;
            int num = version.MinorVersion - this.MinorVersion;
            return !this.IsDtls ? num < 0 : num > 0;
        }

        public override bool Equals( object other )
        {
            if (this == other)
                return true;
            return other is ProtocolVersion && this.Equals( (ProtocolVersion)other );
        }

        public bool Equals( ProtocolVersion other ) => other != null && this.version == other.version;

        public override int GetHashCode() => this.version;

        public static ProtocolVersion Get( int major, int minor )
        {
            switch (major)
            {
                case 3:
                    switch (minor)
                    {
                        case 0:
                            return SSLv3;
                        case 1:
                            return TLSv10;
                        case 2:
                            return TLSv11;
                        case 3:
                            return TLSv12;
                        default:
                            return GetUnknownVersion( major, minor, "TLS" );
                    }
                case 254:
                    switch (minor)
                    {
                        case 253:
                            return DTLSv12;
                        case 254:
                            throw new TlsFatalAlert( 47 );
                        case byte.MaxValue:
                            return DTLSv10;
                        default:
                            return GetUnknownVersion( major, minor, "DTLS" );
                    }
                default:
                    throw new TlsFatalAlert( 47 );
            }
        }

        public override string ToString() => this.name;

        private static ProtocolVersion GetUnknownVersion( int major, int minor, string prefix )
        {
            TlsUtilities.CheckUint8( major );
            TlsUtilities.CheckUint8( minor );
            int v = (major << 8) | minor;
            string upperInvariant = Platform.ToUpperInvariant( Convert.ToString( 65536 | v, 16 ).Substring( 1 ) );
            return new ProtocolVersion( v, prefix + " 0x" + upperInvariant );
        }
    }
}
