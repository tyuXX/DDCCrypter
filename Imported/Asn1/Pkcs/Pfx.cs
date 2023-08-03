// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.Pfx
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class Pfx : Asn1Encodable
    {
        private ContentInfo contentInfo;
        private MacData macData;

        public Pfx( Asn1Sequence seq )
        {
            if (((DerInteger)seq[0]).Value.IntValue != 3)
                throw new ArgumentException( "wrong version for PFX PDU" );
            this.contentInfo = ContentInfo.GetInstance( seq[1] );
            if (seq.Count != 3)
                return;
            this.macData = MacData.GetInstance( seq[2] );
        }

        public Pfx( ContentInfo contentInfo, MacData macData )
        {
            this.contentInfo = contentInfo;
            this.macData = macData;
        }

        public ContentInfo AuthSafe => this.contentInfo;

        public MacData MacData => this.macData;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         new DerInteger(3),
         contentInfo
            } );
            if (this.macData != null)
                v.Add( macData );
            return new BerSequence( v );
        }
    }
}
