// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RsaCoreEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Engines
{
    internal class RsaCoreEngine
    {
        private RsaKeyParameters key;
        private bool forEncryption;
        private int bitSize;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.key = parameters is RsaKeyParameters ? (RsaKeyParameters)parameters : throw new InvalidKeyException( "Not an RSA key" );
            this.forEncryption = forEncryption;
            this.bitSize = this.key.Modulus.BitLength;
        }

        public virtual int GetInputBlockSize() => this.forEncryption ? (this.bitSize - 1) / 8 : (this.bitSize + 7) / 8;

        public virtual int GetOutputBlockSize() => this.forEncryption ? (this.bitSize + 7) / 8 : (this.bitSize - 1) / 8;

        public virtual BigInteger ConvertInput( byte[] inBuf, int inOff, int inLen )
        {
            int num = (this.bitSize + 7) / 8;
            if (inLen > num)
                throw new DataLengthException( "input too large for RSA cipher." );
            BigInteger bigInteger = new( 1, inBuf, inOff, inLen );
            if (bigInteger.CompareTo( this.key.Modulus ) >= 0)
                throw new DataLengthException( "input too large for RSA cipher." );
            return bigInteger;
        }

        public virtual byte[] ConvertOutput( BigInteger result )
        {
            byte[] numArray1 = result.ToByteArrayUnsigned();
            if (this.forEncryption)
            {
                int outputBlockSize = this.GetOutputBlockSize();
                if (numArray1.Length < outputBlockSize)
                {
                    byte[] numArray2 = new byte[outputBlockSize];
                    numArray1.CopyTo( numArray2, numArray2.Length - numArray1.Length );
                    numArray1 = numArray2;
                }
            }
            return numArray1;
        }

        public virtual BigInteger ProcessBlock( BigInteger input )
        {
            if (!(this.key is RsaPrivateCrtKeyParameters))
                return input.ModPow( this.key.Exponent, this.key.Modulus );
            RsaPrivateCrtKeyParameters key = (RsaPrivateCrtKeyParameters)this.key;
            BigInteger p = key.P;
            BigInteger q = key.Q;
            BigInteger dp = key.DP;
            BigInteger dq = key.DQ;
            BigInteger qinv = key.QInv;
            BigInteger bigInteger = input.Remainder( p ).ModPow( dp, p );
            BigInteger n = input.Remainder( q ).ModPow( dq, q );
            return bigInteger.Subtract( n ).Multiply( qinv ).Mod( p ).Multiply( q ).Add( n );
        }
    }
}
