// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.GlvMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Endo;
using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class GlvMultiplier : AbstractECMultiplier
    {
        protected readonly ECCurve curve;
        protected readonly GlvEndomorphism glvEndomorphism;

        public GlvMultiplier( ECCurve curve, GlvEndomorphism glvEndomorphism )
        {
            this.curve = curve != null && curve.Order != null ? curve : throw new ArgumentException( "Need curve with known group order", nameof( curve ) );
            this.glvEndomorphism = glvEndomorphism;
        }

        protected override ECPoint MultiplyPositive( ECPoint p, BigInteger k )
        {
            if (!this.curve.Equals( p.Curve ))
                throw new InvalidOperationException();
            BigInteger order = p.Curve.Order;
            BigInteger[] bigIntegerArray = this.glvEndomorphism.DecomposeScalar( k.Mod( order ) );
            BigInteger k1 = bigIntegerArray[0];
            BigInteger l = bigIntegerArray[1];
            ECPointMap pointMap = this.glvEndomorphism.PointMap;
            return this.glvEndomorphism.HasEfficientPointMap ? ECAlgorithms.ImplShamirsTrickWNaf( p, k1, pointMap, l ) : ECAlgorithms.ImplShamirsTrickWNaf( p, k1, pointMap.Map( p ), l );
        }
    }
}
