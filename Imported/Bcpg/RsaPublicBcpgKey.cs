// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.RsaPublicBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Bcpg
{
    public class RsaPublicBcpgKey : BcpgObject, IBcpgKey
    {
        private readonly MPInteger n;
        private readonly MPInteger e;

        public RsaPublicBcpgKey( BcpgInputStream bcpgIn )
        {
            this.n = new MPInteger( bcpgIn );
            this.e = new MPInteger( bcpgIn );
        }

        public RsaPublicBcpgKey( BigInteger n, BigInteger e )
        {
            this.n = new MPInteger( n );
            this.e = new MPInteger( e );
        }

        public BigInteger PublicExponent => this.e.Value;

        public BigInteger Modulus => this.n.Value;

        public string Format => "PGP";

        public override byte[] GetEncoded()
        {
            try
            {
                return base.GetEncoded();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override void Encode( BcpgOutputStream bcpgOut ) => bcpgOut.WriteObjects( n, e );
    }
}
