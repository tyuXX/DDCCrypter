// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.Gost3411Digest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class Gost3411Digest : IDigest, IMemoable
    {
        private const int DIGEST_LENGTH = 32;
        private byte[] H = new byte[32];
        private byte[] L = new byte[32];
        private byte[] M = new byte[32];
        private byte[] Sum = new byte[32];
        private byte[][] C = MakeC();
        private byte[] xBuf = new byte[32];
        private int xBufOff;
        private ulong byteCount;
        private readonly IBlockCipher cipher = new Gost28147Engine();
        private byte[] sBox;
        private byte[] K = new byte[32];
        private byte[] a = new byte[8];
        internal short[] wS = new short[16];
        internal short[] w_S = new short[16];
        internal byte[] S = new byte[32];
        internal byte[] U = new byte[32];
        internal byte[] V = new byte[32];
        internal byte[] W = new byte[32];
        private static readonly byte[] C2 = new byte[32]
        {
       0,
      byte.MaxValue,
       0,
      byte.MaxValue,
       0,
      byte.MaxValue,
       0,
      byte.MaxValue,
      byte.MaxValue,
       0,
      byte.MaxValue,
       0,
      byte.MaxValue,
       0,
      byte.MaxValue,
       0,
       0,
      byte.MaxValue,
      byte.MaxValue,
       0,
      byte.MaxValue,
       0,
       0,
      byte.MaxValue,
      byte.MaxValue,
       0,
       0,
       0,
      byte.MaxValue,
      byte.MaxValue,
       0,
      byte.MaxValue
        };

        private static byte[][] MakeC()
        {
            byte[][] numArray = new byte[4][];
            for (int index = 0; index < 4; ++index)
                numArray[index] = new byte[32];
            return numArray;
        }

        public Gost3411Digest()
        {
            this.sBox = Gost28147Engine.GetSBox( "D-A" );
            this.cipher.Init( true, new ParametersWithSBox( null, this.sBox ) );
            this.Reset();
        }

        public Gost3411Digest( byte[] sBoxParam )
        {
            this.sBox = Arrays.Clone( sBoxParam );
            this.cipher.Init( true, new ParametersWithSBox( null, this.sBox ) );
            this.Reset();
        }

        public Gost3411Digest( Gost3411Digest t ) => this.Reset( t );

        public string AlgorithmName => "Gost3411";

        public int GetDigestSize() => 32;

        public void Update( byte input )
        {
            this.xBuf[this.xBufOff++] = input;
            if (this.xBufOff == this.xBuf.Length)
            {
                this.sumByteArray( this.xBuf );
                this.processBlock( this.xBuf, 0 );
                this.xBufOff = 0;
            }
            ++this.byteCount;
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
                Array.Copy( input, inOff, xBuf, 0, this.xBuf.Length );
                this.sumByteArray( this.xBuf );
                this.processBlock( this.xBuf, 0 );
                inOff += this.xBuf.Length;
                length -= this.xBuf.Length;
                this.byteCount += (uint)this.xBuf.Length;
            }
            for (; length > 0; --length)
            {
                this.Update( input[inOff] );
                ++inOff;
            }
        }

        private byte[] P( byte[] input )
        {
            int num1 = 0;
            for (int index1 = 0; index1 < 8; ++index1)
            {
                byte[] k1 = this.K;
                int index2 = num1;
                int num2 = index2 + 1;
                int num3 = input[index1];
                k1[index2] = (byte)num3;
                byte[] k2 = this.K;
                int index3 = num2;
                int num4 = index3 + 1;
                int num5 = input[8 + index1];
                k2[index3] = (byte)num5;
                byte[] k3 = this.K;
                int index4 = num4;
                int num6 = index4 + 1;
                int num7 = input[16 + index1];
                k3[index4] = (byte)num7;
                byte[] k4 = this.K;
                int index5 = num6;
                num1 = index5 + 1;
                int num8 = input[24 + index1];
                k4[index5] = (byte)num8;
            }
            return this.K;
        }

        private byte[] A( byte[] input )
        {
            for (int index = 0; index < 8; ++index)
                this.a[index] = (byte)(input[index] ^ (uint)input[index + 8]);
            Array.Copy( input, 8, input, 0, 24 );
            Array.Copy( a, 0, input, 24, 8 );
            return input;
        }

        private void E( byte[] key, byte[] s, int sOff, byte[] input, int inOff )
        {
            this.cipher.Init( true, new KeyParameter( key ) );
            this.cipher.ProcessBlock( input, inOff, s, sOff );
        }

        private void fw( byte[] input )
        {
            cpyBytesToShort( input, this.wS );
            this.w_S[15] = (short)(this.wS[0] ^ this.wS[1] ^ this.wS[2] ^ this.wS[3] ^ this.wS[12] ^ this.wS[15]);
            Array.Copy( wS, 1, w_S, 0, 15 );
            cpyShortToBytes( this.w_S, input );
        }

        private void processBlock( byte[] input, int inOff )
        {
            Array.Copy( input, inOff, M, 0, 32 );
            this.H.CopyTo( U, 0 );
            this.M.CopyTo( V, 0 );
            for (int index = 0; index < 32; ++index)
                this.W[index] = (byte)(this.U[index] ^ (uint)this.V[index]);
            this.E( this.P( this.W ), this.S, 0, this.H, 0 );
            for (int index1 = 1; index1 < 4; ++index1)
            {
                byte[] numArray = this.A( this.U );
                for (int index2 = 0; index2 < 32; ++index2)
                    this.U[index2] = (byte)(numArray[index2] ^ (uint)this.C[index1][index2]);
                this.V = this.A( this.A( this.V ) );
                for (int index3 = 0; index3 < 32; ++index3)
                    this.W[index3] = (byte)(this.U[index3] ^ (uint)this.V[index3]);
                this.E( this.P( this.W ), this.S, index1 * 8, this.H, index1 * 8 );
            }
            for (int index = 0; index < 12; ++index)
                this.fw( this.S );
            for (int index = 0; index < 32; ++index)
                this.S[index] = (byte)(this.S[index] ^ (uint)this.M[index]);
            this.fw( this.S );
            for (int index = 0; index < 32; ++index)
                this.S[index] = (byte)(this.H[index] ^ (uint)this.S[index]);
            for (int index = 0; index < 61; ++index)
                this.fw( this.S );
            Array.Copy( S, 0, H, 0, this.H.Length );
        }

        private void finish()
        {
            Pack.UInt64_To_LE( this.byteCount * 8UL, this.L );
            while (this.xBufOff != 0)
                this.Update( 0 );
            this.processBlock( this.L, 0 );
            this.processBlock( this.Sum, 0 );
        }

        public int DoFinal( byte[] output, int outOff )
        {
            this.finish();
            this.H.CopyTo( output, outOff );
            this.Reset();
            return 32;
        }

        public void Reset()
        {
            this.byteCount = 0UL;
            this.xBufOff = 0;
            Array.Clear( H, 0, this.H.Length );
            Array.Clear( L, 0, this.L.Length );
            Array.Clear( M, 0, this.M.Length );
            Array.Clear( this.C[1], 0, this.C[1].Length );
            Array.Clear( this.C[3], 0, this.C[3].Length );
            Array.Clear( Sum, 0, this.Sum.Length );
            Array.Clear( xBuf, 0, this.xBuf.Length );
            C2.CopyTo( this.C[2], 0 );
        }

        private void sumByteArray( byte[] input )
        {
            int num1 = 0;
            for (int index = 0; index != this.Sum.Length; ++index)
            {
                int num2 = (this.Sum[index] & byte.MaxValue) + (input[index] & byte.MaxValue) + num1;
                this.Sum[index] = (byte)num2;
                num1 = num2 >> 8;
            }
        }

        private static void cpyBytesToShort( byte[] S, short[] wS )
        {
            for (int index = 0; index < S.Length / 2; ++index)
                wS[index] = (short)(((S[(index * 2) + 1] << 8) & 65280) | (S[index * 2] & byte.MaxValue));
        }

        private static void cpyShortToBytes( short[] wS, byte[] S )
        {
            for (int index = 0; index < S.Length / 2; ++index)
            {
                S[(index * 2) + 1] = (byte)((uint)wS[index] >> 8);
                S[index * 2] = (byte)wS[index];
            }
        }

        public int GetByteLength() => 32;

        public IMemoable Copy() => new Gost3411Digest( this );

        public void Reset( IMemoable other )
        {
            Gost3411Digest gost3411Digest = (Gost3411Digest)other;
            this.sBox = gost3411Digest.sBox;
            this.cipher.Init( true, new ParametersWithSBox( null, this.sBox ) );
            this.Reset();
            Array.Copy( gost3411Digest.H, 0, H, 0, gost3411Digest.H.Length );
            Array.Copy( gost3411Digest.L, 0, L, 0, gost3411Digest.L.Length );
            Array.Copy( gost3411Digest.M, 0, M, 0, gost3411Digest.M.Length );
            Array.Copy( gost3411Digest.Sum, 0, Sum, 0, gost3411Digest.Sum.Length );
            Array.Copy( gost3411Digest.C[1], 0, this.C[1], 0, gost3411Digest.C[1].Length );
            Array.Copy( gost3411Digest.C[2], 0, this.C[2], 0, gost3411Digest.C[2].Length );
            Array.Copy( gost3411Digest.C[3], 0, this.C[3], 0, gost3411Digest.C[3].Length );
            Array.Copy( gost3411Digest.xBuf, 0, xBuf, 0, gost3411Digest.xBuf.Length );
            this.xBufOff = gost3411Digest.xBufOff;
            this.byteCount = gost3411Digest.byteCount;
        }
    }
}
