// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.MetaData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class MetaData : Asn1Encodable
    {
        private DerBoolean hashProtected;
        private DerUtf8String fileName;
        private DerIA5String mediaType;
        private Attributes otherMetaData;

        public MetaData(
          DerBoolean hashProtected,
          DerUtf8String fileName,
          DerIA5String mediaType,
          Attributes otherMetaData )
        {
            this.hashProtected = hashProtected;
            this.fileName = fileName;
            this.mediaType = mediaType;
            this.otherMetaData = otherMetaData;
        }

        private MetaData( Asn1Sequence seq )
        {
            this.hashProtected = DerBoolean.GetInstance( seq[0] );
            int index1 = 1;
            if (index1 < seq.Count && seq[index1] is DerUtf8String)
                this.fileName = DerUtf8String.GetInstance( seq[index1++] );
            if (index1 < seq.Count && seq[index1] is DerIA5String)
                this.mediaType = DerIA5String.GetInstance( seq[index1++] );
            if (index1 >= seq.Count)
                return;
            Asn1Sequence asn1Sequence = seq;
            int index2 = index1;
            int num = index2 + 1;
            this.otherMetaData = Attributes.GetInstance( asn1Sequence[index2] );
        }

        public static MetaData GetInstance( object obj )
        {
            if (obj is MetaData)
                return (MetaData)obj;
            return obj != null ? new MetaData( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         hashProtected
            } );
            v.AddOptional( fileName, mediaType, otherMetaData );
            return new DerSequence( v );
        }

        public virtual bool IsHashProtected => this.hashProtected.IsTrue;

        public virtual DerUtf8String FileName => this.fileName;

        public virtual DerIA5String MediaType => this.mediaType;

        public virtual Attributes OtherMetaData => this.otherMetaData;
    }
}
