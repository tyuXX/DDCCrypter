// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.SinglePubInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class SinglePubInfo : Asn1Encodable
    {
        private readonly DerInteger pubMethod;
        private readonly GeneralName pubLocation;

        private SinglePubInfo( Asn1Sequence seq )
        {
            this.pubMethod = DerInteger.GetInstance( seq[0] );
            if (seq.Count != 2)
                return;
            this.pubLocation = GeneralName.GetInstance( seq[1] );
        }

        public static SinglePubInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case SinglePubInfo _:
                    return (SinglePubInfo)obj;
                case Asn1Sequence _:
                    return new SinglePubInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual GeneralName PubLocation => this.pubLocation;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         pubMethod
            } );
            v.AddOptional( pubLocation );
            return new DerSequence( v );
        }
    }
}
