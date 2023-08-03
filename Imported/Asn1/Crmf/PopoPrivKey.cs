// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.PopoPrivKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cms;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class PopoPrivKey : Asn1Encodable, IAsn1Choice
    {
        public const int thisMessage = 0;
        public const int subsequentMessage = 1;
        public const int dhMAC = 2;
        public const int agreeMAC = 3;
        public const int encryptedKey = 4;
        private readonly int tagNo;
        private readonly Asn1Encodable obj;

        private PopoPrivKey( Asn1TaggedObject obj )
        {
            this.tagNo = obj.TagNo;
            switch (this.tagNo)
            {
                case 0:
                    this.obj = DerBitString.GetInstance( obj, false );
                    break;
                case 1:
                    this.obj = SubsequentMessage.ValueOf( DerInteger.GetInstance( obj, false ).Value.IntValue );
                    break;
                case 2:
                    this.obj = DerBitString.GetInstance( obj, false );
                    break;
                case 3:
                    this.obj = PKMacValue.GetInstance( obj, false );
                    break;
                case 4:
                    this.obj = EnvelopedData.GetInstance( obj, false );
                    break;
                default:
                    throw new ArgumentException( "unknown tag in PopoPrivKey", nameof( obj ) );
            }
        }

        public static PopoPrivKey GetInstance( Asn1TaggedObject tagged, bool isExplicit ) => new( Asn1TaggedObject.GetInstance( tagged.GetObject() ) );

        public PopoPrivKey( SubsequentMessage msg )
        {
            this.tagNo = 1;
            this.obj = msg;
        }

        public virtual int Type => this.tagNo;

        public virtual Asn1Encodable Value => this.obj;

        public override Asn1Object ToAsn1Object() => new DerTaggedObject( false, this.tagNo, this.obj );
    }
}
