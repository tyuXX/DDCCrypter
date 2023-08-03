// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.Rfc3280CertPathUtilities
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
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Pkix
{
    public class Rfc3280CertPathUtilities
    {
        private static readonly PkixCrlUtilities CrlUtilities = new PkixCrlUtilities();
        internal static readonly string ANY_POLICY = "2.5.29.32.0";
        internal static readonly int KEY_CERT_SIGN = 5;
        internal static readonly int CRL_SIGN = 6;
        internal static readonly string[] CrlReasons = new string[11]
        {
      "unspecified",
      "keyCompromise",
      "cACompromise",
      "affiliationChanged",
      "superseded",
      "cessationOfOperation",
      "certificateHold",
      "unknown",
      "removeFromCRL",
      "privilegeWithdrawn",
      "aACompromise"
        };

        internal static void ProcessCrlB2( DistributionPoint dp, object cert, X509Crl crl )
        {
            // ISSUE: unable to decompile the method.
        }

        internal static void ProcessCertBC(
          PkixCertPath certPath,
          int index,
          PkixNameConstraintValidator nameConstraintValidator )
        {
            IList certificates = certPath.Certificates;
            X509Certificate x509Certificate = (X509Certificate)certificates[index];
            int count = certificates.Count;
            int num = count - index;
            if (PkixCertPathValidatorUtilities.IsSelfIssued( x509Certificate ) && num < count)
                return;
            Asn1InputStream asn1InputStream = new Asn1InputStream( x509Certificate.SubjectDN.GetEncoded() );
            Asn1Sequence instance1;
            try
            {
                instance1 = Asn1Sequence.GetInstance( asn1InputStream.ReadObject() );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Exception extracting subject name when checking subtrees.", ex, certPath, index );
            }
            try
            {
                nameConstraintValidator.CheckPermittedDN( instance1 );
                nameConstraintValidator.CheckExcludedDN( instance1 );
            }
            catch (PkixNameConstraintValidatorException ex)
            {
                throw new PkixCertPathValidatorException( "Subtree check for certificate subject failed.", ex, certPath, index );
            }
            GeneralNames instance2;
            try
            {
                instance2 = GeneralNames.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( x509Certificate, X509Extensions.SubjectAlternativeName ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Subject alternative name extension could not be decoded.", ex, certPath, index );
            }
            foreach (string name1 in (IEnumerable)X509Name.GetInstance( instance1 ).GetValueList( X509Name.EmailAddress ))
            {
                GeneralName name2 = new GeneralName( 1, name1 );
                try
                {
                    nameConstraintValidator.checkPermitted( name2 );
                    nameConstraintValidator.checkExcluded( name2 );
                }
                catch (PkixNameConstraintValidatorException ex)
                {
                    throw new PkixCertPathValidatorException( "Subtree check for certificate subject alternative email failed.", ex, certPath, index );
                }
            }
            if (instance2 == null)
                return;
            GeneralName[] names;
            try
            {
                names = instance2.GetNames();
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Subject alternative name contents could not be decoded.", ex, certPath, index );
            }
            foreach (GeneralName name in names)
            {
                try
                {
                    nameConstraintValidator.checkPermitted( name );
                    nameConstraintValidator.checkExcluded( name );
                }
                catch (PkixNameConstraintValidatorException ex)
                {
                    throw new PkixCertPathValidatorException( "Subtree check for certificate subject alternative name failed.", ex, certPath, index );
                }
            }
        }

        internal static void PrepareNextCertA( PkixCertPath certPath, int index )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            Asn1Sequence instance1;
            try
            {
                instance1 = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.PolicyMappings ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Policy mappings extension could not be decoded.", ex, certPath, index );
            }
            if (instance1 == null)
                return;
            Asn1Sequence asn1Sequence = instance1;
            for (int index1 = 0; index1 < asn1Sequence.Count; ++index1)
            {
                DerObjectIdentifier instance2;
                DerObjectIdentifier instance3;
                try
                {
                    Asn1Sequence instance4 = Asn1Sequence.GetInstance( asn1Sequence[index1] );
                    instance2 = DerObjectIdentifier.GetInstance( instance4[0] );
                    instance3 = DerObjectIdentifier.GetInstance( instance4[1] );
                }
                catch (Exception ex)
                {
                    throw new PkixCertPathValidatorException( "Policy mappings extension contents could not be decoded.", ex, certPath, index );
                }
                if (ANY_POLICY.Equals( instance2.Id ))
                    throw new PkixCertPathValidatorException( "IssuerDomainPolicy is anyPolicy", null, certPath, index );
                if (ANY_POLICY.Equals( instance3.Id ))
                    throw new PkixCertPathValidatorException( "SubjectDomainPolicy is anyPolicy,", null, certPath, index );
            }
        }

        internal static PkixPolicyNode ProcessCertD(
          PkixCertPath certPath,
          int index,
          ISet acceptablePolicies,
          PkixPolicyNode validPolicyTree,
          IList[] policyNodes,
          int inhibitAnyPolicy )
        {
            IList certificates = certPath.Certificates;
            X509Certificate x509Certificate = (X509Certificate)certificates[index];
            int count = certificates.Count;
            int index1 = count - index;
            Asn1Sequence instance1;
            try
            {
                instance1 = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( x509Certificate, X509Extensions.CertificatePolicies ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Could not read certificate policies extension from certificate.", ex, certPath, index );
            }
            if (instance1 == null || validPolicyTree == null)
                return null;
            ISet e1 = new HashSet();
            foreach (Asn1Encodable asn1Encodable in instance1)
            {
                PolicyInformation instance2 = PolicyInformation.GetInstance( asn1Encodable.ToAsn1Object() );
                DerObjectIdentifier policyIdentifier = instance2.PolicyIdentifier;
                e1.Add( policyIdentifier.Id );
                if (!ANY_POLICY.Equals( policyIdentifier.Id ))
                {
                    ISet qualifierSet;
                    try
                    {
                        qualifierSet = PkixCertPathValidatorUtilities.GetQualifierSet( instance2.PolicyQualifiers );
                    }
                    catch (PkixCertPathValidatorException ex)
                    {
                        throw new PkixCertPathValidatorException( "Policy qualifier info set could not be build.", ex, certPath, index );
                    }
                    if (!PkixCertPathValidatorUtilities.ProcessCertD1i( index1, policyNodes, policyIdentifier, qualifierSet ))
                        PkixCertPathValidatorUtilities.ProcessCertD1ii( index1, policyNodes, policyIdentifier, qualifierSet );
                }
            }
            if (acceptablePolicies.IsEmpty || acceptablePolicies.Contains( ANY_POLICY ))
            {
                acceptablePolicies.Clear();
                acceptablePolicies.AddAll( e1 );
            }
            else
            {
                ISet e2 = new HashSet();
                foreach (object acceptablePolicy in (IEnumerable)acceptablePolicies)
                {
                    if (e1.Contains( acceptablePolicy ))
                        e2.Add( acceptablePolicy );
                }
                acceptablePolicies.Clear();
                acceptablePolicies.AddAll( e2 );
            }
            if (inhibitAnyPolicy > 0 || (index1 < count && PkixCertPathValidatorUtilities.IsSelfIssued( x509Certificate )))
            {
                foreach (Asn1Encodable asn1Encodable in instance1)
                {
                    PolicyInformation instance3 = PolicyInformation.GetInstance( asn1Encodable.ToAsn1Object() );
                    if (ANY_POLICY.Equals( instance3.PolicyIdentifier.Id ))
                    {
                        ISet qualifierSet = PkixCertPathValidatorUtilities.GetQualifierSet( instance3.PolicyQualifiers );
                        IList policyNode = policyNodes[index1 - 1];
                        for (int index2 = 0; index2 < policyNode.Count; ++index2)
                        {
                            PkixPolicyNode parent = (PkixPolicyNode)policyNode[index2];
                            foreach (object expectedPolicy in (IEnumerable)parent.ExpectedPolicies)
                            {
                                string str;
                                if (expectedPolicy is string)
                                    str = (string)expectedPolicy;
                                else if (expectedPolicy is DerObjectIdentifier)
                                    str = ((DerObjectIdentifier)expectedPolicy).Id;
                                else
                                    continue;
                                bool flag = false;
                                foreach (PkixPolicyNode child in parent.Children)
                                {
                                    if (str.Equals( child.ValidPolicy ))
                                        flag = true;
                                }
                                if (!flag)
                                {
                                    ISet expectedPolicies = new HashSet();
                                    expectedPolicies.Add( str );
                                    PkixPolicyNode child = new PkixPolicyNode( Platform.CreateArrayList(), index1, expectedPolicies, parent, qualifierSet, str, false );
                                    parent.AddChild( child );
                                    policyNodes[index1].Add( child );
                                }
                            }
                        }
                        break;
                    }
                }
            }
            PkixPolicyNode validPolicyTree1 = validPolicyTree;
            for (int index3 = index1 - 1; index3 >= 0; --index3)
            {
                IList policyNode = policyNodes[index3];
                for (int index4 = 0; index4 < policyNode.Count; ++index4)
                {
                    PkixPolicyNode _node = (PkixPolicyNode)policyNode[index4];
                    if (!_node.HasChildren)
                    {
                        validPolicyTree1 = PkixCertPathValidatorUtilities.RemovePolicyNode( validPolicyTree1, policyNodes, _node );
                        if (validPolicyTree1 == null)
                            break;
                    }
                }
            }
            ISet criticalExtensionOids = x509Certificate.GetCriticalExtensionOids();
            if (criticalExtensionOids != null)
            {
                bool flag = criticalExtensionOids.Contains( X509Extensions.CertificatePolicies.Id );
                IList policyNode = policyNodes[index1];
                for (int index5 = 0; index5 < policyNode.Count; ++index5)
                    ((PkixPolicyNode)policyNode[index5]).IsCritical = flag;
            }
            return validPolicyTree1;
        }

        internal static void ProcessCrlB1( DistributionPoint dp, object cert, X509Crl crl )
        {
            Asn1Object extensionValue = PkixCertPathValidatorUtilities.GetExtensionValue( crl, X509Extensions.IssuingDistributionPoint );
            bool flag1 = false;
            if (extensionValue != null && IssuingDistributionPoint.GetInstance( extensionValue ).IsIndirectCrl)
                flag1 = true;
            byte[] encoded = crl.IssuerDN.GetEncoded();
            bool flag2 = false;
            if (dp.CrlIssuer != null)
            {
                GeneralName[] names = dp.CrlIssuer.GetNames();
                for (int index = 0; index < names.Length; ++index)
                {
                    if (names[index].TagNo == 4)
                    {
                        try
                        {
                            if (Arrays.AreEqual( names[index].Name.ToAsn1Object().GetEncoded(), encoded ))
                                flag2 = true;
                        }
                        catch (IOException ex)
                        {
                            throw new Exception( "CRL issuer information from distribution point cannot be decoded.", ex );
                        }
                    }
                }
                if (flag2 && !flag1)
                    throw new Exception( "Distribution point contains cRLIssuer field but CRL is not indirect." );
                if (!flag2)
                    throw new Exception( "CRL issuer of CRL does not match CRL issuer of distribution point." );
            }
            else if (crl.IssuerDN.Equivalent( PkixCertPathValidatorUtilities.GetIssuerPrincipal( cert ), true ))
                flag2 = true;
            if (!flag2)
                throw new Exception( "Cannot find matching CRL issuer for certificate." );
        }

        internal static ReasonsMask ProcessCrlD( X509Crl crl, DistributionPoint dp )
        {
            IssuingDistributionPoint instance;
            try
            {
                instance = IssuingDistributionPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( crl, X509Extensions.IssuingDistributionPoint ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "issuing distribution point extension could not be decoded.", ex );
            }
            if (instance != null && instance.OnlySomeReasons != null && dp.Reasons != null)
                return new ReasonsMask( dp.Reasons.IntValue ).Intersect( new ReasonsMask( instance.OnlySomeReasons.IntValue ) );
            if ((instance == null || instance.OnlySomeReasons == null) && dp.Reasons == null)
                return ReasonsMask.AllReasons;
            return (dp.Reasons != null ? new ReasonsMask( dp.Reasons.IntValue ) : ReasonsMask.AllReasons).Intersect( instance != null ? new ReasonsMask( instance.OnlySomeReasons.IntValue ) : ReasonsMask.AllReasons );
        }

        internal static ISet ProcessCrlF(
          X509Crl crl,
          object cert,
          X509Certificate defaultCRLSignCert,
          AsymmetricKeyParameter defaultCRLSignKey,
          PkixParameters paramsPKIX,
          IList certPathCerts )
        {
            X509CertStoreSelector certSelect = new X509CertStoreSelector();
            try
            {
                certSelect.Subject = crl.IssuerDN;
            }
            catch (IOException ex)
            {
                throw new Exception( "Subject criteria for certificate selector to find issuer certificate for CRL could not be set.", ex );
            }
            IList arrayList1 = Platform.CreateArrayList();
            try
            {
                CollectionUtilities.AddRange( arrayList1, PkixCertPathValidatorUtilities.FindCertificates( certSelect, paramsPKIX.GetStores() ) );
                CollectionUtilities.AddRange( arrayList1, PkixCertPathValidatorUtilities.FindCertificates( certSelect, paramsPKIX.GetAdditionalStores() ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "Issuer certificate for CRL cannot be searched.", ex );
            }
            arrayList1.Add( defaultCRLSignCert );
            IEnumerator enumerator = arrayList1.GetEnumerator();
            IList arrayList2 = Platform.CreateArrayList();
            IList arrayList3 = Platform.CreateArrayList();
            while (enumerator.MoveNext())
            {
                X509Certificate current = (X509Certificate)enumerator.Current;
                if (current.Equals( defaultCRLSignCert ))
                {
                    arrayList2.Add( current );
                    arrayList3.Add( defaultCRLSignKey );
                }
                else
                {
                    try
                    {
                        PkixCertPathBuilder pkixCertPathBuilder = new PkixCertPathBuilder();
                        X509CertStoreSelector selector = new X509CertStoreSelector
                        {
                            Certificate = current
                        };
                        PkixParameters pkixParams = (PkixParameters)paramsPKIX.Clone();
                        pkixParams.SetTargetCertConstraints( selector );
                        PkixBuilderParameters instance = PkixBuilderParameters.GetInstance( pkixParams );
                        if (certPathCerts.Contains( current ))
                            instance.IsRevocationEnabled = false;
                        else
                            instance.IsRevocationEnabled = true;
                        IList certificates = pkixCertPathBuilder.Build( instance ).CertPath.Certificates;
                        arrayList2.Add( current );
                        arrayList3.Add( PkixCertPathValidatorUtilities.GetNextWorkingKey( certificates, 0 ) );
                    }
                    catch (PkixCertPathBuilderException ex)
                    {
                        throw new Exception( "Internal error.", ex );
                    }
                    catch (PkixCertPathValidatorException ex)
                    {
                        throw new Exception( "Public key of issuer certificate of CRL could not be retrieved.", ex );
                    }
                }
            }
            ISet set = new HashSet();
            Exception exception = null;
            for (int index = 0; index < arrayList2.Count; ++index)
            {
                bool[] keyUsage = ((X509Certificate)arrayList2[index]).GetKeyUsage();
                if (keyUsage != null && (keyUsage.Length < 7 || !keyUsage[CRL_SIGN]))
                    exception = new Exception( "Issuer certificate key usage extension does not permit CRL signing." );
                else
                    set.Add( arrayList3[index] );
            }
            if (set.Count == 0 && exception == null)
                throw new Exception( "Cannot find a valid issuer certificate." );
            if (set.Count == 0 && exception != null)
                throw exception;
            return set;
        }

        internal static AsymmetricKeyParameter ProcessCrlG( X509Crl crl, ISet keys )
        {
            Exception innerException = null;
            foreach (AsymmetricKeyParameter key in (IEnumerable)keys)
            {
                try
                {
                    crl.Verify( key );
                    return key;
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }
            throw new Exception( "Cannot verify CRL.", innerException );
        }

        internal static X509Crl ProcessCrlH( ISet deltaCrls, AsymmetricKeyParameter key )
        {
            Exception innerException = null;
            foreach (X509Crl deltaCrl in (IEnumerable)deltaCrls)
            {
                try
                {
                    deltaCrl.Verify( key );
                    return deltaCrl;
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }
            if (innerException != null)
                throw new Exception( "Cannot verify delta CRL.", innerException );
            return null;
        }

        private static void CheckCrl(
          DistributionPoint dp,
          PkixParameters paramsPKIX,
          X509Certificate cert,
          DateTime validDate,
          X509Certificate defaultCRLSignCert,
          AsymmetricKeyParameter defaultCRLSignKey,
          CertStatus certStatus,
          ReasonsMask reasonMask,
          IList certPathCerts )
        {
            DateTime utcNow = DateTime.UtcNow;
            if (validDate.Ticks > utcNow.Ticks)
                throw new Exception( "Validation time is in future." );
            ISet completeCrls = PkixCertPathValidatorUtilities.GetCompleteCrls( dp, cert, utcNow, paramsPKIX );
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
                        ReasonsMask mask = ProcessCrlD( current, dp );
                        if (mask.HasNewReasons( reasonMask ))
                        {
                            ISet keys = ProcessCrlF( current, cert, defaultCRLSignCert, defaultCRLSignKey, paramsPKIX, certPathCerts );
                            AsymmetricKeyParameter key = ProcessCrlG( current, keys );
                            X509Crl x509Crl = null;
                            if (paramsPKIX.IsUseDeltasEnabled)
                                x509Crl = ProcessCrlH( PkixCertPathValidatorUtilities.GetDeltaCrls( utcNow, paramsPKIX, current ), key );
                            if (paramsPKIX.ValidityModel != 1 && cert.NotAfter.Ticks < current.ThisUpdate.Ticks)
                                throw new Exception( "No valid CRL for current time found." );
                            ProcessCrlB1( dp, cert, current );
                            ProcessCrlB2( dp, cert, current );
                            ProcessCrlC( x509Crl, current, paramsPKIX );
                            ProcessCrlI( validDate, x509Crl, cert, certStatus, paramsPKIX );
                            ProcessCrlJ( validDate, current, cert, certStatus );
                            if (certStatus.Status == 8)
                                certStatus.Status = 11;
                            reasonMask.AddReasons( mask );
                            ISet criticalExtensionOids1 = current.GetCriticalExtensionOids();
                            if (criticalExtensionOids1 != null)
                            {
                                ISet set = new HashSet( criticalExtensionOids1 );
                                set.Remove( X509Extensions.IssuingDistributionPoint.Id );
                                set.Remove( X509Extensions.DeltaCrlIndicator.Id );
                                if (!set.IsEmpty)
                                    throw new Exception( "CRL contains unsupported critical extensions." );
                            }
                            if (x509Crl != null)
                            {
                                ISet criticalExtensionOids2 = x509Crl.GetCriticalExtensionOids();
                                if (criticalExtensionOids2 != null)
                                {
                                    ISet set = new HashSet( criticalExtensionOids2 );
                                    set.Remove( X509Extensions.IssuingDistributionPoint.Id );
                                    set.Remove( X509Extensions.DeltaCrlIndicator.Id );
                                    if (!set.IsEmpty)
                                        throw new Exception( "Delta CRL contains unsupported critical extension." );
                                }
                            }
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

        protected static void CheckCrls(
          PkixParameters paramsPKIX,
          X509Certificate cert,
          DateTime validDate,
          X509Certificate sign,
          AsymmetricKeyParameter workingPublicKey,
          IList certPathCerts )
        {
            Exception exception = null;
            CrlDistPoint instance;
            try
            {
                instance = CrlDistPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( cert, X509Extensions.CrlDistributionPoints ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "CRL distribution point extension could not be read.", ex );
            }
            try
            {
                PkixCertPathValidatorUtilities.AddAdditionalStoresFromCrlDistributionPoint( instance, paramsPKIX );
            }
            catch (Exception ex)
            {
                throw new Exception( "No additional CRL locations could be decoded from CRL distribution point extension.", ex );
            }
            CertStatus certStatus = new CertStatus();
            ReasonsMask reasonMask = new ReasonsMask();
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
                    throw new Exception( "Distribution points could not be read.", ex );
                }
                if (distributionPoints != null)
                {
                    for (int index = 0; index < distributionPoints.Length && certStatus.Status == 11 && !reasonMask.IsAllReasons; ++index)
                    {
                        PkixParameters paramsPKIX1 = (PkixParameters)paramsPKIX.Clone();
                        try
                        {
                            CheckCrl( distributionPoints[index], paramsPKIX1, cert, validDate, sign, workingPublicKey, certStatus, reasonMask, certPathCerts );
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                    }
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
                            name = new Asn1InputStream( cert.IssuerDN.GetEncoded() ).ReadObject();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception( "Issuer from certificate for CRL could not be reencoded.", ex );
                        }
                        CheckCrl( new DistributionPoint( new DistributionPointName( 0, new GeneralNames( new GeneralName( 4, name ) ) ), null, null ), (PkixParameters)paramsPKIX.Clone(), cert, validDate, sign, workingPublicKey, certStatus, reasonMask, certPathCerts );
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }
            }
            if (!flag)
                throw exception;
            if (certStatus.Status != 11)
                throw new Exception( "Certificate revocation after " + certStatus.RevocationDate.Value.ToString( "ddd MMM dd HH:mm:ss K yyyy" ) + ", reason: " + CrlReasons[certStatus.Status] );
            if (!reasonMask.IsAllReasons && certStatus.Status == 11)
                certStatus.Status = 12;
            if (certStatus.Status == 12)
                throw new Exception( "Certificate status could not be determined." );
        }

        internal static PkixPolicyNode PrepareCertB(
          PkixCertPath certPath,
          int index,
          IList[] policyNodes,
          PkixPolicyNode validPolicyTree,
          int policyMapping )
        {
            IList certificates = certPath.Certificates;
            X509Certificate ext = (X509Certificate)certificates[index];
            int depth = certificates.Count - index;
            Asn1Sequence instance1;
            try
            {
                instance1 = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( ext, X509Extensions.PolicyMappings ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Policy mappings extension could not be decoded.", ex, certPath, index );
            }
            PkixPolicyNode validPolicyTree1 = validPolicyTree;
            if (instance1 != null)
            {
                Asn1Sequence asn1Sequence1 = instance1;
                IDictionary hashtable = Platform.CreateHashtable();
                ISet set1 = new HashSet();
                for (int index1 = 0; index1 < asn1Sequence1.Count; ++index1)
                {
                    Asn1Sequence asn1Sequence2 = (Asn1Sequence)asn1Sequence1[index1];
                    string id1 = ((DerObjectIdentifier)asn1Sequence2[0]).Id;
                    string id2 = ((DerObjectIdentifier)asn1Sequence2[1]).Id;
                    if (!hashtable.Contains( id1 ))
                    {
                        ISet set2 = new HashSet();
                        set2.Add( id2 );
                        hashtable[id1] = set2;
                        set1.Add( id1 );
                    }
                    else
                        ((ISet)hashtable[id1]).Add( id2 );
                }
                foreach (string str in (IEnumerable)set1)
                {
                    if (policyMapping > 0)
                    {
                        bool flag = false;
                        foreach (PkixPolicyNode pkixPolicyNode in (IEnumerable)policyNodes[depth])
                        {
                            if (pkixPolicyNode.ValidPolicy.Equals( str ))
                            {
                                flag = true;
                                pkixPolicyNode.ExpectedPolicies = (ISet)hashtable[str];
                                break;
                            }
                        }
                        if (!flag)
                        {
                            foreach (PkixPolicyNode pkixPolicyNode in (IEnumerable)policyNodes[depth])
                            {
                                if (ANY_POLICY.Equals( pkixPolicyNode.ValidPolicy ))
                                {
                                    ISet policyQualifiers = null;
                                    Asn1Sequence extensionValue;
                                    try
                                    {
                                        extensionValue = (Asn1Sequence)PkixCertPathValidatorUtilities.GetExtensionValue( ext, X509Extensions.CertificatePolicies );
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new PkixCertPathValidatorException( "Certificate policies extension could not be decoded.", ex, certPath, index );
                                    }
                                    foreach (Asn1Encodable asn1Encodable in extensionValue)
                                    {
                                        PolicyInformation instance2;
                                        try
                                        {
                                            instance2 = PolicyInformation.GetInstance( asn1Encodable.ToAsn1Object() );
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new PkixCertPathValidatorException( "Policy information could not be decoded.", ex, certPath, index );
                                        }
                                        if (ANY_POLICY.Equals( instance2.PolicyIdentifier.Id ))
                                        {
                                            try
                                            {
                                                policyQualifiers = PkixCertPathValidatorUtilities.GetQualifierSet( instance2.PolicyQualifiers );
                                                break;
                                            }
                                            catch (PkixCertPathValidatorException ex)
                                            {
                                                throw new PkixCertPathValidatorException( "Policy qualifier info set could not be decoded.", ex, certPath, index );
                                            }
                                        }
                                    }
                                    bool critical = false;
                                    ISet criticalExtensionOids = ext.GetCriticalExtensionOids();
                                    if (criticalExtensionOids != null)
                                        critical = criticalExtensionOids.Contains( X509Extensions.CertificatePolicies.Id );
                                    PkixPolicyNode parent = pkixPolicyNode.Parent;
                                    if (ANY_POLICY.Equals( parent.ValidPolicy ))
                                    {
                                        PkixPolicyNode child = new PkixPolicyNode( Platform.CreateArrayList(), depth, (ISet)hashtable[str], parent, policyQualifiers, str, critical );
                                        parent.AddChild( child );
                                        policyNodes[depth].Add( child );
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else if (policyMapping <= 0)
                    {
                        foreach (PkixPolicyNode array1 in (IEnumerable)Platform.CreateArrayList( policyNodes[depth] ))
                        {
                            if (array1.ValidPolicy.Equals( str ))
                            {
                                array1.Parent.RemoveChild( array1 );
                                for (int index2 = depth - 1; index2 >= 0; --index2)
                                {
                                    foreach (PkixPolicyNode array2 in (IEnumerable)Platform.CreateArrayList( policyNodes[index2] ))
                                    {
                                        if (!array2.HasChildren)
                                        {
                                            validPolicyTree1 = PkixCertPathValidatorUtilities.RemovePolicyNode( validPolicyTree1, policyNodes, array2 );
                                            if (validPolicyTree1 == null)
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return validPolicyTree1;
        }

        internal static ISet[] ProcessCrlA1ii(
          DateTime currentDate,
          PkixParameters paramsPKIX,
          X509Certificate cert,
          X509Crl crl )
        {
            ISet set = new HashSet();
            X509CrlStoreSelector crlselect = new X509CrlStoreSelector
            {
                CertificateChecking = cert
            };
            try
            {
                IList arrayList = Platform.CreateArrayList();
                arrayList.Add( crl.IssuerDN );
                crlselect.Issuers = arrayList;
            }
            catch (IOException ex)
            {
                throw new Exception( "Cannot extract issuer from CRL." + ex, ex );
            }
            crlselect.CompleteCrlEnabled = true;
            ISet crls = CrlUtilities.FindCrls( crlselect, paramsPKIX, currentDate );
            if (paramsPKIX.IsUseDeltasEnabled)
            {
                try
                {
                    set.AddAll( PkixCertPathValidatorUtilities.GetDeltaCrls( currentDate, paramsPKIX, crl ) );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Exception obtaining delta CRLs.", ex );
                }
            }
            return new ISet[2] { crls, set };
        }

        internal static ISet ProcessCrlA1i(
          DateTime currentDate,
          PkixParameters paramsPKIX,
          X509Certificate cert,
          X509Crl crl )
        {
            ISet set = new HashSet();
            if (paramsPKIX.IsUseDeltasEnabled)
            {
                CrlDistPoint instance;
                try
                {
                    instance = CrlDistPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( cert, X509Extensions.FreshestCrl ) );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Freshest CRL extension could not be decoded from certificate.", ex );
                }
                if (instance == null)
                {
                    try
                    {
                        instance = CrlDistPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( crl, X509Extensions.FreshestCrl ) );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( "Freshest CRL extension could not be decoded from CRL.", ex );
                    }
                }
                if (instance != null)
                {
                    try
                    {
                        PkixCertPathValidatorUtilities.AddAdditionalStoresFromCrlDistributionPoint( instance, paramsPKIX );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( "No new delta CRL locations could be added from Freshest CRL extension.", ex );
                    }
                    try
                    {
                        set.AddAll( PkixCertPathValidatorUtilities.GetDeltaCrls( currentDate, paramsPKIX, crl ) );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( "Exception obtaining delta CRLs.", ex );
                    }
                }
            }
            return set;
        }

        internal static void ProcessCertF(
          PkixCertPath certPath,
          int index,
          PkixPolicyNode validPolicyTree,
          int explicitPolicy )
        {
            if (explicitPolicy <= 0 && validPolicyTree == null)
                throw new PkixCertPathValidatorException( "No valid policy tree found when one expected.", null, certPath, index );
        }

        internal static void ProcessCertA(
          PkixCertPath certPath,
          PkixParameters paramsPKIX,
          int index,
          AsymmetricKeyParameter workingPublicKey,
          X509Name workingIssuerName,
          X509Certificate sign )
        {
            IList certificates = certPath.Certificates;
            X509Certificate cert = (X509Certificate)certificates[index];
            try
            {
                cert.Verify( workingPublicKey );
            }
            catch (GeneralSecurityException ex)
            {
                throw new PkixCertPathValidatorException( "Could not validate certificate signature.", ex, certPath, index );
            }
            try
            {
                cert.CheckValidity( PkixCertPathValidatorUtilities.GetValidCertDateFromValidityModel( paramsPKIX, certPath, index ) );
            }
            catch (CertificateExpiredException ex)
            {
                throw new PkixCertPathValidatorException( "Could not validate certificate: " + ex.Message, ex, certPath, index );
            }
            catch (CertificateNotYetValidException ex)
            {
                throw new PkixCertPathValidatorException( "Could not validate certificate: " + ex.Message, ex, certPath, index );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Could not validate time of certificate.", ex, certPath, index );
            }
            if (paramsPKIX.IsRevocationEnabled)
            {
                try
                {
                    CheckCrls( paramsPKIX, cert, PkixCertPathValidatorUtilities.GetValidCertDateFromValidityModel( paramsPKIX, certPath, index ), sign, workingPublicKey, certificates );
                }
                catch (Exception ex)
                {
                    Exception cause = ex.InnerException ?? ex;
                    throw new PkixCertPathValidatorException( ex.Message, cause, certPath, index );
                }
            }
            X509Name issuerPrincipal = PkixCertPathValidatorUtilities.GetIssuerPrincipal( cert );
            if (!issuerPrincipal.Equivalent( workingIssuerName, true ))
                throw new PkixCertPathValidatorException( "IssuerName(" + issuerPrincipal + ") does not match SubjectName(" + workingIssuerName + ") of signing certificate.", null, certPath, index );
        }

        internal static int PrepareNextCertI1( PkixCertPath certPath, int index, int explicitPolicy )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            Asn1Sequence instance1;
            try
            {
                instance1 = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.PolicyConstraints ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Policy constraints extension cannot be decoded.", ex, certPath, index );
            }
            if (instance1 != null)
            {
                foreach (object obj in instance1)
                {
                    try
                    {
                        Asn1TaggedObject instance2 = Asn1TaggedObject.GetInstance( obj );
                        if (instance2.TagNo == 0)
                        {
                            int intValue = DerInteger.GetInstance( instance2, false ).Value.IntValue;
                            if (intValue < explicitPolicy)
                                return intValue;
                            break;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        throw new PkixCertPathValidatorException( "Policy constraints extension contents cannot be decoded.", ex, certPath, index );
                    }
                }
            }
            return explicitPolicy;
        }

        internal static int PrepareNextCertI2( PkixCertPath certPath, int index, int policyMapping )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            Asn1Sequence instance1;
            try
            {
                instance1 = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.PolicyConstraints ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Policy constraints extension cannot be decoded.", ex, certPath, index );
            }
            if (instance1 != null)
            {
                foreach (object obj in instance1)
                {
                    try
                    {
                        Asn1TaggedObject instance2 = Asn1TaggedObject.GetInstance( obj );
                        if (instance2.TagNo == 1)
                        {
                            int intValue = DerInteger.GetInstance( instance2, false ).Value.IntValue;
                            if (intValue < policyMapping)
                                return intValue;
                            break;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        throw new PkixCertPathValidatorException( "Policy constraints extension contents cannot be decoded.", ex, certPath, index );
                    }
                }
            }
            return policyMapping;
        }

        internal static void PrepareNextCertG(
          PkixCertPath certPath,
          int index,
          PkixNameConstraintValidator nameConstraintValidator )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            NameConstraints nameConstraints = null;
            try
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.NameConstraints ) );
                if (instance != null)
                    nameConstraints = new NameConstraints( instance );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Name constraints extension could not be decoded.", ex, certPath, index );
            }
            if (nameConstraints == null)
                return;
            Asn1Sequence permittedSubtrees = nameConstraints.PermittedSubtrees;
            if (permittedSubtrees != null)
            {
                try
                {
                    nameConstraintValidator.IntersectPermittedSubtree( permittedSubtrees );
                }
                catch (Exception ex)
                {
                    throw new PkixCertPathValidatorException( "Permitted subtrees cannot be build from name constraints extension.", ex, certPath, index );
                }
            }
            Asn1Sequence excludedSubtrees = nameConstraints.ExcludedSubtrees;
            if (excludedSubtrees == null)
                return;
            IEnumerator enumerator = excludedSubtrees.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    GeneralSubtree instance = GeneralSubtree.GetInstance( enumerator.Current );
                    nameConstraintValidator.AddExcludedSubtree( instance );
                }
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Excluded subtrees cannot be build from name constraints extension.", ex, certPath, index );
            }
        }

        internal static int PrepareNextCertJ( PkixCertPath certPath, int index, int inhibitAnyPolicy )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            DerInteger instance;
            try
            {
                instance = DerInteger.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.InhibitAnyPolicy ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Inhibit any-policy extension cannot be decoded.", ex, certPath, index );
            }
            if (instance != null)
            {
                int intValue = instance.Value.IntValue;
                if (intValue < inhibitAnyPolicy)
                    return intValue;
            }
            return inhibitAnyPolicy;
        }

        internal static void PrepareNextCertK( PkixCertPath certPath, int index )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            BasicConstraints instance;
            try
            {
                instance = BasicConstraints.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.BasicConstraints ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Basic constraints extension cannot be decoded.", ex, certPath, index );
            }
            if (instance == null)
                throw new PkixCertPathValidatorException( "Intermediate certificate lacks BasicConstraints" );
            if (!instance.IsCA())
                throw new PkixCertPathValidatorException( "Not a CA certificate" );
        }

        internal static int PrepareNextCertL( PkixCertPath certPath, int index, int maxPathLength )
        {
            if (PkixCertPathValidatorUtilities.IsSelfIssued( (X509Certificate)certPath.Certificates[index] ))
                return maxPathLength;
            if (maxPathLength <= 0)
                throw new PkixCertPathValidatorException( "Max path length not greater than zero", null, certPath, index );
            return maxPathLength - 1;
        }

        internal static int PrepareNextCertM( PkixCertPath certPath, int index, int maxPathLength )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            BasicConstraints instance;
            try
            {
                instance = BasicConstraints.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.BasicConstraints ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Basic constraints extension cannot be decoded.", ex, certPath, index );
            }
            if (instance != null)
            {
                BigInteger pathLenConstraint = instance.PathLenConstraint;
                if (pathLenConstraint != null)
                {
                    int intValue = pathLenConstraint.IntValue;
                    if (intValue < maxPathLength)
                        return intValue;
                }
            }
            return maxPathLength;
        }

        internal static void PrepareNextCertN( PkixCertPath certPath, int index )
        {
            bool[] keyUsage = ((X509Certificate)certPath.Certificates[index]).GetKeyUsage();
            if (keyUsage != null && !keyUsage[KEY_CERT_SIGN])
                throw new PkixCertPathValidatorException( "Issuer certificate keyusage extension is critical and does not permit key signing.", null, certPath, index );
        }

        internal static void PrepareNextCertO(
          PkixCertPath certPath,
          int index,
          ISet criticalExtensions,
          IList pathCheckers )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            foreach (PkixCertPathChecker pathChecker in (IEnumerable)pathCheckers)
            {
                try
                {
                    pathChecker.Check( certificate, criticalExtensions );
                }
                catch (PkixCertPathValidatorException ex)
                {
                    throw new PkixCertPathValidatorException( ex.Message, ex.InnerException, certPath, index );
                }
            }
            if (!criticalExtensions.IsEmpty)
                throw new PkixCertPathValidatorException( "Certificate has unsupported critical extension.", null, certPath, index );
        }

        internal static int PrepareNextCertH1( PkixCertPath certPath, int index, int explicitPolicy ) => !PkixCertPathValidatorUtilities.IsSelfIssued( (X509Certificate)certPath.Certificates[index] ) && explicitPolicy != 0 ? explicitPolicy - 1 : explicitPolicy;

        internal static int PrepareNextCertH2( PkixCertPath certPath, int index, int policyMapping ) => !PkixCertPathValidatorUtilities.IsSelfIssued( (X509Certificate)certPath.Certificates[index] ) && policyMapping != 0 ? policyMapping - 1 : policyMapping;

        internal static int PrepareNextCertH3( PkixCertPath certPath, int index, int inhibitAnyPolicy ) => !PkixCertPathValidatorUtilities.IsSelfIssued( (X509Certificate)certPath.Certificates[index] ) && inhibitAnyPolicy != 0 ? inhibitAnyPolicy - 1 : inhibitAnyPolicy;

        internal static int WrapupCertA( int explicitPolicy, X509Certificate cert )
        {
            if (!PkixCertPathValidatorUtilities.IsSelfIssued( cert ) && explicitPolicy != 0)
                --explicitPolicy;
            return explicitPolicy;
        }

        internal static int WrapupCertB( PkixCertPath certPath, int index, int explicitPolicy )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            Asn1Sequence instance;
            try
            {
                instance = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.PolicyConstraints ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Policy constraints could not be decoded.", ex, certPath, index );
            }
            if (instance != null)
            {
                foreach (Asn1TaggedObject asn1TaggedObject in instance)
                {
                    if (asn1TaggedObject.TagNo == 0)
                    {
                        int intValue;
                        try
                        {
                            intValue = DerInteger.GetInstance( asn1TaggedObject, false ).Value.IntValue;
                        }
                        catch (Exception ex)
                        {
                            throw new PkixCertPathValidatorException( "Policy constraints requireExplicitPolicy field could not be decoded.", ex, certPath, index );
                        }
                        if (intValue == 0)
                            return 0;
                    }
                }
            }
            return explicitPolicy;
        }

        internal static void WrapupCertF(
          PkixCertPath certPath,
          int index,
          IList pathCheckers,
          ISet criticalExtensions )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            foreach (PkixCertPathChecker pathChecker in (IEnumerable)pathCheckers)
            {
                try
                {
                    pathChecker.Check( certificate, criticalExtensions );
                }
                catch (PkixCertPathValidatorException ex)
                {
                    throw new PkixCertPathValidatorException( "Additional certificate path checker failed.", ex, certPath, index );
                }
            }
            if (!criticalExtensions.IsEmpty)
                throw new PkixCertPathValidatorException( "Certificate has unsupported critical extension", null, certPath, index );
        }

        internal static PkixPolicyNode WrapupCertG(
          PkixCertPath certPath,
          PkixParameters paramsPKIX,
          ISet userInitialPolicySet,
          int index,
          IList[] policyNodes,
          PkixPolicyNode validPolicyTree,
          ISet acceptablePolicies )
        {
            int count = certPath.Certificates.Count;
            PkixPolicyNode pkixPolicyNode1;
            if (validPolicyTree == null)
            {
                if (paramsPKIX.IsExplicitPolicyRequired)
                    throw new PkixCertPathValidatorException( "Explicit policy requested but none available.", null, certPath, index );
                pkixPolicyNode1 = null;
            }
            else if (PkixCertPathValidatorUtilities.IsAnyPolicy( userInitialPolicySet ))
            {
                if (paramsPKIX.IsExplicitPolicyRequired)
                {
                    if (acceptablePolicies.IsEmpty)
                        throw new PkixCertPathValidatorException( "Explicit policy requested but none available.", null, certPath, index );
                    ISet set = new HashSet();
                    for (int index1 = 0; index1 < policyNodes.Length; ++index1)
                    {
                        IList policyNode = policyNodes[index1];
                        for (int index2 = 0; index2 < policyNode.Count; ++index2)
                        {
                            PkixPolicyNode pkixPolicyNode2 = (PkixPolicyNode)policyNode[index2];
                            if (ANY_POLICY.Equals( pkixPolicyNode2.ValidPolicy ))
                            {
                                foreach (object child in pkixPolicyNode2.Children)
                                    set.Add( child );
                            }
                        }
                    }
                    foreach (PkixPolicyNode pkixPolicyNode3 in (IEnumerable)set)
                    {
                        string validPolicy = pkixPolicyNode3.ValidPolicy;
                        acceptablePolicies.Contains( validPolicy );
                    }
                    if (validPolicyTree != null)
                    {
                        for (int index3 = count - 1; index3 >= 0; --index3)
                        {
                            IList policyNode = policyNodes[index3];
                            for (int index4 = 0; index4 < policyNode.Count; ++index4)
                            {
                                PkixPolicyNode _node = (PkixPolicyNode)policyNode[index4];
                                if (!_node.HasChildren)
                                    validPolicyTree = PkixCertPathValidatorUtilities.RemovePolicyNode( validPolicyTree, policyNodes, _node );
                            }
                        }
                    }
                }
                pkixPolicyNode1 = validPolicyTree;
            }
            else
            {
                ISet set = new HashSet();
                for (int index5 = 0; index5 < policyNodes.Length; ++index5)
                {
                    IList policyNode = policyNodes[index5];
                    for (int index6 = 0; index6 < policyNode.Count; ++index6)
                    {
                        PkixPolicyNode pkixPolicyNode4 = (PkixPolicyNode)policyNode[index6];
                        if (ANY_POLICY.Equals( pkixPolicyNode4.ValidPolicy ))
                        {
                            foreach (PkixPolicyNode child in pkixPolicyNode4.Children)
                            {
                                if (!ANY_POLICY.Equals( child.ValidPolicy ))
                                    set.Add( child );
                            }
                        }
                    }
                }
                foreach (PkixPolicyNode _node in (IEnumerable)set)
                {
                    string validPolicy = _node.ValidPolicy;
                    if (!userInitialPolicySet.Contains( validPolicy ))
                        validPolicyTree = PkixCertPathValidatorUtilities.RemovePolicyNode( validPolicyTree, policyNodes, _node );
                }
                if (validPolicyTree != null)
                {
                    for (int index7 = count - 1; index7 >= 0; --index7)
                    {
                        IList policyNode = policyNodes[index7];
                        for (int index8 = 0; index8 < policyNode.Count; ++index8)
                        {
                            PkixPolicyNode _node = (PkixPolicyNode)policyNode[index8];
                            if (!_node.HasChildren)
                                validPolicyTree = PkixCertPathValidatorUtilities.RemovePolicyNode( validPolicyTree, policyNodes, _node );
                        }
                    }
                }
                pkixPolicyNode1 = validPolicyTree;
            }
            return pkixPolicyNode1;
        }

        internal static void ProcessCrlC(
          X509Crl deltaCRL,
          X509Crl completeCRL,
          PkixParameters pkixParams )
        {
            if (deltaCRL == null)
                return;
            IssuingDistributionPoint instance1;
            try
            {
                instance1 = IssuingDistributionPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( completeCRL, X509Extensions.IssuingDistributionPoint ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "000 Issuing distribution point extension could not be decoded.", ex );
            }
            if (!pkixParams.IsUseDeltasEnabled)
                return;
            if (!deltaCRL.IssuerDN.Equivalent( completeCRL.IssuerDN, true ))
                throw new Exception( "Complete CRL issuer does not match delta CRL issuer." );
            IssuingDistributionPoint instance2;
            try
            {
                instance2 = IssuingDistributionPoint.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( deltaCRL, X509Extensions.IssuingDistributionPoint ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "Issuing distribution point extension from delta CRL could not be decoded.", ex );
            }
            if (!Equals( instance1, instance2 ))
                throw new Exception( "Issuing distribution point extension from delta CRL and complete CRL does not match." );
            Asn1Object extensionValue1;
            try
            {
                extensionValue1 = PkixCertPathValidatorUtilities.GetExtensionValue( completeCRL, X509Extensions.AuthorityKeyIdentifier );
            }
            catch (Exception ex)
            {
                throw new Exception( "Authority key identifier extension could not be extracted from complete CRL.", ex );
            }
            Asn1Object extensionValue2;
            try
            {
                extensionValue2 = PkixCertPathValidatorUtilities.GetExtensionValue( deltaCRL, X509Extensions.AuthorityKeyIdentifier );
            }
            catch (Exception ex)
            {
                throw new Exception( "Authority key identifier extension could not be extracted from delta CRL.", ex );
            }
            if (extensionValue1 == null)
                throw new Exception( "CRL authority key identifier is null." );
            if (extensionValue2 == null)
                throw new Exception( "Delta CRL authority key identifier is null." );
            if (!extensionValue1.Equals( extensionValue2 ))
                throw new Exception( "Delta CRL authority key identifier does not match complete CRL authority key identifier." );
        }

        internal static void ProcessCrlI(
          DateTime validDate,
          X509Crl deltacrl,
          object cert,
          CertStatus certStatus,
          PkixParameters pkixParams )
        {
            if (!pkixParams.IsUseDeltasEnabled || deltacrl == null)
                return;
            PkixCertPathValidatorUtilities.GetCertStatus( validDate, deltacrl, cert, certStatus );
        }

        internal static void ProcessCrlJ(
          DateTime validDate,
          X509Crl completecrl,
          object cert,
          CertStatus certStatus )
        {
            if (certStatus.Status != 11)
                return;
            PkixCertPathValidatorUtilities.GetCertStatus( validDate, completecrl, cert, certStatus );
        }

        internal static PkixPolicyNode ProcessCertE(
          PkixCertPath certPath,
          int index,
          PkixPolicyNode validPolicyTree )
        {
            X509Certificate certificate = (X509Certificate)certPath.Certificates[index];
            Asn1Sequence instance;
            try
            {
                instance = Asn1Sequence.GetInstance( PkixCertPathValidatorUtilities.GetExtensionValue( certificate, X509Extensions.CertificatePolicies ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Could not read certificate policies extension from certificate.", ex, certPath, index );
            }
            if (instance == null)
                validPolicyTree = null;
            return validPolicyTree;
        }
    }
}
