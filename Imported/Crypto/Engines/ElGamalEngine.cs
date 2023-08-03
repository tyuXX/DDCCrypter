// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.ElGamalEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class ElGamalEngine : IAsymmetricBlockCipher
    {
        private ElGamalKeyParameters key;
        private SecureRandom random;
        private bool forEncryption;
        private int bitSize;

        public virtual string AlgorithmName => "ElGamal";

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
                this.key = (ElGamalKeyParameters)parametersWithRandom.Parameters;
                this.random = parametersWithRandom.Random;
            }
            else
            {
                this.key = (ElGamalKeyParameters)parameters;
                this.random = new SecureRandom();
            }
            this.forEncryption = forEncryption;
            this.bitSize = this.key.Parameters.P.BitLength;
            if (forEncryption)
            {
                if (!(this.key is ElGamalPublicKeyParameters))
                    throw new ArgumentException( "ElGamalPublicKeyParameters are required for encryption." );
            }
            else if (!(this.key is ElGamalPrivateKeyParameters))
                throw new ArgumentException( "ElGamalPrivateKeyParameters are required for decryption." );
        }

        public virtual int GetInputBlockSize() => this.forEncryption ? (this.bitSize - 1) / 8 : 2 * ((this.bitSize + 7) / 8);

        public virtual int GetOutputBlockSize() => this.forEncryption ? 2 * ((this.bitSize + 7) / 8) : (this.bitSize - 1) / 8;

        public virtual byte[] ProcessBlock( byte[] input, int inOff, int length )
        {
            if (this.key == null)
                throw new InvalidOperationException( "ElGamal engine not initialised" );
            int num = this.forEncryption ? (this.bitSize - 1 + 7) / 8 : this.GetInputBlockSize();
            if (length > num)
                throw new DataLengthException( "input too large for ElGamal cipher.\n" );
            BigInteger p = this.key.Parameters.P;
            byte[] numArray;
            if (this.key is ElGamalPrivateKeyParameters)
            {
                int length1 = length / 2;
                BigInteger bigInteger = new BigInteger( 1, input, inOff, length1 );
                BigInteger val = new BigInteger( 1, input, inOff + length1, length1 );
                ElGamalPrivateKeyParameters key = (ElGamalPrivateKeyParameters)this.key;
                numArray = bigInteger.ModPow( p.Subtract( BigInteger.One ).Subtract( key.X ), p ).Multiply( val ).Mod( p ).ToByteArrayUnsigned();
            }
            else
            {
                BigInteger bigInteger1 = new BigInteger( 1, input, inOff, length );
                if (bigInteger1.BitLength >= p.BitLength)
                    throw new DataLengthException( "input too large for ElGamal cipher.\n" );
                ElGamalPublicKeyParameters key = (ElGamalPublicKeyParameters)this.key;
                BigInteger bigInteger2 = p.Subtract( BigInteger.Two );
                BigInteger e;
                do
                {
                    e = new BigInteger( p.BitLength, random );
                }
                while (e.SignValue == 0 || e.CompareTo( bigInteger2 ) > 0);
                BigInteger bigInteger3 = this.key.Parameters.G.ModPow( e, p );
                BigInteger bigInteger4 = bigInteger1.Multiply( key.Y.ModPow( e, p ) ).Mod( p );
                numArray = new byte[this.GetOutputBlockSize()];
                byte[] byteArrayUnsigned1 = bigInteger3.ToByteArrayUnsigned();
                byte[] byteArrayUnsigned2 = bigInteger4.ToByteArrayUnsigned();
                byteArrayUnsigned1.CopyTo( numArray, (numArray.Length / 2) - byteArrayUnsigned1.Length );
                byteArrayUnsigned2.CopyTo( numArray, numArray.Length - byteArrayUnsigned2.Length );
            }
            return numArray;
        }
    }
}
