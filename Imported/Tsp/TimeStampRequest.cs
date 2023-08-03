// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampRequest : X509ExtensionBase
    {
        private TimeStampReq req;
        private X509Extensions extensions;

        public TimeStampRequest( TimeStampReq req )
        {
            this.req = req;
            this.extensions = req.Extensions;
        }

        public TimeStampRequest( byte[] req )
          : this( new Asn1InputStream( req ) )
        {
        }

        public TimeStampRequest( Stream input )
          : this( new Asn1InputStream( input ) )
        {
        }

        private TimeStampRequest( Asn1InputStream str )
        {
            try
            {
                this.req = TimeStampReq.GetInstance( str.ReadObject() );
            }
            catch (InvalidCastException ex)
            {
                throw new IOException( "malformed request: " + ex );
            }
            catch (ArgumentException ex)
            {
                throw new IOException( "malformed request: " + ex );
            }
        }

        public int Version => this.req.Version.Value.IntValue;

        public string MessageImprintAlgOid => this.req.MessageImprint.HashAlgorithm.Algorithm.Id;

        public byte[] GetMessageImprintDigest() => this.req.MessageImprint.GetHashedMessage();

        public string ReqPolicy => this.req.ReqPolicy != null ? this.req.ReqPolicy.Id : null;

        public BigInteger Nonce => this.req.Nonce != null ? this.req.Nonce.Value : null;

        public bool CertReq => this.req.CertReq != null && this.req.CertReq.IsTrue;

        public void Validate( IList algorithms, IList policies, IList extensions )
        {
            if (!algorithms.Contains( MessageImprintAlgOid ))
                throw new TspValidationException( "request contains unknown algorithm.", 128 );
            if (policies != null && this.ReqPolicy != null && !policies.Contains( ReqPolicy ))
                throw new TspValidationException( "request contains unknown policy.", 256 );
            if (this.Extensions != null && extensions != null)
            {
                foreach (DerObjectIdentifier extensionOid in this.Extensions.ExtensionOids)
                {
                    if (!extensions.Contains( extensionOid.Id ))
                        throw new TspValidationException( "request contains unknown extension.", 8388608 );
                }
            }
            if (TspUtil.GetDigestLength( this.MessageImprintAlgOid ) != this.GetMessageImprintDigest().Length)
                throw new TspValidationException( "imprint digest the wrong length.", 4 );
        }

        public byte[] GetEncoded() => this.req.GetEncoded();

        internal X509Extensions Extensions => this.req.Extensions;

        public virtual bool HasExtensions => this.extensions != null;

        public virtual X509Extension GetExtension( DerObjectIdentifier oid ) => this.extensions != null ? this.extensions.GetExtension( oid ) : null;

        public virtual IList GetExtensionOids() => TspUtil.GetExtensionOids( this.extensions );

        protected override X509Extensions GetX509Extensions() => this.Extensions;
    }
}
