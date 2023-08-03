// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Misc.VerisignCzagExtension
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Misc
{
    public class VerisignCzagExtension : DerIA5String
    {
        public VerisignCzagExtension( DerIA5String str )
          : base( str.GetString() )
        {
        }

        public override string ToString() => "VerisignCzagExtension: " + this.GetString();
    }
}
