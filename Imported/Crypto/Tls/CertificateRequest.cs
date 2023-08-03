// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.CertificateRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class CertificateRequest
    {
        protected readonly byte[] mCertificateTypes;
        protected readonly IList mSupportedSignatureAlgorithms;
        protected readonly IList mCertificateAuthorities;

        public CertificateRequest(
          byte[] certificateTypes,
          IList supportedSignatureAlgorithms,
          IList certificateAuthorities )
        {
            this.mCertificateTypes = certificateTypes;
            this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
            this.mCertificateAuthorities = certificateAuthorities;
        }

        public virtual byte[] CertificateTypes => this.mCertificateTypes;

        public virtual IList SupportedSignatureAlgorithms => this.mSupportedSignatureAlgorithms;

        public virtual IList CertificateAuthorities => this.mCertificateAuthorities;

        public virtual void Encode( Stream output )
        {
            if (this.mCertificateTypes == null || this.mCertificateTypes.Length == 0)
                TlsUtilities.WriteUint8( 0, output );
            else
                TlsUtilities.WriteUint8ArrayWithUint8Length( this.mCertificateTypes, output );
            if (this.mSupportedSignatureAlgorithms != null)
                TlsUtilities.EncodeSupportedSignatureAlgorithms( this.mSupportedSignatureAlgorithms, false, output );
            if (this.mCertificateAuthorities == null || this.mCertificateAuthorities.Count < 1)
            {
                TlsUtilities.WriteUint16( 0, output );
            }
            else
            {
                IList arrayList = Platform.CreateArrayList( this.mCertificateAuthorities.Count );
                int i = 0;
                foreach (Asn1Encodable certificateAuthority in (IEnumerable)this.mCertificateAuthorities)
                {
                    byte[] encoded = certificateAuthority.GetEncoded( "DER" );
                    arrayList.Add( encoded );
                    i += encoded.Length + 2;
                }
                TlsUtilities.CheckUint16( i );
                TlsUtilities.WriteUint16( i, output );
                foreach (byte[] buf in (IEnumerable)arrayList)
                    TlsUtilities.WriteOpaque16( buf, output );
            }
        }

        public static CertificateRequest Parse( TlsContext context, Stream input )
        {
            int length = TlsUtilities.ReadUint8( input );
            byte[] certificateTypes = new byte[length];
            for (int index = 0; index < length; ++index)
                certificateTypes[index] = TlsUtilities.ReadUint8( input );
            IList supportedSignatureAlgorithms = null;
            if (TlsUtilities.IsTlsV12( context ))
                supportedSignatureAlgorithms = TlsUtilities.ParseSupportedSignatureAlgorithms( false, input );
            IList arrayList = Platform.CreateArrayList();
            MemoryStream input1 = new MemoryStream( TlsUtilities.ReadOpaque16( input ), false );
            while (input1.Position < input1.Length)
            {
                Asn1Object asn1Object = TlsUtilities.ReadDerObject( TlsUtilities.ReadOpaque16( input1 ) );
                arrayList.Add( X509Name.GetInstance( asn1Object ) );
            }
            return new CertificateRequest( certificateTypes, supportedSignatureAlgorithms, arrayList );
        }
    }
}
