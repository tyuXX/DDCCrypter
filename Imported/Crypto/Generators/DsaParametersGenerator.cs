// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.DsaParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class DsaParametersGenerator
    {
        private IDigest digest;
        private int L;
        private int N;
        private int certainty;
        private SecureRandom random;
        private bool use186_3;
        private int usageIndex;

        public DsaParametersGenerator()
          : this( new Sha1Digest() )
        {
        }

        public DsaParametersGenerator( IDigest digest ) => this.digest = digest;

        public virtual void Init( int size, int certainty, SecureRandom random )
        {
            if (!IsValidDsaStrength( size ))
                throw new ArgumentException( "size must be from 512 - 1024 and a multiple of 64", nameof( size ) );
            this.use186_3 = false;
            this.L = size;
            this.N = GetDefaultN( size );
            this.certainty = certainty;
            this.random = random;
        }

        public virtual void Init( DsaParameterGenerationParameters parameters )
        {
            this.use186_3 = true;
            this.L = parameters.L;
            this.N = parameters.N;
            this.certainty = parameters.Certainty;
            this.random = parameters.Random;
            this.usageIndex = parameters.UsageIndex;
            if (this.L < 1024 || this.L > 3072 || this.L % 1024 != 0)
                throw new ArgumentException( "Values must be between 1024 and 3072 and a multiple of 1024", "L" );
            if (this.L == 1024 && this.N != 160)
                throw new ArgumentException( "N must be 160 for L = 1024" );
            if (this.L == 2048 && this.N != 224 && this.N != 256)
                throw new ArgumentException( "N must be 224 or 256 for L = 2048" );
            if (this.L == 3072 && this.N != 256)
                throw new ArgumentException( "N must be 256 for L = 3072" );
            if (this.digest.GetDigestSize() * 8 < this.N)
                throw new InvalidOperationException( "Digest output size too small for value of N" );
        }

        public virtual DsaParameters GenerateParameters() => !this.use186_3 ? this.GenerateParameters_FIPS186_2() : this.GenerateParameters_FIPS186_3();

        protected virtual DsaParameters GenerateParameters_FIPS186_2()
        {
            byte[] numArray1 = new byte[20];
            byte[] numArray2 = new byte[20];
            byte[] numArray3 = new byte[20];
            byte[] bytes = new byte[20];
            int num = (this.L - 1) / 160;
            byte[] numArray4 = new byte[this.L / 8];
            if (!(this.digest is Sha1Digest))
                throw new InvalidOperationException( "can only use SHA-1 for generating FIPS 186-2 parameters" );
            label_2:
            BigInteger q;
            do
            {
                this.random.NextBytes( numArray1 );
                Hash( this.digest, numArray1, numArray2 );
                Array.Copy( numArray1, 0, numArray3, 0, numArray1.Length );
                Inc( numArray3 );
                Hash( this.digest, numArray3, numArray3 );
                for (int index = 0; index != bytes.Length; ++index)
                    bytes[index] = (byte)(numArray2[index] ^ (uint)numArray3[index]);
                byte[] numArray5;
                (numArray5 = bytes)[0] = (byte)(numArray5[0] | 128U);
                byte[] numArray6;
                (numArray6 = bytes)[19] = (byte)(numArray6[19] | 1U);
                q = new BigInteger( 1, bytes );
            }
            while (!q.IsProbablePrime( this.certainty ));
            byte[] numArray7 = Arrays.Clone( numArray1 );
            Inc( numArray7 );
            for (int counter = 0; counter < 4096; ++counter)
            {
                for (int index = 0; index < num; ++index)
                {
                    Inc( numArray7 );
                    Hash( this.digest, numArray7, numArray2 );
                    Array.Copy( numArray2, 0, numArray4, numArray4.Length - ((index + 1) * numArray2.Length), numArray2.Length );
                }
                Inc( numArray7 );
                Hash( this.digest, numArray7, numArray2 );
                Array.Copy( numArray2, numArray2.Length - (numArray4.Length - (num * numArray2.Length)), numArray4, 0, numArray4.Length - (num * numArray2.Length) );
                byte[] numArray8;
                (numArray8 = numArray4)[0] = (byte)(numArray8[0] | 128U);
                BigInteger bigInteger1 = new BigInteger( 1, numArray4 );
                BigInteger bigInteger2 = bigInteger1.Mod( q.ShiftLeft( 1 ) );
                BigInteger p = bigInteger1.Subtract( bigInteger2.Subtract( BigInteger.One ) );
                if (p.BitLength == this.L && p.IsProbablePrime( this.certainty ))
                {
                    BigInteger generatorFipS1862 = this.CalculateGenerator_FIPS186_2( p, q, this.random );
                    return new DsaParameters( p, q, generatorFipS1862, new DsaValidationParameters( numArray1, counter ) );
                }
            }
            goto label_2;
        }

        protected virtual BigInteger CalculateGenerator_FIPS186_2(
          BigInteger p,
          BigInteger q,
          SecureRandom r )
        {
            BigInteger e = p.Subtract( BigInteger.One ).Divide( q );
            BigInteger max = p.Subtract( BigInteger.Two );
            BigInteger generatorFipS1862;
            do
            {
                generatorFipS1862 = BigIntegers.CreateRandomInRange( BigInteger.Two, max, r ).ModPow( e, p );
            }
            while (generatorFipS1862.BitLength <= 1);
            return generatorFipS1862;
        }

        protected virtual DsaParameters GenerateParameters_FIPS186_3()
        {
            IDigest digest = this.digest;
            int num1 = digest.GetDigestSize() * 8;
            byte[] numArray1 = new byte[this.N / 8];
            int num2 = (this.L - 1) / num1;
            int n1 = (this.L - 1) % num1;
            byte[] numArray2 = new byte[digest.GetDigestSize()];
        label_1:
            BigInteger q;
            do
            {
                this.random.NextBytes( numArray1 );
                Hash( digest, numArray1, numArray2 );
                q = new BigInteger( 1, numArray2 ).Mod( BigInteger.One.ShiftLeft( this.N - 1 ) ).SetBit( 0 ).SetBit( this.N - 1 );
            }
            while (!q.IsProbablePrime( this.certainty ));
            byte[] numArray3 = Arrays.Clone( numArray1 );
            int num3 = 4 * this.L;
            for (int counter = 0; counter < num3; ++counter)
            {
                BigInteger bigInteger1 = BigInteger.Zero;
                int num4 = 0;
                int n2 = 0;
                while (num4 <= num2)
                {
                    Inc( numArray3 );
                    Hash( digest, numArray3, numArray2 );
                    BigInteger bigInteger2 = new BigInteger( 1, numArray2 );
                    if (num4 == num2)
                        bigInteger2 = bigInteger2.Mod( BigInteger.One.ShiftLeft( n1 ) );
                    bigInteger1 = bigInteger1.Add( bigInteger2.ShiftLeft( n2 ) );
                    ++num4;
                    n2 += num1;
                }
                BigInteger bigInteger3 = bigInteger1.Add( BigInteger.One.ShiftLeft( this.L - 1 ) );
                BigInteger bigInteger4 = bigInteger3.Mod( q.ShiftLeft( 1 ) );
                BigInteger p = bigInteger3.Subtract( bigInteger4.Subtract( BigInteger.One ) );
                if (p.BitLength == this.L && p.IsProbablePrime( this.certainty ))
                {
                    if (this.usageIndex >= 0)
                    {
                        BigInteger fipS1863Verifiable = this.CalculateGenerator_FIPS186_3_Verifiable( digest, p, q, numArray1, this.usageIndex );
                        if (fipS1863Verifiable != null)
                            return new DsaParameters( p, q, fipS1863Verifiable, new DsaValidationParameters( numArray1, counter, this.usageIndex ) );
                    }
                    BigInteger s1863Unverifiable = this.CalculateGenerator_FIPS186_3_Unverifiable( p, q, this.random );
                    return new DsaParameters( p, q, s1863Unverifiable, new DsaValidationParameters( numArray1, counter ) );
                }
            }
            goto label_1;
        }

        protected virtual BigInteger CalculateGenerator_FIPS186_3_Unverifiable(
          BigInteger p,
          BigInteger q,
          SecureRandom r )
        {
            return this.CalculateGenerator_FIPS186_2( p, q, r );
        }

        protected virtual BigInteger CalculateGenerator_FIPS186_3_Verifiable(
          IDigest d,
          BigInteger p,
          BigInteger q,
          byte[] seed,
          int index )
        {
            BigInteger e = p.Subtract( BigInteger.One ).Divide( q );
            byte[] sourceArray = Hex.Decode( "6767656E" );
            byte[] numArray1 = new byte[seed.Length + sourceArray.Length + 1 + 2];
            Array.Copy( seed, 0, numArray1, 0, seed.Length );
            Array.Copy( sourceArray, 0, numArray1, seed.Length, sourceArray.Length );
            numArray1[numArray1.Length - 3] = (byte)index;
            byte[] numArray2 = new byte[d.GetDigestSize()];
            for (int index1 = 1; index1 < 65536; ++index1)
            {
                Inc( numArray1 );
                Hash( d, numArray1, numArray2 );
                BigInteger fipS1863Verifiable = new BigInteger( 1, numArray2 ).ModPow( e, p );
                if (fipS1863Verifiable.CompareTo( BigInteger.Two ) >= 0)
                    return fipS1863Verifiable;
            }
            return null;
        }

        private static bool IsValidDsaStrength( int strength ) => strength >= 512 && strength <= 1024 && strength % 64 == 0;

        protected static void Hash( IDigest d, byte[] input, byte[] output )
        {
            d.BlockUpdate( input, 0, input.Length );
            d.DoFinal( output, 0 );
        }

        private static int GetDefaultN( int L ) => L <= 1024 ? 160 : 256;

        protected static void Inc( byte[] buf )
        {
            for (int index = buf.Length - 1; index >= 0; --index)
            {
                byte num = (byte)(buf[index] + 1U);
                buf[index] = num;
                if (num != 0)
                    break;
            }
        }
    }
}
