// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCrlUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    public class PkixCrlUtilities
    {
        public virtual ISet FindCrls(
          X509CrlStoreSelector crlselect,
          PkixParameters paramsPkix,
          DateTime currentDate )
        {
            ISet set = new HashSet();
            try
            {
                set.AddAll( this.FindCrls( crlselect, paramsPkix.GetAdditionalStores() ) );
                set.AddAll( this.FindCrls( crlselect, paramsPkix.GetStores() ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "Exception obtaining complete CRLs.", ex );
            }
            ISet crls = new HashSet();
            DateTime dateTime = currentDate;
            if (paramsPkix.Date != null)
                dateTime = paramsPkix.Date.Value;
            foreach (X509Crl o in (IEnumerable)set)
            {
                DateTime thisUpdate = o.NextUpdate.Value;
                if (thisUpdate.CompareTo( (object)dateTime ) > 0)
                {
                    X509Certificate certificateChecking = crlselect.CertificateChecking;
                    if (certificateChecking != null)
                    {
                        thisUpdate = o.ThisUpdate;
                        if (thisUpdate.CompareTo( (object)certificateChecking.NotAfter ) < 0)
                            crls.Add( o );
                    }
                    else
                        crls.Add( o );
                }
            }
            return crls;
        }

        public virtual ISet FindCrls( X509CrlStoreSelector crlselect, PkixParameters paramsPkix )
        {
            ISet crls = new HashSet();
            try
            {
                crls.AddAll( this.FindCrls( crlselect, paramsPkix.GetStores() ) );
            }
            catch (Exception ex)
            {
                throw new Exception( "Exception obtaining complete CRLs.", ex );
            }
            return crls;
        }

        private ICollection FindCrls( X509CrlStoreSelector crlSelect, IList crlStores )
        {
            ISet crls = new HashSet();
            Exception exception = null;
            bool flag = false;
            foreach (IX509Store crlStore in (IEnumerable)crlStores)
            {
                try
                {
                    crls.AddAll( crlStore.GetMatches( crlSelect ) );
                    flag = true;
                }
                catch (X509StoreException ex)
                {
                    exception = new Exception( "Exception searching in X.509 CRL store.", ex );
                }
            }
            if (!flag && exception != null)
                throw exception;
            return crls;
        }
    }
}
