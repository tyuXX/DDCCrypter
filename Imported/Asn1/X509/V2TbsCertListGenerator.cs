// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.V2TbsCertListGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1.X509
{
    public class V2TbsCertListGenerator
    {
        private DerInteger version = new DerInteger( 1 );
        private AlgorithmIdentifier signature;
        private X509Name issuer;
        private Time thisUpdate;
        private Time nextUpdate;
        private X509Extensions extensions;
        private IList crlEntries;

        public void SetSignature( AlgorithmIdentifier signature ) => this.signature = signature;

        public void SetIssuer( X509Name issuer ) => this.issuer = issuer;

        public void SetThisUpdate( DerUtcTime thisUpdate ) => this.thisUpdate = new Time( thisUpdate );

        public void SetNextUpdate( DerUtcTime nextUpdate ) => this.nextUpdate = nextUpdate != null ? new Time( nextUpdate ) : null;

        public void SetThisUpdate( Time thisUpdate ) => this.thisUpdate = thisUpdate;

        public void SetNextUpdate( Time nextUpdate ) => this.nextUpdate = nextUpdate;

        public void AddCrlEntry( Asn1Sequence crlEntry )
        {
            if (this.crlEntries == null)
                this.crlEntries = Platform.CreateArrayList();
            this.crlEntries.Add( crlEntry );
        }

        public void AddCrlEntry( DerInteger userCertificate, DerUtcTime revocationDate, int reason ) => this.AddCrlEntry( userCertificate, new Time( revocationDate ), reason );

        public void AddCrlEntry( DerInteger userCertificate, Time revocationDate, int reason ) => this.AddCrlEntry( userCertificate, revocationDate, reason, null );

        public void AddCrlEntry(
          DerInteger userCertificate,
          Time revocationDate,
          int reason,
          DerGeneralizedTime invalidityDate )
        {
            IList arrayList1 = Platform.CreateArrayList();
            IList arrayList2 = Platform.CreateArrayList();
            if (reason != 0)
            {
                CrlReason crlReason = new CrlReason( reason );
                try
                {
                    arrayList1.Add( X509Extensions.ReasonCode );
                    arrayList2.Add( new X509Extension( false, new DerOctetString( crlReason.GetEncoded() ) ) );
                }
                catch (IOException ex)
                {
                    throw new ArgumentException( "error encoding reason: " + ex );
                }
            }
            if (invalidityDate != null)
            {
                try
                {
                    arrayList1.Add( X509Extensions.InvalidityDate );
                    arrayList2.Add( new X509Extension( false, new DerOctetString( invalidityDate.GetEncoded() ) ) );
                }
                catch (IOException ex)
                {
                    throw new ArgumentException( "error encoding invalidityDate: " + ex );
                }
            }
            if (arrayList1.Count != 0)
                this.AddCrlEntry( userCertificate, revocationDate, new X509Extensions( arrayList1, arrayList2 ) );
            else
                this.AddCrlEntry( userCertificate, revocationDate, null );
        }

        public void AddCrlEntry(
          DerInteger userCertificate,
          Time revocationDate,
          X509Extensions extensions )
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         userCertificate,
         revocationDate
            } );
            if (extensions != null)
                v.Add( extensions );
            this.AddCrlEntry( new DerSequence( v ) );
        }

        public void SetExtensions( X509Extensions extensions ) => this.extensions = extensions;

        public TbsCertificateList GenerateTbsCertList()
        {
            if (this.signature == null || this.issuer == null || this.thisUpdate == null)
                throw new InvalidOperationException( "Not all mandatory fields set in V2 TbsCertList generator." );
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[4]
            {
         version,
         signature,
         issuer,
         thisUpdate
            } );
            if (this.nextUpdate != null)
                v.Add( nextUpdate );
            if (this.crlEntries != null)
            {
                Asn1Sequence[] asn1SequenceArray = new Asn1Sequence[this.crlEntries.Count];
                for (int index = 0; index < this.crlEntries.Count; ++index)
                    asn1SequenceArray[index] = (Asn1Sequence)this.crlEntries[index];
                v.Add( new DerSequence( asn1SequenceArray ) );
            }
            if (this.extensions != null)
                v.Add( new DerTaggedObject( 0, extensions ) );
            return new TbsCertificateList( new DerSequence( v ) );
        }
    }
}
