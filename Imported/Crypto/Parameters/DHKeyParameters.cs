// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DHKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHKeyParameters : AsymmetricKeyParameter
    {
        private readonly DHParameters parameters;
        private readonly DerObjectIdentifier algorithmOid;

        protected DHKeyParameters( bool isPrivate, DHParameters parameters )
          : this( isPrivate, parameters, PkcsObjectIdentifiers.DhKeyAgreement )
        {
        }

        protected DHKeyParameters(
          bool isPrivate,
          DHParameters parameters,
          DerObjectIdentifier algorithmOid )
          : base( isPrivate )
        {
            this.parameters = parameters;
            this.algorithmOid = algorithmOid;
        }

        public DHParameters Parameters => this.parameters;

        public DerObjectIdentifier AlgorithmOid => this.algorithmOid;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is DHKeyParameters other && this.Equals( other );
        }

        protected bool Equals( DHKeyParameters other ) => Equals( parameters, other.parameters ) && this.Equals( (AsymmetricKeyParameter)other );

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (this.parameters != null)
                hashCode ^= this.parameters.GetHashCode();
            return hashCode;
        }
    }
}
