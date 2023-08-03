// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.OcspStatusRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class OcspStatusRequest
    {
        protected readonly IList mResponderIDList;
        protected readonly X509Extensions mRequestExtensions;

        public OcspStatusRequest( IList responderIDList, X509Extensions requestExtensions )
        {
            this.mResponderIDList = responderIDList;
            this.mRequestExtensions = requestExtensions;
        }

        public virtual IList ResponderIDList => this.mResponderIDList;

        public virtual X509Extensions RequestExtensions => this.mRequestExtensions;

        public virtual void Encode( Stream output )
        {
            if (this.mResponderIDList == null || this.mResponderIDList.Count < 1)
            {
                TlsUtilities.WriteUint16( 0, output );
            }
            else
            {
                MemoryStream output1 = new MemoryStream();
                for (int index = 0; index < this.mResponderIDList.Count; ++index)
                    TlsUtilities.WriteOpaque16( ((Asn1Encodable)this.mResponderIDList[index]).GetEncoded( "DER" ), output1 );
                TlsUtilities.CheckUint16( output1.Length );
                TlsUtilities.WriteUint16( (int)output1.Length, output );
                output1.WriteTo( output );
            }
            if (this.mRequestExtensions == null)
            {
                TlsUtilities.WriteUint16( 0, output );
            }
            else
            {
                byte[] encoded = this.mRequestExtensions.GetEncoded( "DER" );
                TlsUtilities.CheckUint16( encoded.Length );
                TlsUtilities.WriteUint16( encoded.Length, output );
                output.Write( encoded, 0, encoded.Length );
            }
        }

        public static OcspStatusRequest Parse( Stream input )
        {
            IList arrayList = Platform.CreateArrayList();
            int length1 = TlsUtilities.ReadUint16( input );
            if (length1 > 0)
            {
                MemoryStream input1 = new MemoryStream( TlsUtilities.ReadFully( length1, input ), false );
                do
                {
                    ResponderID instance = ResponderID.GetInstance( TlsUtilities.ReadDerObject( TlsUtilities.ReadOpaque16( input1 ) ) );
                    arrayList.Add( instance );
                }
                while (input1.Position < input1.Length);
            }
            X509Extensions requestExtensions = null;
            int length2 = TlsUtilities.ReadUint16( input );
            if (length2 > 0)
                requestExtensions = X509Extensions.GetInstance( TlsUtilities.ReadDerObject( TlsUtilities.ReadFully( length2, input ) ) );
            return new OcspStatusRequest( arrayList, requestExtensions );
        }
    }
}
