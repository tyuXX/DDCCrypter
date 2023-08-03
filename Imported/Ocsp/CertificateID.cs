// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.CertificateID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;

namespace Org.BouncyCastle.Ocsp
{
    public class CertificateID
    {
        public const string HashSha1 = "1.3.14.3.2.26";
        private readonly CertID id;

        public CertificateID( CertID id ) => this.id = id != null ? id : throw new ArgumentNullException( nameof( id ) );

        public CertificateID( string hashAlgorithm, X509Certificate issuerCert, BigInteger serialNumber ) => this.id = CreateCertID( new AlgorithmIdentifier( new DerObjectIdentifier( hashAlgorithm ), DerNull.Instance ), issuerCert, new DerInteger( serialNumber ) );

        public string HashAlgOid => this.id.HashAlgorithm.Algorithm.Id;

        public byte[] GetIssuerNameHash() => this.id.IssuerNameHash.GetOctets();

        public byte[] GetIssuerKeyHash() => this.id.IssuerKeyHash.GetOctets();

        public BigInteger SerialNumber => this.id.SerialNumber.Value;

        public bool MatchesIssuer( X509Certificate issuerCert ) => CreateCertID( this.id.HashAlgorithm, issuerCert, this.id.SerialNumber ).Equals( id );

        public CertID ToAsn1Object() => this.id;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is CertificateID certificateId && this.id.ToAsn1Object().Equals( certificateId.id.ToAsn1Object() );
        }

        public override int GetHashCode() => this.id.ToAsn1Object().GetHashCode();

        public static CertificateID DeriveCertificateID(
          CertificateID original,
          BigInteger newSerialNumber )
        {
            return new CertificateID( new CertID( original.id.HashAlgorithm, original.id.IssuerNameHash, original.id.IssuerKeyHash, new DerInteger( newSerialNumber ) ) );
        }

        private static CertID CreateCertID(
          AlgorithmIdentifier hashAlg,
          X509Certificate issuerCert,
          DerInteger serialNumber )
        {
            try
            {
                string id = hashAlg.Algorithm.Id;
                X509Name subjectX509Principal = PrincipalUtilities.GetSubjectX509Principal( issuerCert );
                byte[] digest1 = DigestUtilities.CalculateDigest( id, subjectX509Principal.GetEncoded() );
                SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( issuerCert.GetPublicKey() );
                byte[] digest2 = DigestUtilities.CalculateDigest( id, subjectPublicKeyInfo.PublicKeyData.GetBytes() );
                return new CertID( hashAlg, new DerOctetString( digest1 ), new DerOctetString( digest2 ), serialNumber );
            }
            catch (Exception ex)
            {
                throw new OcspException( "problem creating ID: " + ex, ex );
            }
        }
    }
}
