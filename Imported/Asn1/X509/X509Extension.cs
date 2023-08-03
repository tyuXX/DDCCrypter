// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509Extension
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class X509Extension
    {
        internal bool critical;
        internal Asn1OctetString value;

        public X509Extension( DerBoolean critical, Asn1OctetString value )
        {
            this.critical = critical != null ? critical.IsTrue : throw new ArgumentNullException( nameof( critical ) );
            this.value = value;
        }

        public X509Extension( bool critical, Asn1OctetString value )
        {
            this.critical = critical;
            this.value = value;
        }

        public bool IsCritical => this.critical;

        public Asn1OctetString Value => this.value;

        public Asn1Encodable GetParsedValue() => ConvertValueToObject( this );

        public override int GetHashCode()
        {
            int hashCode = this.Value.GetHashCode();
            return !this.IsCritical ? ~hashCode : hashCode;
        }

        public override bool Equals( object obj ) => obj is X509Extension x509Extension && this.Value.Equals( x509Extension.Value ) && this.IsCritical == x509Extension.IsCritical;

        public static Asn1Object ConvertValueToObject( X509Extension ext )
        {
            try
            {
                return Asn1Object.FromByteArray( ext.Value.GetOctets() );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "can't convert extension", ex );
            }
        }
    }
}
