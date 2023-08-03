// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixAttrCertPathValidator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System;

namespace Org.BouncyCastle.Pkix
{
    public class PkixAttrCertPathValidator
    {
        public virtual PkixCertPathValidatorResult Validate(
          PkixCertPath certPath,
          PkixParameters pkixParams )
        {
            IX509Selector targetConstraints = pkixParams.GetTargetConstraints();
            IX509AttributeCertificate attrCert = targetConstraints is X509AttrCertStoreSelector ? ((X509AttrCertStoreSelector)targetConstraints).AttributeCert : throw new ArgumentException( "TargetConstraints must be an instance of " + typeof( X509AttrCertStoreSelector ).FullName, nameof( pkixParams ) );
            PkixCertPath holderCertPath = Rfc3281CertPathUtilities.ProcessAttrCert1( attrCert, pkixParams );
            PkixCertPathValidatorResult pathValidatorResult = Rfc3281CertPathUtilities.ProcessAttrCert2( certPath, pkixParams );
            X509Certificate certificate = (X509Certificate)certPath.Certificates[0];
            Rfc3281CertPathUtilities.ProcessAttrCert3( certificate, pkixParams );
            Rfc3281CertPathUtilities.ProcessAttrCert4( certificate, pkixParams );
            Rfc3281CertPathUtilities.ProcessAttrCert5( attrCert, pkixParams );
            Rfc3281CertPathUtilities.ProcessAttrCert7( attrCert, certPath, holderCertPath, pkixParams );
            Rfc3281CertPathUtilities.AdditionalChecks( attrCert, pkixParams );
            DateTime fromValidityModel;
            try
            {
                fromValidityModel = PkixCertPathValidatorUtilities.GetValidCertDateFromValidityModel( pkixParams, null, -1 );
            }
            catch (Exception ex)
            {
                throw new PkixCertPathValidatorException( "Could not get validity date from attribute certificate.", ex );
            }
            Rfc3281CertPathUtilities.CheckCrls( attrCert, pkixParams, certificate, fromValidityModel, certPath.Certificates );
            return pathValidatorResult;
        }
    }
}
