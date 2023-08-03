// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.Field.GenericPolynomialExtensionField
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.Field
{
    internal class GenericPolynomialExtensionField :
    IPolynomialExtensionField,
    IExtensionField,
    IFiniteField
    {
        protected readonly IFiniteField subfield;
        protected readonly IPolynomial minimalPolynomial;

        internal GenericPolynomialExtensionField( IFiniteField subfield, IPolynomial polynomial )
        {
            this.subfield = subfield;
            this.minimalPolynomial = polynomial;
        }

        public virtual BigInteger Characteristic => this.subfield.Characteristic;

        public virtual int Dimension => this.subfield.Dimension * this.minimalPolynomial.Degree;

        public virtual IFiniteField Subfield => this.subfield;

        public virtual int Degree => this.minimalPolynomial.Degree;

        public virtual IPolynomial MinimalPolynomial => this.minimalPolynomial;

        public override bool Equals( object obj )
        {
            if (this == obj)
                return true;
            return obj is GenericPolynomialExtensionField polynomialExtensionField && this.subfield.Equals( polynomialExtensionField.subfield ) && this.minimalPolynomial.Equals( polynomialExtensionField.minimalPolynomial );
        }

        public override int GetHashCode() => this.subfield.GetHashCode() ^ Integers.RotateLeft( this.minimalPolynomial.GetHashCode(), 16 );
    }
}
