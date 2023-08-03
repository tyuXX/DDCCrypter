// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Apache.Bzip2.CBZip2InputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Apache.Bzip2
{
    public class CBZip2InputStream : Stream
    {
        private const int START_BLOCK_STATE = 1;
        private const int RAND_PART_A_STATE = 2;
        private const int RAND_PART_B_STATE = 3;
        private const int RAND_PART_C_STATE = 4;
        private const int NO_RAND_PART_A_STATE = 5;
        private const int NO_RAND_PART_B_STATE = 6;
        private const int NO_RAND_PART_C_STATE = 7;
        private int last;
        private int origPtr;
        private int blockSize100k;
        private bool blockRandomised;
        private int bsBuff;
        private int bsLive;
        private CRC mCrc = new();
        private bool[] inUse = new bool[256];
        private int nInUse;
        private char[] seqToUnseq = new char[256];
        private char[] unseqToSeq = new char[256];
        private char[] selector = new char[18002];
        private char[] selectorMtf = new char[18002];
        private int[] tt;
        private char[] ll8;
        private int[] unzftab = new int[256];
        private int[][] limit = InitIntArray( 6, 258 );
        private int[][] basev = InitIntArray( 6, 258 );
        private int[][] perm = InitIntArray( 6, 258 );
        private int[] minLens = new int[6];
        private Stream bsStream;
        private bool streamEnd = false;
        private int currentChar = -1;
        private int currentState = 1;
        private int storedBlockCRC;
        private int storedCombinedCRC;
        private int computedBlockCRC;
        private int computedCombinedCRC;
        private int i2;
        private int count;
        private int chPrev;
        private int ch2;
        private int i;
        private int tPos;
        private int rNToGo = 0;
        private int rTPos = 0;
        private int j2;
        private char z;

        private static void Cadvise()
        {
        }

        private static void CompressedStreamEOF() => Cadvise();

        private void MakeMaps()
        {
            this.nInUse = 0;
            for (int index = 0; index < 256; ++index)
            {
                if (this.inUse[index])
                {
                    this.seqToUnseq[this.nInUse] = (char)index;
                    this.unseqToSeq[index] = (char)this.nInUse;
                    ++this.nInUse;
                }
            }
        }

        public CBZip2InputStream( Stream zStream )
        {
            this.ll8 = null;
            this.tt = null;
            this.BsSetStream( zStream );
            this.Initialize();
            this.InitBlock();
            this.SetupBlock();
        }

        internal static int[][] InitIntArray( int n1, int n2 )
        {
            int[][] numArray = new int[n1][];
            for (int index = 0; index < n1; ++index)
                numArray[index] = new int[n2];
            return numArray;
        }

        internal static char[][] InitCharArray( int n1, int n2 )
        {
            char[][] chArray = new char[n1][];
            for (int index = 0; index < n1; ++index)
                chArray[index] = new char[n2];
            return chArray;
        }

        public override int ReadByte()
        {
            if (this.streamEnd)
                return -1;
            int currentChar = this.currentChar;
            switch (this.currentState)
            {
                case 3:
                    this.SetupRandPartB();
                    break;
                case 4:
                    this.SetupRandPartC();
                    break;
                case 6:
                    this.SetupNoRandPartB();
                    break;
                case 7:
                    this.SetupNoRandPartC();
                    break;
            }
            return currentChar;
        }

        private void Initialize()
        {
            char uchar1 = this.BsGetUChar();
            char uchar2 = this.BsGetUChar();
            if (uchar1 != 'B' && uchar2 != 'Z')
                throw new IOException( "Not a BZIP2 marked stream" );
            char uchar3 = this.BsGetUChar();
            char uchar4 = this.BsGetUChar();
            if (uchar3 != 'h' || uchar4 < '1' || uchar4 > '9')
            {
                this.BsFinishedWithStream();
                this.streamEnd = true;
            }
            else
            {
                this.SetDecompressStructureSizes( uchar4 - 48 );
                this.computedCombinedCRC = 0;
            }
        }

        private void InitBlock()
        {
            char uchar1 = this.BsGetUChar();
            char uchar2 = this.BsGetUChar();
            char uchar3 = this.BsGetUChar();
            char uchar4 = this.BsGetUChar();
            char uchar5 = this.BsGetUChar();
            char uchar6 = this.BsGetUChar();
            if (uchar1 == '\u0017' && uchar2 == 'r' && uchar3 == 'E' && uchar4 == '8' && uchar5 == 'P' && uchar6 == '\u0090')
                this.Complete();
            else if (uchar1 != '1' || uchar2 != 'A' || uchar3 != 'Y' || uchar4 != '&' || uchar5 != 'S' || uchar6 != 'Y')
            {
                BadBlockHeader();
                this.streamEnd = true;
            }
            else
            {
                this.storedBlockCRC = this.BsGetInt32();
                this.blockRandomised = this.BsR( 1 ) == 1;
                this.GetAndMoveToFrontDecode();
                this.mCrc.InitialiseCRC();
                this.currentState = 1;
            }
        }

        private void EndBlock()
        {
            this.computedBlockCRC = this.mCrc.GetFinalCRC();
            if (this.storedBlockCRC != this.computedBlockCRC)
                CrcError();
            this.computedCombinedCRC = (this.computedCombinedCRC << 1) | this.computedCombinedCRC >>> 31;
            this.computedCombinedCRC ^= this.computedBlockCRC;
        }

        private void Complete()
        {
            this.storedCombinedCRC = this.BsGetInt32();
            if (this.storedCombinedCRC != this.computedCombinedCRC)
                CrcError();
            this.BsFinishedWithStream();
            this.streamEnd = true;
        }

        private static void BlockOverrun() => Cadvise();

        private static void BadBlockHeader() => Cadvise();

        private static void CrcError() => Cadvise();

        private void BsFinishedWithStream()
        {
            try
            {
                if (this.bsStream == null)
                    return;
                Platform.Dispose( this.bsStream );
                this.bsStream = null;
            }
            catch
            {
            }
        }

        private void BsSetStream( Stream f )
        {
            this.bsStream = f;
            this.bsLive = 0;
            this.bsBuff = 0;
        }

        private int BsR( int n )
        {
            for (; this.bsLive < n; this.bsLive += 8)
            {
                char ch = char.MinValue;
                try
                {
                    ch = (char)this.bsStream.ReadByte();
                }
                catch (IOException ex)
                {
                    CompressedStreamEOF();
                }
                if (ch == char.MaxValue)
                    CompressedStreamEOF();
                this.bsBuff = (this.bsBuff << 8) | (ch & byte.MaxValue);
            }
            int num = (this.bsBuff >> (this.bsLive - n)) & ((1 << n) - 1);
            this.bsLive -= n;
            return num;
        }

        private char BsGetUChar() => (char)this.BsR( 8 );

        private int BsGetint() => (((((((0 << 8) | this.BsR( 8 )) << 8) | this.BsR( 8 )) << 8) | this.BsR( 8 )) << 8) | this.BsR( 8 );

        private int BsGetIntVS( int numBits ) => this.BsR( numBits );

        private int BsGetInt32() => this.BsGetint();

        private void HbCreateDecodeTables(
          int[] limit,
          int[] basev,
          int[] perm,
          char[] length,
          int minLen,
          int maxLen,
          int alphaSize )
        {
            int index1 = 0;
            for (int index2 = minLen; index2 <= maxLen; ++index2)
            {
                for (int index3 = 0; index3 < alphaSize; ++index3)
                {
                    if (length[index3] == index2)
                    {
                        perm[index1] = index3;
                        ++index1;
                    }
                }
            }
            for (int index4 = 0; index4 < 23; ++index4)
                basev[index4] = 0;
            for (int index5 = 0; index5 < alphaSize; ++index5)
                ++basev[length[index5] + 1];
            for (int index6 = 1; index6 < 23; ++index6)
                basev[index6] += basev[index6 - 1];
            for (int index7 = 0; index7 < 23; ++index7)
                limit[index7] = 0;
            int num1 = 0;
            for (int index8 = minLen; index8 <= maxLen; ++index8)
            {
                int num2 = num1 + (basev[index8 + 1] - basev[index8]);
                limit[index8] = num2 - 1;
                num1 = num2 << 1;
            }
            for (int index9 = minLen + 1; index9 <= maxLen; ++index9)
                basev[index9] = ((limit[index9 - 1] + 1) << 1) - basev[index9];
        }

        private void RecvDecodingTables()
        {
            char[][] chArray1 = InitCharArray( 6, 258 );
            bool[] flagArray = new bool[16];
            for (int index = 0; index < 16; ++index)
                flagArray[index] = this.BsR( 1 ) == 1;
            for (int index = 0; index < 256; ++index)
                this.inUse[index] = false;
            for (int index1 = 0; index1 < 16; ++index1)
            {
                if (flagArray[index1])
                {
                    for (int index2 = 0; index2 < 16; ++index2)
                    {
                        if (this.BsR( 1 ) == 1)
                            this.inUse[(index1 * 16) + index2] = true;
                    }
                }
            }
            this.MakeMaps();
            int alphaSize = this.nInUse + 2;
            int num1 = this.BsR( 3 );
            int num2 = this.BsR( 15 );
            for (int index = 0; index < num2; ++index)
            {
                int num3 = 0;
                while (this.BsR( 1 ) == 1)
                    ++num3;
                this.selectorMtf[index] = (char)num3;
            }
            char[] chArray2 = new char[6];
            for (char minValue = char.MinValue; minValue < num1; ++minValue)
                chArray2[minValue] = minValue;
            for (int index3 = 0; index3 < num2; ++index3)
            {
                char index4 = this.selectorMtf[index3];
                char ch = chArray2[index4];
                for (; index4 > char.MinValue; --index4)
                    chArray2[index4] = chArray2[index4 - 1];
                chArray2[0] = ch;
                this.selector[index3] = ch;
            }
            for (int index5 = 0; index5 < num1; ++index5)
            {
                int num4 = this.BsR( 5 );
                for (int index6 = 0; index6 < alphaSize; ++index6)
                {
                    while (this.BsR( 1 ) == 1)
                    {
                        if (this.BsR( 1 ) == 0)
                            ++num4;
                        else
                            --num4;
                    }
                    chArray1[index5][index6] = (char)num4;
                }
            }
            for (int index7 = 0; index7 < num1; ++index7)
            {
                int minLen = 32;
                int maxLen = 0;
                for (int index8 = 0; index8 < alphaSize; ++index8)
                {
                    if (chArray1[index7][index8] > maxLen)
                        maxLen = chArray1[index7][index8];
                    if (chArray1[index7][index8] < minLen)
                        minLen = chArray1[index7][index8];
                }
                this.HbCreateDecodeTables( this.limit[index7], this.basev[index7], this.perm[index7], chArray1[index7], minLen, maxLen, alphaSize );
                this.minLens[index7] = minLen;
            }
        }

        private void GetAndMoveToFrontDecode()
        {
            char[] chArray = new char[256];
            int num1 = 100000 * this.blockSize100k;
            this.origPtr = this.BsGetIntVS( 24 );
            this.RecvDecodingTables();
            int num2 = this.nInUse + 1;
            int index1 = -1;
            int num3 = 0;
            for (int index2 = 0; index2 <= byte.MaxValue; ++index2)
                this.unzftab[index2] = 0;
            for (int index3 = 0; index3 <= byte.MaxValue; ++index3)
                chArray[index3] = (char)index3;
            this.last = -1;
            if (num3 == 0)
            {
                ++index1;
                num3 = 50;
            }
            int num4 = num3 - 1;
            int index4 = this.selector[index1];
            int minLen1 = this.minLens[index4];
            int num5;
            int num6;
            for (num5 = this.BsR( minLen1 ); num5 > this.limit[index4][minLen1]; num5 = (num5 << 1) | num6)
            {
                ++minLen1;
                for (; this.bsLive < 1; this.bsLive += 8)
                {
                    char ch = char.MinValue;
                    try
                    {
                        ch = (char)this.bsStream.ReadByte();
                    }
                    catch (IOException ex)
                    {
                        CompressedStreamEOF();
                    }
                    if (ch == char.MaxValue)
                        CompressedStreamEOF();
                    this.bsBuff = (this.bsBuff << 8) | (ch & byte.MaxValue);
                }
                num6 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                --this.bsLive;
            }
            int num7 = this.perm[index4][num5 - this.basev[index4][minLen1]];
            while (num7 != num2)
            {
                if (num7 == 0 || num7 == 1)
                {
                    int num8 = -1;
                    int num9 = 1;
                label_23:
                    if (num7 == 0)
                        num8 += num9;
                    else if (num7 == 1)
                        num8 += 2 * num9;
                    num9 *= 2;
                    if (num4 == 0)
                    {
                        ++index1;
                        num4 = 50;
                    }
                    --num4;
                    int index5 = this.selector[index1];
                    int minLen2 = this.minLens[index5];
                    int num10;
                    int num11;
                    for (num10 = this.BsR( minLen2 ); num10 > this.limit[index5][minLen2]; num10 = (num10 << 1) | num11)
                    {
                        ++minLen2;
                        for (; this.bsLive < 1; this.bsLive += 8)
                        {
                            char ch = char.MinValue;
                            try
                            {
                                ch = (char)this.bsStream.ReadByte();
                            }
                            catch (IOException ex)
                            {
                                CompressedStreamEOF();
                            }
                            if (ch == char.MaxValue)
                                CompressedStreamEOF();
                            this.bsBuff = (this.bsBuff << 8) | (ch & byte.MaxValue);
                        }
                        num11 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                        --this.bsLive;
                    }
                    num7 = this.perm[index5][num10 - this.basev[index5][minLen2]];
                    switch (num7)
                    {
                        case 0:
                        case 1:
                            goto label_23;
                        default:
                            int num12 = num8 + 1;
                            char index6 = this.seqToUnseq[chArray[0]];
                            this.unzftab[index6] += num12;
                            for (; num12 > 0; --num12)
                            {
                                ++this.last;
                                this.ll8[this.last] = index6;
                            }
                            if (this.last >= num1)
                            {
                                BlockOverrun();
                                continue;
                            }
                            continue;
                    }
                }
                else
                {
                    ++this.last;
                    if (this.last >= num1)
                        BlockOverrun();
                    char index7 = chArray[num7 - 1];
                    ++this.unzftab[this.seqToUnseq[index7]];
                    this.ll8[this.last] = this.seqToUnseq[index7];
                    int index8;
                    for (index8 = num7 - 1; index8 > 3; index8 -= 4)
                    {
                        chArray[index8] = chArray[index8 - 1];
                        chArray[index8 - 1] = chArray[index8 - 2];
                        chArray[index8 - 2] = chArray[index8 - 3];
                        chArray[index8 - 3] = chArray[index8 - 4];
                    }
                    for (; index8 > 0; --index8)
                        chArray[index8] = chArray[index8 - 1];
                    chArray[0] = index7;
                    if (num4 == 0)
                    {
                        ++index1;
                        num4 = 50;
                    }
                    --num4;
                    int index9 = this.selector[index1];
                    int minLen3 = this.minLens[index9];
                    int num13;
                    int num14;
                    for (num13 = this.BsR( minLen3 ); num13 > this.limit[index9][minLen3]; num13 = (num13 << 1) | num14)
                    {
                        ++minLen3;
                        for (; this.bsLive < 1; this.bsLive += 8)
                        {
                            char ch = char.MinValue;
                            try
                            {
                                ch = (char)this.bsStream.ReadByte();
                            }
                            catch (IOException ex)
                            {
                                CompressedStreamEOF();
                            }
                            this.bsBuff = (this.bsBuff << 8) | (ch & byte.MaxValue);
                        }
                        num14 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                        --this.bsLive;
                    }
                    num7 = this.perm[index9][num13 - this.basev[index9][minLen3]];
                }
            }
        }

        private void SetupBlock()
        {
            int[] numArray = new int[257];
            numArray[0] = 0;
            for (this.i = 1; this.i <= 256; ++this.i)
                numArray[this.i] = this.unzftab[this.i - 1];
            for (this.i = 1; this.i <= 256; ++this.i)
                numArray[this.i] += numArray[this.i - 1];
            for (this.i = 0; this.i <= this.last; ++this.i)
            {
                char index = this.ll8[this.i];
                this.tt[numArray[index]] = this.i;
                ++numArray[index];
            }
            this.tPos = this.tt[this.origPtr];
            this.count = 0;
            this.i2 = 0;
            this.ch2 = 256;
            if (this.blockRandomised)
            {
                this.rNToGo = 0;
                this.rTPos = 0;
                this.SetupRandPartA();
            }
            else
                this.SetupNoRandPartA();
        }

        private void SetupRandPartA()
        {
            if (this.i2 <= this.last)
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                if (this.rNToGo == 0)
                {
                    this.rNToGo = BZip2Constants.rNums[this.rTPos];
                    ++this.rTPos;
                    if (this.rTPos == 512)
                        this.rTPos = 0;
                }
                --this.rNToGo;
                this.ch2 ^= this.rNToGo == 1 ? 1 : 0;
                ++this.i2;
                this.currentChar = this.ch2;
                this.currentState = 3;
                this.mCrc.UpdateCRC( this.ch2 );
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
        }

        private void SetupNoRandPartA()
        {
            if (this.i2 <= this.last)
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                ++this.i2;
                this.currentChar = this.ch2;
                this.currentState = 6;
                this.mCrc.UpdateCRC( this.ch2 );
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
        }

        private void SetupRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 2;
                this.count = 1;
                this.SetupRandPartA();
            }
            else
            {
                ++this.count;
                if (this.count >= 4)
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    if (this.rNToGo == 0)
                    {
                        this.rNToGo = BZip2Constants.rNums[this.rTPos];
                        ++this.rTPos;
                        if (this.rTPos == 512)
                            this.rTPos = 0;
                    }
                    --this.rNToGo;
                    this.z ^= this.rNToGo == 1 ? '\u0001' : char.MinValue;
                    this.j2 = 0;
                    this.currentState = 4;
                    this.SetupRandPartC();
                }
                else
                {
                    this.currentState = 2;
                    this.SetupRandPartA();
                }
            }
        }

        private void SetupRandPartC()
        {
            if (this.j2 < z)
            {
                this.currentChar = this.ch2;
                this.mCrc.UpdateCRC( this.ch2 );
                ++this.j2;
            }
            else
            {
                this.currentState = 2;
                ++this.i2;
                this.count = 0;
                this.SetupRandPartA();
            }
        }

        private void SetupNoRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 5;
                this.count = 1;
                this.SetupNoRandPartA();
            }
            else
            {
                ++this.count;
                if (this.count >= 4)
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    this.currentState = 7;
                    this.j2 = 0;
                    this.SetupNoRandPartC();
                }
                else
                {
                    this.currentState = 5;
                    this.SetupNoRandPartA();
                }
            }
        }

        private void SetupNoRandPartC()
        {
            if (this.j2 < z)
            {
                this.currentChar = this.ch2;
                this.mCrc.UpdateCRC( this.ch2 );
                ++this.j2;
            }
            else
            {
                this.currentState = 5;
                ++this.i2;
                this.count = 0;
                this.SetupNoRandPartA();
            }
        }

        private void SetDecompressStructureSizes( int newSize100k )
        {
            if (0 <= newSize100k && newSize100k <= 9 && 0 <= this.blockSize100k)
            {
                int blockSize100k = this.blockSize100k;
            }
            this.blockSize100k = newSize100k;
            if (newSize100k == 0)
                return;
            int length = 100000 * newSize100k;
            this.ll8 = new char[length];
            this.tt = new int[length];
        }

        public override void Flush()
        {
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            int num1;
            for (num1 = 0; num1 < count; ++num1)
            {
                int num2 = this.ReadByte();
                if (num2 != -1)
                    buffer[num1 + offset] = (byte)num2;
                else
                    break;
            }
            return num1;
        }

        public override long Seek( long offset, SeekOrigin origin ) => 0;

        public override void SetLength( long value )
        {
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => 0;

        public override long Position
        {
            get => 0;
            set
            {
            }
        }
    }
}
