// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Field.GF2Polynomial
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.Field
{
    internal class GF2Polynomial : IPolynomial
    {
        protected readonly int[] exponents;

        internal GF2Polynomial( int[] exponents ) => this.exponents = Arrays.Clone( exponents );

        public virtual int Degree => this.exponents[this.exponents.Length - 1];

        public virtual int[] GetExponentsPresent() => Arrays.Clone( this.exponents );

        public override bool Equals( object obj )
        {
            if (this == obj)
                return true;
            return obj is GF2Polynomial gf2Polynomial && Arrays.AreEqual( this.exponents, gf2Polynomial.exponents );
        }

        public override int GetHashCode() => Arrays.GetHashCode( this.exponents );
    }
}
