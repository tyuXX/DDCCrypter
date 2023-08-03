// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ElGamalKeyParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ElGamalKeyParameters : AsymmetricKeyParameter
    {
        private readonly ElGamalParameters parameters;

        protected ElGamalKeyParameters( bool isPrivate, ElGamalParameters parameters )
          : base( isPrivate )
        {
            this.parameters = parameters;
        }

        public ElGamalParameters Parameters => this.parameters;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is ElGamalKeyParameters other && this.Equals( other );
        }

        protected bool Equals( ElGamalKeyParameters other ) => Equals( parameters, other.parameters ) && this.Equals( (AsymmetricKeyParameter)other );

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (this.parameters != null)
                hashCode ^= this.parameters.GetHashCode();
            return hashCode;
        }
    }
}
