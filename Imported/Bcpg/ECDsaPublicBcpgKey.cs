// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ECDsaPublicBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Bcpg
{
    public class ECDsaPublicBcpgKey : ECPublicBcpgKey
    {
        protected internal ECDsaPublicBcpgKey( BcpgInputStream bcpgIn )
          : base( bcpgIn )
        {
        }

        public ECDsaPublicBcpgKey( DerObjectIdentifier oid, ECPoint point )
          : base( oid, point )
        {
        }

        public ECDsaPublicBcpgKey( DerObjectIdentifier oid, BigInteger encodedPoint )
          : base( oid, encodedPoint )
        {
        }
    }
}
