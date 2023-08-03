// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.ECDHBasicAgreement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Crypto.Agreement
{
    public class ECDHBasicAgreement : IBasicAgreement
    {
        protected internal ECPrivateKeyParameters privKey;

        public virtual void Init( ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.privKey = (ECPrivateKeyParameters)parameters;
        }

        public virtual int GetFieldSize() => (this.privKey.Parameters.Curve.FieldSize + 7) / 8;

        public virtual BigInteger CalculateAgreement( ICipherParameters pubKey )
        {
            ECPoint ecPoint = ((ECPublicKeyParameters)pubKey).Q.Multiply( this.privKey.D ).Normalize();
            return !ecPoint.IsInfinity ? ecPoint.AffineXCoord.ToBigInteger() : throw new InvalidOperationException( "Infinity is not a valid agreement value for ECDH" );
        }
    }
}
