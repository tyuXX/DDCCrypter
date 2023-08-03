// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.Certificate
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
    public class Certificate
    {
        public static readonly Certificate EmptyChain = new( new X509CertificateStructure[0] );
        protected readonly X509CertificateStructure[] mCertificateList;

        public Certificate( X509CertificateStructure[] certificateList ) => this.mCertificateList = certificateList != null ? certificateList : throw new ArgumentNullException( nameof( certificateList ) );

        public virtual X509CertificateStructure[] GetCertificateList() => this.CloneCertificateList();

        public virtual X509CertificateStructure GetCertificateAt( int index ) => this.mCertificateList[index];

        public virtual int Length => this.mCertificateList.Length;

        public virtual bool IsEmpty => this.mCertificateList.Length == 0;

        public virtual void Encode( Stream output )
        {
            IList arrayList = Platform.CreateArrayList( this.mCertificateList.Length );
            int i = 0;
            foreach (Asn1Encodable mCertificate in this.mCertificateList)
            {
                byte[] encoded = mCertificate.GetEncoded( "DER" );
                arrayList.Add( encoded );
                i += encoded.Length + 3;
            }
            TlsUtilities.CheckUint24( i );
            TlsUtilities.WriteUint24( i, output );
            foreach (byte[] buf in (IEnumerable)arrayList)
                TlsUtilities.WriteOpaque24( buf, output );
        }

        public static Certificate Parse( Stream input )
        {
            int length = TlsUtilities.ReadUint24( input );
            if (length == 0)
                return EmptyChain;
            MemoryStream input1 = new( TlsUtilities.ReadFully( length, input ), false );
            IList arrayList = Platform.CreateArrayList();
            while (input1.Position < input1.Length)
            {
                Asn1Object asn1Object = TlsUtilities.ReadDerObject( TlsUtilities.ReadOpaque24( input1 ) );
                arrayList.Add( X509CertificateStructure.GetInstance( asn1Object ) );
            }
            X509CertificateStructure[] certificateList = new X509CertificateStructure[arrayList.Count];
            for (int index = 0; index < arrayList.Count; ++index)
                certificateList[index] = (X509CertificateStructure)arrayList[index];
            return new Certificate( certificateList );
        }

        protected virtual X509CertificateStructure[] CloneCertificateList() => (X509CertificateStructure[])this.mCertificateList.Clone();
    }
}
