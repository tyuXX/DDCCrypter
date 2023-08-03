// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Misc.Cast5CbcParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Misc
{
    public class Cast5CbcParameters : Asn1Encodable
    {
        private readonly DerInteger keyLength;
        private readonly Asn1OctetString iv;

        public static Cast5CbcParameters GetInstance( object o )
        {
            switch (o)
            {
                case Cast5CbcParameters _:
                    return (Cast5CbcParameters)o;
                case Asn1Sequence _:
                    return new Cast5CbcParameters( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in Cast5CbcParameters factory" );
            }
        }

        public Cast5CbcParameters( byte[] iv, int keyLength )
        {
            this.iv = new DerOctetString( iv );
            this.keyLength = new DerInteger( keyLength );
        }

        private Cast5CbcParameters( Asn1Sequence seq )
        {
            this.iv = seq.Count == 2 ? (Asn1OctetString)seq[0] : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.keyLength = (DerInteger)seq[1];
        }

        public byte[] GetIV() => Arrays.Clone( this.iv.GetOctets() );

        public int KeyLength => this.keyLength.Value.IntValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       iv,
       keyLength
        } );
    }
}
