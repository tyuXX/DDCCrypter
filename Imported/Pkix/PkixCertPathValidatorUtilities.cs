// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathValidatorUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.IsisMtt;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using Org.BouncyCastle.X509.Store;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCertPathValidatorUtilities
    {
        private static readonly PkixCrlUtilities CrlUtilities = new();
        internal static readonly string ANY_POLICY = "2.5.29.32.0";
        internal static readonly string CRL_NUMBER = X509Extensions.CrlNumber.Id;
        internal static readonly int KEY_CERT_SIGN = 5;
        internal static readonly int CRL_SIGN = 6;
        internal static readonly string[] crlReasons = new string[11]
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

        internal static TrustAnchor FindTrustAnchor( X509Certificate cert, ISet trustAnchors )
        {
            IEnumerator enumerator = trustAnchors.GetEnumerator();
            TrustAnchor trustAnchor = null;
            AsymmetricKeyParameter key = null;
            Exception innerException = null;
            X509CertStoreSelector certStoreSelector = new();
            try
            {
                certStoreSelector.Subject = GetIssuerPrincipal( cert );
            }
            catch (IOException ex)
            {
                throw new Exception( "Cannot set subject search criteria for trust anchor.", ex );
            }
            while (enumerator.MoveNext() && trustAnchor == null)
            {
                trustAnchor = (TrustAnchor)enumerator.Current;
                if (trustAnchor.TrustedCert != null)
                {
                    if (certStoreSelector.Match( trustAnchor.TrustedCert ))
                        key = trustAnchor.TrustedCert.GetPublicKey();
                    else
                        trustAnchor = null;
                }
                else
                {
                    if (trustAnchor.CAName != null)
                    {
                        if (trustAnchor.CAPublicKey != null)
                        {
                            try
                            {
                                if (GetIssuerPrincipal( cert ).Equivalent( new X509Name( trustAnchor.CAName ), true ))
                                {
                                    key = trustAnchor.CAPublicKey;
                                    goto label_14;
                                }
                                else
                                {
                                    trustAnchor = null;
                                    goto label_14;
                                }
                            }
                            catch (InvalidParameterException ex)
                            {
                                trustAnchor = null;
                                goto label_14;
                            }
                        }
                    }
                    trustAnchor = null;
                }
            label_14:
                if (key != null)
                {
                    try
                    {
                        cert.Verify( key );
                    }
                    catch (Exception ex)
                    {
                        innerException = ex;
                        trustAnchor = null;
                    }
                }
            }
            return trustAnchor != null || innerException == null ? trustAnchor : throw new Exception( "TrustAnchor found but certificate validation failed.", innerException );
        }

        internal static void AddAdditionalStoresFromAltNames(
          X509Certificate cert,
          PkixParameters pkixParams )
        {
            if (cert.GetIssuerAlternativeNames() == null)
                return;
            foreach (IList issuerAlternativeName in (IEnumerable)cert.GetIssuerAlternativeNames())
            {
                if (issuerAlternativeName[0].Equals( 6 ))
                    AddAdditionalStoreFromLocation( (string)issuerAlternativeName[1], pkixParams );
            }
        }

        internal static DateTime GetValidDate( PkixParameters paramsPKIX )
        {
            DateTimeObject date = paramsPKIX.Date;
            return date == null ? DateTime.UtcNow : date.Value;
        }

        internal static X509Name GetIssuerPrincipal( object cert ) => cert is X509Certificate ? ((X509Certificate)cert).IssuerDN : ((IX509AttributeCertificate)cert).Issuer.GetPrincipals()[0];

        internal static bool IsSelfIssued( X509Certificate cert ) => cert.SubjectDN.Equivalent( cert.IssuerDN, true );

        internal static AlgorithmIdentifier GetAlgorithmIdentifier( AsymmetricKeyParameter key )
        {
            try
            {
                return SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( key ).AlgorithmID;
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Subject public key cannot be decoded.", ex );
            }
        }

        internal static bool IsAnyPolicy( ISet policySet ) => policySet == null || policySet.Contains( ANY_POLICY ) || policySet.Count == 0;

        internal static void AddAdditionalStoreFromLocation( string location, PkixParameters pkixParams )
        {
            if (!pkixParams.IsAdditionalLocationsEnabled)
                return;
            try
            {
                if (Platform.StartsWith( location, "ldap://" ))
                {
                    location = location.Substring( 7 );
                    int length = location.IndexOf( '/' );
                    if (length != -1)
                    {
                        string str1 = "ldap://" + location.Substring( 0, length );
                    }
                    else
                    {
                        string str2 = "ldap://" + location;
                    }
                    throw Platform.CreateNotImplementedException( "LDAP cert/CRL stores" );
                }
            }
            catch (Exception ex)
            {
                throw new Exception( "Exception adding X.509 stores." );
            }
        }

        private static BigInteger GetSerialNumber( object cert ) => cert is X509Certificate ? ((X509Certificate)cert).SerialNumber : ((X509V2AttributeCertificate)cert).SerialNumber;

        internal static ISet GetQualifierSet( Asn1Sequence qualifiers )
        {
            ISet qualifierSet = new HashSet();
            if (qualifiers == null)
                return qualifierSet;
            foreach (Asn1Encodable qualifier in qualifiers)
            {
                try
                {
                    qualifierSet.Add( PolicyQualifierInfo.GetInstance( qualifier.ToAsn1Object() ) );
                }
                catch (IOException ex)
                {
                    throw new PkixCertPathValidatorException( "Policy qualifier info cannot be decoded.", ex );
                }
            }
            return qualifierSet;
        }

        internal static PkixPolicyNode RemovePolicyNode(
          PkixPolicyNode validPolicyTree,
          IList[] policyNodes,
          PkixPolicyNode _node )
        {
            PkixPolicyNode parent = _node.Parent;
            if (validPolicyTree == null)
                return null;
            if (parent == null)
            {
                for (int index = 0; index < policyNodes.Length; ++index)
                    policyNodes[index] = Platform.CreateArrayList();
                return null;
            }
            parent.RemoveChild( _node );
            RemovePolicyNodeRecurse( policyNodes, _node );
            return validPolicyTree;
        }

        private static void RemovePolicyNodeRecurse( IList[] policyNodes, PkixPolicyNode _node )
        {
            policyNodes[_node.Depth].Remove( _node );
            if (!_node.HasChildren)
                return;
            foreach (PkixPolicyNode child in _node.Children)
                RemovePolicyNodeRecurse( policyNodes, child );
        }

        internal static void PrepareNextCertB1(
          int i,
          IList[] policyNodes,
          string id_p,
          IDictionary m_idp,
          X509Certificate cert )
        {
            bool flag = false;
            foreach (PkixPolicyNode pkixPolicyNode in (IEnumerable)policyNodes[i])
            {
                if (pkixPolicyNode.ValidPolicy.Equals( id_p ))
                {
                    flag = true;
                    pkixPolicyNode.ExpectedPolicies = (ISet)m_idp[id_p];
                    break;
                }
            }
            if (flag)
                return;
            foreach (PkixPolicyNode pkixPolicyNode in (IEnumerable)policyNodes[i])
            {
                if (ANY_POLICY.Equals( pkixPolicyNode.ValidPolicy ))
                {
                    ISet policyQualifiers = null;
                    Asn1Sequence instance1;
                    try
                    {
                        instance1 = Asn1Sequence.GetInstance( GetExtensionValue( cert, X509Extensions.CertificatePolicies ) );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( "Certificate policies cannot be decoded.", ex );
                    }
                    foreach (object obj in instance1)
                    {
                        PolicyInformation instance2;
                        try
                        {
                            instance2 = PolicyInformation.GetInstance( obj );
                        }
                        catch (Exception ex)
                        {
                            throw new Exception( "Policy information cannot be decoded.", ex );
                        }
                        if (ANY_POLICY.Equals( instance2.PolicyIdentifier.Id ))
                        {
                            try
                            {
                                policyQualifiers = GetQualifierSet( instance2.PolicyQualifiers );
                                break;
                            }
                            catch (PkixCertPathValidatorException ex)
                            {
                                throw new PkixCertPathValidatorException( "Policy qualifier info set could not be built.", ex );
                            }
                        }
                    }
                    bool critical = false;
                    ISet criticalExtensionOids = cert.GetCriticalExtensionOids();
                    if (criticalExtensionOids != null)
                        critical = criticalExtensionOids.Contains( X509Extensions.CertificatePolicies.Id );
                    PkixPolicyNode parent = pkixPolicyNode.Parent;
                    if (!ANY_POLICY.Equals( parent.ValidPolicy ))
                        break;
                    PkixPolicyNode child = new( Platform.CreateArrayList(), i, (ISet)m_idp[id_p], parent, policyQualifiers, id_p, critical );
                    parent.AddChild( child );
                    policyNodes[i].Add( child );
                    break;
                }
            }
        }

        internal static PkixPolicyNode PrepareNextCertB2(
          int i,
          IList[] policyNodes,
          string id_p,
          PkixPolicyNode validPolicyTree )
        {
            int index1 = 0;
            foreach (PkixPolicyNode array in (IEnumerable)Platform.CreateArrayList( policyNodes[i] ))
            {
                if (array.ValidPolicy.Equals( id_p ))
                {
                    array.Parent.RemoveChild( array );
                    policyNodes[i].RemoveAt( index1 );
                    for (int index2 = i - 1; index2 >= 0; --index2)
                    {
                        IList policyNode = policyNodes[index2];
                        for (int index3 = 0; index3 < policyNode.Count; ++index3)
                        {
                            PkixPolicyNode _node = (PkixPolicyNode)policyNode[index3];
                            if (!_node.HasChildren)
                            {
                                validPolicyTree = RemovePolicyNode( validPolicyTree, policyNodes, _node );
                                if (validPolicyTree == null)
                                    break;
                            }
                        }
                    }
                }
                else
                    ++index1;
            }
            return validPolicyTree;
        }

        internal static void GetCertStatus(
          DateTime validDate,
          X509Crl crl,
          object cert,
          CertStatus certStatus )
        {
            X509Crl x509Crl;
            try
            {
                x509Crl = new X509Crl( CertificateList.GetInstance( (Asn1Sequence)Asn1Object.FromByteArray( crl.GetEncoded() ) ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "Bouncy Castle X509Crl could not be created.", ex );
            }
            X509CrlEntry revokedCertificate = x509Crl.GetRevokedCertificate( GetSerialNumber( cert ) );
            if (revokedCertificate == null)
                return;
            X509Name issuerPrincipal = GetIssuerPrincipal( cert );
            if (!issuerPrincipal.Equivalent( revokedCertificate.GetCertificateIssuer(), true ) && !issuerPrincipal.Equivalent( crl.IssuerDN, true ))
                return;
            DerEnumerated derEnumerated = null;
            if (revokedCertificate.HasExtensions)
            {
                try
                {
                    derEnumerated = DerEnumerated.GetInstance( GetExtensionValue( revokedCertificate, X509Extensions.ReasonCode ) );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Reason code CRL entry extension could not be decoded.", ex );
                }
            }
            if (validDate.Ticks < revokedCertificate.RevocationDate.Ticks && derEnumerated != null && !derEnumerated.Value.TestBit( 0 ) && !derEnumerated.Value.TestBit( 1 ) && !derEnumerated.Value.TestBit( 2 ) && !derEnumerated.Value.TestBit( 8 ))
                return;
            certStatus.Status = derEnumerated == null ? 0 : derEnumerated.Value.SignValue;
            certStatus.RevocationDate = new DateTimeObject( revokedCertificate.RevocationDate );
        }

        internal static AsymmetricKeyParameter GetNextWorkingKey( IList certs, int index )
        {
            AsymmetricKeyParameter publicKey1 = ((X509Certificate)certs[index]).GetPublicKey();
            if (!(publicKey1 is DsaPublicKeyParameters))
                return publicKey1;
            DsaPublicKeyParameters nextWorkingKey = (DsaPublicKeyParameters)publicKey1;
            if (nextWorkingKey.Parameters != null)
                return nextWorkingKey;
            for (int index1 = index + 1; index1 < certs.Count; ++index1)
            {
                AsymmetricKeyParameter publicKey2 = ((X509Certificate)certs[index1]).GetPublicKey();
                DsaPublicKeyParameters publicKeyParameters = publicKey2 is DsaPublicKeyParameters ? (DsaPublicKeyParameters)publicKey2 : throw new PkixCertPathValidatorException( "DSA parameters cannot be inherited from previous certificate." );
                if (publicKeyParameters.Parameters != null)
                {
                    DsaParameters parameters = publicKeyParameters.Parameters;
                    try
                    {
                        return new DsaPublicKeyParameters( nextWorkingKey.Y, parameters );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( ex.Message );
                    }
                }
            }
            throw new PkixCertPathValidatorException( "DSA parameters cannot be inherited from previous certificate." );
        }

        internal static DateTime GetValidCertDateFromValidityModel(
          PkixParameters paramsPkix,
          PkixCertPath certPath,
          int index )
        {
            if (paramsPkix.ValidityModel != 1 || index <= 0)
                return GetValidDate( paramsPkix );
            if (index - 1 == 0)
            {
                DerGeneralizedTime instance;
                try
                {
                    instance = DerGeneralizedTime.GetInstance( ((X509ExtensionBase)certPath.Certificates[index - 1]).GetExtensionValue( IsisMttObjectIdentifiers.IdIsisMttATDateOfCertGen ) );
                }
                catch (ArgumentException ex)
                {
                    throw new Exception( "Date of cert gen extension could not be read." );
                }
                if (instance != null)
                {
                    try
                    {
                        return instance.ToDateTime();
                    }
                    catch (ArgumentException ex)
                    {
                        throw new Exception( "Date from date of cert gen extension could not be parsed.", ex );
                    }
                }
            }
            return ((X509Certificate)certPath.Certificates[index - 1]).NotBefore;
        }

        internal static ICollection FindCertificates( X509CertStoreSelector certSelect, IList certStores )
        {
            ISet certificates = new HashSet();
            foreach (IX509Store certStore in (IEnumerable)certStores)
            {
                try
                {
                    foreach (X509Certificate match in (IEnumerable)certStore.GetMatches( certSelect ))
                        certificates.Add( match );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Problem while picking certificates from X.509 store.", ex );
                }
            }
            return certificates;
        }

        internal static void GetCrlIssuersFromDistributionPoint(
          DistributionPoint dp,
          ICollection issuerPrincipals,
          X509CrlStoreSelector selector,
          PkixParameters pkixParams )
        {
            IList arrayList = Platform.CreateArrayList();
            if (dp.CrlIssuer != null)
            {
                GeneralName[] names = dp.CrlIssuer.GetNames();
                for (int index = 0; index < names.Length; ++index)
                {
                    if (names[index].TagNo == 4)
                    {
                        try
                        {
                            arrayList.Add( X509Name.GetInstance( names[index].Name.ToAsn1Object() ) );
                        }
                        catch (IOException ex)
                        {
                            throw new Exception( "CRL issuer information from distribution point cannot be decoded.", ex );
                        }
                    }
                }
            }
            else
            {
                if (dp.DistributionPointName == null)
                    throw new Exception( "CRL issuer is omitted from distribution point but no distributionPoint field present." );
                foreach (X509Name issuerPrincipal in (IEnumerable)issuerPrincipals)
                    arrayList.Add( issuerPrincipal );
            }
            selector.Issuers = arrayList;
        }

        internal static ISet GetCompleteCrls(
          DistributionPoint dp,
          object cert,
          DateTime currentDate,
          PkixParameters paramsPKIX )
        {
            X509CrlStoreSelector crlStoreSelector = new();
            try
            {
                ISet issuerPrincipals = new HashSet();
                if (cert is X509V2AttributeCertificate)
                    issuerPrincipals.Add( ((X509V2AttributeCertificate)cert).Issuer.GetPrincipals()[0] );
                else
                    issuerPrincipals.Add( GetIssuerPrincipal( cert ) );
                GetCrlIssuersFromDistributionPoint( dp, issuerPrincipals, crlStoreSelector, paramsPKIX );
            }
            catch (Exception ex)
            {
                throw new Exception( "Could not get issuer information from distribution point.", ex );
            }
            switch (cert)
            {
                case X509Certificate _:
                    crlStoreSelector.CertificateChecking = (X509Certificate)cert;
                    break;
                case X509V2AttributeCertificate _:
                    crlStoreSelector.AttrCertChecking = (IX509AttributeCertificate)cert;
                    break;
            }
            crlStoreSelector.CompleteCrlEnabled = true;
            ISet crls = CrlUtilities.FindCrls( crlStoreSelector, paramsPKIX, currentDate );
            if (!crls.IsEmpty)
                return crls;
            if (cert is IX509AttributeCertificate)
                throw new Exception( "No CRLs found for issuer \"" + ((IX509AttributeCertificate)cert).Issuer.GetPrincipals()[0] + "\"" );
            throw new Exception( "No CRLs found for issuer \"" + ((X509Certificate)cert).IssuerDN + "\"" );
        }

        internal static ISet GetDeltaCrls(
          DateTime currentDate,
          PkixParameters paramsPKIX,
          X509Crl completeCRL )
        {
            X509CrlStoreSelector crlselect = new();
            try
            {
                IList arrayList = Platform.CreateArrayList();
                arrayList.Add( completeCRL.IssuerDN );
                crlselect.Issuers = arrayList;
            }
            catch (IOException ex)
            {
                throw new Exception( "Cannot extract issuer from CRL.", ex );
            }
            BigInteger bigInteger = null;
            try
            {
                Asn1Object extensionValue = GetExtensionValue( completeCRL, X509Extensions.CrlNumber );
                if (extensionValue != null)
                    bigInteger = DerInteger.GetInstance( extensionValue ).PositiveValue;
            }
            catch (Exception ex)
            {
                throw new Exception( "CRL number extension could not be extracted from CRL.", ex );
            }
            byte[] numArray = null;
            try
            {
                Asn1Object extensionValue = GetExtensionValue( completeCRL, X509Extensions.IssuingDistributionPoint );
                if (extensionValue != null)
                    numArray = extensionValue.GetDerEncoded();
            }
            catch (Exception ex)
            {
                throw new Exception( "Issuing distribution point extension value could not be read.", ex );
            }
            crlselect.MinCrlNumber = bigInteger == null ? null : bigInteger.Add( BigInteger.One );
            crlselect.IssuingDistributionPoint = numArray;
            crlselect.IssuingDistributionPointEnabled = true;
            crlselect.MaxBaseCrlNumber = bigInteger;
            ISet crls = CrlUtilities.FindCrls( crlselect, paramsPKIX, currentDate );
            ISet deltaCrls = new HashSet();
            foreach (X509Crl x509Crl in (IEnumerable)crls)
            {
                if (isDeltaCrl( x509Crl ))
                    deltaCrls.Add( x509Crl );
            }
            return deltaCrls;
        }

        private static bool isDeltaCrl( X509Crl crl ) => crl.GetCriticalExtensionOids().Contains( X509Extensions.DeltaCrlIndicator.Id );

        internal static ICollection FindCertificates(
          X509AttrCertStoreSelector certSelect,
          IList certStores )
        {
            ISet certificates = new HashSet();
            foreach (IX509Store certStore in (IEnumerable)certStores)
            {
                try
                {
                    foreach (X509V2AttributeCertificate match in (IEnumerable)certStore.GetMatches( certSelect ))
                        certificates.Add( match );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Problem while picking certificates from X.509 store.", ex );
                }
            }
            return certificates;
        }

        internal static void AddAdditionalStoresFromCrlDistributionPoint(
          CrlDistPoint crldp,
          PkixParameters pkixParams )
        {
            if (crldp == null)
                return;
            DistributionPoint[] distributionPoints;
            try
            {
                distributionPoints = crldp.GetDistributionPoints();
            }
            catch (Exception ex)
            {
                throw new Exception( "Distribution points could not be read.", ex );
            }
            for (int index1 = 0; index1 < distributionPoints.Length; ++index1)
            {
                DistributionPointName distributionPointName = distributionPoints[index1].DistributionPointName;
                if (distributionPointName != null && distributionPointName.PointType == 0)
                {
                    GeneralName[] names = GeneralNames.GetInstance( distributionPointName.Name ).GetNames();
                    for (int index2 = 0; index2 < names.Length; ++index2)
                    {
                        if (names[index2].TagNo == 6)
                            AddAdditionalStoreFromLocation( DerIA5String.GetInstance( names[index2].Name ).GetString(), pkixParams );
                    }
                }
            }
        }

        internal static bool ProcessCertD1i(
          int index,
          IList[] policyNodes,
          DerObjectIdentifier pOid,
          ISet pq )
        {
            IList policyNode = policyNodes[index - 1];
            for (int index1 = 0; index1 < policyNode.Count; ++index1)
            {
                PkixPolicyNode parent = (PkixPolicyNode)policyNode[index1];
                if (parent.ExpectedPolicies.Contains( pOid.Id ))
                {
                    ISet expectedPolicies = new HashSet
                    {
                        pOid.Id
                    };
                    PkixPolicyNode child = new( Platform.CreateArrayList(), index, expectedPolicies, parent, pq, pOid.Id, false );
                    parent.AddChild( child );
                    policyNodes[index].Add( child );
                    return true;
                }
            }
            return false;
        }

        internal static void ProcessCertD1ii(
          int index,
          IList[] policyNodes,
          DerObjectIdentifier _poid,
          ISet _pq )
        {
            IList policyNode = policyNodes[index - 1];
            for (int index1 = 0; index1 < policyNode.Count; ++index1)
            {
                PkixPolicyNode parent = (PkixPolicyNode)policyNode[index1];
                if (ANY_POLICY.Equals( parent.ValidPolicy ))
                {
                    ISet expectedPolicies = new HashSet
                    {
                        _poid.Id
                    };
                    PkixPolicyNode child = new( Platform.CreateArrayList(), index, expectedPolicies, parent, _pq, _poid.Id, false );
                    parent.AddChild( child );
                    policyNodes[index].Add( child );
                    break;
                }
            }
        }

        internal static ICollection FindIssuerCerts(
          X509Certificate cert,
          PkixBuilderParameters pkixParams )
        {
            X509CertStoreSelector certSelect = new();
            ISet issuerCerts = new HashSet();
            try
            {
                certSelect.Subject = cert.IssuerDN;
            }
            catch (IOException ex)
            {
                throw new Exception( "Subject criteria for certificate selector to find issuer certificate could not be set.", ex );
            }
            try
            {
                issuerCerts.AddAll( FindCertificates( certSelect, pkixParams.GetStores() ) );
                issuerCerts.AddAll( FindCertificates( certSelect, pkixParams.GetAdditionalStores() ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "Issuer certificate cannot be searched.", ex );
            }
            return issuerCerts;
        }

        internal static Asn1Object GetExtensionValue( IX509Extension ext, DerObjectIdentifier oid )
        {
            Asn1OctetString extensionValue = ext.GetExtensionValue( oid );
            return extensionValue == null ? null : X509ExtensionUtilities.FromExtensionValue( extensionValue );
        }
    }
}
