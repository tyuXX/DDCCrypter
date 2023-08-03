// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.OtherRevocationInfoFormat
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class OtherRevocationInfoFormat : Asn1Encodable
    {
        private readonly DerObjectIdentifier otherRevInfoFormat;
        private readonly Asn1Encodable otherRevInfo;

        public OtherRevocationInfoFormat(
          DerObjectIdentifier otherRevInfoFormat,
          Asn1Encodable otherRevInfo )
        {
            this.otherRevInfoFormat = otherRevInfoFormat;
            this.otherRevInfo = otherRevInfo;
        }

        private OtherRevocationInfoFormat( Asn1Sequence seq )
        {
            this.otherRevInfoFormat = DerObjectIdentifier.GetInstance( seq[0] );
            this.otherRevInfo = seq[1];
        }

        public static OtherRevocationInfoFormat GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static OtherRevocationInfoFormat GetInstance( object obj )
        {
            if (obj is OtherRevocationInfoFormat)
                return (OtherRevocationInfoFormat)obj;
            return obj != null ? new OtherRevocationInfoFormat( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public virtual DerObjectIdentifier InfoFormat => this.otherRevInfoFormat;

        public virtual Asn1Encodable Info => this.otherRevInfo;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       otherRevInfoFormat,
      this.otherRevInfo
        } );
    }
}
