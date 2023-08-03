// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathValidator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCertPathValidator
    {
        public virtual PkixCertPathValidatorResult Validate(
          PkixCertPath certPath,
          PkixParameters paramsPkix )
        {
            if (paramsPkix.GetTrustAnchors() == null)
                throw new ArgumentException( "trustAnchors is null, this is not allowed for certification path validation.", "parameters" );
            IList certificates = certPath.Certificates;
            int count = certificates.Count;
            if (certificates.Count == 0)
                throw new PkixCertPathValidatorException( "Certification path is empty.", null, certPath, 0 );
            ISet initialPolicies = paramsPkix.GetInitialPolicies();
            TrustAnchor trustAnchor;
            try
            {
                trustAnchor = PkixCertPathValidatorUtilities.FindTrustAnchor( (X509Certificate)certificates[certificates.Count - 1], paramsPkix.GetTrustAnchors() );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( ex.Message, ex, certPath, certificates.Count - 1 );
            }
            if (trustAnchor == null)
                throw new PkixCertPathValidatorException( "Trust anchor for certification path not found.", null, certPath, -1 );
            IList[] policyNodes = new IList[count + 1];
            for (int index = 0; index < policyNodes.Length; ++index)
                policyNodes[index] = Platform.CreateArrayList();
            ISet expectedPolicies = new HashSet();
            expectedPolicies.Add( Rfc3280CertPathUtilities.ANY_POLICY );
            PkixPolicyNode validPolicyTree1 = new PkixPolicyNode( Platform.CreateArrayList(), 0, expectedPolicies, null, new HashSet(), Rfc3280CertPathUtilities.ANY_POLICY, false );
            policyNodes[0].Add( validPolicyTree1 );
            PkixNameConstraintValidator nameConstraintValidator = new PkixNameConstraintValidator();
            ISet acceptablePolicies = new HashSet();
            int explicitPolicy1 = !paramsPkix.IsExplicitPolicyRequired ? count + 1 : 0;
            int inhibitAnyPolicy1 = !paramsPkix.IsAnyPolicyInhibited ? count + 1 : 0;
            int policyMapping1 = !paramsPkix.IsPolicyMappingInhibited ? count + 1 : 0;
            X509Certificate sign = trustAnchor.TrustedCert;
            X509Name workingIssuerName;
            AsymmetricKeyParameter asymmetricKeyParameter;
            try
            {
                if (sign != null)
                {
                    workingIssuerName = sign.SubjectDN;
                    asymmetricKeyParameter = sign.GetPublicKey();
                }
                else
                {
                    workingIssuerName = new X509Name( trustAnchor.CAName );
                    asymmetricKeyParameter = trustAnchor.CAPublicKey;
                }
            }
            catch (ArgumentException ex)
            {
                throw new PkixCertPathValidatorException( "Subject of trust anchor could not be (re)encoded.", ex, certPath, -1 );
            }
            try
            {
                PkixCertPathValidatorUtilities.GetAlgorithmIdentifier( asymmetricKeyParameter );
            }
            catch (PkixCertPathValidatorException ex)
            {
                throw new PkixCertPathValidatorException( "Algorithm identifier of public key of trust anchor could not be read.", ex, certPath, -1 );
            }
            int maxPathLength1 = count;
            X509CertStoreSelector targetCertConstraints = paramsPkix.GetTargetCertConstraints();
            if (targetCertConstraints != null && !targetCertConstraints.Match( (X509Certificate)certificates[0] ))
                throw new PkixCertPathValidatorException( "Target certificate in certification path does not match targetConstraints.", null, certPath, 0 );
            IList certPathCheckers = paramsPkix.GetCertPathCheckers();
            foreach (PkixCertPathChecker pkixCertPathChecker in (IEnumerable)certPathCheckers)
                pkixCertPathChecker.Init( false );
            X509Certificate cert = null;
            int index1;
            for (index1 = certificates.Count - 1; index1 >= 0; --index1)
            {
                int num = count - index1;
                cert = (X509Certificate)certificates[index1];
                Rfc3280CertPathUtilities.ProcessCertA( certPath, paramsPkix, index1, asymmetricKeyParameter, workingIssuerName, sign );
                Rfc3280CertPathUtilities.ProcessCertBC( certPath, index1, nameConstraintValidator );
                PkixPolicyNode validPolicyTree2 = Rfc3280CertPathUtilities.ProcessCertD( certPath, index1, acceptablePolicies, validPolicyTree1, policyNodes, inhibitAnyPolicy1 );
                validPolicyTree1 = Rfc3280CertPathUtilities.ProcessCertE( certPath, index1, validPolicyTree2 );
                Rfc3280CertPathUtilities.ProcessCertF( certPath, index1, validPolicyTree1, explicitPolicy1 );
                if (num != count)
                {
                    if (cert != null && cert.Version == 1)
                        throw new PkixCertPathValidatorException( "Version 1 certificates can't be used as CA ones.", null, certPath, index1 );
                    Rfc3280CertPathUtilities.PrepareNextCertA( certPath, index1 );
                    validPolicyTree1 = Rfc3280CertPathUtilities.PrepareCertB( certPath, index1, policyNodes, validPolicyTree1, policyMapping1 );
                    Rfc3280CertPathUtilities.PrepareNextCertG( certPath, index1, nameConstraintValidator );
                    int explicitPolicy2 = Rfc3280CertPathUtilities.PrepareNextCertH1( certPath, index1, explicitPolicy1 );
                    int policyMapping2 = Rfc3280CertPathUtilities.PrepareNextCertH2( certPath, index1, policyMapping1 );
                    int inhibitAnyPolicy2 = Rfc3280CertPathUtilities.PrepareNextCertH3( certPath, index1, inhibitAnyPolicy1 );
                    explicitPolicy1 = Rfc3280CertPathUtilities.PrepareNextCertI1( certPath, index1, explicitPolicy2 );
                    policyMapping1 = Rfc3280CertPathUtilities.PrepareNextCertI2( certPath, index1, policyMapping2 );
                    inhibitAnyPolicy1 = Rfc3280CertPathUtilities.PrepareNextCertJ( certPath, index1, inhibitAnyPolicy2 );
                    Rfc3280CertPathUtilities.PrepareNextCertK( certPath, index1 );
                    int maxPathLength2 = Rfc3280CertPathUtilities.PrepareNextCertL( certPath, index1, maxPathLength1 );
                    maxPathLength1 = Rfc3280CertPathUtilities.PrepareNextCertM( certPath, index1, maxPathLength2 );
                    Rfc3280CertPathUtilities.PrepareNextCertN( certPath, index1 );
                    ISet criticalExtensionOids = cert.GetCriticalExtensionOids();
                    ISet criticalExtensions;
                    if (criticalExtensionOids != null)
                    {
                        criticalExtensions = new HashSet( criticalExtensionOids );
                        criticalExtensions.Remove( X509Extensions.KeyUsage.Id );
                        criticalExtensions.Remove( X509Extensions.CertificatePolicies.Id );
                        criticalExtensions.Remove( X509Extensions.PolicyMappings.Id );
                        criticalExtensions.Remove( X509Extensions.InhibitAnyPolicy.Id );
                        criticalExtensions.Remove( X509Extensions.IssuingDistributionPoint.Id );
                        criticalExtensions.Remove( X509Extensions.DeltaCrlIndicator.Id );
                        criticalExtensions.Remove( X509Extensions.PolicyConstraints.Id );
                        criticalExtensions.Remove( X509Extensions.BasicConstraints.Id );
                        criticalExtensions.Remove( X509Extensions.SubjectAlternativeName.Id );
                        criticalExtensions.Remove( X509Extensions.NameConstraints.Id );
                    }
                    else
                        criticalExtensions = new HashSet();
                    Rfc3280CertPathUtilities.PrepareNextCertO( certPath, index1, criticalExtensions, certPathCheckers );
                    sign = cert;
                    workingIssuerName = sign.SubjectDN;
                    try
                    {
                        asymmetricKeyParameter = PkixCertPathValidatorUtilities.GetNextWorkingKey( certPath.Certificates, index1 );
                    }
                    catch (PkixCertPathValidatorException ex)
                    {
                        throw new PkixCertPathValidatorException( "Next working key could not be retrieved.", ex, certPath, index1 );
                    }
                    PkixCertPathValidatorUtilities.GetAlgorithmIdentifier( asymmetricKeyParameter );
                }
            }
            int explicitPolicy3 = Rfc3280CertPathUtilities.WrapupCertA( explicitPolicy1, cert );
            int num1 = Rfc3280CertPathUtilities.WrapupCertB( certPath, index1 + 1, explicitPolicy3 );
            ISet criticalExtensionOids1 = cert.GetCriticalExtensionOids();
            ISet criticalExtensions1;
            if (criticalExtensionOids1 != null)
            {
                criticalExtensions1 = new HashSet( criticalExtensionOids1 );
                criticalExtensions1.Remove( X509Extensions.KeyUsage.Id );
                criticalExtensions1.Remove( X509Extensions.CertificatePolicies.Id );
                criticalExtensions1.Remove( X509Extensions.PolicyMappings.Id );
                criticalExtensions1.Remove( X509Extensions.InhibitAnyPolicy.Id );
                criticalExtensions1.Remove( X509Extensions.IssuingDistributionPoint.Id );
                criticalExtensions1.Remove( X509Extensions.DeltaCrlIndicator.Id );
                criticalExtensions1.Remove( X509Extensions.PolicyConstraints.Id );
                criticalExtensions1.Remove( X509Extensions.BasicConstraints.Id );
                criticalExtensions1.Remove( X509Extensions.SubjectAlternativeName.Id );
                criticalExtensions1.Remove( X509Extensions.NameConstraints.Id );
                criticalExtensions1.Remove( X509Extensions.CrlDistributionPoints.Id );
            }
            else
                criticalExtensions1 = new HashSet();
            Rfc3280CertPathUtilities.WrapupCertF( certPath, index1 + 1, certPathCheckers, criticalExtensions1 );
            PkixPolicyNode policyTree = Rfc3280CertPathUtilities.WrapupCertG( certPath, paramsPkix, initialPolicies, index1 + 1, policyNodes, validPolicyTree1, acceptablePolicies );
            if (num1 > 0 || policyTree != null)
                return new PkixCertPathValidatorResult( trustAnchor, policyTree, cert.GetPublicKey() );
            throw new PkixCertPathValidatorException( "Path processing failed on policy.", null, certPath, index1 );
        }
    }
}
