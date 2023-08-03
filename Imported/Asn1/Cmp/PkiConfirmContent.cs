// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiConfirmContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiConfirmContent : Asn1Encodable
    {
        public static PkiConfirmContent GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiConfirmContent _:
                    return (PkiConfirmContent)obj;
                case Asn1Null _:
                    return new PkiConfirmContent();
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public override Asn1Object ToAsn1Object() => DerNull.Instance;
    }
}
