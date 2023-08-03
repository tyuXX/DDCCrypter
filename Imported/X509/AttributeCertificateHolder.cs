// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.AttributeCertificateHolder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509.Store;

namespace Org.BouncyCastle.X509
{
    public class AttributeCertificateHolder : IX509Selector, ICloneable
    {
        internal readonly Holder holder;

        internal AttributeCertificateHolder( Asn1Sequence seq ) => this.holder = Holder.GetInstance( seq );

        public AttributeCertificateHolder( X509Name issuerName, BigInteger serialNumber ) => this.holder = new Holder( new IssuerSerial( this.GenerateGeneralNames( issuerName ), new DerInteger( serialNumber ) ) );

        public AttributeCertificateHolder( X509Certificate cert )
        {
            X509Name issuerX509Principal;
            try
            {
                issuerX509Principal = PrincipalUtilities.GetIssuerX509Principal( cert );
            }
            catch (Exception ex)
            {
                throw new CertificateParsingException( ex.Message );
            }
            this.holder = new Holder( new IssuerSerial( this.GenerateGeneralNames( issuerX509Principal ), new DerInteger( cert.SerialNumber ) ) );
        }

        public AttributeCertificateHolder( X509Name principal ) => this.holder = new Holder( this.GenerateGeneralNames( principal ) );

        public AttributeCertificateHolder(
          int digestedObjectType,
          string digestAlgorithm,
          string otherObjectTypeID,
          byte[] objectDigest )
        {
            this.holder = new Holder( new ObjectDigestInfo( digestedObjectType, otherObjectTypeID, new AlgorithmIdentifier( new DerObjectIdentifier( digestAlgorithm ) ), Arrays.Clone( objectDigest ) ) );
        }

        public int DigestedObjectType
        {
            get
            {
                ObjectDigestInfo objectDigestInfo = this.holder.ObjectDigestInfo;
                return objectDigestInfo != null ? objectDigestInfo.DigestedObjectType.Value.IntValue : -1;
            }
        }

        public string DigestAlgorithm => this.holder.ObjectDigestInfo?.DigestAlgorithm.Algorithm.Id;

        public byte[] GetObjectDigest() => this.holder.ObjectDigestInfo?.ObjectDigest.GetBytes();

        public string OtherObjectTypeID => this.holder.ObjectDigestInfo?.OtherObjectTypeID.Id;

        private GeneralNames GenerateGeneralNames( X509Name principal ) => new( new GeneralName( principal ) );

        private bool MatchesDN( X509Name subject, GeneralNames targets )
        {
            GeneralName[] names = targets.GetNames();
            for (int index = 0; index != names.Length; ++index)
            {
                GeneralName generalName = names[index];
                if (generalName.TagNo == 4)
                {
                    try
                    {
                        if (X509Name.GetInstance( generalName.Name ).Equivalent( subject ))
                            return true;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return false;
        }

        private object[] GetNames( GeneralName[] names )
        {
            int length = 0;
            for (int index = 0; index != names.Length; ++index)
            {
                if (names[index].TagNo == 4)
                    ++length;
            }
            object[] names1 = new object[length];
            int num = 0;
            for (int index = 0; index != names.Length; ++index)
            {
                if (names[index].TagNo == 4)
                    names1[num++] = X509Name.GetInstance( names[index].Name );
            }
            return names1;
        }

        private X509Name[] GetPrincipals( GeneralNames names )
        {
            object[] names1 = this.GetNames( names.GetNames() );
            int length = 0;
            for (int index = 0; index != names1.Length; ++index)
            {
                if (names1[index] is X509Name)
                    ++length;
            }
            X509Name[] principals = new X509Name[length];
            int num = 0;
            for (int index = 0; index != names1.Length; ++index)
            {
                if (names1[index] is X509Name)
                    principals[num++] = (X509Name)names1[index];
            }
            return principals;
        }

        public X509Name[] GetEntityNames() => this.holder.EntityName != null ? this.GetPrincipals( this.holder.EntityName ) : null;

        public X509Name[] GetIssuer() => this.holder.BaseCertificateID != null ? this.GetPrincipals( this.holder.BaseCertificateID.Issuer ) : null;

        public BigInteger SerialNumber => this.holder.BaseCertificateID != null ? this.holder.BaseCertificateID.Serial.Value : null;

        public object Clone() => new AttributeCertificateHolder( (Asn1Sequence)this.holder.ToAsn1Object() );

        public bool Match( X509Certificate x509Cert )
        {
            try
            {
                if (this.holder.BaseCertificateID != null)
                    return this.holder.BaseCertificateID.Serial.Value.Equals( x509Cert.SerialNumber ) && this.MatchesDN( PrincipalUtilities.GetIssuerX509Principal( x509Cert ), this.holder.BaseCertificateID.Issuer );
                if (this.holder.EntityName != null && this.MatchesDN( PrincipalUtilities.GetSubjectX509Principal( x509Cert ), this.holder.EntityName ))
                    return true;
                if (this.holder.ObjectDigestInfo != null)
                {
                    IDigest digest;
                    try
                    {
                        digest = DigestUtilities.GetDigest( this.DigestAlgorithm );
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    switch (this.DigestedObjectType)
                    {
                        case 0:
                            byte[] encoded1 = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( x509Cert.GetPublicKey() ).GetEncoded();
                            digest.BlockUpdate( encoded1, 0, encoded1.Length );
                            break;
                        case 1:
                            byte[] encoded2 = x509Cert.GetEncoded();
                            digest.BlockUpdate( encoded2, 0, encoded2.Length );
                            break;
                    }
                    if (!Arrays.AreEqual( DigestUtilities.DoFinal( digest ), this.GetObjectDigest() ))
                        return false;
                }
            }
            catch (CertificateEncodingException ex)
            {
                return false;
            }
            return false;
        }

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is AttributeCertificateHolder && this.holder.Equals( ((AttributeCertificateHolder)obj).holder );
        }

        public override int GetHashCode() => this.holder.GetHashCode();

        public bool Match( object obj ) => obj is X509Certificate && this.Match( (X509Certificate)obj );
    }
}
