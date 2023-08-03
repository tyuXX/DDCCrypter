// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.ECPointBase
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC
{
    public abstract class ECPointBase : ECPoint
    {
        protected internal ECPointBase(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          bool withCompression )
          : base( curve, x, y, withCompression )
        {
        }

        protected internal ECPointBase(
          ECCurve curve,
          ECFieldElement x,
          ECFieldElement y,
          ECFieldElement[] zs,
          bool withCompression )
          : base( curve, x, y, zs, withCompression )
        {
        }

        public override byte[] GetEncoded( bool compressed )
        {
            if (this.IsInfinity)
                return new byte[1];
            ECPoint ecPoint = this.Normalize();
            byte[] encoded1 = ecPoint.XCoord.GetEncoded();
            if (compressed)
            {
                byte[] destinationArray = new byte[encoded1.Length + 1];
                destinationArray[0] = ecPoint.CompressionYTilde ? (byte)3 : (byte)2;
                Array.Copy( encoded1, 0, destinationArray, 1, encoded1.Length );
                return destinationArray;
            }
            byte[] encoded2 = ecPoint.YCoord.GetEncoded();
            byte[] destinationArray1 = new byte[encoded1.Length + encoded2.Length + 1];
            destinationArray1[0] = 4;
            Array.Copy( encoded1, 0, destinationArray1, 1, encoded1.Length );
            Array.Copy( encoded2, 0, destinationArray1, encoded1.Length + 1, encoded2.Length );
            return destinationArray1;
        }

        public override ECPoint Multiply( BigInteger k ) => this.Curve.GetMultiplier().Multiply( this, k );
    }
}
