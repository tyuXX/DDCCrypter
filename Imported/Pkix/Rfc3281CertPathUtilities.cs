// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.Rfc3281CertPathUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    internal class Rfc3281CertPathUtilities
    {
        internal static void ProcessAttrCert7(
          IX509AttributeCertificate attrCert,
          PkixCertPath certPath,
          PkixCertPath holderCertPath,
          PkixParameters pkixParams )
        {
            ISet criticalExtensionOids = attrCert.GetCriticalExtensionOids();
            if (criticalExtensionOids.Contains( X509Extensions.TargetInformation.Id ))
            {
                try
                {
                    TargetInformation.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( attrCert, X509Extensions.TargetInformation ) );
                }
                catch (Exception ex)
                {
                    throw new PkixCertPathValidatorException( "Target information extension could not be read.", ex );
                }
            }
            criticalExtensionOids.Remove( X509Extensions.TargetInformation.Id );
            foreach (PkixAttrCertChecker attrCertChecker in (IEnumerable)pkixParams.GetAttrCertCheckers())
                attrCertChecker.Check( attrCert, certPath, holderCertPath, criticalExtensionOids );
            if (!criticalExtensionOids.IsEmpty)
                throw new PkixCertPathValidatorException( "Attribute certificate contains unsupported critical extensions: " + criticalExtensionOids );
        }

        internal static void CheckCrls(
          IX509AttributeCertificate attrCert,
          PkixParameters paramsPKIX,
          X509Certificate issuerCert,
          DateTime validDate,
          IList certPathCerts )
        {
            if (!paramsPKIX.IsRevocationEnabled)
                return;
            if (attrCert.GetExtensionValue( X509Extensions.NoRevAvail ) == null)
            {
                CrlDistPoint instance;
                try
                {
                    instance = CrlDistPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( attrCert, X509Extensions.CrlDistributionPoints ) );
                }
                catch (Exception ex)
                {
                    throw new PkixCertPathValidatorException( "CRL distribution point extension could not be read.", ex );
                }
                try
                {
                    PkixCertPathValidatorUtilities.AddAdditionalStoresFromCrlDistributionPoint( instance, paramsPKIX );
                }
                catch (Exception ex)
                {
                    throw new PkixCertPathValidatorException( "No additional CRL locations could be decoded from CRL distribution point extension.", ex );
                }
                CertStatus certStatus = new();
                ReasonsMask reasonMask = new();
                Exception cause = null;
                bool flag = false;
                if (instance != null)
                {
                    DistributionPoint[] distributionPoints;
                    try
                    {
                        distributionPoints = instance.GetDistributionPoints();
                    }
                    catch (Exception ex)
                    {
                        throw new PkixCertPathValidatorException( "Distribution points could not be read.", ex );
                    }
                    try
                    {
                        for (int index = 0; index < distributionPoints.Length; ++index)
                        {
                            if (certStatus.Status == 11)
                            {
                                if (!reasonMask.IsAllReasons)
                                {
                                    PkixParameters paramsPKIX1 = (PkixParameters)paramsPKIX.Clone();
                                    CheckCrl( distributionPoints[index], attrCert, paramsPKIX1, validDate, issuerCert, certStatus, reasonMask, certPathCerts );
                                    flag = true;
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        cause = new Exception( "No valid CRL for distribution point found.", ex );
                    }
                }
                if (certStatus.Status == 11)
                {
                    if (!reasonMask.IsAllReasons)
                    {
                        try
                        {
                            Asn1Object name;
                            try
                            {
                                name = new Asn1InputStream( attrCert.Issuer.GetPrincipals()[0].GetEncoded() ).ReadObject();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception( "Issuer from certificate for CRL could not be reencoded.", ex );
                            }
                            DistributionPoint dp = new( new DistributionPointName( 0, new GeneralNames( new GeneralName( 4, name ) ) ), null, null );
                            PkixParameters paramsPKIX2 = (PkixParameters)paramsPKIX.Clone();
                            CheckCrl( dp, attrCert, paramsPKIX2, validDate, issuerCert, certStatus, reasonMask, certPathCerts );
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            cause = new Exception( "No valid CRL for distribution point found.", ex );
                        }
                    }
                }
                if (!flag)
                    throw new PkixCertPathValidatorException( "No valid CRL found.", cause );
                if (certStatus.Status != 11)
                    throw new PkixCertPathValidatorException( "Attribute certificate revocation after " + certStatus.RevocationDate.Value.ToString( "ddd MMM dd HH:mm:ss K yyyy" ) + ", reason: " + Rfc3280CertPathUtilities.CrlReasons[certStatus.Status] );
                if (!reasonMask.IsAllReasons && certStatus.Status == 11)
                    certStatus.Status = 12;
                if (certStatus.Status == 12)
                    throw new PkixCertPathValidatorException( "Attribute certificate status could not be determined." );
            }
            else if (attrCert.GetExtensionValue( X509Extensions.CrlDistributionPoints ) != null || attrCert.GetExtensionValue( X509Extensions.AuthorityInfoAccess ) != null)
                throw new PkixCertPathValidatorException( "No rev avail extension is set, but also an AC revocation pointer." );
        }

        internal static void AdditionalChecks(
          IX509AttributeCertificate attrCert,
          PkixParameters pkixParams )
        {
            foreach (string prohibitedAcAttribute in (IEnumerable)pkixParams.GetProhibitedACAttributes())
            {
                if (attrCert.GetAttributes( prohibitedAcAttribute ) != null)
                    throw new PkixCertPathValidatorException( "Attribute certificate contains prohibited attribute: " + prohibitedAcAttribute + "." );
            }
            foreach (string necessaryAcAttribute in (IEnumerable)pkixParams.GetNecessaryACAttributes())
            {
                if (attrCert.GetAttributes( necessaryAcAttribute ) == null)
                    throw new PkixCertPathValidatorException( "Attribute certificate does not contain necessary attribute: " + necessaryAcAttribute + "." );
            }
        }

        internal static void ProcessAttrCert5(
          IX509AttributeCertificate attrCert,
          PkixParameters pkixParams )
        {
            try
            {
                attrCert.CheckValidity( PkixCertPathValidatorUtilities.GetValidDate( pkixParams ) );
            }
            catch (CertificateExpiredException ex)
            {
                throw new PkixCertPathValidatorException( "Attribute certificate is not valid.", ex );
            }
            catch (CertificateNotYetValidException ex)
            {
                throw new PkixCertPathValidatorException( "Attribute certificate is not valid.", ex );
            }
        }

        internal static void ProcessAttrCert4( X509Certificate acIssuerCert, PkixParameters pkixParams )
        {
            ISet trustedAcIssuers = pkixParams.GetTrustedACIssuers();
            bool flag = false;
            foreach (TrustAnchor trustAnchor in (IEnumerable)trustedAcIssuers)
            {
                IDictionary rfC2253Symbols = X509Name.RFC2253Symbols;
                if (acIssuerCert.SubjectDN.ToString( false, rfC2253Symbols ).Equals( trustAnchor.CAName ) || acIssuerCert.Equals( trustAnchor.TrustedCert ))
                    flag = true;
            }
            if (!flag)
                throw new PkixCertPathValidatorException( "Attribute certificate issuer is not directly trusted." );
        }

        internal static void ProcessAttrCert3( X509Certificate acIssuerCert, PkixParameters pkixParams )
        {
            if (acIssuerCert.GetKeyUsage() != null && !acIssuerCert.GetKeyUsage()[0] && !acIssuerCert.GetKeyUsage()[1])
                throw new PkixCertPathValidatorException( "Attribute certificate issuer public key cannot be used to validate digital signatures." );
            if (acIssuerCert.GetBasicConstraints() != -1)
                throw new PkixCertPathValidatorException( "Attribute certificate issuer is also a public key certificate issuer." );
        }

        internal static PkixCertPathValidatorResult ProcessAttrCert2(
          PkixCertPath certPath,
          PkixParameters pkixParams )
        {
            PkixCertPathValidator certPathValidator = new();
            try
            {
                return certPathValidator.Validate( certPath, pkixParams );
            }
            catch (PkixCertPathValidatorException ex)
            {
                throw new PkixCertPathValidatorException( "Certification path for issuer certificate of attribute certificate could not be validated.", ex );
            }
        }

        internal static PkixCertPath ProcessAttrCert1(
          IX509AttributeCertificate attrCert,
          PkixParameters pkixParams )
        {
            PkixCertPathBuilderResult pathBuilderResult = null;
            ISet set = new HashSet();
            if (attrCert.Holder.GetIssuer() != null)
            {
                X509CertStoreSelector certSelect = new()
                {
                    SerialNumber = attrCert.Holder.SerialNumber
                };
                foreach (X509Name x509Name in attrCert.Holder.GetIssuer())
                {
                    try
                    {
                        certSelect.Issuer = x509Name;
                        set.AddAll( PkixCertPathValidatorUtilities.FindCertificates( certSelect, pkixParams.GetStores() ) );
                    }
                    catch (Exception ex)
                    {
                        throw new PkixCertPathValidatorException( "Public key certificate for attribute certificate cannot be searched.", ex );
                    }
                }
                if (set.IsEmpty)
                    throw new PkixCertPathValidatorException( "Public key certificate specified in base certificate ID for attribute certificate cannot be found." );
            }
            if (attrCert.Holder.GetEntityNames() != null)
            {
                X509CertStoreSelector certSelect = new();
                foreach (X509Name entityName in attrCert.Holder.GetEntityNames())
                {
                    try
                    {
                        certSelect.Issuer = entityName;
                        set.AddAll( PkixCertPathValidatorUtilities.FindCertificates( certSelect, pkixParams.GetStores() ) );
                    }
                    catch (Exception ex)
                    {
                        throw new PkixCertPathValidatorException( "Public key certificate for attribute certificate cannot be searched.", ex );
                    }
                }
                if (set.IsEmpty)
                    throw new PkixCertPathValidatorException( "Public key certificate specified in entity name for attribute certificate cannot be found." );
            }
            PkixBuilderParameters instance = PkixBuilderParameters.GetInstance( pkixParams );
            PkixCertPathValidatorException validatorException = null;
            foreach (X509Certificate x509Certificate in (IEnumerable)set)
            {
                instance.SetTargetConstraints( new X509CertStoreSelector()
                {
                    Certificate = x509Certificate
                } );
                PkixCertPathBuilder pkixCertPathBuilder = new();
                try
                {
                    pathBuilderResult = pkixCertPathBuilder.Build( PkixBuilderParameters.GetInstance( instance ) );
                }
                catch (PkixCertPathBuilderException ex)
                {
                    validatorException = new PkixCertPathValidatorException( "Certification path for public key certificate of attribute certificate could not be build.", ex );
                }
            }
            if (validatorException != null)
                throw validatorException;
            return pathBuilderResult.CertPath;
        }

        private static void CheckCrl(
          DistributionPoint dp,
          IX509AttributeCertificate attrCert,
          PkixParameters paramsPKIX,
          DateTime validDate,
          X509Certificate issuerCert,
          CertStatus certStatus,
          ReasonsMask reasonMask,
          IList certPathCerts )
        {
            if (attrCert.GetExtensionValue( X509Extensions.NoRevAvail ) != null)
                return;
            DateTime utcNow = DateTime.UtcNow;
            if (validDate.CompareTo( (object)utcNow ) > 0)
                throw new Exception( "Validation time is in future." );
            ISet completeCrls = PkixCertPathValidatorUtilities.GetCompleteCrls( dp, attrCert, utcNow, paramsPKIX );
            bool flag = false;
            Exception exception = null;
            IEnumerator enumerator = completeCrls.GetEnumerator();
            while (enumerator.MoveNext() && certStatus.Status == 11)
            {
                if (!reasonMask.IsAllReasons)
                {
                    try
                    {
                        X509Crl current = (X509Crl)enumerator.Current;
                        ReasonsMask mask = Rfc3280CertPathUtilities.ProcessCrlD( current, dp );
                        if (mask.HasNewReasons( reasonMask ))
                        {
                            ISet keys = Rfc3280CertPathUtilities.ProcessCrlF( current, attrCert, null, null, paramsPKIX, certPathCerts );
                            AsymmetricKeyParameter key = Rfc3280CertPathUtilities.ProcessCrlG( current, keys );
                            X509Crl x509Crl = null;
                            if (paramsPKIX.IsUseDeltasEnabled)
                                x509Crl = Rfc3280CertPathUtilities.ProcessCrlH( PkixCertPathValidatorUtilities.GetDeltaCrls( utcNow, paramsPKIX, current ), key );
                            if (paramsPKIX.ValidityModel != 1 && attrCert.NotAfter.CompareTo( (object)current.ThisUpdate ) < 0)
                                throw new Exception( "No valid CRL for current time found." );
                            Rfc3280CertPathUtilities.ProcessCrlB1( dp, attrCert, current );
                            Rfc3280CertPathUtilities.ProcessCrlB2( dp, attrCert, current );
                            Rfc3280CertPathUtilities.ProcessCrlC( x509Crl, current, paramsPKIX );
                            Rfc3280CertPathUtilities.ProcessCrlI( validDate, x509Crl, attrCert, certStatus, paramsPKIX );
                            Rfc3280CertPathUtilities.ProcessCrlJ( validDate, current, attrCert, certStatus );
                            if (certStatus.Status == 8)
                                certStatus.Status = 11;
                            reasonMask.AddReasons( mask );
                            flag = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }
                else
                    break;
            }
            if (!flag)
                throw exception;
        }
    }
}
