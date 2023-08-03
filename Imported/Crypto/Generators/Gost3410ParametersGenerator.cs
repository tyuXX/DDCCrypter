// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Gost3410ParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Gost3410ParametersGenerator
    {
        private int size;
        private int typeproc;
        private SecureRandom init_random;

        public void Init( int size, int typeProcedure, SecureRandom random )
        {
            this.size = size;
            this.typeproc = typeProcedure;
            this.init_random = random;
        }

        private int procedure_A( int x0, int c, BigInteger[] pq, int size )
        {
            while (x0 < 0 || x0 > 65536)
                x0 = this.init_random.NextInt() / 32768;
            while (c < 0 || c > 65536 || c / 2 == 0)
                c = (this.init_random.NextInt() / 32768) + 1;
            BigInteger bigInteger1 = BigInteger.ValueOf( c );
            BigInteger val = BigInteger.ValueOf( 19381L );
            BigInteger[] bigIntegerArray1 = new BigInteger[1]
            {
        BigInteger.ValueOf( x0)
            };
            int[] numArray1 = new int[1] { size };
            int index1 = 0;
            for (int index2 = 0; numArray1[index2] >= 17; ++index2)
            {
                int[] numArray2 = new int[numArray1.Length + 1];
                Array.Copy( numArray1, 0, numArray2, 0, numArray1.Length );
                numArray1 = new int[numArray2.Length];
                Array.Copy( numArray2, 0, numArray1, 0, numArray2.Length );
                numArray1[index2 + 1] = numArray1[index2] / 2;
                index1 = index2 + 1;
            }
            BigInteger[] bigIntegerArray2 = new BigInteger[index1 + 1];
            bigIntegerArray2[index1] = new BigInteger( "8003", 16 );
            int index3 = index1 - 1;
            for (int index4 = 0; index4 < index1; ++index4)
            {
                int index5 = numArray1[index3] / 16;
            label_10:
                BigInteger[] bigIntegerArray3 = new BigInteger[bigIntegerArray1.Length];
                Array.Copy( bigIntegerArray1, 0, bigIntegerArray3, 0, bigIntegerArray1.Length );
                bigIntegerArray1 = new BigInteger[index5 + 1];
                Array.Copy( bigIntegerArray3, 0, bigIntegerArray1, 0, bigIntegerArray3.Length );
                for (int index6 = 0; index6 < index5; ++index6)
                    bigIntegerArray1[index6 + 1] = bigIntegerArray1[index6].Multiply( val ).Add( bigInteger1 ).Mod( BigInteger.Two.Pow( 16 ) );
                BigInteger bigInteger2 = BigInteger.Zero;
                for (int index7 = 0; index7 < index5; ++index7)
                    bigInteger2 = bigInteger2.Add( bigIntegerArray1[index7].ShiftLeft( 16 * index7 ) );
                bigIntegerArray1[0] = bigIntegerArray1[index5];
                BigInteger e1 = BigInteger.One.ShiftLeft( numArray1[index3] - 1 ).Divide( bigIntegerArray2[index3 + 1] ).Add( bigInteger2.ShiftLeft( numArray1[index3] - 1 ).Divide( bigIntegerArray2[index3 + 1].ShiftLeft( 16 * index5 ) ) );
                if (e1.TestBit( 0 ))
                    e1 = e1.Add( BigInteger.One );
                while (true)
                {
                    BigInteger e2 = e1.Multiply( bigIntegerArray2[index3 + 1] );
                    if (e2.BitLength <= numArray1[index3])
                    {
                        bigIntegerArray2[index3] = e2.Add( BigInteger.One );
                        if (BigInteger.Two.ModPow( e2, bigIntegerArray2[index3] ).CompareTo( BigInteger.One ) != 0 || BigInteger.Two.ModPow( e1, bigIntegerArray2[index3] ).CompareTo( BigInteger.One ) == 0)
                            e1 = e1.Add( BigInteger.Two );
                        else
                            break;
                    }
                    else
                        goto label_10;
                }
                if (--index3 < 0)
                {
                    pq[0] = bigIntegerArray2[0];
                    pq[1] = bigIntegerArray2[1];
                    return bigIntegerArray1[0].IntValue;
                }
            }
            return bigIntegerArray1[0].IntValue;
        }

        private long procedure_Aa( long x0, long c, BigInteger[] pq, int size )
        {
            while (x0 < 0L || x0 > 4294967296L)
                x0 = this.init_random.NextInt() * 2;
            while (c < 0L || c > 4294967296L || c / 2L == 0L)
                c = (this.init_random.NextInt() * 2) + 1;
            BigInteger bigInteger1 = BigInteger.ValueOf( c );
            BigInteger val = BigInteger.ValueOf( 97781173L );
            BigInteger[] bigIntegerArray1 = new BigInteger[1]
            {
        BigInteger.ValueOf(x0)
            };
            int[] numArray1 = new int[1] { size };
            int index1 = 0;
            for (int index2 = 0; numArray1[index2] >= 33; ++index2)
            {
                int[] numArray2 = new int[numArray1.Length + 1];
                Array.Copy( numArray1, 0, numArray2, 0, numArray1.Length );
                numArray1 = new int[numArray2.Length];
                Array.Copy( numArray2, 0, numArray1, 0, numArray2.Length );
                numArray1[index2 + 1] = numArray1[index2] / 2;
                index1 = index2 + 1;
            }
            BigInteger[] bigIntegerArray2 = new BigInteger[index1 + 1];
            bigIntegerArray2[index1] = new BigInteger( "8000000B", 16 );
            int index3 = index1 - 1;
            for (int index4 = 0; index4 < index1; ++index4)
            {
                int index5 = numArray1[index3] / 32;
            label_10:
                BigInteger[] bigIntegerArray3 = new BigInteger[bigIntegerArray1.Length];
                Array.Copy( bigIntegerArray1, 0, bigIntegerArray3, 0, bigIntegerArray1.Length );
                bigIntegerArray1 = new BigInteger[index5 + 1];
                Array.Copy( bigIntegerArray3, 0, bigIntegerArray1, 0, bigIntegerArray3.Length );
                for (int index6 = 0; index6 < index5; ++index6)
                    bigIntegerArray1[index6 + 1] = bigIntegerArray1[index6].Multiply( val ).Add( bigInteger1 ).Mod( BigInteger.Two.Pow( 32 ) );
                BigInteger bigInteger2 = BigInteger.Zero;
                for (int index7 = 0; index7 < index5; ++index7)
                    bigInteger2 = bigInteger2.Add( bigIntegerArray1[index7].ShiftLeft( 32 * index7 ) );
                bigIntegerArray1[0] = bigIntegerArray1[index5];
                BigInteger e1 = BigInteger.One.ShiftLeft( numArray1[index3] - 1 ).Divide( bigIntegerArray2[index3 + 1] ).Add( bigInteger2.ShiftLeft( numArray1[index3] - 1 ).Divide( bigIntegerArray2[index3 + 1].ShiftLeft( 32 * index5 ) ) );
                if (e1.TestBit( 0 ))
                    e1 = e1.Add( BigInteger.One );
                while (true)
                {
                    BigInteger e2 = e1.Multiply( bigIntegerArray2[index3 + 1] );
                    if (e2.BitLength <= numArray1[index3])
                    {
                        bigIntegerArray2[index3] = e2.Add( BigInteger.One );
                        if (BigInteger.Two.ModPow( e2, bigIntegerArray2[index3] ).CompareTo( BigInteger.One ) != 0 || BigInteger.Two.ModPow( e1, bigIntegerArray2[index3] ).CompareTo( BigInteger.One ) == 0)
                            e1 = e1.Add( BigInteger.Two );
                        else
                            break;
                    }
                    else
                        goto label_10;
                }
                if (--index3 < 0)
                {
                    pq[0] = bigIntegerArray2[0];
                    pq[1] = bigIntegerArray2[1];
                    return bigIntegerArray1[0].LongValue;
                }
            }
            return bigIntegerArray1[0].LongValue;
        }

        private void procedure_B( int x0, int c, BigInteger[] pq )
        {
            while (x0 < 0 || x0 > 65536)
                x0 = this.init_random.NextInt() / 32768;
            while (c < 0 || c > 65536 || c / 2 == 0)
                c = (this.init_random.NextInt() / 32768) + 1;
            BigInteger[] pq1 = new BigInteger[2];
            BigInteger bigInteger1 = BigInteger.ValueOf( c );
            BigInteger val1 = BigInteger.ValueOf( 19381L );
            x0 = this.procedure_A( x0, c, pq1, 256 );
            BigInteger bigInteger2 = pq1[0];
            x0 = this.procedure_A( x0, c, pq1, 512 );
            BigInteger val2 = pq1[0];
            BigInteger[] bigIntegerArray = new BigInteger[65];
            bigIntegerArray[0] = BigInteger.ValueOf( x0 );
            BigInteger val3 = bigInteger2.Multiply( val2 );
        label_6:
            for (int index = 0; index < 64; ++index)
                bigIntegerArray[index + 1] = bigIntegerArray[index].Multiply( val1 ).Add( bigInteger1 ).Mod( BigInteger.Two.Pow( 16 ) );
            BigInteger bigInteger3 = BigInteger.Zero;
            for (int index = 0; index < 64; ++index)
                bigInteger3 = bigInteger3.Add( bigIntegerArray[index].ShiftLeft( 16 * index ) );
            bigIntegerArray[0] = bigIntegerArray[64];
            BigInteger val4 = BigInteger.One.ShiftLeft( 1023 ).Divide( val3 ).Add( bigInteger3.ShiftLeft( 1023 ).Divide( val3.ShiftLeft( 1024 ) ) );
            if (val4.TestBit( 0 ))
                val4 = val4.Add( BigInteger.One );
            BigInteger m;
            while (true)
            {
                BigInteger e = val3.Multiply( val4 );
                if (e.BitLength <= 1024)
                {
                    m = e.Add( BigInteger.One );
                    if (BigInteger.Two.ModPow( e, m ).CompareTo( BigInteger.One ) != 0 || BigInteger.Two.ModPow( bigInteger2.Multiply( val4 ), m ).CompareTo( BigInteger.One ) == 0)
                        val4 = val4.Add( BigInteger.Two );
                    else
                        break;
                }
                else
                    goto label_6;
            }
            pq[0] = m;
            pq[1] = bigInteger2;
        }

        private void procedure_Bb( long x0, long c, BigInteger[] pq )
        {
            while (x0 < 0L || x0 > 4294967296L)
                x0 = this.init_random.NextInt() * 2;
            while (c < 0L || c > 4294967296L || c / 2L == 0L)
                c = (this.init_random.NextInt() * 2) + 1;
            BigInteger[] pq1 = new BigInteger[2];
            BigInteger bigInteger1 = BigInteger.ValueOf( c );
            BigInteger val1 = BigInteger.ValueOf( 97781173L );
            x0 = this.procedure_Aa( x0, c, pq1, 256 );
            BigInteger bigInteger2 = pq1[0];
            x0 = this.procedure_Aa( x0, c, pq1, 512 );
            BigInteger val2 = pq1[0];
            BigInteger[] bigIntegerArray = new BigInteger[33];
            bigIntegerArray[0] = BigInteger.ValueOf( x0 );
            BigInteger val3 = bigInteger2.Multiply( val2 );
        label_6:
            for (int index = 0; index < 32; ++index)
                bigIntegerArray[index + 1] = bigIntegerArray[index].Multiply( val1 ).Add( bigInteger1 ).Mod( BigInteger.Two.Pow( 32 ) );
            BigInteger bigInteger3 = BigInteger.Zero;
            for (int index = 0; index < 32; ++index)
                bigInteger3 = bigInteger3.Add( bigIntegerArray[index].ShiftLeft( 32 * index ) );
            bigIntegerArray[0] = bigIntegerArray[32];
            BigInteger val4 = BigInteger.One.ShiftLeft( 1023 ).Divide( val3 ).Add( bigInteger3.ShiftLeft( 1023 ).Divide( val3.ShiftLeft( 1024 ) ) );
            if (val4.TestBit( 0 ))
                val4 = val4.Add( BigInteger.One );
            BigInteger m;
            while (true)
            {
                BigInteger e = val3.Multiply( val4 );
                if (e.BitLength <= 1024)
                {
                    m = e.Add( BigInteger.One );
                    if (BigInteger.Two.ModPow( e, m ).CompareTo( BigInteger.One ) != 0 || BigInteger.Two.ModPow( bigInteger2.Multiply( val4 ), m ).CompareTo( BigInteger.One ) == 0)
                        val4 = val4.Add( BigInteger.Two );
                    else
                        break;
                }
                else
                    goto label_6;
            }
            pq[0] = m;
            pq[1] = bigInteger2;
        }

        private BigInteger procedure_C( BigInteger p, BigInteger q )
        {
            BigInteger bigInteger1 = p.Subtract( BigInteger.One );
            BigInteger e = bigInteger1.Divide( q );
            BigInteger bigInteger2;
            do
            {
                BigInteger bigInteger3;
                do
                {
                    bigInteger3 = new BigInteger( p.BitLength, init_random );
                }
                while (bigInteger3.CompareTo( BigInteger.One ) <= 0 || bigInteger3.CompareTo( bigInteger1 ) >= 0);
                bigInteger2 = bigInteger3.ModPow( e, p );
            }
            while (bigInteger2.CompareTo( BigInteger.One ) == 0);
            return bigInteger2;
        }

        public Gost3410Parameters GenerateParameters()
        {
            BigInteger[] pq = new BigInteger[2];
            if (this.typeproc == 1)
            {
                int x0 = this.init_random.NextInt();
                int c = this.init_random.NextInt();
                switch (this.size)
                {
                    case 512:
                        this.procedure_A( x0, c, pq, 512 );
                        break;
                    case 1024:
                        this.procedure_B( x0, c, pq );
                        break;
                    default:
                        throw new ArgumentException( "Ooops! key size 512 or 1024 bit." );
                }
                BigInteger p = pq[0];
                BigInteger q = pq[1];
                BigInteger a = this.procedure_C( p, q );
                return new Gost3410Parameters( p, q, a, new Gost3410ValidationParameters( x0, c ) );
            }
            long num1 = this.init_random.NextLong();
            long num2 = this.init_random.NextLong();
            switch (this.size)
            {
                case 512:
                    this.procedure_Aa( num1, num2, pq, 512 );
                    break;
                case 1024:
                    this.procedure_Bb( num1, num2, pq );
                    break;
                default:
                    throw new InvalidOperationException( "Ooops! key size 512 or 1024 bit." );
            }
            BigInteger p1 = pq[0];
            BigInteger q1 = pq[1];
            BigInteger a1 = this.procedure_C( p1, q1 );
            return new Gost3410Parameters( p1, q1, a1, new Gost3410ValidationParameters( num1, num2 ) );
        }
    }
}
