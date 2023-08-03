// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509CertificateParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.X509
{
    public class X509CertificateParser
    {
        private static readonly PemParser PemCertParser = new( "CERTIFICATE" );
        private Asn1Set sData;
        private int sDataObjectCount;
        private Stream currentStream;

        private X509Certificate ReadDerCertificate( Asn1InputStream dIn )
        {
            Asn1Sequence asn1Sequence = (Asn1Sequence)dIn.ReadObject();
            if (asn1Sequence.Count <= 1 || !(asn1Sequence[0] is DerObjectIdentifier) || !asn1Sequence[0].Equals( PkcsObjectIdentifiers.SignedData ))
                return this.CreateX509Certificate( X509CertificateStructure.GetInstance( asn1Sequence ) );
            this.sData = SignedData.GetInstance( Asn1Sequence.GetInstance( (Asn1TaggedObject)asn1Sequence[1], true ) ).Certificates;
            return this.GetCertificate();
        }

        private X509Certificate GetCertificate()
        {
            if (this.sData != null)
            {
                while (this.sDataObjectCount < this.sData.Count)
                {
                    object obj = this.sData[this.sDataObjectCount++];
                    if (obj is Asn1Sequence)
                        return this.CreateX509Certificate( X509CertificateStructure.GetInstance( obj ) );
                }
            }
            return null;
        }

        private X509Certificate ReadPemCertificate( Stream inStream )
        {
            Asn1Sequence asn1Sequence = PemCertParser.ReadPemObject( inStream );
            return asn1Sequence != null ? this.CreateX509Certificate( X509CertificateStructure.GetInstance( asn1Sequence ) ) : null;
        }

        protected virtual X509Certificate CreateX509Certificate( X509CertificateStructure c ) => new( c );

        public X509Certificate ReadCertificate( byte[] input ) => this.ReadCertificate( new MemoryStream( input, false ) );

        public ICollection ReadCertificates( byte[] input ) => this.ReadCertificates( new MemoryStream( input, false ) );

        public X509Certificate ReadCertificate( Stream inStream )
        {
            if (inStream == null)
                throw new ArgumentNullException( nameof( inStream ) );
            if (!inStream.CanRead)
                throw new ArgumentException( "inStream must be read-able", nameof( inStream ) );
            if (this.currentStream == null)
            {
                this.currentStream = inStream;
                this.sData = null;
                this.sDataObjectCount = 0;
            }
            else if (this.currentStream != inStream)
            {
                this.currentStream = inStream;
                this.sData = null;
                this.sDataObjectCount = 0;
            }
            try
            {
                if (this.sData != null)
                {
                    if (this.sDataObjectCount != this.sData.Count)
                        return this.GetCertificate();
                    this.sData = null;
                    this.sDataObjectCount = 0;
                    return null;
                }
                PushbackStream pushbackStream = new( inStream );
                int b = pushbackStream.ReadByte();
                if (b < 0)
                    return null;
                pushbackStream.Unread( b );
                return b != 48 ? this.ReadPemCertificate( pushbackStream ) : this.ReadDerCertificate( new Asn1InputStream( pushbackStream ) );
            }
            catch (Exception ex)
            {
                throw new CertificateException( "Failed to read certificate", ex );
            }
        }

        public ICollection ReadCertificates( Stream inStream )
        {
            IList arrayList = Platform.CreateArrayList();
            X509Certificate x509Certificate;
            while ((x509Certificate = this.ReadCertificate( inStream )) != null)
                arrayList.Add( x509Certificate );
            return arrayList;
        }
    }
}
