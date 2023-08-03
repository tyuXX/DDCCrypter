// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509CrlParser
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
    public class X509CrlParser
    {
        private static readonly PemParser PemCrlParser = new( "CRL" );
        private readonly bool lazyAsn1;
        private Asn1Set sCrlData;
        private int sCrlDataObjectCount;
        private Stream currentCrlStream;

        public X509CrlParser()
          : this( false )
        {
        }

        public X509CrlParser( bool lazyAsn1 ) => this.lazyAsn1 = lazyAsn1;

        private X509Crl ReadPemCrl( Stream inStream )
        {
            Asn1Sequence asn1Sequence = PemCrlParser.ReadPemObject( inStream );
            return asn1Sequence != null ? this.CreateX509Crl( CertificateList.GetInstance( asn1Sequence ) ) : null;
        }

        private X509Crl ReadDerCrl( Asn1InputStream dIn )
        {
            Asn1Sequence asn1Sequence = (Asn1Sequence)dIn.ReadObject();
            if (asn1Sequence.Count <= 1 || !(asn1Sequence[0] is DerObjectIdentifier) || !asn1Sequence[0].Equals( PkcsObjectIdentifiers.SignedData ))
                return this.CreateX509Crl( CertificateList.GetInstance( asn1Sequence ) );
            this.sCrlData = SignedData.GetInstance( Asn1Sequence.GetInstance( (Asn1TaggedObject)asn1Sequence[1], true ) ).Crls;
            return this.GetCrl();
        }

        private X509Crl GetCrl() => this.sCrlData == null || this.sCrlDataObjectCount >= this.sCrlData.Count ? null : this.CreateX509Crl( CertificateList.GetInstance( this.sCrlData[this.sCrlDataObjectCount++] ) );

        protected virtual X509Crl CreateX509Crl( CertificateList c ) => new( c );

        public X509Crl ReadCrl( byte[] input ) => this.ReadCrl( new MemoryStream( input, false ) );

        public ICollection ReadCrls( byte[] input ) => this.ReadCrls( new MemoryStream( input, false ) );

        public X509Crl ReadCrl( Stream inStream )
        {
            if (inStream == null)
                throw new ArgumentNullException( nameof( inStream ) );
            if (!inStream.CanRead)
                throw new ArgumentException( "inStream must be read-able", nameof( inStream ) );
            if (this.currentCrlStream == null)
            {
                this.currentCrlStream = inStream;
                this.sCrlData = null;
                this.sCrlDataObjectCount = 0;
            }
            else if (this.currentCrlStream != inStream)
            {
                this.currentCrlStream = inStream;
                this.sCrlData = null;
                this.sCrlDataObjectCount = 0;
            }
            try
            {
                if (this.sCrlData != null)
                {
                    if (this.sCrlDataObjectCount != this.sCrlData.Count)
                        return this.GetCrl();
                    this.sCrlData = null;
                    this.sCrlDataObjectCount = 0;
                    return null;
                }
                PushbackStream pushbackStream = new( inStream );
                int b = pushbackStream.ReadByte();
                if (b < 0)
                    return null;
                pushbackStream.Unread( b );
                return b != 48 ? this.ReadPemCrl( pushbackStream ) : this.ReadDerCrl( this.lazyAsn1 ? new LazyAsn1InputStream( pushbackStream ) : new Asn1InputStream( pushbackStream ) );
            }
            catch (CrlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrlException( ex.ToString() );
            }
        }

        public ICollection ReadCrls( Stream inStream )
        {
            IList arrayList = Platform.CreateArrayList();
            X509Crl x509Crl;
            while ((x509Crl = this.ReadCrl( inStream )) != null)
                arrayList.Add( x509Crl );
            return arrayList;
        }
    }
}
