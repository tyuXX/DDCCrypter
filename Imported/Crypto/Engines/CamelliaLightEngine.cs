// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class CamelliaLightEngine : IBlockCipher
    {
        private const int BLOCK_SIZE = 16;
        private bool initialised;
        private bool _keyis128;
        private uint[] subkey = new uint[96];
        private uint[] kw = new uint[8];
        private uint[] ke = new uint[12];
        private uint[] state = new uint[4];
        private static readonly uint[] SIGMA = new uint[12]
        {
      2694735487U,
      1003262091U,
      3061508184U,
      1286239154U,
      3337565999U,
      3914302142U,
      1426019237U,
      4057165596U,
      283453434U,
      3731369245U,
      2958461122U,
      3018244605U
        };
        private static readonly byte[] SBOX1 = new byte[256]
        {
       112,
       130,
       44,
       236,
       179,
       39,
       192,
       229,
       228,
       133,
       87,
       53,
       234,
       12,
       174,
       65,
       35,
       239,
       107,
       147,
       69,
       25,
       165,
       33,
       237,
       14,
       79,
       78,
       29,
       101,
       146,
       189,
       134,
       184,
       175,
       143,
       124,
       235,
       31,
       206,
       62,
       48,
       220,
       95,
       94,
       197,
       11,
       26,
       166,
       225,
       57,
       202,
       213,
       71,
       93,
       61,
       217,
       1,
       90,
       214,
       81,
       86,
       108,
       77,
       139,
       13,
       154,
       102,
       251,
       204,
       176,
       45,
       116,
       18,
       43,
       32,
       240,
       177,
       132,
       153,
       223,
       76,
       203,
       194,
       52,
       126,
       118,
       5,
       109,
       183,
       169,
       49,
       209,
       23,
       4,
       215,
       20,
       88,
       58,
       97,
       222,
       27,
       17,
       28,
       50,
       15,
       156,
       22,
       83,
       24,
       242,
       34,
       254,
       68,
       207,
       178,
       195,
       181,
       122,
       145,
       36,
       8,
       232,
       168,
       96,
       252,
       105,
       80,
       170,
       208,
       160,
       125,
       161,
       137,
       98,
       151,
       84,
       91,
       30,
       149,
       224,
      byte.MaxValue,
       100,
       210,
       16,
       196,
       0,
       72,
       163,
       247,
       117,
       219,
       138,
       3,
       230,
       218,
       9,
       63,
       221,
       148,
       135,
       92,
       131,
       2,
       205,
       74,
       144,
       51,
       115,
       103,
       246,
       243,
       157,
       127,
       191,
       226,
       82,
       155,
       216,
       38,
       200,
       55,
       198,
       59,
       129,
       150,
       111,
       75,
       19,
       190,
       99,
       46,
       233,
       121,
       167,
       140,
       159,
       110,
       188,
       142,
       41,
       245,
       249,
       182,
       47,
       253,
       180,
       89,
       120,
       152,
       6,
       106,
       231,
       70,
       113,
       186,
       212,
       37,
       171,
       66,
       136,
       162,
       141,
       250,
       114,
       7,
       185,
       85,
       248,
       238,
       172,
       10,
       54,
       73,
       42,
       104,
       60,
       56,
       241,
       164,
       64,
       40,
       211,
       123,
       187,
       201,
       67,
       193,
       21,
       227,
       173,
       244,
       119,
       199,
       128,
       158
        };

        private static uint rightRotate( uint x, int s ) => (x >> s) + (x << (32 - s));

        private static uint leftRotate( uint x, int s ) => (x << s) + (x >> (32 - s));

        private static void roldq( int rot, uint[] ki, int ioff, uint[] ko, int ooff )
        {
            ko[ooff] = (ki[ioff] << rot) | (ki[1 + ioff] >> (32 - rot));
            ko[1 + ooff] = (ki[1 + ioff] << rot) | (ki[2 + ioff] >> (32 - rot));
            ko[2 + ooff] = (ki[2 + ioff] << rot) | (ki[3 + ioff] >> (32 - rot));
            ko[3 + ooff] = (ki[3 + ioff] << rot) | (ki[ioff] >> (32 - rot));
            ki[ioff] = ko[ooff];
            ki[1 + ioff] = ko[1 + ooff];
            ki[2 + ioff] = ko[2 + ooff];
            ki[3 + ioff] = ko[3 + ooff];
        }

        private static void decroldq( int rot, uint[] ki, int ioff, uint[] ko, int ooff )
        {
            ko[2 + ooff] = (ki[ioff] << rot) | (ki[1 + ioff] >> (32 - rot));
            ko[3 + ooff] = (ki[1 + ioff] << rot) | (ki[2 + ioff] >> (32 - rot));
            ko[ooff] = (ki[2 + ioff] << rot) | (ki[3 + ioff] >> (32 - rot));
            ko[1 + ooff] = (ki[3 + ioff] << rot) | (ki[ioff] >> (32 - rot));
            ki[ioff] = ko[2 + ooff];
            ki[1 + ioff] = ko[3 + ooff];
            ki[2 + ioff] = ko[ooff];
            ki[3 + ioff] = ko[1 + ooff];
        }

        private static void roldqo32( int rot, uint[] ki, int ioff, uint[] ko, int ooff )
        {
            ko[ooff] = (ki[1 + ioff] << (rot - 32)) | (ki[2 + ioff] >> (64 - rot));
            ko[1 + ooff] = (ki[2 + ioff] << (rot - 32)) | (ki[3 + ioff] >> (64 - rot));
            ko[2 + ooff] = (ki[3 + ioff] << (rot - 32)) | (ki[ioff] >> (64 - rot));
            ko[3 + ooff] = (ki[ioff] << (rot - 32)) | (ki[1 + ioff] >> (64 - rot));
            ki[ioff] = ko[ooff];
            ki[1 + ioff] = ko[1 + ooff];
            ki[2 + ioff] = ko[2 + ooff];
            ki[3 + ioff] = ko[3 + ooff];
        }

        private static void decroldqo32( int rot, uint[] ki, int ioff, uint[] ko, int ooff )
        {
            ko[2 + ooff] = (ki[1 + ioff] << (rot - 32)) | (ki[2 + ioff] >> (64 - rot));
            ko[3 + ooff] = (ki[2 + ioff] << (rot - 32)) | (ki[3 + ioff] >> (64 - rot));
            ko[ooff] = (ki[3 + ioff] << (rot - 32)) | (ki[ioff] >> (64 - rot));
            ko[1 + ooff] = (ki[ioff] << (rot - 32)) | (ki[1 + ioff] >> (64 - rot));
            ki[ioff] = ko[2 + ooff];
            ki[1 + ioff] = ko[3 + ooff];
            ki[2 + ioff] = ko[ooff];
            ki[3 + ioff] = ko[1 + ooff];
        }

        private static uint bytes2uint( byte[] src, int offset )
        {
            uint num = 0;
            for (int index = 0; index < 4; ++index)
                num = (num << 8) + src[index + offset];
            return num;
        }

        private static void uint2bytes( uint word, byte[] dst, int offset )
        {
            for (int index = 0; index < 4; ++index)
            {
                dst[3 - index + offset] = (byte)word;
                word >>= 8;
            }
        }

        private byte lRot8( byte v, int rot ) => (byte)(((uint)v << rot) | ((uint)v >> (8 - rot)));

        private uint sbox2( int x ) => this.lRot8( SBOX1[x], 1 );

        private uint sbox3( int x ) => this.lRot8( SBOX1[x], 7 );

        private uint sbox4( int x ) => SBOX1[this.lRot8( (byte)x, 1 )];

        private void camelliaF2( uint[] s, uint[] skey, int keyoff )
        {
            uint x1 = s[0] ^ skey[keyoff];
            uint num1 = this.sbox4( (byte)x1 ) | (this.sbox3( (byte)(x1 >> 8) ) << 8) | (this.sbox2( (byte)(x1 >> 16) ) << 16) | ((uint)SBOX1[(byte)(x1 >> 24)] << 24);
            uint index1 = s[1] ^ skey[1 + keyoff];
            uint x2 = leftRotate( SBOX1[(byte)index1] | (this.sbox4( (byte)(index1 >> 8) ) << 8) | (this.sbox3( (byte)(index1 >> 16) ) << 16) | (this.sbox2( (byte)(index1 >> 24) ) << 24), 8 );
            uint x3 = num1 ^ x2;
            uint x4 = leftRotate( x2, 8 ) ^ x3;
            uint x5 = rightRotate( x3, 8 ) ^ x4;
            uint[] numArray1;
            (numArray1 = s)[2] = numArray1[2] ^ leftRotate( x4, 16 ) ^ x5;
            uint[] numArray2;
            (numArray2 = s)[3] = numArray2[3] ^ leftRotate( x5, 8 );
            uint x6 = s[2] ^ skey[2 + keyoff];
            uint num2 = this.sbox4( (byte)x6 ) | (this.sbox3( (byte)(x6 >> 8) ) << 8) | (this.sbox2( (byte)(x6 >> 16) ) << 16) | ((uint)SBOX1[(byte)(x6 >> 24)] << 24);
            uint index2 = s[3] ^ skey[3 + keyoff];
            uint x7 = leftRotate( SBOX1[(byte)index2] | (this.sbox4( (byte)(index2 >> 8) ) << 8) | (this.sbox3( (byte)(index2 >> 16) ) << 16) | (this.sbox2( (byte)(index2 >> 24) ) << 24), 8 );
            uint x8 = num2 ^ x7;
            uint x9 = leftRotate( x7, 8 ) ^ x8;
            uint x10 = rightRotate( x8, 8 ) ^ x9;
            uint[] numArray3;
            (numArray3 = s)[0] = numArray3[0] ^ leftRotate( x9, 16 ) ^ x10;
            uint[] numArray4;
            (numArray4 = s)[1] = numArray4[1] ^ leftRotate( x10, 8 );
        }

        private void camelliaFLs( uint[] s, uint[] fkey, int keyoff )
        {
            uint[] numArray1;
            (numArray1 = s)[1] = numArray1[1] ^ leftRotate( s[0] & fkey[keyoff], 1 );
            uint[] numArray2;
            (numArray2 = s)[0] = numArray2[0] ^ (fkey[1 + keyoff] | s[1]);
            uint[] numArray3;
            (numArray3 = s)[2] = numArray3[2] ^ (fkey[3 + keyoff] | s[3]);
            uint[] numArray4;
            (numArray4 = s)[3] = numArray4[3] ^ leftRotate( fkey[2 + keyoff] & s[2], 1 );
        }

        private void setKey( bool forEncryption, byte[] key )
        {
            uint[] ki = new uint[8];
            uint[] numArray1 = new uint[4];
            uint[] numArray2 = new uint[4];
            uint[] ko = new uint[4];
            switch (key.Length)
            {
                case 16:
                    this._keyis128 = true;
                    ki[0] = bytes2uint( key, 0 );
                    ki[1] = bytes2uint( key, 4 );
                    ki[2] = bytes2uint( key, 8 );
                    ki[3] = bytes2uint( key, 12 );
                    ki[4] = ki[5] = ki[6] = ki[7] = 0U;
                    break;
                case 24:
                    ki[0] = bytes2uint( key, 0 );
                    ki[1] = bytes2uint( key, 4 );
                    ki[2] = bytes2uint( key, 8 );
                    ki[3] = bytes2uint( key, 12 );
                    ki[4] = bytes2uint( key, 16 );
                    ki[5] = bytes2uint( key, 20 );
                    ki[6] = ~ki[4];
                    ki[7] = ~ki[5];
                    this._keyis128 = false;
                    break;
                case 32:
                    ki[0] = bytes2uint( key, 0 );
                    ki[1] = bytes2uint( key, 4 );
                    ki[2] = bytes2uint( key, 8 );
                    ki[3] = bytes2uint( key, 12 );
                    ki[4] = bytes2uint( key, 16 );
                    ki[5] = bytes2uint( key, 20 );
                    ki[6] = bytes2uint( key, 24 );
                    ki[7] = bytes2uint( key, 28 );
                    this._keyis128 = false;
                    break;
                default:
                    throw new ArgumentException( "key sizes are only 16/24/32 bytes." );
            }
            for (int index = 0; index < 4; ++index)
                numArray1[index] = ki[index] ^ ki[index + 4];
            this.camelliaF2( numArray1, SIGMA, 0 );
            for (int index = 0; index < 4; ++index)
                numArray1[index] ^= ki[index];
            this.camelliaF2( numArray1, SIGMA, 4 );
            if (this._keyis128)
            {
                if (forEncryption)
                {
                    this.kw[0] = ki[0];
                    this.kw[1] = ki[1];
                    this.kw[2] = ki[2];
                    this.kw[3] = ki[3];
                    roldq( 15, ki, 0, this.subkey, 4 );
                    roldq( 30, ki, 0, this.subkey, 12 );
                    roldq( 15, ki, 0, ko, 0 );
                    this.subkey[18] = ko[2];
                    this.subkey[19] = ko[3];
                    roldq( 17, ki, 0, this.ke, 4 );
                    roldq( 17, ki, 0, this.subkey, 24 );
                    roldq( 17, ki, 0, this.subkey, 32 );
                    this.subkey[0] = numArray1[0];
                    this.subkey[1] = numArray1[1];
                    this.subkey[2] = numArray1[2];
                    this.subkey[3] = numArray1[3];
                    roldq( 15, numArray1, 0, this.subkey, 8 );
                    roldq( 15, numArray1, 0, this.ke, 0 );
                    roldq( 15, numArray1, 0, ko, 0 );
                    this.subkey[16] = ko[0];
                    this.subkey[17] = ko[1];
                    roldq( 15, numArray1, 0, this.subkey, 20 );
                    roldqo32( 34, numArray1, 0, this.subkey, 28 );
                    roldq( 17, numArray1, 0, this.kw, 4 );
                }
                else
                {
                    this.kw[4] = ki[0];
                    this.kw[5] = ki[1];
                    this.kw[6] = ki[2];
                    this.kw[7] = ki[3];
                    decroldq( 15, ki, 0, this.subkey, 28 );
                    decroldq( 30, ki, 0, this.subkey, 20 );
                    decroldq( 15, ki, 0, ko, 0 );
                    this.subkey[16] = ko[0];
                    this.subkey[17] = ko[1];
                    decroldq( 17, ki, 0, this.ke, 0 );
                    decroldq( 17, ki, 0, this.subkey, 8 );
                    decroldq( 17, ki, 0, this.subkey, 0 );
                    this.subkey[34] = numArray1[0];
                    this.subkey[35] = numArray1[1];
                    this.subkey[32] = numArray1[2];
                    this.subkey[33] = numArray1[3];
                    decroldq( 15, numArray1, 0, this.subkey, 24 );
                    decroldq( 15, numArray1, 0, this.ke, 4 );
                    decroldq( 15, numArray1, 0, ko, 0 );
                    this.subkey[18] = ko[2];
                    this.subkey[19] = ko[3];
                    decroldq( 15, numArray1, 0, this.subkey, 12 );
                    decroldqo32( 34, numArray1, 0, this.subkey, 4 );
                    roldq( 17, numArray1, 0, this.kw, 0 );
                }
            }
            else
            {
                for (int index = 0; index < 4; ++index)
                    numArray2[index] = numArray1[index] ^ ki[index + 4];
                this.camelliaF2( numArray2, SIGMA, 8 );
                if (forEncryption)
                {
                    this.kw[0] = ki[0];
                    this.kw[1] = ki[1];
                    this.kw[2] = ki[2];
                    this.kw[3] = ki[3];
                    roldqo32( 45, ki, 0, this.subkey, 16 );
                    roldq( 15, ki, 0, this.ke, 4 );
                    roldq( 17, ki, 0, this.subkey, 32 );
                    roldqo32( 34, ki, 0, this.subkey, 44 );
                    roldq( 15, ki, 4, this.subkey, 4 );
                    roldq( 15, ki, 4, this.ke, 0 );
                    roldq( 30, ki, 4, this.subkey, 24 );
                    roldqo32( 34, ki, 4, this.subkey, 36 );
                    roldq( 15, numArray1, 0, this.subkey, 8 );
                    roldq( 30, numArray1, 0, this.subkey, 20 );
                    this.ke[8] = numArray1[1];
                    this.ke[9] = numArray1[2];
                    this.ke[10] = numArray1[3];
                    this.ke[11] = numArray1[0];
                    roldqo32( 49, numArray1, 0, this.subkey, 40 );
                    this.subkey[0] = numArray2[0];
                    this.subkey[1] = numArray2[1];
                    this.subkey[2] = numArray2[2];
                    this.subkey[3] = numArray2[3];
                    roldq( 30, numArray2, 0, this.subkey, 12 );
                    roldq( 30, numArray2, 0, this.subkey, 28 );
                    roldqo32( 51, numArray2, 0, this.kw, 4 );
                }
                else
                {
                    this.kw[4] = ki[0];
                    this.kw[5] = ki[1];
                    this.kw[6] = ki[2];
                    this.kw[7] = ki[3];
                    decroldqo32( 45, ki, 0, this.subkey, 28 );
                    decroldq( 15, ki, 0, this.ke, 4 );
                    decroldq( 17, ki, 0, this.subkey, 12 );
                    decroldqo32( 34, ki, 0, this.subkey, 0 );
                    decroldq( 15, ki, 4, this.subkey, 40 );
                    decroldq( 15, ki, 4, this.ke, 8 );
                    decroldq( 30, ki, 4, this.subkey, 20 );
                    decroldqo32( 34, ki, 4, this.subkey, 8 );
                    decroldq( 15, numArray1, 0, this.subkey, 36 );
                    decroldq( 30, numArray1, 0, this.subkey, 24 );
                    this.ke[2] = numArray1[1];
                    this.ke[3] = numArray1[2];
                    this.ke[0] = numArray1[3];
                    this.ke[1] = numArray1[0];
                    decroldqo32( 49, numArray1, 0, this.subkey, 4 );
                    this.subkey[46] = numArray2[0];
                    this.subkey[47] = numArray2[1];
                    this.subkey[44] = numArray2[2];
                    this.subkey[45] = numArray2[3];
                    decroldq( 30, numArray2, 0, this.subkey, 32 );
                    decroldq( 30, numArray2, 0, this.subkey, 16 );
                    roldqo32( 51, numArray2, 0, this.kw, 0 );
                }
            }
        }

        private int processBlock128( byte[] input, int inOff, byte[] output, int outOff )
        {
            for (int index = 0; index < 4; ++index)
            {
                this.state[index] = bytes2uint( input, inOff + (index * 4) );
                this.state[index] ^= this.kw[index];
            }
            this.camelliaF2( this.state, this.subkey, 0 );
            this.camelliaF2( this.state, this.subkey, 4 );
            this.camelliaF2( this.state, this.subkey, 8 );
            this.camelliaFLs( this.state, this.ke, 0 );
            this.camelliaF2( this.state, this.subkey, 12 );
            this.camelliaF2( this.state, this.subkey, 16 );
            this.camelliaF2( this.state, this.subkey, 20 );
            this.camelliaFLs( this.state, this.ke, 4 );
            this.camelliaF2( this.state, this.subkey, 24 );
            this.camelliaF2( this.state, this.subkey, 28 );
            this.camelliaF2( this.state, this.subkey, 32 );
            uint[] state1;
            (state1 = this.state)[2] = state1[2] ^ this.kw[4];
            uint[] state2;
            (state2 = this.state)[3] = state2[3] ^ this.kw[5];
            uint[] state3;
            (state3 = this.state)[0] = state3[0] ^ this.kw[6];
            uint[] state4;
            (state4 = this.state)[1] = state4[1] ^ this.kw[7];
            uint2bytes( this.state[2], output, outOff );
            uint2bytes( this.state[3], output, outOff + 4 );
            uint2bytes( this.state[0], output, outOff + 8 );
            uint2bytes( this.state[1], output, outOff + 12 );
            return 16;
        }

        private int processBlock192or256( byte[] input, int inOff, byte[] output, int outOff )
        {
            for (int index = 0; index < 4; ++index)
            {
                this.state[index] = bytes2uint( input, inOff + (index * 4) );
                this.state[index] ^= this.kw[index];
            }
            this.camelliaF2( this.state, this.subkey, 0 );
            this.camelliaF2( this.state, this.subkey, 4 );
            this.camelliaF2( this.state, this.subkey, 8 );
            this.camelliaFLs( this.state, this.ke, 0 );
            this.camelliaF2( this.state, this.subkey, 12 );
            this.camelliaF2( this.state, this.subkey, 16 );
            this.camelliaF2( this.state, this.subkey, 20 );
            this.camelliaFLs( this.state, this.ke, 4 );
            this.camelliaF2( this.state, this.subkey, 24 );
            this.camelliaF2( this.state, this.subkey, 28 );
            this.camelliaF2( this.state, this.subkey, 32 );
            this.camelliaFLs( this.state, this.ke, 8 );
            this.camelliaF2( this.state, this.subkey, 36 );
            this.camelliaF2( this.state, this.subkey, 40 );
            this.camelliaF2( this.state, this.subkey, 44 );
            uint[] state1;
            (state1 = this.state)[2] = state1[2] ^ this.kw[4];
            uint[] state2;
            (state2 = this.state)[3] = state2[3] ^ this.kw[5];
            uint[] state3;
            (state3 = this.state)[0] = state3[0] ^ this.kw[6];
            uint[] state4;
            (state4 = this.state)[1] = state4[1] ^ this.kw[7];
            uint2bytes( this.state[2], output, outOff );
            uint2bytes( this.state[3], output, outOff + 4 );
            uint2bytes( this.state[0], output, outOff + 8 );
            uint2bytes( this.state[1], output, outOff + 12 );
            return 16;
        }

        public CamelliaLightEngine() => this.initialised = false;

        public virtual string AlgorithmName => "Camellia";

        public virtual bool IsPartialBlockOkay => false;

        public virtual int GetBlockSize() => 16;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException( "only simple KeyParameter expected." );
            this.setKey( forEncryption, ((KeyParameter)parameters).GetKey() );
            this.initialised = true;
        }

        public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (!this.initialised)
                throw new InvalidOperationException( "Camellia engine not initialised" );
            Check.DataLength( input, inOff, 16, "input buffer too short" );
            Check.OutputLength( output, outOff, 16, "output buffer too short" );
            return this._keyis128 ? this.processBlock128( input, inOff, output, outOff ) : this.processBlock192or256( input, inOff, output, outOff );
        }

        public virtual void Reset()
        {
        }
    }
}
