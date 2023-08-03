// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.SignedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.Collections;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class SignedData : Asn1Encodable
    {
        private readonly DerInteger version;
        private readonly Asn1Set digestAlgorithms;
        private readonly ContentInfo contentInfo;
        private readonly Asn1Set certificates;
        private readonly Asn1Set crls;
        private readonly Asn1Set signerInfos;

        public static SignedData GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is SignedData signedData ? signedData : new SignedData( Asn1Sequence.GetInstance( obj ) );
        }

        public SignedData(
          DerInteger _version,
          Asn1Set _digestAlgorithms,
          ContentInfo _contentInfo,
          Asn1Set _certificates,
          Asn1Set _crls,
          Asn1Set _signerInfos )
        {
            this.version = _version;
            this.digestAlgorithms = _digestAlgorithms;
            this.contentInfo = _contentInfo;
            this.certificates = _certificates;
            this.crls = _crls;
            this.signerInfos = _signerInfos;
        }

        private SignedData( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.version = (DerInteger)enumerator.Current;
            enumerator.MoveNext();
            this.digestAlgorithms = (Asn1Set)enumerator.Current;
            enumerator.MoveNext();
            this.contentInfo = ContentInfo.GetInstance( enumerator.Current );
            while (enumerator.MoveNext())
            {
                Asn1Object current = (Asn1Object)enumerator.Current;
                if (current is DerTaggedObject)
                {
                    DerTaggedObject derTaggedObject = (DerTaggedObject)current;
                    switch (derTaggedObject.TagNo)
                    {
                        case 0:
                            this.certificates = Asn1Set.GetInstance( derTaggedObject, false );
                            continue;
                        case 1:
                            this.crls = Asn1Set.GetInstance( derTaggedObject, false );
                            continue;
                        default:
                            throw new ArgumentException( "unknown tag value " + derTaggedObject.TagNo );
                    }
                }
                else
                    this.signerInfos = (Asn1Set)current;
            }
        }

        public DerInteger Version => this.version;

        public Asn1Set DigestAlgorithms => this.digestAlgorithms;

        public ContentInfo ContentInfo => this.contentInfo;

        public Asn1Set Certificates => this.certificates;

        public Asn1Set Crls => this.crls;

        public Asn1Set SignerInfos => this.signerInfos;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         version,
         digestAlgorithms,
         contentInfo
            } );
            if (this.certificates != null)
                v.Add( new DerTaggedObject( false, 0, certificates ) );
            if (this.crls != null)
                v.Add( new DerTaggedObject( false, 1, crls ) );
            v.Add( signerInfos );
            return new BerSequence( v );
        }
    }
}
