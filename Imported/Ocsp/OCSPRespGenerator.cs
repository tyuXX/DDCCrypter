// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.OCSPRespGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using System;

namespace Org.BouncyCastle.Ocsp
{
    public class OCSPRespGenerator
    {
        public const int Successful = 0;
        public const int MalformedRequest = 1;
        public const int InternalError = 2;
        public const int TryLater = 3;
        public const int SigRequired = 5;
        public const int Unauthorized = 6;

        public OcspResp Generate( int status, object response )
        {
            if (response == null)
                return new OcspResp( new OcspResponse( new OcspResponseStatus( status ), null ) );
            BasicOcspResp basicOcspResp = response is BasicOcspResp ? (BasicOcspResp)response : throw new OcspException( "unknown response object" );
            Asn1OctetString response1;
            try
            {
                response1 = new DerOctetString( basicOcspResp.GetEncoded() );
            }
            catch (Exception ex)
            {
                throw new OcspException( "can't encode object.", ex );
            }
            ResponseBytes responseBytes = new ResponseBytes( OcspObjectIdentifiers.PkixOcspBasic, response1 );
            return new OcspResp( new OcspResponse( new OcspResponseStatus( status ), responseBytes ) );
        }
    }
}
