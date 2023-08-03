// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.Ocsp.RequestedCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Asn1.IsisMtt.Ocsp
{
    public class RequestedCertificate : Asn1Encodable, IAsn1Choice
    {
        private readonly X509CertificateStructure cert;
        private readonly byte[] publicKeyCert;
        private readonly byte[] attributeCert;

        public static RequestedCertificate GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RequestedCertificate _:
                    return (RequestedCertificate)obj;
                case Asn1Sequence _:
                    return new RequestedCertificate( X509CertificateStructure.GetInstance( obj ) );
                case Asn1TaggedObject _:
                    return new RequestedCertificate( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static RequestedCertificate GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            if (!isExplicit)
                throw new ArgumentException( "choice item must be explicitly tagged" );
            return GetInstance( obj.GetObject() );
        }

        private RequestedCertificate( Asn1TaggedObject tagged )
        {
            switch (tagged.TagNo)
            {
                case 0:
                    this.publicKeyCert = Asn1OctetString.GetInstance( tagged, true ).GetOctets();
                    break;
                case 1:
                    this.attributeCert = Asn1OctetString.GetInstance( tagged, true ).GetOctets();
                    break;
                default:
                    throw new ArgumentException( "unknown tag number: " + tagged.TagNo );
            }
        }

        public RequestedCertificate( X509CertificateStructure certificate ) => this.cert = certificate;

        public RequestedCertificate( RequestedCertificate.Choice type, byte[] certificateOctets )
          : this( new DerTaggedObject( (int)type, new DerOctetString( certificateOctets ) ) )
        {
        }

        public RequestedCertificate.Choice Type
        {
            get
            {
                if (this.cert != null)
                    return Choice.Certificate;
                return this.publicKeyCert != null ? Choice.PublicKeyCertificate : Choice.AttributeCertificate;
            }
        }

        public byte[] GetCertificateBytes()
        {
            if (this.cert != null)
            {
                try
                {
                    return this.cert.GetEncoded();
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException( "can't decode certificate: " + ex );
                }
            }
            else
                return this.publicKeyCert != null ? this.publicKeyCert : this.attributeCert;
        }

        public override Asn1Object ToAsn1Object()
        {
            if (this.publicKeyCert != null)
                return new DerTaggedObject( 0, new DerOctetString( this.publicKeyCert ) );
            return this.attributeCert != null ? new DerTaggedObject( 1, new DerOctetString( this.attributeCert ) ) : this.cert.ToAsn1Object();
        }

        public enum Choice
        {
            Certificate = -1, // 0xFFFFFFFF
            PublicKeyCertificate = 0,
            AttributeCertificate = 1,
        }
    }
}
