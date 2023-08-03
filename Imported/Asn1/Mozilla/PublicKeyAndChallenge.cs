// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Mozilla.PublicKeyAndChallenge
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Mozilla
{
    public class PublicKeyAndChallenge : Asn1Encodable
    {
        private Asn1Sequence pkacSeq;
        private SubjectPublicKeyInfo spki;
        private DerIA5String challenge;

        public static PublicKeyAndChallenge GetInstance( object obj )
        {
            switch (obj)
            {
                case PublicKeyAndChallenge _:
                    return (PublicKeyAndChallenge)obj;
                case Asn1Sequence _:
                    return new PublicKeyAndChallenge( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in 'PublicKeyAndChallenge' factory : " + Platform.GetTypeName( obj ) + "." );
            }
        }

        public PublicKeyAndChallenge( Asn1Sequence seq )
        {
            this.pkacSeq = seq;
            this.spki = SubjectPublicKeyInfo.GetInstance( seq[0] );
            this.challenge = DerIA5String.GetInstance( seq[1] );
        }

        public override Asn1Object ToAsn1Object() => pkacSeq;

        public SubjectPublicKeyInfo SubjectPublicKeyInfo => this.spki;

        public DerIA5String Challenge => this.challenge;
    }
}
