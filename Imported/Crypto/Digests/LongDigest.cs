// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.LongDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public abstract class LongDigest : IDigest, IMemoable
    {
        private int MyByteLength = 128;
        private byte[] xBuf;
        private int xBufOff;
        private long byteCount1;
        private long byteCount2;
        internal ulong H1;
        internal ulong H2;
        internal ulong H3;
        internal ulong H4;
        internal ulong H5;
        internal ulong H6;
        internal ulong H7;
        internal ulong H8;
        private ulong[] W = new ulong[80];
        private int wOff;
        internal static readonly ulong[] K = new ulong[80]
        {
      4794697086780616226UL,
      8158064640168781261UL,
      13096744586834688815UL,
      16840607885511220156UL,
      4131703408338449720UL,
      6480981068601479193UL,
      10538285296894168987UL,
      12329834152419229976UL,
      15566598209576043074UL,
      1334009975649890238UL,
      2608012711638119052UL,
      6128411473006802146UL,
      8268148722764581231UL,
      9286055187155687089UL,
      11230858885718282805UL,
      13951009754708518548UL,
      16472876342353939154UL,
      17275323862435702243UL,
      1135362057144423861UL,
      2597628984639134821UL,
      3308224258029322869UL,
      5365058923640841347UL,
      6679025012923562964UL,
      8573033837759648693UL,
      10970295158949994411UL,
      12119686244451234320UL,
      12683024718118986047UL,
      13788192230050041572UL,
      14330467153632333762UL,
      15395433587784984357UL,
      489312712824947311UL,
      1452737877330783856UL,
      2861767655752347644UL,
      3322285676063803686UL,
      5560940570517711597UL,
      5996557281743188959UL,
      7280758554555802590UL,
      8532644243296465576UL,
      9350256976987008742UL,
      10552545826968843579UL,
      11727347734174303076UL,
      12113106623233404929UL,
      14000437183269869457UL,
      14369950271660146224UL,
      15101387698204529176UL,
      15463397548674623760UL,
      17586052441742319658UL,
      1182934255886127544UL,
      1847814050463011016UL,
      2177327727835720531UL,
      2830643537854262169UL,
      3796741975233480872UL,
      4115178125766777443UL,
      5681478168544905931UL,
      6601373596472566643UL,
      7507060721942968483UL,
      8399075790359081724UL,
      8693463985226723168UL,
      9568029438360202098UL,
      10144078919501101548UL,
      10430055236837252648UL,
      11840083180663258601UL,
      13761210420658862357UL,
      14299343276471374635UL,
      14566680578165727644UL,
      15097957966210449927UL,
      16922976911328602910UL,
      17689382322260857208UL,
      500013540394364858UL,
      748580250866718886UL,
      1242879168328830382UL,
      1977374033974150939UL,
      2944078676154940804UL,
      3659926193048069267UL,
      4368137639120453308UL,
      4836135668995329356UL,
      5532061633213252278UL,
      6448918945643986474UL,
      6902733635092675308UL,
      7801388544844847127UL
        };

        internal LongDigest()
        {
            this.xBuf = new byte[8];
            this.Reset();
        }

        internal LongDigest( LongDigest t )
        {
            this.xBuf = new byte[t.xBuf.Length];
            this.CopyIn( t );
        }

        protected void CopyIn( LongDigest t )
        {
            Array.Copy( t.xBuf, 0, xBuf, 0, t.xBuf.Length );
            this.xBufOff = t.xBufOff;
            this.byteCount1 = t.byteCount1;
            this.byteCount2 = t.byteCount2;
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            this.H6 = t.H6;
            this.H7 = t.H7;
            this.H8 = t.H8;
            Array.Copy( t.W, 0, W, 0, t.W.Length );
            this.wOff = t.wOff;
        }

        public void Update( byte input )
        {
            this.xBuf[this.xBufOff++] = input;
            if (this.xBufOff == this.xBuf.Length)
            {
                this.ProcessWord( this.xBuf, 0 );
                this.xBufOff = 0;
            }
            ++this.byteCount1;
        }

        public void BlockUpdate( byte[] input, int inOff, int length )
        {
            for (; this.xBufOff != 0 && length > 0; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
            while (length > this.xBuf.Length)
            {
                this.ProcessWord( input, inOff );
                inOff += this.xBuf.Length;
                length -= this.xBuf.Length;
                this.byteCount1 += xBuf.Length;
            }
            for (; length > 0; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
        }

        public void Finish()
        {
            this.AdjustByteCounts();
            long lowW = this.byteCount1 << 3;
            long byteCount2 = this.byteCount2;
            this.Update( 128 );
            while (this.xBufOff != 0)
                this.Update( 0 );
            this.ProcessLength( lowW, byteCount2 );
            this.ProcessBlock();
        }

        public virtual void Reset()
        {
            this.byteCount1 = 0L;
            this.byteCount2 = 0L;
            this.xBufOff = 0;
            for (int index = 0; index < this.xBuf.Length; ++index)
                this.xBuf[index] = 0;
            this.wOff = 0;
            Array.Clear( W, 0, this.W.Length );
        }

        internal void ProcessWord( byte[] input, int inOff )
        {
            this.W[this.wOff] = Pack.BE_To_UInt64( input, inOff );
            if (++this.wOff != 16)
                return;
            this.ProcessBlock();
        }

        private void AdjustByteCounts()
        {
            if (this.byteCount1 <= 2305843009213693951L)
                return;
            this.byteCount2 += this.byteCount1 >>> 61;
            this.byteCount1 &= 2305843009213693951L;
        }

        internal void ProcessLength( long lowW, long hiW )
        {
            if (this.wOff > 14)
                this.ProcessBlock();
            this.W[14] = (ulong)hiW;
            this.W[15] = (ulong)lowW;
        }

        internal void ProcessBlock()
        {
            this.AdjustByteCounts();
            for (int index = 16; index <= 79; ++index)
                this.W[index] = Sigma1( this.W[index - 2] ) + this.W[index - 7] + Sigma0( this.W[index - 15] ) + this.W[index - 16];
            ulong num1 = this.H1;
            ulong num2 = this.H2;
            ulong num3 = this.H3;
            ulong num4 = this.H4;
            ulong num5 = this.H5;
            ulong num6 = this.H6;
            ulong num7 = this.H7;
            ulong num8 = this.H8;
            int index1 = 0;
            for (int index2 = 0; index2 < 10; ++index2)
            {
                long num9 = (long)num8;
                long num10 = (long)Sum1( num5 ) + (long)Ch( num5, num6, num7 ) + (long)K[index1];
                ulong[] w1 = this.W;
                int index3 = index1;
                int index4 = index3 + 1;
                long num11 = (long)w1[index3];
                long num12 = num10 + num11;
                ulong num13 = (ulong)(num9 + num12);
                ulong num14 = num4 + num13;
                ulong num15 = num13 + Sum0( num1 ) + Maj( num1, num2, num3 );
                long num16 = (long)num7;
                long num17 = (long)Sum1( num14 ) + (long)Ch( num14, num5, num6 ) + (long)K[index4];
                ulong[] w2 = this.W;
                int index5 = index4;
                int index6 = index5 + 1;
                long num18 = (long)w2[index5];
                long num19 = num17 + num18;
                ulong num20 = (ulong)(num16 + num19);
                ulong num21 = num3 + num20;
                ulong num22 = num20 + Sum0( num15 ) + Maj( num15, num1, num2 );
                long num23 = (long)num6;
                long num24 = (long)Sum1( num21 ) + (long)Ch( num21, num14, num5 ) + (long)K[index6];
                ulong[] w3 = this.W;
                int index7 = index6;
                int index8 = index7 + 1;
                long num25 = (long)w3[index7];
                long num26 = num24 + num25;
                ulong num27 = (ulong)(num23 + num26);
                ulong num28 = num2 + num27;
                ulong num29 = num27 + Sum0( num22 ) + Maj( num22, num15, num1 );
                long num30 = (long)num5;
                long num31 = (long)Sum1( num28 ) + (long)Ch( num28, num21, num14 ) + (long)K[index8];
                ulong[] w4 = this.W;
                int index9 = index8;
                int index10 = index9 + 1;
                long num32 = (long)w4[index9];
                long num33 = num31 + num32;
                ulong num34 = (ulong)(num30 + num33);
                ulong num35 = num1 + num34;
                ulong num36 = num34 + Sum0( num29 ) + Maj( num29, num22, num15 );
                long num37 = (long)num14;
                long num38 = (long)Sum1( num35 ) + (long)Ch( num35, num28, num21 ) + (long)K[index10];
                ulong[] w5 = this.W;
                int index11 = index10;
                int index12 = index11 + 1;
                long num39 = (long)w5[index11];
                long num40 = num38 + num39;
                ulong num41 = (ulong)(num37 + num40);
                num8 = num15 + num41;
                num4 = num41 + Sum0( num36 ) + Maj( num36, num29, num22 );
                long num42 = (long)num21;
                long num43 = (long)Sum1( num8 ) + (long)Ch( num8, num35, num28 ) + (long)K[index12];
                ulong[] w6 = this.W;
                int index13 = index12;
                int index14 = index13 + 1;
                long num44 = (long)w6[index13];
                long num45 = num43 + num44;
                ulong num46 = (ulong)(num42 + num45);
                num7 = num22 + num46;
                num3 = num46 + Sum0( num4 ) + Maj( num4, num36, num29 );
                long num47 = (long)num28;
                long num48 = (long)Sum1( num7 ) + (long)Ch( num7, num8, num35 ) + (long)K[index14];
                ulong[] w7 = this.W;
                int index15 = index14;
                int index16 = index15 + 1;
                long num49 = (long)w7[index15];
                long num50 = num48 + num49;
                ulong num51 = (ulong)(num47 + num50);
                num6 = num29 + num51;
                num2 = num51 + Sum0( num3 ) + Maj( num3, num4, num36 );
                long num52 = (long)num35;
                long num53 = (long)Sum1( num6 ) + (long)Ch( num6, num7, num8 ) + (long)K[index16];
                ulong[] w8 = this.W;
                int index17 = index16;
                index1 = index17 + 1;
                long num54 = (long)w8[index17];
                long num55 = num53 + num54;
                ulong num56 = (ulong)(num52 + num55);
                num5 = num36 + num56;
                num1 = num56 + Sum0( num2 ) + Maj( num2, num3, num4 );
            }
            this.H1 += num1;
            this.H2 += num2;
            this.H3 += num3;
            this.H4 += num4;
            this.H5 += num5;
            this.H6 += num6;
            this.H7 += num7;
            this.H8 += num8;
            this.wOff = 0;
            Array.Clear( W, 0, 16 );
        }

        private static ulong Ch( ulong x, ulong y, ulong z ) => (ulong)(((long)x & (long)y) ^ (~(long)x & (long)z));

        private static ulong Maj( ulong x, ulong y, ulong z ) => (ulong)(((long)x & (long)y) ^ ((long)x & (long)z) ^ ((long)y & (long)z));

        private static ulong Sum0( ulong x ) => (ulong)((((long)x << 36) | (long)(x >> 28)) ^ (((long)x << 30) | (long)(x >> 34)) ^ (((long)x << 25) | (long)(x >> 39)));

        private static ulong Sum1( ulong x ) => (ulong)((((long)x << 50) | (long)(x >> 14)) ^ (((long)x << 46) | (long)(x >> 18)) ^ (((long)x << 23) | (long)(x >> 41)));

        private static ulong Sigma0( ulong x ) => (ulong)((((long)x << 63) | (long)(x >> 1)) ^ (((long)x << 56) | (long)(x >> 8))) ^ (x >> 7);

        private static ulong Sigma1( ulong x ) => (ulong)((((long)x << 45) | (long)(x >> 19)) ^ (((long)x << 3) | (long)(x >> 61))) ^ (x >> 6);

        public int GetByteLength() => this.MyByteLength;

        public abstract string AlgorithmName { get; }

        public abstract int GetDigestSize();

        public abstract int DoFinal( byte[] output, int outOff );

        public abstract IMemoable Copy();

        public abstract void Reset( IMemoable t );
    }
}
