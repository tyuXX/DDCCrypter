// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.SignedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class SignedData : Asn1Encodable
    {
        private static readonly DerInteger Version1 = new( 1 );
        private static readonly DerInteger Version3 = new( 3 );
        private static readonly DerInteger Version4 = new( 4 );
        private static readonly DerInteger Version5 = new( 5 );
        private readonly DerInteger version;
        private readonly Asn1Set digestAlgorithms;
        private readonly ContentInfo contentInfo;
        private readonly Asn1Set certificates;
        private readonly Asn1Set crls;
        private readonly Asn1Set signerInfos;
        private readonly bool certsBer;
        private readonly bool crlsBer;

        public static SignedData GetInstance( object obj )
        {
            switch (obj)
            {
                case SignedData _:
                    return (SignedData)obj;
                case Asn1Sequence _:
                    return new SignedData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public SignedData(
          Asn1Set digestAlgorithms,
          ContentInfo contentInfo,
          Asn1Set certificates,
          Asn1Set crls,
          Asn1Set signerInfos )
        {
            this.version = this.CalculateVersion( contentInfo.ContentType, certificates, crls, signerInfos );
            this.digestAlgorithms = digestAlgorithms;
            this.contentInfo = contentInfo;
            this.certificates = certificates;
            this.crls = crls;
            this.signerInfos = signerInfos;
            this.crlsBer = crls is BerSet;
            this.certsBer = certificates is BerSet;
        }

        private DerInteger CalculateVersion(
          DerObjectIdentifier contentOid,
          Asn1Set certs,
          Asn1Set crls,
          Asn1Set signerInfs )
        {
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            if (certs != null)
            {
                foreach (object cert in certs)
                {
                    if (cert is Asn1TaggedObject)
                    {
                        Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)cert;
                        if (asn1TaggedObject.TagNo == 1)
                            flag3 = true;
                        else if (asn1TaggedObject.TagNo == 2)
                            flag4 = true;
                        else if (asn1TaggedObject.TagNo == 3)
                        {
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            if (flag1)
                return Version5;
            if (crls != null)
            {
                foreach (object crl in crls)
                {
                    if (crl is Asn1TaggedObject)
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (flag2)
                return Version5;
            if (flag4)
                return Version4;
            return flag3 || !CmsObjectIdentifiers.Data.Equals( contentOid ) || this.CheckForVersion3( signerInfs ) ? Version3 : Version1;
        }

        private bool CheckForVersion3( Asn1Set signerInfs )
        {
            foreach (object signerInf in signerInfs)
            {
                if (SignerInfo.GetInstance( signerInf ).Version.Value.IntValue == 3)
                    return true;
            }
            return false;
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
                if (current is Asn1TaggedObject)
                {
                    Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)current;
                    switch (asn1TaggedObject.TagNo)
                    {
                        case 0:
                            this.certsBer = asn1TaggedObject is BerTaggedObject;
                            this.certificates = Asn1Set.GetInstance( asn1TaggedObject, false );
                            continue;
                        case 1:
                            this.crlsBer = asn1TaggedObject is BerTaggedObject;
                            this.crls = Asn1Set.GetInstance( asn1TaggedObject, false );
                            continue;
                        default:
                            throw new ArgumentException( "unknown tag value " + asn1TaggedObject.TagNo );
                    }
                }
                else
                    this.signerInfos = (Asn1Set)current;
            }
        }

        public DerInteger Version => this.version;

        public Asn1Set DigestAlgorithms => this.digestAlgorithms;

        public ContentInfo EncapContentInfo => this.contentInfo;

        public Asn1Set Certificates => this.certificates;

        public Asn1Set CRLs => this.crls;

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
            {
                if (this.certsBer)
                    v.Add( new BerTaggedObject( false, 0, certificates ) );
                else
                    v.Add( new DerTaggedObject( false, 0, certificates ) );
            }
            if (this.crls != null)
            {
                if (this.crlsBer)
                    v.Add( new BerTaggedObject( false, 1, crls ) );
                else
                    v.Add( new DerTaggedObject( false, 1, crls ) );
            }
            v.Add( signerInfos );
            return new BerSequence( v );
        }
    }
}
