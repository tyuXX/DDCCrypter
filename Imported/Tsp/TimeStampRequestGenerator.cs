// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampRequestGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampRequestGenerator
    {
        private DerObjectIdentifier reqPolicy;
        private DerBoolean certReq;
        private IDictionary extensions = Platform.CreateHashtable();
        private IList extOrdering = Platform.CreateArrayList();

        public void SetReqPolicy( string reqPolicy ) => this.reqPolicy = new DerObjectIdentifier( reqPolicy );

        public void SetCertReq( bool certReq ) => this.certReq = DerBoolean.GetInstance( certReq );

        [Obsolete( "Use method taking DerObjectIdentifier" )]
        public void AddExtension( string oid, bool critical, Asn1Encodable value ) => this.AddExtension( oid, critical, value.GetEncoded() );

        [Obsolete( "Use method taking DerObjectIdentifier" )]
        public void AddExtension( string oid, bool critical, byte[] value )
        {
            DerObjectIdentifier key = new( oid );
            this.extensions[key] = new X509Extension( critical, new DerOctetString( value ) );
            this.extOrdering.Add( key );
        }

        public virtual void AddExtension(
          DerObjectIdentifier oid,
          bool critical,
          Asn1Encodable extValue )
        {
            this.AddExtension( oid, critical, extValue.GetEncoded() );
        }

        public virtual void AddExtension( DerObjectIdentifier oid, bool critical, byte[] extValue )
        {
            this.extensions.Add( oid, new X509Extension( critical, new DerOctetString( extValue ) ) );
            this.extOrdering.Add( oid );
        }

        public TimeStampRequest Generate( string digestAlgorithm, byte[] digest ) => this.Generate( digestAlgorithm, digest, null );

        public TimeStampRequest Generate( string digestAlgorithmOid, byte[] digest, BigInteger nonce )
        {
            if (digestAlgorithmOid == null)
                throw new ArgumentException( "No digest algorithm specified" );
            MessageImprint messageImprint = new( new AlgorithmIdentifier( new DerObjectIdentifier( digestAlgorithmOid ), DerNull.Instance ), digest );
            X509Extensions extensions = null;
            if (this.extOrdering.Count != 0)
                extensions = new X509Extensions( this.extOrdering, this.extensions );
            DerInteger nonce1 = nonce == null ? null : new DerInteger( nonce );
            return new TimeStampRequest( new TimeStampReq( messageImprint, this.reqPolicy, nonce1, this.certReq, extensions ) );
        }

        public virtual TimeStampRequest Generate( DerObjectIdentifier digestAlgorithm, byte[] digest ) => this.Generate( digestAlgorithm.Id, digest );

        public virtual TimeStampRequest Generate(
          DerObjectIdentifier digestAlgorithm,
          byte[] digest,
          BigInteger nonce )
        {
            return this.Generate( digestAlgorithm.Id, digest, nonce );
        }
    }
}
