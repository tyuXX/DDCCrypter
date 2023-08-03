// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    internal class CmsUtilities
    {
        internal static int MaximumMemory
        {
            get
            {
                long maxValue = int.MaxValue;
                return maxValue > int.MaxValue ? int.MaxValue : (int)maxValue;
            }
        }

        internal static ContentInfo ReadContentInfo( byte[] input ) => ReadContentInfo( new Asn1InputStream( input ) );

        internal static ContentInfo ReadContentInfo( Stream input ) => ReadContentInfo( new Asn1InputStream( input, MaximumMemory ) );

        private static ContentInfo ReadContentInfo( Asn1InputStream aIn )
        {
            try
            {
                return ContentInfo.GetInstance( aIn.ReadObject() );
            }
            catch (IOException ex)
            {
                throw new CmsException( "IOException reading content.", ex );
            }
            catch (InvalidCastException ex)
            {
                throw new CmsException( "Malformed content.", ex );
            }
            catch (ArgumentException ex)
            {
                throw new CmsException( "Malformed content.", ex );
            }
        }

        public static byte[] StreamToByteArray( Stream inStream ) => Streams.ReadAll( inStream );

        public static byte[] StreamToByteArray( Stream inStream, int limit ) => Streams.ReadAllLimited( inStream, limit );

        public static IList GetCertificatesFromStore( IX509Store certStore )
        {
            try
            {
                IList arrayList = Platform.CreateArrayList();
                if (certStore != null)
                {
                    foreach (X509Certificate match in (IEnumerable)certStore.GetMatches( null ))
                        arrayList.Add( X509CertificateStructure.GetInstance( Asn1Object.FromByteArray( match.GetEncoded() ) ) );
                }
                return arrayList;
            }
            catch (CertificateEncodingException ex)
            {
                throw new CmsException( "error encoding certs", ex );
            }
            catch (Exception ex)
            {
                throw new CmsException( "error processing certs", ex );
            }
        }

        public static IList GetCrlsFromStore( IX509Store crlStore )
        {
            try
            {
                IList arrayList = Platform.CreateArrayList();
                if (crlStore != null)
                {
                    foreach (X509Crl match in (IEnumerable)crlStore.GetMatches( null ))
                        arrayList.Add( CertificateList.GetInstance( Asn1Object.FromByteArray( match.GetEncoded() ) ) );
                }
                return arrayList;
            }
            catch (CrlException ex)
            {
                throw new CmsException( "error encoding crls", ex );
            }
            catch (Exception ex)
            {
                throw new CmsException( "error processing crls", ex );
            }
        }

        public static Asn1Set CreateBerSetFromList( IList berObjects )
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            foreach (Asn1Encodable berObject in (IEnumerable)berObjects)
                v.Add( berObject );
            return new BerSet( v );
        }

        public static Asn1Set CreateDerSetFromList( IList derObjects )
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            foreach (Asn1Encodable derObject in (IEnumerable)derObjects)
                v.Add( derObject );
            return new DerSet( v );
        }

        internal static Stream CreateBerOctetOutputStream(
          Stream s,
          int tagNo,
          bool isExplicit,
          int bufferSize )
        {
            return new BerOctetStringGenerator( s, tagNo, isExplicit ).GetOctetOutputStream( bufferSize );
        }

        internal static TbsCertificateStructure GetTbsCertificateStructure( X509Certificate cert ) => TbsCertificateStructure.GetInstance( Asn1Object.FromByteArray( cert.GetTbsCertificate() ) );

        internal static IssuerAndSerialNumber GetIssuerAndSerialNumber( X509Certificate cert )
        {
            TbsCertificateStructure certificateStructure = GetTbsCertificateStructure( cert );
            return new IssuerAndSerialNumber( certificateStructure.Issuer, certificateStructure.SerialNumber.Value );
        }
    }
}
