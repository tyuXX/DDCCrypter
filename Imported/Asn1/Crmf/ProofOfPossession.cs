// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.ProofOfPossession
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class ProofOfPossession : Asn1Encodable, IAsn1Choice
    {
        public const int TYPE_RA_VERIFIED = 0;
        public const int TYPE_SIGNING_KEY = 1;
        public const int TYPE_KEY_ENCIPHERMENT = 2;
        public const int TYPE_KEY_AGREEMENT = 3;
        private readonly int tagNo;
        private readonly Asn1Encodable obj;

        private ProofOfPossession( Asn1TaggedObject tagged )
        {
            this.tagNo = tagged.TagNo;
            switch (this.tagNo)
            {
                case 0:
                    this.obj = DerNull.Instance;
                    break;
                case 1:
                    this.obj = PopoSigningKey.GetInstance( tagged, false );
                    break;
                case 2:
                case 3:
                    this.obj = PopoPrivKey.GetInstance( tagged, false );
                    break;
                default:
                    throw new ArgumentException( "unknown tag: " + tagNo, nameof( tagged ) );
            }
        }

        public static ProofOfPossession GetInstance( object obj )
        {
            switch (obj)
            {
                case ProofOfPossession _:
                    return (ProofOfPossession)obj;
                case Asn1TaggedObject _:
                    return new ProofOfPossession( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public ProofOfPossession()
        {
            this.tagNo = 0;
            this.obj = DerNull.Instance;
        }

        public ProofOfPossession( PopoSigningKey Poposk )
        {
            this.tagNo = 1;
            this.obj = Poposk;
        }

        public ProofOfPossession( int type, PopoPrivKey privkey )
        {
            this.tagNo = type;
            this.obj = privkey;
        }

        public virtual int Type => this.tagNo;

        public virtual Asn1Encodable Object => this.obj;

        public override Asn1Object ToAsn1Object() => new DerTaggedObject( false, this.tagNo, this.obj );
    }
}
