// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPath
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCertPath
    {
        internal static readonly IList certPathEncodings;
        private readonly IList certificates;

        static PkixCertPath()
        {
            IList arrayList = Platform.CreateArrayList();
            arrayList.Add( "PkiPath" );
            arrayList.Add( "PEM" );
            arrayList.Add( "PKCS7" );
            certPathEncodings = CollectionUtilities.ReadOnly( arrayList );
        }

        private static IList SortCerts( IList certs )
        {
            if (certs.Count < 2)
                return certs;
            X509Name issuerDn1 = ((X509Certificate)certs[0]).IssuerDN;
            bool flag1 = true;
            for (int index = 1; index != certs.Count; ++index)
            {
                X509Certificate cert = (X509Certificate)certs[index];
                if (issuerDn1.Equivalent( cert.SubjectDN, true ))
                {
                    issuerDn1 = ((X509Certificate)certs[index]).IssuerDN;
                }
                else
                {
                    flag1 = false;
                    break;
                }
            }
            if (flag1)
                return certs;
            IList arrayList1 = Platform.CreateArrayList( certs.Count );
            IList arrayList2 = Platform.CreateArrayList( certs );
            for (int index = 0; index < certs.Count; ++index)
            {
                X509Certificate cert1 = (X509Certificate)certs[index];
                bool flag2 = false;
                X509Name subjectDn = cert1.SubjectDN;
                foreach (X509Certificate cert2 in (IEnumerable)certs)
                {
                    if (cert2.IssuerDN.Equivalent( subjectDn, true ))
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    arrayList1.Add( cert1 );
                    certs.RemoveAt( index );
                }
            }
            if (arrayList1.Count > 1)
                return arrayList2;
            for (int index1 = 0; index1 != arrayList1.Count; ++index1)
            {
                X509Name issuerDn2 = ((X509Certificate)arrayList1[index1]).IssuerDN;
                for (int index2 = 0; index2 < certs.Count; ++index2)
                {
                    X509Certificate cert = (X509Certificate)certs[index2];
                    if (issuerDn2.Equivalent( cert.SubjectDN, true ))
                    {
                        arrayList1.Add( cert );
                        certs.RemoveAt( index2 );
                        break;
                    }
                }
            }
            return certs.Count > 0 ? arrayList2 : arrayList1;
        }

        public PkixCertPath( ICollection certificates ) => this.certificates = SortCerts( Platform.CreateArrayList( certificates ) );

        public PkixCertPath( Stream inStream )
          : this( inStream, "PkiPath" )
        {
        }

        public PkixCertPath( Stream inStream, string encoding )
        {
            string upperInvariant = Platform.ToUpperInvariant( encoding );
            IList arrayList;
            try
            {
                if (upperInvariant.Equals( Platform.ToUpperInvariant( "PkiPath" ) ))
                {
                    Asn1Object asn1Object = new Asn1InputStream( inStream ).ReadObject();
                    if (!(asn1Object is Asn1Sequence))
                        throw new CertificateException( "input stream does not contain a ASN1 SEQUENCE while reading PkiPath encoded data to load CertPath" );
                    arrayList = Platform.CreateArrayList();
                    foreach (Asn1Encodable asn1Encodable in (Asn1Sequence)asn1Object)
                    {
                        Stream inStream1 = new MemoryStream( asn1Encodable.GetEncoded( "DER" ), false );
                        arrayList.Insert( 0, new X509CertificateParser().ReadCertificate( inStream1 ) );
                    }
                }
                else
                {
                    if (!upperInvariant.Equals( "PKCS7" ) && !upperInvariant.Equals( "PEM" ))
                        throw new CertificateException( "unsupported encoding: " + encoding );
                    arrayList = Platform.CreateArrayList( new X509CertificateParser().ReadCertificates( inStream ) );
                }
            }
            catch (IOException ex)
            {
                throw new CertificateException( "IOException throw while decoding CertPath:\n" + ex.ToString() );
            }
            this.certificates = SortCerts( arrayList );
        }

        public virtual IEnumerable Encodings => new EnumerableProxy( certPathEncodings );

        public override bool Equals( object obj )
        {
            if (this == obj)
                return true;
            if (!(obj is PkixCertPath pkixCertPath))
                return false;
            IList certificates1 = this.Certificates;
            IList certificates2 = pkixCertPath.Certificates;
            if (certificates1.Count != certificates2.Count)
                return false;
            IEnumerator enumerator1 = certificates1.GetEnumerator();
            IEnumerator enumerator2 = certificates1.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                enumerator2.MoveNext();
                if (!Equals( enumerator1.Current, enumerator2.Current ))
                    return false;
            }
            return true;
        }

        public override int GetHashCode() => this.Certificates.GetHashCode();

        public virtual byte[] GetEncoded()
        {
            foreach (object encoding in this.Encodings)
            {
                if (encoding is string)
                    return this.GetEncoded( (string)encoding );
            }
            return null;
        }

        public virtual byte[] GetEncoded( string encoding )
        {
            if (Platform.EqualsIgnoreCase( encoding, "PkiPath" ))
            {
                Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
                for (int index = this.certificates.Count - 1; index >= 0; --index)
                    v.Add( this.ToAsn1Object( (X509Certificate)this.certificates[index] ) );
                return this.ToDerEncoded( new DerSequence( v ) );
            }
            if (Platform.EqualsIgnoreCase( encoding, "PKCS7" ))
            {
                ContentInfo _contentInfo = new ContentInfo( PkcsObjectIdentifiers.Data, null );
                Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
                for (int index = 0; index != this.certificates.Count; ++index)
                    v.Add( this.ToAsn1Object( (X509Certificate)this.certificates[index] ) );
                SignedData content = new SignedData( new DerInteger( 1 ), new DerSet(), _contentInfo, new DerSet( v ), null, new DerSet() );
                return this.ToDerEncoded( new ContentInfo( PkcsObjectIdentifiers.SignedData, content ) );
            }
            if (!Platform.EqualsIgnoreCase( encoding, "PEM" ))
                throw new CertificateEncodingException( "unsupported encoding: " + encoding );
            MemoryStream memoryStream = new MemoryStream();
            PemWriter pemWriter = new PemWriter( new StreamWriter( memoryStream ) );
            try
            {
                for (int index = 0; index != this.certificates.Count; ++index)
                    pemWriter.WriteObject( this.certificates[index] );
                Platform.Dispose( pemWriter.Writer );
            }
            catch (Exception ex)
            {
                throw new CertificateEncodingException( "can't encode certificate for PEM encoded path" );
            }
            return memoryStream.ToArray();
        }

        public virtual IList Certificates => CollectionUtilities.ReadOnly( this.certificates );

        private Asn1Object ToAsn1Object( X509Certificate cert )
        {
            try
            {
                return Asn1Object.FromByteArray( cert.GetEncoded() );
            }
            catch (Exception ex)
            {
                throw new CertificateEncodingException( "Exception while encoding certificate", ex );
            }
        }

        private byte[] ToDerEncoded( Asn1Encodable obj )
        {
            try
            {
                return obj.GetEncoded( "DER" );
            }
            catch (IOException ex)
            {
                throw new CertificateEncodingException( "Exception thrown", ex );
            }
        }
    }
}
