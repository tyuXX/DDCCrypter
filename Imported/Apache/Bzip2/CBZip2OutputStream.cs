// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Apache.Bzip2.CBZip2OutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Apache.Bzip2
{
    public class CBZip2OutputStream : Stream
    {
        protected const int SETMASK = 2097152;
        protected const int CLEARMASK = -2097153;
        protected const int GREATER_ICOST = 15;
        protected const int LESSER_ICOST = 0;
        protected const int SMALL_THRESH = 20;
        protected const int DEPTH_THRESH = 10;
        protected const int QSORT_STACK_SIZE = 1000;
        private bool finished;
        private int last;
        private int origPtr;
        private int blockSize100k;
        private bool blockRandomised;
        private int bytesOut;
        private int bsBuff;
        private int bsLive;
        private CRC mCrc = new CRC();
        private bool[] inUse = new bool[256];
        private int nInUse;
        private char[] seqToUnseq = new char[256];
        private char[] unseqToSeq = new char[256];
        private char[] selector = new char[18002];
        private char[] selectorMtf = new char[18002];
        private char[] block;
        private int[] quadrant;
        private int[] zptr;
        private short[] szptr;
        private int[] ftab;
        private int nMTF;
        private int[] mtfFreq = new int[258];
        private int workFactor;
        private int workDone;
        private int workLimit;
        private bool firstAttempt;
        private int nBlocksRandomised;
        private int currentChar = -1;
        private int runLength = 0;
        private bool closed = false;
        private int blockCRC;
        private int combinedCRC;
        private int allowableBlockSize;
        private Stream bsStream;
        private int[] incs = new int[14]
        {
      1,
      4,
      13,
      40,
      121,
      364,
      1093,
      3280,
      9841,
      29524,
      88573,
      265720,
      797161,
      2391484
        };

        private static void Panic()
        {
        }

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

        protected static void HbMakeCodeLengths( char[] len, int[] freq, int alphaSize, int maxLen )
        {
            int[] numArray1 = new int[260];
            int[] numArray2 = new int[516];
            int[] numArray3 = new int[516];
            for (int index = 0; index < alphaSize; ++index)
                numArray2[index + 1] = (freq[index] == 0 ? 1 : freq[index]) << 8;
            label_3:
            int index1 = alphaSize;
            int index2 = 0;
            numArray1[0] = 0;
            numArray2[0] = 0;
            numArray3[0] = -2;
            for (int index3 = 1; index3 <= alphaSize; ++index3)
            {
                numArray3[index3] = -1;
                ++index2;
                numArray1[index2] = index3;
                int index4 = index2;
                int index5;
                for (index5 = numArray1[index4]; numArray2[index5] < numArray2[numArray1[index4 >> 1]]; index4 >>= 1)
                    numArray1[index4] = numArray1[index4 >> 1];
                numArray1[index4] = index5;
            }
            if (index2 >= 260)
                Panic();
            while (index2 > 1)
            {
                int index6 = numArray1[1];
                numArray1[1] = numArray1[index2];
                int index7 = index2 - 1;
                int index8 = 1;
                int index9 = numArray1[index8];
                while (true)
                {
                    int index10 = index8 << 1;
                    if (index10 <= index7)
                    {
                        if (index10 < index7 && numArray2[numArray1[index10 + 1]] < numArray2[numArray1[index10]])
                            ++index10;
                        if (numArray2[index9] >= numArray2[numArray1[index10]])
                        {
                            numArray1[index8] = numArray1[index10];
                            index8 = index10;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
                numArray1[index8] = index9;
                int index11 = numArray1[1];
                numArray1[1] = numArray1[index7];
                int num = index7 - 1;
                int index12 = 1;
                int index13 = numArray1[index12];
                while (true)
                {
                    int index14 = index12 << 1;
                    if (index14 <= num)
                    {
                        if (index14 < num && numArray2[numArray1[index14 + 1]] < numArray2[numArray1[index14]])
                            ++index14;
                        if (numArray2[index13] >= numArray2[numArray1[index14]])
                        {
                            numArray1[index12] = numArray1[index14];
                            index12 = index14;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
                numArray1[index12] = index13;
                ++index1;
                numArray3[index6] = numArray3[index11] = index1;
                numArray2[index1] = (int)(uint)((numArray2[index6] & 4294967040L) + (numArray2[index11] & 4294967040L)) | (1 + ((numArray2[index6] & byte.MaxValue) > (numArray2[index11] & byte.MaxValue) ? numArray2[index6] & byte.MaxValue : numArray2[index11] & byte.MaxValue));
                numArray3[index1] = -1;
                index2 = num + 1;
                numArray1[index2] = index1;
                int index15 = index2;
                int index16;
                for (index16 = numArray1[index15]; numArray2[index16] < numArray2[numArray1[index15 >> 1]]; index15 >>= 1)
                    numArray1[index15] = numArray1[index15 >> 1];
                numArray1[index15] = index16;
            }
            if (index1 >= 516)
                Panic();
            bool flag = false;
            for (int index17 = 1; index17 <= alphaSize; ++index17)
            {
                int num = 0;
                int index18 = index17;
                while (numArray3[index18] >= 0)
                {
                    index18 = numArray3[index18];
                    ++num;
                }
                len[index17 - 1] = (char)num;
                if (num > maxLen)
                    flag = true;
            }
            if (!flag)
                return;
            for (int index19 = 1; index19 < alphaSize; ++index19)
            {
                int num = 1 + ((numArray2[index19] >> 8) / 2);
                numArray2[index19] = num << 8;
            }
            goto label_3;
        }

        public CBZip2OutputStream( Stream inStream )
          : this( inStream, 9 )
        {
        }

        public CBZip2OutputStream( Stream inStream, int inBlockSize )
        {
            this.block = null;
            this.quadrant = null;
            this.zptr = null;
            this.ftab = null;
            inStream.WriteByte( 66 );
            inStream.WriteByte( 90 );
            this.BsSetStream( inStream );
            this.workFactor = 50;
            if (inBlockSize > 9)
                inBlockSize = 9;
            if (inBlockSize < 1)
                inBlockSize = 1;
            this.blockSize100k = inBlockSize;
            this.AllocateCompressStructures();
            this.Initialize();
            this.InitBlock();
        }

        public override void WriteByte( byte bv )
        {
            int num = (256 + bv) % 256;
            if (this.currentChar != -1)
            {
                if (this.currentChar == num)
                {
                    ++this.runLength;
                    if (this.runLength <= 254)
                        return;
                    this.WriteRun();
                    this.currentChar = -1;
                    this.runLength = 0;
                }
                else
                {
                    this.WriteRun();
                    this.runLength = 1;
                    this.currentChar = num;
                }
            }
            else
            {
                this.currentChar = num;
                ++this.runLength;
            }
        }

        private void WriteRun()
        {
            if (this.last < this.allowableBlockSize)
            {
                this.inUse[this.currentChar] = true;
                for (int index = 0; index < this.runLength; ++index)
                    this.mCrc.UpdateCRC( (ushort)this.currentChar );
                switch (this.runLength)
                {
                    case 1:
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        break;
                    case 2:
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        break;
                    case 3:
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        break;
                    default:
                        this.inUse[this.runLength - 4] = true;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)this.currentChar;
                        ++this.last;
                        this.block[this.last + 1] = (char)(this.runLength - 4);
                        break;
                }
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.WriteRun();
            }
        }

        public override void Close()
        {
            if (this.closed)
                return;
            this.Finish();
            this.closed = true;
            Platform.Dispose( this.bsStream );
            base.Close();
        }

        public void Finish()
        {
            if (this.finished)
                return;
            if (this.runLength > 0)
                this.WriteRun();
            this.currentChar = -1;
            this.EndBlock();
            this.EndCompression();
            this.finished = true;
            this.Flush();
        }

        public override void Flush() => this.bsStream.Flush();

        private void Initialize()
        {
            this.bytesOut = 0;
            this.nBlocksRandomised = 0;
            this.BsPutUChar( 104 );
            this.BsPutUChar( 48 + this.blockSize100k );
            this.combinedCRC = 0;
        }

        private void InitBlock()
        {
            this.mCrc.InitialiseCRC();
            this.last = -1;
            for (int index = 0; index < 256; ++index)
                this.inUse[index] = false;
            this.allowableBlockSize = (100000 * this.blockSize100k) - 20;
        }

        private void EndBlock()
        {
            this.blockCRC = this.mCrc.GetFinalCRC();
            this.combinedCRC = (this.combinedCRC << 1) | this.combinedCRC >>> 31;
            this.combinedCRC ^= this.blockCRC;
            this.DoReversibleTransformation();
            this.BsPutUChar( 49 );
            this.BsPutUChar( 65 );
            this.BsPutUChar( 89 );
            this.BsPutUChar( 38 );
            this.BsPutUChar( 83 );
            this.BsPutUChar( 89 );
            this.BsPutint( this.blockCRC );
            if (this.blockRandomised)
            {
                this.BsW( 1, 1 );
                ++this.nBlocksRandomised;
            }
            else
                this.BsW( 1, 0 );
            this.MoveToFrontCodeAndSend();
        }

        private void EndCompression()
        {
            this.BsPutUChar( 23 );
            this.BsPutUChar( 114 );
            this.BsPutUChar( 69 );
            this.BsPutUChar( 56 );
            this.BsPutUChar( 80 );
            this.BsPutUChar( 144 );
            this.BsPutint( this.combinedCRC );
            this.BsFinishedWithStream();
        }

        private void HbAssignCodes( int[] code, char[] length, int minLen, int maxLen, int alphaSize )
        {
            int num = 0;
            for (int index1 = minLen; index1 <= maxLen; ++index1)
            {
                for (int index2 = 0; index2 < alphaSize; ++index2)
                {
                    if (length[index2] == index1)
                    {
                        code[index2] = num;
                        ++num;
                    }
                }
                num <<= 1;
            }
        }

        private void BsSetStream( Stream f )
        {
            this.bsStream = f;
            this.bsLive = 0;
            this.bsBuff = 0;
            this.bytesOut = 0;
        }

        private void BsFinishedWithStream()
        {
            while (this.bsLive > 0)
            {
                int num = this.bsBuff >> 24;
                try
                {
                    this.bsStream.WriteByte( (byte)num );
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                this.bsBuff <<= 8;
                this.bsLive -= 8;
                ++this.bytesOut;
            }
        }

        private void BsW( int n, int v )
        {
            while (this.bsLive >= 8)
            {
                int num = this.bsBuff >> 24;
                try
                {
                    this.bsStream.WriteByte( (byte)num );
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                this.bsBuff <<= 8;
                this.bsLive -= 8;
                ++this.bytesOut;
            }
            this.bsBuff |= v << (32 - this.bsLive - n);
            this.bsLive += n;
        }

        private void BsPutUChar( int c ) => this.BsW( 8, c );

        private void BsPutint( int u )
        {
            this.BsW( 8, (u >> 24) & byte.MaxValue );
            this.BsW( 8, (u >> 16) & byte.MaxValue );
            this.BsW( 8, (u >> 8) & byte.MaxValue );
            this.BsW( 8, u & byte.MaxValue );
        }

        private void BsPutIntVS( int numBits, int c ) => this.BsW( numBits, c );

        private void SendMTFValues()
        {
            char[][] chArray1 = CBZip2InputStream.InitCharArray( 6, 258 );
            int v1 = 0;
            int alphaSize = this.nInUse + 2;
            for (int index1 = 0; index1 < 6; ++index1)
            {
                for (int index2 = 0; index2 < alphaSize; ++index2)
                    chArray1[index1][index2] = '\u000F';
            }
            if (this.nMTF <= 0)
                Panic();
            int v2 = this.nMTF >= 200 ? (this.nMTF >= 600 ? (this.nMTF >= 1200 ? (this.nMTF >= 2400 ? 6 : 5) : 4) : 3) : 2;
            int num1 = v2;
            int nMtf = this.nMTF;
            int num2 = 0;
            while (num1 > 0)
            {
                int num3 = nMtf / num1;
                int index3 = num2 - 1;
                int num4;
                for (num4 = 0; num4 < num3 && index3 < alphaSize - 1; num4 += this.mtfFreq[index3])
                    ++index3;
                if (index3 > num2 && num1 != v2 && num1 != 1 && (v2 - num1) % 2 == 1)
                {
                    num4 -= this.mtfFreq[index3];
                    --index3;
                }
                for (int index4 = 0; index4 < alphaSize; ++index4)
                    chArray1[num1 - 1][index4] = index4 < num2 || index4 > index3 ? '\u000F' : char.MinValue;
                --num1;
                num2 = index3 + 1;
                nMtf -= num4;
            }
            int[][] numArray1 = CBZip2InputStream.InitIntArray( 6, 258 );
            int[] numArray2 = new int[6];
            short[] numArray3 = new short[6];
            for (int index5 = 0; index5 < 4; ++index5)
            {
                for (int index6 = 0; index6 < v2; ++index6)
                    numArray2[index6] = 0;
                for (int index7 = 0; index7 < v2; ++index7)
                {
                    for (int index8 = 0; index8 < alphaSize; ++index8)
                        numArray1[index7][index8] = 0;
                }
                v1 = 0;
                int num5 = 0;
                int num6;
                for (int index9 = 0; index9 < this.nMTF; index9 = num6 + 1)
                {
                    num6 = index9 + 50 - 1;
                    if (num6 >= this.nMTF)
                        num6 = this.nMTF - 1;
                    for (int index10 = 0; index10 < v2; ++index10)
                        numArray3[index10] = 0;
                    if (v2 == 6)
                    {
                        int num7;
                        short num8 = (short)(num7 = 0);
                        short num9 = (short)num7;
                        short num10 = (short)num7;
                        short num11 = (short)num7;
                        short num12 = (short)num7;
                        short num13 = (short)num7;
                        for (int index11 = index9; index11 <= num6; ++index11)
                        {
                            short index12 = this.szptr[index11];
                            num13 += (short)chArray1[0][index12];
                            num12 += (short)chArray1[1][index12];
                            num11 += (short)chArray1[2][index12];
                            num10 += (short)chArray1[3][index12];
                            num9 += (short)chArray1[4][index12];
                            num8 += (short)chArray1[5][index12];
                        }
                        numArray3[0] = num13;
                        numArray3[1] = num12;
                        numArray3[2] = num11;
                        numArray3[3] = num10;
                        numArray3[4] = num9;
                        numArray3[5] = num8;
                    }
                    else
                    {
                        for (int index13 = index9; index13 <= num6; ++index13)
                        {
                            short index14 = this.szptr[index13];
                            for (int index15 = 0; index15 < v2; ++index15)
                            {
                                short[] numArray4;
                                IntPtr index16;
                                (numArray4 = numArray3)[(int)(index16 = (IntPtr)index15)] = (short)(numArray4[(int)index16] + (short)chArray1[index15][index14]);
                            }
                        }
                    }
                    int num14 = 999999999;
                    int index17 = -1;
                    for (int index18 = 0; index18 < v2; ++index18)
                    {
                        if (numArray3[index18] < num14)
                        {
                            num14 = numArray3[index18];
                            index17 = index18;
                        }
                    }
                    num5 += num14;
                    ++numArray2[index17];
                    this.selector[v1] = (char)index17;
                    ++v1;
                    for (int index19 = index9; index19 <= num6; ++index19)
                        ++numArray1[index17][this.szptr[index19]];
                }
                for (int index20 = 0; index20 < v2; ++index20)
                    HbMakeCodeLengths( chArray1[index20], numArray1[index20], alphaSize, 20 );
            }
            if (v2 >= 8)
                Panic();
            if (v1 >= 32768 || v1 > 18002)
                Panic();
            char[] chArray2 = new char[6];
            for (int index = 0; index < v2; ++index)
                chArray2[index] = (char)index;
            for (int index21 = 0; index21 < v1; ++index21)
            {
                char ch1 = this.selector[index21];
                int index22 = 0;
                char ch2 = chArray2[index22];
                while (ch1 != ch2)
                {
                    ++index22;
                    char ch3 = ch2;
                    ch2 = chArray2[index22];
                    chArray2[index22] = ch3;
                }
                chArray2[0] = ch2;
                this.selectorMtf[index21] = (char)index22;
            }
            int[][] numArray5 = CBZip2InputStream.InitIntArray( 6, 258 );
            for (int index23 = 0; index23 < v2; ++index23)
            {
                int minLen = 32;
                int maxLen = 0;
                for (int index24 = 0; index24 < alphaSize; ++index24)
                {
                    if (chArray1[index23][index24] > maxLen)
                        maxLen = chArray1[index23][index24];
                    if (chArray1[index23][index24] < minLen)
                        minLen = chArray1[index23][index24];
                }
                if (maxLen > 20)
                    Panic();
                if (minLen < 1)
                    Panic();
                this.HbAssignCodes( numArray5[index23], chArray1[index23], minLen, maxLen, alphaSize );
            }
            bool[] flagArray = new bool[16];
            for (int index25 = 0; index25 < 16; ++index25)
            {
                flagArray[index25] = false;
                for (int index26 = 0; index26 < 16; ++index26)
                {
                    if (this.inUse[(index25 * 16) + index26])
                        flagArray[index25] = true;
                }
            }
            for (int index = 0; index < 16; ++index)
            {
                if (flagArray[index])
                    this.BsW( 1, 1 );
                else
                    this.BsW( 1, 0 );
            }
            for (int index27 = 0; index27 < 16; ++index27)
            {
                if (flagArray[index27])
                {
                    for (int index28 = 0; index28 < 16; ++index28)
                    {
                        if (this.inUse[(index27 * 16) + index28])
                            this.BsW( 1, 1 );
                        else
                            this.BsW( 1, 0 );
                    }
                }
            }
            this.BsW( 3, v2 );
            this.BsW( 15, v1 );
            for (int index29 = 0; index29 < v1; ++index29)
            {
                for (int index30 = 0; index30 < this.selectorMtf[index29]; ++index30)
                    this.BsW( 1, 1 );
                this.BsW( 1, 0 );
            }
            for (int index31 = 0; index31 < v2; ++index31)
            {
                int v3 = chArray1[index31][0];
                this.BsW( 5, v3 );
                for (int index32 = 0; index32 < alphaSize; ++index32)
                {
                    for (; v3 < chArray1[index31][index32]; ++v3)
                        this.BsW( 2, 2 );
                    for (; v3 > chArray1[index31][index32]; --v3)
                        this.BsW( 2, 3 );
                    this.BsW( 1, 0 );
                }
            }
            int index33 = 0;
            int num15 = 0;
            while (num15 < this.nMTF)
            {
                int num16 = num15 + 50 - 1;
                if (num16 >= this.nMTF)
                    num16 = this.nMTF - 1;
                for (int index34 = num15; index34 <= num16; ++index34)
                    this.BsW( chArray1[this.selector[index33]][this.szptr[index34]], numArray5[this.selector[index33]][this.szptr[index34]] );
                num15 = num16 + 1;
                ++index33;
            }
            if (index33 == v1)
                return;
            Panic();
        }

        private void MoveToFrontCodeAndSend()
        {
            this.BsPutIntVS( 24, this.origPtr );
            this.GenerateMTFValues();
            this.SendMTFValues();
        }

        private void SimpleSort( int lo, int hi, int d )
        {
            int num1 = hi - lo + 1;
            if (num1 < 2)
                return;
            int index1 = 0;
            while (this.incs[index1] < num1)
                ++index1;
            for (int index2 = index1 - 1; index2 >= 0; --index2)
            {
                int inc = this.incs[index2];
                int index3 = lo + inc;
                while (index3 <= hi)
                {
                    int num2 = this.zptr[index3];
                    int index4 = index3;
                    while (this.FullGtU( this.zptr[index4 - inc] + d, num2 + d ))
                    {
                        this.zptr[index4] = this.zptr[index4 - inc];
                        index4 -= inc;
                        if (index4 <= lo + inc - 1)
                            break;
                    }
                    this.zptr[index4] = num2;
                    int index5 = index3 + 1;
                    if (index5 <= hi)
                    {
                        int num3 = this.zptr[index5];
                        int index6 = index5;
                        while (this.FullGtU( this.zptr[index6 - inc] + d, num3 + d ))
                        {
                            this.zptr[index6] = this.zptr[index6 - inc];
                            index6 -= inc;
                            if (index6 <= lo + inc - 1)
                                break;
                        }
                        this.zptr[index6] = num3;
                        int index7 = index5 + 1;
                        if (index7 <= hi)
                        {
                            int num4 = this.zptr[index7];
                            int index8 = index7;
                            while (this.FullGtU( this.zptr[index8 - inc] + d, num4 + d ))
                            {
                                this.zptr[index8] = this.zptr[index8 - inc];
                                index8 -= inc;
                                if (index8 <= lo + inc - 1)
                                    break;
                            }
                            this.zptr[index8] = num4;
                            index3 = index7 + 1;
                            if (this.workDone > this.workLimit && this.firstAttempt)
                                return;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
            }
        }

        private void Vswap( int p1, int p2, int n )
        {
            for (; n > 0; --n)
            {
                int num = this.zptr[p1];
                this.zptr[p1] = this.zptr[p2];
                this.zptr[p2] = num;
                ++p1;
                ++p2;
            }
        }

        private char Med3( char a, char b, char c )
        {
            if (a > b)
            {
                char ch = a;
                a = b;
                b = ch;
            }
            if (b > c)
            {
                char ch = b;
                b = c;
                c = ch;
            }
            if (a > b)
                b = a;
            return b;
        }

        private void QSort3( int loSt, int hiSt, int dSt )
        {
            CBZip2OutputStream.StackElem[] stackElemArray = new CBZip2OutputStream.StackElem[1000];
            for (int index = 0; index < 1000; ++index)
                stackElemArray[index] = new CBZip2OutputStream.StackElem();
            int index1 = 0;
            stackElemArray[index1].ll = loSt;
            stackElemArray[index1].hh = hiSt;
            stackElemArray[index1].dd = dSt;
            int index2 = index1 + 1;
            while (index2 > 0)
            {
                if (index2 >= 1000)
                    Panic();
                --index2;
                int ll = stackElemArray[index2].ll;
                int hh = stackElemArray[index2].hh;
                int dd = stackElemArray[index2].dd;
                if (hh - ll < 20 || dd > 10)
                {
                    this.SimpleSort( ll, hh, dd );
                    if (this.workDone > this.workLimit && this.firstAttempt)
                        break;
                }
                else
                {
                    int num1 = this.Med3( this.block[this.zptr[ll] + dd + 1], this.block[this.zptr[hh] + dd + 1], this.block[this.zptr[(ll + hh) >> 1] + dd + 1] );
                    int index3;
                    int p1 = index3 = ll;
                    int index4;
                    int index5 = index4 = hh;
                    while (true)
                    {
                        while (p1 <= index5)
                        {
                            int num2 = this.block[this.zptr[p1] + dd + 1] - num1;
                            if (num2 == 0)
                            {
                                int num3 = this.zptr[p1];
                                this.zptr[p1] = this.zptr[index3];
                                this.zptr[index3] = num3;
                                ++index3;
                                ++p1;
                            }
                            else if (num2 <= 0)
                                ++p1;
                            else
                                break;
                        }
                        while (p1 <= index5)
                        {
                            int num4 = this.block[this.zptr[index5] + dd + 1] - num1;
                            if (num4 == 0)
                            {
                                int num5 = this.zptr[index5];
                                this.zptr[index5] = this.zptr[index4];
                                this.zptr[index4] = num5;
                                --index4;
                                --index5;
                            }
                            else if (num4 >= 0)
                                --index5;
                            else
                                break;
                        }
                        if (p1 <= index5)
                        {
                            int num6 = this.zptr[p1];
                            this.zptr[p1] = this.zptr[index5];
                            this.zptr[index5] = num6;
                            ++p1;
                            --index5;
                        }
                        else
                            break;
                    }
                    if (index4 < index3)
                    {
                        stackElemArray[index2].ll = ll;
                        stackElemArray[index2].hh = hh;
                        stackElemArray[index2].dd = dd + 1;
                        ++index2;
                    }
                    else
                    {
                        int n1 = index3 - ll < p1 - index3 ? index3 - ll : p1 - index3;
                        this.Vswap( ll, p1 - n1, n1 );
                        int n2 = hh - index4 < index4 - index5 ? hh - index4 : index4 - index5;
                        this.Vswap( p1, hh - n2 + 1, n2 );
                        int num7 = ll + p1 - index3 - 1;
                        int num8 = hh - (index4 - index5) + 1;
                        stackElemArray[index2].ll = ll;
                        stackElemArray[index2].hh = num7;
                        stackElemArray[index2].dd = dd;
                        int index6 = index2 + 1;
                        stackElemArray[index6].ll = num7 + 1;
                        stackElemArray[index6].hh = num8 - 1;
                        stackElemArray[index6].dd = dd + 1;
                        int index7 = index6 + 1;
                        stackElemArray[index7].ll = num8;
                        stackElemArray[index7].hh = hh;
                        stackElemArray[index7].dd = dd;
                        index2 = index7 + 1;
                    }
                }
            }
        }

        private void MainSort()
        {
            int[] numArray1 = new int[256];
            int[] numArray2 = new int[256];
            bool[] flagArray = new bool[256];
            for (int index = 0; index < 20; ++index)
                this.block[this.last + index + 2] = this.block[(index % (this.last + 1)) + 1];
            for (int index = 0; index <= this.last + 20; ++index)
                this.quadrant[index] = 0;
            this.block[0] = this.block[this.last + 1];
            if (this.last < 4000)
            {
                for (int index = 0; index <= this.last; ++index)
                    this.zptr[index] = index;
                this.firstAttempt = false;
                this.workDone = this.workLimit = 0;
                this.SimpleSort( 0, this.last, 0 );
            }
            else
            {
                int num1 = 0;
                for (int index = 0; index <= byte.MaxValue; ++index)
                    flagArray[index] = false;
                for (int index = 0; index <= 65536; ++index)
                    this.ftab[index] = 0;
                int num2 = this.block[0];
                for (int index = 0; index <= this.last; ++index)
                {
                    int num3 = this.block[index + 1];
                    ++this.ftab[(num2 << 8) + num3];
                    num2 = num3;
                }
                for (int index = 1; index <= 65536; ++index)
                    this.ftab[index] += this.ftab[index - 1];
                int num4 = this.block[1];
                for (int index1 = 0; index1 < this.last; ++index1)
                {
                    int num5 = this.block[index1 + 2];
                    int index2 = (num4 << 8) + num5;
                    num4 = num5;
                    --this.ftab[index2];
                    this.zptr[this.ftab[index2]] = index1;
                }
                int index3 = (this.block[this.last + 1] << 8) + this.block[1];
                --this.ftab[index3];
                this.zptr[this.ftab[index3]] = this.last;
                for (int index4 = 0; index4 <= byte.MaxValue; ++index4)
                    numArray1[index4] = index4;
                int num6 = 1;
                do
                {
                    num6 = (3 * num6) + 1;
                }
                while (num6 <= 256);
                do
                {
                    num6 /= 3;
                    for (int index5 = num6; index5 <= byte.MaxValue; ++index5)
                    {
                        int num7 = numArray1[index5];
                        int index6 = index5;
                        while (this.ftab[(numArray1[index6 - num6] + 1) << 8] - this.ftab[numArray1[index6 - num6] << 8] > this.ftab[(num7 + 1) << 8] - this.ftab[num7 << 8])
                        {
                            numArray1[index6] = numArray1[index6 - num6];
                            index6 -= num6;
                            if (index6 <= num6 - 1)
                                break;
                        }
                        numArray1[index6] = num7;
                    }
                }
                while (num6 != 1);
                for (int index7 = 0; index7 <= byte.MaxValue; ++index7)
                {
                    int index8 = numArray1[index7];
                    for (int index9 = 0; index9 <= byte.MaxValue; ++index9)
                    {
                        int index10 = (index8 << 8) + index9;
                        if ((this.ftab[index10] & 2097152) != 2097152)
                        {
                            int loSt = this.ftab[index10] & -2097153;
                            int hiSt = (this.ftab[index10 + 1] & -2097153) - 1;
                            if (hiSt > loSt)
                            {
                                this.QSort3( loSt, hiSt, 2 );
                                num1 += hiSt - loSt + 1;
                                if (this.workDone > this.workLimit && this.firstAttempt)
                                    return;
                            }
                            this.ftab[index10] |= 2097152;
                        }
                    }
                    flagArray[index8] = true;
                    if (index7 < byte.MaxValue)
                    {
                        int num8 = this.ftab[index8 << 8] & -2097153;
                        int num9 = (this.ftab[(index8 + 1) << 8] & -2097153) - num8;
                        int num10 = 0;
                        while (num9 >> num10 > 65534)
                            ++num10;
                        for (int index11 = 0; index11 < num9; ++index11)
                        {
                            int index12 = this.zptr[num8 + index11];
                            int num11 = index11 >> num10;
                            this.quadrant[index12] = num11;
                            if (index12 < 20)
                                this.quadrant[index12 + this.last + 1] = num11;
                        }
                        if ((num9 - 1) >> num10 > ushort.MaxValue)
                            Panic();
                    }
                    for (int index13 = 0; index13 <= byte.MaxValue; ++index13)
                        numArray2[index13] = this.ftab[(index13 << 8) + index8] & -2097153;
                    for (int index14 = this.ftab[index8 << 8] & -2097153; index14 < (this.ftab[(index8 + 1) << 8] & -2097153); ++index14)
                    {
                        int index15 = this.block[this.zptr[index14]];
                        if (!flagArray[index15])
                        {
                            this.zptr[numArray2[index15]] = this.zptr[index14] == 0 ? this.last : this.zptr[index14] - 1;
                            ++numArray2[index15];
                        }
                    }
                    for (int index16 = 0; index16 <= byte.MaxValue; ++index16)
                        this.ftab[(index16 << 8) + index8] |= 2097152;
                }
            }
        }

        private void RandomiseBlock()
        {
            int num1 = 0;
            int index1 = 0;
            for (int index2 = 0; index2 < 256; ++index2)
                this.inUse[index2] = false;
            for (int index3 = 0; index3 <= this.last; ++index3)
            {
                if (num1 == 0)
                {
                    num1 = (ushort)BZip2Constants.rNums[index1];
                    ++index1;
                    if (index1 == 512)
                        index1 = 0;
                }
                --num1;
                char[] block1;
                int index4;
                int num2 = (ushort)((block1 = this.block)[(int)(IntPtr)(index4 = index3 + 1)] ^ (num1 == 1 ? 1 : 0));
                block1[index4] = (char)num2;
                char[] block2;
                IntPtr index5;
                (block2 = this.block)[(int)(index5 = (IntPtr)(index3 + 1))] = (char)(block2[(int)index5] & (uint)byte.MaxValue);
                this.inUse[this.block[index3 + 1]] = true;
            }
        }

        private void DoReversibleTransformation()
        {
            this.workLimit = this.workFactor * this.last;
            this.workDone = 0;
            this.blockRandomised = false;
            this.firstAttempt = true;
            this.MainSort();
            if (this.workDone > this.workLimit && this.firstAttempt)
            {
                this.RandomiseBlock();
                this.workLimit = this.workDone = 0;
                this.blockRandomised = true;
                this.firstAttempt = false;
                this.MainSort();
            }
            this.origPtr = -1;
            for (int index = 0; index <= this.last; ++index)
            {
                if (this.zptr[index] == 0)
                {
                    this.origPtr = index;
                    break;
                }
            }
            if (this.origPtr != -1)
                return;
            Panic();
        }

        private bool FullGtU( int i1, int i2 )
        {
            char ch1 = this.block[i1 + 1];
            char ch2 = this.block[i2 + 1];
            if (ch1 != ch2)
                return ch1 > ch2;
            ++i1;
            ++i2;
            char ch3 = this.block[i1 + 1];
            char ch4 = this.block[i2 + 1];
            if (ch3 != ch4)
                return ch3 > ch4;
            ++i1;
            ++i2;
            char ch5 = this.block[i1 + 1];
            char ch6 = this.block[i2 + 1];
            if (ch5 != ch6)
                return ch5 > ch6;
            ++i1;
            ++i2;
            char ch7 = this.block[i1 + 1];
            char ch8 = this.block[i2 + 1];
            if (ch7 != ch8)
                return ch7 > ch8;
            ++i1;
            ++i2;
            char ch9 = this.block[i1 + 1];
            char ch10 = this.block[i2 + 1];
            if (ch9 != ch10)
                return ch9 > ch10;
            ++i1;
            ++i2;
            char ch11 = this.block[i1 + 1];
            char ch12 = this.block[i2 + 1];
            if (ch11 != ch12)
                return ch11 > ch12;
            ++i1;
            ++i2;
            int num1 = this.last + 1;
            do
            {
                char ch13 = this.block[i1 + 1];
                char ch14 = this.block[i2 + 1];
                if (ch13 != ch14)
                    return ch13 > ch14;
                int num2 = this.quadrant[i1];
                int num3 = this.quadrant[i2];
                if (num2 != num3)
                    return num2 > num3;
                ++i1;
                ++i2;
                char ch15 = this.block[i1 + 1];
                char ch16 = this.block[i2 + 1];
                if (ch15 != ch16)
                    return ch15 > ch16;
                int num4 = this.quadrant[i1];
                int num5 = this.quadrant[i2];
                if (num4 != num5)
                    return num4 > num5;
                ++i1;
                ++i2;
                char ch17 = this.block[i1 + 1];
                char ch18 = this.block[i2 + 1];
                if (ch17 != ch18)
                    return ch17 > ch18;
                int num6 = this.quadrant[i1];
                int num7 = this.quadrant[i2];
                if (num6 != num7)
                    return num6 > num7;
                ++i1;
                ++i2;
                char ch19 = this.block[i1 + 1];
                char ch20 = this.block[i2 + 1];
                if (ch19 != ch20)
                    return ch19 > ch20;
                int num8 = this.quadrant[i1];
                int num9 = this.quadrant[i2];
                if (num8 != num9)
                    return num8 > num9;
                ++i1;
                ++i2;
                if (i1 > this.last)
                {
                    i1 -= this.last;
                    --i1;
                }
                if (i2 > this.last)
                {
                    i2 -= this.last;
                    --i2;
                }
                num1 -= 4;
                ++this.workDone;
            }
            while (num1 >= 0);
            return false;
        }

        private void AllocateCompressStructures()
        {
            int length = 100000 * this.blockSize100k;
            this.block = new char[length + 1 + 20];
            this.quadrant = new int[length + 20];
            this.zptr = new int[length];
            this.ftab = new int[65537];
            if (this.block != null && this.quadrant != null && this.zptr != null)
            {
                int[] ftab = this.ftab;
            }
            this.szptr = new short[2 * length];
        }

        private void GenerateMTFValues()
        {
            char[] chArray = new char[256];
            this.MakeMaps();
            int index1 = this.nInUse + 1;
            for (int index2 = 0; index2 <= index1; ++index2)
                this.mtfFreq[index2] = 0;
            int index3 = 0;
            int num1 = 0;
            for (int index4 = 0; index4 < this.nInUse; ++index4)
                chArray[index4] = (char)index4;
            for (int index5 = 0; index5 <= this.last; ++index5)
            {
                char ch1 = this.unseqToSeq[this.block[this.zptr[index5]]];
                int index6 = 0;
                char ch2 = chArray[index6];
                while (ch1 != ch2)
                {
                    ++index6;
                    char ch3 = ch2;
                    ch2 = chArray[index6];
                    chArray[index6] = ch3;
                }
                chArray[0] = ch2;
                if (index6 == 0)
                {
                    ++num1;
                }
                else
                {
                    if (num1 > 0)
                    {
                        int num2 = num1 - 1;
                        while (true)
                        {
                            switch (num2 % 2)
                            {
                                case 0:
                                    this.szptr[index3] = 0;
                                    ++index3;
                                    int[] mtfFreq1;
                                    (mtfFreq1 = this.mtfFreq)[0] = mtfFreq1[0] + 1;
                                    break;
                                case 1:
                                    this.szptr[index3] = 1;
                                    ++index3;
                                    int[] mtfFreq2;
                                    (mtfFreq2 = this.mtfFreq)[1] = mtfFreq2[1] + 1;
                                    break;
                            }
                            if (num2 >= 2)
                                num2 = (num2 - 2) / 2;
                            else
                                break;
                        }
                        num1 = 0;
                    }
                    this.szptr[index3] = (short)(index6 + 1);
                    ++index3;
                    ++this.mtfFreq[index6 + 1];
                }
            }
            if (num1 > 0)
            {
                int num3 = num1 - 1;
                while (true)
                {
                    switch (num3 % 2)
                    {
                        case 0:
                            this.szptr[index3] = 0;
                            ++index3;
                            int[] mtfFreq3;
                            (mtfFreq3 = this.mtfFreq)[0] = mtfFreq3[0] + 1;
                            break;
                        case 1:
                            this.szptr[index3] = 1;
                            ++index3;
                            int[] mtfFreq4;
                            (mtfFreq4 = this.mtfFreq)[1] = mtfFreq4[1] + 1;
                            break;
                    }
                    if (num3 >= 2)
                        num3 = (num3 - 2) / 2;
                    else
                        break;
                }
            }
            this.szptr[index3] = (short)index1;
            int num4 = index3 + 1;
            ++this.mtfFreq[index1];
            this.nMTF = num4;
        }

        public override int Read( byte[] buffer, int offset, int count ) => 0;

        public override long Seek( long offset, SeekOrigin origin ) => 0;

        public override void SetLength( long value )
        {
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            for (int index = 0; index < count; ++index)
                this.WriteByte( buffer[index + offset] );
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position
        {
            get => 0;
            set
            {
            }
        }

        internal class StackElem
        {
            internal int ll;
            internal int hh;
            internal int dd;
        }
    }
}
