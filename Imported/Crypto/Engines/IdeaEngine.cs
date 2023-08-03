// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.IdeaEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class IdeaEngine : IBlockCipher
    {
        private const int BLOCK_SIZE = 8;
        private int[] workingKey;
        private static readonly int MASK = ushort.MaxValue;
        private static readonly int BASE = 65537;

        public virtual void Init( bool forEncryption, ICipherParameters parameters ) => this.workingKey = parameters is KeyParameter ? this.GenerateWorkingKey( forEncryption, ((KeyParameter)parameters).GetKey() ) : throw new ArgumentException( "invalid parameter passed to IDEA init - " + Platform.GetTypeName( parameters ) );

        public virtual string AlgorithmName => "IDEA";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 8;

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.workingKey == null)
                throw new InvalidOperationException( "IDEA engine not initialised" );
            Check.DataLength( input, inOff, 8, "input buffer too short" );
            Check.OutputLength( output, outOff, 8, "output buffer too short" );
            this.IdeaFunc( this.workingKey, input, inOff, output, outOff );
            return 8;
        }

        public virtual void Reset()
        {
        }

        private int BytesToWord( byte[] input, int inOff ) => ((input[inOff] << 8) & 65280) + (input[inOff + 1] & byte.MaxValue);

        private void WordToBytes( int word, byte[] outBytes, int outOff )
        {
            outBytes[outOff] = (byte)(word >>> 8);
            outBytes[outOff + 1] = (byte)word;
        }

        private int Mul( int x, int y )
        {
            if (x == 0)
                x = BASE - y;
            else if (y == 0)
            {
                x = BASE - x;
            }
            else
            {
                int num = x * y;
                y = num & MASK;
                x = num >>> 16;
                x = y - x + (y < x ? 1 : 0);
            }
            return x & MASK;
        }

        private void IdeaFunc( int[] workingKey, byte[] input, int inOff, byte[] outBytes, int outOff )
        {
            int num1 = 0;
            int num2 = this.BytesToWord( input, inOff );
            int num3 = this.BytesToWord( input, inOff + 2 );
            int num4 = this.BytesToWord( input, inOff + 4 );
            int x1 = this.BytesToWord( input, inOff + 6 );
            for (int index1 = 0; index1 < 8; ++index1)
            {
                int x2 = num2;
                int[] numArray1 = workingKey;
                int index2 = num1;
                int num5 = index2 + 1;
                int y1 = numArray1[index2];
                int num6 = this.Mul( x2, y1 );
                int num7 = num3;
                int[] numArray2 = workingKey;
                int index3 = num5;
                int num8 = index3 + 1;
                int num9 = numArray2[index3];
                int num10 = (num7 + num9) & MASK;
                int num11 = num4;
                int[] numArray3 = workingKey;
                int index4 = num8;
                int num12 = index4 + 1;
                int num13 = numArray3[index4];
                int num14 = (num11 + num13) & MASK;
                int x3 = x1;
                int[] numArray4 = workingKey;
                int index5 = num12;
                int num15 = index5 + 1;
                int y2 = numArray4[index5];
                int num16 = this.Mul( x3, y2 );
                int num17 = num10;
                int num18 = num14;
                int num19 = num14 ^ num6;
                int num20 = num10 ^ num16;
                int x4 = num19;
                int[] numArray5 = workingKey;
                int index6 = num15;
                int num21 = index6 + 1;
                int y3 = numArray5[index6];
                int num22 = this.Mul( x4, y3 );
                int x5 = (num20 + num22) & MASK;
                int[] numArray6 = workingKey;
                int index7 = num21;
                num1 = index7 + 1;
                int y4 = numArray6[index7];
                int num23 = this.Mul( x5, y4 );
                int num24 = (num22 + num23) & MASK;
                num2 = num6 ^ num23;
                x1 = num16 ^ num24;
                num3 = num23 ^ num18;
                num4 = num24 ^ num17;
            }
            int x6 = num2;
            int[] numArray7 = workingKey;
            int index8 = num1;
            int num25 = index8 + 1;
            int y = numArray7[index8];
            this.WordToBytes( this.Mul( x6, y ), outBytes, outOff );
            int num26 = num4;
            int[] numArray8 = workingKey;
            int index9 = num25;
            int num27 = index9 + 1;
            int num28 = numArray8[index9];
            this.WordToBytes( num26 + num28, outBytes, outOff + 2 );
            int num29 = num3;
            int[] numArray9 = workingKey;
            int index10 = num27;
            int index11 = index10 + 1;
            int num30 = numArray9[index10];
            this.WordToBytes( num29 + num30, outBytes, outOff + 4 );
            this.WordToBytes( this.Mul( x1, workingKey[index11] ), outBytes, outOff + 6 );
        }

        private int[] ExpandKey( byte[] uKey )
        {
            int[] numArray = new int[52];
            if (uKey.Length < 16)
            {
                byte[] destinationArray = new byte[16];
                Array.Copy( uKey, 0, destinationArray, destinationArray.Length - uKey.Length, uKey.Length );
                uKey = destinationArray;
            }
            for (int index = 0; index < 8; ++index)
                numArray[index] = this.BytesToWord( uKey, index * 2 );
            for (int index = 8; index < 52; ++index)
                numArray[index] = (index & 7) >= 6 ? ((index & 7) != 6 ? (((numArray[index - 15] & sbyte.MaxValue) << 9) | (numArray[index - 14] >> 7)) & MASK : (((numArray[index - 7] & sbyte.MaxValue) << 9) | (numArray[index - 14] >> 7)) & MASK) : (((numArray[index - 7] & sbyte.MaxValue) << 9) | (numArray[index - 6] >> 7)) & MASK;
            return numArray;
        }

        private int MulInv( int x )
        {
            if (x < 2)
                return x;
            int num1 = 1;
            int num2 = BASE / x;
            int num3 = BASE % x;
            while (num3 != 1)
            {
                int num4 = x / num3;
                x %= num3;
                num1 = (num1 + (num2 * num4)) & MASK;
                if (x == 1)
                    return num1;
                int num5 = num3 / x;
                num3 %= x;
                num2 = (num2 + (num1 * num5)) & MASK;
            }
            return (1 - num2) & MASK;
        }

        private int AddInv( int x ) => -x & MASK;

        private int[] InvertKey( int[] inKey )
        {
            int num1 = 52;
            int[] numArray1 = new int[52];
            int num2 = 0;
            int[] numArray2 = inKey;
            int index1 = num2;
            int num3 = index1 + 1;
            int num4 = this.MulInv( numArray2[index1] );
            int[] numArray3 = inKey;
            int index2 = num3;
            int num5 = index2 + 1;
            int num6 = this.AddInv( numArray3[index2] );
            int[] numArray4 = inKey;
            int index3 = num5;
            int num7 = index3 + 1;
            int num8 = this.AddInv( numArray4[index3] );
            int[] numArray5 = inKey;
            int index4 = num7;
            int num9 = index4 + 1;
            int num10 = this.MulInv( numArray5[index4] );
            int num11;
            numArray1[num11 = num1 - 1] = num10;
            int num12;
            numArray1[num12 = num11 - 1] = num8;
            int num13;
            numArray1[num13 = num12 - 1] = num6;
            int num14;
            numArray1[num14 = num13 - 1] = num4;
            for (int index5 = 1; index5 < 8; ++index5)
            {
                int[] numArray6 = inKey;
                int index6 = num9;
                int num15 = index6 + 1;
                int num16 = numArray6[index6];
                int[] numArray7 = inKey;
                int index7 = num15;
                int num17 = index7 + 1;
                int num18 = numArray7[index7];
                int num19;
                numArray1[num19 = num14 - 1] = num18;
                int num20;
                numArray1[num20 = num19 - 1] = num16;
                int[] numArray8 = inKey;
                int index8 = num17;
                int num21 = index8 + 1;
                int num22 = this.MulInv( numArray8[index8] );
                int[] numArray9 = inKey;
                int index9 = num21;
                int num23 = index9 + 1;
                int num24 = this.AddInv( numArray9[index9] );
                int[] numArray10 = inKey;
                int index10 = num23;
                int num25 = index10 + 1;
                int num26 = this.AddInv( numArray10[index10] );
                int[] numArray11 = inKey;
                int index11 = num25;
                num9 = index11 + 1;
                int num27 = this.MulInv( numArray11[index11] );
                int num28;
                numArray1[num28 = num20 - 1] = num27;
                int num29;
                numArray1[num29 = num28 - 1] = num24;
                int num30;
                numArray1[num30 = num29 - 1] = num26;
                numArray1[num14 = num30 - 1] = num22;
            }
            int[] numArray12 = inKey;
            int index12 = num9;
            int num31 = index12 + 1;
            int num32 = numArray12[index12];
            int[] numArray13 = inKey;
            int index13 = num31;
            int num33 = index13 + 1;
            int num34 = numArray13[index13];
            int num35;
            numArray1[num35 = num14 - 1] = num34;
            int num36;
            numArray1[num36 = num35 - 1] = num32;
            int[] numArray14 = inKey;
            int index14 = num33;
            int num37 = index14 + 1;
            int num38 = this.MulInv( numArray14[index14] );
            int[] numArray15 = inKey;
            int index15 = num37;
            int num39 = index15 + 1;
            int num40 = this.AddInv( numArray15[index15] );
            int[] numArray16 = inKey;
            int index16 = num39;
            int index17 = index16 + 1;
            int num41 = this.AddInv( numArray16[index16] );
            int num42 = this.MulInv( inKey[index17] );
            int num43;
            numArray1[num43 = num36 - 1] = num42;
            int num44;
            numArray1[num44 = num43 - 1] = num41;
            int num45;
            numArray1[num45 = num44 - 1] = num40;
            int num46;
            numArray1[num46 = num45 - 1] = num38;
            return numArray1;
        }

        private int[] GenerateWorkingKey( bool forEncryption, byte[] userKey ) => forEncryption ? this.ExpandKey( userKey ) : this.InvertKey( this.ExpandKey( userKey ) );
    }
}
