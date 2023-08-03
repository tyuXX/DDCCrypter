// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.AbstractECMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public abstract class AbstractECMultiplier : ECMultiplier
    {
        public virtual ECPoint Multiply( ECPoint p, BigInteger k )
        {
            int signValue = k.SignValue;
            if (signValue == 0 || p.IsInfinity)
                return p.Curve.Infinity;
            ECPoint ecPoint = this.MultiplyPositive( p, k.Abs() );
            return ECAlgorithms.ValidatePoint( signValue > 0 ? ecPoint : ecPoint.Negate() );
        }

        protected abstract ECPoint MultiplyPositive( ECPoint p, BigInteger k );
    }
}
