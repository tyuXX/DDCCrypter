// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixPolicyNode
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.Pkix
{
    public class PkixPolicyNode
    {
        protected IList mChildren;
        protected int mDepth;
        protected ISet mExpectedPolicies;
        protected PkixPolicyNode mParent;
        protected ISet mPolicyQualifiers;
        protected string mValidPolicy;
        protected bool mCritical;

        public virtual int Depth => this.mDepth;

        public virtual IEnumerable Children => new EnumerableProxy( mChildren );

        public virtual bool IsCritical
        {
            get => this.mCritical;
            set => this.mCritical = value;
        }

        public virtual ISet PolicyQualifiers => new HashSet( mPolicyQualifiers );

        public virtual string ValidPolicy => this.mValidPolicy;

        public virtual bool HasChildren => this.mChildren.Count != 0;

        public virtual ISet ExpectedPolicies
        {
            get => new HashSet( mExpectedPolicies );
            set => this.mExpectedPolicies = new HashSet( value );
        }

        public virtual PkixPolicyNode Parent
        {
            get => this.mParent;
            set => this.mParent = value;
        }

        public PkixPolicyNode(
          IList children,
          int depth,
          ISet expectedPolicies,
          PkixPolicyNode parent,
          ISet policyQualifiers,
          string validPolicy,
          bool critical )
        {
            this.mChildren = children != null ? Platform.CreateArrayList( children ) : Platform.CreateArrayList();
            this.mDepth = depth;
            this.mExpectedPolicies = expectedPolicies;
            this.mParent = parent;
            this.mPolicyQualifiers = policyQualifiers;
            this.mValidPolicy = validPolicy;
            this.mCritical = critical;
        }

        public virtual void AddChild( PkixPolicyNode child )
        {
            child.Parent = this;
            this.mChildren.Add( child );
        }

        public virtual void RemoveChild( PkixPolicyNode child ) => this.mChildren.Remove( child );

        public override string ToString() => this.ToString( "" );

        public virtual string ToString( string indent )
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( indent );
            stringBuilder.Append( this.mValidPolicy );
            stringBuilder.Append( " {" );
            stringBuilder.Append( Platform.NewLine );
            foreach (PkixPolicyNode mChild in (IEnumerable)this.mChildren)
                stringBuilder.Append( mChild.ToString( indent + "    " ) );
            stringBuilder.Append( indent );
            stringBuilder.Append( "}" );
            stringBuilder.Append( Platform.NewLine );
            return stringBuilder.ToString();
        }

        public virtual object Clone() => this.Copy();

        public virtual PkixPolicyNode Copy()
        {
            PkixPolicyNode pkixPolicyNode = new PkixPolicyNode( Platform.CreateArrayList(), this.mDepth, new HashSet( mExpectedPolicies ), null, new HashSet( mPolicyQualifiers ), this.mValidPolicy, this.mCritical );
            foreach (PkixPolicyNode mChild in (IEnumerable)this.mChildren)
            {
                PkixPolicyNode child = mChild.Copy();
                child.Parent = pkixPolicyNode;
                pkixPolicyNode.AddChild( child );
            }
            return pkixPolicyNode;
        }
    }
}
