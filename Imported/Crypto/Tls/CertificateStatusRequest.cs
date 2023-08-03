// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.CertificateStatusRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class CertificateStatusRequest
    {
        protected readonly byte mStatusType;
        protected readonly object mRequest;

        public CertificateStatusRequest( byte statusType, object request )
        {
            this.mStatusType = IsCorrectType( statusType, request ) ? statusType : throw new ArgumentException( "not an instance of the correct type", nameof( request ) );
            this.mRequest = request;
        }

        public virtual byte StatusType => this.mStatusType;

        public virtual object Request => this.mRequest;

        public virtual OcspStatusRequest GetOcspStatusRequest() => IsCorrectType( 1, this.mRequest ) ? (OcspStatusRequest)this.mRequest : throw new InvalidOperationException( "'request' is not an OCSPStatusRequest" );

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteUint8( this.mStatusType, output );
            if (this.mStatusType != 1)
                throw new TlsFatalAlert( 80 );
            ((OcspStatusRequest)this.mRequest).Encode( output );
        }

        public static CertificateStatusRequest Parse( Stream input )
        {
            byte statusType = TlsUtilities.ReadUint8( input );
            if (statusType != 1)
                throw new TlsFatalAlert( 50 );
            object request = OcspStatusRequest.Parse( input );
            return new CertificateStatusRequest( statusType, request );
        }

        protected static bool IsCorrectType( byte statusType, object request )
        {
            if (statusType == 1)
                return request is OcspStatusRequest;
            throw new ArgumentException( "unsupported value", nameof( statusType ) );
        }
    }
}
