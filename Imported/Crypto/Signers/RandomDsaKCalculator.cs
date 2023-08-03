// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.RandomDsaKCalculator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class RandomDsaKCalculator : IDsaKCalculator
    {
        private BigInteger q;
        private SecureRandom random;

        public virtual bool IsDeterministic => false;

        public virtual void Init( BigInteger n, SecureRandom random )
        {
            this.q = n;
            this.random = random;
        }

        public virtual void Init( BigInteger n, BigInteger d, byte[] message ) => throw new InvalidOperationException( "Operation not supported" );

        public virtual BigInteger NextK()
        {
            int bitLength = this.q.BitLength;
            BigInteger bigInteger;
            do
            {
                bigInteger = new BigInteger( bitLength, random );
            }
            while (bigInteger.SignValue < 1 || bigInteger.CompareTo( this.q ) >= 0);
            return bigInteger;
        }
    }
}
