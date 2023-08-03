// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.OcspResp
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IO;

namespace Org.BouncyCastle.Ocsp
{
    public class OcspResp
    {
        private OcspResponse resp;

        public OcspResp( OcspResponse resp ) => this.resp = resp;

        public OcspResp( byte[] resp )
          : this( new Asn1InputStream( resp ) )
        {
        }

        public OcspResp( Stream inStr )
          : this( new Asn1InputStream( inStr ) )
        {
        }

        private OcspResp( Asn1InputStream aIn )
        {
            try
            {
                this.resp = OcspResponse.GetInstance( aIn.ReadObject() );
            }
            catch (Exception ex)
            {
                throw new IOException( "malformed response: " + ex.Message, ex );
            }
        }

        public int Status => this.resp.ResponseStatus.Value.IntValue;

        public object GetResponseObject()
        {
            ResponseBytes responseBytes = this.resp.ResponseBytes;
            if (responseBytes == null)
                return null;
            if (!responseBytes.ResponseType.Equals( OcspObjectIdentifiers.PkixOcspBasic ))
                return responseBytes.Response;
            try
            {
                return new BasicOcspResp( BasicOcspResponse.GetInstance( Asn1Object.FromByteArray( responseBytes.Response.GetOctets() ) ) );
            }
            catch (Exception ex)
            {
                throw new OcspException( "problem decoding object: " + ex, ex );
            }
        }

        public byte[] GetEncoded() => this.resp.GetEncoded();

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is OcspResp ocspResp && this.resp.Equals( ocspResp.resp );
        }

        public override int GetHashCode() => this.resp.GetHashCode();
    }
}
