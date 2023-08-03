// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerStringBase
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public abstract class DerStringBase : Asn1Object, IAsn1String
    {
        public abstract string GetString();

        public override string ToString() => this.GetString();

        protected override int Asn1GetHashCode() => this.GetString().GetHashCode();
    }
}
