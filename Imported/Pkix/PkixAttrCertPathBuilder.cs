// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixAttrCertPathBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    public class PkixAttrCertPathBuilder
    {
        private Exception certPathException;

        public virtual PkixCertPathBuilderResult Build( PkixBuilderParameters pkixParams )
        {
            IX509Selector targetConstraints = pkixParams.GetTargetConstraints();
            if (!(targetConstraints is X509AttrCertStoreSelector))
                throw new PkixCertPathBuilderException( "TargetConstraints must be an instance of " + typeof( X509AttrCertStoreSelector ).FullName + " for " + typeof( PkixAttrCertPathBuilder ).FullName + " class." );
            ICollection certificates;
            try
            {
                certificates = PkixCertPathValidatorUtilities.FindCertificates( (X509AttrCertStoreSelector)targetConstraints, pkixParams.GetStores() );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathBuilderException( "Error finding target attribute certificate.", ex );
            }
            if (certificates.Count == 0)
                throw new PkixCertPathBuilderException( "No attribute certificate found matching targetContraints." );
            PkixCertPathBuilderResult pathBuilderResult = null;
            foreach (IX509AttributeCertificate attrCert in (IEnumerable)certificates)
            {
                X509CertStoreSelector certSelect = new();
                X509Name[] principals = attrCert.Issuer.GetPrincipals();
                ISet set = new HashSet();
                for (int index = 0; index < principals.Length; ++index)
                {
                    try
                    {
                        certSelect.Subject = principals[index];
                        set.AddAll( PkixCertPathValidatorUtilities.FindCertificates( certSelect, pkixParams.GetStores() ) );
                    }
                    catch (Exception ex)
                    {
                        throw new PkixCertPathBuilderException( "Public key certificate for attribute certificate cannot be searched.", ex );
                    }
                }
                if (set.IsEmpty)
                    throw new PkixCertPathBuilderException( "Public key certificate for attribute certificate cannot be found." );
                IList arrayList = Platform.CreateArrayList();
                foreach (X509Certificate tbvCert in (IEnumerable)set)
                {
                    pathBuilderResult = this.Build( attrCert, tbvCert, pkixParams, arrayList );
                    if (pathBuilderResult != null)
                        break;
                }
                if (pathBuilderResult != null)
                    break;
            }
            if (pathBuilderResult == null && this.certPathException != null)
                throw new PkixCertPathBuilderException( "Possible certificate chain could not be validated.", this.certPathException );
            return pathBuilderResult != null || this.certPathException != null ? pathBuilderResult : throw new PkixCertPathBuilderException( "Unable to find certificate chain." );
        }

        private PkixCertPathBuilderResult Build(
          IX509AttributeCertificate attrCert,
          X509Certificate tbvCert,
          PkixBuilderParameters pkixParams,
          IList tbvPath )
        {
            if (tbvPath.Contains( tbvCert ))
                return null;
            if (pkixParams.GetExcludedCerts().Contains( tbvCert ))
                return null;
            if (pkixParams.MaxPathLength != -1 && tbvPath.Count - 1 > pkixParams.MaxPathLength)
                return null;
            tbvPath.Add( tbvCert );
            PkixCertPathBuilderResult pathBuilderResult = null;
            PkixAttrCertPathValidator certPathValidator = new();
            try
            {
                if (PkixCertPathValidatorUtilities.FindTrustAnchor( tbvCert, pkixParams.GetTrustAnchors() ) != null)
                {
                    PkixCertPath certPath = new( tbvPath );
                    PkixCertPathValidatorResult pathValidatorResult;
                    try
                    {
                        pathValidatorResult = certPathValidator.Validate( certPath, pkixParams );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( "Certification path could not be validated.", ex );
                    }
                    return new PkixCertPathBuilderResult( certPath, pathValidatorResult.TrustAnchor, pathValidatorResult.PolicyTree, pathValidatorResult.SubjectPublicKey );
                }
                try
                {
                    PkixCertPathValidatorUtilities.AddAdditionalStoresFromAltNames( tbvCert, pkixParams );
                }
                catch (CertificateParsingException ex)
                {
                    throw new Exception( "No additional X.509 stores can be added from certificate locations.", ex );
                }
                ISet set = new HashSet();
                try
                {
                    set.AddAll( PkixCertPathValidatorUtilities.FindIssuerCerts( tbvCert, pkixParams ) );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Cannot find issuer certificate for certificate in certification path.", ex );
                }
                if (set.IsEmpty)
                    throw new Exception( "No issuer certificate for certificate in certification path found." );
                foreach (X509Certificate x509Certificate in (IEnumerable)set)
                {
                    if (!PkixCertPathValidatorUtilities.IsSelfIssued( x509Certificate ))
                    {
                        pathBuilderResult = this.Build( attrCert, x509Certificate, pkixParams, tbvPath );
                        if (pathBuilderResult != null)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.certPathException = new Exception( "No valid certification path could be build.", ex );
            }
            if (pathBuilderResult == null)
                tbvPath.Remove( tbvCert );
            return pathBuilderResult;
        }
    }
}
