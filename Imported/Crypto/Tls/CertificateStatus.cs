// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.CertificateStatus
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class CertificateStatus
    {
        protected readonly byte mStatusType;
        protected readonly object mResponse;

        public CertificateStatus( byte statusType, object response )
        {
            this.mStatusType = IsCorrectType( statusType, response ) ? statusType : throw new ArgumentException( "not an instance of the correct type", nameof( response ) );
            this.mResponse = response;
        }

        public virtual byte StatusType => this.mStatusType;

        public virtual object Response => this.mResponse;

        public virtual OcspResponse GetOcspResponse() => IsCorrectType( 1, this.mResponse ) ? (OcspResponse)this.mResponse : throw new InvalidOperationException( "'response' is not an OcspResponse" );

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteUint8( this.mStatusType, output );
            if (this.mStatusType != 1)
                throw new TlsFatalAlert( 80 );
            TlsUtilities.WriteOpaque24( ((Asn1Encodable)this.mResponse).GetEncoded( "DER" ), output );
        }

        public static CertificateStatus Parse( Stream input )
        {
            byte statusType = TlsUtilities.ReadUint8( input );
            if (statusType != 1)
                throw new TlsFatalAlert( 50 );
            object instance = OcspResponse.GetInstance( TlsUtilities.ReadDerObject( TlsUtilities.ReadOpaque24( input ) ) );
            return new CertificateStatus( statusType, instance );
        }

        protected static bool IsCorrectType( byte statusType, object response )
        {
            if (statusType == 1)
                return response is OcspResponse;
            throw new ArgumentException( "unsupported value", nameof( statusType ) );
        }
    }
}
