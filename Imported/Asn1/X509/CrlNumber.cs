// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CrlNumber
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.X509
{
    public class CrlNumber : DerInteger
    {
        public CrlNumber( BigInteger number )
          : base( number )
        {
        }

        public BigInteger Number => this.PositiveValue;

        public override string ToString() => "CRLNumber: " + Number;
    }
}
