// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9ECPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
    public class X9ECPoint : Asn1Encodable
    {
        private readonly Asn1OctetString encoding;
        private ECCurve c;
        private ECPoint p;

        public X9ECPoint( ECPoint p )
          : this( p, false )
        {
        }

        public X9ECPoint( ECPoint p, bool compressed )
        {
            this.p = p.Normalize();
            this.encoding = new DerOctetString( p.GetEncoded( compressed ) );
        }

        public X9ECPoint( ECCurve c, byte[] encoding )
        {
            this.c = c;
            this.encoding = new DerOctetString( Arrays.Clone( encoding ) );
        }

        public X9ECPoint( ECCurve c, Asn1OctetString s )
          : this( c, s.GetOctets() )
        {
        }

        public byte[] GetPointEncoding() => Arrays.Clone( this.encoding.GetOctets() );

        public ECPoint Point
        {
            get
            {
                if (this.p == null)
                    this.p = this.c.DecodePoint( this.encoding.GetOctets() ).Normalize();
                return this.p;
            }
        }

        public bool IsPointCompressed
        {
            get
            {
                byte[] octets = this.encoding.GetOctets();
                if (octets == null || octets.Length <= 0)
                    return false;
                return octets[0] == 2 || octets[0] == 3;
            }
        }

        public override Asn1Object ToAsn1Object() => encoding;
    }
}
