// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Misc.IdeaCbcPar
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Misc
{
    public class IdeaCbcPar : Asn1Encodable
    {
        internal Asn1OctetString iv;

        public static IdeaCbcPar GetInstance( object o )
        {
            switch (o)
            {
                case IdeaCbcPar _:
                    return (IdeaCbcPar)o;
                case Asn1Sequence _:
                    return new IdeaCbcPar( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "unknown object in IDEACBCPar factory" );
            }
        }

        public IdeaCbcPar( byte[] iv ) => this.iv = new DerOctetString( iv );

        private IdeaCbcPar( Asn1Sequence seq )
        {
            if (seq.Count != 1)
                return;
            this.iv = (Asn1OctetString)seq[0];
        }

        public byte[] GetIV() => this.iv != null ? this.iv.GetOctets() : null;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.iv != null)
                v.Add( iv );
            return new DerSequence( v );
        }
    }
}
