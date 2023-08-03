// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509CertPairParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.X509
{
    public class X509CertPairParser
    {
        private Stream currentStream;

        private X509CertificatePair ReadDerCrossCertificatePair( Stream inStream ) => new( CertificatePair.GetInstance( (Asn1Sequence)new Asn1InputStream( inStream ).ReadObject() ) );

        public X509CertificatePair ReadCertPair( byte[] input ) => this.ReadCertPair( new MemoryStream( input, false ) );

        public ICollection ReadCertPairs( byte[] input ) => this.ReadCertPairs( new MemoryStream( input, false ) );

        public X509CertificatePair ReadCertPair( Stream inStream )
        {
            if (inStream == null)
                throw new ArgumentNullException( nameof( inStream ) );
            if (!inStream.CanRead)
                throw new ArgumentException( "inStream must be read-able", nameof( inStream ) );
            if (this.currentStream == null)
                this.currentStream = inStream;
            else if (this.currentStream != inStream)
                this.currentStream = inStream;
            try
            {
                PushbackStream inStream1 = new( inStream );
                int b = inStream1.ReadByte();
                if (b < 0)
                    return null;
                inStream1.Unread( b );
                return this.ReadDerCrossCertificatePair( inStream1 );
            }
            catch (Exception ex)
            {
                throw new CertificateException( ex.ToString() );
            }
        }

        public ICollection ReadCertPairs( Stream inStream )
        {
            IList arrayList = Platform.CreateArrayList();
            X509CertificatePair x509CertificatePair;
            while ((x509CertificatePair = this.ReadCertPair( inStream )) != null)
                arrayList.Add( x509CertificatePair );
            return arrayList;
        }
    }
}
