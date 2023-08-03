// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.DHPublicKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
    public class DHPublicKey : Asn1Encodable
    {
        private readonly DerInteger y;

        public static DHPublicKey GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( DerInteger.GetInstance( obj, isExplicit ) );

        public static DHPublicKey GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DHPublicKey _:
                    return (DHPublicKey)obj;
                case DerInteger _:
                    return new DHPublicKey( (DerInteger)obj );
                default:
                    throw new ArgumentException( "Invalid DHPublicKey: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public DHPublicKey( DerInteger y ) => this.y = y != null ? y : throw new ArgumentNullException( nameof( y ) );

        public DerInteger Y => this.y;

        public override Asn1Object ToAsn1Object() => y;
    }
}
