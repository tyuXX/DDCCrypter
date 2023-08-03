// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.SipHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class SipHash : IMac
    {
        protected readonly int c;
        protected readonly int d;
        protected long k0;
        protected long k1;
        protected long v0;
        protected long v1;
        protected long v2;
        protected long v3;
        protected long m = 0;
        protected int wordPos = 0;
        protected int wordCount = 0;

        public SipHash()
          : this( 2, 4 )
        {
        }

        public SipHash( int c, int d )
        {
            this.c = c;
            this.d = d;
        }

        public virtual string AlgorithmName => "SipHash-" + c + "-" + d;

        public virtual int GetMacSize() => 8;

        public virtual void Init( ICipherParameters parameters )
        {
            byte[] bs = parameters is KeyParameter keyParameter ? keyParameter.GetKey() : throw new ArgumentException( "must be an instance of KeyParameter", nameof( parameters ) );
            this.k0 = bs.Length == 16 ? (long)Pack.LE_To_UInt64( bs, 0 ) : throw new ArgumentException( "must be a 128-bit key", nameof( parameters ) );
            this.k1 = (long)Pack.LE_To_UInt64( bs, 8 );
            this.Reset();
        }

        public virtual void Update( byte input )
        {
            this.m = this.m >>> 8 | ((long)input << 56);
            if (++this.wordPos != 8)
                return;
            this.ProcessMessageWord();
            this.wordPos = 0;
        }

        public virtual void BlockUpdate( byte[] input, int offset, int length )
        {
            int num1 = 0;
            int num2 = length & -8;
            if (this.wordPos == 0)
            {
                for (; num1 < num2; num1 += 8)
                {
                    this.m = (long)Pack.LE_To_UInt64( input, offset + num1 );
                    this.ProcessMessageWord();
                }
                for (; num1 < length; ++num1)
                    this.m = this.m >>> 8 | ((long)input[offset + num1] << 56);
                this.wordPos = length - num2;
            }
            else
            {
                int num3 = this.wordPos << 3;
                for (; num1 < num2; num1 += 8)
                {
                    ulong uint64 = Pack.LE_To_UInt64( input, offset + num1 );
                    this.m = ((long)uint64 << num3) | this.m >>> -num3;
                    this.ProcessMessageWord();
                    this.m = (long)uint64;
                }
                for (; num1 < length; ++num1)
                {
                    this.m = this.m >>> 8 | ((long)input[offset + num1] << 56);
                    if (++this.wordPos == 8)
                    {
                        this.ProcessMessageWord();
                        this.wordPos = 0;
                    }
                }
            }
        }

        public virtual long DoFinal()
        {
            this.m >>>= (7 - this.wordPos) << 3;
            this.m >>>= 8;
            this.m |= (long)((this.wordCount << 3) + this.wordPos) << 56;
            this.ProcessMessageWord();
            this.v2 ^= byte.MaxValue;
            this.ApplySipRounds( this.d );
            long num = this.v0 ^ this.v1 ^ this.v2 ^ this.v3;
            this.Reset();
            return num;
        }

        public virtual int DoFinal( byte[] output, int outOff )
        {
            Pack.UInt64_To_LE( (ulong)this.DoFinal(), output, outOff );
            return 8;
        }

        public virtual void Reset()
        {
            this.v0 = this.k0 ^ 8317987319222330741L;
            this.v1 = this.k1 ^ 7237128888997146477L;
            this.v2 = this.k0 ^ 7816392313619706465L;
            this.v3 = this.k1 ^ 8387220255154660723L;
            this.m = 0L;
            this.wordPos = 0;
            this.wordCount = 0;
        }

        protected virtual void ProcessMessageWord()
        {
            ++this.wordCount;
            this.v3 ^= this.m;
            this.ApplySipRounds( this.c );
            this.v0 ^= this.m;
        }

        protected virtual void ApplySipRounds( int n )
        {
            long num1 = this.v0;
            long x1 = this.v1;
            long num2 = this.v2;
            long x2 = this.v3;
            for (int index = 0; index < n; ++index)
            {
                long x3 = num1 + x1;
                long num3 = num2 + x2;
                long num4 = RotateLeft( x1, 13 );
                long num5 = RotateLeft( x2, 16 );
                long x4 = num4 ^ x3;
                long x5 = num5 ^ num3;
                long num6 = RotateLeft( x3, 32 );
                long x6 = num3 + x4;
                num1 = num6 + x5;
                long num7 = RotateLeft( x4, 17 );
                long num8 = RotateLeft( x5, 21 );
                x1 = num7 ^ x6;
                x2 = num8 ^ num1;
                num2 = RotateLeft( x6, 32 );
            }
            this.v0 = num1;
            this.v1 = x1;
            this.v2 = num2;
            this.v3 = x2;
        }

        protected static long RotateLeft( long x, int n )
        {
            ulong num = (ulong)x;
            return (long)((num << n) | (num >> -n));
        }
    }
}
