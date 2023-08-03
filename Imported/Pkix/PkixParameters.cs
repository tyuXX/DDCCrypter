// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.X509.Store;
using System.Collections;

namespace Org.BouncyCastle.Pkix
{
    public class PkixParameters
    {
        public const int PkixValidityModel = 0;
        public const int ChainValidityModel = 1;
        private ISet trustAnchors;
        private DateTimeObject date;
        private IList certPathCheckers;
        private bool revocationEnabled = true;
        private ISet initialPolicies;
        private bool explicitPolicyRequired = false;
        private bool anyPolicyInhibited = false;
        private bool policyMappingInhibited = false;
        private bool policyQualifiersRejected = true;
        private IX509Selector certSelector;
        private IList stores;
        private IX509Selector selector;
        private bool additionalLocationsEnabled;
        private IList additionalStores;
        private ISet trustedACIssuers;
        private ISet necessaryACAttributes;
        private ISet prohibitedACAttributes;
        private ISet attrCertCheckers;
        private int validityModel = 0;
        private bool useDeltas = false;

        public PkixParameters( ISet trustAnchors )
        {
            this.SetTrustAnchors( trustAnchors );
            this.initialPolicies = new HashSet();
            this.certPathCheckers = Platform.CreateArrayList();
            this.stores = Platform.CreateArrayList();
            this.additionalStores = Platform.CreateArrayList();
            this.trustedACIssuers = new HashSet();
            this.necessaryACAttributes = new HashSet();
            this.prohibitedACAttributes = new HashSet();
            this.attrCertCheckers = new HashSet();
        }

        public virtual bool IsRevocationEnabled
        {
            get => this.revocationEnabled;
            set => this.revocationEnabled = value;
        }

        public virtual bool IsExplicitPolicyRequired
        {
            get => this.explicitPolicyRequired;
            set => this.explicitPolicyRequired = value;
        }

        public virtual bool IsAnyPolicyInhibited
        {
            get => this.anyPolicyInhibited;
            set => this.anyPolicyInhibited = value;
        }

        public virtual bool IsPolicyMappingInhibited
        {
            get => this.policyMappingInhibited;
            set => this.policyMappingInhibited = value;
        }

        public virtual bool IsPolicyQualifiersRejected
        {
            get => this.policyQualifiersRejected;
            set => this.policyQualifiersRejected = value;
        }

        public virtual DateTimeObject Date
        {
            get => this.date;
            set => this.date = value;
        }

        public virtual ISet GetTrustAnchors() => new HashSet( trustAnchors );

        public virtual void SetTrustAnchors( ISet tas )
        {
            if (tas == null)
                throw new ArgumentNullException( "value" );
            if (tas.IsEmpty)
                throw new ArgumentException( "non-empty set required", "value" );
            this.trustAnchors = new HashSet();
            foreach (TrustAnchor ta in (IEnumerable)tas)
            {
                if (ta != null)
                    this.trustAnchors.Add( ta );
            }
        }

        public virtual X509CertStoreSelector GetTargetCertConstraints() => this.certSelector == null ? null : (X509CertStoreSelector)this.certSelector.Clone();

        public virtual void SetTargetCertConstraints( IX509Selector selector )
        {
            if (selector == null)
                this.certSelector = null;
            else
                this.certSelector = (IX509Selector)selector.Clone();
        }

        public virtual ISet GetInitialPolicies()
        {
            ISet s = this.initialPolicies;
            if (this.initialPolicies == null)
                s = new HashSet();
            return new HashSet( s );
        }

        public virtual void SetInitialPolicies( ISet initialPolicies )
        {
            this.initialPolicies = new HashSet();
            if (initialPolicies == null)
                return;
            foreach (string initialPolicy in (IEnumerable)initialPolicies)
            {
                if (initialPolicy != null)
                    this.initialPolicies.Add( initialPolicy );
            }
        }

        public virtual void SetCertPathCheckers( IList checkers )
        {
            this.certPathCheckers = Platform.CreateArrayList();
            if (checkers == null)
                return;
            foreach (PkixCertPathChecker checker in (IEnumerable)checkers)
                this.certPathCheckers.Add( checker.Clone() );
        }

        public virtual IList GetCertPathCheckers()
        {
            IList arrayList = Platform.CreateArrayList();
            foreach (PkixCertPathChecker certPathChecker in (IEnumerable)this.certPathCheckers)
                arrayList.Add( certPathChecker.Clone() );
            return arrayList;
        }

        public virtual void AddCertPathChecker( PkixCertPathChecker checker )
        {
            if (checker == null)
                return;
            this.certPathCheckers.Add( checker.Clone() );
        }

        public virtual object Clone()
        {
            PkixParameters pkixParameters = new( this.GetTrustAnchors() );
            pkixParameters.SetParams( this );
            return pkixParameters;
        }

