// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.OriginatorInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class OriginatorInformation
    {
        private readonly OriginatorInfo originatorInfo;

        internal OriginatorInformation( OriginatorInfo originatorInfo ) => this.originatorInfo = originatorInfo;

        public virtual IX509Store GetCertificates()
        {
            Asn1Set certificates = this.originatorInfo.Certificates;
            if (certificates == null)
                return X509StoreFactory.Create( "Certificate/Collection", new X509CollectionStoreParameters( Platform.CreateArrayList() ) );
            IList arrayList = Platform.CreateArrayList( certificates.Count );
            foreach (Asn1Encodable asn1Encodable in certificates)
            {
                Asn1Object asn1Object = asn1Encodable.ToAsn1Object();
                if (asn1Object is Asn1Sequence)
                    arrayList.Add( new X509Certificate( X509CertificateStructure.GetInstance( asn1Object ) ) );
            }
            return X509StoreFactory.Create( "Certificate/Collection", new X509CollectionStoreParameters( arrayList ) );
        }

        public virtual IX509Store GetCrls()
        {
            Asn1Set certificates = this.originatorInfo.Certificates;
            if (certificates == null)
                return X509StoreFactory.Create( "CRL/Collection", new X509CollectionStoreParameters( Platform.CreateArrayList() ) );
            IList arrayList = Platform.CreateArrayList( certificates.Count );
            foreach (Asn1Encodable asn1Encodable in certificates)
            {
                Asn1Object asn1Object = asn1Encodable.ToAsn1Object();
                if (asn1Object is Asn1Sequence)
                    arrayList.Add( new X509Crl( CertificateList.GetInstance( asn1Object ) ) );
            }
            return X509StoreFactory.Create( "CRL/Collection", new X509CollectionStoreParameters( arrayList ) );
        }

        public virtual OriginatorInfo ToAsn1Structure() => this.originatorInfo;
    }
}
