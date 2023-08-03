// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampResponse
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampResponse
    {
        private TimeStampResp resp;
        private TimeStampToken timeStampToken;

        public TimeStampResponse( TimeStampResp resp )
        {
            this.resp = resp;
            if (resp.TimeStampToken == null)
                return;
            this.timeStampToken = new TimeStampToken( resp.TimeStampToken );
        }

        public TimeStampResponse( byte[] resp )
          : this( readTimeStampResp( new Asn1InputStream( resp ) ) )
        {
        }

        public TimeStampResponse( Stream input )
          : this( readTimeStampResp( new Asn1InputStream( input ) ) )
        {
        }

        private static TimeStampResp readTimeStampResp( Asn1InputStream input )
        {
            try
            {
                return TimeStampResp.GetInstance( input.ReadObject() );
            }
            catch (ArgumentException ex)
            {
                throw new TspException( "malformed timestamp response: " + ex, ex );
            }
            catch (InvalidCastException ex)
            {
                throw new TspException( "malformed timestamp response: " + ex, ex );
            }
        }

        public int Status => this.resp.Status.Status.IntValue;

        public string GetStatusString()
        {
            if (this.resp.Status.StatusString == null)
                return null;
            StringBuilder stringBuilder = new();
            PkiFreeText statusString = this.resp.Status.StatusString;
            for (int index = 0; index != statusString.Count; ++index)
                stringBuilder.Append( statusString[index].GetString() );
            return stringBuilder.ToString();
        }

        public PkiFailureInfo GetFailInfo() => this.resp.Status.FailInfo == null ? null : new PkiFailureInfo( this.resp.Status.FailInfo );

        public TimeStampToken TimeStampToken => this.timeStampToken;

        public void Validate( TimeStampRequest request )
        {
            TimeStampToken timeStampToken = this.TimeStampToken;
            if (timeStampToken != null)
            {
                TimeStampTokenInfo timeStampInfo = timeStampToken.TimeStampInfo;
                if (request.Nonce != null && !request.Nonce.Equals( timeStampInfo.Nonce ))
                    throw new TspValidationException( "response contains wrong nonce value." );
                if (this.Status != 0 && this.Status != 1)
                    throw new TspValidationException( "time stamp token found in failed request." );
                if (!Arrays.ConstantTimeAreEqual( request.GetMessageImprintDigest(), timeStampInfo.GetMessageImprintDigest() ))
                    throw new TspValidationException( "response for different message imprint digest." );
                if (!timeStampInfo.MessageImprintAlgOid.Equals( request.MessageImprintAlgOid ))
                    throw new TspValidationException( "response for different message imprint algorithm." );
                Org.BouncyCastle.Asn1.Cms.Attribute signedAttribute1 = timeStampToken.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificate];
                Org.BouncyCastle.Asn1.Cms.Attribute signedAttribute2 = timeStampToken.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificateV2];
                if (signedAttribute1 == null && signedAttribute2 == null)
                    throw new TspValidationException( "no signing certificate attribute present." );
                if (signedAttribute1 != null)
                    ;
                if (request.ReqPolicy != null && !request.ReqPolicy.Equals( timeStampInfo.Policy ))
                    throw new TspValidationException( "TSA policy wrong for request." );
            }
            else if (this.Status == 0 || this.Status == 1)
                throw new TspValidationException( "no time stamp token found and one expected." );
        }

        public byte[] GetEncoded() => this.resp.GetEncoded();
    }
}
