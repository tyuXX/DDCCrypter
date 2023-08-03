// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCertPathBuilder
    {
        private Exception certPathException;

        public virtual PkixCertPathBuilderResult Build( PkixBuilderParameters pkixParams )
        {
            IX509Selector targetCertConstraints = pkixParams.GetTargetCertConstraints();
            if (!(targetCertConstraints is X509CertStoreSelector))
                throw new PkixCertPathBuilderException( "TargetConstraints must be an instance of " + typeof( X509CertStoreSelector ).FullName + " for " + Platform.GetTypeName( this ) + " class." );
            ISet set = new HashSet();
            try
            {
                set.AddAll( PkixCertPathValidatorUtilities.FindCertificates( (X509CertStoreSelector)targetCertConstraints, pkixParams.GetStores() ) );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathBuilderException( "Error finding target certificate.", ex );
            }
            if (set.IsEmpty)
                throw new PkixCertPathBuilderException( "No certificate found matching targetContraints." );
            PkixCertPathBuilderResult pathBuilderResult = null;
            IList arrayList = Platform.CreateArrayList();
            foreach (X509Certificate tbvCert in (IEnumerable)set)
            {
                pathBuilderResult = this.Build( tbvCert, pkixParams, arrayList );
                if (pathBuilderResult != null)
                    break;
            }
            if (pathBuilderResult == null && this.certPathException != null)
                throw new PkixCertPathBuilderException( this.certPathException.Message, this.certPathException.InnerException );
            return pathBuilderResult != null || this.certPathException != null ? pathBuilderResult : throw new PkixCertPathBuilderException( "Unable to find certificate chain." );
        }

        protected virtual PkixCertPathBuilderResult Build(
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
            PkixCertPathValidator certPathValidator = new PkixCertPathValidator();
            try
            {
                if (PkixCertPathValidatorUtilities.FindTrustAnchor( tbvCert, pkixParams.GetTrustAnchors() ) != null)
                {
                    PkixCertPath certPath;
                    try
                    {
                        certPath = new PkixCertPath( tbvPath );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception( "Certification path could not be constructed from certificate list.", ex );
                    }
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
                    throw new Exception( "No additiontal X.509 stores can be added from certificate locations.", ex );
                }
                HashSet hashSet = new HashSet();
                try
                {
                    hashSet.AddAll( PkixCertPathValidatorUtilities.FindIssuerCerts( tbvCert, pkixParams ) );
                }
                catch (Exception ex)
                {
                    throw new Exception( "Cannot find issuer certificate for certificate in certification path.", ex );
                }
                if (hashSet.IsEmpty)
                    throw new Exception( "No issuer certificate for certificate in certification path found." );
                foreach (X509Certificate tbvCert1 in hashSet)
                {
                    pathBuilderResult = this.Build( tbvCert1, pkixParams, tbvPath );
                    if (pathBuilderResult != null)
                        break;
                }
            }
            catch (Exception ex)
            {
                this.certPathException = ex;
            }
            if (pathBuilderResult == null)
                tbvPath.Remove( tbvCert );
            return pathBuilderResult;
        }
    }
}