        protected virtual void SetParams( PkixParameters parameters )
        {
            this.Date = parameters.Date;
            this.SetCertPathCheckers( parameters.GetCertPathCheckers() );
            this.IsAnyPolicyInhibited = parameters.IsAnyPolicyInhibited;
            this.IsExplicitPolicyRequired = parameters.IsExplicitPolicyRequired;
            this.IsPolicyMappingInhibited = parameters.IsPolicyMappingInhibited;
            this.IsRevocationEnabled = parameters.IsRevocationEnabled;
            this.SetInitialPolicies( parameters.GetInitialPolicies() );
            this.IsPolicyQualifiersRejected = parameters.IsPolicyQualifiersRejected;
            this.SetTargetCertConstraints( parameters.GetTargetCertConstraints() );
            this.SetTrustAnchors( parameters.GetTrustAnchors() );
            this.validityModel = parameters.validityModel;
            this.useDeltas = parameters.useDeltas;
            this.additionalLocationsEnabled = parameters.additionalLocationsEnabled;
            this.selector = parameters.selector == null ? null : (IX509Selector)parameters.selector.Clone();
            this.stores = Platform.CreateArrayList( parameters.stores );
            this.additionalStores = Platform.CreateArrayList( parameters.additionalStores );
            this.trustedACIssuers = new HashSet( parameters.trustedACIssuers );
            this.prohibitedACAttributes = new HashSet( parameters.prohibitedACAttributes );
            this.necessaryACAttributes = new HashSet( parameters.necessaryACAttributes );
            this.attrCertCheckers = new HashSet( parameters.attrCertCheckers );
        }

        public virtual bool IsUseDeltasEnabled
        {
            get => this.useDeltas;
            set => this.useDeltas = value;
        }

        public virtual int ValidityModel
        {
            get => this.validityModel;
            set => this.validityModel = value;
        }

        public virtual void SetStores( IList stores )
        {
            if (stores == null)
            {
                this.stores = Platform.CreateArrayList();
            }
            else
            {
                foreach (object store in (IEnumerable)stores)
                {
                    if (!(store is IX509Store))
                        throw new InvalidCastException( "All elements of list must be of type " + typeof( IX509Store ).FullName );
                }
                this.stores = Platform.CreateArrayList( stores );
            }
        }

        public virtual void AddStore( IX509Store store )
        {
            if (store == null)
                return;
            this.stores.Add( store );
        }

        public virtual void AddAdditionalStore( IX509Store store )
        {
            if (store == null)
                return;
            this.additionalStores.Add( store );
        }

        public virtual IList GetAdditionalStores() => Platform.CreateArrayList( additionalStores );

        public virtual IList GetStores() => Platform.CreateArrayList( stores );

        public virtual bool IsAdditionalLocationsEnabled => this.additionalLocationsEnabled;

        public virtual void SetAdditionalLocationsEnabled( bool enabled ) => this.additionalLocationsEnabled = enabled;

        public virtual IX509Selector GetTargetConstraints() => this.selector != null ? (IX509Selector)this.selector.Clone() : null;

        public virtual void SetTargetConstraints( IX509Selector selector )
        {
            if (selector != null)
                this.selector = (IX509Selector)selector.Clone();
            else
                this.selector = null;
        }

        public virtual ISet GetTrustedACIssuers() => new HashSet( trustedACIssuers );

        public virtual void SetTrustedACIssuers( ISet trustedACIssuers )
        {
            if (trustedACIssuers == null)
            {
                this.trustedACIssuers = new HashSet();
            }
            else
            {
                foreach (object trustedAcIssuer in (IEnumerable)trustedACIssuers)
                {
                    if (!(trustedAcIssuer is TrustAnchor))
                        throw new InvalidCastException( "All elements of set must be of type " + typeof( TrustAnchor ).FullName + "." );
                }
                this.trustedACIssuers = new HashSet( trustedACIssuers );
            }
        }

        public virtual ISet GetNecessaryACAttributes() => new HashSet( necessaryACAttributes );

        public virtual void SetNecessaryACAttributes( ISet necessaryACAttributes )
        {
            if (necessaryACAttributes == null)
            {
                this.necessaryACAttributes = new HashSet();
            }
            else
            {
                foreach (object necessaryAcAttribute in (IEnumerable)necessaryACAttributes)
                {
                    if (!(necessaryAcAttribute is string))
                        throw new InvalidCastException( "All elements of set must be of type string." );
                }
                this.necessaryACAttributes = new HashSet( necessaryACAttributes );
            }
        }

        public virtual ISet GetProhibitedACAttributes() => new HashSet( prohibitedACAttributes );

        public virtual void SetProhibitedACAttributes( ISet prohibitedACAttributes )
        {
            if (prohibitedACAttributes == null)
            {
                this.prohibitedACAttributes = new HashSet();
            }
            else
            {
                foreach (object prohibitedAcAttribute in (IEnumerable)prohibitedACAttributes)
                {
                    if (!(prohibitedAcAttribute is string))
                        throw new InvalidCastException( "All elements of set must be of type string." );
                }
                this.prohibitedACAttributes = new HashSet( prohibitedACAttributes );
            }
        }

        public virtual ISet GetAttrCertCheckers() => new HashSet( attrCertCheckers );

        public virtual void SetAttrCertCheckers( ISet attrCertCheckers )
        {
            if (attrCertCheckers == null)
            {
                this.attrCertCheckers = new HashSet();
            }
            else
            {
                foreach (object attrCertChecker in (IEnumerable)attrCertCheckers)
                {
                    if (!(attrCertChecker is PkixAttrCertChecker))
                        throw new InvalidCastException( "All elements of set must be of type " + typeof( PkixAttrCertChecker ).FullName + "." );
                }
                this.attrCertCheckers = new HashSet( attrCertCheckers );
            }
        }
    }
}
