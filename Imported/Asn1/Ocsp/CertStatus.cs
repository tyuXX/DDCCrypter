// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.CertStatus
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class CertStatus : Asn1Encodable, IAsn1Choice
    {
        private readonly int tagNo;
        private readonly Asn1Encodable value;

        public CertStatus()
        {
            this.tagNo = 0;
            this.value = DerNull.Instance;
        }

        public CertStatus( RevokedInfo info )
        {
            this.tagNo = 1;
            this.value = info;
        }

        public CertStatus( int tagNo, Asn1Encodable value )
        {
            this.tagNo = tagNo;
            this.value = value;
        }

        public CertStatus( Asn1TaggedObject choice )
        {
            this.tagNo = choice.TagNo;
            switch (choice.TagNo)
            {
                case 0:
                case 2:
                    this.value = DerNull.Instance;
                    break;
                case 1:
                    this.value = RevokedInfo.GetInstance( choice, false );
                    break;
            }
        }

        public static CertStatus GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CertStatus _:
                    return (CertStatus)obj;
                case Asn1TaggedObject _:
                    return new CertStatus( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public int TagNo => this.tagNo;

        public Asn1Encodable Status => this.value;

        public override Asn1Object ToAsn1Object() => new DerTaggedObject( false, this.tagNo, this.value );
    }
}
