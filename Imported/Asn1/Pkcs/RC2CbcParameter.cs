// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.RC2CbcParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class RC2CbcParameter : Asn1Encodable
    {
        internal DerInteger version;
        internal Asn1OctetString iv;

        public static RC2CbcParameter GetInstance( object obj ) => obj is Asn1Sequence ? new RC2CbcParameter( (Asn1Sequence)obj ) : throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );

        public RC2CbcParameter( byte[] iv ) => this.iv = new DerOctetString( iv );

        public RC2CbcParameter( int parameterVersion, byte[] iv )
        {
            this.version = new DerInteger( parameterVersion );
            this.iv = new DerOctetString( iv );
        }

        private RC2CbcParameter( Asn1Sequence seq )
        {
            if (seq.Count == 1)
            {
                this.iv = (Asn1OctetString)seq[0];
            }
            else
            {
                this.version = (DerInteger)seq[0];
                this.iv = (Asn1OctetString)seq[1];
            }
        }

        public BigInteger RC2ParameterVersion => this.version != null ? this.version.Value : null;

        public byte[] GetIV() => Arrays.Clone( this.iv.GetOctets() );

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.version != null)
                v.Add( version );
            v.Add( iv );
            return new DerSequence( v );
        }
    }
}
