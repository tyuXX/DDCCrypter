// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.WTauNafMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math.EC.Abc;
using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class WTauNafMultiplier : AbstractECMultiplier
    {
        internal static readonly string PRECOMP_NAME = "bc_wtnaf";

        protected override ECPoint MultiplyPositive( ECPoint point, BigInteger k )
        {
            AbstractF2mPoint abstractF2mPoint = point is AbstractF2mPoint ? (AbstractF2mPoint)point : throw new ArgumentException( "Only AbstractF2mPoint can be used in WTauNafMultiplier" );
            AbstractF2mCurve curve = (AbstractF2mCurve)abstractF2mPoint.Curve;
            int fieldSize = curve.FieldSize;
            sbyte intValue = (sbyte)curve.A.ToBigInteger().IntValue;
            sbyte mu = Tnaf.GetMu( intValue );
            BigInteger[] si = curve.GetSi();
            ZTauElement lambda = Tnaf.PartModReduction( k, fieldSize, intValue, si, mu, 10 );
            return this.MultiplyWTnaf( abstractF2mPoint, lambda, curve.GetPreCompInfo( abstractF2mPoint, PRECOMP_NAME ), intValue, mu );
        }

        private AbstractF2mPoint MultiplyWTnaf(
          AbstractF2mPoint p,
          ZTauElement lambda,
          PreCompInfo preCompInfo,
          sbyte a,
          sbyte mu )
        {
            ZTauElement[] alpha = a == 0 ? Tnaf.Alpha0 : Tnaf.Alpha1;
            BigInteger tw = Tnaf.GetTw( mu, 4 );
            sbyte[] u = Tnaf.TauAdicWNaf( mu, lambda, 4, BigInteger.ValueOf( 16L ), tw, alpha );
            return MultiplyFromWTnaf( p, u, preCompInfo );
        }

        private static AbstractF2mPoint MultiplyFromWTnaf(
          AbstractF2mPoint p,
          sbyte[] u,
          PreCompInfo preCompInfo )
        {
            AbstractF2mCurve curve = (AbstractF2mCurve)p.Curve;
            sbyte intValue = (sbyte)curve.A.ToBigInteger().IntValue;
            AbstractF2mPoint[] preComp;
            if (preCompInfo == null || !(preCompInfo is WTauNafPreCompInfo))
            {
                preComp = Tnaf.GetPreComp( p, intValue );
                curve.SetPreCompInfo( p, PRECOMP_NAME, new WTauNafPreCompInfo()
                {
                    PreComp = preComp
                } );
            }
            else
                preComp = ((WTauNafPreCompInfo)preCompInfo).PreComp;
            AbstractF2mPoint[] abstractF2mPointArray = new AbstractF2mPoint[preComp.Length];
            for (int index = 0; index < preComp.Length; ++index)
                abstractF2mPointArray[index] = (AbstractF2mPoint)preComp[index].Negate();
            AbstractF2mPoint abstractF2mPoint1 = (AbstractF2mPoint)p.Curve.Infinity;
            int pow = 0;
            for (int index = u.Length - 1; index >= 0; --index)
            {
                ++pow;
                int num = u[index];
                if (num != 0)
                {
                    AbstractF2mPoint abstractF2mPoint2 = abstractF2mPoint1.TauPow( pow );
                    pow = 0;
                    ECPoint b = num > 0 ? preComp[num >> 1] : (ECPoint)abstractF2mPointArray[-num >> 1];
                    abstractF2mPoint1 = (AbstractF2mPoint)abstractF2mPoint2.Add( b );
                }
            }
            if (pow > 0)
                abstractF2mPoint1 = abstractF2mPoint1.TauPow( pow );
            return abstractF2mPoint1;
        }
    }
}
