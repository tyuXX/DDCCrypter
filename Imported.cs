using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace DDCCrypter;
public class MD4Digest : GeneralDigest
{
    private const int DigestLength = 16;
    private const int S11 = 3;
    private const int S12 = 7;
    private const int S13 = 11;
    private const int S14 = 19;
    private const int S21 = 3;
    private const int S22 = 5;
    private const int S23 = 9;
    private const int S24 = 13;
    private const int S31 = 3;
    private const int S32 = 9;
    private const int S33 = 11;
    private const int S34 = 15;
    private int H1;
    private int H2;
    private int H3;
    private int H4;
    private int[] X = new int[16];
    private int xOff;

    public MD4Digest() => Reset();

    public MD4Digest( MD4Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( MD4Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "MD4";

    public override int GetDigestSize() => 16;

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff++] = (input[inOff] & byte.MaxValue) | ((input[inOff + 1] & byte.MaxValue) << 8) | ((input[inOff + 2] & byte.MaxValue) << 16) | ((input[inOff + 3] & byte.MaxValue) << 24);
        if (xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (int)(bitLength & uint.MaxValue);
        X[15] = (int)(bitLength >>> 32);
    }

    private void UnpackWord( int word, byte[] outBytes, int outOff )
    {
        outBytes[outOff] = (byte)word;
        outBytes[outOff + 1] = (byte)(word >>> 8);
        outBytes[outOff + 2] = (byte)(word >>> 16);
        outBytes[outOff + 3] = (byte)(word >>> 24);
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UnpackWord( H1, output, outOff );
        UnpackWord( H2, output, outOff + 4 );
        UnpackWord( H3, output, outOff + 8 );
        UnpackWord( H4, output, outOff + 12 );
        Reset();
        return 16;
    }

    public override void Reset()
    {
        base.Reset();
        H1 = 1732584193;
        H2 = -271733879;
        H3 = -1732584194;
        H4 = 271733878;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    private int RotateLeft( int x, int n ) => (x << n) | x >>> 32 - n;

    private int F( int u, int v, int w ) => (u & v) | (~u & w);

    private int G( int u, int v, int w ) => (u & v) | (u & w) | (v & w);

    private int H( int u, int v, int w ) => u ^ v ^ w;

    internal override void ProcessBlock()
    {
        int h1 = H1;
        int h2 = H2;
        int h3 = H3;
        int h4 = H4;
        int num1 = RotateLeft( h1 + F( h2, h3, h4 ) + X[0], 3 );
        int num2 = RotateLeft( h4 + F( num1, h2, h3 ) + X[1], 7 );
        int num3 = RotateLeft( h3 + F( num2, num1, h2 ) + X[2], 11 );
        int num4 = RotateLeft( h2 + F( num3, num2, num1 ) + X[3], 19 );
        int num5 = RotateLeft( num1 + F( num4, num3, num2 ) + X[4], 3 );
        int num6 = RotateLeft( num2 + F( num5, num4, num3 ) + X[5], 7 );
        int num7 = RotateLeft( num3 + F( num6, num5, num4 ) + X[6], 11 );
        int num8 = RotateLeft( num4 + F( num7, num6, num5 ) + X[7], 19 );
        int num9 = RotateLeft( num5 + F( num8, num7, num6 ) + X[8], 3 );
        int num10 = RotateLeft( num6 + F( num9, num8, num7 ) + X[9], 7 );
        int num11 = RotateLeft( num7 + F( num10, num9, num8 ) + X[10], 11 );
        int num12 = RotateLeft( num8 + F( num11, num10, num9 ) + X[11], 19 );
        int num13 = RotateLeft( num9 + F( num12, num11, num10 ) + X[12], 3 );
        int num14 = RotateLeft( num10 + F( num13, num12, num11 ) + X[13], 7 );
        int num15 = RotateLeft( num11 + F( num14, num13, num12 ) + X[14], 11 );
        int num16 = RotateLeft( num12 + F( num15, num14, num13 ) + X[15], 19 );
        int num17 = RotateLeft( num13 + G( num16, num15, num14 ) + X[0] + 1518500249, 3 );
        int num18 = RotateLeft( num14 + G( num17, num16, num15 ) + X[4] + 1518500249, 5 );
        int num19 = RotateLeft( num15 + G( num18, num17, num16 ) + X[8] + 1518500249, 9 );
        int num20 = RotateLeft( num16 + G( num19, num18, num17 ) + X[12] + 1518500249, 13 );
        int num21 = RotateLeft( num17 + G( num20, num19, num18 ) + X[1] + 1518500249, 3 );
        int num22 = RotateLeft( num18 + G( num21, num20, num19 ) + X[5] + 1518500249, 5 );
        int num23 = RotateLeft( num19 + G( num22, num21, num20 ) + X[9] + 1518500249, 9 );
        int num24 = RotateLeft( num20 + G( num23, num22, num21 ) + X[13] + 1518500249, 13 );
        int num25 = RotateLeft( num21 + G( num24, num23, num22 ) + X[2] + 1518500249, 3 );
        int num26 = RotateLeft( num22 + G( num25, num24, num23 ) + X[6] + 1518500249, 5 );
        int num27 = RotateLeft( num23 + G( num26, num25, num24 ) + X[10] + 1518500249, 9 );
        int num28 = RotateLeft( num24 + G( num27, num26, num25 ) + X[14] + 1518500249, 13 );
        int num29 = RotateLeft( num25 + G( num28, num27, num26 ) + X[3] + 1518500249, 3 );
        int num30 = RotateLeft( num26 + G( num29, num28, num27 ) + X[7] + 1518500249, 5 );
        int num31 = RotateLeft( num27 + G( num30, num29, num28 ) + X[11] + 1518500249, 9 );
        int num32 = RotateLeft( num28 + G( num31, num30, num29 ) + X[15] + 1518500249, 13 );
        int num33 = RotateLeft( num29 + H( num32, num31, num30 ) + X[0] + 1859775393, 3 );
        int num34 = RotateLeft( num30 + H( num33, num32, num31 ) + X[8] + 1859775393, 9 );
        int num35 = RotateLeft( num31 + H( num34, num33, num32 ) + X[4] + 1859775393, 11 );
        int num36 = RotateLeft( num32 + H( num35, num34, num33 ) + X[12] + 1859775393, 15 );
        int num37 = RotateLeft( num33 + H( num36, num35, num34 ) + X[2] + 1859775393, 3 );
        int num38 = RotateLeft( num34 + H( num37, num36, num35 ) + X[10] + 1859775393, 9 );
        int num39 = RotateLeft( num35 + H( num38, num37, num36 ) + X[6] + 1859775393, 11 );
        int num40 = RotateLeft( num36 + H( num39, num38, num37 ) + X[14] + 1859775393, 15 );
        int num41 = RotateLeft( num37 + H( num40, num39, num38 ) + X[1] + 1859775393, 3 );
        int num42 = RotateLeft( num38 + H( num41, num40, num39 ) + X[9] + 1859775393, 9 );
        int num43 = RotateLeft( num39 + H( num42, num41, num40 ) + X[5] + 1859775393, 11 );
        int num44 = RotateLeft( num40 + H( num43, num42, num41 ) + X[13] + 1859775393, 15 );
        int num45 = RotateLeft( num41 + H( num44, num43, num42 ) + X[3] + 1859775393, 3 );
        int num46 = RotateLeft( num42 + H( num45, num44, num43 ) + X[11] + 1859775393, 9 );
        int u = RotateLeft( num43 + H( num46, num45, num44 ) + X[7] + 1859775393, 11 );
        int num47 = RotateLeft( num44 + H( u, num46, num45 ) + X[15] + 1859775393, 15 );
        H1 += num45;
        H2 += num47;
        H3 += u;
        H4 += num46;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    public override IMemoable Copy() => new MD4Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (MD4Digest)other );
}
public abstract class GeneralDigest : IDigest, IMemoable
{
    private const int BYTE_LENGTH = 64;
    private byte[] xBuf;
    private int xBufOff;
    private long byteCount;

    internal GeneralDigest() => xBuf = new byte[4];

    internal GeneralDigest( GeneralDigest t )
    {
        xBuf = new byte[t.xBuf.Length];
        CopyIn( t );
    }

    protected void CopyIn( GeneralDigest t )
    {
        Array.Copy( t.xBuf, 0, xBuf, 0, t.xBuf.Length );
        xBufOff = t.xBufOff;
        byteCount = t.byteCount;
    }

    public void Update( byte input )
    {
        xBuf[xBufOff++] = input;
        if (xBufOff == xBuf.Length)
        {
            ProcessWord( xBuf, 0 );
            xBufOff = 0;
        }
        ++byteCount;
    }

    public void BlockUpdate( byte[] input, int inOff, int length )
    {
        length = Math.Max( 0, length );
        int num = 0;
        if (xBufOff != 0)
        {
            while (num < length)
            {
                xBuf[xBufOff++] = input[inOff + num++];
                if (xBufOff == 4)
                {
                    ProcessWord( xBuf, 0 );
                    xBufOff = 0;
                    break;
                }
            }
        }
        for (int index = ((length - num) & -4) + num; num < index; num += 4)
            ProcessWord( input, inOff + num );
        while (num < length)
            xBuf[xBufOff++] = input[inOff + num++];
        byteCount += length;
    }

    public void Finish()
    {
        long bitLength = byteCount << 3;
        Update( 128 );
        while (xBufOff != 0)
            Update( 0 );
        ProcessLength( bitLength );
        ProcessBlock();
    }

    public virtual void Reset()
    {
        byteCount = 0L;
        xBufOff = 0;
        Array.Clear( xBuf, 0, xBuf.Length );
    }

    public int GetByteLength() => 64;

    internal abstract void ProcessWord( byte[] input, int inOff );

    internal abstract void ProcessLength( long bitLength );

    internal abstract void ProcessBlock();

    public abstract string AlgorithmName { get; }

    public abstract int GetDigestSize();

    public abstract int DoFinal( byte[] output, int outOff );

    public abstract IMemoable Copy();

    public abstract void Reset( IMemoable t );
}
public interface IMemoable
{
    IMemoable Copy();

    void Reset( IMemoable other );
}
public interface IDigest
{
    string AlgorithmName { get; }

    int GetDigestSize();

    int GetByteLength();

    void Update( byte input );

    void BlockUpdate( byte[] input, int inOff, int length );

    int DoFinal( byte[] output, int outOff );

    void Reset();
}
public class MD2Digest : IDigest, IMemoable
{
    private const int DigestLength = 16;
    private const int BYTE_LENGTH = 16;
    private byte[] X = new byte[48];
    private int xOff;
    private byte[] M = new byte[16];
    private int mOff;
    private byte[] C = new byte[16];
    private int COff;
    private static readonly byte[] S = new byte[256]
    {
       41,
       46,
       67,
       201,
       162,
       216,
       124,
       1,
       61,
       54,
       84,
       161,
       236,
       240,
       6,
       19,
       98,
       167,
       5,
       243,
       192,
       199,
       115,
       140,
       152,
       147,
       43,
       217,
       188,
       76,
       130,
       202,
       30,
       155,
       87,
       60,
       253,
       212,
       224,
       22,
       103,
       66,
       111,
       24,
       138,
       23,
       229,
       18,
       190,
       78,
       196,
       214,
       218,
       158,
       222,
       73,
       160,
       251,
       245,
       142,
       187,
       47,
       238,
       122,
       169,
       104,
       121,
       145,
       21,
       178,
       7,
       63,
       148,
       194,
       16,
       137,
       11,
       34,
       95,
       33,
       128,
       127,
       93,
       154,
       90,
       144,
       50,
       39,
       53,
       62,
       204,
       231,
       191,
       247,
       151,
       3,
      byte.MaxValue,
       25,
       48,
       179,
       72,
       165,
       181,
       209,
       215,
       94,
       146,
       42,
       172,
       86,
       170,
       198,
       79,
       184,
       56,
       210,
       150,
       164,
       125,
       182,
       118,
       252,
       107,
       226,
       156,
       116,
       4,
       241,
       69,
       157,
       112,
       89,
       100,
       113,
       135,
       32,
       134,
       91,
       207,
       101,
       230,
       45,
       168,
       2,
       27,
       96,
       37,
       173,
       174,
       176,
       185,
       246,
       28,
       70,
       97,
       105,
       52,
       64,
       126,
       15,
       85,
       71,
       163,
       35,
       221,
       81,
       175,
       58,
       195,
       92,
       249,
       206,
       186,
       197,
       234,
       38,
       44,
       83,
       13,
       110,
       133,
       40,
       132,
       9,
       211,
       223,
       205,
       244,
       65,
       129,
       77,
       82,
       106,
       220,
       55,
       200,
       108,
       193,
       171,
       250,
       36,
       225,
       123,
       8,
       12,
       189,
       177,
       74,
       120,
       136,
       149,
       139,
       227,
       99,
       232,
       109,
       233,
       203,
       213,
       254,
       59,
       0,
       29,
       57,
       242,
       239,
       183,
       14,
       102,
       88,
       208,
       228,
       166,
       119,
       114,
       248,
       235,
       117,
       75,
       10,
       49,
       68,
       80,
       180,
       143,
       237,
       31,
       26,
       219,
       153,
       141,
       51,
       159,
       17,
       131,
       20
    };

    public MD2Digest() => Reset();

    public MD2Digest( MD2Digest t ) => CopyIn( t );

    private void CopyIn( MD2Digest t )
    {
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
        Array.Copy( t.M, 0, M, 0, t.M.Length );
        mOff = t.mOff;
        Array.Copy( t.C, 0, C, 0, t.C.Length );
        COff = t.COff;
    }

    public string AlgorithmName => "MD2";

    public int GetDigestSize() => 16;

    public int GetByteLength() => 16;

    public int DoFinal( byte[] output, int outOff )
    {
        byte num = (byte)(M.Length - mOff);
        for (int mOff = this.mOff; mOff < M.Length; ++mOff)
            M[mOff] = num;
        ProcessChecksum( M );
        ProcessBlock( M );
        ProcessBlock( C );
        Array.Copy( X, xOff, output, outOff, 16 );
        Reset();
        return 16;
    }

    public void Reset()
    {
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
        mOff = 0;
        for (int index = 0; index != M.Length; ++index)
            M[index] = 0;
        COff = 0;
        for (int index = 0; index != C.Length; ++index)
            C[index] = 0;
    }

    public void Update( byte input )
    {
        M[mOff++] = input;
        if (mOff != 16)
            return;
        ProcessChecksum( M );
        ProcessBlock( M );
        mOff = 0;
    }

    public void BlockUpdate( byte[] input, int inOff, int length )
    {
        for (; mOff != 0 && length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
        while (length > 16)
        {
            Array.Copy( input, inOff, M, 0, 16 );
            ProcessChecksum( M );
            ProcessBlock( M );
            length -= 16;
            inOff += 16;
        }
        for (; length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
    }

    internal void ProcessChecksum( byte[] m )
    {
        int num = C[15];
        for (int index1 = 0; index1 < 16; ++index1)
        {
            byte[] c;
            IntPtr index2;
            (c = C)[(int)(index2 = (IntPtr)index1)] = (byte)(c[(int)index2] ^ (uint)S[(m[index1] ^ num) & byte.MaxValue]);
            num = C[index1];
        }
    }

    internal void ProcessBlock( byte[] m )
    {
        for (int index = 0; index < 16; ++index)
        {
            X[index + 16] = m[index];
            X[index + 32] = (byte)(m[index] ^ (uint)X[index]);
        }
        int index1 = 0;
        for (int index2 = 0; index2 < 18; ++index2)
        {
            for (int index3 = 0; index3 < 48; ++index3)
            {
                byte[] x;
                IntPtr index4;
                index1 = ((x = X)[(int)(index4 = (IntPtr)index3)] = (byte)(x[(int)index4] ^ (uint)S[index1])) & byte.MaxValue;
            }
            index1 = (index1 + index2) % 256;
        }
    }

    public IMemoable Copy() => new MD2Digest( this );

    public void Reset( IMemoable other ) => CopyIn( (MD2Digest)other );
}
public class NullDigest : IDigest
{
    private readonly MemoryStream bOut = new MemoryStream();

    public string AlgorithmName => "NULL";

    public int GetByteLength() => 0;

    public int GetDigestSize() => (int)bOut.Length;

    public void Update( byte b ) => bOut.WriteByte( b );

    public void BlockUpdate( byte[] inBytes, int inOff, int len ) => bOut.Write( inBytes, inOff, len );

    public int DoFinal( byte[] outBytes, int outOff )
    {
        byte[] array = bOut.ToArray();
        array.CopyTo( outBytes, outOff );
        Reset();
        return array.Length;
    }

    public void Reset() => bOut.SetLength( 0L );
}
[Serializable]
public class BigInteger
{
    private const long IMASK = 4294967295;
    private const ulong UIMASK = 4294967295;
    private const int chunk2 = 1;
    private const int chunk8 = 1;
    private const int chunk10 = 19;
    private const int chunk16 = 16;
    private const int BitsPerByte = 8;
    private const int BitsPerInt = 32;
    private const int BytesPerInt = 4;
    internal static readonly int[][] primeLists = new int[64][]
    {
      new int[8]{ 3, 5, 7, 11, 13, 17, 19, 23 },
      new int[5]{ 29, 31, 37, 41, 43 },
      new int[5]{ 47, 53, 59, 61, 67 },
      new int[4]{ 71, 73, 79, 83 },
      new int[4]{ 89, 97, 101, 103 },
      new int[4]{ 107, 109, 113,  sbyte.MaxValue },
      new int[4]{ 131, 137, 139, 149 },
      new int[4]{ 151, 157, 163, 167 },
      new int[4]{ 173, 179, 181, 191 },
      new int[4]{ 193, 197, 199, 211 },
      new int[3]{ 223, 227, 229 },
      new int[3]{ 233, 239, 241 },
      new int[3]{ 251, 257, 263 },
      new int[3]{ 269, 271, 277 },
      new int[3]{ 281, 283, 293 },
      new int[3]{ 307, 311, 313 },
      new int[3]{ 317, 331, 337 },
      new int[3]{ 347, 349, 353 },
      new int[3]{ 359, 367, 373 },
      new int[3]{ 379, 383, 389 },
      new int[3]{ 397, 401, 409 },
      new int[3]{ 419, 421, 431 },
      new int[3]{ 433, 439, 443 },
      new int[3]{ 449, 457, 461 },
      new int[3]{ 463, 467, 479 },
      new int[3]{ 487, 491, 499 },
      new int[3]{ 503, 509, 521 },
      new int[3]{ 523, 541, 547 },
      new int[3]{ 557, 563, 569 },
      new int[3]{ 571, 577, 587 },
      new int[3]{ 593, 599, 601 },
      new int[3]{ 607, 613, 617 },
      new int[3]{ 619, 631, 641 },
      new int[3]{ 643, 647, 653 },
      new int[3]{ 659, 661, 673 },
      new int[3]{ 677, 683, 691 },
      new int[3]{ 701, 709, 719 },
      new int[3]{ 727, 733, 739 },
      new int[3]{ 743, 751, 757 },
      new int[3]{ 761, 769, 773 },
      new int[3]{ 787, 797, 809 },
      new int[3]{ 811, 821, 823 },
      new int[3]{ 827, 829, 839 },
      new int[3]{ 853, 857, 859 },
      new int[3]{ 863, 877, 881 },
      new int[3]{ 883, 887, 907 },
      new int[3]{ 911, 919, 929 },
      new int[3]{ 937, 941, 947 },
      new int[3]{ 953, 967, 971 },
      new int[3]{ 977, 983, 991 },
      new int[3]{ 997, 1009, 1013 },
      new int[3]{ 1019, 1021, 1031 },
      new int[3]{ 1033, 1039, 1049 },
      new int[3]{ 1051, 1061, 1063 },
      new int[3]{ 1069, 1087, 1091 },
      new int[3]{ 1093, 1097, 1103 },
      new int[3]{ 1109, 1117, 1123 },
      new int[3]{ 1129, 1151, 1153 },
      new int[3]{ 1163, 1171, 1181 },
      new int[3]{ 1187, 1193, 1201 },
      new int[3]{ 1213, 1217, 1223 },
      new int[3]{ 1229, 1231, 1237 },
      new int[3]{ 1249, 1259, 1277 },
      new int[3]{ 1279, 1283, 1289 }
    };
    internal static readonly int[] primeProducts;
    private static readonly int[] ZeroMagnitude = new int[0];
    private static readonly byte[] ZeroEncoding = new byte[0];
    private static readonly BigInteger[] SMALL_CONSTANTS = new BigInteger[17];
    public static readonly BigInteger Zero;
    public static readonly BigInteger One;
    public static readonly BigInteger Two;
    public static readonly BigInteger Three;
    public static readonly BigInteger Ten;
    private static readonly byte[] BitLengthTable = new byte[256]
    {
       0,
       1,
       2,
       2,
       3,
       3,
       3,
       3,
       4,
       4,
       4,
       4,
       4,
       4,
       4,
       4,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       5,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       6,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       7,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8,
       8
    };
    private static readonly BigInteger radix2;
    private static readonly BigInteger radix2E;
    private static readonly BigInteger radix8;
    private static readonly BigInteger radix8E;
    private static readonly BigInteger radix10;
    private static readonly BigInteger radix10E;
    private static readonly BigInteger radix16;
    private static readonly BigInteger radix16E;
    private static readonly SecureRandom RandomSource = new SecureRandom();
    private static readonly int[] ExpWindowThresholds = new int[8]
    {
      7,
      25,
      81,
      241,
      673,
      1793,
      4609,
      int.MaxValue
    };
    private int[] magnitude;
    private int sign;
    private int nBits = -1;
    private int nBitLength = -1;
    private int mQuote = 0;

    static BigInteger()
    {
        Zero = new BigInteger( 0, ZeroMagnitude, false )
        {
            nBits = 0,
            nBitLength = 0
        };
        SMALL_CONSTANTS[0] = Zero;
        for (uint index = 1; index < SMALL_CONSTANTS.Length; ++index)
            SMALL_CONSTANTS[(int)(IntPtr)index] = CreateUValueOf( index );
        One = SMALL_CONSTANTS[1];
        Two = SMALL_CONSTANTS[2];
        Three = SMALL_CONSTANTS[3];
        Ten = SMALL_CONSTANTS[10];
        radix2 = ValueOf( 2L );
        radix2E = radix2.Pow( 1 );
        radix8 = ValueOf( 8L );
        radix8E = radix8.Pow( 1 );
        radix10 = ValueOf( 10L );
        radix10E = radix10.Pow( 19 );
        radix16 = ValueOf( 16L );
        radix16E = radix16.Pow( 16 );
        primeProducts = new int[primeLists.Length];
        for (int index1 = 0; index1 < primeLists.Length; ++index1)
        {
            int[] primeList = primeLists[index1];
            int num = primeList[0];
            for (int index2 = 1; index2 < primeList.Length; ++index2)
                num *= primeList[index2];
            primeProducts[index1] = num;
        }
    }

    private static int GetByteLength( int nBits ) => (nBits + 8 - 1) / 8;

    internal static BigInteger Arbitrary( int sizeInBits ) => new BigInteger( sizeInBits, RandomSource );

    private BigInteger( int signum, int[] mag, bool checkMag )
    {
        if (checkMag)
        {
            int sourceIndex = 0;
            while (sourceIndex < mag.Length && mag[sourceIndex] == 0)
                ++sourceIndex;
            if (sourceIndex == mag.Length)
            {
                sign = 0;
                magnitude = ZeroMagnitude;
            }
            else
            {
                sign = signum;
                if (sourceIndex == 0)
                {
                    magnitude = mag;
                }
                else
                {
                    magnitude = new int[mag.Length - sourceIndex];
                    Array.Copy( mag, sourceIndex, magnitude, 0, magnitude.Length );
                }
            }
        }
        else
        {
            sign = signum;
            magnitude = mag;
        }
    }

    public BigInteger( string value )
      : this( value, 10 )
    {
    }

    public BigInteger( string str, int radix )
    {
        if (str.Length == 0)
            throw new FormatException( "Zero length BigInteger" );
        NumberStyles style;
        int length;
        BigInteger bigInteger1;
        BigInteger val;
        switch (radix)
        {
            case 2:
                style = NumberStyles.Integer;
                length = 1;
                bigInteger1 = radix2;
                val = radix2E;
                break;
            case 8:
                style = NumberStyles.Integer;
                length = 1;
                bigInteger1 = radix8;
                val = radix8E;
                break;
            case 10:
                style = NumberStyles.Integer;
                length = 19;
                bigInteger1 = radix10;
                val = radix10E;
                break;
            case 16:
                style = NumberStyles.AllowHexSpecifier;
                length = 16;
                bigInteger1 = radix16;
                val = radix16E;
                break;
            default:
                throw new FormatException( "Only bases 2, 8, 10, or 16 allowed" );
        }
        int num1 = 0;
        sign = 1;
        if (str[0] == '-')
        {
            if (str.Length == 1)
                throw new FormatException( "Zero length BigInteger" );
            sign = -1;
            num1 = 1;
        }
        while (num1 < str.Length && int.Parse( str[num1].ToString(), style ) == 0)
            ++num1;
        if (num1 >= str.Length)
        {
            sign = 0;
            magnitude = ZeroMagnitude;
        }
        else
        {
            BigInteger bigInteger2 = Zero;
            int num2 = num1 + length;
            if (num2 <= str.Length)
            {
                do
                {
                    string s = str.Substring( num1, length );
                    ulong num3 = ulong.Parse( s, style );
                    BigInteger uvalueOf = CreateUValueOf( num3 );
                    BigInteger bigInteger3;
                    switch (radix)
                    {
                        case 2:
                            if (num3 >= 2UL)
                                throw new FormatException( "Bad character in radix 2 string: " + s );
                            bigInteger3 = bigInteger2.ShiftLeft( 1 );
                            break;
                        case 8:
                            if (num3 >= 8UL)
                                throw new FormatException( "Bad character in radix 8 string: " + s );
                            bigInteger3 = bigInteger2.ShiftLeft( 3 );
                            break;
                        case 16:
                            bigInteger3 = bigInteger2.ShiftLeft( 64 );
                            break;
                        default:
                            bigInteger3 = bigInteger2.Multiply( val );
                            break;
                    }
                    bigInteger2 = bigInteger3.Add( uvalueOf );
                    num1 = num2;
                    num2 += length;
                }
                while (num2 <= str.Length);
            }
            if (num1 < str.Length)
            {
                string s = str.Substring( num1 );
                BigInteger uvalueOf = CreateUValueOf( ulong.Parse( s, style ) );
                if (bigInteger2.sign > 0)
                {
                    switch (radix)
                    {
                        case 2:
                        case 8:
                            bigInteger2 = bigInteger2.Add( uvalueOf );
                            break;
                        case 16:
                            bigInteger2 = bigInteger2.ShiftLeft( s.Length << 2 );
                            goto case 2;
                        default:
                            bigInteger2 = bigInteger2.Multiply( bigInteger1.Pow( s.Length ) );
                            goto case 2;
                    }
                }
                else
                    bigInteger2 = uvalueOf;
            }
            magnitude = bigInteger2.magnitude;
        }
    }

    public BigInteger( byte[] bytes )
      : this( bytes, 0, bytes.Length )
    {
    }

    public BigInteger( byte[] bytes, int offset, int length )
    {
        if (length == 0)
            throw new FormatException( "Zero length BigInteger" );
        if ((sbyte)bytes[offset] < 0)
        {
            sign = -1;
            int num = offset + length;
            int index1 = offset;
            while (index1 < num && bytes[index1] == byte.MaxValue)
                ++index1;
            if (index1 >= num)
            {
                magnitude = One.magnitude;
            }
            else
            {
                int length1 = num - index1;
                byte[] bytes1 = new byte[length1];
                int index2 = 0;
                while (index2 < length1)
                    bytes1[index2++] = (byte)~bytes[index1++];
                while (bytes1[--index2] == byte.MaxValue)
                    bytes1[index2] = 0;
                byte[] numArray;
                IntPtr index3;
                (numArray = bytes1)[(int)(index3 = (IntPtr)index2)] = (byte)(numArray[(int)index3] + 1U);
                magnitude = MakeMagnitude( bytes1, 0, bytes1.Length );
            }
        }
        else
        {
            magnitude = MakeMagnitude( bytes, offset, length );
            sign = magnitude.Length > 0 ? 1 : 0;
        }
    }

    private static int[] MakeMagnitude( byte[] bytes, int offset, int length )
    {
        int num1 = offset + length;
        int index1 = offset;
        while (index1 < num1 && bytes[index1] == 0)
            ++index1;
        if (index1 >= num1)
            return ZeroMagnitude;
        int length1 = (num1 - index1 + 3) / 4;
        int num2 = (num1 - index1) % 4;
        if (num2 == 0)
            num2 = 4;
        if (length1 < 1)
            return ZeroMagnitude;
        int[] numArray = new int[length1];
        int num3 = 0;
        int index2 = 0;
        for (int index3 = index1; index3 < num1; ++index3)
        {
            num3 = (num3 << 8) | (bytes[index3] & byte.MaxValue);
            --num2;
            if (num2 <= 0)
            {
                numArray[index2] = num3;
                ++index2;
                num2 = 4;
                num3 = 0;
            }
        }
        if (index2 < numArray.Length)
            numArray[index2] = num3;
        return numArray;
    }

    public BigInteger( int sign, byte[] bytes )
      : this( sign, bytes, 0, bytes.Length )
    {
    }

    public BigInteger( int sign, byte[] bytes, int offset, int length )
    {
        if (sign < -1 || sign > 1)
            throw new FormatException( "Invalid sign value" );
        if (sign == 0)
        {
            this.sign = 0;
            magnitude = ZeroMagnitude;
        }
        else
        {
            magnitude = MakeMagnitude( bytes, offset, length );
            this.sign = magnitude.Length < 1 ? 0 : sign;
        }
    }

    public BigInteger( int sizeInBits, Random random )
    {
        if (sizeInBits < 0)
            throw new ArgumentException( "sizeInBits must be non-negative" );
        nBits = -1;
        nBitLength = -1;
        if (sizeInBits == 0)
        {
            sign = 0;
            magnitude = ZeroMagnitude;
        }
        else
        {
            int byteLength = GetByteLength( sizeInBits );
            byte[] numArray1 = new byte[byteLength];
            random.NextBytes( numArray1 );
            int num = (8 * byteLength) - sizeInBits;
            byte[] numArray2;
            (numArray2 = numArray1)[0] = (byte)(numArray2[0] & (uint)(byte)((uint)byte.MaxValue >> num));
            magnitude = MakeMagnitude( numArray1, 0, numArray1.Length );
            sign = magnitude.Length < 1 ? 0 : 1;
        }
    }

    public BigInteger( int bitLength, int certainty, Random random )
    {
        if (bitLength < 2)
            throw new ArithmeticException( "bitLength < 2" );
        sign = 1;
        nBitLength = bitLength;
        if (bitLength == 2)
        {
            magnitude = random.Next( 2 ) == 0 ? Two.magnitude : Three.magnitude;
        }
        else
        {
            int byteLength = GetByteLength( bitLength );
            byte[] numArray1 = new byte[byteLength];
            int num1 = (8 * byteLength) - bitLength;
            byte num2 = (byte)((uint)byte.MaxValue >> num1);
            byte num3 = (byte)(1 << (7 - num1));
        label_5:
            random.NextBytes( numArray1 );
            byte[] numArray2;
            (numArray2 = numArray1)[0] = (byte)(numArray2[0] & (uint)num2);
            byte[] numArray3;
            (numArray3 = numArray1)[0] = (byte)(numArray3[0] | (uint)num3);
            byte[] numArray4;
            IntPtr index1;
            (numArray4 = numArray1)[(int)(index1 = (IntPtr)(byteLength - 1))] = (byte)(numArray4[(int)index1] | 1U);
            magnitude = MakeMagnitude( numArray1, 0, numArray1.Length );
            nBits = -1;
            mQuote = 0;
            if (certainty < 1 || CheckProbablePrime( certainty, random, true ))
                return;
            for (int index2 = 1; index2 < magnitude.Length - 1; ++index2)
            {
                magnitude[index2] ^= random.Next();
                if (CheckProbablePrime( certainty, random, true ))
                    return;
            }
            goto label_5;
        }
    }

    public BigInteger Abs() => sign < 0 ? Negate() : this;

    private static int[] AddMagnitudes( int[] a, int[] b )
    {
        int index1 = a.Length - 1;
        int num1 = b.Length - 1;
        long num2 = 0;
        while (num1 >= 0)
        {
            long num3 = num2 + (uint)a[index1] + (uint)b[num1--];
            a[index1--] = (int)num3;
            num2 = num3 >>> 32;
        }
        if (num2 != 0L)
        {
            int[] numArray;
            int index2;
            while (index1 >= 0 && ((numArray = a)[index2 = index1--] = numArray[(int)(IntPtr)index2] + 1) == 0)
                ;
        }
        return a;
    }

    public BigInteger Add( BigInteger value )
    {
        if (sign == 0)
            return value;
        if (sign == value.sign)
            return AddToMagnitude( value.magnitude );
        if (value.sign == 0)
            return this;
        return value.sign < 0 ? Subtract( value.Negate() ) : value.Subtract( Negate() );
    }

    private BigInteger AddToMagnitude( int[] magToAdd )
    {
        int[] numArray;
        int[] b;
        if (magnitude.Length < magToAdd.Length)
        {
            numArray = magToAdd;
            b = magnitude;
        }
        else
        {
            numArray = magnitude;
            b = magToAdd;
        }
        uint maxValue = uint.MaxValue;
        if (numArray.Length == b.Length)
            maxValue -= (uint)b[0];
        bool checkMag = (uint)numArray[0] >= maxValue;
        int[] a;
        if (checkMag)
        {
            a = new int[numArray.Length + 1];
            numArray.CopyTo( a, 1 );
        }
        else
            a = (int[])numArray.Clone();
        return new BigInteger( sign, AddMagnitudes( a, b ), checkMag );
    }

    public BigInteger And( BigInteger value )
    {
        if (sign == 0 || value.sign == 0)
            return Zero;
        int[] numArray1 = sign > 0 ? magnitude : Add( One ).magnitude;
        int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add( One ).magnitude;
        bool flag = sign < 0 && value.sign < 0;
        int[] mag = new int[System.Math.Max( numArray1.Length, numArray2.Length )];
        int num1 = mag.Length - numArray1.Length;
        int num2 = mag.Length - numArray2.Length;
        for (int index = 0; index < mag.Length; ++index)
        {
            int num3 = index >= num1 ? numArray1[index - num1] : 0;
            int num4 = index >= num2 ? numArray2[index - num2] : 0;
            if (sign < 0)
                num3 = ~num3;
            if (value.sign < 0)
                num4 = ~num4;
            mag[index] = num3 & num4;
            if (flag)
                mag[index] = ~mag[index];
        }
        BigInteger bigInteger = new BigInteger( 1, mag, true );
        if (flag)
            bigInteger = bigInteger.Not();
        return bigInteger;
    }

    public BigInteger AndNot( BigInteger val ) => And( val.Not() );

    public int BitCount
    {
        get
        {
            if (nBits == -1)
            {
                if (sign < 0)
                {
                    nBits = Not().BitCount;
                }
                else
                {
                    int num = 0;
                    for (int index = 0; index < magnitude.Length; ++index)
                        num += BitCnt( magnitude[index] );
                    nBits = num;
                }
            }
            return nBits;
        }
    }

    public static int BitCnt( int i )
    {
        uint num1 = (uint)i;
        uint num2 = num1 - ((num1 >> 1) & 1431655765U);
        uint num3 = (uint)(((int)num2 & 858993459) + ((int)(num2 >> 2) & 858993459));
        uint num4 = (uint)(((int)num3 + (int)(num3 >> 4)) & 252645135);
        uint num5 = num4 + (num4 >> 8);
        return (int)((num5 + (num5 >> 16)) & 63U);
    }

    private static int CalcBitLength( int sign, int indx, int[] mag )
    {
        for (; indx < mag.Length; ++indx)
        {
            if (mag[indx] != 0)
            {
                int num1 = 32 * (mag.Length - indx - 1);
                int w = mag[indx];
                int num2 = num1 + BitLen( w );
                if (sign < 0 && (w & -w) == w)
                {
                    while (++indx < mag.Length)
                    {
                        if (mag[indx] != 0)
                            goto label_8;
                    }
                    --num2;
                }
            label_8:
                return num2;
            }
        }
        return 0;
    }

    public int BitLength
    {
        get
        {
            if (nBitLength == -1)
                nBitLength = sign == 0 ? 0 : CalcBitLength( sign, 0, magnitude );
            return nBitLength;
        }
    }

    internal static int BitLen( int w )
    {
        uint index1 = (uint)w;
        uint index2 = index1 >> 24;
        if (index2 != 0U)
            return 24 + BitLengthTable[(int)(IntPtr)index2];
        uint index3 = index1 >> 16;
        if (index3 != 0U)
            return 16 + BitLengthTable[(int)(IntPtr)index3];
        uint index4 = index1 >> 8;
        return index4 != 0U ? 8 + BitLengthTable[(int)(IntPtr)index4] : BitLengthTable[(int)(IntPtr)index1];
    }

    private bool QuickPow2Check() => sign > 0 && nBits == 1;

    public int CompareTo( object obj ) => CompareTo( (BigInteger)obj );

    private static int CompareTo( int xIndx, int[] x, int yIndx, int[] y )
    {
        while (xIndx != x.Length && x[xIndx] == 0)
            ++xIndx;
        while (yIndx != y.Length && y[yIndx] == 0)
            ++yIndx;
        return CompareNoLeadingZeroes( xIndx, x, yIndx, y );
    }

    private static int CompareNoLeadingZeroes( int xIndx, int[] x, int yIndx, int[] y )
    {
        int num1 = x.Length - y.Length - (xIndx - yIndx);
        if (num1 != 0)
            return num1 >= 0 ? 1 : -1;
        while (xIndx < x.Length)
        {
            uint num2 = (uint)x[xIndx++];
            uint num3 = (uint)y[yIndx++];
            if ((int)num2 != (int)num3)
                return num2 >= num3 ? 1 : -1;
        }
        return 0;
    }

    public int CompareTo( BigInteger value )
    {
        if (sign < value.sign)
            return -1;
        if (sign > value.sign)
            return 1;
        return sign != 0 ? sign * CompareNoLeadingZeroes( 0, magnitude, 0, value.magnitude ) : 0;
    }

    private int[] Divide( int[] x, int[] y )
    {
        int index1 = 0;
        while (index1 < x.Length && x[index1] == 0)
            ++index1;
        int index2 = 0;
        while (index2 < y.Length && y[index2] == 0)
            ++index2;
        int num1 = CompareNoLeadingZeroes( index1, x, index2, y );
        int[] a;
        if (num1 > 0)
        {
            int num2 = CalcBitLength( 1, index2, y );
            int num3 = CalcBitLength( 1, index1, x );
            int n1 = num3 - num2;
            int start = 0;
            int index3 = 0;
            int num4 = num2;
            int[] numArray1;
            int[] numArray2;
            if (n1 > 0)
            {
                numArray1 = new int[(n1 >> 5) + 1];
                numArray1[0] = 1 << (n1 % 32);
                numArray2 = ShiftLeft( y, n1 );
                num4 += n1;
            }
            else
            {
                numArray1 = new int[1] { 1 };
                int length = y.Length - index2;
                numArray2 = new int[length];
                Array.Copy( y, index2, numArray2, 0, length );
            }
            a = new int[numArray1.Length];
        label_11:
            if (num4 < num3 || CompareNoLeadingZeroes( index1, x, index3, numArray2 ) >= 0)
            {
                Subtract( index1, x, index3, numArray2 );
                AddMagnitudes( a, numArray1 );
                while (x[index1] == 0)
                {
                    if (++index1 == x.Length)
                        return a;
                }
                num3 = (32 * (x.Length - index1 - 1)) + BitLen( x[index1] );
                if (num3 <= num2)
                {
                    if (num3 < num2)
                        return a;
                    num1 = CompareNoLeadingZeroes( index1, x, index2, y );
                    if (num1 <= 0)
                        goto label_30;
                }
            }
            int n2 = num4 - num3;
            if (n2 == 1 && (uint)(numArray2[index3] >>> 1) > (uint)x[index1])
                ++n2;
            if (n2 < 2)
            {
                ShiftRightOneInPlace( index3, numArray2 );
                --num4;
                ShiftRightOneInPlace( start, numArray1 );
            }
            else
            {
                ShiftRightInPlace( index3, numArray2, n2 );
                num4 -= n2;
                ShiftRightInPlace( start, numArray1, n2 );
            }
            while (numArray2[index3] == 0)
                ++index3;
            while (numArray1[start] == 0)
                ++start;
            goto label_11;
        }
        else
            a = new int[1];
        label_30:
        if (num1 == 0)
        {
            AddMagnitudes( a, One.magnitude );
            Array.Clear( x, index1, x.Length - index1 );
        }
        return a;
    }

    public BigInteger Divide( BigInteger val )
    {
        if (val.sign == 0)
            throw new ArithmeticException( "Division by zero error" );
        if (sign == 0)
            return Zero;
        if (val.QuickPow2Check())
        {
            BigInteger bigInteger = Abs().ShiftRight( val.Abs().BitLength - 1 );
            return val.sign != sign ? bigInteger.Negate() : bigInteger;
        }
        int[] x = (int[])magnitude.Clone();
        return new BigInteger( sign * val.sign, Divide( x, val.magnitude ), true );
    }

    public BigInteger[] DivideAndRemainder( BigInteger val )
    {
        if (val.sign == 0)
            throw new ArithmeticException( "Division by zero error" );
        BigInteger[] bigIntegerArray = new BigInteger[2];
        if (sign == 0)
        {
            bigIntegerArray[0] = Zero;
            bigIntegerArray[1] = Zero;
        }
        else if (val.QuickPow2Check())
        {
            int n = val.Abs().BitLength - 1;
            BigInteger bigInteger = Abs().ShiftRight( n );
            int[] mag = LastNBits( n );
            bigIntegerArray[0] = val.sign == sign ? bigInteger : bigInteger.Negate();
            bigIntegerArray[1] = new BigInteger( sign, mag, true );
        }
        else
        {
            int[] numArray = (int[])magnitude.Clone();
            int[] mag = Divide( numArray, val.magnitude );
            bigIntegerArray[0] = new BigInteger( sign * val.sign, mag, true );
            bigIntegerArray[1] = new BigInteger( sign, numArray, true );
        }
        return bigIntegerArray;
    }

    public override bool Equals( object obj )
    {
        if (obj == this)
            return true;
        return obj is BigInteger x && sign == x.sign && IsEqualMagnitude( x );
    }

    private bool IsEqualMagnitude( BigInteger x )
    {
        int[] magnitude = x.magnitude;
        if (this.magnitude.Length != x.magnitude.Length)
            return false;
        for (int index = 0; index < this.magnitude.Length; ++index)
        {
            if (this.magnitude[index] != x.magnitude[index])
                return false;
        }
        return true;
    }

    public BigInteger Gcd( BigInteger value )
    {
        if (value.sign == 0)
            return Abs();
        if (sign == 0)
            return value.Abs();
        BigInteger bigInteger1 = this;
        BigInteger bigInteger2;
        for (BigInteger m = value; m.sign != 0; m = bigInteger2)
        {
            bigInteger2 = bigInteger1.Mod( m );
            bigInteger1 = m;
        }
        return bigInteger1;
    }

    public override int GetHashCode()
    {
        int length = magnitude.Length;
        if (magnitude.Length > 0)
        {
            length ^= magnitude[0];
            if (magnitude.Length > 1)
                length ^= magnitude[magnitude.Length - 1];
        }
        return sign >= 0 ? length : ~length;
    }

    private BigInteger Inc()
    {
        if (sign == 0)
            return One;
        return sign < 0 ? new BigInteger( -1, doSubBigLil( magnitude, One.magnitude ), true ) : AddToMagnitude( One.magnitude );
    }

    public int IntValue
    {
        get
        {
            if (sign == 0)
                return 0;
            int num = magnitude[magnitude.Length - 1];
            return sign >= 0 ? num : -num;
        }
    }

    public bool IsProbablePrime( int certainty ) => IsProbablePrime( certainty, false );

    internal bool IsProbablePrime( int certainty, bool randomlySelected )
    {
        if (certainty <= 0)
            return true;
        BigInteger bigInteger = Abs();
        if (!bigInteger.TestBit( 0 ))
            return bigInteger.Equals( Two );
        return !bigInteger.Equals( One ) && bigInteger.CheckProbablePrime( certainty, RandomSource, randomlySelected );
    }

    private bool CheckProbablePrime( int certainty, Random random, bool randomlySelected )
    {
        int num1 = System.Math.Min( BitLength - 1, primeLists.Length );
        for (int index = 0; index < num1; ++index)
        {
            int num2 = Remainder( primeProducts[index] );
            foreach (int num3 in primeLists[index])
            {
                if (num2 % num3 == 0)
                    return BitLength < 16 && IntValue == num3;
            }
        }
        return RabinMillerTest( certainty, random, randomlySelected );
    }

    public bool RabinMillerTest( int certainty, Random random ) => RabinMillerTest( certainty, random, false );

    internal bool RabinMillerTest( int certainty, Random random, bool randomlySelected )
    {
        int bitLength = BitLength;
        int val2 = ((certainty - 1) / 2) + 1;
        if (randomlySelected)
        {
            int val1 = bitLength >= 1024 ? 4 : (bitLength >= 512 ? 8 : (bitLength >= 256 ? 16 : 50));
            val2 = certainty >= 100 ? val2 - 50 + val1 : System.Math.Min( val1, val2 );
        }
        BigInteger bigInteger1 = this;
        int lowestSetBitMaskFirst = bigInteger1.GetLowestSetBitMaskFirst( -2 );
        BigInteger e = bigInteger1.ShiftRight( lowestSetBitMaskFirst );
        BigInteger bigInteger2 = One.ShiftLeft( 32 * bigInteger1.magnitude.Length ).Remainder( bigInteger1 );
        BigInteger x = bigInteger1.Subtract( bigInteger2 );
        do
        {
            BigInteger b1;
            do
            {
                b1 = new BigInteger( bigInteger1.BitLength, random );
            }
            while (b1.sign == 0 || b1.CompareTo( bigInteger1 ) >= 0 || b1.IsEqualMagnitude( bigInteger2 ) || b1.IsEqualMagnitude( x ));
            BigInteger b2 = ModPowMonty( b1, e, bigInteger1, false );
            if (!b2.Equals( bigInteger2 ))
            {
                int num = 0;
                while (!b2.Equals( x ))
                {
                    if (++num == lowestSetBitMaskFirst)
                        return false;
                    b2 = ModPowMonty( b2, Two, bigInteger1, false );
                    if (b2.Equals( bigInteger2 ))
                        return false;
                }
            }
        }
        while (--val2 > 0);
        return true;
    }

    public long LongValue
    {
        get
        {
            if (sign == 0)
                return 0;
            int length = magnitude.Length;
            long num = magnitude[length - 1] & uint.MaxValue;
            if (length > 1)
                num |= (magnitude[length - 2] & uint.MaxValue) << 32;
            return sign >= 0 ? num : -num;
        }
    }

    public BigInteger Max( BigInteger value ) => CompareTo( value ) <= 0 ? value : this;

    public BigInteger Min( BigInteger value ) => CompareTo( value ) >= 0 ? value : this;

    public BigInteger Mod( BigInteger m )
    {
        BigInteger bigInteger = m.sign >= 1 ? Remainder( m ) : throw new ArithmeticException( "Modulus must be positive" );
        return bigInteger.sign < 0 ? bigInteger.Add( m ) : bigInteger;
    }

    public BigInteger ModInverse( BigInteger m )
    {
        if (m.sign < 1)
            throw new ArithmeticException( "Modulus must be positive" );
        if (m.QuickPow2Check())
            return ModInversePow2( m );
        BigInteger u1Out;
        if (!ExtEuclid( Remainder( m ), m, out u1Out ).Equals( One ))
            throw new ArithmeticException( "Numbers not relatively prime." );
        if (u1Out.sign < 0)
            u1Out = u1Out.Add( m );
        return u1Out;
    }

    private BigInteger ModInversePow2( BigInteger m )
    {
        if (!TestBit( 0 ))
            throw new ArithmeticException( "Numbers not relatively prime." );
        int num1 = m.BitLength - 1;
        long num2 = ModInverse64( LongValue );
        if (num1 < 64)
            num2 &= (1L << num1) - 1L;
        BigInteger bigInteger = ValueOf( num2 );
        if (num1 > 64)
        {
            BigInteger val = Remainder( m );
            int num3 = 64;
            do
            {
                BigInteger n = bigInteger.Multiply( val ).Remainder( m );
                bigInteger = bigInteger.Multiply( Two.Subtract( n ) ).Remainder( m );
                num3 <<= 1;
            }
            while (num3 < num1);
        }
        if (bigInteger.sign < 0)
            bigInteger = bigInteger.Add( m );
        return bigInteger;
    }

    private static int ModInverse32( int d )
    {
        int num1 = d + (((d + 1) & 4) << 1);
        int num2 = num1 * (2 - (d * num1));
        int num3 = num2 * (2 - (d * num2));
        return num3 * (2 - (d * num3));
    }

    private static long ModInverse64( long d )
    {
        long num1 = d + (((d + 1L) & 4L) << 1);
        long num2 = num1 * (2L - (d * num1));
        long num3 = num2 * (2L - (d * num2));
        long num4 = num3 * (2L - (d * num3));
        return num4 * (2L - (d * num4));
    }

    private static BigInteger ExtEuclid( BigInteger a, BigInteger b, out BigInteger u1Out )
    {
        BigInteger bigInteger1 = One;
        BigInteger bigInteger2 = Zero;
        BigInteger bigInteger3 = a;
        BigInteger val = b;
        if (val.sign > 0)
        {
            while (true)
            {
                BigInteger[] bigIntegerArray = bigInteger3.DivideAndRemainder( val );
                bigInteger3 = val;
                val = bigIntegerArray[1];
                BigInteger bigInteger4 = bigInteger1;
                bigInteger1 = bigInteger2;
                if (val.sign > 0)
                    bigInteger2 = bigInteger4.Subtract( bigInteger2.Multiply( bigIntegerArray[0] ) );
                else
                    break;
            }
        }
        u1Out = bigInteger1;
        return bigInteger3;
    }

    private static void ZeroOut( int[] x ) => Array.Clear( x, 0, x.Length );

    public BigInteger ModPow( BigInteger e, BigInteger m )
    {
        if (m.sign < 1)
            throw new ArithmeticException( "Modulus must be positive" );
        if (m.Equals( One ))
            return Zero;
        if (e.sign == 0)
            return One;
        if (sign == 0)
            return Zero;
        bool flag = e.sign < 0;
        if (flag)
            e = e.Negate();
        BigInteger b = Mod( m );
        if (!e.Equals( One ))
            b = (m.magnitude[m.magnitude.Length - 1] & 1) != 0 ? ModPowMonty( b, e, m, true ) : ModPowBarrett( b, e, m );
        if (flag)
            b = b.ModInverse( m );
        return b;
    }

    private static BigInteger ModPowBarrett( BigInteger b, BigInteger e, BigInteger m )
    {
        int length1 = m.magnitude.Length;
        BigInteger mr = One.ShiftLeft( (length1 + 1) << 5 );
        BigInteger yu = One.ShiftLeft( length1 << 6 ).Divide( m );
        int extraBits = 0;
        int bitLength = e.BitLength;
        while (bitLength > ExpWindowThresholds[extraBits])
            ++extraBits;
        int length2 = 1 << extraBits;
        BigInteger[] bigIntegerArray = new BigInteger[length2];
        bigIntegerArray[0] = b;
        BigInteger val = ReduceBarrett( b.Square(), m, mr, yu );
        for (int index = 1; index < length2; ++index)
            bigIntegerArray[index] = ReduceBarrett( bigIntegerArray[index - 1].Multiply( val ), m, mr, yu );
        int[] windowList = GetWindowList( e.magnitude, extraBits );
        int num1 = windowList[0];
        int num2 = num1 & byte.MaxValue;
        int num3 = num1 >> 8;
        BigInteger bigInteger;
        if (num2 == 1)
        {
            bigInteger = val;
            --num3;
        }
        else
            bigInteger = bigIntegerArray[num2 >> 1];
        int num4 = 1;
        while (true)
        {
            int[] numArray = windowList;
            int index1 = num4++;
            int num5;
            if ((num5 = numArray[index1]) != -1)
            {
                int index2 = num5 & byte.MaxValue;
                int num6 = num3 + BitLengthTable[index2];
                for (int index3 = 0; index3 < num6; ++index3)
                    bigInteger = ReduceBarrett( bigInteger.Square(), m, mr, yu );
                bigInteger = ReduceBarrett( bigInteger.Multiply( bigIntegerArray[index2 >> 1] ), m, mr, yu );
                num3 = num5 >> 8;
            }
            else
                break;
        }
        for (int index = 0; index < num3; ++index)
            bigInteger = ReduceBarrett( bigInteger.Square(), m, mr, yu );
        return bigInteger;
    }

    private static BigInteger ReduceBarrett(
      BigInteger x,
      BigInteger m,
      BigInteger mr,
      BigInteger yu )
    {
        int bitLength1 = x.BitLength;
        int bitLength2 = m.BitLength;
        if (bitLength1 < bitLength2)
            return x;
        if (bitLength1 - bitLength2 > 1)
        {
            int length = m.magnitude.Length;
            BigInteger bigInteger = x.DivideWords( length - 1 ).Multiply( yu ).DivideWords( length + 1 );
            x = x.RemainderWords( length + 1 ).Subtract( bigInteger.Multiply( m ).RemainderWords( length + 1 ) );
            if (x.sign < 0)
                x = x.Add( mr );
        }
        while (x.CompareTo( m ) >= 0)
            x = x.Subtract( m );
        return x;
    }

    private static BigInteger ModPowMonty( BigInteger b, BigInteger e, BigInteger m, bool convert )
    {
        int length1 = m.magnitude.Length;
        int n = 32 * length1;
        bool smallMontyModulus = m.BitLength + 2 <= n;
        uint mquote = (uint)m.GetMQuote();
        if (convert)
            b = b.ShiftLeft( n ).Remainder( m );
        int[] a = new int[length1 + 1];
        int[] data = b.magnitude;
        if (data.Length < length1)
        {
            int[] numArray = new int[length1];
            data.CopyTo( numArray, length1 - data.Length );
            data = numArray;
        }
        int extraBits = 0;
        if (e.magnitude.Length > 1 || e.BitCount > 2)
        {
            int bitLength = e.BitLength;
            while (bitLength > ExpWindowThresholds[extraBits])
                ++extraBits;
        }
        int length2 = 1 << extraBits;
        int[][] numArray1 = new int[length2][];
        numArray1[0] = data;
        int[] numArray2 = Arrays.Clone( data );
        SquareMonty( a, numArray2, m.magnitude, mquote, smallMontyModulus );
        for (int index = 1; index < length2; ++index)
        {
            numArray1[index] = Arrays.Clone( numArray1[index - 1] );
            MultiplyMonty( a, numArray1[index], numArray2, m.magnitude, mquote, smallMontyModulus );
        }
        int[] windowList = GetWindowList( e.magnitude, extraBits );
        int num1 = windowList[0];
        int num2 = num1 & byte.MaxValue;
        int num3 = num1 >> 8;
        int[] numArray3;
        if (num2 == 1)
        {
            numArray3 = numArray2;
            --num3;
        }
        else
            numArray3 = Arrays.Clone( numArray1[num2 >> 1] );
        int num4 = 1;
        while (true)
        {
            int[] numArray4 = windowList;
            int index1 = num4++;
            int num5;
            if ((num5 = numArray4[index1]) != -1)
            {
                int index2 = num5 & byte.MaxValue;
                int num6 = num3 + BitLengthTable[index2];
                for (int index3 = 0; index3 < num6; ++index3)
                    SquareMonty( a, numArray3, m.magnitude, mquote, smallMontyModulus );
                MultiplyMonty( a, numArray3, numArray1[index2 >> 1], m.magnitude, mquote, smallMontyModulus );
                num3 = num5 >> 8;
            }
            else
                break;
        }
        for (int index = 0; index < num3; ++index)
            SquareMonty( a, numArray3, m.magnitude, mquote, smallMontyModulus );
        if (convert)
            MontgomeryReduce( numArray3, m.magnitude, mquote );
        else if (smallMontyModulus && CompareTo( 0, numArray3, 0, m.magnitude ) >= 0)
            Subtract( 0, numArray3, 0, m.magnitude );
        return new BigInteger( 1, numArray3, true );
    }

    private static int[] GetWindowList( int[] mag, int extraBits )
    {
        int w = mag[0];
        int num1 = BitLen( w );
        int[] windowList = new int[((((mag.Length - 1) << 5) + num1) / (1 + extraBits)) + 2];
        int num2 = 0;
        int num3 = 33 - num1;
        int num4 = w << num3;
        int mult = 1;
        int num5 = 1 << extraBits;
        int zeroes = 0;
        int index1 = 0;
        while (true)
        {
            for (; num3 < 32; ++num3)
            {
                if (mult < num5)
                    mult = (mult << 1) | num4 >>> 31;
                else if (num4 < 0)
                {
                    windowList[num2++] = CreateWindowEntry( mult, zeroes );
                    mult = 1;
                    zeroes = 0;
                }
                else
                    ++zeroes;
                num4 <<= 1;
            }
            if (++index1 != mag.Length)
            {
                num4 = mag[index1];
                num3 = 0;
            }
            else
                break;
        }
        int[] numArray = windowList;
        int index2 = num2;
        int index3 = index2 + 1;
        int windowEntry = CreateWindowEntry( mult, zeroes );
        numArray[index2] = windowEntry;
        windowList[index3] = -1;
        return windowList;
    }

    private static int CreateWindowEntry( int mult, int zeroes )
    {
        while ((mult & 1) == 0)
        {
            mult >>= 1;
            ++zeroes;
        }
        return mult | (zeroes << 8);
    }

    private static int[] Square( int[] w, int[] x )
    {
        int index1 = w.Length - 1;
        for (int index2 = x.Length - 1; index2 > 0; --index2)
        {
            ulong num1 = (uint)x[index2];
            ulong num2 = (num1 * num1) + (uint)w[index1];
            w[index1] = (int)num2;
            ulong num3 = num2 >> 32;
            for (int index3 = index2 - 1; index3 >= 0; --index3)
            {
                ulong num4 = num1 * (uint)x[index3];
                ulong num5 = num3 + ((uint)w[--index1] & (ulong)uint.MaxValue) + ((uint)num4 << 1);
                w[index1] = (int)num5;
                num3 = (num5 >> 32) + (num4 >> 31);
            }
            int index4;
            ulong num6 = num3 + (uint)w[index4 = index1 - 1];
            w[index4] = (int)num6;
            int index5;
            if ((index5 = index4 - 1) >= 0)
                w[index5] = (int)(num6 >> 32);
            index1 = index5 + index2;
        }
        ulong num7 = (uint)x[0];
        ulong num8 = (num7 * num7) + (uint)w[index1];
        w[index1] = (int)num8;
        int index6;
        if ((index6 = index1 - 1) >= 0)
            w[index6] += (int)(num8 >> 32);
        return w;
    }

    private static int[] Multiply( int[] x, int[] y, int[] z )
    {
        int length = z.Length;
        if (length < 1)
            return x;
        int index1 = x.Length - y.Length;
        do
        {
            long num1 = z[--length] & uint.MaxValue;
            long num2 = 0;
            if (num1 != 0L)
            {
                for (int index2 = y.Length - 1; index2 >= 0; --index2)
                {
                    long num3 = num2 + (num1 * (y[index2] & uint.MaxValue)) + (x[index1 + index2] & uint.MaxValue);
                    x[index1 + index2] = (int)num3;
                    num2 = num3 >>> 32;
                }
            }
            --index1;
            if (index1 >= 0)
                x[index1] = (int)num2;
        }
        while (length > 0);
        return x;
    }

    private int GetMQuote() => mQuote != 0 ? mQuote : (mQuote = ModInverse32( -magnitude[magnitude.Length - 1] ));

    private static void MontgomeryReduce( int[] x, int[] m, uint mDash )
    {
        int length = m.Length;
        for (int index1 = length - 1; index1 >= 0; --index1)
        {
            uint num1 = (uint)x[length - 1];
            ulong num2 = num1 * mDash;
            ulong num3 = ((num2 * (uint)m[length - 1]) + num1) >> 32;
            for (int index2 = length - 2; index2 >= 0; --index2)
            {
                ulong num4 = num3 + (num2 * (uint)m[index2]) + (uint)x[index2];
                x[index2 + 1] = (int)num4;
                num3 = num4 >> 32;
            }
            x[0] = (int)num3;
        }
        if (CompareTo( 0, x, 0, m ) < 0)
            return;
        Subtract( 0, x, 0, m );
    }

    private static void MultiplyMonty(
      int[] a,
      int[] x,
      int[] y,
      int[] m,
      uint mDash,
      bool smallMontyModulus )
    {
        int length = m.Length;
        if (length == 1)
        {
            x[0] = (int)MultiplyMontyNIsOne( (uint)x[0], (uint)y[0], (uint)m[0], mDash );
        }
        else
        {
            uint num1 = (uint)y[length - 1];
            ulong num2 = (uint)x[length - 1];
            ulong num3 = num2 * num1;
            ulong num4 = (uint)num3 * mDash;
            ulong num5 = num4 * (uint)m[length - 1];
            ulong num6 = ((num3 + (uint)num5) >> 32) + (num5 >> 32);
            for (int index = length - 2; index >= 0; --index)
            {
                ulong num7 = num2 * (uint)y[index];
                ulong num8 = num4 * (uint)m[index];
                ulong num9 = num6 + (num7 & uint.MaxValue) + (uint)num8;
                a[index + 2] = (int)num9;
                num6 = (num9 >> 32) + (num7 >> 32) + (num8 >> 32);
            }
            a[1] = (int)num6;
            int num10 = (int)(num6 >> 32);
            for (int index1 = length - 2; index1 >= 0; --index1)
            {
                uint num11 = (uint)a[length];
                ulong num12 = (uint)x[index1];
                ulong num13 = num12 * num1;
                ulong num14 = (num13 & uint.MaxValue) + num11;
                ulong num15 = (uint)num14 * mDash;
                ulong num16 = num15 * (uint)m[length - 1];
                ulong num17 = ((num14 + (uint)num16) >> 32) + (num13 >> 32) + (num16 >> 32);
                for (int index2 = length - 2; index2 >= 0; --index2)
                {
                    ulong num18 = num12 * (uint)y[index2];
                    ulong num19 = num15 * (uint)m[index2];
                    ulong num20 = num17 + (num18 & uint.MaxValue) + (uint)num19 + (uint)a[index2 + 1];
                    a[index2 + 2] = (int)num20;
                    num17 = (num20 >> 32) + (num18 >> 32) + (num19 >> 32);
                }
                ulong num21 = num17 + (uint)num10;
                a[1] = (int)num21;
                num10 = (int)(num21 >> 32);
            }
            a[0] = num10;
            if (!smallMontyModulus && CompareTo( 0, a, 0, m ) >= 0)
                Subtract( 0, a, 0, m );
            Array.Copy( a, 1, x, 0, length );
        }
    }

    private static void SquareMonty(
      int[] a,
      int[] x,
      int[] m,
      uint mDash,
      bool smallMontyModulus )
    {
        int length = m.Length;
        if (length == 1)
        {
            uint num = (uint)x[0];
            x[0] = (int)MultiplyMontyNIsOne( num, num, (uint)m[0], mDash );
        }
        else
        {
            ulong num1 = (uint)x[length - 1];
            ulong num2 = num1 * num1;
            ulong num3 = (uint)num2 * mDash;
            ulong num4 = num3 * (uint)m[length - 1];
            ulong num5 = ((num2 + (uint)num4) >> 32) + (num4 >> 32);
            for (int index = length - 2; index >= 0; --index)
            {
                ulong num6 = num1 * (uint)x[index];
                ulong num7 = num3 * (uint)m[index];
                ulong num8 = num5 + (num7 & uint.MaxValue) + ((uint)num6 << 1);
                a[index + 2] = (int)num8;
                num5 = (num8 >> 32) + (num6 >> 31) + (num7 >> 32);
            }
            a[1] = (int)num5;
            int num9 = (int)(num5 >> 32);
            for (int index1 = length - 2; index1 >= 0; --index1)
            {
                uint num10 = (uint)a[length];
                ulong num11 = num10 * mDash;
                ulong num12 = ((num11 * (uint)m[length - 1]) + num10) >> 32;
                for (int index2 = length - 2; index2 > index1; --index2)
                {
                    ulong num13 = num12 + (num11 * (uint)m[index2]) + (uint)a[index2 + 1];
                    a[index2 + 2] = (int)num13;
                    num12 = num13 >> 32;
                }
                ulong num14 = (uint)x[index1];
                ulong num15 = num14 * num14;
                ulong num16 = num11 * (uint)m[index1];
                ulong num17 = num12 + (num15 & uint.MaxValue) + (uint)num16 + (uint)a[index1 + 1];
                a[index1 + 2] = (int)num17;
                ulong num18 = (num17 >> 32) + (num15 >> 32) + (num16 >> 32);
                for (int index3 = index1 - 1; index3 >= 0; --index3)
                {
                    ulong num19 = num14 * (uint)x[index3];
                    ulong num20 = num11 * (uint)m[index3];
                    ulong num21 = num18 + (num20 & uint.MaxValue) + ((uint)num19 << 1) + (uint)a[index3 + 1];
                    a[index3 + 2] = (int)num21;
                    num18 = (num21 >> 32) + (num19 >> 31) + (num20 >> 32);
                }
                ulong num22 = num18 + (uint)num9;
                a[1] = (int)num22;
                num9 = (int)(num22 >> 32);
            }
            a[0] = num9;
            if (!smallMontyModulus && CompareTo( 0, a, 0, m ) >= 0)
                Subtract( 0, a, 0, m );
            Array.Copy( a, 1, x, 0, length );
        }
    }

    private static uint MultiplyMontyNIsOne( uint x, uint y, uint m, uint mDash )
    {
        ulong num1 = x * (ulong)y;
        uint num2 = (uint)num1 * mDash;
        ulong num3 = m;
        ulong num4 = num3 * num2;
        ulong num5 = ((num1 + (uint)num4) >> 32) + (num4 >> 32);
        if (num5 > num3)
            num5 -= num3;
        return (uint)num5;
    }

    public BigInteger Multiply( BigInteger val )
    {
        if (val == this)
            return Square();
        if ((sign & val.sign) == 0)
            return Zero;
        if (val.QuickPow2Check())
        {
            BigInteger bigInteger = ShiftLeft( val.Abs().BitLength - 1 );
            return val.sign <= 0 ? bigInteger.Negate() : bigInteger;
        }
        if (QuickPow2Check())
        {
            BigInteger bigInteger = val.ShiftLeft( Abs().BitLength - 1 );
            return sign <= 0 ? bigInteger.Negate() : bigInteger;
        }
        int[] numArray = new int[magnitude.Length + val.magnitude.Length];
        Multiply( numArray, magnitude, val.magnitude );
        return new BigInteger( sign ^ val.sign ^ 1, numArray, true );
    }

    public BigInteger Square()
    {
        if (sign == 0)
            return Zero;
        if (QuickPow2Check())
            return ShiftLeft( Abs().BitLength - 1 );
        int length = magnitude.Length << 1;
        if (magnitude[0] >>> 16 == 0)
            --length;
        int[] numArray = new int[length];
        Square( numArray, magnitude );
        return new BigInteger( 1, numArray, false );
    }

    public BigInteger Negate() => sign == 0 ? this : new BigInteger( -sign, magnitude, false );

    public BigInteger NextProbablePrime()
    {
        if (sign < 0)
            throw new ArithmeticException( "Cannot be called on value < 0" );
        if (CompareTo( Two ) < 0)
            return Two;
        BigInteger bigInteger = Inc().SetBit( 0 );
        while (!bigInteger.CheckProbablePrime( 100, RandomSource, false ))
            bigInteger = bigInteger.Add( Two );
        return bigInteger;
    }

    public BigInteger Not() => Inc().Negate();

    public BigInteger Pow( int exp )
    {
        if (exp <= 0)
        {
            if (exp < 0)
                throw new ArithmeticException( "Negative exponent" );
            return One;
        }
        if (sign == 0)
            return this;
        if (QuickPow2Check())
        {
            long n = exp * (long)(BitLength - 1);
            return n <= int.MaxValue ? One.ShiftLeft( (int)n ) : throw new ArithmeticException( "Result too large" );
        }
        BigInteger bigInteger = One;
        BigInteger val = this;
        while (true)
        {
            if ((exp & 1) == 1)
                bigInteger = bigInteger.Multiply( val );
            exp >>= 1;
            if (exp != 0)
                val = val.Multiply( val );
            else
                break;
        }
        return bigInteger;
    }

    public static BigInteger ProbablePrime( int bitLength, Random random ) => new BigInteger( bitLength, 100, random );

    private int Remainder( int m )
    {
        long num1 = 0;
        for (int index = 0; index < magnitude.Length; ++index)
        {
            long num2 = (uint)magnitude[index];
            num1 = ((num1 << 32) | num2) % m;
        }
        return (int)num1;
    }

    private static int[] Remainder( int[] x, int[] y )
    {
        int index1 = 0;
        while (index1 < x.Length && x[index1] == 0)
            ++index1;
        int index2 = 0;
        while (index2 < y.Length && y[index2] == 0)
            ++index2;
        int num1 = CompareNoLeadingZeroes( index1, x, index2, y );
        if (num1 > 0)
        {
            int num2 = CalcBitLength( 1, index2, y );
            int num3 = CalcBitLength( 1, index1, x );
            int n1 = num3 - num2;
            int index3 = 0;
            int num4 = num2;
            int[] numArray;
            if (n1 > 0)
            {
                numArray = ShiftLeft( y, n1 );
                num4 += n1;
            }
            else
            {
                int length = y.Length - index2;
                numArray = new int[length];
                Array.Copy( y, index2, numArray, 0, length );
            }
        label_10:
            if (num4 < num3 || CompareNoLeadingZeroes( index1, x, index3, numArray ) >= 0)
            {
                Subtract( index1, x, index3, numArray );
                while (x[index1] == 0)
                {
                    if (++index1 == x.Length)
                        return x;
                }
                num3 = (32 * (x.Length - index1 - 1)) + BitLen( x[index1] );
                if (num3 <= num2)
                {
                    if (num3 < num2)
                        return x;
                    num1 = CompareNoLeadingZeroes( index1, x, index2, y );
                    if (num1 <= 0)
                        goto label_26;
                }
            }
            int n2 = num4 - num3;
            if (n2 == 1 && (uint)(numArray[index3] >>> 1) > (uint)x[index1])
                ++n2;
            if (n2 < 2)
            {
                ShiftRightOneInPlace( index3, numArray );
                --num4;
            }
            else
            {
                ShiftRightInPlace( index3, numArray, n2 );
                num4 -= n2;
            }
            while (numArray[index3] == 0)
                ++index3;
            goto label_10;
        }
    label_26:
        if (num1 == 0)
            Array.Clear( x, index1, x.Length - index1 );
        return x;
    }

    public BigInteger Remainder( BigInteger n )
    {
        if (n.sign == 0)
            throw new ArithmeticException( "Division by zero error" );
        if (sign == 0)
            return Zero;
        if (n.magnitude.Length == 1)
        {
            int m = n.magnitude[0];
            if (m > 0)
            {
                if (m == 1)
                    return Zero;
                int num = Remainder( m );
                if (num == 0)
                    return Zero;
                return new BigInteger( sign, new int[1] { num }, false );
            }
        }
        return CompareNoLeadingZeroes( 0, magnitude, 0, n.magnitude ) < 0 ? this : new BigInteger( sign, !n.QuickPow2Check() ? Remainder( (int[])magnitude.Clone(), n.magnitude ) : LastNBits( n.Abs().BitLength - 1 ), true );
    }

    private int[] LastNBits( int n )
    {
        if (n < 1)
            return ZeroMagnitude;
        int length = System.Math.Min( (n + 32 - 1) / 32, magnitude.Length );
        int[] destinationArray = new int[length];
        Array.Copy( magnitude, magnitude.Length - length, destinationArray, 0, length );
        int num = (length << 5) - n;
        if (num > 0)
        {
            int[] numArray;
            (numArray = destinationArray)[0] = numArray[0] & (int)(uint.MaxValue >> num);
        }
        return destinationArray;
    }

    private BigInteger DivideWords( int w )
    {
        int length = magnitude.Length;
        if (w >= length)
            return Zero;
        int[] numArray = new int[length - w];
        Array.Copy( magnitude, 0, numArray, 0, length - w );
        return new BigInteger( sign, numArray, false );
    }

    private BigInteger RemainderWords( int w )
    {
        int length = magnitude.Length;
        if (w >= length)
            return this;
        int[] numArray = new int[w];
        Array.Copy( magnitude, length - w, numArray, 0, w );
        return new BigInteger( sign, numArray, false );
    }

    private static int[] ShiftLeft( int[] mag, int n )
    {
        int num1 = n >>> 5;
        int num2 = n & 31;
        int length = mag.Length;
        int[] numArray;
        if (num2 == 0)
        {
            numArray = new int[length + num1];
            mag.CopyTo( numArray, 0 );
        }
        else
        {
            int index1 = 0;
            int num3 = 32 - num2;
            int num4 = mag[0] >>> num3;
            if (num4 != 0)
            {
                numArray = new int[length + num1 + 1];
                numArray[index1++] = num4;
            }
            else
                numArray = new int[length + num1];
            int num5 = mag[0];
            for (int index2 = 0; index2 < length - 1; ++index2)
            {
                int num6 = mag[index2 + 1];
                numArray[index1++] = (num5 << num2) | num6 >>> num3;
                num5 = num6;
            }
            numArray[index1] = mag[length - 1] << num2;
        }
        return numArray;
    }

    private static int ShiftLeftOneInPlace( int[] x, int carry )
    {
        int length = x.Length;
        while (--length >= 0)
        {
            uint num = (uint)x[length];
            x[length] = ((int)num << 1) | carry;
            carry = (int)(num >> 31);
        }
        return carry;
    }

    public BigInteger ShiftLeft( int n )
    {
        if (sign == 0 || magnitude.Length == 0)
            return Zero;
        if (n == 0)
            return this;
        if (n < 0)
            return ShiftRight( -n );
        BigInteger bigInteger = new BigInteger( sign, ShiftLeft( magnitude, n ), true );
        if (nBits != -1)
            bigInteger.nBits = sign > 0 ? nBits : nBits + n;
        if (nBitLength != -1)
            bigInteger.nBitLength = nBitLength + n;
        return bigInteger;
    }

    private static void ShiftRightInPlace( int start, int[] mag, int n )
    {
        int index1 = (n >>> 5) + start;
        int num1 = n & 31;
        int index2 = mag.Length - 1;
        if (index1 != start)
        {
            int num2 = index1 - start;
            for (int index3 = index2; index3 >= index1; --index3)
                mag[index3] = mag[index3 - num2];
            for (int index4 = index1 - 1; index4 >= start; --index4)
                mag[index4] = 0;
        }
        if (num1 == 0)
            return;
        int num3 = 32 - num1;
        int num4 = mag[index2];
        for (int index5 = index2; index5 > index1; --index5)
        {
            int num5 = mag[index5 - 1];
            mag[index5] = num4 >>> num1 | (num5 << num3);
            num4 = num5;
        }
        mag[index1] = mag[index1] >>> num1;
    }

    private static void ShiftRightOneInPlace( int start, int[] mag )
    {
        int length = mag.Length;
        int num1 = mag[length - 1];
        while (--length > start)
        {
            int num2 = mag[length - 1];
            mag[length] = num1 >>> 1 | (num2 << 31);
            num1 = num2;
        }
        mag[start] = mag[start] >>> 1;
    }

    public BigInteger ShiftRight( int n )
    {
        if (n == 0)
            return this;
        if (n < 0)
            return ShiftLeft( -n );
        if (n >= BitLength)
            return sign >= 0 ? Zero : One.Negate();
        int length = (BitLength - n + 31) >> 5;
        int[] numArray = new int[length];
        int num1 = n >> 5;
        int num2 = n & 31;
        if (num2 == 0)
        {
            Array.Copy( magnitude, 0, numArray, 0, numArray.Length );
        }
        else
        {
            int num3 = 32 - num2;
            int index1 = magnitude.Length - 1 - num1;
            for (int index2 = length - 1; index2 >= 0; --index2)
            {
                numArray[index2] = magnitude[index1--] >>> num2;
                if (index1 >= 0)
                    numArray[index2] |= magnitude[index1] << num3;
            }
        }
        return new BigInteger( sign, numArray, false );
    }

    public int SignValue => sign;

    private static int[] Subtract( int xStart, int[] x, int yStart, int[] y )
    {
        int index1 = x.Length;
        int length = y.Length;
        int num1 = 0;
        do
        {
            long num2 = (x[--index1] & uint.MaxValue) - (y[--length] & uint.MaxValue) + num1;
            x[index1] = (int)num2;
            num1 = (int)(num2 >> 63);
        }
        while (length > yStart);
        if (num1 != 0)
        {
            int num3;
            do
            {
                int[] numArray1;
                int[] numArray2 = numArray1 = x;
                int index2;
                index1 = index2 = index1 - 1;
                int index3 = index2;
                int num4;
                num3 = num4 = numArray2[(int)(IntPtr)index3] - 1;
                numArray1[index2] = num4;
            }
            while (num3 == -1);
        }
        return x;
    }

    public BigInteger Subtract( BigInteger n )
    {
        if (n.sign == 0)
            return this;
        if (sign == 0)
            return n.Negate();
        if (sign != n.sign)
            return Add( n.Negate() );
        int num = CompareNoLeadingZeroes( 0, magnitude, 0, n.magnitude );
        if (num == 0)
            return Zero;
        BigInteger bigInteger1;
        BigInteger bigInteger2;
        if (num < 0)
        {
            bigInteger1 = n;
            bigInteger2 = this;
        }
        else
        {
            bigInteger1 = this;
            bigInteger2 = n;
        }
        return new BigInteger( sign * num, doSubBigLil( bigInteger1.magnitude, bigInteger2.magnitude ), true );
    }

    private static int[] doSubBigLil( int[] bigMag, int[] lilMag ) => Subtract( 0, (int[])bigMag.Clone(), 0, lilMag );

    public byte[] ToByteArray() => ToByteArray( false );

    public byte[] ToByteArrayUnsigned() => ToByteArray( true );

    private byte[] ToByteArray( bool unsigned )
    {
        if (sign == 0)
            return !unsigned ? new byte[1] : ZeroEncoding;
        byte[] byteArray = new byte[GetByteLength( !unsigned || sign <= 0 ? BitLength + 1 : BitLength )];
        int length = magnitude.Length;
        int num1 = byteArray.Length;
        int num2;
        if (sign > 0)
        {
            while (length > 1)
            {
                uint num3 = (uint)magnitude[--length];
                int num4;
                byteArray[num4 = num1 - 1] = (byte)num3;
                int num5;
                byteArray[num5 = num4 - 1] = (byte)(num3 >> 8);
                int num6;
                byteArray[num6 = num5 - 1] = (byte)(num3 >> 16);
                byteArray[num1 = num6 - 1] = (byte)(num3 >> 24);
            }
            uint num7;
            for (num7 = (uint)magnitude[0]; num7 > byte.MaxValue; num7 >>= 8)
                byteArray[--num1] = (byte)num7;
            byteArray[num2 = num1 - 1] = (byte)num7;
        }
        else
        {
            bool flag = true;
            while (length > 1)
            {
                uint num8 = (uint)~magnitude[--length];
                if (flag)
                    flag = ++num8 == 0U;
                int num9;
                byteArray[num9 = num1 - 1] = (byte)num8;
                int num10;
                byteArray[num10 = num9 - 1] = (byte)(num8 >> 8);
                int num11;
                byteArray[num11 = num10 - 1] = (byte)(num8 >> 16);
                byteArray[num1 = num11 - 1] = (byte)(num8 >> 24);
            }
            uint num12 = (uint)magnitude[0];
            if (flag)
                --num12;
            for (; num12 > byte.MaxValue; num12 >>= 8)
                byteArray[--num1] = (byte)~num12;
            int num13;
            byteArray[num13 = num1 - 1] = (byte)~num12;
            if (num13 > 0)
                byteArray[num2 = num13 - 1] = byte.MaxValue;
        }
        return byteArray;
    }

    public override string ToString() => ToString( 10 );

    public string ToString( int radix )
    {
        switch (radix)
        {
            case 2:
            case 8:
            case 10:
            case 16:
                if (magnitude == null)
                    return "null";
                if (sign == 0)
                    return "0";
                int index1 = 0;
                while (index1 < magnitude.Length && magnitude[index1] == 0)
                    ++index1;
                if (index1 == magnitude.Length)
                    return "0";
                StringBuilder sb = new StringBuilder();
                if (sign == -1)
                    sb.Append( '-' );
                switch (radix)
                {
                    case 2:
                        int index2 = index1;
                        sb.Append( Convert.ToString( magnitude[index2], 2 ) );
                        while (++index2 < magnitude.Length)
                            AppendZeroExtendedString( sb, Convert.ToString( magnitude[index2], 2 ), 32 );
                        break;
                    case 8:
                        int num1 = 1073741823;
                        BigInteger bigInteger1 = Abs();
                        int bitLength = bigInteger1.BitLength;
                        IList arrayList1 = Platform.CreateArrayList();
                        for (; bitLength > 30; bitLength -= 30)
                        {
                            arrayList1.Add( Convert.ToString( bigInteger1.IntValue & num1, 8 ) );
                            bigInteger1 = bigInteger1.ShiftRight( 30 );
                        }
                        sb.Append( Convert.ToString( bigInteger1.IntValue, 8 ) );
                        for (int index3 = arrayList1.Count - 1; index3 >= 0; --index3)
                            AppendZeroExtendedString( sb, (string)arrayList1[index3], 10 );
                        break;
                    case 10:
                        BigInteger bigInteger2 = Abs();
                        if (bigInteger2.BitLength < 64)
                        {
                            sb.Append( Convert.ToString( bigInteger2.LongValue, radix ) );
                            break;
                        }
                        long num2 = long.MaxValue / radix;
                        long num3 = radix;
                        int minLength = 1;
                        while (num3 <= num2)
                        {
                            num3 *= radix;
                            ++minLength;
                        }
                        BigInteger val = ValueOf( num3 );
                        IList arrayList2 = Platform.CreateArrayList();
                        BigInteger[] bigIntegerArray;
                        for (; bigInteger2.CompareTo( val ) >= 0; bigInteger2 = bigIntegerArray[0])
                        {
                            bigIntegerArray = bigInteger2.DivideAndRemainder( val );
                            arrayList2.Add( Convert.ToString( bigIntegerArray[1].LongValue, radix ) );
                        }
                        sb.Append( Convert.ToString( bigInteger2.LongValue, radix ) );
                        for (int index4 = arrayList2.Count - 1; index4 >= 0; --index4)
                            AppendZeroExtendedString( sb, (string)arrayList2[index4], minLength );
                        break;
                    case 16:
                        int index5 = index1;
                        sb.Append( Convert.ToString( magnitude[index5], 16 ) );
                        while (++index5 < magnitude.Length)
                            AppendZeroExtendedString( sb, Convert.ToString( magnitude[index5], 16 ), 8 );
                        break;
                }
                return sb.ToString();
            default:
                throw new FormatException( "Only bases 2, 8, 10, 16 are allowed" );
        }
    }

    private static void AppendZeroExtendedString( StringBuilder sb, string s, int minLength )
    {
        for (int length = s.Length; length < minLength; ++length)
            sb.Append( '0' );
        sb.Append( s );
    }

    private static BigInteger CreateUValueOf( ulong value )
    {
        int num1 = (int)(value >> 32);
        int num2 = (int)value;
        if (num1 != 0)
            return new BigInteger( 1, new int[2] { num1, num2 }, false );
        if (num2 == 0)
            return Zero;
        BigInteger uvalueOf = new BigInteger( 1, new int[1]
        {
        num2
        }, false );
        if ((num2 & -num2) == num2)
            uvalueOf.nBits = 1;
        return uvalueOf;
    }

    private static BigInteger CreateValueOf( long value )
    {
        if (value >= 0L)
            return CreateUValueOf( (ulong)value );
        return value == long.MinValue ? CreateValueOf( ~value ).Not() : CreateValueOf( -value ).Negate();
    }

    public static BigInteger ValueOf( long value ) => value >= 0L && value < SMALL_CONSTANTS.Length ? SMALL_CONSTANTS[value] : CreateValueOf( value );

    public int GetLowestSetBit() => sign == 0 ? -1 : GetLowestSetBitMaskFirst( -1 );

    private int GetLowestSetBitMaskFirst( int firstWordMask )
    {
        int length = magnitude.Length;
        int lowestSetBitMaskFirst = 0;
        int num1;
        uint num2 = (uint)(magnitude[num1 = length - 1] & firstWordMask);
        while (num2 == 0U)
        {
            num2 = (uint)magnitude[--num1];
            lowestSetBitMaskFirst += 32;
        }
        while (((int)num2 & byte.MaxValue) == 0)
        {
            num2 >>= 8;
            lowestSetBitMaskFirst += 8;
        }
        while (((int)num2 & 1) == 0)
        {
            num2 >>= 1;
            ++lowestSetBitMaskFirst;
        }
        return lowestSetBitMaskFirst;
    }

    public bool TestBit( int n )
    {
        if (n < 0)
            throw new ArithmeticException( "Bit position must not be negative" );
        if (sign < 0)
            return !Not().TestBit( n );
        int num = n / 32;
        return num < magnitude.Length && ((magnitude[magnitude.Length - 1 - num] >> (n % 32)) & 1) > 0;
    }

    public BigInteger Or( BigInteger value )
    {
        if (sign == 0)
            return value;
        if (value.sign == 0)
            return this;
        int[] numArray1 = sign > 0 ? magnitude : Add( One ).magnitude;
        int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add( One ).magnitude;
        bool flag = sign < 0 || value.sign < 0;
        int[] mag = new int[System.Math.Max( numArray1.Length, numArray2.Length )];
        int num1 = mag.Length - numArray1.Length;
        int num2 = mag.Length - numArray2.Length;
        for (int index = 0; index < mag.Length; ++index)
        {
            int num3 = index >= num1 ? numArray1[index - num1] : 0;
            int num4 = index >= num2 ? numArray2[index - num2] : 0;
            if (sign < 0)
                num3 = ~num3;
            if (value.sign < 0)
                num4 = ~num4;
            mag[index] = num3 | num4;
            if (flag)
                mag[index] = ~mag[index];
        }
        BigInteger bigInteger = new BigInteger( 1, mag, true );
        if (flag)
            bigInteger = bigInteger.Not();
        return bigInteger;
    }

    public BigInteger Xor( BigInteger value )
    {
        if (sign == 0)
            return value;
        if (value.sign == 0)
            return this;
        int[] numArray1 = sign > 0 ? magnitude : Add( One ).magnitude;
        int[] numArray2 = value.sign > 0 ? value.magnitude : value.Add( One ).magnitude;
        bool flag = (sign < 0 && value.sign >= 0) || (sign >= 0 && value.sign < 0);
        int[] mag = new int[System.Math.Max( numArray1.Length, numArray2.Length )];
        int num1 = mag.Length - numArray1.Length;
        int num2 = mag.Length - numArray2.Length;
        for (int index = 0; index < mag.Length; ++index)
        {
            int num3 = index >= num1 ? numArray1[index - num1] : 0;
            int num4 = index >= num2 ? numArray2[index - num2] : 0;
            if (sign < 0)
                num3 = ~num3;
            if (value.sign < 0)
                num4 = ~num4;
            mag[index] = num3 ^ num4;
            if (flag)
                mag[index] = ~mag[index];
        }
        BigInteger bigInteger = new BigInteger( 1, mag, true );
        if (flag)
            bigInteger = bigInteger.Not();
        return bigInteger;
    }

    public BigInteger SetBit( int n )
    {
        if (n < 0)
            throw new ArithmeticException( "Bit address less than zero" );
        if (TestBit( n ))
            return this;
        return sign > 0 && n < BitLength - 1 ? FlipExistingBit( n ) : Or( One.ShiftLeft( n ) );
    }

    public BigInteger ClearBit( int n )
    {
        if (n < 0)
            throw new ArithmeticException( "Bit address less than zero" );
        if (!TestBit( n ))
            return this;
        return sign > 0 && n < BitLength - 1 ? FlipExistingBit( n ) : AndNot( One.ShiftLeft( n ) );
    }

    public BigInteger FlipBit( int n )
    {
        if (n < 0)
            throw new ArithmeticException( "Bit address less than zero" );
        return sign > 0 && n < BitLength - 1 ? FlipExistingBit( n ) : Xor( One.ShiftLeft( n ) );
    }

    private BigInteger FlipExistingBit( int n )
    {
        int[] mag = (int[])magnitude.Clone();
        mag[mag.Length - 1 - (n >> 5)] ^= 1 << n;
        return new BigInteger( sign, mag, false );
    }
}
public abstract class Primes
{
    public static readonly int SmallFactorLimit = 211;
    private static readonly BigInteger One = BigInteger.One;
    private static readonly BigInteger Two = BigInteger.Two;
    private static readonly BigInteger Three = BigInteger.Three;

    public static STOutput GenerateSTRandomPrime( IDigest hash, int length, byte[] inputSeed )
    {
        if (hash == null)
            throw new ArgumentNullException( nameof( hash ) );
        if (length < 2)
            throw new ArgumentException( "must be >= 2", nameof( length ) );
        if (inputSeed == null)
            throw new ArgumentNullException( nameof( inputSeed ) );
        if (inputSeed.Length == 0)
            throw new ArgumentException( "cannot be empty", nameof( inputSeed ) );
        return ImplSTRandomPrime( hash, length, Arrays.Clone( inputSeed ) );
    }

    public static MROutput EnhancedMRProbablePrimeTest(
      BigInteger candidate,
      SecureRandom random,
      int iterations )
    {
        CheckCandidate( candidate, nameof( candidate ) );
        if (random == null)
            throw new ArgumentNullException( nameof( random ) );
        if (iterations < 1)
            throw new ArgumentException( "must be > 0", nameof( iterations ) );
        if (candidate.BitLength == 2)
            return MROutput.ProbablyPrime();
        if (!candidate.TestBit( 0 ))
            return MROutput.ProvablyCompositeWithFactor( Two );
        BigInteger m = candidate;
        BigInteger bigInteger1 = candidate.Subtract( One );
        BigInteger max = candidate.Subtract( Two );
        int lowestSetBit = bigInteger1.GetLowestSetBit();
        BigInteger e = bigInteger1.ShiftRight( lowestSetBit );
        for (int index1 = 0; index1 < iterations; ++index1)
        {
            BigInteger randomInRange = BigIntegers.CreateRandomInRange( Two, max, random );
            BigInteger factor1 = randomInRange.Gcd( m );
            if (factor1.CompareTo( One ) > 0)
                return MROutput.ProvablyCompositeWithFactor( factor1 );
            BigInteger bigInteger2 = randomInRange.ModPow( e, m );
            if (!bigInteger2.Equals( One ) && !bigInteger2.Equals( bigInteger1 ))
            {
                bool flag = false;
                BigInteger bigInteger3 = bigInteger2;
                for (int index2 = 1; index2 < lowestSetBit; ++index2)
                {
                    bigInteger2 = bigInteger2.ModPow( Two, m );
                    if (bigInteger2.Equals( bigInteger1 ))
                    {
                        flag = true;
                        break;
                    }
                    if (!bigInteger2.Equals( One ))
                        bigInteger3 = bigInteger2;
                    else
                        break;
                }
                if (!flag)
                {
                    if (!bigInteger2.Equals( One ))
                    {
                        bigInteger3 = bigInteger2;
                        BigInteger bigInteger4 = bigInteger2.ModPow( Two, m );
                        if (!bigInteger4.Equals( One ))
                            bigInteger3 = bigInteger4;
                    }
                    BigInteger factor2 = bigInteger3.Subtract( One ).Gcd( m );
                    return factor2.CompareTo( One ) > 0 ? MROutput.ProvablyCompositeWithFactor( factor2 ) : MROutput.ProvablyCompositeNotPrimePower();
                }
            }
        }
        return MROutput.ProbablyPrime();
    }

    public static bool HasAnySmallFactors( BigInteger candidate )
    {
        CheckCandidate( candidate, nameof( candidate ) );
        return ImplHasAnySmallFactors( candidate );
    }

    public static bool IsMRProbablePrime( BigInteger candidate, SecureRandom random, int iterations )
    {
        CheckCandidate( candidate, nameof( candidate ) );
        if (random == null)
            throw new ArgumentException( "cannot be null", nameof( random ) );
        if (iterations < 1)
            throw new ArgumentException( "must be > 0", nameof( iterations ) );
        if (candidate.BitLength == 2)
            return true;
        if (!candidate.TestBit( 0 ))
            return false;
        BigInteger w = candidate;
        BigInteger wSubOne = candidate.Subtract( One );
        BigInteger max = candidate.Subtract( Two );
        int lowestSetBit = wSubOne.GetLowestSetBit();
        BigInteger m = wSubOne.ShiftRight( lowestSetBit );
        for (int index = 0; index < iterations; ++index)
        {
            BigInteger randomInRange = BigIntegers.CreateRandomInRange( Two, max, random );
            if (!ImplMRProbablePrimeToBase( w, wSubOne, m, lowestSetBit, randomInRange ))
                return false;
        }
        return true;
    }

    public static bool IsMRProbablePrimeToBase( BigInteger candidate, BigInteger baseValue )
    {
        CheckCandidate( candidate, nameof( candidate ) );
        CheckCandidate( baseValue, nameof( baseValue ) );
        if (baseValue.CompareTo( candidate.Subtract( One ) ) >= 0)
            throw new ArgumentException( "must be < ('candidate' - 1)", nameof( baseValue ) );
        if (candidate.BitLength == 2)
            return true;
        BigInteger w = candidate;
        BigInteger wSubOne = candidate.Subtract( One );
        int lowestSetBit = wSubOne.GetLowestSetBit();
        BigInteger m = wSubOne.ShiftRight( lowestSetBit );
        return ImplMRProbablePrimeToBase( w, wSubOne, m, lowestSetBit, baseValue );
    }

    private static void CheckCandidate( BigInteger n, string name )
    {
        if (n == null || n.SignValue < 1 || n.BitLength < 2)
            throw new ArgumentException( "must be non-null and >= 2", name );
    }

    private static bool ImplHasAnySmallFactors( BigInteger x )
    {
        int num1 = 223092870;
        int intValue1 = x.Mod( BigInteger.ValueOf( num1 ) ).IntValue;
        if (intValue1 % 2 == 0 || intValue1 % 3 == 0 || intValue1 % 5 == 0 || intValue1 % 7 == 0 || intValue1 % 11 == 0 || intValue1 % 13 == 0 || intValue1 % 17 == 0 || intValue1 % 19 == 0 || intValue1 % 23 == 0)
            return true;
        int num2 = 58642669;
        int intValue2 = x.Mod( BigInteger.ValueOf( num2 ) ).IntValue;
        if (intValue2 % 29 == 0 || intValue2 % 31 == 0 || intValue2 % 37 == 0 || intValue2 % 41 == 0 || intValue2 % 43 == 0)
            return true;
        int num3 = 600662303;
        int intValue3 = x.Mod( BigInteger.ValueOf( num3 ) ).IntValue;
        if (intValue3 % 47 == 0 || intValue3 % 53 == 0 || intValue3 % 59 == 0 || intValue3 % 61 == 0 || intValue3 % 67 == 0)
            return true;
        int num4 = 33984931;
        int intValue4 = x.Mod( BigInteger.ValueOf( num4 ) ).IntValue;
        if (intValue4 % 71 == 0 || intValue4 % 73 == 0 || intValue4 % 79 == 0 || intValue4 % 83 == 0)
            return true;
        int num5 = 89809099;
        int intValue5 = x.Mod( BigInteger.ValueOf( num5 ) ).IntValue;
        if (intValue5 % 89 == 0 || intValue5 % 97 == 0 || intValue5 % 101 == 0 || intValue5 % 103 == 0)
            return true;
        int num6 = 167375713;
        int intValue6 = x.Mod( BigInteger.ValueOf( num6 ) ).IntValue;
        if (intValue6 % 107 == 0 || intValue6 % 109 == 0 || intValue6 % 113 == 0 || intValue6 % sbyte.MaxValue == 0)
            return true;
        int num7 = 371700317;
        int intValue7 = x.Mod( BigInteger.ValueOf( num7 ) ).IntValue;
        if (intValue7 % 131 == 0 || intValue7 % 137 == 0 || intValue7 % 139 == 0 || intValue7 % 149 == 0)
            return true;
        int num8 = 645328247;
        int intValue8 = x.Mod( BigInteger.ValueOf( num8 ) ).IntValue;
        if (intValue8 % 151 == 0 || intValue8 % 157 == 0 || intValue8 % 163 == 0 || intValue8 % 167 == 0)
            return true;
        int num9 = 1070560157;
        int intValue9 = x.Mod( BigInteger.ValueOf( num9 ) ).IntValue;
        if (intValue9 % 173 == 0 || intValue9 % 179 == 0 || intValue9 % 181 == 0 || intValue9 % 191 == 0)
            return true;
        int num10 = 1596463769;
        int intValue10 = x.Mod( BigInteger.ValueOf( num10 ) ).IntValue;
        return intValue10 % 193 == 0 || intValue10 % 197 == 0 || intValue10 % 199 == 0 || intValue10 % 211 == 0;
    }

    private static bool ImplMRProbablePrimeToBase(
      BigInteger w,
      BigInteger wSubOne,
      BigInteger m,
      int a,
      BigInteger b )
    {
        BigInteger bigInteger = b.ModPow( m, w );
        if (bigInteger.Equals( One ) || bigInteger.Equals( wSubOne ))
            return true;
        bool flag = false;
        for (int index = 1; index < a; ++index)
        {
            bigInteger = bigInteger.ModPow( Two, w );
            if (bigInteger.Equals( wSubOne ))
            {
                flag = true;
                break;
            }
            if (bigInteger.Equals( One ))
                return false;
        }
        return flag;
    }

    private static STOutput ImplSTRandomPrime( IDigest d, int length, byte[] primeSeed )
    {
        int digestSize = d.GetDigestSize();
        if (length < 33)
        {
            int primeGenCounter = 0;
            byte[] numArray1 = new byte[digestSize];
            byte[] numArray2 = new byte[digestSize];
            do
            {
                Hash( d, primeSeed, numArray1, 0 );
                Inc( primeSeed, 1 );
                Hash( d, primeSeed, numArray2, 0 );
                Inc( primeSeed, 1 );
                uint x = ((Extract32( numArray1 ) ^ Extract32( numArray2 )) & (uint.MaxValue >> (32 - length))) | (uint)((1 << (length - 1)) | 1);
                ++primeGenCounter;
                if (IsPrime32( x ))
                    return new STOutput( BigInteger.ValueOf( x ), primeSeed, primeGenCounter );
            }
            while (primeGenCounter <= 4 * length);
            throw new InvalidOperationException( "Too many iterations in Shawe-Taylor Random_Prime Routine" );
        }
        STOutput stOutput = ImplSTRandomPrime( d, (length + 3) / 2, primeSeed );
        BigInteger prime = stOutput.Prime;
        primeSeed = stOutput.PrimeSeed;
        int primeGenCounter1 = stOutput.PrimeGenCounter;
        int num1 = 8 * digestSize;
        int num2 = (length - 1) / num1;
        int num3 = primeGenCounter1;
        BigInteger bigInteger1 = HashGen( d, primeSeed, num2 + 1 ).Mod( One.ShiftLeft( length - 1 ) ).SetBit( length - 1 );
        BigInteger val = prime.ShiftLeft( 1 );
        BigInteger e = bigInteger1.Subtract( One ).Divide( val ).Add( One ).ShiftLeft( 1 );
        int num4 = 0;
        BigInteger bigInteger2 = e.Multiply( prime ).Add( One );
        while (true)
        {
            if (bigInteger2.BitLength > length)
            {
                e = One.ShiftLeft( length - 1 ).Subtract( One ).Divide( val ).Add( One ).ShiftLeft( 1 );
                bigInteger2 = e.Multiply( prime ).Add( One );
            }
            ++primeGenCounter1;
            if (!ImplHasAnySmallFactors( bigInteger2 ))
            {
                BigInteger bigInteger3 = HashGen( d, primeSeed, num2 + 1 ).Mod( bigInteger2.Subtract( Three ) ).Add( Two );
                e = e.Add( BigInteger.ValueOf( num4 ) );
                num4 = 0;
                BigInteger bigInteger4 = bigInteger3.ModPow( e, bigInteger2 );
                if (bigInteger2.Gcd( bigInteger4.Subtract( One ) ).Equals( One ) && bigInteger4.ModPow( prime, bigInteger2 ).Equals( One ))
                    break;
            }
            else
                Inc( primeSeed, num2 + 1 );
            if (primeGenCounter1 < (4 * length) + num3)
            {
                num4 += 2;
                bigInteger2 = bigInteger2.Add( val );
            }
            else
                goto label_14;
        }
        return new STOutput( bigInteger2, primeSeed, primeGenCounter1 );
    label_14:
        throw new InvalidOperationException( "Too many iterations in Shawe-Taylor Random_Prime Routine" );
    }

    private static uint Extract32( byte[] bs )
    {
        uint num1 = 0;
        int num2 = System.Math.Min( 4, bs.Length );
        for (int index = 0; index < num2; ++index)
        {
            uint b = bs[bs.Length - (index + 1)];
            num1 |= b << (8 * index);
        }
        return num1;
    }

    private static void Hash( IDigest d, byte[] input, byte[] output, int outPos )
    {
        d.BlockUpdate( input, 0, input.Length );
        d.DoFinal( output, outPos );
    }

    private static BigInteger HashGen( IDigest d, byte[] seed, int count )
    {
        int digestSize = d.GetDigestSize();
        int outPos = count * digestSize;
        byte[] numArray = new byte[outPos];
        for (int index = 0; index < count; ++index)
        {
            outPos -= digestSize;
            Hash( d, seed, numArray, outPos );
            Inc( seed, 1 );
        }
        return new BigInteger( 1, numArray );
    }

    private static void Inc( byte[] seed, int c )
    {
        for (int length = seed.Length; c > 0 && --length >= 0; c >>= 8)
        {
            c += seed[length];
            seed[length] = (byte)c;
        }
    }

    private static bool IsPrime32( uint x )
    {
        switch (x)
        {
            case 0:
            case 1:
            case 4:
            case 5:
                return x == 5U;
            case 2:
            case 3:
                return true;
            default:
                if (((int)x & 1) == 0 || x % 3U == 0U || x % 5U == 0U)
                    return false;
                uint[] numArray = new uint[8]
                {
            1U,
            7U,
            11U,
            13U,
            17U,
            19U,
            23U,
            29U
                };
                uint num1 = 0;
                int index = 1;
                while (true)
                {
                    for (; index < numArray.Length; ++index)
                    {
                        uint num2 = num1 + numArray[index];
                        if (x % num2 == 0U)
                            return x < 30U;
                    }
                    num1 += 30U;
                    if (num1 >> 16 == 0U && num1 * num1 < x)
                        index = 0;
                    else
                        break;
                }
                return true;
        }
    }

    public class MROutput
    {
        private readonly bool mProvablyComposite;
        private readonly BigInteger mFactor;

        internal static MROutput ProbablyPrime() => new MROutput( false, null );

        internal static MROutput ProvablyCompositeWithFactor( BigInteger factor ) => new MROutput( true, factor );

        internal static MROutput ProvablyCompositeNotPrimePower() => new MROutput( true, null );

        private MROutput( bool provablyComposite, BigInteger factor )
        {
            mProvablyComposite = provablyComposite;
            mFactor = factor;
        }

        public BigInteger Factor => mFactor;

        public bool IsProvablyComposite => mProvablyComposite;

        public bool IsNotPrimePower => mProvablyComposite && mFactor == null;
    }

    public class STOutput
    {
        private readonly BigInteger mPrime;
        private readonly byte[] mPrimeSeed;
        private readonly int mPrimeGenCounter;

        internal STOutput( BigInteger prime, byte[] primeSeed, int primeGenCounter )
        {
            mPrime = prime;
            mPrimeSeed = primeSeed;
            mPrimeGenCounter = primeGenCounter;
        }

        public BigInteger Prime => mPrime;

        public byte[] PrimeSeed => mPrimeSeed;

        public int PrimeGenCounter => mPrimeGenCounter;
    }
}
internal abstract class Platform
{
    private static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;
    internal static readonly string NewLine = GetNewLine();

    private static string GetNewLine() => Environment.NewLine;

    internal static bool EqualsIgnoreCase( string a, string b ) => ToUpperInvariant( a ) == ToUpperInvariant( b );

    internal static string GetEnvironmentVariable( string variable )
    {
        try
        {
            return Environment.GetEnvironmentVariable( variable );
        }
        catch (SecurityException)
        {
            return null;
        }
    }

    internal static Exception CreateNotImplementedException( string message ) => new NotImplementedException( message );

    internal static IList CreateArrayList() => new ArrayList();

    internal static IList CreateArrayList( int capacity ) => new ArrayList( capacity );

    internal static IList CreateArrayList( ICollection collection ) => new ArrayList( collection );

    internal static IList CreateArrayList( IEnumerable collection )
    {
        ArrayList arrayList = new ArrayList();
        foreach (object obj in collection)
            arrayList.Add( obj );
        return arrayList;
    }

    internal static IDictionary CreateHashtable() => new Hashtable();

    internal static IDictionary CreateHashtable( int capacity ) => new Hashtable( capacity );

    internal static IDictionary CreateHashtable( IDictionary dictionary ) => new Hashtable( dictionary );

    internal static string ToLowerInvariant( string s ) => s.ToLower( CultureInfo.InvariantCulture );

    internal static string ToUpperInvariant( string s ) => s.ToUpper( CultureInfo.InvariantCulture );

    internal static void Dispose( Stream s ) => s.Close();

    internal static void Dispose( TextWriter t ) => t.Close();

    internal static int IndexOf( string source, string value ) => InvariantCompareInfo.IndexOf( source, value, CompareOptions.Ordinal );

    internal static int LastIndexOf( string source, string value ) => InvariantCompareInfo.LastIndexOf( source, value, CompareOptions.Ordinal );

    internal static bool StartsWith( string source, string prefix ) => InvariantCompareInfo.IsPrefix( source, prefix, CompareOptions.Ordinal );

    internal static bool EndsWith( string source, string suffix ) => InvariantCompareInfo.IsSuffix( source, suffix, CompareOptions.Ordinal );

    internal static string GetTypeName( object obj ) => obj.GetType().FullName;
}
public class SecureRandom : Random
{
    private static long counter = Times.NanoTime();
    private static readonly SecureRandom master = new SecureRandom( new CryptoApiRandomGenerator() );
    protected readonly IRandomGenerator generator;
    private static readonly double DoubleScale = System.Math.Pow( 2.0, 64.0 );

    private static long NextCounterValue() => Interlocked.Increment( ref counter );

    private static SecureRandom Master => master;

    private static DigestRandomGenerator CreatePrng( string digestName, bool autoSeed )
    {
        IDigest digest = DigestUtilities.GetDigest( digestName );
        if (digest == null)
            return null;
        DigestRandomGenerator prng = new DigestRandomGenerator( digest );
        if (autoSeed)
        {
            prng.AddSeedMaterial( NextCounterValue() );
            prng.AddSeedMaterial( GetNextBytes( Master, digest.GetDigestSize() ) );
        }
        return prng;
    }

    public static byte[] GetNextBytes( SecureRandom secureRandom, int length )
    {
        byte[] buffer = new byte[length];
        secureRandom.NextBytes( buffer );
        return buffer;
    }

    public static SecureRandom GetInstance( string algorithm ) => GetInstance( algorithm, true );

    public static SecureRandom GetInstance( string algorithm, bool autoSeed )
    {
        string upperInvariant = Platform.ToUpperInvariant( algorithm );
        if (Platform.EndsWith( upperInvariant, "PRNG" ))
        {
            DigestRandomGenerator prng = CreatePrng( upperInvariant.Substring( 0, upperInvariant.Length - "PRNG".Length ), autoSeed );
            if (prng != null)
                return new SecureRandom( prng );
        }
        throw new ArgumentException( "Unrecognised PRNG algorithm: " + algorithm, nameof( algorithm ) );
    }

    [Obsolete( "Call GenerateSeed() on a SecureRandom instance instead" )]
    public static byte[] GetSeed( int length ) => GetNextBytes( Master, length );

    public SecureRandom()
      : this( CreatePrng( "SHA256", true ) )
    {
    }

    [Obsolete( "Use GetInstance/SetSeed instead" )]
    public SecureRandom( byte[] seed )
      : this( CreatePrng( "SHA1", false ) )
    {
        SetSeed( seed );
    }

    public SecureRandom( IRandomGenerator generator )
      : base( 0 )
    {
        this.generator = generator;
    }

    public virtual byte[] GenerateSeed( int length ) => GetNextBytes( Master, length );

    public virtual void SetSeed( byte[] seed ) => generator.AddSeedMaterial( seed );

    public virtual void SetSeed( long seed ) => generator.AddSeedMaterial( seed );

    public override int Next() => NextInt() & int.MaxValue;

    public override int Next( int maxValue )
    {
        if (maxValue < 2)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException( nameof( maxValue ), "cannot be negative" );
            return 0;
        }
        if ((maxValue & (maxValue - 1)) == 0)
            return NextInt() & (maxValue - 1);
        int num1;
        int num2;
        do
        {
            num1 = NextInt() & int.MaxValue;
            num2 = num1 % maxValue;
        }
        while (num1 - num2 + (maxValue - 1) < 0);
        return num2;
    }

    public override int Next( int minValue, int maxValue )
    {
        if (maxValue <= minValue)
            return maxValue == minValue ? minValue : throw new ArgumentException( "maxValue cannot be less than minValue" );
        int maxValue1 = maxValue - minValue;
        if (maxValue1 > 0)
            return minValue + Next( maxValue1 );
        int num;
        do
        {
            num = NextInt();
        }
        while (num < minValue || num >= maxValue);
        return num;
    }

    public override void NextBytes( byte[] buf ) => generator.NextBytes( buf );

    public virtual void NextBytes( byte[] buf, int off, int len ) => generator.NextBytes( buf, off, len );

    public override double NextDouble() => Convert.ToDouble( (ulong)NextLong() ) / DoubleScale;

    public virtual int NextInt()
    {
        byte[] buffer = new byte[4];
        NextBytes( buffer );
        return (int)(((((((uint)buffer[0] << 8) | buffer[1]) << 8) | buffer[2]) << 8) | buffer[3]);
    }

    public virtual long NextLong() => ((long)(uint)NextInt() << 32) | (uint)NextInt();
}
public interface IRandomGenerator
{
    void AddSeedMaterial( byte[] seed );

    void AddSeedMaterial( long seed );

    void NextBytes( byte[] bytes );

    void NextBytes( byte[] bytes, int start, int len );
}
public class DigestRandomGenerator : IRandomGenerator
{
    private const long CYCLE_COUNT = 10;
    private long stateCounter;
    private long seedCounter;
    private IDigest digest;
    private byte[] state;
    private byte[] seed;

    public DigestRandomGenerator( IDigest digest )
    {
        this.digest = digest;
        seed = new byte[digest.GetDigestSize()];
        seedCounter = 1L;
        state = new byte[digest.GetDigestSize()];
        stateCounter = 1L;
    }

    public void AddSeedMaterial( byte[] inSeed )
    {
        lock (this)
        {
            DigestUpdate( inSeed );
            DigestUpdate( seed );
            DigestDoFinal( seed );
        }
    }

    public void AddSeedMaterial( long rSeed )
    {
        lock (this)
        {
            DigestAddCounter( rSeed );
            DigestUpdate( seed );
            DigestDoFinal( seed );
        }
    }

    public void NextBytes( byte[] bytes ) => NextBytes( bytes, 0, bytes.Length );

    public void NextBytes( byte[] bytes, int start, int len )
    {
        lock (this)
        {
            int num1 = 0;
            GenerateState();
            int num2 = start + len;
            for (int index = start; index < num2; ++index)
            {
                if (num1 == state.Length)
                {
                    GenerateState();
                    num1 = 0;
                }
                bytes[index] = state[num1++];
            }
        }
    }

    private void CycleSeed()
    {
        DigestUpdate( seed );
        DigestAddCounter( seedCounter++ );
        DigestDoFinal( seed );
    }

    private void GenerateState()
    {
        DigestAddCounter( stateCounter++ );
        DigestUpdate( state );
        DigestUpdate( seed );
        DigestDoFinal( state );
        if (stateCounter % 10L != 0L)
            return;
        CycleSeed();
    }

    private void DigestAddCounter( long seedVal )
    {
        byte[] numArray = new byte[8];
        Pack.UInt64_To_LE( (ulong)seedVal, numArray );
        digest.BlockUpdate( numArray, 0, numArray.Length );
    }

    private void DigestUpdate( byte[] inSeed ) => digest.BlockUpdate( inSeed, 0, inSeed.Length );

    private void DigestDoFinal( byte[] result ) => digest.DoFinal( result, 0 );
}
internal sealed class Pack
{
    private Pack()
    {
    }

    internal static void UInt16_To_BE( ushort n, byte[] bs )
    {
        bs[0] = (byte)((uint)n >> 8);
        bs[1] = (byte)n;
    }

    internal static void UInt16_To_BE( ushort n, byte[] bs, int off )
    {
        bs[off] = (byte)((uint)n >> 8);
        bs[off + 1] = (byte)n;
    }

    internal static ushort BE_To_UInt16( byte[] bs ) => (ushort)(((uint)bs[0] << 8) | bs[1]);

    internal static ushort BE_To_UInt16( byte[] bs, int off ) => (ushort)(((uint)bs[off] << 8) | bs[off + 1]);

    internal static byte[] UInt32_To_BE( uint n )
    {
        byte[] bs = new byte[4];
        UInt32_To_BE( n, bs, 0 );
        return bs;
    }

    internal static void UInt32_To_BE( uint n, byte[] bs )
    {
        bs[0] = (byte)(n >> 24);
        bs[1] = (byte)(n >> 16);
        bs[2] = (byte)(n >> 8);
        bs[3] = (byte)n;
    }

    internal static void UInt32_To_BE( uint n, byte[] bs, int off )
    {
        bs[off] = (byte)(n >> 24);
        bs[off + 1] = (byte)(n >> 16);
        bs[off + 2] = (byte)(n >> 8);
        bs[off + 3] = (byte)n;
    }

    internal static byte[] UInt32_To_BE( uint[] ns )
    {
        byte[] bs = new byte[4 * ns.Length];
        UInt32_To_BE( ns, bs, 0 );
        return bs;
    }

    internal static void UInt32_To_BE( uint[] ns, byte[] bs, int off )
    {
        for (int index = 0; index < ns.Length; ++index)
        {
            UInt32_To_BE( ns[index], bs, off );
            off += 4;
        }
    }

    internal static uint BE_To_UInt32( byte[] bs ) => (uint)((bs[0] << 24) | (bs[1] << 16) | (bs[2] << 8)) | bs[3];

    internal static uint BE_To_UInt32( byte[] bs, int off ) => (uint)((bs[off] << 24) | (bs[off + 1] << 16) | (bs[off + 2] << 8)) | bs[off + 3];

    internal static void BE_To_UInt32( byte[] bs, int off, uint[] ns )
    {
        for (int index = 0; index < ns.Length; ++index)
        {
            ns[index] = BE_To_UInt32( bs, off );
            off += 4;
        }
    }

    internal static byte[] UInt64_To_BE( ulong n )
    {
        byte[] bs = new byte[8];
        UInt64_To_BE( n, bs, 0 );
        return bs;
    }

    internal static void UInt64_To_BE( ulong n, byte[] bs )
    {
        UInt32_To_BE( (uint)(n >> 32), bs );
        UInt32_To_BE( (uint)n, bs, 4 );
    }

    internal static void UInt64_To_BE( ulong n, byte[] bs, int off )
    {
        UInt32_To_BE( (uint)(n >> 32), bs, off );
        UInt32_To_BE( (uint)n, bs, off + 4 );
    }

    internal static byte[] UInt64_To_BE( ulong[] ns )
    {
        byte[] bs = new byte[8 * ns.Length];
        UInt64_To_BE( ns, bs, 0 );
        return bs;
    }

    internal static void UInt64_To_BE( ulong[] ns, byte[] bs, int off )
    {
        for (int index = 0; index < ns.Length; ++index)
        {
            UInt64_To_BE( ns[index], bs, off );
            off += 8;
        }
    }

    internal static ulong BE_To_UInt64( byte[] bs ) => ((ulong)BE_To_UInt32( bs ) << 32) | BE_To_UInt32( bs, 4 );

    internal static ulong BE_To_UInt64( byte[] bs, int off ) => ((ulong)BE_To_UInt32( bs, off ) << 32) | BE_To_UInt32( bs, off + 4 );

    internal static void BE_To_UInt64( byte[] bs, int off, ulong[] ns )
    {
        for (int index = 0; index < ns.Length; ++index)
        {
            ns[index] = BE_To_UInt64( bs, off );
            off += 8;
        }
    }

    internal static void UInt16_To_LE( ushort n, byte[] bs )
    {
        bs[0] = (byte)n;
        bs[1] = (byte)((uint)n >> 8);
    }

    internal static void UInt16_To_LE( ushort n, byte[] bs, int off )
    {
        bs[off] = (byte)n;
        bs[off + 1] = (byte)((uint)n >> 8);
    }

    internal static ushort LE_To_UInt16( byte[] bs ) => (ushort)(bs[0] | ((uint)bs[1] << 8));

    internal static ushort LE_To_UInt16( byte[] bs, int off ) => (ushort)(bs[off] | ((uint)bs[off + 1] << 8));

    internal static byte[] UInt32_To_LE( uint n )
    {
        byte[] bs = new byte[4];
        UInt32_To_LE( n, bs, 0 );
        return bs;
    }

    internal static void UInt32_To_LE( uint n, byte[] bs )
    {
        bs[0] = (byte)n;
        bs[1] = (byte)(n >> 8);
        bs[2] = (byte)(n >> 16);
        bs[3] = (byte)(n >> 24);
    }

    internal static void UInt32_To_LE( uint n, byte[] bs, int off )
    {
        bs[off] = (byte)n;
        bs[off + 1] = (byte)(n >> 8);
        bs[off + 2] = (byte)(n >> 16);
        bs[off + 3] = (byte)(n >> 24);
    }

    internal static byte[] UInt32_To_LE( uint[] ns )
    {
        byte[] bs = new byte[4 * ns.Length];
        UInt32_To_LE( ns, bs, 0 );
        return bs;
    }

    internal static void UInt32_To_LE( uint[] ns, byte[] bs, int off )
    {
        for (int index = 0; index < ns.Length; ++index)
        {
            UInt32_To_LE( ns[index], bs, off );
            off += 4;
        }
    }

    internal static uint LE_To_UInt32( byte[] bs ) => (uint)(bs[0] | (bs[1] << 8) | (bs[2] << 16) | (bs[3] << 24));

    internal static uint LE_To_UInt32( byte[] bs, int off ) => (uint)(bs[off] | (bs[off + 1] << 8) | (bs[off + 2] << 16) | (bs[off + 3] << 24));

    internal static void LE_To_UInt32( byte[] bs, int off, uint[] ns )
    {
        for (int index = 0; index < ns.Length; ++index)
        {
            ns[index] = LE_To_UInt32( bs, off );
            off += 4;
        }
    }

    internal static void LE_To_UInt32( byte[] bs, int bOff, uint[] ns, int nOff, int count )
    {
        for (int index = 0; index < count; ++index)
        {
            ns[nOff + index] = LE_To_UInt32( bs, bOff );
            bOff += 4;
        }
    }

    internal static byte[] UInt64_To_LE( ulong n )
    {
        byte[] bs = new byte[8];
        UInt64_To_LE( n, bs, 0 );
        return bs;
    }

    internal static void UInt64_To_LE( ulong n, byte[] bs )
    {
        UInt32_To_LE( (uint)n, bs );
        UInt32_To_LE( (uint)(n >> 32), bs, 4 );
    }

    internal static void UInt64_To_LE( ulong n, byte[] bs, int off )
    {
        UInt32_To_LE( (uint)n, bs, off );
        UInt32_To_LE( (uint)(n >> 32), bs, off + 4 );
    }

    internal static ulong LE_To_UInt64( byte[] bs )
    {
        uint uint32 = LE_To_UInt32( bs );
        return ((ulong)LE_To_UInt32( bs, 4 ) << 32) | uint32;
    }

    internal static ulong LE_To_UInt64( byte[] bs, int off )
    {
        uint uint32 = LE_To_UInt32( bs, off );
        return ((ulong)LE_To_UInt32( bs, off + 4 ) << 32) | uint32;
    }
}
public sealed class DigestUtilities
{
    private static readonly IDictionary algorithms = Platform.CreateHashtable();
    private static readonly IDictionary oids = Platform.CreateHashtable();

    private DigestUtilities()
    {
    }

    static DigestUtilities()
    {
        ((DigestAlgorithm)Enums.GetArbitraryValue( typeof( DigestAlgorithm ) )).ToString();
        algorithms[PkcsObjectIdentifiers.MD2.Id] = "MD2";
        algorithms[PkcsObjectIdentifiers.MD4.Id] = "MD4";
        algorithms[PkcsObjectIdentifiers.MD5.Id] = "MD5";
        algorithms["SHA1"] = "SHA-1";
        algorithms[OiwObjectIdentifiers.IdSha1.Id] = "SHA-1";
        algorithms["SHA224"] = "SHA-224";
        algorithms[NistObjectIdentifiers.IdSha224.Id] = "SHA-224";
        algorithms["SHA256"] = "SHA-256";
        algorithms[NistObjectIdentifiers.IdSha256.Id] = "SHA-256";
        algorithms["SHA384"] = "SHA-384";
        algorithms[NistObjectIdentifiers.IdSha384.Id] = "SHA-384";
        algorithms["SHA512"] = "SHA-512";
        algorithms[NistObjectIdentifiers.IdSha512.Id] = "SHA-512";
        algorithms["SHA512/224"] = "SHA-512/224";
        algorithms[NistObjectIdentifiers.IdSha512_224.Id] = "SHA-512/224";
        algorithms["SHA512/256"] = "SHA-512/256";
        algorithms[NistObjectIdentifiers.IdSha512_256.Id] = "SHA-512/256";
        algorithms["RIPEMD-128"] = "RIPEMD128";
        algorithms[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "RIPEMD128";
        algorithms["RIPEMD-160"] = "RIPEMD160";
        algorithms[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "RIPEMD160";
        algorithms["RIPEMD-256"] = "RIPEMD256";
        algorithms[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "RIPEMD256";
        algorithms["RIPEMD-320"] = "RIPEMD320";
        algorithms[CryptoProObjectIdentifiers.GostR3411.Id] = "GOST3411";
        algorithms[NistObjectIdentifiers.IdSha3_224.Id] = "SHA3-224";
        algorithms[NistObjectIdentifiers.IdSha3_256.Id] = "SHA3-256";
        algorithms[NistObjectIdentifiers.IdSha3_384.Id] = "SHA3-384";
        algorithms[NistObjectIdentifiers.IdSha3_512.Id] = "SHA3-512";
        algorithms[NistObjectIdentifiers.IdShake128.Id] = "SHAKE128";
        algorithms[NistObjectIdentifiers.IdShake256.Id] = "SHAKE256";
        oids["MD2"] = PkcsObjectIdentifiers.MD2;
        oids["MD4"] = PkcsObjectIdentifiers.MD4;
        oids["MD5"] = PkcsObjectIdentifiers.MD5;
        oids["SHA-1"] = OiwObjectIdentifiers.IdSha1;
        oids["SHA-224"] = NistObjectIdentifiers.IdSha224;
        oids["SHA-256"] = NistObjectIdentifiers.IdSha256;
        oids["SHA-384"] = NistObjectIdentifiers.IdSha384;
        oids["SHA-512"] = NistObjectIdentifiers.IdSha512;
        oids["SHA-512/224"] = NistObjectIdentifiers.IdSha512_224;
        oids["SHA-512/256"] = NistObjectIdentifiers.IdSha512_256;
        oids["SHA3-224"] = NistObjectIdentifiers.IdSha3_224;
        oids["SHA3-256"] = NistObjectIdentifiers.IdSha3_256;
        oids["SHA3-384"] = NistObjectIdentifiers.IdSha3_384;
        oids["SHA3-512"] = NistObjectIdentifiers.IdSha3_512;
        oids["SHAKE128"] = NistObjectIdentifiers.IdShake128;
        oids["SHAKE256"] = NistObjectIdentifiers.IdShake256;
        oids["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
        oids["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
        oids["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
        oids["GOST3411"] = CryptoProObjectIdentifiers.GostR3411;
    }

    public static DerObjectIdentifier GetObjectIdentifier( string mechanism )
    {
        mechanism = mechanism != null ? Platform.ToUpperInvariant( mechanism ) : throw new ArgumentNullException( nameof( mechanism ) );
        string algorithm = (string)algorithms[mechanism];
        if (algorithm != null)
            mechanism = algorithm;
        return (DerObjectIdentifier)oids[mechanism];
    }

    public static ICollection Algorithms => oids.Keys;

    public static IDigest GetDigest( DerObjectIdentifier id ) => GetDigest( id.Id );

    public static IDigest GetDigest( string algorithm )
    {
        string upperInvariant = Platform.ToUpperInvariant( algorithm );
        string s = (string)algorithms[upperInvariant] ?? upperInvariant;
        try
        {
            switch ((DigestAlgorithm)Enums.GetEnumValue( typeof( DigestAlgorithm ), s ))
            {
                case DigestAlgorithm.GOST3411:
                    return new Gost3411Digest();
                case DigestAlgorithm.KECCAK_224:
                    return new KeccakDigest( 224 );
                case DigestAlgorithm.KECCAK_256:
                    return new KeccakDigest( 256 );
                case DigestAlgorithm.KECCAK_288:
                    return new KeccakDigest( 288 );
                case DigestAlgorithm.KECCAK_384:
                    return new KeccakDigest( 384 );
                case DigestAlgorithm.KECCAK_512:
                    return new KeccakDigest( 512 );
                case DigestAlgorithm.MD2:
                    return new MD2Digest();
                case DigestAlgorithm.MD4:
                    return new MD4Digest();
                case DigestAlgorithm.MD5:
                    return new MD5Digest();
                case DigestAlgorithm.RIPEMD128:
                    return new RipeMD128Digest();
                case DigestAlgorithm.RIPEMD160:
                    return new RipeMD160Digest();
                case DigestAlgorithm.RIPEMD256:
                    return new RipeMD256Digest();
                case DigestAlgorithm.RIPEMD320:
                    return new RipeMD320Digest();
                case DigestAlgorithm.SHA_1:
                    return new Sha1Digest();
                case DigestAlgorithm.SHA_224:
                    return new Sha224Digest();
                case DigestAlgorithm.SHA_256:
                    return new Sha256Digest();
                case DigestAlgorithm.SHA_384:
                    return new Sha384Digest();
                case DigestAlgorithm.SHA_512:
                    return new Sha512Digest();
                case DigestAlgorithm.SHA_512_224:
                    return new Sha512tDigest( 224 );
                case DigestAlgorithm.SHA_512_256:
                    return new Sha512tDigest( 256 );
                case DigestAlgorithm.SHA3_224:
                    return new Sha3Digest( 224 );
                case DigestAlgorithm.SHA3_256:
                    return new Sha3Digest( 256 );
                case DigestAlgorithm.SHA3_384:
                    return new Sha3Digest( 384 );
                case DigestAlgorithm.SHA3_512:
                    return new Sha3Digest( 512 );
                case DigestAlgorithm.SHAKE128:
                    return new ShakeDigest( 128 );
                case DigestAlgorithm.SHAKE256:
                    return new ShakeDigest( 256 );
                case DigestAlgorithm.TIGER:
                    return new TigerDigest();
                case DigestAlgorithm.WHIRLPOOL:
                    return new WhirlpoolDigest();
            }
        }
        catch (ArgumentException)
        {
        }
        throw new SecurityUtilityException( "Digest " + s + " not recognised." );
    }

    public static string GetAlgorithmName( DerObjectIdentifier oid ) => (string)algorithms[oid.Id];

    public static byte[] CalculateDigest( string algorithm, byte[] input )
    {
        IDigest digest = GetDigest( algorithm );
        digest.BlockUpdate( input, 0, input.Length );
        return DoFinal( digest );
    }

    public static byte[] DoFinal( IDigest digest )
    {
        byte[] output = new byte[digest.GetDigestSize()];
        digest.DoFinal( output, 0 );
        return output;
    }

    public static byte[] DoFinal( IDigest digest, byte[] input )
    {
        digest.BlockUpdate( input, 0, input.Length );
        return DoFinal( digest );
    }

    private enum DigestAlgorithm
    {
        GOST3411,
        KECCAK_224,
        KECCAK_256,
        KECCAK_288,
        KECCAK_384,
        KECCAK_512,
        MD2,
        MD4,
        MD5,
        RIPEMD128,
        RIPEMD160,
        RIPEMD256,
        RIPEMD320,
        SHA_1,
        SHA_224,
        SHA_256,
        SHA_384,
        SHA_512,
        SHA_512_224,
        SHA_512_256,
        SHA3_224,
        SHA3_256,
        SHA3_384,
        SHA3_512,
        SHAKE128,
        SHAKE256,
        TIGER,
        WHIRLPOOL,
    }
}
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
        sBox = Gost28147Engine.GetSBox( "D-A" );
        cipher.Init( true, new ParametersWithSBox( null, sBox ) );
        Reset();
    }

    public Gost3411Digest( byte[] sBoxParam )
    {
        sBox = Arrays.Clone( sBoxParam );
        cipher.Init( true, new ParametersWithSBox( null, sBox ) );
        Reset();
    }

    public Gost3411Digest( Gost3411Digest t ) => Reset( t );

    public string AlgorithmName => "Gost3411";

    public int GetDigestSize() => 32;

    public void Update( byte input )
    {
        xBuf[xBufOff++] = input;
        if (xBufOff == xBuf.Length)
        {
            sumByteArray( xBuf );
            processBlock( xBuf, 0 );
            xBufOff = 0;
        }
        ++byteCount;
    }

    public void BlockUpdate( byte[] input, int inOff, int length )
    {
        for (; xBufOff != 0 && length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
        while (length > xBuf.Length)
        {
            Array.Copy( input, inOff, xBuf, 0, xBuf.Length );
            sumByteArray( xBuf );
            processBlock( xBuf, 0 );
            inOff += xBuf.Length;
            length -= xBuf.Length;
            byteCount += (uint)xBuf.Length;
        }
        for (; length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
    }

    private byte[] P( byte[] input )
    {
        int num1 = 0;
        for (int index1 = 0; index1 < 8; ++index1)
        {
            byte[] k1 = K;
            int index2 = num1;
            int num2 = index2 + 1;
            int num3 = input[index1];
            k1[index2] = (byte)num3;
            byte[] k2 = K;
            int index3 = num2;
            int num4 = index3 + 1;
            int num5 = input[8 + index1];
            k2[index3] = (byte)num5;
            byte[] k3 = K;
            int index4 = num4;
            int num6 = index4 + 1;
            int num7 = input[16 + index1];
            k3[index4] = (byte)num7;
            byte[] k4 = K;
            int index5 = num6;
            num1 = index5 + 1;
            int num8 = input[24 + index1];
            k4[index5] = (byte)num8;
        }
        return K;
    }

    private byte[] A( byte[] input )
    {
        for (int index = 0; index < 8; ++index)
            a[index] = (byte)(input[index] ^ (uint)input[index + 8]);
        Array.Copy( input, 8, input, 0, 24 );
        Array.Copy( a, 0, input, 24, 8 );
        return input;
    }

    private void E( byte[] key, byte[] s, int sOff, byte[] input, int inOff )
    {
        cipher.Init( true, new KeyParameter( key ) );
        cipher.ProcessBlock( input, inOff, s, sOff );
    }

    private void fw( byte[] input )
    {
        cpyBytesToShort( input, wS );
        w_S[15] = (short)(wS[0] ^ wS[1] ^ wS[2] ^ wS[3] ^ wS[12] ^ wS[15]);
        Array.Copy( wS, 1, w_S, 0, 15 );
        cpyShortToBytes( w_S, input );
    }

    private void processBlock( byte[] input, int inOff )
    {
        Array.Copy( input, inOff, M, 0, 32 );
        H.CopyTo( U, 0 );
        M.CopyTo( V, 0 );
        for (int index = 0; index < 32; ++index)
            W[index] = (byte)(U[index] ^ (uint)V[index]);
        E( P( W ), S, 0, H, 0 );
        for (int index1 = 1; index1 < 4; ++index1)
        {
            byte[] numArray = A( U );
            for (int index2 = 0; index2 < 32; ++index2)
                U[index2] = (byte)(numArray[index2] ^ (uint)C[index1][index2]);
            V = A( A( V ) );
            for (int index3 = 0; index3 < 32; ++index3)
                W[index3] = (byte)(U[index3] ^ (uint)V[index3]);
            E( P( W ), S, index1 * 8, H, index1 * 8 );
        }
        for (int index = 0; index < 12; ++index)
            fw( S );
        for (int index = 0; index < 32; ++index)
            S[index] = (byte)(S[index] ^ (uint)M[index]);
        fw( S );
        for (int index = 0; index < 32; ++index)
            S[index] = (byte)(H[index] ^ (uint)S[index]);
        for (int index = 0; index < 61; ++index)
            fw( S );
        Array.Copy( S, 0, H, 0, H.Length );
    }

    private void finish()
    {
        Pack.UInt64_To_LE( byteCount * 8UL, L );
        while (xBufOff != 0)
            Update( 0 );
        processBlock( L, 0 );
        processBlock( Sum, 0 );
    }

    public int DoFinal( byte[] output, int outOff )
    {
        finish();
        H.CopyTo( output, outOff );
        Reset();
        return 32;
    }

    public void Reset()
    {
        byteCount = 0UL;
        xBufOff = 0;
        Array.Clear( H, 0, H.Length );
        Array.Clear( L, 0, L.Length );
        Array.Clear( M, 0, M.Length );
        Array.Clear( C[1], 0, C[1].Length );
        Array.Clear( C[3], 0, C[3].Length );
        Array.Clear( Sum, 0, Sum.Length );
        Array.Clear( xBuf, 0, xBuf.Length );
        C2.CopyTo( C[2], 0 );
    }

    private void sumByteArray( byte[] input )
    {
        int num1 = 0;
        for (int index = 0; index != Sum.Length; ++index)
        {
            int num2 = (Sum[index] & byte.MaxValue) + (input[index] & byte.MaxValue) + num1;
            Sum[index] = (byte)num2;
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
        sBox = gost3411Digest.sBox;
        cipher.Init( true, new ParametersWithSBox( null, sBox ) );
        Reset();
        Array.Copy( gost3411Digest.H, 0, H, 0, gost3411Digest.H.Length );
        Array.Copy( gost3411Digest.L, 0, L, 0, gost3411Digest.L.Length );
        Array.Copy( gost3411Digest.M, 0, M, 0, gost3411Digest.M.Length );
        Array.Copy( gost3411Digest.Sum, 0, Sum, 0, gost3411Digest.Sum.Length );
        Array.Copy( gost3411Digest.C[1], 0, C[1], 0, gost3411Digest.C[1].Length );
        Array.Copy( gost3411Digest.C[2], 0, C[2], 0, gost3411Digest.C[2].Length );
        Array.Copy( gost3411Digest.C[3], 0, C[3], 0, gost3411Digest.C[3].Length );
        Array.Copy( gost3411Digest.xBuf, 0, xBuf, 0, gost3411Digest.xBuf.Length );
        xBufOff = gost3411Digest.xBufOff;
        byteCount = gost3411Digest.byteCount;
    }
}
public class ParametersWithSBox : ICipherParameters
{
    private ICipherParameters parameters;
    private byte[] sBox;

    public ParametersWithSBox( ICipherParameters parameters, byte[] sBox )
    {
        this.parameters = parameters;
        this.sBox = sBox;
    }

    public byte[] GetSBox() => sBox;

    public ICipherParameters Parameters => parameters;
}
public interface ICipherParameters
{

}
public class KeccakDigest : IDigest, IMemoable
{
    private static readonly ulong[] KeccakRoundConstants = KeccakInitializeRoundConstants();
    private static readonly int[] KeccakRhoOffsets = KeccakInitializeRhoOffsets();
    protected byte[] state = new byte[200];
    protected byte[] dataQueue = new byte[192];
    protected int rate;
    protected int bitsInQueue;
    protected int fixedOutputLength;
    protected bool squeezing;
    protected int bitsAvailableForSqueezing;
    protected byte[] chunk;
    protected byte[] oneByte;
    private ulong[] C = new ulong[5];
    private ulong[] tempA = new ulong[25];
    private ulong[] chiC = new ulong[5];

    private static ulong[] KeccakInitializeRoundConstants()
    {
        ulong[] numArray = new ulong[24];
        byte num1 = 1;
        for (int index1 = 0; index1 < 24; ++index1)
        {
            numArray[index1] = 0UL;
            for (int index2 = 0; index2 < 7; ++index2)
            {
                int num2 = (1 << index2) - 1;
                if ((num1 & 1) != 0)
                    numArray[index1] ^= (ulong)(1L << num2);
                bool flag = (num1 & 128) != 0;
                num1 <<= 1;
                if (flag)
                    num1 ^= 113;
            }
        }
        return numArray;
    }

    private static int[] KeccakInitializeRhoOffsets()
    {
        int[] numArray = new int[25];
        int num1 = 0;
        numArray[0] = num1;
        int num2 = 1;
        int num3 = 0;
        for (int index = 1; index < 25; ++index)
        {
            num1 = (num1 + index) & 63;
            numArray[(num2 % 5) + (5 * (num3 % 5))] = num1;
            int num4 = num3 % 5;
            int num5 = ((2 * num2) + (3 * num3)) % 5;
            num2 = num4;
            num3 = num5;
        }
        return numArray;
    }

    private void ClearDataQueueSection( int off, int len )
    {
        for (int index = off; index != off + len; ++index)
            dataQueue[index] = 0;
    }

    public KeccakDigest()
      : this( 288 )
    {
    }

    public KeccakDigest( int bitLength ) => Init( bitLength );

    public KeccakDigest( KeccakDigest source ) => CopyIn( source );

    private void CopyIn( KeccakDigest source )
    {
        Array.Copy( source.state, 0, state, 0, source.state.Length );
        Array.Copy( source.dataQueue, 0, dataQueue, 0, source.dataQueue.Length );
        rate = source.rate;
        bitsInQueue = source.bitsInQueue;
        fixedOutputLength = source.fixedOutputLength;
        squeezing = source.squeezing;
        bitsAvailableForSqueezing = source.bitsAvailableForSqueezing;
        chunk = Arrays.Clone( source.chunk );
        oneByte = Arrays.Clone( source.oneByte );
    }

    public virtual string AlgorithmName => "Keccak-" + fixedOutputLength;

    public virtual int GetDigestSize() => fixedOutputLength / 8;

    public virtual void Update( byte input )
    {
        oneByte[0] = input;
        Absorb( oneByte, 0, 8L );
    }

    public virtual void BlockUpdate( byte[] input, int inOff, int len ) => Absorb( input, inOff, len * 8L );

    public virtual int DoFinal( byte[] output, int outOff )
    {
        Squeeze( output, outOff, fixedOutputLength );
        Reset();
        return GetDigestSize();
    }

    protected virtual int DoFinal( byte[] output, int outOff, byte partialByte, int partialBits )
    {
        if (partialBits > 0)
        {
            oneByte[0] = partialByte;
            Absorb( oneByte, 0, partialBits );
        }
        Squeeze( output, outOff, fixedOutputLength );
        Reset();
        return GetDigestSize();
    }

    public virtual void Reset() => Init( fixedOutputLength );

    public virtual int GetByteLength() => rate / 8;

    private void Init( int bitLength )
    {
        switch (bitLength)
        {
            case 128:
                InitSponge( 1344, 256 );
                break;
            case 224:
                InitSponge( 1152, 448 );
                break;
            case 256:
                InitSponge( 1088, 512 );
                break;
            case 288:
                InitSponge( 1024, 576 );
                break;
            case 384:
                InitSponge( 832, 768 );
                break;
            case 512:
                InitSponge( 576, 1024 );
                break;
            default:
                throw new ArgumentException( "must be one of 128, 224, 256, 288, 384, or 512.", nameof( bitLength ) );
        }
    }

    private void InitSponge( int rate, int capacity )
    {
        if (rate + capacity != 1600)
            throw new InvalidOperationException( "rate + capacity != 1600" );
        this.rate = rate > 0 && rate < 1600 && rate % 64 == 0 ? rate : throw new InvalidOperationException( "invalid rate value" );
        fixedOutputLength = 0;
        Arrays.Fill( state, 0 );
        Arrays.Fill( dataQueue, 0 );
        bitsInQueue = 0;
        squeezing = false;
        bitsAvailableForSqueezing = 0;
        fixedOutputLength = capacity / 2;
        chunk = new byte[rate / 8];
        oneByte = new byte[1];
    }

    private void AbsorbQueue()
    {
        KeccakAbsorb( state, dataQueue, rate / 8 );
        bitsInQueue = 0;
    }

    protected virtual void Absorb( byte[] data, int off, long databitlen )
    {
        if (bitsInQueue % 8 != 0)
            throw new InvalidOperationException( "attempt to absorb with odd length queue." );
        if (squeezing)
            throw new InvalidOperationException( "attempt to absorb while squeezing." );
        long num1 = 0;
        while (num1 < databitlen)
        {
            if (bitsInQueue == 0 && databitlen >= rate && num1 <= databitlen - rate)
            {
                long num2 = (databitlen - num1) / rate;
                for (long index = 0; index < num2; ++index)
                {
                    Array.Copy( data, (int)(off + (num1 / 8L) + (index * chunk.Length)), chunk, 0, chunk.Length );
                    KeccakAbsorb( state, chunk, chunk.Length );
                }
                num1 += num2 * rate;
            }
            else
            {
                int num3 = (int)(databitlen - num1);
                if (num3 + bitsInQueue > rate)
                    num3 = rate - bitsInQueue;
                int num4 = num3 % 8;
                int num5 = num3 - num4;
                Array.Copy( data, off + (int)(num1 / 8L), dataQueue, bitsInQueue / 8, num5 / 8 );
                bitsInQueue += num5;
                num1 += num5;
                if (bitsInQueue == rate)
                    AbsorbQueue();
                if (num4 > 0)
                {
                    int num6 = (1 << num4) - 1;
                    dataQueue[bitsInQueue / 8] = (byte)(data[off + (int)(num1 / 8L)] & (uint)num6);
                    bitsInQueue += num4;
                    num1 += num4;
                }
            }
        }
    }

    private void PadAndSwitchToSqueezingPhase()
    {
        if (bitsInQueue + 1 == rate)
        {
            byte[] dataQueue;
            IntPtr index;
            (dataQueue = this.dataQueue)[(int)(index = (IntPtr)(bitsInQueue / 8))] = (byte)(dataQueue[(int)index] | (uint)(byte)(1 << (bitsInQueue % 8)));
            AbsorbQueue();
            ClearDataQueueSection( 0, rate / 8 );
        }
        else
        {
            ClearDataQueueSection( (bitsInQueue + 7) / 8, (rate / 8) - ((bitsInQueue + 7) / 8) );
            byte[] dataQueue;
            IntPtr index;
            (dataQueue = this.dataQueue)[(int)(index = (IntPtr)(bitsInQueue / 8))] = (byte)(dataQueue[(int)index] | (uint)(byte)(1 << (bitsInQueue % 8)));
        }
        byte[] dataQueue1;
        IntPtr index1;
        (dataQueue1 = dataQueue)[(int)(index1 = (IntPtr)((rate - 1) / 8))] = (byte)(dataQueue1[(int)index1] | (uint)(byte)(1 << ((rate - 1) % 8)));
        AbsorbQueue();
        if (rate == 1024)
        {
            KeccakExtract1024bits( state, dataQueue );
            bitsAvailableForSqueezing = 1024;
        }
        else
        {
            KeccakExtract( state, dataQueue, rate / 64 );
            bitsAvailableForSqueezing = rate;
        }
        squeezing = true;
    }

    protected virtual void Squeeze( byte[] output, int offset, long outputLength )
    {
        if (!squeezing)
            PadAndSwitchToSqueezingPhase();
        if (outputLength % 8L != 0L)
            throw new InvalidOperationException( "outputLength not a multiple of 8" );
        int num;
        for (long index = 0; index < outputLength; index += num)
        {
            if (bitsAvailableForSqueezing == 0)
            {
                KeccakPermutation( state );
                if (rate == 1024)
                {
                    KeccakExtract1024bits( state, dataQueue );
                    bitsAvailableForSqueezing = 1024;
                }
                else
                {
                    KeccakExtract( state, dataQueue, rate / 64 );
                    bitsAvailableForSqueezing = rate;
                }
            }
            num = bitsAvailableForSqueezing;
            if (num > outputLength - index)
                num = (int)(outputLength - index);
            Array.Copy( dataQueue, (rate - bitsAvailableForSqueezing) / 8, output, offset + (int)(index / 8L), num / 8 );
            bitsAvailableForSqueezing -= num;
        }
    }

    private static void FromBytesToWords( ulong[] stateAsWords, byte[] state )
    {
        for (int index1 = 0; index1 < 25; ++index1)
        {
            stateAsWords[index1] = 0UL;
            int num = index1 * 8;
            for (int index2 = 0; index2 < 8; ++index2)
                stateAsWords[index1] |= (ulong)((state[num + index2] & (long)byte.MaxValue) << (8 * index2));
        }
    }

    private static void FromWordsToBytes( byte[] state, ulong[] stateAsWords )
    {
        for (int index1 = 0; index1 < 25; ++index1)
        {
            int num = index1 * 8;
            for (int index2 = 0; index2 < 8; ++index2)
                state[num + index2] = (byte)(stateAsWords[index1] >> (8 * index2));
        }
    }

    private void KeccakPermutation( byte[] state )
    {
        ulong[] numArray = new ulong[state.Length / 8];
        FromBytesToWords( numArray, state );
        KeccakPermutationOnWords( numArray );
        FromWordsToBytes( state, numArray );
    }

    private void KeccakPermutationAfterXor( byte[] state, byte[] data, int dataLengthInBytes )
    {
        for (int index1 = 0; index1 < dataLengthInBytes; ++index1)
        {
            byte[] numArray;
            IntPtr index2;
            (numArray = state)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray[(int)index2] ^ (uint)data[index1]);
        }
        KeccakPermutation( state );
    }

    private void KeccakPermutationOnWords( ulong[] state )
    {
        for (int indexRound = 0; indexRound < 24; ++indexRound)
        {
            Theta( state );
            Rho( state );
            Pi( state );
            Chi( state );
            Iota( state, indexRound );
        }
    }

    private void Theta( ulong[] A )
    {
        for (int index1 = 0; index1 < 5; ++index1)
        {
            C[index1] = 0UL;
            for (int index2 = 0; index2 < 5; ++index2)
                C[index1] ^= A[index1 + (5 * index2)];
        }
        for (int index3 = 0; index3 < 5; ++index3)
        {
            ulong num = (C[(index3 + 1) % 5] << 1) ^ (C[(index3 + 1) % 5] >> 63) ^ C[(index3 + 4) % 5];
            for (int index4 = 0; index4 < 5; ++index4)
                A[index3 + (5 * index4)] ^= num;
        }
    }

    private void Rho( ulong[] A )
    {
        for (int index1 = 0; index1 < 5; ++index1)
        {
            for (int index2 = 0; index2 < 5; ++index2)
            {
                int index3 = index1 + (5 * index2);
                A[index3] = KeccakRhoOffsets[index3] != 0 ? (A[index3] << KeccakRhoOffsets[index3]) ^ (A[index3] >> (64 - KeccakRhoOffsets[index3])) : A[index3];
            }
        }
    }

    private void Pi( ulong[] A )
    {
        Array.Copy( A, 0, tempA, 0, tempA.Length );
        for (int index1 = 0; index1 < 5; ++index1)
        {
            for (int index2 = 0; index2 < 5; ++index2)
                A[index2 + (5 * (((2 * index1) + (3 * index2)) % 5))] = tempA[index1 + (5 * index2)];
        }
    }

    private void Chi( ulong[] A )
    {
        for (int index1 = 0; index1 < 5; ++index1)
        {
            for (int index2 = 0; index2 < 5; ++index2)
                chiC[index2] = A[index2 + (5 * index1)] ^ (~A[((index2 + 1) % 5) + (5 * index1)] & A[((index2 + 2) % 5) + (5 * index1)]);
            for (int index3 = 0; index3 < 5; ++index3)
                A[index3 + (5 * index1)] = chiC[index3];
        }
    }

    private static void Iota( ulong[] A, int indexRound )
    {
        ulong[] numArray;
        (numArray = A)[0] = numArray[0] ^ KeccakRoundConstants[indexRound];
    }

    private void KeccakAbsorb( byte[] byteState, byte[] data, int dataInBytes ) => KeccakPermutationAfterXor( byteState, data, dataInBytes );

    private void KeccakExtract1024bits( byte[] byteState, byte[] data ) => Array.Copy( byteState, 0, data, 0, 128 );

    private void KeccakExtract( byte[] byteState, byte[] data, int laneCount ) => Array.Copy( byteState, 0, data, 0, laneCount * 8 );

    public virtual IMemoable Copy() => new KeccakDigest( this );

    public virtual void Reset( IMemoable other ) => CopyIn( (KeccakDigest)other );
}
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
        xBuf = new byte[8];
        Reset();
    }

    internal LongDigest( LongDigest t )
    {
        xBuf = new byte[t.xBuf.Length];
        CopyIn( t );
    }

    protected void CopyIn( LongDigest t )
    {
        Array.Copy( t.xBuf, 0, xBuf, 0, t.xBuf.Length );
        xBufOff = t.xBufOff;
        byteCount1 = t.byteCount1;
        byteCount2 = t.byteCount2;
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        H5 = t.H5;
        H6 = t.H6;
        H7 = t.H7;
        H8 = t.H8;
        Array.Copy( t.W, 0, W, 0, t.W.Length );
        wOff = t.wOff;
    }

    public void Update( byte input )
    {
        xBuf[xBufOff++] = input;
        if (xBufOff == xBuf.Length)
        {
            ProcessWord( xBuf, 0 );
            xBufOff = 0;
        }
        ++byteCount1;
    }

    public void BlockUpdate( byte[] input, int inOff, int length )
    {
        for (; xBufOff != 0 && length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
        while (length > xBuf.Length)
        {
            ProcessWord( input, inOff );
            inOff += xBuf.Length;
            length -= xBuf.Length;
            byteCount1 += xBuf.Length;
        }
        for (; length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
    }

    public void Finish()
    {
        AdjustByteCounts();
        long lowW = byteCount1 << 3;
        long byteCount2 = this.byteCount2;
        Update( 128 );
        while (xBufOff != 0)
            Update( 0 );
        ProcessLength( lowW, byteCount2 );
        ProcessBlock();
    }

    public virtual void Reset()
    {
        byteCount1 = 0L;
        byteCount2 = 0L;
        xBufOff = 0;
        for (int index = 0; index < xBuf.Length; ++index)
            xBuf[index] = 0;
        wOff = 0;
        Array.Clear( W, 0, W.Length );
    }

    internal void ProcessWord( byte[] input, int inOff )
    {
        W[wOff] = Pack.BE_To_UInt64( input, inOff );
        if (++wOff != 16)
            return;
        ProcessBlock();
    }

    private void AdjustByteCounts()
    {
        if (byteCount1 <= 2305843009213693951L)
            return;
        byteCount2 += byteCount1 >>> 61;
        byteCount1 &= 2305843009213693951L;
    }

    internal void ProcessLength( long lowW, long hiW )
    {
        if (wOff > 14)
            ProcessBlock();
        W[14] = (ulong)hiW;
        W[15] = (ulong)lowW;
    }

    internal void ProcessBlock()
    {
        AdjustByteCounts();
        for (int index = 16; index <= 79; ++index)
            W[index] = Sigma1( W[index - 2] ) + W[index - 7] + Sigma0( W[index - 15] ) + W[index - 16];
        ulong num1 = H1;
        ulong num2 = H2;
        ulong num3 = H3;
        ulong num4 = H4;
        ulong num5 = H5;
        ulong num6 = H6;
        ulong num7 = H7;
        ulong num8 = H8;
        int index1 = 0;
        for (int index2 = 0; index2 < 10; ++index2)
        {
            long num9 = (long)num8;
            long num10 = (long)Sum1( num5 ) + (long)Ch( num5, num6, num7 ) + (long)K[index1];
            ulong[] w1 = W;
            int index3 = index1;
            int index4 = index3 + 1;
            long num11 = (long)w1[index3];
            long num12 = num10 + num11;
            ulong num13 = (ulong)(num9 + num12);
            ulong num14 = num4 + num13;
            ulong num15 = num13 + Sum0( num1 ) + Maj( num1, num2, num3 );
            long num16 = (long)num7;
            long num17 = (long)Sum1( num14 ) + (long)Ch( num14, num5, num6 ) + (long)K[index4];
            ulong[] w2 = W;
            int index5 = index4;
            int index6 = index5 + 1;
            long num18 = (long)w2[index5];
            long num19 = num17 + num18;
            ulong num20 = (ulong)(num16 + num19);
            ulong num21 = num3 + num20;
            ulong num22 = num20 + Sum0( num15 ) + Maj( num15, num1, num2 );
            long num23 = (long)num6;
            long num24 = (long)Sum1( num21 ) + (long)Ch( num21, num14, num5 ) + (long)K[index6];
            ulong[] w3 = W;
            int index7 = index6;
            int index8 = index7 + 1;
            long num25 = (long)w3[index7];
            long num26 = num24 + num25;
            ulong num27 = (ulong)(num23 + num26);
            ulong num28 = num2 + num27;
            ulong num29 = num27 + Sum0( num22 ) + Maj( num22, num15, num1 );
            long num30 = (long)num5;
            long num31 = (long)Sum1( num28 ) + (long)Ch( num28, num21, num14 ) + (long)K[index8];
            ulong[] w4 = W;
            int index9 = index8;
            int index10 = index9 + 1;
            long num32 = (long)w4[index9];
            long num33 = num31 + num32;
            ulong num34 = (ulong)(num30 + num33);
            ulong num35 = num1 + num34;
            ulong num36 = num34 + Sum0( num29 ) + Maj( num29, num22, num15 );
            long num37 = (long)num14;
            long num38 = (long)Sum1( num35 ) + (long)Ch( num35, num28, num21 ) + (long)K[index10];
            ulong[] w5 = W;
            int index11 = index10;
            int index12 = index11 + 1;
            long num39 = (long)w5[index11];
            long num40 = num38 + num39;
            ulong num41 = (ulong)(num37 + num40);
            num8 = num15 + num41;
            num4 = num41 + Sum0( num36 ) + Maj( num36, num29, num22 );
            long num42 = (long)num21;
            long num43 = (long)Sum1( num8 ) + (long)Ch( num8, num35, num28 ) + (long)K[index12];
            ulong[] w6 = W;
            int index13 = index12;
            int index14 = index13 + 1;
            long num44 = (long)w6[index13];
            long num45 = num43 + num44;
            ulong num46 = (ulong)(num42 + num45);
            num7 = num22 + num46;
            num3 = num46 + Sum0( num4 ) + Maj( num4, num36, num29 );
            long num47 = (long)num28;
            long num48 = (long)Sum1( num7 ) + (long)Ch( num7, num8, num35 ) + (long)K[index14];
            ulong[] w7 = W;
            int index15 = index14;
            int index16 = index15 + 1;
            long num49 = (long)w7[index15];
            long num50 = num48 + num49;
            ulong num51 = (ulong)(num47 + num50);
            num6 = num29 + num51;
            num2 = num51 + Sum0( num3 ) + Maj( num3, num4, num36 );
            long num52 = (long)num35;
            long num53 = (long)Sum1( num6 ) + (long)Ch( num6, num7, num8 ) + (long)K[index16];
            ulong[] w8 = W;
            int index17 = index16;
            index1 = index17 + 1;
            long num54 = (long)w8[index17];
            long num55 = num53 + num54;
            ulong num56 = (ulong)(num52 + num55);
            num5 = num36 + num56;
            num1 = num56 + Sum0( num2 ) + Maj( num2, num3, num4 );
        }
        H1 += num1;
        H2 += num2;
        H3 += num3;
        H4 += num4;
        H5 += num5;
        H6 += num6;
        H7 += num7;
        H8 += num8;
        wOff = 0;
        Array.Clear( W, 0, 16 );
    }

    private static ulong Ch( ulong x, ulong y, ulong z ) => (ulong)(((long)x & (long)y) ^ (~(long)x & (long)z));

    private static ulong Maj( ulong x, ulong y, ulong z ) => (ulong)(((long)x & (long)y) ^ ((long)x & (long)z) ^ ((long)y & (long)z));

    private static ulong Sum0( ulong x ) => (ulong)((((long)x << 36) | (long)(x >> 28)) ^ (((long)x << 30) | (long)(x >> 34)) ^ (((long)x << 25) | (long)(x >> 39)));

    private static ulong Sum1( ulong x ) => (ulong)((((long)x << 50) | (long)(x >> 14)) ^ (((long)x << 46) | (long)(x >> 18)) ^ (((long)x << 23) | (long)(x >> 41)));

    private static ulong Sigma0( ulong x ) => (ulong)((((long)x << 63) | (long)(x >> 1)) ^ (((long)x << 56) | (long)(x >> 8))) ^ (x >> 7);

    private static ulong Sigma1( ulong x ) => (ulong)((((long)x << 45) | (long)(x >> 19)) ^ (((long)x << 3) | (long)(x >> 61))) ^ (x >> 6);

    public int GetByteLength() => MyByteLength;

    public abstract string AlgorithmName { get; }

    public abstract int GetDigestSize();

    public abstract int DoFinal( byte[] output, int outOff );

    public abstract IMemoable Copy();

    public abstract void Reset( IMemoable t );
}
public class NonMemoableDigest : IDigest
{
    protected readonly IDigest mBaseDigest;

    public NonMemoableDigest( IDigest baseDigest ) => mBaseDigest = baseDigest != null ? baseDigest : throw new ArgumentNullException( nameof( baseDigest ) );

    public virtual string AlgorithmName => mBaseDigest.AlgorithmName;

    public virtual int GetDigestSize() => mBaseDigest.GetDigestSize();

    public virtual void Update( byte input ) => mBaseDigest.Update( input );

    public virtual void BlockUpdate( byte[] input, int inOff, int len ) => mBaseDigest.BlockUpdate( input, inOff, len );

    public virtual int DoFinal( byte[] output, int outOff ) => mBaseDigest.DoFinal( output, outOff );

    public virtual void Reset() => mBaseDigest.Reset();

    public virtual int GetByteLength() => mBaseDigest.GetByteLength();
}
public class RipeMD128Digest : GeneralDigest
{
    private const int DigestLength = 16;
    private int H0;
    private int H1;
    private int H2;
    private int H3;
    private int[] X = new int[16];
    private int xOff;

    public RipeMD128Digest() => Reset();

    public RipeMD128Digest( RipeMD128Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( RipeMD128Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H0 = t.H0;
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "RIPEMD128";

    public override int GetDigestSize() => 16;

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff++] = (input[inOff] & byte.MaxValue) | ((input[inOff + 1] & byte.MaxValue) << 8) | ((input[inOff + 2] & byte.MaxValue) << 16) | ((input[inOff + 3] & byte.MaxValue) << 24);
        if (xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (int)(bitLength & uint.MaxValue);
        X[15] = (int)(bitLength >>> 32);
    }

    private void UnpackWord( int word, byte[] outBytes, int outOff )
    {
        outBytes[outOff] = (byte)word;
        outBytes[outOff + 1] = (byte)(word >>> 8);
        outBytes[outOff + 2] = (byte)(word >>> 16);
        outBytes[outOff + 3] = (byte)(word >>> 24);
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UnpackWord( H0, output, outOff );
        UnpackWord( H1, output, outOff + 4 );
        UnpackWord( H2, output, outOff + 8 );
        UnpackWord( H3, output, outOff + 12 );
        Reset();
        return 16;
    }

    public override void Reset()
    {
        base.Reset();
        H0 = 1732584193;
        H1 = -271733879;
        H2 = -1732584194;
        H3 = 271733878;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    private int RL( int x, int n ) => (x << n) | x >>> 32 - n;

    private int F1( int x, int y, int z ) => x ^ y ^ z;

    private int F2( int x, int y, int z ) => (x & y) | (~x & z);

    private int F3( int x, int y, int z ) => (x | ~y) ^ z;

    private int F4( int x, int y, int z ) => (x & z) | (y & ~z);

    private int F1( int a, int b, int c, int d, int x, int s ) => RL( a + F1( b, c, d ) + x, s );

    private int F2( int a, int b, int c, int d, int x, int s ) => RL( a + F2( b, c, d ) + x + 1518500249, s );

    private int F3( int a, int b, int c, int d, int x, int s ) => RL( a + F3( b, c, d ) + x + 1859775393, s );

    private int F4( int a, int b, int c, int d, int x, int s ) => RL( a + F4( b, c, d ) + x - 1894007588, s );

    private int FF1( int a, int b, int c, int d, int x, int s ) => RL( a + F1( b, c, d ) + x, s );

    private int FF2( int a, int b, int c, int d, int x, int s ) => RL( a + F2( b, c, d ) + x + 1836072691, s );

    private int FF3( int a, int b, int c, int d, int x, int s ) => RL( a + F3( b, c, d ) + x + 1548603684, s );

    private int FF4( int a, int b, int c, int d, int x, int s ) => RL( a + F4( b, c, d ) + x + 1352829926, s );

    internal override void ProcessBlock()
    {
        int h0;
        int a = h0 = H0;
        int h1;
        int num1 = h1 = H1;
        int h2;
        int num2 = h2 = H2;
        int h3;
        int num3 = h3 = H3;
        int num4 = F1( a, num1, num2, num3, X[0], 11 );
        int num5 = F1( num3, num4, num1, num2, X[1], 14 );
        int num6 = F1( num2, num5, num4, num1, X[2], 15 );
        int num7 = F1( num1, num6, num5, num4, X[3], 12 );
        int num8 = F1( num4, num7, num6, num5, X[4], 5 );
        int num9 = F1( num5, num8, num7, num6, X[5], 8 );
        int num10 = F1( num6, num9, num8, num7, X[6], 7 );
        int num11 = F1( num7, num10, num9, num8, X[7], 9 );
        int num12 = F1( num8, num11, num10, num9, X[8], 11 );
        int num13 = F1( num9, num12, num11, num10, X[9], 13 );
        int num14 = F1( num10, num13, num12, num11, X[10], 14 );
        int num15 = F1( num11, num14, num13, num12, X[11], 15 );
        int num16 = F1( num12, num15, num14, num13, X[12], 6 );
        int num17 = F1( num13, num16, num15, num14, X[13], 7 );
        int num18 = F1( num14, num17, num16, num15, X[14], 9 );
        int num19 = F1( num15, num18, num17, num16, X[15], 8 );
        int num20 = F2( num16, num19, num18, num17, X[7], 7 );
        int num21 = F2( num17, num20, num19, num18, X[4], 6 );
        int num22 = F2( num18, num21, num20, num19, X[13], 8 );
        int num23 = F2( num19, num22, num21, num20, X[1], 13 );
        int num24 = F2( num20, num23, num22, num21, X[10], 11 );
        int num25 = F2( num21, num24, num23, num22, X[6], 9 );
        int num26 = F2( num22, num25, num24, num23, X[15], 7 );
        int num27 = F2( num23, num26, num25, num24, X[3], 15 );
        int num28 = F2( num24, num27, num26, num25, X[12], 7 );
        int num29 = F2( num25, num28, num27, num26, X[0], 12 );
        int num30 = F2( num26, num29, num28, num27, X[9], 15 );
        int num31 = F2( num27, num30, num29, num28, X[5], 9 );
        int num32 = F2( num28, num31, num30, num29, X[2], 11 );
        int num33 = F2( num29, num32, num31, num30, X[14], 7 );
        int num34 = F2( num30, num33, num32, num31, X[11], 13 );
        int num35 = F2( num31, num34, num33, num32, X[8], 12 );
        int num36 = F3( num32, num35, num34, num33, X[3], 11 );
        int num37 = F3( num33, num36, num35, num34, X[10], 13 );
        int num38 = F3( num34, num37, num36, num35, X[14], 6 );
        int num39 = F3( num35, num38, num37, num36, X[4], 7 );
        int num40 = F3( num36, num39, num38, num37, X[9], 14 );
        int num41 = F3( num37, num40, num39, num38, X[15], 9 );
        int num42 = F3( num38, num41, num40, num39, X[8], 13 );
        int num43 = F3( num39, num42, num41, num40, X[1], 15 );
        int num44 = F3( num40, num43, num42, num41, X[2], 14 );
        int num45 = F3( num41, num44, num43, num42, X[7], 8 );
        int num46 = F3( num42, num45, num44, num43, X[0], 13 );
        int num47 = F3( num43, num46, num45, num44, X[6], 6 );
        int num48 = F3( num44, num47, num46, num45, X[13], 5 );
        int num49 = F3( num45, num48, num47, num46, X[11], 12 );
        int num50 = F3( num46, num49, num48, num47, X[5], 7 );
        int num51 = F3( num47, num50, num49, num48, X[12], 5 );
        int num52 = F4( num48, num51, num50, num49, X[1], 11 );
        int num53 = F4( num49, num52, num51, num50, X[9], 12 );
        int num54 = F4( num50, num53, num52, num51, X[11], 14 );
        int num55 = F4( num51, num54, num53, num52, X[10], 15 );
        int num56 = F4( num52, num55, num54, num53, X[0], 14 );
        int num57 = F4( num53, num56, num55, num54, X[8], 15 );
        int num58 = F4( num54, num57, num56, num55, X[12], 9 );
        int num59 = F4( num55, num58, num57, num56, X[4], 8 );
        int num60 = F4( num56, num59, num58, num57, X[13], 9 );
        int num61 = F4( num57, num60, num59, num58, X[3], 14 );
        int num62 = F4( num58, num61, num60, num59, X[7], 5 );
        int num63 = F4( num59, num62, num61, num60, X[15], 6 );
        int num64 = F4( num60, num63, num62, num61, X[14], 8 );
        int num65 = F4( num61, num64, num63, num62, X[5], 6 );
        int b1 = F4( num62, num65, num64, num63, X[6], 5 );
        int num66 = F4( num63, b1, num65, num64, X[2], 12 );
        int num67 = FF4( h0, h1, h2, h3, X[5], 8 );
        int num68 = FF4( h3, num67, h1, h2, X[14], 9 );
        int num69 = FF4( h2, num68, num67, h1, X[7], 9 );
        int num70 = FF4( h1, num69, num68, num67, X[0], 11 );
        int num71 = FF4( num67, num70, num69, num68, X[9], 13 );
        int num72 = FF4( num68, num71, num70, num69, X[2], 15 );
        int num73 = FF4( num69, num72, num71, num70, X[11], 15 );
        int num74 = FF4( num70, num73, num72, num71, X[4], 5 );
        int num75 = FF4( num71, num74, num73, num72, X[13], 7 );
        int num76 = FF4( num72, num75, num74, num73, X[6], 7 );
        int num77 = FF4( num73, num76, num75, num74, X[15], 8 );
        int num78 = FF4( num74, num77, num76, num75, X[8], 11 );
        int num79 = FF4( num75, num78, num77, num76, X[1], 14 );
        int num80 = FF4( num76, num79, num78, num77, X[10], 14 );
        int num81 = FF4( num77, num80, num79, num78, X[3], 12 );
        int num82 = FF4( num78, num81, num80, num79, X[12], 6 );
        int num83 = FF3( num79, num82, num81, num80, X[6], 9 );
        int num84 = FF3( num80, num83, num82, num81, X[11], 13 );
        int num85 = FF3( num81, num84, num83, num82, X[3], 15 );
        int num86 = FF3( num82, num85, num84, num83, X[7], 7 );
        int num87 = FF3( num83, num86, num85, num84, X[0], 12 );
        int num88 = FF3( num84, num87, num86, num85, X[13], 8 );
        int num89 = FF3( num85, num88, num87, num86, X[5], 9 );
        int num90 = FF3( num86, num89, num88, num87, X[10], 11 );
        int num91 = FF3( num87, num90, num89, num88, X[14], 7 );
        int num92 = FF3( num88, num91, num90, num89, X[15], 7 );
        int num93 = FF3( num89, num92, num91, num90, X[8], 12 );
        int num94 = FF3( num90, num93, num92, num91, X[12], 7 );
        int num95 = FF3( num91, num94, num93, num92, X[4], 6 );
        int num96 = FF3( num92, num95, num94, num93, X[9], 15 );
        int num97 = FF3( num93, num96, num95, num94, X[1], 13 );
        int num98 = FF3( num94, num97, num96, num95, X[2], 11 );
        int num99 = FF2( num95, num98, num97, num96, X[15], 9 );
        int num100 = FF2( num96, num99, num98, num97, X[5], 7 );
        int num101 = FF2( num97, num100, num99, num98, X[1], 15 );
        int num102 = FF2( num98, num101, num100, num99, X[3], 11 );
        int num103 = FF2( num99, num102, num101, num100, X[7], 8 );
        int num104 = FF2( num100, num103, num102, num101, X[14], 6 );
        int num105 = FF2( num101, num104, num103, num102, X[6], 6 );
        int num106 = FF2( num102, num105, num104, num103, X[9], 14 );
        int num107 = FF2( num103, num106, num105, num104, X[11], 12 );
        int num108 = FF2( num104, num107, num106, num105, X[8], 13 );
        int num109 = FF2( num105, num108, num107, num106, X[12], 5 );
        int num110 = FF2( num106, num109, num108, num107, X[2], 14 );
        int num111 = FF2( num107, num110, num109, num108, X[10], 13 );
        int num112 = FF2( num108, num111, num110, num109, X[0], 13 );
        int num113 = FF2( num109, num112, num111, num110, X[4], 7 );
        int num114 = FF2( num110, num113, num112, num111, X[13], 5 );
        int num115 = FF1( num111, num114, num113, num112, X[8], 15 );
        int num116 = FF1( num112, num115, num114, num113, X[6], 5 );
        int num117 = FF1( num113, num116, num115, num114, X[4], 8 );
        int num118 = FF1( num114, num117, num116, num115, X[1], 11 );
        int num119 = FF1( num115, num118, num117, num116, X[3], 14 );
        int num120 = FF1( num116, num119, num118, num117, X[11], 14 );
        int num121 = FF1( num117, num120, num119, num118, X[15], 6 );
        int num122 = FF1( num118, num121, num120, num119, X[0], 14 );
        int num123 = FF1( num119, num122, num121, num120, X[5], 6 );
        int num124 = FF1( num120, num123, num122, num121, X[12], 9 );
        int num125 = FF1( num121, num124, num123, num122, X[2], 12 );
        int num126 = FF1( num122, num125, num124, num123, X[13], 9 );
        int num127 = FF1( num123, num126, num125, num124, X[9], 12 );
        int num128 = FF1( num124, num127, num126, num125, X[7], 5 );
        int b2 = FF1( num125, num128, num127, num126, X[10], 15 );
        int num129 = FF1( num126, b2, num128, num127, X[14], 8 );
        int num130 = num128 + b1 + H1;
        H1 = H2 + num65 + num127;
        H2 = H3 + num64 + num129;
        H3 = H0 + num66 + b2;
        H0 = num130;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    public override IMemoable Copy() => new RipeMD128Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (RipeMD128Digest)other );
}
public abstract class Arrays
{
    public static bool AreEqual( bool[] a, bool[] b )
    {
        if (a == b)
            return true;
        return a != null && b != null && HaveSameContents( a, b );
    }

    public static bool AreEqual( char[] a, char[] b )
    {
        if (a == b)
            return true;
        return a != null && b != null && HaveSameContents( a, b );
    }

    public static bool AreEqual( byte[] a, byte[] b )
    {
        if (a == b)
            return true;
        return a != null && b != null && HaveSameContents( a, b );
    }

    [Obsolete( "Use 'AreEqual' method instead" )]
    public static bool AreSame( byte[] a, byte[] b ) => AreEqual( a, b );

    public static bool ConstantTimeAreEqual( byte[] a, byte[] b )
    {
        int length = a.Length;
        if (length != b.Length)
            return false;
        int num = 0;
        while (length != 0)
        {
            --length;
            num |= a[length] ^ b[length];
        }
        return num == 0;
    }

    public static bool AreEqual( int[] a, int[] b )
    {
        if (a == b)
            return true;
        return a != null && b != null && HaveSameContents( a, b );
    }

    [CLSCompliant( false )]
    public static bool AreEqual( uint[] a, uint[] b )
    {
        if (a == b)
            return true;
        return a != null && b != null && HaveSameContents( a, b );
    }

    private static bool HaveSameContents( bool[] a, bool[] b )
    {
        int length = a.Length;
        if (length != b.Length)
            return false;
        while (length != 0)
        {
            --length;
            if (a[length] != b[length])
                return false;
        }
        return true;
    }

    private static bool HaveSameContents( char[] a, char[] b )
    {
        int length = a.Length;
        if (length != b.Length)
            return false;
        while (length != 0)
        {
            --length;
            if (a[length] != b[length])
                return false;
        }
        return true;
    }

    private static bool HaveSameContents( byte[] a, byte[] b )
    {
        int length = a.Length;
        if (length != b.Length)
            return false;
        while (length != 0)
        {
            --length;
            if (a[length] != b[length])
                return false;
        }
        return true;
    }

    private static bool HaveSameContents( int[] a, int[] b )
    {
        int length = a.Length;
        if (length != b.Length)
            return false;
        while (length != 0)
        {
            --length;
            if (a[length] != b[length])
                return false;
        }
        return true;
    }

    private static bool HaveSameContents( uint[] a, uint[] b )
    {
        int length = a.Length;
        if (length != b.Length)
            return false;
        while (length != 0)
        {
            --length;
            if ((int)a[length] != (int)b[length])
                return false;
        }
        return true;
    }

    public static string ToString( object[] a )
    {
        StringBuilder stringBuilder = new StringBuilder( 91 );
        if (a.Length > 0)
        {
            stringBuilder.Append( a[0] );
            for (int index = 1; index < a.Length; ++index)
                stringBuilder.Append( ", " ).Append( a[index] );
        }
        stringBuilder.Append( ']' );
        return stringBuilder.ToString();
    }

    public static int GetHashCode( byte[] data )
    {
        if (data == null)
            return 0;
        int length = data.Length;
        int hashCode = length + 1;
        while (--length >= 0)
            hashCode = (hashCode * 257) ^ data[length];
        return hashCode;
    }

    public static int GetHashCode( byte[] data, int off, int len )
    {
        if (data == null)
            return 0;
        int num = len;
        int hashCode = num + 1;
        while (--num >= 0)
            hashCode = (hashCode * 257) ^ data[off + num];
        return hashCode;
    }

    public static int GetHashCode( int[] data )
    {
        if (data == null)
            return 0;
        int length = data.Length;
        int hashCode = length + 1;
        while (--length >= 0)
            hashCode = (hashCode * 257) ^ data[length];
        return hashCode;
    }

    public static int GetHashCode( int[] data, int off, int len )
    {
        if (data == null)
            return 0;
        int num = len;
        int hashCode = num + 1;
        while (--num >= 0)
            hashCode = (hashCode * 257) ^ data[off + num];
        return hashCode;
    }

    [CLSCompliant( false )]
    public static int GetHashCode( uint[] data )
    {
        if (data == null)
            return 0;
        int length = data.Length;
        int hashCode = length + 1;
        while (--length >= 0)
            hashCode = (hashCode * 257) ^ (int)data[length];
        return hashCode;
    }

    [CLSCompliant( false )]
    public static int GetHashCode( uint[] data, int off, int len )
    {
        if (data == null)
            return 0;
        int num = len;
        int hashCode = num + 1;
        while (--num >= 0)
            hashCode = (hashCode * 257) ^ (int)data[off + num];
        return hashCode;
    }

    [CLSCompliant( false )]
    public static int GetHashCode( ulong[] data )
    {
        if (data == null)
            return 0;
        int length = data.Length;
        int hashCode = length + 1;
        while (--length >= 0)
        {
            ulong num = data[length];
            hashCode = (((hashCode * 257) ^ (int)num) * 257) ^ (int)(num >> 32);
        }
        return hashCode;
    }

    [CLSCompliant( false )]
    public static int GetHashCode( ulong[] data, int off, int len )
    {
        if (data == null)
            return 0;
        int num1 = len;
        int hashCode = num1 + 1;
        while (--num1 >= 0)
        {
            ulong num2 = data[off + num1];
            hashCode = (((hashCode * 257) ^ (int)num2) * 257) ^ (int)(num2 >> 32);
        }
        return hashCode;
    }

    public static byte[] Clone( byte[] data ) => data != null ? (byte[])data.Clone() : null;

    public static byte[] Clone( byte[] data, byte[] existing )
    {
        if (data == null)
            return null;
        if (existing == null || existing.Length != data.Length)
            return Clone( data );
        Array.Copy( data, 0, existing, 0, existing.Length );
        return existing;
    }

    public static int[] Clone( int[] data ) => data != null ? (int[])data.Clone() : null;

    internal static uint[] Clone( uint[] data ) => data != null ? (uint[])data.Clone() : null;

    public static long[] Clone( long[] data ) => data != null ? (long[])data.Clone() : null;

    [CLSCompliant( false )]
    public static ulong[] Clone( ulong[] data ) => data != null ? (ulong[])data.Clone() : null;

    [CLSCompliant( false )]
    public static ulong[] Clone( ulong[] data, ulong[] existing )
    {
        if (data == null)
            return null;
        if (existing == null || existing.Length != data.Length)
            return Clone( data );
        Array.Copy( data, 0, existing, 0, existing.Length );
        return existing;
    }

    public static bool Contains( byte[] a, byte n )
    {
        for (int index = 0; index < a.Length; ++index)
        {
            if (a[index] == n)
                return true;
        }
        return false;
    }

    public static bool Contains( short[] a, short n )
    {
        for (int index = 0; index < a.Length; ++index)
        {
            if (a[index] == n)
                return true;
        }
        return false;
    }

    public static bool Contains( int[] a, int n )
    {
        for (int index = 0; index < a.Length; ++index)
        {
            if (a[index] == n)
                return true;
        }
        return false;
    }

    public static void Fill( byte[] buf, byte b )
    {
        int length = buf.Length;
        while (length > 0)
            buf[--length] = b;
    }

    public static byte[] CopyOf( byte[] data, int newLength )
    {
        byte[] destinationArray = new byte[newLength];
        Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
        return destinationArray;
    }

    public static char[] CopyOf( char[] data, int newLength )
    {
        char[] destinationArray = new char[newLength];
        Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
        return destinationArray;
    }

    public static int[] CopyOf( int[] data, int newLength )
    {
        int[] destinationArray = new int[newLength];
        Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
        return destinationArray;
    }

    public static long[] CopyOf( long[] data, int newLength )
    {
        long[] destinationArray = new long[newLength];
        Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
        return destinationArray;
    }

    public static BigInteger[] CopyOf( BigInteger[] data, int newLength )
    {
        BigInteger[] destinationArray = new BigInteger[newLength];
        Array.Copy( data, 0, destinationArray, 0, System.Math.Min( newLength, data.Length ) );
        return destinationArray;
    }

    public static byte[] CopyOfRange( byte[] data, int from, int to )
    {
        int length = GetLength( from, to );
        byte[] destinationArray = new byte[length];
        Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
        return destinationArray;
    }

    public static int[] CopyOfRange( int[] data, int from, int to )
    {
        int length = GetLength( from, to );
        int[] destinationArray = new int[length];
        Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
        return destinationArray;
    }

    public static long[] CopyOfRange( long[] data, int from, int to )
    {
        int length = GetLength( from, to );
        long[] destinationArray = new long[length];
        Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
        return destinationArray;
    }

    public static BigInteger[] CopyOfRange( BigInteger[] data, int from, int to )
    {
        int length = GetLength( from, to );
        BigInteger[] destinationArray = new BigInteger[length];
        Array.Copy( data, from, destinationArray, 0, System.Math.Min( length, data.Length - from ) );
        return destinationArray;
    }

    private static int GetLength( int from, int to )
    {
        int num = to - from;
        return num >= 0 ? num : throw new ArgumentException( from.ToString() + " > " + to );
    }

    public static byte[] Append( byte[] a, byte b )
    {
        if (a == null)
            return new byte[1] { b };
        int length = a.Length;
        byte[] destinationArray = new byte[length + 1];
        Array.Copy( a, 0, destinationArray, 0, length );
        destinationArray[length] = b;
        return destinationArray;
    }

    public static short[] Append( short[] a, short b )
    {
        if (a == null)
            return new short[1] { b };
        int length = a.Length;
        short[] destinationArray = new short[length + 1];
        Array.Copy( a, 0, destinationArray, 0, length );
        destinationArray[length] = b;
        return destinationArray;
    }

    public static int[] Append( int[] a, int b )
    {
        if (a == null)
            return new int[1] { b };
        int length = a.Length;
        int[] destinationArray = new int[length + 1];
        Array.Copy( a, 0, destinationArray, 0, length );
        destinationArray[length] = b;
        return destinationArray;
    }

    public static byte[] Concatenate( byte[] a, byte[] b )
    {
        if (a == null)
            return Clone( b );
        if (b == null)
            return Clone( a );
        byte[] destinationArray = new byte[a.Length + b.Length];
        Array.Copy( a, 0, destinationArray, 0, a.Length );
        Array.Copy( b, 0, destinationArray, a.Length, b.Length );
        return destinationArray;
    }

    public static byte[] ConcatenateAll( params byte[][] vs )
    {
        byte[][] numArray = new byte[vs.Length][];
        int num = 0;
        int length = 0;
        for (int index = 0; index < vs.Length; ++index)
        {
            byte[] v = vs[index];
            if (v != null)
            {
                numArray[num++] = v;
                length += v.Length;
            }
        }
        byte[] destinationArray = new byte[length];
        int destinationIndex = 0;
        for (int index = 0; index < num; ++index)
        {
            byte[] sourceArray = numArray[index];
            Array.Copy( sourceArray, 0, destinationArray, destinationIndex, sourceArray.Length );
            destinationIndex += sourceArray.Length;
        }
        return destinationArray;
    }

    public static int[] Concatenate( int[] a, int[] b )
    {
        if (a == null)
            return Clone( b );
        if (b == null)
            return Clone( a );
        int[] destinationArray = new int[a.Length + b.Length];
        Array.Copy( a, 0, destinationArray, 0, a.Length );
        Array.Copy( b, 0, destinationArray, a.Length, b.Length );
        return destinationArray;
    }

    public static byte[] Prepend( byte[] a, byte b )
    {
        if (a == null)
            return new byte[1] { b };
        int length = a.Length;
        byte[] destinationArray = new byte[length + 1];
        Array.Copy( a, 0, destinationArray, 1, length );
        destinationArray[0] = b;
        return destinationArray;
    }

    public static short[] Prepend( short[] a, short b )
    {
        if (a == null)
            return new short[1] { b };
        int length = a.Length;
        short[] destinationArray = new short[length + 1];
        Array.Copy( a, 0, destinationArray, 1, length );
        destinationArray[0] = b;
        return destinationArray;
    }

    public static int[] Prepend( int[] a, int b )
    {
        if (a == null)
            return new int[1] { b };
        int length = a.Length;
        int[] destinationArray = new int[length + 1];
        Array.Copy( a, 0, destinationArray, 1, length );
        destinationArray[0] = b;
        return destinationArray;
    }

    public static byte[] Reverse( byte[] a )
    {
        if (a == null)
            return null;
        int num = 0;
        int length = a.Length;
        byte[] numArray = new byte[length];
        while (--length >= 0)
            numArray[length] = a[num++];
        return numArray;
    }

    public static int[] Reverse( int[] a )
    {
        if (a == null)
            return null;
        int num = 0;
        int length = a.Length;
        int[] numArray = new int[length];
        while (--length >= 0)
            numArray[length] = a[num++];
        return numArray;
    }
}
public abstract class BigIntegers
{
    private const int MaxIterations = 1000;

    public static byte[] AsUnsignedByteArray( BigInteger n ) => n.ToByteArrayUnsigned();

    public static byte[] AsUnsignedByteArray( int length, BigInteger n )
    {
        byte[] byteArrayUnsigned = n.ToByteArrayUnsigned();
        if (byteArrayUnsigned.Length > length)
            throw new ArgumentException( "standard length exceeded", nameof( n ) );
        if (byteArrayUnsigned.Length == length)
            return byteArrayUnsigned;
        byte[] destinationArray = new byte[length];
        Array.Copy( byteArrayUnsigned, 0, destinationArray, destinationArray.Length - byteArrayUnsigned.Length, byteArrayUnsigned.Length );
        return destinationArray;
    }

    public static BigInteger CreateRandomInRange(
      BigInteger min,
      BigInteger max,
      SecureRandom random )
    {
        int num = min.CompareTo( max );
        if (num >= 0)
        {
            if (num > 0)
                throw new ArgumentException( "'min' may not be greater than 'max'" );
            return min;
        }
        if (min.BitLength > max.BitLength / 2)
            return CreateRandomInRange( BigInteger.Zero, max.Subtract( min ), random ).Add( min );
        for (int index = 0; index < 1000; ++index)
        {
            BigInteger randomInRange = new BigInteger( max.BitLength, random );
            if (randomInRange.CompareTo( min ) >= 0 && randomInRange.CompareTo( max ) <= 0)
                return randomInRange;
        }
        return new BigInteger( max.Subtract( min ).BitLength - 1, random ).Add( min );
    }
}
public class RipeMD160Digest : GeneralDigest
{
    private const int DigestLength = 20;
    private int H0;
    private int H1;
    private int H2;
    private int H3;
    private int H4;
    private int[] X = new int[16];
    private int xOff;

    public RipeMD160Digest() => Reset();

    public RipeMD160Digest( RipeMD160Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( RipeMD160Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H0 = t.H0;
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "RIPEMD160";

    public override int GetDigestSize() => 20;

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff++] = (input[inOff] & byte.MaxValue) | ((input[inOff + 1] & byte.MaxValue) << 8) | ((input[inOff + 2] & byte.MaxValue) << 16) | ((input[inOff + 3] & byte.MaxValue) << 24);
        if (xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (int)(bitLength & uint.MaxValue);
        X[15] = (int)(bitLength >>> 32);
    }

    private void UnpackWord( int word, byte[] outBytes, int outOff )
    {
        outBytes[outOff] = (byte)word;
        outBytes[outOff + 1] = (byte)(word >>> 8);
        outBytes[outOff + 2] = (byte)(word >>> 16);
        outBytes[outOff + 3] = (byte)(word >>> 24);
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UnpackWord( H0, output, outOff );
        UnpackWord( H1, output, outOff + 4 );
        UnpackWord( H2, output, outOff + 8 );
        UnpackWord( H3, output, outOff + 12 );
        UnpackWord( H4, output, outOff + 16 );
        Reset();
        return 20;
    }

    public override void Reset()
    {
        base.Reset();
        H0 = 1732584193;
        H1 = -271733879;
        H2 = -1732584194;
        H3 = 271733878;
        H4 = -1009589776;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    private int RL( int x, int n ) => (x << n) | x >>> 32 - n;

    private int F1( int x, int y, int z ) => x ^ y ^ z;

    private int F2( int x, int y, int z ) => (x & y) | (~x & z);

    private int F3( int x, int y, int z ) => (x | ~y) ^ z;

    private int F4( int x, int y, int z ) => (x & z) | (y & ~z);

    private int F5( int x, int y, int z ) => x ^ (y | ~z);

    internal override void ProcessBlock()
    {
        int h0;
        int num1 = h0 = H0;
        int h1;
        int num2 = h1 = H1;
        int h2;
        int num3 = h2 = H2;
        int h3;
        int z1 = h3 = H3;
        int h4;
        int num4 = h4 = H4;
        int num5 = RL( num1 + F1( num2, num3, z1 ) + X[0], 11 ) + num4;
        int z2 = RL( num3, 10 );
        int num6 = RL( num4 + F1( num5, num2, z2 ) + X[1], 14 ) + z1;
        int z3 = RL( num2, 10 );
        int num7 = RL( z1 + F1( num6, num5, z3 ) + X[2], 15 ) + z2;
        int z4 = RL( num5, 10 );
        int num8 = RL( z2 + F1( num7, num6, z4 ) + X[3], 12 ) + z3;
        int z5 = RL( num6, 10 );
        int num9 = RL( z3 + F1( num8, num7, z5 ) + X[4], 5 ) + z4;
        int z6 = RL( num7, 10 );
        int num10 = RL( z4 + F1( num9, num8, z6 ) + X[5], 8 ) + z5;
        int z7 = RL( num8, 10 );
        int num11 = RL( z5 + F1( num10, num9, z7 ) + X[6], 7 ) + z6;
        int z8 = RL( num9, 10 );
        int num12 = RL( z6 + F1( num11, num10, z8 ) + X[7], 9 ) + z7;
        int z9 = RL( num10, 10 );
        int num13 = RL( z7 + F1( num12, num11, z9 ) + X[8], 11 ) + z8;
        int z10 = RL( num11, 10 );
        int num14 = RL( z8 + F1( num13, num12, z10 ) + X[9], 13 ) + z9;
        int z11 = RL( num12, 10 );
        int num15 = RL( z9 + F1( num14, num13, z11 ) + X[10], 14 ) + z10;
        int z12 = RL( num13, 10 );
        int num16 = RL( z10 + F1( num15, num14, z12 ) + X[11], 15 ) + z11;
        int z13 = RL( num14, 10 );
        int num17 = RL( z11 + F1( num16, num15, z13 ) + X[12], 6 ) + z12;
        int z14 = RL( num15, 10 );
        int num18 = RL( z12 + F1( num17, num16, z14 ) + X[13], 7 ) + z13;
        int z15 = RL( num16, 10 );
        int num19 = RL( z13 + F1( num18, num17, z15 ) + X[14], 9 ) + z14;
        int z16 = RL( num17, 10 );
        int num20 = RL( z14 + F1( num19, num18, z16 ) + X[15], 8 ) + z15;
        int z17 = RL( num18, 10 );
        int num21 = RL( h0 + F5( h1, h2, h3 ) + X[5] + 1352829926, 8 ) + h4;
        int z18 = RL( h2, 10 );
        int num22 = RL( h4 + F5( num21, h1, z18 ) + X[14] + 1352829926, 9 ) + h3;
        int z19 = RL( h1, 10 );
        int num23 = RL( h3 + F5( num22, num21, z19 ) + X[7] + 1352829926, 9 ) + z18;
        int z20 = RL( num21, 10 );
        int num24 = RL( z18 + F5( num23, num22, z20 ) + X[0] + 1352829926, 11 ) + z19;
        int z21 = RL( num22, 10 );
        int num25 = RL( z19 + F5( num24, num23, z21 ) + X[9] + 1352829926, 13 ) + z20;
        int z22 = RL( num23, 10 );
        int num26 = RL( z20 + F5( num25, num24, z22 ) + X[2] + 1352829926, 15 ) + z21;
        int z23 = RL( num24, 10 );
        int num27 = RL( z21 + F5( num26, num25, z23 ) + X[11] + 1352829926, 15 ) + z22;
        int z24 = RL( num25, 10 );
        int num28 = RL( z22 + F5( num27, num26, z24 ) + X[4] + 1352829926, 5 ) + z23;
        int z25 = RL( num26, 10 );
        int num29 = RL( z23 + F5( num28, num27, z25 ) + X[13] + 1352829926, 7 ) + z24;
        int z26 = RL( num27, 10 );
        int num30 = RL( z24 + F5( num29, num28, z26 ) + X[6] + 1352829926, 7 ) + z25;
        int z27 = RL( num28, 10 );
        int num31 = RL( z25 + F5( num30, num29, z27 ) + X[15] + 1352829926, 8 ) + z26;
        int z28 = RL( num29, 10 );
        int num32 = RL( z26 + F5( num31, num30, z28 ) + X[8] + 1352829926, 11 ) + z27;
        int z29 = RL( num30, 10 );
        int num33 = RL( z27 + F5( num32, num31, z29 ) + X[1] + 1352829926, 14 ) + z28;
        int z30 = RL( num31, 10 );
        int num34 = RL( z28 + F5( num33, num32, z30 ) + X[10] + 1352829926, 14 ) + z29;
        int z31 = RL( num32, 10 );
        int num35 = RL( z29 + F5( num34, num33, z31 ) + X[3] + 1352829926, 12 ) + z30;
        int z32 = RL( num33, 10 );
        int num36 = RL( z30 + F5( num35, num34, z32 ) + X[12] + 1352829926, 6 ) + z31;
        int z33 = RL( num34, 10 );
        int num37 = RL( z15 + F2( num20, num19, z17 ) + X[7] + 1518500249, 7 ) + z16;
        int z34 = RL( num19, 10 );
        int num38 = RL( z16 + F2( num37, num20, z34 ) + X[4] + 1518500249, 6 ) + z17;
        int z35 = RL( num20, 10 );
        int num39 = RL( z17 + F2( num38, num37, z35 ) + X[13] + 1518500249, 8 ) + z34;
        int z36 = RL( num37, 10 );
        int num40 = RL( z34 + F2( num39, num38, z36 ) + X[1] + 1518500249, 13 ) + z35;
        int z37 = RL( num38, 10 );
        int num41 = RL( z35 + F2( num40, num39, z37 ) + X[10] + 1518500249, 11 ) + z36;
        int z38 = RL( num39, 10 );
        int num42 = RL( z36 + F2( num41, num40, z38 ) + X[6] + 1518500249, 9 ) + z37;
        int z39 = RL( num40, 10 );
        int num43 = RL( z37 + F2( num42, num41, z39 ) + X[15] + 1518500249, 7 ) + z38;
        int z40 = RL( num41, 10 );
        int num44 = RL( z38 + F2( num43, num42, z40 ) + X[3] + 1518500249, 15 ) + z39;
        int z41 = RL( num42, 10 );
        int num45 = RL( z39 + F2( num44, num43, z41 ) + X[12] + 1518500249, 7 ) + z40;
        int z42 = RL( num43, 10 );
        int num46 = RL( z40 + F2( num45, num44, z42 ) + X[0] + 1518500249, 12 ) + z41;
        int z43 = RL( num44, 10 );
        int num47 = RL( z41 + F2( num46, num45, z43 ) + X[9] + 1518500249, 15 ) + z42;
        int z44 = RL( num45, 10 );
        int num48 = RL( z42 + F2( num47, num46, z44 ) + X[5] + 1518500249, 9 ) + z43;
        int z45 = RL( num46, 10 );
        int num49 = RL( z43 + F2( num48, num47, z45 ) + X[2] + 1518500249, 11 ) + z44;
        int z46 = RL( num47, 10 );
        int num50 = RL( z44 + F2( num49, num48, z46 ) + X[14] + 1518500249, 7 ) + z45;
        int z47 = RL( num48, 10 );
        int num51 = RL( z45 + F2( num50, num49, z47 ) + X[11] + 1518500249, 13 ) + z46;
        int z48 = RL( num49, 10 );
        int num52 = RL( z46 + F2( num51, num50, z48 ) + X[8] + 1518500249, 12 ) + z47;
        int z49 = RL( num50, 10 );
        int num53 = RL( z31 + F4( num36, num35, z33 ) + X[6] + 1548603684, 9 ) + z32;
        int z50 = RL( num35, 10 );
        int num54 = RL( z32 + F4( num53, num36, z50 ) + X[11] + 1548603684, 13 ) + z33;
        int z51 = RL( num36, 10 );
        int num55 = RL( z33 + F4( num54, num53, z51 ) + X[3] + 1548603684, 15 ) + z50;
        int z52 = RL( num53, 10 );
        int num56 = RL( z50 + F4( num55, num54, z52 ) + X[7] + 1548603684, 7 ) + z51;
        int z53 = RL( num54, 10 );
        int num57 = RL( z51 + F4( num56, num55, z53 ) + X[0] + 1548603684, 12 ) + z52;
        int z54 = RL( num55, 10 );
        int num58 = RL( z52 + F4( num57, num56, z54 ) + X[13] + 1548603684, 8 ) + z53;
        int z55 = RL( num56, 10 );
        int num59 = RL( z53 + F4( num58, num57, z55 ) + X[5] + 1548603684, 9 ) + z54;
        int z56 = RL( num57, 10 );
        int num60 = RL( z54 + F4( num59, num58, z56 ) + X[10] + 1548603684, 11 ) + z55;
        int z57 = RL( num58, 10 );
        int num61 = RL( z55 + F4( num60, num59, z57 ) + X[14] + 1548603684, 7 ) + z56;
        int z58 = RL( num59, 10 );
        int num62 = RL( z56 + F4( num61, num60, z58 ) + X[15] + 1548603684, 7 ) + z57;
        int z59 = RL( num60, 10 );
        int num63 = RL( z57 + F4( num62, num61, z59 ) + X[8] + 1548603684, 12 ) + z58;
        int z60 = RL( num61, 10 );
        int num64 = RL( z58 + F4( num63, num62, z60 ) + X[12] + 1548603684, 7 ) + z59;
        int z61 = RL( num62, 10 );
        int num65 = RL( z59 + F4( num64, num63, z61 ) + X[4] + 1548603684, 6 ) + z60;
        int z62 = RL( num63, 10 );
        int num66 = RL( z60 + F4( num65, num64, z62 ) + X[9] + 1548603684, 15 ) + z61;
        int z63 = RL( num64, 10 );
        int num67 = RL( z61 + F4( num66, num65, z63 ) + X[1] + 1548603684, 13 ) + z62;
        int z64 = RL( num65, 10 );
        int num68 = RL( z62 + F4( num67, num66, z64 ) + X[2] + 1548603684, 11 ) + z63;
        int z65 = RL( num66, 10 );
        int num69 = RL( z47 + F3( num52, num51, z49 ) + X[3] + 1859775393, 11 ) + z48;
        int z66 = RL( num51, 10 );
        int num70 = RL( z48 + F3( num69, num52, z66 ) + X[10] + 1859775393, 13 ) + z49;
        int z67 = RL( num52, 10 );
        int num71 = RL( z49 + F3( num70, num69, z67 ) + X[14] + 1859775393, 6 ) + z66;
        int z68 = RL( num69, 10 );
        int num72 = RL( z66 + F3( num71, num70, z68 ) + X[4] + 1859775393, 7 ) + z67;
        int z69 = RL( num70, 10 );
        int num73 = RL( z67 + F3( num72, num71, z69 ) + X[9] + 1859775393, 14 ) + z68;
        int z70 = RL( num71, 10 );
        int num74 = RL( z68 + F3( num73, num72, z70 ) + X[15] + 1859775393, 9 ) + z69;
        int z71 = RL( num72, 10 );
        int num75 = RL( z69 + F3( num74, num73, z71 ) + X[8] + 1859775393, 13 ) + z70;
        int z72 = RL( num73, 10 );
        int num76 = RL( z70 + F3( num75, num74, z72 ) + X[1] + 1859775393, 15 ) + z71;
        int z73 = RL( num74, 10 );
        int num77 = RL( z71 + F3( num76, num75, z73 ) + X[2] + 1859775393, 14 ) + z72;
        int z74 = RL( num75, 10 );
        int num78 = RL( z72 + F3( num77, num76, z74 ) + X[7] + 1859775393, 8 ) + z73;
        int z75 = RL( num76, 10 );
        int num79 = RL( z73 + F3( num78, num77, z75 ) + X[0] + 1859775393, 13 ) + z74;
        int z76 = RL( num77, 10 );
        int num80 = RL( z74 + F3( num79, num78, z76 ) + X[6] + 1859775393, 6 ) + z75;
        int z77 = RL( num78, 10 );
        int num81 = RL( z75 + F3( num80, num79, z77 ) + X[13] + 1859775393, 5 ) + z76;
        int z78 = RL( num79, 10 );
        int num82 = RL( z76 + F3( num81, num80, z78 ) + X[11] + 1859775393, 12 ) + z77;
        int z79 = RL( num80, 10 );
        int num83 = RL( z77 + F3( num82, num81, z79 ) + X[5] + 1859775393, 7 ) + z78;
        int z80 = RL( num81, 10 );
        int num84 = RL( z78 + F3( num83, num82, z80 ) + X[12] + 1859775393, 5 ) + z79;
        int z81 = RL( num82, 10 );
        int num85 = RL( z63 + F3( num68, num67, z65 ) + X[15] + 1836072691, 9 ) + z64;
        int z82 = RL( num67, 10 );
        int num86 = RL( z64 + F3( num85, num68, z82 ) + X[5] + 1836072691, 7 ) + z65;
        int z83 = RL( num68, 10 );
        int num87 = RL( z65 + F3( num86, num85, z83 ) + X[1] + 1836072691, 15 ) + z82;
        int z84 = RL( num85, 10 );
        int num88 = RL( z82 + F3( num87, num86, z84 ) + X[3] + 1836072691, 11 ) + z83;
        int z85 = RL( num86, 10 );
        int num89 = RL( z83 + F3( num88, num87, z85 ) + X[7] + 1836072691, 8 ) + z84;
        int z86 = RL( num87, 10 );
        int num90 = RL( z84 + F3( num89, num88, z86 ) + X[14] + 1836072691, 6 ) + z85;
        int z87 = RL( num88, 10 );
        int num91 = RL( z85 + F3( num90, num89, z87 ) + X[6] + 1836072691, 6 ) + z86;
        int z88 = RL( num89, 10 );
        int num92 = RL( z86 + F3( num91, num90, z88 ) + X[9] + 1836072691, 14 ) + z87;
        int z89 = RL( num90, 10 );
        int num93 = RL( z87 + F3( num92, num91, z89 ) + X[11] + 1836072691, 12 ) + z88;
        int z90 = RL( num91, 10 );
        int num94 = RL( z88 + F3( num93, num92, z90 ) + X[8] + 1836072691, 13 ) + z89;
        int z91 = RL( num92, 10 );
        int num95 = RL( z89 + F3( num94, num93, z91 ) + X[12] + 1836072691, 5 ) + z90;
        int z92 = RL( num93, 10 );
        int num96 = RL( z90 + F3( num95, num94, z92 ) + X[2] + 1836072691, 14 ) + z91;
        int z93 = RL( num94, 10 );
        int num97 = RL( z91 + F3( num96, num95, z93 ) + X[10] + 1836072691, 13 ) + z92;
        int z94 = RL( num95, 10 );
        int num98 = RL( z92 + F3( num97, num96, z94 ) + X[0] + 1836072691, 13 ) + z93;
        int z95 = RL( num96, 10 );
        int num99 = RL( z93 + F3( num98, num97, z95 ) + X[4] + 1836072691, 7 ) + z94;
        int z96 = RL( num97, 10 );
        int num100 = RL( z94 + F3( num99, num98, z96 ) + X[13] + 1836072691, 5 ) + z95;
        int z97 = RL( num98, 10 );
        int num101 = RL( z79 + F4( num84, num83, z81 ) + X[1] - 1894007588, 11 ) + z80;
        int z98 = RL( num83, 10 );
        int num102 = RL( z80 + F4( num101, num84, z98 ) + X[9] - 1894007588, 12 ) + z81;
        int z99 = RL( num84, 10 );
        int num103 = RL( z81 + F4( num102, num101, z99 ) + X[11] - 1894007588, 14 ) + z98;
        int z100 = RL( num101, 10 );
        int num104 = RL( z98 + F4( num103, num102, z100 ) + X[10] - 1894007588, 15 ) + z99;
        int z101 = RL( num102, 10 );
        int num105 = RL( z99 + F4( num104, num103, z101 ) + X[0] - 1894007588, 14 ) + z100;
        int z102 = RL( num103, 10 );
        int num106 = RL( z100 + F4( num105, num104, z102 ) + X[8] - 1894007588, 15 ) + z101;
        int z103 = RL( num104, 10 );
        int num107 = RL( z101 + F4( num106, num105, z103 ) + X[12] - 1894007588, 9 ) + z102;
        int z104 = RL( num105, 10 );
        int num108 = RL( z102 + F4( num107, num106, z104 ) + X[4] - 1894007588, 8 ) + z103;
        int z105 = RL( num106, 10 );
        int num109 = RL( z103 + F4( num108, num107, z105 ) + X[13] - 1894007588, 9 ) + z104;
        int z106 = RL( num107, 10 );
        int num110 = RL( z104 + F4( num109, num108, z106 ) + X[3] - 1894007588, 14 ) + z105;
        int z107 = RL( num108, 10 );
        int num111 = RL( z105 + F4( num110, num109, z107 ) + X[7] - 1894007588, 5 ) + z106;
        int z108 = RL( num109, 10 );
        int num112 = RL( z106 + F4( num111, num110, z108 ) + X[15] - 1894007588, 6 ) + z107;
        int z109 = RL( num110, 10 );
        int num113 = RL( z107 + F4( num112, num111, z109 ) + X[14] - 1894007588, 8 ) + z108;
        int z110 = RL( num111, 10 );
        int num114 = RL( z108 + F4( num113, num112, z110 ) + X[5] - 1894007588, 6 ) + z109;
        int z111 = RL( num112, 10 );
        int num115 = RL( z109 + F4( num114, num113, z111 ) + X[6] - 1894007588, 5 ) + z110;
        int z112 = RL( num113, 10 );
        int num116 = RL( z110 + F4( num115, num114, z112 ) + X[2] - 1894007588, 12 ) + z111;
        int z113 = RL( num114, 10 );
        int num117 = RL( z95 + F2( num100, num99, z97 ) + X[8] + 2053994217, 15 ) + z96;
        int z114 = RL( num99, 10 );
        int num118 = RL( z96 + F2( num117, num100, z114 ) + X[6] + 2053994217, 5 ) + z97;
        int z115 = RL( num100, 10 );
        int num119 = RL( z97 + F2( num118, num117, z115 ) + X[4] + 2053994217, 8 ) + z114;
        int z116 = RL( num117, 10 );
        int num120 = RL( z114 + F2( num119, num118, z116 ) + X[1] + 2053994217, 11 ) + z115;
        int z117 = RL( num118, 10 );
        int num121 = RL( z115 + F2( num120, num119, z117 ) + X[3] + 2053994217, 14 ) + z116;
        int z118 = RL( num119, 10 );
        int num122 = RL( z116 + F2( num121, num120, z118 ) + X[11] + 2053994217, 14 ) + z117;
        int z119 = RL( num120, 10 );
        int num123 = RL( z117 + F2( num122, num121, z119 ) + X[15] + 2053994217, 6 ) + z118;
        int z120 = RL( num121, 10 );
        int num124 = RL( z118 + F2( num123, num122, z120 ) + X[0] + 2053994217, 14 ) + z119;
        int z121 = RL( num122, 10 );
        int num125 = RL( z119 + F2( num124, num123, z121 ) + X[5] + 2053994217, 6 ) + z120;
        int z122 = RL( num123, 10 );
        int num126 = RL( z120 + F2( num125, num124, z122 ) + X[12] + 2053994217, 9 ) + z121;
        int z123 = RL( num124, 10 );
        int num127 = RL( z121 + F2( num126, num125, z123 ) + X[2] + 2053994217, 12 ) + z122;
        int z124 = RL( num125, 10 );
        int num128 = RL( z122 + F2( num127, num126, z124 ) + X[13] + 2053994217, 9 ) + z123;
        int z125 = RL( num126, 10 );
        int num129 = RL( z123 + F2( num128, num127, z125 ) + X[9] + 2053994217, 12 ) + z124;
        int z126 = RL( num127, 10 );
        int num130 = RL( z124 + F2( num129, num128, z126 ) + X[7] + 2053994217, 5 ) + z125;
        int z127 = RL( num128, 10 );
        int num131 = RL( z125 + F2( num130, num129, z127 ) + X[10] + 2053994217, 15 ) + z126;
        int z128 = RL( num129, 10 );
        int num132 = RL( z126 + F2( num131, num130, z128 ) + X[14] + 2053994217, 8 ) + z127;
        int z129 = RL( num130, 10 );
        int num133 = RL( z111 + F5( num116, num115, z113 ) + X[4] - 1454113458, 9 ) + z112;
        int z130 = RL( num115, 10 );
        int num134 = RL( z112 + F5( num133, num116, z130 ) + X[0] - 1454113458, 15 ) + z113;
        int z131 = RL( num116, 10 );
        int num135 = RL( z113 + F5( num134, num133, z131 ) + X[5] - 1454113458, 5 ) + z130;
        int z132 = RL( num133, 10 );
        int num136 = RL( z130 + F5( num135, num134, z132 ) + X[9] - 1454113458, 11 ) + z131;
        int z133 = RL( num134, 10 );
        int num137 = RL( z131 + F5( num136, num135, z133 ) + X[7] - 1454113458, 6 ) + z132;
        int z134 = RL( num135, 10 );
        int num138 = RL( z132 + F5( num137, num136, z134 ) + X[12] - 1454113458, 8 ) + z133;
        int z135 = RL( num136, 10 );
        int num139 = RL( z133 + F5( num138, num137, z135 ) + X[2] - 1454113458, 13 ) + z134;
        int z136 = RL( num137, 10 );
        int num140 = RL( z134 + F5( num139, num138, z136 ) + X[10] - 1454113458, 12 ) + z135;
        int z137 = RL( num138, 10 );
        int num141 = RL( z135 + F5( num140, num139, z137 ) + X[14] - 1454113458, 5 ) + z136;
        int z138 = RL( num139, 10 );
        int num142 = RL( z136 + F5( num141, num140, z138 ) + X[1] - 1454113458, 12 ) + z137;
        int z139 = RL( num140, 10 );
        int num143 = RL( z137 + F5( num142, num141, z139 ) + X[3] - 1454113458, 13 ) + z138;
        int z140 = RL( num141, 10 );
        int num144 = RL( z138 + F5( num143, num142, z140 ) + X[8] - 1454113458, 14 ) + z139;
        int z141 = RL( num142, 10 );
        int num145 = RL( z139 + F5( num144, num143, z141 ) + X[11] - 1454113458, 11 ) + z140;
        int z142 = RL( num143, 10 );
        int num146 = RL( z140 + F5( num145, num144, z142 ) + X[6] - 1454113458, 8 ) + z141;
        int z143 = RL( num144, 10 );
        int x1 = RL( z141 + F5( num146, num145, z143 ) + X[15] - 1454113458, 5 ) + z142;
        int z144 = RL( num145, 10 );
        int num147 = RL( z142 + F5( x1, num146, z144 ) + X[13] - 1454113458, 6 ) + z143;
        int num148 = RL( num146, 10 );
        int num149 = RL( z127 + F1( num132, num131, z129 ) + X[12], 8 ) + z128;
        int z145 = RL( num131, 10 );
        int num150 = RL( z128 + F1( num149, num132, z145 ) + X[15], 5 ) + z129;
        int z146 = RL( num132, 10 );
        int num151 = RL( z129 + F1( num150, num149, z146 ) + X[10], 12 ) + z145;
        int z147 = RL( num149, 10 );
        int num152 = RL( z145 + F1( num151, num150, z147 ) + X[4], 9 ) + z146;
        int z148 = RL( num150, 10 );
        int num153 = RL( z146 + F1( num152, num151, z148 ) + X[1], 12 ) + z147;
        int z149 = RL( num151, 10 );
        int num154 = RL( z147 + F1( num153, num152, z149 ) + X[5], 5 ) + z148;
        int z150 = RL( num152, 10 );
        int num155 = RL( z148 + F1( num154, num153, z150 ) + X[8], 14 ) + z149;
        int z151 = RL( num153, 10 );
        int num156 = RL( z149 + F1( num155, num154, z151 ) + X[7], 6 ) + z150;
        int z152 = RL( num154, 10 );
        int num157 = RL( z150 + F1( num156, num155, z152 ) + X[6], 8 ) + z151;
        int z153 = RL( num155, 10 );
        int num158 = RL( z151 + F1( num157, num156, z153 ) + X[2], 13 ) + z152;
        int z154 = RL( num156, 10 );
        int num159 = RL( z152 + F1( num158, num157, z154 ) + X[13], 6 ) + z153;
        int z155 = RL( num157, 10 );
        int num160 = RL( z153 + F1( num159, num158, z155 ) + X[14], 5 ) + z154;
        int z156 = RL( num158, 10 );
        int num161 = RL( z154 + F1( num160, num159, z156 ) + X[0], 15 ) + z155;
        int z157 = RL( num159, 10 );
        int num162 = RL( z155 + F1( num161, num160, z157 ) + X[3], 13 ) + z156;
        int z158 = RL( num160, 10 );
        int x2 = RL( z156 + F1( num162, num161, z158 ) + X[9], 11 ) + z157;
        int z159 = RL( num161, 10 );
        int num163 = RL( z157 + F1( x2, num162, z159 ) + X[11], 11 ) + z158;
        int num164 = RL( num162, 10 ) + x1 + H1;
        H1 = H2 + num148 + z159;
        H2 = H3 + z144 + z158;
        H3 = H4 + z143 + num163;
        H4 = H0 + num147 + x2;
        H0 = num164;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    public override IMemoable Copy() => new RipeMD160Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (RipeMD160Digest)other );
}
public class RipeMD256Digest : GeneralDigest
{
    private const int DigestLength = 32;
    private int H0;
    private int H1;
    private int H2;
    private int H3;
    private int H4;
    private int H5;
    private int H6;
    private int H7;
    private int[] X = new int[16];
    private int xOff;

    public override string AlgorithmName => "RIPEMD256";

    public override int GetDigestSize() => 32;

    public RipeMD256Digest() => Reset();

    public RipeMD256Digest( RipeMD256Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( RipeMD256Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H0 = t.H0;
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        H5 = t.H5;
        H6 = t.H6;
        H7 = t.H7;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff++] = (input[inOff] & byte.MaxValue) | ((input[inOff + 1] & byte.MaxValue) << 8) | ((input[inOff + 2] & byte.MaxValue) << 16) | ((input[inOff + 3] & byte.MaxValue) << 24);
        if (xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (int)(bitLength & uint.MaxValue);
        X[15] = (int)(bitLength >>> 32);
    }

    private void UnpackWord( int word, byte[] outBytes, int outOff )
    {
        outBytes[outOff] = (byte)word;
        outBytes[outOff + 1] = (byte)(word >>> 8);
        outBytes[outOff + 2] = (byte)(word >>> 16);
        outBytes[outOff + 3] = (byte)(word >>> 24);
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UnpackWord( H0, output, outOff );
        UnpackWord( H1, output, outOff + 4 );
        UnpackWord( H2, output, outOff + 8 );
        UnpackWord( H3, output, outOff + 12 );
        UnpackWord( H4, output, outOff + 16 );
        UnpackWord( H5, output, outOff + 20 );
        UnpackWord( H6, output, outOff + 24 );
        UnpackWord( H7, output, outOff + 28 );
        Reset();
        return 32;
    }

    public override void Reset()
    {
        base.Reset();
        H0 = 1732584193;
        H1 = -271733879;
        H2 = -1732584194;
        H3 = 271733878;
        H4 = 1985229328;
        H5 = -19088744;
        H6 = -1985229329;
        H7 = 19088743;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    private int RL( int x, int n ) => (x << n) | x >>> 32 - n;

    private int F1( int x, int y, int z ) => x ^ y ^ z;

    private int F2( int x, int y, int z ) => (x & y) | (~x & z);

    private int F3( int x, int y, int z ) => (x | ~y) ^ z;

    private int F4( int x, int y, int z ) => (x & z) | (y & ~z);

    private int F1( int a, int b, int c, int d, int x, int s ) => RL( a + F1( b, c, d ) + x, s );

    private int F2( int a, int b, int c, int d, int x, int s ) => RL( a + F2( b, c, d ) + x + 1518500249, s );

    private int F3( int a, int b, int c, int d, int x, int s ) => RL( a + F3( b, c, d ) + x + 1859775393, s );

    private int F4( int a, int b, int c, int d, int x, int s ) => RL( a + F4( b, c, d ) + x - 1894007588, s );

    private int FF1( int a, int b, int c, int d, int x, int s ) => RL( a + F1( b, c, d ) + x, s );

    private int FF2( int a, int b, int c, int d, int x, int s ) => RL( a + F2( b, c, d ) + x + 1836072691, s );

    private int FF3( int a, int b, int c, int d, int x, int s ) => RL( a + F3( b, c, d ) + x + 1548603684, s );

    private int FF4( int a, int b, int c, int d, int x, int s ) => RL( a + F4( b, c, d ) + x + 1352829926, s );

    internal override void ProcessBlock()
    {
        int h0 = H0;
        int h1 = H1;
        int h2 = H2;
        int h3 = H3;
        int h4 = H4;
        int h5 = H5;
        int h6 = H6;
        int h7 = H7;
        int num1 = F1( h0, h1, h2, h3, X[0], 11 );
        int num2 = F1( h3, num1, h1, h2, X[1], 14 );
        int num3 = F1( h2, num2, num1, h1, X[2], 15 );
        int num4 = F1( h1, num3, num2, num1, X[3], 12 );
        int num5 = F1( num1, num4, num3, num2, X[4], 5 );
        int num6 = F1( num2, num5, num4, num3, X[5], 8 );
        int num7 = F1( num3, num6, num5, num4, X[6], 7 );
        int num8 = F1( num4, num7, num6, num5, X[7], 9 );
        int num9 = F1( num5, num8, num7, num6, X[8], 11 );
        int num10 = F1( num6, num9, num8, num7, X[9], 13 );
        int num11 = F1( num7, num10, num9, num8, X[10], 14 );
        int num12 = F1( num8, num11, num10, num9, X[11], 15 );
        int num13 = F1( num9, num12, num11, num10, X[12], 6 );
        int num14 = F1( num10, num13, num12, num11, X[13], 7 );
        int num15 = F1( num11, num14, num13, num12, X[14], 9 );
        int num16 = F1( num12, num15, num14, num13, X[15], 8 );
        int num17 = FF4( h4, h5, h6, h7, X[5], 8 );
        int num18 = FF4( h7, num17, h5, h6, X[14], 9 );
        int num19 = FF4( h6, num18, num17, h5, X[7], 9 );
        int num20 = FF4( h5, num19, num18, num17, X[0], 11 );
        int num21 = FF4( num17, num20, num19, num18, X[9], 13 );
        int num22 = FF4( num18, num21, num20, num19, X[2], 15 );
        int num23 = FF4( num19, num22, num21, num20, X[11], 15 );
        int num24 = FF4( num20, num23, num22, num21, X[4], 5 );
        int num25 = FF4( num21, num24, num23, num22, X[13], 7 );
        int num26 = FF4( num22, num25, num24, num23, X[6], 7 );
        int num27 = FF4( num23, num26, num25, num24, X[15], 8 );
        int num28 = FF4( num24, num27, num26, num25, X[8], 11 );
        int num29 = FF4( num25, num28, num27, num26, X[1], 14 );
        int num30 = FF4( num26, num29, num28, num27, X[10], 14 );
        int num31 = FF4( num27, num30, num29, num28, X[3], 12 );
        int num32 = FF4( num28, num31, num30, num29, X[12], 6 );
        int num33 = num13;
        int a1 = num29;
        int a2 = num33;
        int num34 = F2( a1, num16, num15, num14, X[7], 7 );
        int num35 = F2( num14, num34, num16, num15, X[4], 6 );
        int num36 = F2( num15, num35, num34, num16, X[13], 8 );
        int num37 = F2( num16, num36, num35, num34, X[1], 13 );
        int num38 = F2( num34, num37, num36, num35, X[10], 11 );
        int num39 = F2( num35, num38, num37, num36, X[6], 9 );
        int num40 = F2( num36, num39, num38, num37, X[15], 7 );
        int num41 = F2( num37, num40, num39, num38, X[3], 15 );
        int num42 = F2( num38, num41, num40, num39, X[12], 7 );
        int num43 = F2( num39, num42, num41, num40, X[0], 12 );
        int num44 = F2( num40, num43, num42, num41, X[9], 15 );
        int num45 = F2( num41, num44, num43, num42, X[5], 9 );
        int num46 = F2( num42, num45, num44, num43, X[2], 11 );
        int num47 = F2( num43, num46, num45, num44, X[14], 7 );
        int num48 = F2( num44, num47, num46, num45, X[11], 13 );
        int num49 = F2( num45, num48, num47, num46, X[8], 12 );
        int num50 = FF3( a2, num32, num31, num30, X[6], 9 );
        int num51 = FF3( num30, num50, num32, num31, X[11], 13 );
        int num52 = FF3( num31, num51, num50, num32, X[3], 15 );
        int num53 = FF3( num32, num52, num51, num50, X[7], 7 );
        int num54 = FF3( num50, num53, num52, num51, X[0], 12 );
        int num55 = FF3( num51, num54, num53, num52, X[13], 8 );
        int num56 = FF3( num52, num55, num54, num53, X[5], 9 );
        int num57 = FF3( num53, num56, num55, num54, X[10], 11 );
        int num58 = FF3( num54, num57, num56, num55, X[14], 7 );
        int num59 = FF3( num55, num58, num57, num56, X[15], 7 );
        int num60 = FF3( num56, num59, num58, num57, X[8], 12 );
        int num61 = FF3( num57, num60, num59, num58, X[12], 7 );
        int num62 = FF3( num58, num61, num60, num59, X[4], 6 );
        int num63 = FF3( num59, num62, num61, num60, X[9], 15 );
        int num64 = FF3( num60, num63, num62, num61, X[1], 13 );
        int num65 = FF3( num61, num64, num63, num62, X[2], 11 );
        int num66 = num49;
        int num67 = num65;
        int num68 = num66;
        int num69 = F3( num46, num67, num48, num47, X[3], 11 );
        int num70 = F3( num47, num69, num67, num48, X[10], 13 );
        int num71 = F3( num48, num70, num69, num67, X[14], 6 );
        int num72 = F3( num67, num71, num70, num69, X[4], 7 );
        int num73 = F3( num69, num72, num71, num70, X[9], 14 );
        int num74 = F3( num70, num73, num72, num71, X[15], 9 );
        int num75 = F3( num71, num74, num73, num72, X[8], 13 );
        int num76 = F3( num72, num75, num74, num73, X[1], 15 );
        int num77 = F3( num73, num76, num75, num74, X[2], 14 );
        int num78 = F3( num74, num77, num76, num75, X[7], 8 );
        int num79 = F3( num75, num78, num77, num76, X[0], 13 );
        int num80 = F3( num76, num79, num78, num77, X[6], 6 );
        int num81 = F3( num77, num80, num79, num78, X[13], 5 );
        int num82 = F3( num78, num81, num80, num79, X[11], 12 );
        int b1 = F3( num79, num82, num81, num80, X[5], 7 );
        int num83 = F3( num80, b1, num82, num81, X[12], 5 );
        int num84 = FF2( num62, num68, num64, num63, X[15], 9 );
        int num85 = FF2( num63, num84, num68, num64, X[5], 7 );
        int num86 = FF2( num64, num85, num84, num68, X[1], 15 );
        int num87 = FF2( num68, num86, num85, num84, X[3], 11 );
        int num88 = FF2( num84, num87, num86, num85, X[7], 8 );
        int num89 = FF2( num85, num88, num87, num86, X[14], 6 );
        int num90 = FF2( num86, num89, num88, num87, X[6], 6 );
        int num91 = FF2( num87, num90, num89, num88, X[9], 14 );
        int num92 = FF2( num88, num91, num90, num89, X[11], 12 );
        int num93 = FF2( num89, num92, num91, num90, X[8], 13 );
        int num94 = FF2( num90, num93, num92, num91, X[12], 5 );
        int num95 = FF2( num91, num94, num93, num92, X[2], 14 );
        int num96 = FF2( num92, num95, num94, num93, X[10], 13 );
        int num97 = FF2( num93, num96, num95, num94, X[0], 13 );
        int b2 = FF2( num94, num97, num96, num95, X[4], 7 );
        int num98 = FF2( num95, b2, num97, num96, X[13], 5 );
        int num99 = b1;
        int num100 = b2;
        int num101 = num99;
        int num102 = F4( num81, num83, num100, num82, X[1], 11 );
        int num103 = F4( num82, num102, num83, num100, X[9], 12 );
        int num104 = F4( num100, num103, num102, num83, X[11], 14 );
        int num105 = F4( num83, num104, num103, num102, X[10], 15 );
        int num106 = F4( num102, num105, num104, num103, X[0], 14 );
        int num107 = F4( num103, num106, num105, num104, X[8], 15 );
        int num108 = F4( num104, num107, num106, num105, X[12], 9 );
        int num109 = F4( num105, num108, num107, num106, X[4], 8 );
        int num110 = F4( num106, num109, num108, num107, X[13], 9 );
        int num111 = F4( num107, num110, num109, num108, X[3], 14 );
        int num112 = F4( num108, num111, num110, num109, X[7], 5 );
        int num113 = F4( num109, num112, num111, num110, X[15], 6 );
        int num114 = F4( num110, num113, num112, num111, X[14], 8 );
        int num115 = F4( num111, num114, num113, num112, X[5], 6 );
        int b3 = F4( num112, num115, num114, num113, X[6], 5 );
        int num116 = F4( num113, b3, num115, num114, X[2], 12 );
        int num117 = FF1( num96, num98, num101, num97, X[8], 15 );
        int num118 = FF1( num97, num117, num98, num101, X[6], 5 );
        int num119 = FF1( num101, num118, num117, num98, X[4], 8 );
        int num120 = FF1( num98, num119, num118, num117, X[1], 11 );
        int num121 = FF1( num117, num120, num119, num118, X[3], 14 );
        int num122 = FF1( num118, num121, num120, num119, X[11], 14 );
        int num123 = FF1( num119, num122, num121, num120, X[15], 6 );
        int num124 = FF1( num120, num123, num122, num121, X[0], 14 );
        int num125 = FF1( num121, num124, num123, num122, X[5], 6 );
        int num126 = FF1( num122, num125, num124, num123, X[12], 9 );
        int num127 = FF1( num123, num126, num125, num124, X[2], 12 );
        int num128 = FF1( num124, num127, num126, num125, X[13], 9 );
        int num129 = FF1( num125, num128, num127, num126, X[9], 12 );
        int num130 = FF1( num126, num129, num128, num127, X[7], 5 );
        int b4 = FF1( num127, num130, num129, num128, X[10], 15 );
        int num131 = FF1( num128, b4, num130, num129, X[14], 8 );
        int num132 = num115;
        int num133 = num130;
        int num134 = num132;
        H0 += num114;
        H1 += num116;
        H2 += b3;
        H3 += num133;
        H4 += num129;
        H5 += num131;
        H6 += b4;
        H7 += num134;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    public override IMemoable Copy() => new RipeMD256Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (RipeMD256Digest)other );
}
public class RipeMD320Digest : GeneralDigest
{
    private const int DigestLength = 40;
    private int H0;
    private int H1;
    private int H2;
    private int H3;
    private int H4;
    private int H5;
    private int H6;
    private int H7;
    private int H8;
    private int H9;
    private int[] X = new int[16];
    private int xOff;

    public override string AlgorithmName => "RIPEMD320";

    public override int GetDigestSize() => 40;

    public RipeMD320Digest() => Reset();

    public RipeMD320Digest( RipeMD320Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( RipeMD320Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H0 = t.H0;
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        H5 = t.H5;
        H6 = t.H6;
        H7 = t.H7;
        H8 = t.H8;
        H9 = t.H9;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff++] = (input[inOff] & byte.MaxValue) | ((input[inOff + 1] & byte.MaxValue) << 8) | ((input[inOff + 2] & byte.MaxValue) << 16) | ((input[inOff + 3] & byte.MaxValue) << 24);
        if (xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (int)(bitLength & uint.MaxValue);
        X[15] = (int)(bitLength >>> 32);
    }

    private void UnpackWord( int word, byte[] outBytes, int outOff )
    {
        outBytes[outOff] = (byte)word;
        outBytes[outOff + 1] = (byte)(word >>> 8);
        outBytes[outOff + 2] = (byte)(word >>> 16);
        outBytes[outOff + 3] = (byte)(word >>> 24);
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UnpackWord( H0, output, outOff );
        UnpackWord( H1, output, outOff + 4 );
        UnpackWord( H2, output, outOff + 8 );
        UnpackWord( H3, output, outOff + 12 );
        UnpackWord( H4, output, outOff + 16 );
        UnpackWord( H5, output, outOff + 20 );
        UnpackWord( H6, output, outOff + 24 );
        UnpackWord( H7, output, outOff + 28 );
        UnpackWord( H8, output, outOff + 32 );
        UnpackWord( H9, output, outOff + 36 );
        Reset();
        return 40;
    }

    public override void Reset()
    {
        base.Reset();
        H0 = 1732584193;
        H1 = -271733879;
        H2 = -1732584194;
        H3 = 271733878;
        H4 = -1009589776;
        H5 = 1985229328;
        H6 = -19088744;
        H7 = -1985229329;
        H8 = 19088743;
        H9 = 1009589775;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    private int RL( int x, int n ) => (x << n) | x >>> 32 - n;

    private int F1( int x, int y, int z ) => x ^ y ^ z;

    private int F2( int x, int y, int z ) => (x & y) | (~x & z);

    private int F3( int x, int y, int z ) => (x | ~y) ^ z;

    private int F4( int x, int y, int z ) => (x & z) | (y & ~z);

    private int F5( int x, int y, int z ) => x ^ (y | ~z);

    internal override void ProcessBlock()
    {
        int h0 = H0;
        int h1 = H1;
        int h2 = H2;
        int h3 = H3;
        int h4 = H4;
        int h5 = H5;
        int h6 = H6;
        int h7 = H7;
        int h8 = H8;
        int h9 = H9;
        int num1 = RL( h0 + F1( h1, h2, h3 ) + X[0], 11 ) + h4;
        int z1 = RL( h2, 10 );
        int num2 = RL( h4 + F1( num1, h1, z1 ) + X[1], 14 ) + h3;
        int z2 = RL( h1, 10 );
        int num3 = RL( h3 + F1( num2, num1, z2 ) + X[2], 15 ) + z1;
        int z3 = RL( num1, 10 );
        int num4 = RL( z1 + F1( num3, num2, z3 ) + X[3], 12 ) + z2;
        int z4 = RL( num2, 10 );
        int num5 = RL( z2 + F1( num4, num3, z4 ) + X[4], 5 ) + z3;
        int z5 = RL( num3, 10 );
        int num6 = RL( z3 + F1( num5, num4, z5 ) + X[5], 8 ) + z4;
        int z6 = RL( num4, 10 );
        int num7 = RL( z4 + F1( num6, num5, z6 ) + X[6], 7 ) + z5;
        int z7 = RL( num5, 10 );
        int num8 = RL( z5 + F1( num7, num6, z7 ) + X[7], 9 ) + z6;
        int z8 = RL( num6, 10 );
        int num9 = RL( z6 + F1( num8, num7, z8 ) + X[8], 11 ) + z7;
        int z9 = RL( num7, 10 );
        int num10 = RL( z7 + F1( num9, num8, z9 ) + X[9], 13 ) + z8;
        int z10 = RL( num8, 10 );
        int num11 = RL( z8 + F1( num10, num9, z10 ) + X[10], 14 ) + z9;
        int z11 = RL( num9, 10 );
        int num12 = RL( z9 + F1( num11, num10, z11 ) + X[11], 15 ) + z10;
        int z12 = RL( num10, 10 );
        int num13 = RL( z10 + F1( num12, num11, z12 ) + X[12], 6 ) + z11;
        int z13 = RL( num11, 10 );
        int num14 = RL( z11 + F1( num13, num12, z13 ) + X[13], 7 ) + z12;
        int z14 = RL( num12, 10 );
        int num15 = RL( z12 + F1( num14, num13, z14 ) + X[14], 9 ) + z13;
        int z15 = RL( num13, 10 );
        int num16 = RL( z13 + F1( num15, num14, z15 ) + X[15], 8 ) + z14;
        int z16 = RL( num14, 10 );
        int num17 = RL( h5 + F5( h6, h7, h8 ) + X[5] + 1352829926, 8 ) + h9;
        int z17 = RL( h7, 10 );
        int num18 = RL( h9 + F5( num17, h6, z17 ) + X[14] + 1352829926, 9 ) + h8;
        int z18 = RL( h6, 10 );
        int num19 = RL( h8 + F5( num18, num17, z18 ) + X[7] + 1352829926, 9 ) + z17;
        int z19 = RL( num17, 10 );
        int num20 = RL( z17 + F5( num19, num18, z19 ) + X[0] + 1352829926, 11 ) + z18;
        int z20 = RL( num18, 10 );
        int num21 = RL( z18 + F5( num20, num19, z20 ) + X[9] + 1352829926, 13 ) + z19;
        int z21 = RL( num19, 10 );
        int num22 = RL( z19 + F5( num21, num20, z21 ) + X[2] + 1352829926, 15 ) + z20;
        int z22 = RL( num20, 10 );
        int num23 = RL( z20 + F5( num22, num21, z22 ) + X[11] + 1352829926, 15 ) + z21;
        int z23 = RL( num21, 10 );
        int num24 = RL( z21 + F5( num23, num22, z23 ) + X[4] + 1352829926, 5 ) + z22;
        int z24 = RL( num22, 10 );
        int num25 = RL( z22 + F5( num24, num23, z24 ) + X[13] + 1352829926, 7 ) + z23;
        int z25 = RL( num23, 10 );
        int num26 = RL( z23 + F5( num25, num24, z25 ) + X[6] + 1352829926, 7 ) + z24;
        int z26 = RL( num24, 10 );
        int num27 = RL( z24 + F5( num26, num25, z26 ) + X[15] + 1352829926, 8 ) + z25;
        int z27 = RL( num25, 10 );
        int num28 = RL( z25 + F5( num27, num26, z27 ) + X[8] + 1352829926, 11 ) + z26;
        int z28 = RL( num26, 10 );
        int num29 = RL( z26 + F5( num28, num27, z28 ) + X[1] + 1352829926, 14 ) + z27;
        int z29 = RL( num27, 10 );
        int num30 = RL( z27 + F5( num29, num28, z29 ) + X[10] + 1352829926, 14 ) + z28;
        int z30 = RL( num28, 10 );
        int num31 = RL( z28 + F5( num30, num29, z30 ) + X[3] + 1352829926, 12 ) + z29;
        int z31 = RL( num29, 10 );
        int num32 = RL( z29 + F5( num31, num30, z31 ) + X[12] + 1352829926, 6 ) + z30;
        int z32 = RL( num30, 10 );
        int num33 = num16;
        int num34 = num32;
        int num35 = num33;
        int num36 = RL( z14 + F2( num34, num15, z16 ) + X[7] + 1518500249, 7 ) + z15;
        int z33 = RL( num15, 10 );
        int num37 = RL( z15 + F2( num36, num34, z33 ) + X[4] + 1518500249, 6 ) + z16;
        int z34 = RL( num34, 10 );
        int num38 = RL( z16 + F2( num37, num36, z34 ) + X[13] + 1518500249, 8 ) + z33;
        int z35 = RL( num36, 10 );
        int num39 = RL( z33 + F2( num38, num37, z35 ) + X[1] + 1518500249, 13 ) + z34;
        int z36 = RL( num37, 10 );
        int num40 = RL( z34 + F2( num39, num38, z36 ) + X[10] + 1518500249, 11 ) + z35;
        int z37 = RL( num38, 10 );
        int num41 = RL( z35 + F2( num40, num39, z37 ) + X[6] + 1518500249, 9 ) + z36;
        int z38 = RL( num39, 10 );
        int num42 = RL( z36 + F2( num41, num40, z38 ) + X[15] + 1518500249, 7 ) + z37;
        int z39 = RL( num40, 10 );
        int num43 = RL( z37 + F2( num42, num41, z39 ) + X[3] + 1518500249, 15 ) + z38;
        int z40 = RL( num41, 10 );
        int num44 = RL( z38 + F2( num43, num42, z40 ) + X[12] + 1518500249, 7 ) + z39;
        int z41 = RL( num42, 10 );
        int num45 = RL( z39 + F2( num44, num43, z41 ) + X[0] + 1518500249, 12 ) + z40;
        int z42 = RL( num43, 10 );
        int num46 = RL( z40 + F2( num45, num44, z42 ) + X[9] + 1518500249, 15 ) + z41;
        int z43 = RL( num44, 10 );
        int num47 = RL( z41 + F2( num46, num45, z43 ) + X[5] + 1518500249, 9 ) + z42;
        int z44 = RL( num45, 10 );
        int num48 = RL( z42 + F2( num47, num46, z44 ) + X[2] + 1518500249, 11 ) + z43;
        int z45 = RL( num46, 10 );
        int num49 = RL( z43 + F2( num48, num47, z45 ) + X[14] + 1518500249, 7 ) + z44;
        int z46 = RL( num47, 10 );
        int num50 = RL( z44 + F2( num49, num48, z46 ) + X[11] + 1518500249, 13 ) + z45;
        int z47 = RL( num48, 10 );
        int num51 = RL( z45 + F2( num50, num49, z47 ) + X[8] + 1518500249, 12 ) + z46;
        int num52 = RL( num49, 10 );
        int num53 = RL( z30 + F4( num35, num31, z32 ) + X[6] + 1548603684, 9 ) + z31;
        int z48 = RL( num31, 10 );
        int num54 = RL( z31 + F4( num53, num35, z48 ) + X[11] + 1548603684, 13 ) + z32;
        int z49 = RL( num35, 10 );
        int num55 = RL( z32 + F4( num54, num53, z49 ) + X[3] + 1548603684, 15 ) + z48;
        int z50 = RL( num53, 10 );
        int num56 = RL( z48 + F4( num55, num54, z50 ) + X[7] + 1548603684, 7 ) + z49;
        int z51 = RL( num54, 10 );
        int num57 = RL( z49 + F4( num56, num55, z51 ) + X[0] + 1548603684, 12 ) + z50;
        int z52 = RL( num55, 10 );
        int num58 = RL( z50 + F4( num57, num56, z52 ) + X[13] + 1548603684, 8 ) + z51;
        int z53 = RL( num56, 10 );
        int num59 = RL( z51 + F4( num58, num57, z53 ) + X[5] + 1548603684, 9 ) + z52;
        int z54 = RL( num57, 10 );
        int num60 = RL( z52 + F4( num59, num58, z54 ) + X[10] + 1548603684, 11 ) + z53;
        int z55 = RL( num58, 10 );
        int num61 = RL( z53 + F4( num60, num59, z55 ) + X[14] + 1548603684, 7 ) + z54;
        int z56 = RL( num59, 10 );
        int num62 = RL( z54 + F4( num61, num60, z56 ) + X[15] + 1548603684, 7 ) + z55;
        int z57 = RL( num60, 10 );
        int num63 = RL( z55 + F4( num62, num61, z57 ) + X[8] + 1548603684, 12 ) + z56;
        int z58 = RL( num61, 10 );
        int num64 = RL( z56 + F4( num63, num62, z58 ) + X[12] + 1548603684, 7 ) + z57;
        int z59 = RL( num62, 10 );
        int num65 = RL( z57 + F4( num64, num63, z59 ) + X[4] + 1548603684, 6 ) + z58;
        int z60 = RL( num63, 10 );
        int num66 = RL( z58 + F4( num65, num64, z60 ) + X[9] + 1548603684, 15 ) + z59;
        int z61 = RL( num64, 10 );
        int num67 = RL( z59 + F4( num66, num65, z61 ) + X[1] + 1548603684, 13 ) + z60;
        int z62 = RL( num65, 10 );
        int num68 = RL( z60 + F4( num67, num66, z62 ) + X[2] + 1548603684, 11 ) + z61;
        int num69 = RL( num66, 10 );
        int num70 = num52;
        int z63 = num69;
        int z64 = num70;
        int num71 = RL( z46 + F3( num51, num50, z63 ) + X[3] + 1859775393, 11 ) + z47;
        int z65 = RL( num50, 10 );
        int num72 = RL( z47 + F3( num71, num51, z65 ) + X[10] + 1859775393, 13 ) + z63;
        int z66 = RL( num51, 10 );
        int num73 = RL( z63 + F3( num72, num71, z66 ) + X[14] + 1859775393, 6 ) + z65;
        int z67 = RL( num71, 10 );
        int num74 = RL( z65 + F3( num73, num72, z67 ) + X[4] + 1859775393, 7 ) + z66;
        int z68 = RL( num72, 10 );
        int num75 = RL( z66 + F3( num74, num73, z68 ) + X[9] + 1859775393, 14 ) + z67;
        int z69 = RL( num73, 10 );
        int num76 = RL( z67 + F3( num75, num74, z69 ) + X[15] + 1859775393, 9 ) + z68;
        int z70 = RL( num74, 10 );
        int num77 = RL( z68 + F3( num76, num75, z70 ) + X[8] + 1859775393, 13 ) + z69;
        int z71 = RL( num75, 10 );
        int num78 = RL( z69 + F3( num77, num76, z71 ) + X[1] + 1859775393, 15 ) + z70;
        int z72 = RL( num76, 10 );
        int num79 = RL( z70 + F3( num78, num77, z72 ) + X[2] + 1859775393, 14 ) + z71;
        int z73 = RL( num77, 10 );
        int num80 = RL( z71 + F3( num79, num78, z73 ) + X[7] + 1859775393, 8 ) + z72;
        int z74 = RL( num78, 10 );
        int num81 = RL( z72 + F3( num80, num79, z74 ) + X[0] + 1859775393, 13 ) + z73;
        int z75 = RL( num79, 10 );
        int num82 = RL( z73 + F3( num81, num80, z75 ) + X[6] + 1859775393, 6 ) + z74;
        int z76 = RL( num80, 10 );
        int num83 = RL( z74 + F3( num82, num81, z76 ) + X[13] + 1859775393, 5 ) + z75;
        int z77 = RL( num81, 10 );
        int num84 = RL( z75 + F3( num83, num82, z77 ) + X[11] + 1859775393, 12 ) + z76;
        int z78 = RL( num82, 10 );
        int num85 = RL( z76 + F3( num84, num83, z78 ) + X[5] + 1859775393, 7 ) + z77;
        int z79 = RL( num83, 10 );
        int num86 = RL( z77 + F3( num85, num84, z79 ) + X[12] + 1859775393, 5 ) + z78;
        int z80 = RL( num84, 10 );
        int num87 = RL( z61 + F3( num68, num67, z64 ) + X[15] + 1836072691, 9 ) + z62;
        int z81 = RL( num67, 10 );
        int num88 = RL( z62 + F3( num87, num68, z81 ) + X[5] + 1836072691, 7 ) + z64;
        int z82 = RL( num68, 10 );
        int num89 = RL( z64 + F3( num88, num87, z82 ) + X[1] + 1836072691, 15 ) + z81;
        int z83 = RL( num87, 10 );
        int num90 = RL( z81 + F3( num89, num88, z83 ) + X[3] + 1836072691, 11 ) + z82;
        int z84 = RL( num88, 10 );
        int num91 = RL( z82 + F3( num90, num89, z84 ) + X[7] + 1836072691, 8 ) + z83;
        int z85 = RL( num89, 10 );
        int num92 = RL( z83 + F3( num91, num90, z85 ) + X[14] + 1836072691, 6 ) + z84;
        int z86 = RL( num90, 10 );
        int num93 = RL( z84 + F3( num92, num91, z86 ) + X[6] + 1836072691, 6 ) + z85;
        int z87 = RL( num91, 10 );
        int num94 = RL( z85 + F3( num93, num92, z87 ) + X[9] + 1836072691, 14 ) + z86;
        int z88 = RL( num92, 10 );
        int num95 = RL( z86 + F3( num94, num93, z88 ) + X[11] + 1836072691, 12 ) + z87;
        int z89 = RL( num93, 10 );
        int num96 = RL( z87 + F3( num95, num94, z89 ) + X[8] + 1836072691, 13 ) + z88;
        int z90 = RL( num94, 10 );
        int num97 = RL( z88 + F3( num96, num95, z90 ) + X[12] + 1836072691, 5 ) + z89;
        int z91 = RL( num95, 10 );
        int num98 = RL( z89 + F3( num97, num96, z91 ) + X[2] + 1836072691, 14 ) + z90;
        int z92 = RL( num96, 10 );
        int num99 = RL( z90 + F3( num98, num97, z92 ) + X[10] + 1836072691, 13 ) + z91;
        int z93 = RL( num97, 10 );
        int num100 = RL( z91 + F3( num99, num98, z93 ) + X[0] + 1836072691, 13 ) + z92;
        int z94 = RL( num98, 10 );
        int num101 = RL( z92 + F3( num100, num99, z94 ) + X[4] + 1836072691, 7 ) + z93;
        int z95 = RL( num99, 10 );
        int num102 = RL( z93 + F3( num101, num100, z95 ) + X[13] + 1836072691, 5 ) + z94;
        int z96 = RL( num100, 10 );
        int num103 = z78;
        int num104 = z94;
        int num105 = num103;
        int num106 = RL( num104 + F4( num86, num85, z80 ) + X[1] - 1894007588, 11 ) + z79;
        int z97 = RL( num85, 10 );
        int num107 = RL( z79 + F4( num106, num86, z97 ) + X[9] - 1894007588, 12 ) + z80;
        int z98 = RL( num86, 10 );
        int num108 = RL( z80 + F4( num107, num106, z98 ) + X[11] - 1894007588, 14 ) + z97;
        int z99 = RL( num106, 10 );
        int num109 = RL( z97 + F4( num108, num107, z99 ) + X[10] - 1894007588, 15 ) + z98;
        int z100 = RL( num107, 10 );
        int num110 = RL( z98 + F4( num109, num108, z100 ) + X[0] - 1894007588, 14 ) + z99;
        int z101 = RL( num108, 10 );
        int num111 = RL( z99 + F4( num110, num109, z101 ) + X[8] - 1894007588, 15 ) + z100;
        int z102 = RL( num109, 10 );
        int num112 = RL( z100 + F4( num111, num110, z102 ) + X[12] - 1894007588, 9 ) + z101;
        int z103 = RL( num110, 10 );
        int num113 = RL( z101 + F4( num112, num111, z103 ) + X[4] - 1894007588, 8 ) + z102;
        int z104 = RL( num111, 10 );
        int num114 = RL( z102 + F4( num113, num112, z104 ) + X[13] - 1894007588, 9 ) + z103;
        int z105 = RL( num112, 10 );
        int num115 = RL( z103 + F4( num114, num113, z105 ) + X[3] - 1894007588, 14 ) + z104;
        int z106 = RL( num113, 10 );
        int num116 = RL( z104 + F4( num115, num114, z106 ) + X[7] - 1894007588, 5 ) + z105;
        int z107 = RL( num114, 10 );
        int num117 = RL( z105 + F4( num116, num115, z107 ) + X[15] - 1894007588, 6 ) + z106;
        int z108 = RL( num115, 10 );
        int num118 = RL( z106 + F4( num117, num116, z108 ) + X[14] - 1894007588, 8 ) + z107;
        int z109 = RL( num116, 10 );
        int num119 = RL( z107 + F4( num118, num117, z109 ) + X[5] - 1894007588, 6 ) + z108;
        int z110 = RL( num117, 10 );
        int x1 = RL( z108 + F4( num119, num118, z110 ) + X[6] - 1894007588, 5 ) + z109;
        int z111 = RL( num118, 10 );
        int num120 = RL( z109 + F4( x1, num119, z111 ) + X[2] - 1894007588, 12 ) + z110;
        int z112 = RL( num119, 10 );
        int num121 = RL( num105 + F2( num102, num101, z96 ) + X[8] + 2053994217, 15 ) + z95;
        int z113 = RL( num101, 10 );
        int num122 = RL( z95 + F2( num121, num102, z113 ) + X[6] + 2053994217, 5 ) + z96;
        int z114 = RL( num102, 10 );
        int num123 = RL( z96 + F2( num122, num121, z114 ) + X[4] + 2053994217, 8 ) + z113;
        int z115 = RL( num121, 10 );
        int num124 = RL( z113 + F2( num123, num122, z115 ) + X[1] + 2053994217, 11 ) + z114;
        int z116 = RL( num122, 10 );
        int num125 = RL( z114 + F2( num124, num123, z116 ) + X[3] + 2053994217, 14 ) + z115;
        int z117 = RL( num123, 10 );
        int num126 = RL( z115 + F2( num125, num124, z117 ) + X[11] + 2053994217, 14 ) + z116;
        int z118 = RL( num124, 10 );
        int num127 = RL( z116 + F2( num126, num125, z118 ) + X[15] + 2053994217, 6 ) + z117;
        int z119 = RL( num125, 10 );
        int num128 = RL( z117 + F2( num127, num126, z119 ) + X[0] + 2053994217, 14 ) + z118;
        int z120 = RL( num126, 10 );
        int num129 = RL( z118 + F2( num128, num127, z120 ) + X[5] + 2053994217, 6 ) + z119;
        int z121 = RL( num127, 10 );
        int num130 = RL( z119 + F2( num129, num128, z121 ) + X[12] + 2053994217, 9 ) + z120;
        int z122 = RL( num128, 10 );
        int num131 = RL( z120 + F2( num130, num129, z122 ) + X[2] + 2053994217, 12 ) + z121;
        int z123 = RL( num129, 10 );
        int num132 = RL( z121 + F2( num131, num130, z123 ) + X[13] + 2053994217, 9 ) + z122;
        int z124 = RL( num130, 10 );
        int num133 = RL( z122 + F2( num132, num131, z124 ) + X[9] + 2053994217, 12 ) + z123;
        int z125 = RL( num131, 10 );
        int num134 = RL( z123 + F2( num133, num132, z125 ) + X[7] + 2053994217, 5 ) + z124;
        int z126 = RL( num132, 10 );
        int x2 = RL( z124 + F2( num134, num133, z126 ) + X[10] + 2053994217, 15 ) + z125;
        int z127 = RL( num133, 10 );
        int num135 = RL( z125 + F2( x2, num134, z127 ) + X[14] + 2053994217, 8 ) + z126;
        int z128 = RL( num134, 10 );
        int num136 = x1;
        int num137 = x2;
        int num138 = num136;
        int num139 = RL( z110 + F5( num120, num137, z112 ) + X[4] - 1454113458, 9 ) + z111;
        int z129 = RL( num137, 10 );
        int num140 = RL( z111 + F5( num139, num120, z129 ) + X[0] - 1454113458, 15 ) + z112;
        int z130 = RL( num120, 10 );
        int num141 = RL( z112 + F5( num140, num139, z130 ) + X[5] - 1454113458, 5 ) + z129;
        int z131 = RL( num139, 10 );
        int num142 = RL( z129 + F5( num141, num140, z131 ) + X[9] - 1454113458, 11 ) + z130;
        int z132 = RL( num140, 10 );
        int num143 = RL( z130 + F5( num142, num141, z132 ) + X[7] - 1454113458, 6 ) + z131;
        int z133 = RL( num141, 10 );
        int num144 = RL( z131 + F5( num143, num142, z133 ) + X[12] - 1454113458, 8 ) + z132;
        int z134 = RL( num142, 10 );
        int num145 = RL( z132 + F5( num144, num143, z134 ) + X[2] - 1454113458, 13 ) + z133;
        int z135 = RL( num143, 10 );
        int num146 = RL( z133 + F5( num145, num144, z135 ) + X[10] - 1454113458, 12 ) + z134;
        int z136 = RL( num144, 10 );
        int num147 = RL( z134 + F5( num146, num145, z136 ) + X[14] - 1454113458, 5 ) + z135;
        int z137 = RL( num145, 10 );
        int num148 = RL( z135 + F5( num147, num146, z137 ) + X[1] - 1454113458, 12 ) + z136;
        int z138 = RL( num146, 10 );
        int num149 = RL( z136 + F5( num148, num147, z138 ) + X[3] - 1454113458, 13 ) + z137;
        int z139 = RL( num147, 10 );
        int num150 = RL( z137 + F5( num149, num148, z139 ) + X[8] - 1454113458, 14 ) + z138;
        int z140 = RL( num148, 10 );
        int num151 = RL( z138 + F5( num150, num149, z140 ) + X[11] - 1454113458, 11 ) + z139;
        int z141 = RL( num149, 10 );
        int num152 = RL( z139 + F5( num151, num150, z141 ) + X[6] - 1454113458, 8 ) + z140;
        int z142 = RL( num150, 10 );
        int x3 = RL( z140 + F5( num152, num151, z142 ) + X[15] - 1454113458, 5 ) + z141;
        int z143 = RL( num151, 10 );
        int num153 = RL( z141 + F5( x3, num152, z143 ) + X[13] - 1454113458, 6 ) + z142;
        int num154 = RL( num152, 10 );
        int num155 = RL( z126 + F1( num135, num138, z128 ) + X[12], 8 ) + z127;
        int z144 = RL( num138, 10 );
        int num156 = RL( z127 + F1( num155, num135, z144 ) + X[15], 5 ) + z128;
        int z145 = RL( num135, 10 );
        int num157 = RL( z128 + F1( num156, num155, z145 ) + X[10], 12 ) + z144;
        int z146 = RL( num155, 10 );
        int num158 = RL( z144 + F1( num157, num156, z146 ) + X[4], 9 ) + z145;
        int z147 = RL( num156, 10 );
        int num159 = RL( z145 + F1( num158, num157, z147 ) + X[1], 12 ) + z146;
        int z148 = RL( num157, 10 );
        int num160 = RL( z146 + F1( num159, num158, z148 ) + X[5], 5 ) + z147;
        int z149 = RL( num158, 10 );
        int num161 = RL( z147 + F1( num160, num159, z149 ) + X[8], 14 ) + z148;
        int z150 = RL( num159, 10 );
        int num162 = RL( z148 + F1( num161, num160, z150 ) + X[7], 6 ) + z149;
        int z151 = RL( num160, 10 );
        int num163 = RL( z149 + F1( num162, num161, z151 ) + X[6], 8 ) + z150;
        int z152 = RL( num161, 10 );
        int num164 = RL( z150 + F1( num163, num162, z152 ) + X[2], 13 ) + z151;
        int z153 = RL( num162, 10 );
        int num165 = RL( z151 + F1( num164, num163, z153 ) + X[13], 6 ) + z152;
        int z154 = RL( num163, 10 );
        int num166 = RL( z152 + F1( num165, num164, z154 ) + X[14], 5 ) + z153;
        int z155 = RL( num164, 10 );
        int num167 = RL( z153 + F1( num166, num165, z155 ) + X[0], 15 ) + z154;
        int z156 = RL( num165, 10 );
        int num168 = RL( z154 + F1( num167, num166, z156 ) + X[3], 13 ) + z155;
        int z157 = RL( num166, 10 );
        int x4 = RL( z155 + F1( num168, num167, z157 ) + X[9], 11 ) + z156;
        int z158 = RL( num167, 10 );
        int num169 = RL( z156 + F1( x4, num168, z158 ) + X[11], 11 ) + z157;
        int num170 = RL( num168, 10 );
        H0 += z142;
        H1 += num153;
        H2 += x3;
        H3 += num154;
        H4 += z158;
        H5 += z157;
        H6 += num169;
        H7 += x4;
        H8 += num170;
        H9 += z143;
        xOff = 0;
        for (int index = 0; index != X.Length; ++index)
            X[index] = 0;
    }

    public override IMemoable Copy() => new RipeMD320Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (RipeMD320Digest)other );
}
public class Sha1Digest : GeneralDigest
{
    private const int DigestLength = 20;
    private const uint Y1 = 1518500249;
    private const uint Y2 = 1859775393;
    private const uint Y3 = 2400959708;
    private const uint Y4 = 3395469782;
    private uint H1;
    private uint H2;
    private uint H3;
    private uint H4;
    private uint H5;
    private uint[] X = new uint[80];
    private int xOff;

    public Sha1Digest() => Reset();

    public Sha1Digest( Sha1Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( Sha1Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        H5 = t.H5;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "SHA-1";

    public override int GetDigestSize() => 20;

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff] = Pack.BE_To_UInt32( input, inOff );
        if (++xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (uint)(bitLength >>> 32);
        X[15] = (uint)bitLength;
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        Pack.UInt32_To_BE( H1, output, outOff );
        Pack.UInt32_To_BE( H2, output, outOff + 4 );
        Pack.UInt32_To_BE( H3, output, outOff + 8 );
        Pack.UInt32_To_BE( H4, output, outOff + 12 );
        Pack.UInt32_To_BE( H5, output, outOff + 16 );
        Reset();
        return 20;
    }

    public override void Reset()
    {
        base.Reset();
        H1 = 1732584193U;
        H2 = 4023233417U;
        H3 = 2562383102U;
        H4 = 271733878U;
        H5 = 3285377520U;
        xOff = 0;
        Array.Clear( X, 0, X.Length );
    }

    private static uint F( uint u, uint v, uint w ) => (uint)(((int)u & (int)v) | (~(int)u & (int)w));

    private static uint H( uint u, uint v, uint w ) => u ^ v ^ w;

    private static uint G( uint u, uint v, uint w ) => (uint)(((int)u & (int)v) | ((int)u & (int)w) | ((int)v & (int)w));

    internal override void ProcessBlock()
    {
        for (int index = 16; index < 80; ++index)
        {
            uint num = X[index - 3] ^ X[index - 8] ^ X[index - 14] ^ X[index - 16];
            X[index] = (num << 1) | (num >> 31);
        }
        uint u1 = H1;
        uint u2 = H2;
        uint num1 = H3;
        uint num2 = H4;
        uint num3 = H5;
        int num4 = 0;
        for (int index1 = 0; index1 < 4; ++index1)
        {
            int num5 = (int)num3;
            int num6 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)F( u2, num1, num2 );
            uint[] x1 = X;
            int index2 = num4;
            int num7 = index2 + 1;
            int num8 = (int)x1[index2];
            int num9 = num6 + num8 + 1518500249;
            uint u3 = (uint)(num5 + num9);
            uint num10 = (u2 << 30) | (u2 >> 2);
            int num11 = (int)num2;
            int num12 = (((int)u3 << 5) | (int)(u3 >> 27)) + (int)F( u1, num10, num1 );
            uint[] x2 = X;
            int index3 = num7;
            int num13 = index3 + 1;
            int num14 = (int)x2[index3];
            int num15 = num12 + num14 + 1518500249;
            uint u4 = (uint)(num11 + num15);
            uint num16 = (u1 << 30) | (u1 >> 2);
            int num17 = (int)num1;
            int num18 = (((int)u4 << 5) | (int)(u4 >> 27)) + (int)F( u3, num16, num10 );
            uint[] x3 = X;
            int index4 = num13;
            int num19 = index4 + 1;
            int num20 = (int)x3[index4];
            int num21 = num18 + num20 + 1518500249;
            uint u5 = (uint)(num17 + num21);
            num3 = (u3 << 30) | (u3 >> 2);
            int num22 = (int)num10;
            int num23 = (((int)u5 << 5) | (int)(u5 >> 27)) + (int)F( u4, num3, num16 );
            uint[] x4 = X;
            int index5 = num19;
            int num24 = index5 + 1;
            int num25 = (int)x4[index5];
            int num26 = num23 + num25 + 1518500249;
            u2 = (uint)(num22 + num26);
            num2 = (u4 << 30) | (u4 >> 2);
            int num27 = (int)num16;
            int num28 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)F( u5, num2, num3 );
            uint[] x5 = X;
            int index6 = num24;
            num4 = index6 + 1;
            int num29 = (int)x5[index6];
            int num30 = num28 + num29 + 1518500249;
            u1 = (uint)(num27 + num30);
            num1 = (u5 << 30) | (u5 >> 2);
        }
        for (int index7 = 0; index7 < 4; ++index7)
        {
            int num31 = (int)num3;
            int num32 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)H( u2, num1, num2 );
            uint[] x6 = X;
            int index8 = num4;
            int num33 = index8 + 1;
            int num34 = (int)x6[index8];
            int num35 = num32 + num34 + 1859775393;
            uint u6 = (uint)(num31 + num35);
            uint num36 = (u2 << 30) | (u2 >> 2);
            int num37 = (int)num2;
            int num38 = (((int)u6 << 5) | (int)(u6 >> 27)) + (int)H( u1, num36, num1 );
            uint[] x7 = X;
            int index9 = num33;
            int num39 = index9 + 1;
            int num40 = (int)x7[index9];
            int num41 = num38 + num40 + 1859775393;
            uint u7 = (uint)(num37 + num41);
            uint num42 = (u1 << 30) | (u1 >> 2);
            int num43 = (int)num1;
            int num44 = (((int)u7 << 5) | (int)(u7 >> 27)) + (int)H( u6, num42, num36 );
            uint[] x8 = X;
            int index10 = num39;
            int num45 = index10 + 1;
            int num46 = (int)x8[index10];
            int num47 = num44 + num46 + 1859775393;
            uint u8 = (uint)(num43 + num47);
            num3 = (u6 << 30) | (u6 >> 2);
            int num48 = (int)num36;
            int num49 = (((int)u8 << 5) | (int)(u8 >> 27)) + (int)H( u7, num3, num42 );
            uint[] x9 = X;
            int index11 = num45;
            int num50 = index11 + 1;
            int num51 = (int)x9[index11];
            int num52 = num49 + num51 + 1859775393;
            u2 = (uint)(num48 + num52);
            num2 = (u7 << 30) | (u7 >> 2);
            int num53 = (int)num42;
            int num54 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)H( u8, num2, num3 );
            uint[] x10 = X;
            int index12 = num50;
            num4 = index12 + 1;
            int num55 = (int)x10[index12];
            int num56 = num54 + num55 + 1859775393;
            u1 = (uint)(num53 + num56);
            num1 = (u8 << 30) | (u8 >> 2);
        }
        for (int index13 = 0; index13 < 4; ++index13)
        {
            int num57 = (int)num3;
            int num58 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)G( u2, num1, num2 );
            uint[] x11 = X;
            int index14 = num4;
            int num59 = index14 + 1;
            int num60 = (int)x11[index14];
            int num61 = num58 + num60 - 1894007588;
            uint u9 = (uint)(num57 + num61);
            uint num62 = (u2 << 30) | (u2 >> 2);
            int num63 = (int)num2;
            int num64 = (((int)u9 << 5) | (int)(u9 >> 27)) + (int)G( u1, num62, num1 );
            uint[] x12 = X;
            int index15 = num59;
            int num65 = index15 + 1;
            int num66 = (int)x12[index15];
            int num67 = num64 + num66 - 1894007588;
            uint u10 = (uint)(num63 + num67);
            uint num68 = (u1 << 30) | (u1 >> 2);
            int num69 = (int)num1;
            int num70 = (((int)u10 << 5) | (int)(u10 >> 27)) + (int)G( u9, num68, num62 );
            uint[] x13 = X;
            int index16 = num65;
            int num71 = index16 + 1;
            int num72 = (int)x13[index16];
            int num73 = num70 + num72 - 1894007588;
            uint u11 = (uint)(num69 + num73);
            num3 = (u9 << 30) | (u9 >> 2);
            int num74 = (int)num62;
            int num75 = (((int)u11 << 5) | (int)(u11 >> 27)) + (int)G( u10, num3, num68 );
            uint[] x14 = X;
            int index17 = num71;
            int num76 = index17 + 1;
            int num77 = (int)x14[index17];
            int num78 = num75 + num77 - 1894007588;
            u2 = (uint)(num74 + num78);
            num2 = (u10 << 30) | (u10 >> 2);
            int num79 = (int)num68;
            int num80 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)G( u11, num2, num3 );
            uint[] x15 = X;
            int index18 = num76;
            num4 = index18 + 1;
            int num81 = (int)x15[index18];
            int num82 = num80 + num81 - 1894007588;
            u1 = (uint)(num79 + num82);
            num1 = (u11 << 30) | (u11 >> 2);
        }
        for (int index19 = 0; index19 < 4; ++index19)
        {
            int num83 = (int)num3;
            int num84 = (((int)u1 << 5) | (int)(u1 >> 27)) + (int)H( u2, num1, num2 );
            uint[] x16 = X;
            int index20 = num4;
            int num85 = index20 + 1;
            int num86 = (int)x16[index20];
            int num87 = num84 + num86 - 899497514;
            uint u12 = (uint)(num83 + num87);
            uint num88 = (u2 << 30) | (u2 >> 2);
            int num89 = (int)num2;
            int num90 = (((int)u12 << 5) | (int)(u12 >> 27)) + (int)H( u1, num88, num1 );
            uint[] x17 = X;
            int index21 = num85;
            int num91 = index21 + 1;
            int num92 = (int)x17[index21];
            int num93 = num90 + num92 - 899497514;
            uint u13 = (uint)(num89 + num93);
            uint num94 = (u1 << 30) | (u1 >> 2);
            int num95 = (int)num1;
            int num96 = (((int)u13 << 5) | (int)(u13 >> 27)) + (int)H( u12, num94, num88 );
            uint[] x18 = X;
            int index22 = num91;
            int num97 = index22 + 1;
            int num98 = (int)x18[index22];
            int num99 = num96 + num98 - 899497514;
            uint u14 = (uint)(num95 + num99);
            num3 = (u12 << 30) | (u12 >> 2);
            int num100 = (int)num88;
            int num101 = (((int)u14 << 5) | (int)(u14 >> 27)) + (int)H( u13, num3, num94 );
            uint[] x19 = X;
            int index23 = num97;
            int num102 = index23 + 1;
            int num103 = (int)x19[index23];
            int num104 = num101 + num103 - 899497514;
            u2 = (uint)(num100 + num104);
            num2 = (u13 << 30) | (u13 >> 2);
            int num105 = (int)num94;
            int num106 = (((int)u2 << 5) | (int)(u2 >> 27)) + (int)H( u14, num2, num3 );
            uint[] x20 = X;
            int index24 = num102;
            num4 = index24 + 1;
            int num107 = (int)x20[index24];
            int num108 = num106 + num107 - 899497514;
            u1 = (uint)(num105 + num108);
            num1 = (u14 << 30) | (u14 >> 2);
        }
        H1 += u1;
        H2 += u2;
        H3 += num1;
        H4 += num2;
        H5 += num3;
        xOff = 0;
        Array.Clear( X, 0, 16 );
    }

    public override IMemoable Copy() => new Sha1Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (Sha1Digest)other );
}
public class Sha224Digest : GeneralDigest
{
    private const int DigestLength = 28;
    private uint H1;
    private uint H2;
    private uint H3;
    private uint H4;
    private uint H5;
    private uint H6;
    private uint H7;
    private uint H8;
    private uint[] X = new uint[64];
    private int xOff;
    internal static readonly uint[] K = new uint[64]
    {
      1116352408U,
      1899447441U,
      3049323471U,
      3921009573U,
      961987163U,
      1508970993U,
      2453635748U,
      2870763221U,
      3624381080U,
      310598401U,
      607225278U,
      1426881987U,
      1925078388U,
      2162078206U,
      2614888103U,
      3248222580U,
      3835390401U,
      4022224774U,
      264347078U,
      604807628U,
      770255983U,
      1249150122U,
      1555081692U,
      1996064986U,
      2554220882U,
      2821834349U,
      2952996808U,
      3210313671U,
      3336571891U,
      3584528711U,
      113926993U,
      338241895U,
      666307205U,
      773529912U,
      1294757372U,
      1396182291U,
      1695183700U,
      1986661051U,
      2177026350U,
      2456956037U,
      2730485921U,
      2820302411U,
      3259730800U,
      3345764771U,
      3516065817U,
      3600352804U,
      4094571909U,
      275423344U,
      430227734U,
      506948616U,
      659060556U,
      883997877U,
      958139571U,
      1322822218U,
      1537002063U,
      1747873779U,
      1955562222U,
      2024104815U,
      2227730452U,
      2361852424U,
      2428436474U,
      2756734187U,
      3204031479U,
      3329325298U
    };

    public Sha224Digest() => Reset();

    public Sha224Digest( Sha224Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( Sha224Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        H5 = t.H5;
        H6 = t.H6;
        H7 = t.H7;
        H8 = t.H8;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "SHA-224";

    public override int GetDigestSize() => 28;

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff] = Pack.BE_To_UInt32( input, inOff );
        if (++xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (uint)(bitLength >>> 32);
        X[15] = (uint)bitLength;
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        Pack.UInt32_To_BE( H1, output, outOff );
        Pack.UInt32_To_BE( H2, output, outOff + 4 );
        Pack.UInt32_To_BE( H3, output, outOff + 8 );
        Pack.UInt32_To_BE( H4, output, outOff + 12 );
        Pack.UInt32_To_BE( H5, output, outOff + 16 );
        Pack.UInt32_To_BE( H6, output, outOff + 20 );
        Pack.UInt32_To_BE( H7, output, outOff + 24 );
        Reset();
        return 28;
    }

    public override void Reset()
    {
        base.Reset();
        H1 = 3238371032U;
        H2 = 914150663U;
        H3 = 812702999U;
        H4 = 4144912697U;
        H5 = 4290775857U;
        H6 = 1750603025U;
        H7 = 1694076839U;
        H8 = 3204075428U;
        xOff = 0;
        Array.Clear( X, 0, X.Length );
    }

    internal override void ProcessBlock()
    {
        for (int index = 16; index <= 63; ++index)
            X[index] = Theta1( X[index - 2] ) + X[index - 7] + Theta0( X[index - 15] ) + X[index - 16];
        uint num1 = H1;
        uint num2 = H2;
        uint num3 = H3;
        uint num4 = H4;
        uint num5 = H5;
        uint num6 = H6;
        uint num7 = H7;
        uint num8 = H8;
        int index1 = 0;
        for (int index2 = 0; index2 < 8; ++index2)
        {
            uint num9 = num8 + Sum1( num5 ) + Ch( num5, num6, num7 ) + K[index1] + X[index1];
            uint num10 = num4 + num9;
            uint num11 = num9 + Sum0( num1 ) + Maj( num1, num2, num3 );
            int index3 = index1 + 1;
            uint num12 = num7 + Sum1( num10 ) + Ch( num10, num5, num6 ) + K[index3] + X[index3];
            uint num13 = num3 + num12;
            uint num14 = num12 + Sum0( num11 ) + Maj( num11, num1, num2 );
            int index4 = index3 + 1;
            uint num15 = num6 + Sum1( num13 ) + Ch( num13, num10, num5 ) + K[index4] + X[index4];
            uint num16 = num2 + num15;
            uint num17 = num15 + Sum0( num14 ) + Maj( num14, num11, num1 );
            int index5 = index4 + 1;
            uint num18 = num5 + Sum1( num16 ) + Ch( num16, num13, num10 ) + K[index5] + X[index5];
            uint num19 = num1 + num18;
            uint num20 = num18 + Sum0( num17 ) + Maj( num17, num14, num11 );
            int index6 = index5 + 1;
            uint num21 = num10 + Sum1( num19 ) + Ch( num19, num16, num13 ) + K[index6] + X[index6];
            num8 = num11 + num21;
            num4 = num21 + Sum0( num20 ) + Maj( num20, num17, num14 );
            int index7 = index6 + 1;
            uint num22 = num13 + Sum1( num8 ) + Ch( num8, num19, num16 ) + K[index7] + X[index7];
            num7 = num14 + num22;
            num3 = num22 + Sum0( num4 ) + Maj( num4, num20, num17 );
            int index8 = index7 + 1;
            uint num23 = num16 + Sum1( num7 ) + Ch( num7, num8, num19 ) + K[index8] + X[index8];
            num6 = num17 + num23;
            num2 = num23 + Sum0( num3 ) + Maj( num3, num4, num20 );
            int index9 = index8 + 1;
            uint num24 = num19 + Sum1( num6 ) + Ch( num6, num7, num8 ) + K[index9] + X[index9];
            num5 = num20 + num24;
            num1 = num24 + Sum0( num2 ) + Maj( num2, num3, num4 );
            index1 = index9 + 1;
        }
        H1 += num1;
        H2 += num2;
        H3 += num3;
        H4 += num4;
        H5 += num5;
        H6 += num6;
        H7 += num7;
        H8 += num8;
        xOff = 0;
        Array.Clear( X, 0, 16 );
    }

    private static uint Ch( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) ^ (~(int)x & (int)z));

    private static uint Maj( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) ^ ((int)x & (int)z) ^ ((int)y & (int)z));

    private static uint Sum0( uint x ) => (uint)(((int)(x >> 2) | ((int)x << 30)) ^ ((int)(x >> 13) | ((int)x << 19)) ^ ((int)(x >> 22) | ((int)x << 10)));

    private static uint Sum1( uint x ) => (uint)(((int)(x >> 6) | ((int)x << 26)) ^ ((int)(x >> 11) | ((int)x << 21)) ^ ((int)(x >> 25) | ((int)x << 7)));

    private static uint Theta0( uint x ) => (uint)(((int)(x >> 7) | ((int)x << 25)) ^ ((int)(x >> 18) | ((int)x << 14))) ^ (x >> 3);

    private static uint Theta1( uint x ) => (uint)(((int)(x >> 17) | ((int)x << 15)) ^ ((int)(x >> 19) | ((int)x << 13))) ^ (x >> 10);

    public override IMemoable Copy() => new Sha224Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (Sha224Digest)other );
}
public class Sha256Digest : GeneralDigest
{
    private const int DigestLength = 32;
    private uint H1;
    private uint H2;
    private uint H3;
    private uint H4;
    private uint H5;
    private uint H6;
    private uint H7;
    private uint H8;
    private uint[] X = new uint[64];
    private int xOff;
    private static readonly uint[] K = new uint[64]
    {
      1116352408U,
      1899447441U,
      3049323471U,
      3921009573U,
      961987163U,
      1508970993U,
      2453635748U,
      2870763221U,
      3624381080U,
      310598401U,
      607225278U,
      1426881987U,
      1925078388U,
      2162078206U,
      2614888103U,
      3248222580U,
      3835390401U,
      4022224774U,
      264347078U,
      604807628U,
      770255983U,
      1249150122U,
      1555081692U,
      1996064986U,
      2554220882U,
      2821834349U,
      2952996808U,
      3210313671U,
      3336571891U,
      3584528711U,
      113926993U,
      338241895U,
      666307205U,
      773529912U,
      1294757372U,
      1396182291U,
      1695183700U,
      1986661051U,
      2177026350U,
      2456956037U,
      2730485921U,
      2820302411U,
      3259730800U,
      3345764771U,
      3516065817U,
      3600352804U,
      4094571909U,
      275423344U,
      430227734U,
      506948616U,
      659060556U,
      883997877U,
      958139571U,
      1322822218U,
      1537002063U,
      1747873779U,
      1955562222U,
      2024104815U,
      2227730452U,
      2361852424U,
      2428436474U,
      2756734187U,
      3204031479U,
      3329325298U
    };

    public Sha256Digest() => initHs();

    public Sha256Digest( Sha256Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( Sha256Digest t )
    {
        CopyIn( (GeneralDigest)t );
        H1 = t.H1;
        H2 = t.H2;
        H3 = t.H3;
        H4 = t.H4;
        H5 = t.H5;
        H6 = t.H6;
        H7 = t.H7;
        H8 = t.H8;
        Array.Copy( t.X, 0, X, 0, t.X.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "SHA-256";

    public override int GetDigestSize() => 32;

    internal override void ProcessWord( byte[] input, int inOff )
    {
        X[xOff] = Pack.BE_To_UInt32( input, inOff );
        if (++xOff != 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
            ProcessBlock();
        X[14] = (uint)(bitLength >>> 32);
        X[15] = (uint)bitLength;
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        Pack.UInt32_To_BE( H1, output, outOff );
        Pack.UInt32_To_BE( H2, output, outOff + 4 );
        Pack.UInt32_To_BE( H3, output, outOff + 8 );
        Pack.UInt32_To_BE( H4, output, outOff + 12 );
        Pack.UInt32_To_BE( H5, output, outOff + 16 );
        Pack.UInt32_To_BE( H6, output, outOff + 20 );
        Pack.UInt32_To_BE( H7, output, outOff + 24 );
        Pack.UInt32_To_BE( H8, output, outOff + 28 );
        Reset();
        return 32;
    }

    public override void Reset()
    {
        base.Reset();
        initHs();
        xOff = 0;
        Array.Clear( X, 0, X.Length );
    }

    private void initHs()
    {
        H1 = 1779033703U;
        H2 = 3144134277U;
        H3 = 1013904242U;
        H4 = 2773480762U;
        H5 = 1359893119U;
        H6 = 2600822924U;
        H7 = 528734635U;
        H8 = 1541459225U;
    }

    internal override void ProcessBlock()
    {
        for (int index = 16; index <= 63; ++index)
            X[index] = Theta1( X[index - 2] ) + X[index - 7] + Theta0( X[index - 15] ) + X[index - 16];
        uint num1 = H1;
        uint num2 = H2;
        uint num3 = H3;
        uint num4 = H4;
        uint num5 = H5;
        uint num6 = H6;
        uint num7 = H7;
        uint num8 = H8;
        int index1 = 0;
        for (int index2 = 0; index2 < 8; ++index2)
        {
            uint num9 = num8 + Sum1Ch( num5, num6, num7 ) + K[index1] + X[index1];
            uint num10 = num4 + num9;
            uint num11 = num9 + Sum0Maj( num1, num2, num3 );
            int index3 = index1 + 1;
            uint num12 = num7 + Sum1Ch( num10, num5, num6 ) + K[index3] + X[index3];
            uint num13 = num3 + num12;
            uint num14 = num12 + Sum0Maj( num11, num1, num2 );
            int index4 = index3 + 1;
            uint num15 = num6 + Sum1Ch( num13, num10, num5 ) + K[index4] + X[index4];
            uint num16 = num2 + num15;
            uint num17 = num15 + Sum0Maj( num14, num11, num1 );
            int index5 = index4 + 1;
            uint num18 = num5 + Sum1Ch( num16, num13, num10 ) + K[index5] + X[index5];
            uint num19 = num1 + num18;
            uint num20 = num18 + Sum0Maj( num17, num14, num11 );
            int index6 = index5 + 1;
            uint num21 = num10 + Sum1Ch( num19, num16, num13 ) + K[index6] + X[index6];
            num8 = num11 + num21;
            num4 = num21 + Sum0Maj( num20, num17, num14 );
            int index7 = index6 + 1;
            uint num22 = num13 + Sum1Ch( num8, num19, num16 ) + K[index7] + X[index7];
            num7 = num14 + num22;
            num3 = num22 + Sum0Maj( num4, num20, num17 );
            int index8 = index7 + 1;
            uint num23 = num16 + Sum1Ch( num7, num8, num19 ) + K[index8] + X[index8];
            num6 = num17 + num23;
            num2 = num23 + Sum0Maj( num3, num4, num20 );
            int index9 = index8 + 1;
            uint num24 = num19 + Sum1Ch( num6, num7, num8 ) + K[index9] + X[index9];
            num5 = num20 + num24;
            num1 = num24 + Sum0Maj( num2, num3, num4 );
            index1 = index9 + 1;
        }
        H1 += num1;
        H2 += num2;
        H3 += num3;
        H4 += num4;
        H5 += num5;
        H6 += num6;
        H7 += num7;
        H8 += num8;
        xOff = 0;
        Array.Clear( X, 0, 16 );
    }

    private static uint Sum1Ch( uint x, uint y, uint z ) => (uint)((((int)(x >> 6) | ((int)x << 26)) ^ ((int)(x >> 11) | ((int)x << 21)) ^ ((int)(x >> 25) | ((int)x << 7))) + (((int)x & (int)y) ^ (~(int)x & (int)z)));

    private static uint Sum0Maj( uint x, uint y, uint z ) => (uint)((((int)(x >> 2) | ((int)x << 30)) ^ ((int)(x >> 13) | ((int)x << 19)) ^ ((int)(x >> 22) | ((int)x << 10))) + (((int)x & (int)y) ^ ((int)x & (int)z) ^ ((int)y & (int)z)));

    private static uint Theta0( uint x ) => (uint)(((int)(x >> 7) | ((int)x << 25)) ^ ((int)(x >> 18) | ((int)x << 14))) ^ (x >> 3);

    private static uint Theta1( uint x ) => (uint)(((int)(x >> 17) | ((int)x << 15)) ^ ((int)(x >> 19) | ((int)x << 13))) ^ (x >> 10);

    public override IMemoable Copy() => new Sha256Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (Sha256Digest)other );
}
public class Sha384Digest : LongDigest
{
    private const int DigestLength = 48;

    public Sha384Digest()
    {
    }

    public Sha384Digest( Sha384Digest t )
      : base( t )
    {
    }

    public override string AlgorithmName => "SHA-384";

    public override int GetDigestSize() => 48;

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        Pack.UInt64_To_BE( H1, output, outOff );
        Pack.UInt64_To_BE( H2, output, outOff + 8 );
        Pack.UInt64_To_BE( H3, output, outOff + 16 );
        Pack.UInt64_To_BE( H4, output, outOff + 24 );
        Pack.UInt64_To_BE( H5, output, outOff + 32 );
        Pack.UInt64_To_BE( H6, output, outOff + 40 );
        Reset();
        return 48;
    }

    public override void Reset()
    {
        base.Reset();
        H1 = 14680500436340154072UL;
        H2 = 7105036623409894663UL;
        H3 = 10473403895298186519UL;
        H4 = 1526699215303891257UL;
        H5 = 7436329637833083697UL;
        H6 = 10282925794625328401UL;
        H7 = 15784041429090275239UL;
        H8 = 5167115440072839076UL;
    }

    public override IMemoable Copy() => new Sha384Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (LongDigest)other );
}
public class Sha3Digest : KeccakDigest
{
    private static int CheckBitLength( int bitLength )
    {
        switch (bitLength)
        {
            case 224:
            case 256:
            case 384:
            case 512:
                return bitLength;
            default:
                throw new ArgumentException( bitLength.ToString() + " not supported for SHA-3", nameof( bitLength ) );
        }
    }

    public Sha3Digest()
      : this( 256 )
    {
    }

    public Sha3Digest( int bitLength )
      : base( CheckBitLength( bitLength ) )
    {
    }

    public Sha3Digest( Sha3Digest source )
      : base( source )
    {
    }

    public override string AlgorithmName => "SHA3-" + fixedOutputLength;

    public override int DoFinal( byte[] output, int outOff )
    {
        Absorb( new byte[1] { 2 }, 0, 2L );
        return base.DoFinal( output, outOff );
    }

    protected override int DoFinal( byte[] output, int outOff, byte partialByte, int partialBits )
    {
        if (partialBits < 0 || partialBits > 7)
            throw new ArgumentException( "must be in the range [0,7]", nameof( partialBits ) );
        int partialByte1 = (partialByte & ((1 << partialBits) - 1)) | (2 << partialBits);
        int partialBits1 = partialBits + 2;
        if (partialBits1 >= 8)
        {
            oneByte[0] = (byte)partialByte1;
            Absorb( oneByte, 0, 8L );
            partialBits1 -= 8;
            partialByte1 >>= 8;
        }
        return base.DoFinal( output, outOff, (byte)partialByte1, partialBits1 );
    }

    public override IMemoable Copy() => new Sha3Digest( this );
}
public class Sha512Digest : LongDigest
{
    private const int DigestLength = 64;

    public Sha512Digest()
    {
    }

    public Sha512Digest( Sha512Digest t )
      : base( t )
    {
    }

    public override string AlgorithmName => "SHA-512";

    public override int GetDigestSize() => 64;

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        Pack.UInt64_To_BE( H1, output, outOff );
        Pack.UInt64_To_BE( H2, output, outOff + 8 );
        Pack.UInt64_To_BE( H3, output, outOff + 16 );
        Pack.UInt64_To_BE( H4, output, outOff + 24 );
        Pack.UInt64_To_BE( H5, output, outOff + 32 );
        Pack.UInt64_To_BE( H6, output, outOff + 40 );
        Pack.UInt64_To_BE( H7, output, outOff + 48 );
        Pack.UInt64_To_BE( H8, output, outOff + 56 );
        Reset();
        return 64;
    }

    public override void Reset()
    {
        base.Reset();
        H1 = 7640891576956012808UL;
        H2 = 13503953896175478587UL;
        H3 = 4354685564936845355UL;
        H4 = 11912009170470909681UL;
        H5 = 5840696475078001361UL;
        H6 = 11170449401992604703UL;
        H7 = 2270897969802886507UL;
        H8 = 6620516959819538809UL;
    }

    public override IMemoable Copy() => new Sha512Digest( this );

    public override void Reset( IMemoable other ) => CopyIn( (LongDigest)other );
}
public class Sha512tDigest : LongDigest
{
    private const ulong A5 = 11936128518282651045;
    private readonly int digestLength;
    private ulong H1t;
    private ulong H2t;
    private ulong H3t;
    private ulong H4t;
    private ulong H5t;
    private ulong H6t;
    private ulong H7t;
    private ulong H8t;

    public Sha512tDigest( int bitLength )
    {
        if (bitLength >= 512)
            throw new ArgumentException( "cannot be >= 512", nameof( bitLength ) );
        if (bitLength % 8 != 0)
            throw new ArgumentException( "needs to be a multiple of 8", nameof( bitLength ) );
        if (bitLength == 384)
            throw new ArgumentException( "cannot be 384 use SHA384 instead", nameof( bitLength ) );
        digestLength = bitLength / 8;
        tIvGenerate( digestLength * 8 );
        Reset();
    }

    public Sha512tDigest( Sha512tDigest t )
      : base( t )
    {
        digestLength = t.digestLength;
        Reset( t );
    }

    public override string AlgorithmName => "SHA-512/" + (digestLength * 8);

    public override int GetDigestSize() => digestLength;

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UInt64_To_BE( H1, output, outOff, digestLength );
        UInt64_To_BE( H2, output, outOff + 8, digestLength - 8 );
        UInt64_To_BE( H3, output, outOff + 16, digestLength - 16 );
        UInt64_To_BE( H4, output, outOff + 24, digestLength - 24 );
        UInt64_To_BE( H5, output, outOff + 32, digestLength - 32 );
        UInt64_To_BE( H6, output, outOff + 40, digestLength - 40 );
        UInt64_To_BE( H7, output, outOff + 48, digestLength - 48 );
        UInt64_To_BE( H8, output, outOff + 56, digestLength - 56 );
        Reset();
        return digestLength;
    }

    public override void Reset()
    {
        base.Reset();
        H1 = H1t;
        H2 = H2t;
        H3 = H3t;
        H4 = H4t;
        H5 = H5t;
        H6 = H6t;
        H7 = H7t;
        H8 = H8t;
    }

    private void tIvGenerate( int bitLength )
    {
        H1 = 14964410163792538797UL;
        H2 = 2216346199247487646UL;
        H3 = 11082046791023156622UL;
        H4 = 65953792586715988UL;
        H5 = 17630457682085488500UL;
        H6 = 4512832404995164602UL;
        H7 = 13413544941332994254UL;
        H8 = 18322165818757711068UL;
        Update( 83 );
        Update( 72 );
        Update( 65 );
        Update( 45 );
        Update( 53 );
        Update( 49 );
        Update( 50 );
        Update( 47 );
        if (bitLength > 100)
        {
            Update( (byte)((bitLength / 100) + 48) );
            bitLength %= 100;
            Update( (byte)((bitLength / 10) + 48) );
            bitLength %= 10;
            Update( (byte)(bitLength + 48) );
        }
        else if (bitLength > 10)
        {
            Update( (byte)((bitLength / 10) + 48) );
            bitLength %= 10;
            Update( (byte)(bitLength + 48) );
        }
        else
            Update( (byte)(bitLength + 48) );
        Finish();
        H1t = H1;
        H2t = H2;
        H3t = H3;
        H4t = H4;
        H5t = H5;
        H6t = H6;
        H7t = H7;
        H8t = H8;
    }

    private static void UInt64_To_BE( ulong n, byte[] bs, int off, int max )
    {
        if (max <= 0)
            return;
        UInt32_To_BE( (uint)(n >> 32), bs, off, max );
        if (max <= 4)
            return;
        UInt32_To_BE( (uint)n, bs, off + 4, max - 4 );
    }

    private static void UInt32_To_BE( uint n, byte[] bs, int off, int max )
    {
        int num1 = System.Math.Min( 4, max );
        while (--num1 >= 0)
        {
            int num2 = 8 * (3 - num1);
            bs[off + num1] = (byte)(n >> num2);
        }
    }

    public override IMemoable Copy() => new Sha512tDigest( this );

    public override void Reset( IMemoable other )
    {
        Sha512tDigest t = (Sha512tDigest)other;
        if (digestLength != t.digestLength)
            throw new MemoableResetException( "digestLength inappropriate in other" );
        CopyIn( t );
        H1t = t.H1t;
        H2t = t.H2t;
        H3t = t.H3t;
        H4t = t.H4t;
        H5t = t.H5t;
        H6t = t.H6t;
        H7t = t.H7t;
        H8t = t.H8t;
    }
}
public class MemoableResetException : InvalidCastException
{
    public MemoableResetException( string msg )
      : base( msg )
    {
    }
}
public class ShakeDigest : KeccakDigest, IXof, IDigest
{
    private static int CheckBitLength( int bitLength )
    {
        switch (bitLength)
        {
            case 128:
            case 256:
                return bitLength;
            default:
                throw new ArgumentException( bitLength.ToString() + " not supported for SHAKE", nameof( bitLength ) );
        }
    }

    public ShakeDigest()
      : this( 128 )
    {
    }

    public ShakeDigest( int bitLength )
      : base( CheckBitLength( bitLength ) )
    {
    }

    public ShakeDigest( ShakeDigest source )
      : base( source )
    {
    }

    public override string AlgorithmName => "SHAKE" + fixedOutputLength;

    public override int DoFinal( byte[] output, int outOff ) => DoFinal( output, outOff, GetDigestSize() );

    public virtual int DoFinal( byte[] output, int outOff, int outLen )
    {
        Absorb( new byte[1] { 15 }, 0, 4L );
        Squeeze( output, outOff, outLen * 8L );
        Reset();
        return outLen;
    }

    protected override int DoFinal( byte[] output, int outOff, byte partialByte, int partialBits ) => DoFinal( output, outOff, GetDigestSize(), partialByte, partialBits );

    protected virtual int DoFinal(
      byte[] output,
      int outOff,
      int outLen,
      byte partialByte,
      int partialBits )
    {
        if (partialBits < 0 || partialBits > 7)
            throw new ArgumentException( "must be in the range [0,7]", nameof( partialBits ) );
        int num = (partialByte & ((1 << partialBits) - 1)) | (15 << partialBits);
        int databitlen = partialBits + 4;
        if (databitlen >= 8)
        {
            oneByte[0] = (byte)num;
            Absorb( oneByte, 0, 8L );
            databitlen -= 8;
            num >>= 8;
        }
        if (databitlen > 0)
        {
            oneByte[0] = (byte)num;
            Absorb( oneByte, 0, databitlen );
        }
        Squeeze( output, outOff, outLen * 8L );
        Reset();
        return outLen;
    }

    public override IMemoable Copy() => new ShakeDigest( this );
}
public class ShortenedDigest : IDigest
{
    private IDigest baseDigest;
    private int length;

    public ShortenedDigest( IDigest baseDigest, int length )
    {
        if (baseDigest == null)
            throw new ArgumentNullException( nameof( baseDigest ) );
        this.baseDigest = length <= baseDigest.GetDigestSize() ? baseDigest : throw new ArgumentException( "baseDigest output not large enough to support length" );
        this.length = length;
    }

    public string AlgorithmName => baseDigest.AlgorithmName + "(" + (length * 8) + ")";

    public int GetDigestSize() => length;

    public void Update( byte input ) => baseDigest.Update( input );

    public void BlockUpdate( byte[] input, int inOff, int length ) => baseDigest.BlockUpdate( input, inOff, length );

    public int DoFinal( byte[] output, int outOff )
    {
        byte[] numArray = new byte[baseDigest.GetDigestSize()];
        baseDigest.DoFinal( numArray, 0 );
        Array.Copy( numArray, 0, output, outOff, length );
        return length;
    }

    public void Reset() => baseDigest.Reset();

    public int GetByteLength() => baseDigest.GetByteLength();
}
public class SkeinDigest : IDigest, IMemoable
{
    public const int SKEIN_256 = 256;
    public const int SKEIN_512 = 512;
    public const int SKEIN_1024 = 1024;
    private readonly SkeinEngine engine;

    public SkeinDigest( int stateSizeBits, int digestSizeBits )
    {
        engine = new SkeinEngine( stateSizeBits, digestSizeBits );
        Init( null );
    }

    public SkeinDigest( SkeinDigest digest ) => engine = new SkeinEngine( digest.engine );

    public void Reset( IMemoable other ) => engine.Reset( ((SkeinDigest)other).engine );

    public IMemoable Copy() => new SkeinDigest( this );

    public string AlgorithmName => "Skein-" + (engine.BlockSize * 8) + "-" + (engine.OutputSize * 8);

    public int GetDigestSize() => engine.OutputSize;

    public int GetByteLength() => engine.BlockSize;

    public void Init( SkeinParameters parameters ) => engine.Init( parameters );

    public void Reset() => engine.Reset();

    public void Update( byte inByte ) => engine.Update( inByte );

    public void BlockUpdate( byte[] inBytes, int inOff, int len ) => engine.Update( inBytes, inOff, len );

    public int DoFinal( byte[] outBytes, int outOff ) => engine.DoFinal( outBytes, outOff );
}
public class SkeinEngine : IMemoable
{
    public const int SKEIN_256 = 256;
    public const int SKEIN_512 = 512;
    public const int SKEIN_1024 = 1024;
    private const int PARAM_TYPE_KEY = 0;
    private const int PARAM_TYPE_CONFIG = 4;
    private const int PARAM_TYPE_MESSAGE = 48;
    private const int PARAM_TYPE_OUTPUT = 63;
    private static readonly IDictionary INITIAL_STATES = Platform.CreateHashtable();
    private readonly ThreefishEngine threefish;
    private readonly int outputSizeBytes;
    private ulong[] chain;
    private ulong[] initialState;
    private byte[] key;
    private Parameter[] preMessageParameters;
    private Parameter[] postMessageParameters;
    private readonly UBI ubi;
    private readonly byte[] singleByte = new byte[1];

    static SkeinEngine()
    {
        InitialState( 256, 128, new ulong[4]
  {
        16217771249220022880UL,
        9817190399063458076UL,
        1155188648486244218UL,
        14769517481627992514UL
  } );
        InitialState( 256, 160, new ulong[4]
  {
        1450197650740764312UL,
        3081844928540042640UL,
        15310647011875280446UL,
        3301952811952417661UL
  } );
        InitialState( 256, 224, new ulong[4]
  {
        14270089230798940683UL,
        9758551101254474012UL,
        11082101768697755780UL,
        4056579644589979102UL
  } );
        InitialState( 256, 256, new ulong[4]
  {
        18202890402666165321UL,
        3443677322885453875UL,
        12915131351309911055UL,
        7662005193972177513UL
  } );
        InitialState( 512, 128, new ulong[8]
  {
        12158729379475595090UL,
        2204638249859346602UL,
        3502419045458743507UL,
        13617680570268287068UL,
        983504137758028059UL,
        1880512238245786339UL,
        11730851291495443074UL,
        7602827311880509485UL
  } );
        InitialState( 512, 160, new ulong[8]
  {
        2934123928682216849UL,
        14047033351726823311UL,
        1684584802963255058UL,
        5744138295201861711UL,
        2444857010922934358UL,
        15638910433986703544UL,
        13325156239043941114UL,
        118355523173251694UL
  } );
        InitialState( 512, 224, new ulong[8]
  {
        14758403053642543652UL,
        14674518637417806319UL,
        10145881904771976036UL,
        4146387520469897396UL,
        1106145742801415120UL,
        7455425944880474941UL,
        11095680972475339753UL,
        11397762726744039159UL
  } );
        InitialState( 512, 384, new ulong[8]
  {
        11814849197074935647UL,
        12753905853581818532UL,
        11346781217370868990UL,
        15535391162178797018UL,
        2000907093792408677UL,
        9140007292425499655UL,
        6093301768906360022UL,
        2769176472213098488UL
  } );
        InitialState( 512, 512, new ulong[8]
  {
        5261240102383538638UL,
        978932832955457283UL,
        10363226125605772238UL,
        11107378794354519217UL,
        6752626034097301424UL,
        16915020251879818228UL,
        11029617608758768931UL,
        12544957130904423475UL
  } );
    }

    private static void InitialState( int blockSize, int outputSize, ulong[] state ) => INITIAL_STATES.Add( VariantIdentifier( blockSize / 8, outputSize / 8 ), state );

    private static int VariantIdentifier( int blockSizeBytes, int outputSizeBytes ) => (outputSizeBytes << 16) | blockSizeBytes;

    public SkeinEngine( int blockSizeBits, int outputSizeBits )
    {
        if (outputSizeBits % 8 != 0)
            throw new ArgumentException( "Output size must be a multiple of 8 bits. :" + outputSizeBits );
        outputSizeBytes = outputSizeBits / 8;
        threefish = new ThreefishEngine( blockSizeBits );
        ubi = new UBI( this, threefish.GetBlockSize() );
    }

    public SkeinEngine( SkeinEngine engine )
      : this( engine.BlockSize * 8, engine.OutputSize * 8 )
    {
        CopyIn( engine );
    }

    private void CopyIn( SkeinEngine engine )
    {
        ubi.Reset( engine.ubi );
        chain = Arrays.Clone( engine.chain, chain );
        initialState = Arrays.Clone( engine.initialState, initialState );
        key = Arrays.Clone( engine.key, key );
        preMessageParameters = Clone( engine.preMessageParameters, preMessageParameters );
        postMessageParameters = Clone( engine.postMessageParameters, postMessageParameters );
    }

    private static Parameter[] Clone(
      Parameter[] data,
      Parameter[] existing )
    {
        if (data == null)
            return null;
        if (existing == null || existing.Length != data.Length)
            existing = new Parameter[data.Length];
        Array.Copy( data, 0, existing, 0, existing.Length );
        return existing;
    }

    public IMemoable Copy() => new SkeinEngine( this );

    public void Reset( IMemoable other )
    {
        SkeinEngine engine = (SkeinEngine)other;
        if (BlockSize != engine.BlockSize || outputSizeBytes != engine.outputSizeBytes)
            throw new MemoableResetException( "Incompatible parameters in provided SkeinEngine." );
        CopyIn( engine );
    }

    public int OutputSize => outputSizeBytes;

    public int BlockSize => threefish.GetBlockSize();

    public void Init( SkeinParameters parameters )
    {
        chain = null;
        key = null;
        preMessageParameters = null;
        postMessageParameters = null;
        if (parameters != null)
        {
            if (parameters.GetKey().Length < 16)
                throw new ArgumentException( "Skein key must be at least 128 bits." );
            this.InitParams( parameters.GetParameters() );
        }
        CreateInitialState();
        UbiInit( 48 );
    }

    private void InitParams( IDictionary parameters )
    {
        IEnumerator enumerator = parameters.Keys.GetEnumerator();
        IList arrayList1 = Platform.CreateArrayList();
        IList arrayList2 = Platform.CreateArrayList();
        while (enumerator.MoveNext())
        {
            int current = (int)enumerator.Current;
            byte[] parameter = (byte[])parameters[current];
            if (current == 0)
                key = parameter;
            else if (current < 48)
                arrayList1.Add( new Parameter( current, parameter ) );
            else
                arrayList2.Add( new Parameter( current, parameter ) );
        }
        preMessageParameters = new Parameter[arrayList1.Count];
        arrayList1.CopyTo( preMessageParameters, 0 );
        Array.Sort( (Array)preMessageParameters );
        postMessageParameters = new Parameter[arrayList2.Count];
        arrayList2.CopyTo( postMessageParameters, 0 );
        Array.Sort( (Array)postMessageParameters );
    }

    private void CreateInitialState()
    {
        ulong[] data = (ulong[])INITIAL_STATES[VariantIdentifier( BlockSize, OutputSize )];
        if (key == null && data != null)
        {
            chain = Arrays.Clone( data );
        }
        else
        {
            chain = new ulong[BlockSize / 8];
            if (key != null)
                UbiComplete( 0, key );
            UbiComplete( 4, new Configuration( outputSizeBytes * 8 ).Bytes );
        }
        if (preMessageParameters != null)
        {
            for (int index = 0; index < preMessageParameters.Length; ++index)
            {
                Parameter messageParameter = preMessageParameters[index];
                UbiComplete( messageParameter.Type, messageParameter.Value );
            }
        }
        initialState = Arrays.Clone( chain );
    }

    public void Reset()
    {
        Array.Copy( initialState, 0, chain, 0, chain.Length );
        UbiInit( 48 );
    }

    private void UbiComplete( int type, byte[] value )
    {
        UbiInit( type );
        ubi.Update( value, 0, value.Length, chain );
        UbiFinal();
    }

    private void UbiInit( int type ) => ubi.Reset( type );

    private void UbiFinal() => ubi.DoFinal( chain );

    private void CheckInitialised()
    {
        if (ubi == null)
            throw new ArgumentException( "Skein engine is not initialised." );
    }

    public void Update( byte inByte )
    {
        singleByte[0] = inByte;
        Update( singleByte, 0, 1 );
    }

    public void Update( byte[] inBytes, int inOff, int len )
    {
        CheckInitialised();
        ubi.Update( inBytes, inOff, len, chain );
    }

    public int DoFinal( byte[] outBytes, int outOff )
    {
        CheckInitialised();
        if (outBytes.Length < outOff + outputSizeBytes)
            throw new DataLengthException( "Output buffer is too short to hold output" );
        UbiFinal();
        if (postMessageParameters != null)
        {
            for (int index = 0; index < postMessageParameters.Length; ++index)
            {
                Parameter messageParameter = postMessageParameters[index];
                UbiComplete( messageParameter.Type, messageParameter.Value );
            }
        }
        int blockSize = BlockSize;
        int num = (outputSizeBytes + blockSize - 1) / blockSize;
        for (int outputSequence = 0; outputSequence < num; ++outputSequence)
        {
            int outputBytes = System.Math.Min( blockSize, outputSizeBytes - (outputSequence * blockSize) );
            Output( (ulong)outputSequence, outBytes, outOff + (outputSequence * blockSize), outputBytes );
        }
        Reset();
        return outputSizeBytes;
    }

    private void Output( ulong outputSequence, byte[] outBytes, int outOff, int outputBytes )
    {
        byte[] numArray = new byte[8];
        ThreefishEngine.WordToBytes( outputSequence, numArray, 0 );
        ulong[] output = new ulong[chain.Length];
        UbiInit( 63 );
        ubi.Update( numArray, 0, numArray.Length, output );
        ubi.DoFinal( output );
        int num = (outputBytes + 8 - 1) / 8;
        for (int index = 0; index < num; ++index)
        {
            int length = System.Math.Min( 8, outputBytes - (index * 8) );
            if (length == 8)
            {
                ThreefishEngine.WordToBytes( output[index], outBytes, outOff + (index * 8) );
            }
            else
            {
                ThreefishEngine.WordToBytes( output[index], numArray, 0 );
                Array.Copy( numArray, 0, outBytes, outOff + (index * 8), length );
            }
        }
    }

    private class Configuration
    {
        private byte[] bytes = new byte[32];

        public Configuration( long outputSizeBits )
        {
            bytes[0] = 83;
            bytes[1] = 72;
            bytes[2] = 65;
            bytes[3] = 51;
            bytes[4] = 1;
            bytes[5] = 0;
            ThreefishEngine.WordToBytes( (ulong)outputSizeBits, bytes, 8 );
        }

        public byte[] Bytes => bytes;
    }

    public class Parameter
    {
        private int type;
        private byte[] value;

        public Parameter( int type, byte[] value )
        {
            this.type = type;
            this.value = value;
        }

        public int Type => type;

        public byte[] Value => value;
    }

    private class UbiTweak
    {
        private const ulong LOW_RANGE = 18446744069414584320;
        private const ulong T1_FINAL = 9223372036854775808;
        private const ulong T1_FIRST = 4611686018427387904;
        private ulong[] tweak = new ulong[2];
        private bool extendedPosition;

        public UbiTweak() => Reset();

        public void Reset( UbiTweak tweak )
        {
            this.tweak = Arrays.Clone( tweak.tweak, this.tweak );
            extendedPosition = tweak.extendedPosition;
        }

        public void Reset()
        {
            tweak[0] = 0UL;
            tweak[1] = 0UL;
            extendedPosition = false;
            First = true;
        }

        public uint Type
        {
            get => (uint)((tweak[1] >> 56) & 63UL);
            set => tweak[1] = (ulong)(((long)tweak[1] & -274877906944L) | ((value & 63L) << 56));
        }

        public bool First
        {
            get => ((long)tweak[1] & 4611686018427387904L) != 0L;
            set
            {
                if (value)
                {
                    ulong[] tweak;
                    (tweak = this.tweak)[1] = tweak[1] | 4611686018427387904UL;
                }
                else
                {
                    ulong[] tweak;
                    (tweak = this.tweak)[1] = tweak[1] & 13835058055282163711UL;
                }
            }
        }

        public bool Final
        {
            get => ((long)tweak[1] & long.MinValue) != 0L;
            set
            {
                if (value)
                {
                    ulong[] tweak;
                    (tweak = this.tweak)[1] = tweak[1] | 9223372036854775808UL;
                }
                else
                {
                    ulong[] tweak;
                    (tweak = this.tweak)[1] = tweak[1] & long.MaxValue;
                }
            }
        }

        public void AdvancePosition( int advance )
        {
            if (extendedPosition)
            {
                ulong[] numArray = new ulong[3]
                {
            tweak[0] &  uint.MaxValue,
            (tweak[0] >> 32) &  uint.MaxValue,
            tweak[1] &  uint.MaxValue
                };
                ulong num1 = (ulong)advance;
                for (int index = 0; index < numArray.Length; ++index)
                {
                    ulong num2 = num1 + numArray[index];
                    numArray[index] = num2;
                    num1 = num2 >> 32;
                }
                tweak[0] = (ulong)((((long)numArray[1] & uint.MaxValue) << 32) | ((long)numArray[0] & uint.MaxValue));
                tweak[1] = (ulong)(((long)tweak[1] & -4294967296L) | ((long)numArray[2] & uint.MaxValue));
            }
            else
            {
                ulong num = tweak[0] + (uint)advance;
                tweak[0] = num;
                if (num <= 18446744069414584320UL)
                    return;
                extendedPosition = true;
            }
        }

        public ulong[] GetWords() => tweak;

        public override string ToString() => Type.ToString() + " first: " + First + ", final: " + Final;
    }

    private class UBI
    {
        private readonly UbiTweak tweak = new UbiTweak();
        private readonly SkeinEngine engine;
        private byte[] currentBlock;
        private int currentOffset;
        private ulong[] message;

        public UBI( SkeinEngine engine, int blockSize )
        {
            this.engine = engine;
            currentBlock = new byte[blockSize];
            message = new ulong[currentBlock.Length / 8];
        }

        public void Reset( UBI ubi )
        {
            currentBlock = Arrays.Clone( ubi.currentBlock, currentBlock );
            currentOffset = ubi.currentOffset;
            message = Arrays.Clone( ubi.message, message );
            tweak.Reset( ubi.tweak );
        }

        public void Reset( int type )
        {
            tweak.Reset();
            tweak.Type = (uint)type;
            currentOffset = 0;
        }

        public void Update( byte[] value, int offset, int len, ulong[] output )
        {
            int num1 = 0;
            while (len > num1)
            {
                if (currentOffset == currentBlock.Length)
                {
                    ProcessBlock( output );
                    tweak.First = false;
                    currentOffset = 0;
                }
                int num2 = System.Math.Min( len - num1, currentBlock.Length - currentOffset );
                Array.Copy( value, offset + num1, currentBlock, currentOffset, num2 );
                num1 += num2;
                currentOffset += num2;
                tweak.AdvancePosition( num2 );
            }
        }

        private void ProcessBlock( ulong[] output )
        {
            engine.threefish.Init( true, engine.chain, tweak.GetWords() );
            for (int index = 0; index < message.Length; ++index)
                message[index] = ThreefishEngine.BytesToWord( currentBlock, index * 8 );
            engine.threefish.ProcessBlock( message, output );
            for (int index = 0; index < output.Length; ++index)
                output[index] ^= message[index];
        }

        public void DoFinal( ulong[] output )
        {
            for (int currentOffset = this.currentOffset; currentOffset < currentBlock.Length; ++currentOffset)
                currentBlock[currentOffset] = 0;
            tweak.Final = true;
            ProcessBlock( output );
        }
    }
}
public class SM3Digest : GeneralDigest
{
    private const int DIGEST_LENGTH = 32;
    private const int BLOCK_SIZE = 16;
    private uint[] V = new uint[8];
    private uint[] inwords = new uint[16];
    private int xOff;
    private uint[] W = new uint[68];
    private uint[] W1 = new uint[64];
    private static readonly uint[] T = new uint[64];

    static SM3Digest()
    {
        for (int index = 0; index < 16; ++index)
        {
            uint num = 2043430169;
            T[index] = (num << index) | (num >> (32 - index));
        }
        for (int index = 16; index < 64; ++index)
        {
            int num1 = index % 32;
            uint num2 = 2055708042;
            T[index] = (num2 << num1) | (num2 >> (32 - num1));
        }
    }

    public SM3Digest() => Reset();

    public SM3Digest( SM3Digest t )
      : base( t )
    {
        CopyIn( t );
    }

    private void CopyIn( SM3Digest t )
    {
        Array.Copy( t.V, 0, V, 0, V.Length );
        Array.Copy( t.inwords, 0, inwords, 0, inwords.Length );
        xOff = t.xOff;
    }

    public override string AlgorithmName => "SM3";

    public override int GetDigestSize() => 32;

    public override IMemoable Copy() => new SM3Digest( this );

    public override void Reset( IMemoable other )
    {
        SM3Digest t = (SM3Digest)other;
        CopyIn( (GeneralDigest)t );
        CopyIn( t );
    }

    public override void Reset()
    {
        base.Reset();
        V[0] = 1937774191U;
        V[1] = 1226093241U;
        V[2] = 388252375U;
        V[3] = 3666478592U;
        V[4] = 2842636476U;
        V[5] = 372324522U;
        V[6] = 3817729613U;
        V[7] = 2969243214U;
        xOff = 0;
    }

    public override int DoFinal( byte[] output, int outOff )
    {
        Finish();
        Pack.UInt32_To_BE( V[0], output, outOff );
        Pack.UInt32_To_BE( V[1], output, outOff + 4 );
        Pack.UInt32_To_BE( V[2], output, outOff + 8 );
        Pack.UInt32_To_BE( V[3], output, outOff + 12 );
        Pack.UInt32_To_BE( V[4], output, outOff + 16 );
        Pack.UInt32_To_BE( V[5], output, outOff + 20 );
        Pack.UInt32_To_BE( V[6], output, outOff + 24 );
        Pack.UInt32_To_BE( V[7], output, outOff + 28 );
        Reset();
        return 32;
    }

    internal override void ProcessWord( byte[] input, int inOff )
    {
        inwords[xOff] = Pack.BE_To_UInt32( input, inOff );
        ++xOff;
        if (xOff < 16)
            return;
        ProcessBlock();
    }

    internal override void ProcessLength( long bitLength )
    {
        if (xOff > 14)
        {
            inwords[xOff] = 0U;
            ++xOff;
            ProcessBlock();
        }
        for (; xOff < 14; ++xOff)
            inwords[xOff] = 0U;
        inwords[xOff++] = (uint)(bitLength >> 32);
        inwords[xOff++] = (uint)bitLength;
    }

    private uint P0( uint x )
    {
        uint num1 = (x << 9) | (x >> 23);
        uint num2 = (x << 17) | (x >> 15);
        return x ^ num1 ^ num2;
    }

    private uint P1( uint x )
    {
        uint num1 = (x << 15) | (x >> 17);
        uint num2 = (x << 23) | (x >> 9);
        return x ^ num1 ^ num2;
    }

    private uint FF0( uint x, uint y, uint z ) => x ^ y ^ z;

    private uint FF1( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) | ((int)x & (int)z) | ((int)y & (int)z));

    private uint GG0( uint x, uint y, uint z ) => x ^ y ^ z;

    private uint GG1( uint x, uint y, uint z ) => (uint)(((int)x & (int)y) | (~(int)x & (int)z));

    internal override void ProcessBlock()
    {
        for (int index = 0; index < 16; ++index)
            W[index] = inwords[index];
        for (int index = 16; index < 68; ++index)
        {
            uint num1 = W[index - 3];
            uint num2 = (num1 << 15) | (num1 >> 17);
            uint num3 = W[index - 13];
            uint num4 = (num3 << 7) | (num3 >> 25);
            W[index] = P1( W[index - 16] ^ W[index - 9] ^ num2 ) ^ num4 ^ W[index - 6];
        }
        for (int index = 0; index < 64; ++index)
            W1[index] = W[index] ^ W[index + 4];
        uint x1 = V[0];
        uint y1 = V[1];
        uint z1 = V[2];
        uint num5 = V[3];
        uint x2 = V[4];
        uint y2 = V[5];
        uint z2 = V[6];
        uint num6 = V[7];
        for (int index = 0; index < 16; ++index)
        {
            uint num7 = (x1 << 12) | (x1 >> 20);
            uint num8 = num7 + x2 + T[index];
            uint num9 = (num8 << 7) | (num8 >> 25);
            uint num10 = num9 ^ num7;
            uint num11 = FF0( x1, y1, z1 ) + num5 + num10 + W1[index];
            uint x3 = GG0( x2, y2, z2 ) + num6 + num9 + W[index];
            num5 = z1;
            z1 = (y1 << 9) | (y1 >> 23);
            y1 = x1;
            x1 = num11;
            num6 = z2;
            z2 = (y2 << 19) | (y2 >> 13);
            y2 = x2;
            x2 = P0( x3 );
        }
        for (int index = 16; index < 64; ++index)
        {
            uint num12 = (x1 << 12) | (x1 >> 20);
            uint num13 = num12 + x2 + T[index];
            uint num14 = (num13 << 7) | (num13 >> 25);
            uint num15 = num14 ^ num12;
            uint num16 = FF1( x1, y1, z1 ) + num5 + num15 + W1[index];
            uint x4 = GG1( x2, y2, z2 ) + num6 + num14 + W[index];
            num5 = z1;
            z1 = (y1 << 9) | (y1 >> 23);
            y1 = x1;
            x1 = num16;
            num6 = z2;
            z2 = (y2 << 19) | (y2 >> 13);
            y2 = x2;
            x2 = P0( x4 );
        }
        uint[] v1;
        (v1 = V)[0] = v1[0] ^ x1;
        uint[] v2;
        (v2 = V)[1] = v2[1] ^ y1;
        uint[] v3;
        (v3 = V)[2] = v3[2] ^ z1;
        uint[] v4;
        (v4 = V)[3] = v4[3] ^ num5;
        uint[] v5;
        (v5 = V)[4] = v5[4] ^ x2;
        uint[] v6;
        (v6 = V)[5] = v6[5] ^ y2;
        uint[] v7;
        (v7 = V)[6] = v7[6] ^ z2;
        uint[] v8;
        (v8 = V)[7] = v8[7] ^ num6;
        xOff = 0;
    }
}
public class ThreefishEngine : IBlockCipher
{
    public const int BLOCKSIZE_256 = 256;
    public const int BLOCKSIZE_512 = 512;
    public const int BLOCKSIZE_1024 = 1024;
    private const int TWEAK_SIZE_BYTES = 16;
    private const int TWEAK_SIZE_WORDS = 2;
    private const int ROUNDS_256 = 72;
    private const int ROUNDS_512 = 72;
    private const int ROUNDS_1024 = 80;
    private const int MAX_ROUNDS = 80;
    private const ulong C_240 = 2004413935125273122;
    private static readonly int[] MOD9 = new int[80];
    private static readonly int[] MOD17 = new int[MOD9.Length];
    private static readonly int[] MOD5 = new int[MOD9.Length];
    private static readonly int[] MOD3 = new int[MOD9.Length];
    private readonly int blocksizeBytes;
    private readonly int blocksizeWords;
    private readonly ulong[] currentBlock;
    private readonly ulong[] t = new ulong[5];
    private readonly ulong[] kw;
    private readonly ThreefishCipher cipher;
    private bool forEncryption;

    static ThreefishEngine()
    {
        for (int index = 0; index < MOD9.Length; ++index)
        {
            MOD17[index] = index % 17;
            MOD9[index] = index % 9;
            MOD5[index] = index % 5;
            MOD3[index] = index % 3;
        }
    }

    public ThreefishEngine( int blocksizeBits )
    {
        blocksizeBytes = blocksizeBits / 8;
        blocksizeWords = blocksizeBytes / 8;
        currentBlock = new ulong[blocksizeWords];
        kw = new ulong[(2 * blocksizeWords) + 1];
        switch (blocksizeBits)
        {
            case 256:
                cipher = new Threefish256Cipher( kw, t );
                break;
            case 512:
                cipher = new Threefish512Cipher( kw, t );
                break;
            case 1024:
                cipher = new Threefish1024Cipher( kw, t );
                break;
            default:
                throw new ArgumentException( "Invalid blocksize - Threefish is defined with block size of 256, 512, or 1024 bits" );
        }
    }

    public virtual void Init( bool forEncryption, ICipherParameters parameters )
    {
        byte[] key1;
        byte[] bytes;
        switch (parameters)
        {
            case TweakableBlockCipherParameters _:
                TweakableBlockCipherParameters cipherParameters = (TweakableBlockCipherParameters)parameters;
                key1 = cipherParameters.Key.GetKey();
                bytes = cipherParameters.Tweak;
                break;
            case KeyParameter _:
                key1 = ((KeyParameter)parameters).GetKey();
                bytes = null;
                break;
            default:
                throw new ArgumentException( "Invalid parameter passed to Threefish init - " + Platform.GetTypeName( parameters ) );
        }
        ulong[] key2 = null;
        ulong[] tweak = null;
        if (key1 != null)
        {
            if (key1.Length != blocksizeBytes)
                throw new ArgumentException( "Threefish key must be same size as block (" + blocksizeBytes + " bytes)" );
            key2 = new ulong[blocksizeWords];
            for (int index = 0; index < key2.Length; ++index)
                key2[index] = BytesToWord( key1, index * 8 );
        }
        if (bytes != null)
            tweak = bytes.Length == 16 ? new ulong[2]
            {
          BytesToWord(bytes, 0),
          BytesToWord(bytes, 8)
            } : throw new ArgumentException( "Threefish tweak must be " + 16 + " bytes" );
        Init( forEncryption, key2, tweak );
    }

    internal void Init( bool forEncryption, ulong[] key, ulong[] tweak )
    {
        this.forEncryption = forEncryption;
        if (key != null)
            SetKey( key );
        if (tweak == null)
            return;
        SetTweak( tweak );
    }

    private void SetKey( ulong[] key )
    {
        if (key.Length != blocksizeWords)
            throw new ArgumentException( "Threefish key must be same size as block (" + blocksizeWords + " words)" );
        ulong num = 2004413935125273122;
        for (int index = 0; index < blocksizeWords; ++index)
        {
            kw[index] = key[index];
            num ^= kw[index];
        }
        kw[blocksizeWords] = num;
        Array.Copy( kw, 0, kw, blocksizeWords + 1, blocksizeWords );
    }

    private void SetTweak( ulong[] tweak )
    {
        t[0] = tweak.Length == 2 ? tweak[0] : throw new ArgumentException( "Tweak must be " + 2 + " words." );
        t[1] = tweak[1];
        t[2] = t[0] ^ t[1];
        t[3] = t[0];
        t[4] = t[1];
    }

    public virtual string AlgorithmName => "Threefish-" + (blocksizeBytes * 8);

    public virtual bool IsPartialBlockOkay => false;

    public virtual int GetBlockSize() => blocksizeBytes;

    public virtual void Reset()
    {
    }

    public virtual int ProcessBlock( byte[] inBytes, int inOff, byte[] outBytes, int outOff )
    {
        if (outOff + blocksizeBytes > outBytes.Length)
            throw new DataLengthException( "Output buffer too short" );
        if (inOff + blocksizeBytes > inBytes.Length)
            throw new DataLengthException( "Input buffer too short" );
        for (int index = 0; index < blocksizeBytes; index += 8)
            currentBlock[index >> 3] = BytesToWord( inBytes, inOff + index );
        ProcessBlock( currentBlock, currentBlock );
        for (int index = 0; index < blocksizeBytes; index += 8)
            WordToBytes( currentBlock[index >> 3], outBytes, outOff + index );
        return blocksizeBytes;
    }

    internal int ProcessBlock( ulong[] inWords, ulong[] outWords )
    {
        if (kw[blocksizeWords] == 0UL)
            throw new InvalidOperationException( "Threefish engine not initialised" );
        if (inWords.Length != blocksizeWords)
            throw new DataLengthException( "Input buffer too short" );
        if (outWords.Length != blocksizeWords)
            throw new DataLengthException( "Output buffer too short" );
        if (forEncryption)
            cipher.EncryptBlock( inWords, outWords );
        else
            cipher.DecryptBlock( inWords, outWords );
        return blocksizeWords;
    }

    internal static ulong BytesToWord( byte[] bytes, int off )
    {
        if (off + 8 > bytes.Length)
            throw new ArgumentException();
        int num1 = off;
        byte[] numArray1 = bytes;
        int index1 = num1;
        int num2 = index1 + 1;
        long num3 = (long)(numArray1[index1] & (ulong)byte.MaxValue);
        byte[] numArray2 = bytes;
        int index2 = num2;
        int num4 = index2 + 1;
        long num5 = (numArray2[index2] & (long)byte.MaxValue) << 8;
        long num6 = num3 | num5;
        byte[] numArray3 = bytes;
        int index3 = num4;
        int num7 = index3 + 1;
        long num8 = (numArray3[index3] & (long)byte.MaxValue) << 16;
        long num9 = num6 | num8;
        byte[] numArray4 = bytes;
        int index4 = num7;
        int num10 = index4 + 1;
        long num11 = (numArray4[index4] & (long)byte.MaxValue) << 24;
        long num12 = num9 | num11;
        byte[] numArray5 = bytes;
        int index5 = num10;
        int num13 = index5 + 1;
        long num14 = (numArray5[index5] & (long)byte.MaxValue) << 32;
        long num15 = num12 | num14;
        byte[] numArray6 = bytes;
        int index6 = num13;
        int num16 = index6 + 1;
        long num17 = (numArray6[index6] & (long)byte.MaxValue) << 40;
        long num18 = num15 | num17;
        byte[] numArray7 = bytes;
        int index7 = num16;
        int num19 = index7 + 1;
        long num20 = (numArray7[index7] & (long)byte.MaxValue) << 48;
        long num21 = num18 | num20;
        byte[] numArray8 = bytes;
        int index8 = num19;
        int num22 = index8 + 1;
        long num23 = (numArray8[index8] & (long)byte.MaxValue) << 56;
        return (ulong)(num21 | num23);
    }

    internal static void WordToBytes( ulong word, byte[] bytes, int off )
    {
        if (off + 8 > bytes.Length)
            throw new ArgumentException();
        int num1 = off;
        byte[] numArray1 = bytes;
        int index1 = num1;
        int num2 = index1 + 1;
        int num3 = (byte)word;
        numArray1[index1] = (byte)num3;
        byte[] numArray2 = bytes;
        int index2 = num2;
        int num4 = index2 + 1;
        int num5 = (byte)(word >> 8);
        numArray2[index2] = (byte)num5;
        byte[] numArray3 = bytes;
        int index3 = num4;
        int num6 = index3 + 1;
        int num7 = (byte)(word >> 16);
        numArray3[index3] = (byte)num7;
        byte[] numArray4 = bytes;
        int index4 = num6;
        int num8 = index4 + 1;
        int num9 = (byte)(word >> 24);
        numArray4[index4] = (byte)num9;
        byte[] numArray5 = bytes;
        int index5 = num8;
        int num10 = index5 + 1;
        int num11 = (byte)(word >> 32);
        numArray5[index5] = (byte)num11;
        byte[] numArray6 = bytes;
        int index6 = num10;
        int num12 = index6 + 1;
        int num13 = (byte)(word >> 40);
        numArray6[index6] = (byte)num13;
        byte[] numArray7 = bytes;
        int index7 = num12;
        int num14 = index7 + 1;
        int num15 = (byte)(word >> 48);
        numArray7[index7] = (byte)num15;
        byte[] numArray8 = bytes;
        int index8 = num14;
        int num16 = index8 + 1;
        int num17 = (byte)(word >> 56);
        numArray8[index8] = (byte)num17;
    }

    private static ulong RotlXor( ulong x, int n, ulong xor ) => ((x << n) | (x >> (64 - n))) ^ xor;

    private static ulong XorRotr( ulong x, int n, ulong xor )
    {
        ulong num = x ^ xor;
        return (num >> n) | (num << (64 - n));
    }

    private abstract class ThreefishCipher
    {
        protected readonly ulong[] t;
        protected readonly ulong[] kw;

        protected ThreefishCipher( ulong[] kw, ulong[] t )
        {
            this.kw = kw;
            this.t = t;
        }

        internal abstract void EncryptBlock( ulong[] block, ulong[] outWords );

        internal abstract void DecryptBlock( ulong[] block, ulong[] outWords );
    }

    private sealed class Threefish256Cipher : ThreefishCipher
    {
        private const int ROTATION_0_0 = 14;
        private const int ROTATION_0_1 = 16;
        private const int ROTATION_1_0 = 52;
        private const int ROTATION_1_1 = 57;
        private const int ROTATION_2_0 = 23;
        private const int ROTATION_2_1 = 40;
        private const int ROTATION_3_0 = 5;
        private const int ROTATION_3_1 = 37;
        private const int ROTATION_4_0 = 25;
        private const int ROTATION_4_1 = 33;
        private const int ROTATION_5_0 = 46;
        private const int ROTATION_5_1 = 12;
        private const int ROTATION_6_0 = 58;
        private const int ROTATION_6_1 = 22;
        private const int ROTATION_7_0 = 32;
        private const int ROTATION_7_1 = 32;

        public Threefish256Cipher( ulong[] kw, ulong[] t )
          : base( kw, t )
        {
        }

        internal override void EncryptBlock( ulong[] block, ulong[] outWords )
        {
            ulong[] kw = this.kw;
            ulong[] t = this.t;
            int[] moD5 = MOD5;
            int[] moD3 = MOD3;
            if (kw.Length != 9)
                throw new ArgumentException();
            if (t.Length != 5)
                throw new ArgumentException();
            ulong num1 = block[0];
            ulong num2 = block[1];
            ulong num3 = block[2];
            ulong num4 = block[3];
            ulong num5 = num1 + kw[0];
            ulong x1 = num2 + kw[1] + t[0];
            ulong num6 = num3 + kw[2] + t[1];
            ulong x2 = num4 + kw[3];
            for (int index1 = 1; index1 < 18; index1 += 2)
            {
                int index2 = moD5[index1];
                int index3 = moD3[index1];
                ulong num7;
                ulong x3 = RotlXor( x1, 14, num7 = num5 + x1 );
                ulong num8;
                ulong x4 = RotlXor( x2, 16, num8 = num6 + x2 );
                ulong num9;
                ulong x5 = RotlXor( x4, 52, num9 = num7 + x4 );
                ulong num10;
                ulong x6 = RotlXor( x3, 57, num10 = num8 + x3 );
                ulong num11;
                ulong x7 = RotlXor( x6, 23, num11 = num9 + x6 );
                ulong num12;
                ulong x8 = RotlXor( x5, 40, num12 = num10 + x5 );
                ulong num13;
                ulong num14 = RotlXor( x8, 5, num13 = num11 + x8 );
                ulong num15;
                ulong num16 = RotlXor( x7, 37, num15 = num12 + x7 );
                ulong num17 = num13 + kw[index2];
                ulong x9 = num16 + kw[index2 + 1] + t[index3];
                ulong num18 = num15 + kw[index2 + 2] + t[index3 + 1];
                ulong x10 = num14 + kw[index2 + 3] + (uint)index1;
                ulong num19;
                ulong x11 = RotlXor( x9, 25, num19 = num17 + x9 );
                ulong num20;
                ulong x12 = RotlXor( x10, 33, num20 = num18 + x10 );
                ulong num21;
                ulong x13 = RotlXor( x12, 46, num21 = num19 + x12 );
                ulong num22;
                ulong x14 = RotlXor( x11, 12, num22 = num20 + x11 );
                ulong num23;
                ulong x15 = RotlXor( x14, 58, num23 = num21 + x14 );
                ulong num24;
                ulong x16 = RotlXor( x13, 22, num24 = num22 + x13 );
                ulong num25;
                ulong num26 = RotlXor( x16, 32, num25 = num23 + x16 );
                ulong num27;
                ulong num28 = RotlXor( x15, 32, num27 = num24 + x15 );
                num5 = num25 + kw[index2 + 1];
                x1 = num28 + kw[index2 + 2] + t[index3 + 1];
                num6 = num27 + kw[index2 + 3] + t[index3 + 2];
                x2 = num26 + (ulong)((long)kw[index2 + 4] + (uint)index1 + 1L);
            }
            outWords[0] = num5;
            outWords[1] = x1;
            outWords[2] = num6;
            outWords[3] = x2;
        }

        internal override void DecryptBlock( ulong[] block, ulong[] state )
        {
            ulong[] kw = this.kw;
            ulong[] t = this.t;
            int[] moD5 = MOD5;
            int[] moD3 = MOD3;
            if (kw.Length != 9)
                throw new ArgumentException();
            if (t.Length != 5)
                throw new ArgumentException();
            ulong num1 = block[0];
            ulong num2 = block[1];
            ulong num3 = block[2];
            ulong num4 = block[3];
            for (int index1 = 17; index1 >= 1; index1 -= 2)
            {
                int index2 = moD5[index1];
                int index3 = moD3[index1];
                ulong xor1 = num1 - kw[index2 + 1];
                ulong x1 = num2 - (kw[index2 + 2] + t[index3 + 1]);
                ulong xor2 = num3 - (kw[index2 + 3] + t[index3 + 2]);
                ulong x2 = XorRotr( num4 - (ulong)((long)kw[index2 + 4] + (uint)index1 + 1L), 32, xor1 );
                ulong xor3 = xor1 - x2;
                ulong x3 = XorRotr( x1, 32, xor2 );
                ulong xor4 = xor2 - x3;
                ulong x4 = XorRotr( x3, 58, xor3 );
                ulong xor5 = xor3 - x4;
                ulong x5 = XorRotr( x2, 22, xor4 );
                ulong xor6 = xor4 - x5;
                ulong x6 = XorRotr( x5, 46, xor5 );
                ulong xor7 = xor5 - x6;
                ulong x7 = XorRotr( x4, 12, xor6 );
                ulong xor8 = xor6 - x7;
                ulong num5 = XorRotr( x7, 25, xor7 );
                ulong num6 = xor7 - num5;
                ulong num7 = XorRotr( x6, 33, xor8 );
                ulong num8 = xor8 - num7;
                ulong xor9 = num6 - kw[index2];
                ulong x8 = num5 - (kw[index2 + 1] + t[index3]);
                ulong xor10 = num8 - (kw[index2 + 2] + t[index3 + 1]);
                ulong x9 = XorRotr( num7 - (kw[index2 + 3] + (uint)index1), 5, xor9 );
                ulong xor11 = xor9 - x9;
                ulong x10 = XorRotr( x8, 37, xor10 );
                ulong xor12 = xor10 - x10;
                ulong x11 = XorRotr( x10, 23, xor11 );
                ulong xor13 = xor11 - x11;
                ulong x12 = XorRotr( x9, 40, xor12 );
                ulong xor14 = xor12 - x12;
                ulong x13 = XorRotr( x12, 52, xor13 );
                ulong xor15 = xor13 - x13;
                ulong x14 = XorRotr( x11, 57, xor14 );
                ulong xor16 = xor14 - x14;
                num2 = XorRotr( x14, 14, xor15 );
                num1 = xor15 - num2;
                num4 = XorRotr( x13, 16, xor16 );
                num3 = xor16 - num4;
            }
            ulong num9 = num1 - kw[0];
            ulong num10 = num2 - (kw[1] + t[0]);
            ulong num11 = num3 - (kw[2] + t[1]);
            ulong num12 = num4 - kw[3];
            state[0] = num9;
            state[1] = num10;
            state[2] = num11;
            state[3] = num12;
        }
    }

    private sealed class Threefish512Cipher : ThreefishCipher
    {
        private const int ROTATION_0_0 = 46;
        private const int ROTATION_0_1 = 36;
        private const int ROTATION_0_2 = 19;
        private const int ROTATION_0_3 = 37;
        private const int ROTATION_1_0 = 33;
        private const int ROTATION_1_1 = 27;
        private const int ROTATION_1_2 = 14;
        private const int ROTATION_1_3 = 42;
        private const int ROTATION_2_0 = 17;
        private const int ROTATION_2_1 = 49;
        private const int ROTATION_2_2 = 36;
        private const int ROTATION_2_3 = 39;
        private const int ROTATION_3_0 = 44;
        private const int ROTATION_3_1 = 9;
        private const int ROTATION_3_2 = 54;
        private const int ROTATION_3_3 = 56;
        private const int ROTATION_4_0 = 39;
        private const int ROTATION_4_1 = 30;
        private const int ROTATION_4_2 = 34;
        private const int ROTATION_4_3 = 24;
        private const int ROTATION_5_0 = 13;
        private const int ROTATION_5_1 = 50;
        private const int ROTATION_5_2 = 10;
        private const int ROTATION_5_3 = 17;
        private const int ROTATION_6_0 = 25;
        private const int ROTATION_6_1 = 29;
        private const int ROTATION_6_2 = 39;
        private const int ROTATION_6_3 = 43;
        private const int ROTATION_7_0 = 8;
        private const int ROTATION_7_1 = 35;
        private const int ROTATION_7_2 = 56;
        private const int ROTATION_7_3 = 22;

        internal Threefish512Cipher( ulong[] kw, ulong[] t )
          : base( kw, t )
        {
        }

        internal override void EncryptBlock( ulong[] block, ulong[] outWords )
        {
            ulong[] kw = this.kw;
            ulong[] t = this.t;
            int[] moD9 = MOD9;
            int[] moD3 = MOD3;
            if (kw.Length != 17)
                throw new ArgumentException();
            if (t.Length != 5)
                throw new ArgumentException();
            ulong num1 = block[0];
            ulong num2 = block[1];
            ulong num3 = block[2];
            ulong num4 = block[3];
            ulong num5 = block[4];
            ulong num6 = block[5];
            ulong num7 = block[6];
            ulong num8 = block[7];
            ulong num9 = num1 + kw[0];
            ulong x1 = num2 + kw[1];
            ulong num10 = num3 + kw[2];
            ulong x2 = num4 + kw[3];
            ulong num11 = num5 + kw[4];
            ulong x3 = num6 + kw[5] + t[0];
            ulong num12 = num7 + kw[6] + t[1];
            ulong x4 = num8 + kw[7];
            for (int index1 = 1; index1 < 18; index1 += 2)
            {
                int index2 = moD9[index1];
                int index3 = moD3[index1];
                ulong num13;
                ulong x5 = RotlXor( x1, 46, num13 = num9 + x1 );
                ulong num14;
                ulong x6 = RotlXor( x2, 36, num14 = num10 + x2 );
                ulong num15;
                ulong x7 = RotlXor( x3, 19, num15 = num11 + x3 );
                ulong num16;
                ulong x8 = RotlXor( x4, 37, num16 = num12 + x4 );
                ulong num17;
                ulong x9 = RotlXor( x5, 33, num17 = num14 + x5 );
                ulong num18;
                ulong x10 = RotlXor( x8, 27, num18 = num15 + x8 );
                ulong num19;
                ulong x11 = RotlXor( x7, 14, num19 = num16 + x7 );
                ulong num20;
                ulong x12 = RotlXor( x6, 42, num20 = num13 + x6 );
                ulong num21;
                ulong x13 = RotlXor( x9, 17, num21 = num18 + x9 );
                ulong num22;
                ulong x14 = RotlXor( x12, 49, num22 = num19 + x12 );
                ulong num23;
                ulong x15 = RotlXor( x11, 36, num23 = num20 + x11 );
                ulong num24;
                ulong x16 = RotlXor( x10, 39, num24 = num17 + x10 );
                ulong num25;
                ulong num26 = RotlXor( x13, 44, num25 = num22 + x13 );
                ulong num27;
                ulong num28 = RotlXor( x16, 9, num27 = num23 + x16 );
                ulong num29;
                ulong num30 = RotlXor( x15, 54, num29 = num24 + x15 );
                ulong num31;
                ulong num32 = RotlXor( x14, 56, num31 = num21 + x14 );
                ulong num33 = num27 + kw[index2];
                ulong x17 = num26 + kw[index2 + 1];
                ulong num34 = num29 + kw[index2 + 2];
                ulong x18 = num32 + kw[index2 + 3];
                ulong num35 = num31 + kw[index2 + 4];
                ulong x19 = num30 + kw[index2 + 5] + t[index3];
                ulong num36 = num25 + kw[index2 + 6] + t[index3 + 1];
                ulong x20 = num28 + kw[index2 + 7] + (uint)index1;
                ulong num37;
                ulong x21 = RotlXor( x17, 39, num37 = num33 + x17 );
                ulong num38;
                ulong x22 = RotlXor( x18, 30, num38 = num34 + x18 );
                ulong num39;
                ulong x23 = RotlXor( x19, 34, num39 = num35 + x19 );
                ulong num40;
                ulong x24 = RotlXor( x20, 24, num40 = num36 + x20 );
                ulong num41;
                ulong x25 = RotlXor( x21, 13, num41 = num38 + x21 );
                ulong num42;
                ulong x26 = RotlXor( x24, 50, num42 = num39 + x24 );
                ulong num43;
                ulong x27 = RotlXor( x23, 10, num43 = num40 + x23 );
                ulong num44;
                ulong x28 = RotlXor( x22, 17, num44 = num37 + x22 );
                ulong num45;
                ulong x29 = RotlXor( x25, 25, num45 = num42 + x25 );
                ulong num46;
                ulong x30 = RotlXor( x28, 29, num46 = num43 + x28 );
                ulong num47;
                ulong x31 = RotlXor( x27, 39, num47 = num44 + x27 );
                ulong num48;
                ulong x32 = RotlXor( x26, 43, num48 = num41 + x26 );
                ulong num49;
                ulong num50 = RotlXor( x29, 8, num49 = num46 + x29 );
                ulong num51;
                ulong num52 = RotlXor( x32, 35, num51 = num47 + x32 );
                ulong num53;
                ulong num54 = RotlXor( x31, 56, num53 = num48 + x31 );
                ulong num55;
                ulong num56 = RotlXor( x30, 22, num55 = num45 + x30 );
                num9 = num51 + kw[index2 + 1];
                x1 = num50 + kw[index2 + 2];
                num10 = num53 + kw[index2 + 3];
                x2 = num56 + kw[index2 + 4];
                num11 = num55 + kw[index2 + 5];
                x3 = num54 + kw[index2 + 6] + t[index3 + 1];
                num12 = num49 + kw[index2 + 7] + t[index3 + 2];
                x4 = num52 + (ulong)((long)kw[index2 + 8] + (uint)index1 + 1L);
            }
            outWords[0] = num9;
            outWords[1] = x1;
            outWords[2] = num10;
            outWords[3] = x2;
            outWords[4] = num11;
            outWords[5] = x3;
            outWords[6] = num12;
            outWords[7] = x4;
        }

        internal override void DecryptBlock( ulong[] block, ulong[] state )
        {
            ulong[] kw = this.kw;
            ulong[] t = this.t;
            int[] moD9 = MOD9;
            int[] moD3 = MOD3;
            if (kw.Length != 17)
                throw new ArgumentException();
            if (t.Length != 5)
                throw new ArgumentException();
            ulong num1 = block[0];
            ulong num2 = block[1];
            ulong num3 = block[2];
            ulong num4 = block[3];
            ulong num5 = block[4];
            ulong num6 = block[5];
            ulong num7 = block[6];
            ulong num8 = block[7];
            for (int index1 = 17; index1 >= 1; index1 -= 2)
            {
                int index2 = moD9[index1];
                int index3 = moD3[index1];
                ulong xor1 = num1 - kw[index2 + 1];
                ulong x1 = num2 - kw[index2 + 2];
                ulong xor2 = num3 - kw[index2 + 3];
                ulong x2 = num4 - kw[index2 + 4];
                ulong xor3 = num5 - kw[index2 + 5];
                ulong x3 = num6 - (kw[index2 + 6] + t[index3 + 1]);
                ulong xor4 = num7 - (kw[index2 + 7] + t[index3 + 2]);
                ulong x4 = num8 - (ulong)((long)kw[index2 + 8] + (uint)index1 + 1L);
                ulong x5 = XorRotr( x1, 8, xor4 );
                ulong xor5 = xor4 - x5;
                ulong x6 = XorRotr( x4, 35, xor1 );
                ulong xor6 = xor1 - x6;
                ulong x7 = XorRotr( x3, 56, xor2 );
                ulong xor7 = xor2 - x7;
                ulong x8 = XorRotr( x2, 22, xor3 );
                ulong xor8 = xor3 - x8;
                ulong x9 = XorRotr( x5, 25, xor8 );
                ulong xor9 = xor8 - x9;
                ulong x10 = XorRotr( x8, 29, xor5 );
                ulong xor10 = xor5 - x10;
                ulong x11 = XorRotr( x7, 39, xor6 );
                ulong xor11 = xor6 - x11;
                ulong x12 = XorRotr( x6, 43, xor7 );
                ulong xor12 = xor7 - x12;
                ulong x13 = XorRotr( x9, 13, xor12 );
                ulong xor13 = xor12 - x13;
                ulong x14 = XorRotr( x12, 50, xor9 );
                ulong xor14 = xor9 - x14;
                ulong x15 = XorRotr( x11, 10, xor10 );
                ulong xor15 = xor10 - x15;
                ulong x16 = XorRotr( x10, 17, xor11 );
                ulong xor16 = xor11 - x16;
                ulong num9 = XorRotr( x13, 39, xor16 );
                ulong num10 = xor16 - num9;
                ulong num11 = XorRotr( x16, 30, xor13 );
                ulong num12 = xor13 - num11;
                ulong num13 = XorRotr( x15, 34, xor14 );
                ulong num14 = xor14 - num13;
                ulong num15 = XorRotr( x14, 24, xor15 );
                ulong num16 = xor15 - num15;
                ulong xor17 = num10 - kw[index2];
                ulong x17 = num9 - kw[index2 + 1];
                ulong xor18 = num12 - kw[index2 + 2];
                ulong x18 = num11 - kw[index2 + 3];
                ulong xor19 = num14 - kw[index2 + 4];
                ulong x19 = num13 - (kw[index2 + 5] + t[index3]);
                ulong xor20 = num16 - (kw[index2 + 6] + t[index3 + 1]);
                ulong x20 = num15 - (kw[index2 + 7] + (uint)index1);
                ulong x21 = XorRotr( x17, 44, xor20 );
                ulong xor21 = xor20 - x21;
                ulong x22 = XorRotr( x20, 9, xor17 );
                ulong xor22 = xor17 - x22;
                ulong x23 = XorRotr( x19, 54, xor18 );
                ulong xor23 = xor18 - x23;
                ulong x24 = XorRotr( x18, 56, xor19 );
                ulong xor24 = xor19 - x24;
                ulong x25 = XorRotr( x21, 17, xor24 );
                ulong xor25 = xor24 - x25;
                ulong x26 = XorRotr( x24, 49, xor21 );
                ulong xor26 = xor21 - x26;
                ulong x27 = XorRotr( x23, 36, xor22 );
                ulong xor27 = xor22 - x27;
                ulong x28 = XorRotr( x22, 39, xor23 );
                ulong xor28 = xor23 - x28;
                ulong x29 = XorRotr( x25, 33, xor28 );
                ulong xor29 = xor28 - x29;
                ulong x30 = XorRotr( x28, 27, xor25 );
                ulong xor30 = xor25 - x30;
                ulong x31 = XorRotr( x27, 14, xor26 );
                ulong xor31 = xor26 - x31;
                ulong x32 = XorRotr( x26, 42, xor27 );
                ulong xor32 = xor27 - x32;
                num2 = XorRotr( x29, 46, xor32 );
                num1 = xor32 - num2;
                num4 = XorRotr( x32, 36, xor29 );
                num3 = xor29 - num4;
                num6 = XorRotr( x31, 19, xor30 );
                num5 = xor30 - num6;
                num8 = XorRotr( x30, 37, xor31 );
                num7 = xor31 - num8;
            }
            ulong num17 = num1 - kw[0];
            ulong num18 = num2 - kw[1];
            ulong num19 = num3 - kw[2];
            ulong num20 = num4 - kw[3];
            ulong num21 = num5 - kw[4];
            ulong num22 = num6 - (kw[5] + t[0]);
            ulong num23 = num7 - (kw[6] + t[1]);
            ulong num24 = num8 - kw[7];
            state[0] = num17;
            state[1] = num18;
            state[2] = num19;
            state[3] = num20;
            state[4] = num21;
            state[5] = num22;
            state[6] = num23;
            state[7] = num24;
        }
    }

    private sealed class Threefish1024Cipher : ThreefishCipher
    {
        private const int ROTATION_0_0 = 24;
        private const int ROTATION_0_1 = 13;
        private const int ROTATION_0_2 = 8;
        private const int ROTATION_0_3 = 47;
        private const int ROTATION_0_4 = 8;
        private const int ROTATION_0_5 = 17;
        private const int ROTATION_0_6 = 22;
        private const int ROTATION_0_7 = 37;
        private const int ROTATION_1_0 = 38;
        private const int ROTATION_1_1 = 19;
        private const int ROTATION_1_2 = 10;
        private const int ROTATION_1_3 = 55;
        private const int ROTATION_1_4 = 49;
        private const int ROTATION_1_5 = 18;
        private const int ROTATION_1_6 = 23;
        private const int ROTATION_1_7 = 52;
        private const int ROTATION_2_0 = 33;
        private const int ROTATION_2_1 = 4;
        private const int ROTATION_2_2 = 51;
        private const int ROTATION_2_3 = 13;
        private const int ROTATION_2_4 = 34;
        private const int ROTATION_2_5 = 41;
        private const int ROTATION_2_6 = 59;
        private const int ROTATION_2_7 = 17;
        private const int ROTATION_3_0 = 5;
        private const int ROTATION_3_1 = 20;
        private const int ROTATION_3_2 = 48;
        private const int ROTATION_3_3 = 41;
        private const int ROTATION_3_4 = 47;
        private const int ROTATION_3_5 = 28;
        private const int ROTATION_3_6 = 16;
        private const int ROTATION_3_7 = 25;
        private const int ROTATION_4_0 = 41;
        private const int ROTATION_4_1 = 9;
        private const int ROTATION_4_2 = 37;
        private const int ROTATION_4_3 = 31;
        private const int ROTATION_4_4 = 12;
        private const int ROTATION_4_5 = 47;
        private const int ROTATION_4_6 = 44;
        private const int ROTATION_4_7 = 30;
        private const int ROTATION_5_0 = 16;
        private const int ROTATION_5_1 = 34;
        private const int ROTATION_5_2 = 56;
        private const int ROTATION_5_3 = 51;
        private const int ROTATION_5_4 = 4;
        private const int ROTATION_5_5 = 53;
        private const int ROTATION_5_6 = 42;
        private const int ROTATION_5_7 = 41;
        private const int ROTATION_6_0 = 31;
        private const int ROTATION_6_1 = 44;
        private const int ROTATION_6_2 = 47;
        private const int ROTATION_6_3 = 46;
        private const int ROTATION_6_4 = 19;
        private const int ROTATION_6_5 = 42;
        private const int ROTATION_6_6 = 44;
        private const int ROTATION_6_7 = 25;
        private const int ROTATION_7_0 = 9;
        private const int ROTATION_7_1 = 48;
        private const int ROTATION_7_2 = 35;
        private const int ROTATION_7_3 = 52;
        private const int ROTATION_7_4 = 23;
        private const int ROTATION_7_5 = 31;
        private const int ROTATION_7_6 = 37;
        private const int ROTATION_7_7 = 20;

        public Threefish1024Cipher( ulong[] kw, ulong[] t )
          : base( kw, t )
        {
        }

        internal override void EncryptBlock( ulong[] block, ulong[] outWords )
        {
            ulong[] kw = this.kw;
            ulong[] t = this.t;
            int[] moD17 = MOD17;
            int[] moD3 = MOD3;
            if (kw.Length != 33)
                throw new ArgumentException();
            if (t.Length != 5)
                throw new ArgumentException();
            ulong num1 = block[0];
            ulong num2 = block[1];
            ulong num3 = block[2];
            ulong num4 = block[3];
            ulong num5 = block[4];
            ulong num6 = block[5];
            ulong num7 = block[6];
            ulong num8 = block[7];
            ulong num9 = block[8];
            ulong num10 = block[9];
            ulong num11 = block[10];
            ulong num12 = block[11];
            ulong num13 = block[12];
            ulong num14 = block[13];
            ulong num15 = block[14];
            ulong num16 = block[15];
            ulong num17 = num1 + kw[0];
            ulong x1 = num2 + kw[1];
            ulong num18 = num3 + kw[2];
            ulong x2 = num4 + kw[3];
            ulong num19 = num5 + kw[4];
            ulong x3 = num6 + kw[5];
            ulong num20 = num7 + kw[6];
            ulong x4 = num8 + kw[7];
            ulong num21 = num9 + kw[8];
            ulong x5 = num10 + kw[9];
            ulong num22 = num11 + kw[10];
            ulong x6 = num12 + kw[11];
            ulong num23 = num13 + kw[12];
            ulong x7 = num14 + kw[13] + t[0];
            ulong num24 = num15 + kw[14] + t[1];
            ulong x8 = num16 + kw[15];
            for (int index1 = 1; index1 < 20; index1 += 2)
            {
                int index2 = moD17[index1];
                int index3 = moD3[index1];
                ulong num25;
                ulong x9 = RotlXor( x1, 24, num25 = num17 + x1 );
                ulong num26;
                ulong x10 = RotlXor( x2, 13, num26 = num18 + x2 );
                ulong num27;
                ulong x11 = RotlXor( x3, 8, num27 = num19 + x3 );
                ulong num28;
                ulong x12 = RotlXor( x4, 47, num28 = num20 + x4 );
                ulong num29;
                ulong x13 = RotlXor( x5, 8, num29 = num21 + x5 );
                ulong num30;
                ulong x14 = RotlXor( x6, 17, num30 = num22 + x6 );
                ulong num31;
                ulong x15 = RotlXor( x7, 22, num31 = num23 + x7 );
                ulong num32;
                ulong x16 = RotlXor( x8, 37, num32 = num24 + x8 );
                ulong num33;
                ulong x17 = RotlXor( x13, 38, num33 = num25 + x13 );
                ulong num34;
                ulong x18 = RotlXor( x15, 19, num34 = num26 + x15 );
                ulong num35;
                ulong x19 = RotlXor( x14, 10, num35 = num28 + x14 );
                ulong num36;
                ulong x20 = RotlXor( x16, 55, num36 = num27 + x16 );
                ulong num37;
                ulong x21 = RotlXor( x12, 49, num37 = num30 + x12 );
                ulong num38;
                ulong x22 = RotlXor( x10, 18, num38 = num31 + x10 );
                ulong num39;
                ulong x23 = RotlXor( x11, 23, num39 = num32 + x11 );
                ulong num40;
                ulong x24 = RotlXor( x9, 52, num40 = num29 + x9 );
                ulong num41;
                ulong x25 = RotlXor( x21, 33, num41 = num33 + x21 );
                ulong num42;
                ulong x26 = RotlXor( x23, 4, num42 = num34 + x23 );
                ulong num43;
                ulong x27 = RotlXor( x22, 51, num43 = num36 + x22 );
                ulong num44;
                ulong x28 = RotlXor( x24, 13, num44 = num35 + x24 );
                ulong num45;
                ulong x29 = RotlXor( x20, 34, num45 = num38 + x20 );
                ulong num46;
                ulong x30 = RotlXor( x18, 41, num46 = num39 + x18 );
                ulong num47;
                ulong x31 = RotlXor( x19, 59, num47 = num40 + x19 );
                ulong num48;
                ulong x32 = RotlXor( x17, 17, num48 = num37 + x17 );
                ulong num49;
                ulong num50 = RotlXor( x29, 5, num49 = num41 + x29 );
                ulong num51;
                ulong num52 = RotlXor( x31, 20, num51 = num42 + x31 );
                ulong num53;
                ulong num54 = RotlXor( x30, 48, num53 = num44 + x30 );
                ulong num55;
                ulong num56 = RotlXor( x32, 41, num55 = num43 + x32 );
                ulong num57;
                ulong num58 = RotlXor( x28, 47, num57 = num46 + x28 );
                ulong num59;
                ulong num60 = RotlXor( x26, 28, num59 = num47 + x26 );
                ulong num61;
                ulong num62 = RotlXor( x27, 16, num61 = num48 + x27 );
                ulong num63;
                ulong num64 = RotlXor( x25, 25, num63 = num45 + x25 );
                ulong num65 = num49 + kw[index2];
                ulong x33 = num58 + kw[index2 + 1];
                ulong num66 = num51 + kw[index2 + 2];
                ulong x34 = num62 + kw[index2 + 3];
                ulong num67 = num55 + kw[index2 + 4];
                ulong x35 = num60 + kw[index2 + 5];
                ulong num68 = num53 + kw[index2 + 6];
                ulong x36 = num64 + kw[index2 + 7];
                ulong num69 = num59 + kw[index2 + 8];
                ulong x37 = num56 + kw[index2 + 9];
                ulong num70 = num61 + kw[index2 + 10];
                ulong x38 = num52 + kw[index2 + 11];
                ulong num71 = num63 + kw[index2 + 12];
                ulong x39 = num54 + kw[index2 + 13] + t[index3];
                ulong num72 = num57 + kw[index2 + 14] + t[index3 + 1];
                ulong x40 = num50 + kw[index2 + 15] + (uint)index1;
                ulong num73;
                ulong x41 = RotlXor( x33, 41, num73 = num65 + x33 );
                ulong num74;
                ulong x42 = RotlXor( x34, 9, num74 = num66 + x34 );
                ulong num75;
                ulong x43 = RotlXor( x35, 37, num75 = num67 + x35 );
                ulong num76;
                ulong x44 = RotlXor( x36, 31, num76 = num68 + x36 );
                ulong num77;
                ulong x45 = RotlXor( x37, 12, num77 = num69 + x37 );
                ulong num78;
                ulong x46 = RotlXor( x38, 47, num78 = num70 + x38 );
                ulong num79;
                ulong x47 = RotlXor( x39, 44, num79 = num71 + x39 );
                ulong num80;
                ulong x48 = RotlXor( x40, 30, num80 = num72 + x40 );
                ulong num81;
                ulong x49 = RotlXor( x45, 16, num81 = num73 + x45 );
                ulong num82;
                ulong x50 = RotlXor( x47, 34, num82 = num74 + x47 );
                ulong num83;
                ulong x51 = RotlXor( x46, 56, num83 = num76 + x46 );
                ulong num84;
                ulong x52 = RotlXor( x48, 51, num84 = num75 + x48 );
                ulong num85;
                ulong x53 = RotlXor( x44, 4, num85 = num78 + x44 );
                ulong num86;
                ulong x54 = RotlXor( x42, 53, num86 = num79 + x42 );
                ulong num87;
                ulong x55 = RotlXor( x43, 42, num87 = num80 + x43 );
                ulong num88;
                ulong x56 = RotlXor( x41, 41, num88 = num77 + x41 );
                ulong num89;
                ulong x57 = RotlXor( x53, 31, num89 = num81 + x53 );
                ulong num90;
                ulong x58 = RotlXor( x55, 44, num90 = num82 + x55 );
                ulong num91;
                ulong x59 = RotlXor( x54, 47, num91 = num84 + x54 );
                ulong num92;
                ulong x60 = RotlXor( x56, 46, num92 = num83 + x56 );
                ulong num93;
                ulong x61 = RotlXor( x52, 19, num93 = num86 + x52 );
                ulong num94;
                ulong x62 = RotlXor( x50, 42, num94 = num87 + x50 );
                ulong num95;
                ulong x63 = RotlXor( x51, 44, num95 = num88 + x51 );
                ulong num96;
                ulong x64 = RotlXor( x49, 25, num96 = num85 + x49 );
                ulong num97;
                ulong num98 = RotlXor( x61, 9, num97 = num89 + x61 );
                ulong num99;
                ulong num100 = RotlXor( x63, 48, num99 = num90 + x63 );
                ulong num101;
                ulong num102 = RotlXor( x62, 35, num101 = num92 + x62 );
                ulong num103;
                ulong num104 = RotlXor( x64, 52, num103 = num91 + x64 );
                ulong num105;
                ulong num106 = RotlXor( x60, 23, num105 = num94 + x60 );
                ulong num107;
                ulong num108 = RotlXor( x58, 31, num107 = num95 + x58 );
                ulong num109;
                ulong num110 = RotlXor( x59, 37, num109 = num96 + x59 );
                ulong num111;
                ulong num112 = RotlXor( x57, 20, num111 = num93 + x57 );
                num17 = num97 + kw[index2 + 1];
                x1 = num106 + kw[index2 + 2];
                num18 = num99 + kw[index2 + 3];
                x2 = num110 + kw[index2 + 4];
                num19 = num103 + kw[index2 + 5];
                x3 = num108 + kw[index2 + 6];
                num20 = num101 + kw[index2 + 7];
                x4 = num112 + kw[index2 + 8];
                num21 = num107 + kw[index2 + 9];
                x5 = num104 + kw[index2 + 10];
                num22 = num109 + kw[index2 + 11];
                x6 = num100 + kw[index2 + 12];
                num23 = num111 + kw[index2 + 13];
                x7 = num102 + kw[index2 + 14] + t[index3 + 1];
                num24 = num105 + kw[index2 + 15] + t[index3 + 2];
                x8 = num98 + (ulong)((long)kw[index2 + 16] + (uint)index1 + 1L);
            }
            outWords[0] = num17;
            outWords[1] = x1;
            outWords[2] = num18;
            outWords[3] = x2;
            outWords[4] = num19;
            outWords[5] = x3;
            outWords[6] = num20;
            outWords[7] = x4;
            outWords[8] = num21;
            outWords[9] = x5;
            outWords[10] = num22;
            outWords[11] = x6;
            outWords[12] = num23;
            outWords[13] = x7;
            outWords[14] = num24;
            outWords[15] = x8;
        }

        internal override void DecryptBlock( ulong[] block, ulong[] state )
        {
            ulong[] kw = this.kw;
            ulong[] t = this.t;
            int[] moD17 = MOD17;
            int[] moD3 = MOD3;
            if (kw.Length != 33)
                throw new ArgumentException();
            if (t.Length != 5)
                throw new ArgumentException();
            ulong num1 = block[0];
            ulong num2 = block[1];
            ulong num3 = block[2];
            ulong num4 = block[3];
            ulong num5 = block[4];
            ulong num6 = block[5];
            ulong num7 = block[6];
            ulong num8 = block[7];
            ulong num9 = block[8];
            ulong num10 = block[9];
            ulong num11 = block[10];
            ulong num12 = block[11];
            ulong num13 = block[12];
            ulong num14 = block[13];
            ulong num15 = block[14];
            ulong num16 = block[15];
            for (int index1 = 19; index1 >= 1; index1 -= 2)
            {
                int index2 = moD17[index1];
                int index3 = moD3[index1];
                ulong xor1 = num1 - kw[index2 + 1];
                ulong x1 = num2 - kw[index2 + 2];
                ulong xor2 = num3 - kw[index2 + 3];
                ulong x2 = num4 - kw[index2 + 4];
                ulong xor3 = num5 - kw[index2 + 5];
                ulong x3 = num6 - kw[index2 + 6];
                ulong xor4 = num7 - kw[index2 + 7];
                ulong x4 = num8 - kw[index2 + 8];
                ulong xor5 = num9 - kw[index2 + 9];
                ulong x5 = num10 - kw[index2 + 10];
                ulong xor6 = num11 - kw[index2 + 11];
                ulong x6 = num12 - kw[index2 + 12];
                ulong xor7 = num13 - kw[index2 + 13];
                ulong x7 = num14 - (kw[index2 + 14] + t[index3 + 1]);
                ulong xor8 = num15 - (kw[index2 + 15] + t[index3 + 2]);
                ulong x8 = XorRotr( num16 - (ulong)((long)kw[index2 + 16] + (uint)index1 + 1L), 9, xor1 );
                ulong xor9 = xor1 - x8;
                ulong x9 = XorRotr( x6, 48, xor2 );
                ulong xor10 = xor2 - x9;
                ulong x10 = XorRotr( x7, 35, xor4 );
                ulong xor11 = xor4 - x10;
                ulong x11 = XorRotr( x5, 52, xor3 );
                ulong xor12 = xor3 - x11;
                ulong x12 = XorRotr( x1, 23, xor8 );
                ulong xor13 = xor8 - x12;
                ulong x13 = XorRotr( x3, 31, xor5 );
                ulong xor14 = xor5 - x13;
                ulong x14 = XorRotr( x2, 37, xor6 );
                ulong xor15 = xor6 - x14;
                ulong x15 = XorRotr( x4, 20, xor7 );
                ulong xor16 = xor7 - x15;
                ulong x16 = XorRotr( x15, 31, xor9 );
                ulong xor17 = xor9 - x16;
                ulong x17 = XorRotr( x13, 44, xor10 );
                ulong xor18 = xor10 - x17;
                ulong x18 = XorRotr( x14, 47, xor12 );
                ulong xor19 = xor12 - x18;
                ulong x19 = XorRotr( x12, 46, xor11 );
                ulong xor20 = xor11 - x19;
                ulong x20 = XorRotr( x8, 19, xor16 );
                ulong xor21 = xor16 - x20;
                ulong x21 = XorRotr( x10, 42, xor13 );
                ulong xor22 = xor13 - x21;
                ulong x22 = XorRotr( x9, 44, xor14 );
                ulong xor23 = xor14 - x22;
                ulong x23 = XorRotr( x11, 25, xor15 );
                ulong xor24 = xor15 - x23;
                ulong x24 = XorRotr( x23, 16, xor17 );
                ulong xor25 = xor17 - x24;
                ulong x25 = XorRotr( x21, 34, xor18 );
                ulong xor26 = xor18 - x25;
                ulong x26 = XorRotr( x22, 56, xor20 );
                ulong xor27 = xor20 - x26;
                ulong x27 = XorRotr( x20, 51, xor19 );
                ulong xor28 = xor19 - x27;
                ulong x28 = XorRotr( x16, 4, xor24 );
                ulong xor29 = xor24 - x28;
                ulong x29 = XorRotr( x18, 53, xor21 );
                ulong xor30 = xor21 - x29;
                ulong x30 = XorRotr( x17, 42, xor22 );
                ulong xor31 = xor22 - x30;
                ulong x31 = XorRotr( x19, 41, xor23 );
                ulong xor32 = xor23 - x31;
                ulong num17 = XorRotr( x31, 41, xor25 );
                ulong num18 = xor25 - num17;
                ulong num19 = XorRotr( x29, 9, xor26 );
                ulong num20 = xor26 - num19;
                ulong num21 = XorRotr( x30, 37, xor28 );
                ulong num22 = xor28 - num21;
                ulong num23 = XorRotr( x28, 31, xor27 );
                ulong num24 = xor27 - num23;
                ulong num25 = XorRotr( x24, 12, xor32 );
                ulong num26 = xor32 - num25;
                ulong num27 = XorRotr( x26, 47, xor29 );
                ulong num28 = xor29 - num27;
                ulong num29 = XorRotr( x25, 44, xor30 );
                ulong num30 = xor30 - num29;
                ulong num31 = XorRotr( x27, 30, xor31 );
                ulong num32 = xor31 - num31;
                ulong xor33 = num18 - kw[index2];
                ulong x32 = num17 - kw[index2 + 1];
                ulong xor34 = num20 - kw[index2 + 2];
                ulong x33 = num19 - kw[index2 + 3];
                ulong xor35 = num22 - kw[index2 + 4];
                ulong x34 = num21 - kw[index2 + 5];
                ulong xor36 = num24 - kw[index2 + 6];
                ulong x35 = num23 - kw[index2 + 7];
                ulong xor37 = num26 - kw[index2 + 8];
                ulong x36 = num25 - kw[index2 + 9];
                ulong xor38 = num28 - kw[index2 + 10];
                ulong x37 = num27 - kw[index2 + 11];
                ulong xor39 = num30 - kw[index2 + 12];
                ulong x38 = num29 - (kw[index2 + 13] + t[index3]);
                ulong xor40 = num32 - (kw[index2 + 14] + t[index3 + 1]);
                ulong x39 = XorRotr( num31 - (kw[index2 + 15] + (uint)index1), 5, xor33 );
                ulong xor41 = xor33 - x39;
                ulong x40 = XorRotr( x37, 20, xor34 );
                ulong xor42 = xor34 - x40;
                ulong x41 = XorRotr( x38, 48, xor36 );
                ulong xor43 = xor36 - x41;
                ulong x42 = XorRotr( x36, 41, xor35 );
                ulong xor44 = xor35 - x42;
                ulong x43 = XorRotr( x32, 47, xor40 );
                ulong xor45 = xor40 - x43;
                ulong x44 = XorRotr( x34, 28, xor37 );
                ulong xor46 = xor37 - x44;
                ulong x45 = XorRotr( x33, 16, xor38 );
                ulong xor47 = xor38 - x45;
                ulong x46 = XorRotr( x35, 25, xor39 );
                ulong xor48 = xor39 - x46;
                ulong x47 = XorRotr( x46, 33, xor41 );
                ulong xor49 = xor41 - x47;
                ulong x48 = XorRotr( x44, 4, xor42 );
                ulong xor50 = xor42 - x48;
                ulong x49 = XorRotr( x45, 51, xor44 );
                ulong xor51 = xor44 - x49;
                ulong x50 = XorRotr( x43, 13, xor43 );
                ulong xor52 = xor43 - x50;
                ulong x51 = XorRotr( x39, 34, xor48 );
                ulong xor53 = xor48 - x51;
                ulong x52 = XorRotr( x41, 41, xor45 );
                ulong xor54 = xor45 - x52;
                ulong x53 = XorRotr( x40, 59, xor46 );
                ulong xor55 = xor46 - x53;
                ulong x54 = XorRotr( x42, 17, xor47 );
                ulong xor56 = xor47 - x54;
                ulong x55 = XorRotr( x54, 38, xor49 );
                ulong xor57 = xor49 - x55;
                ulong x56 = XorRotr( x52, 19, xor50 );
                ulong xor58 = xor50 - x56;
                ulong x57 = XorRotr( x53, 10, xor52 );
                ulong xor59 = xor52 - x57;
                ulong x58 = XorRotr( x51, 55, xor51 );
                ulong xor60 = xor51 - x58;
                ulong x59 = XorRotr( x47, 49, xor56 );
                ulong xor61 = xor56 - x59;
                ulong x60 = XorRotr( x49, 18, xor53 );
                ulong xor62 = xor53 - x60;
                ulong x61 = XorRotr( x48, 23, xor54 );
                ulong xor63 = xor54 - x61;
                ulong x62 = XorRotr( x50, 52, xor55 );
                ulong xor64 = xor55 - x62;
                num2 = XorRotr( x62, 24, xor57 );
                num1 = xor57 - num2;
                num4 = XorRotr( x60, 13, xor58 );
                num3 = xor58 - num4;
                num6 = XorRotr( x61, 8, xor60 );
                num5 = xor60 - num6;
                num8 = XorRotr( x59, 47, xor59 );
                num7 = xor59 - num8;
                num10 = XorRotr( x55, 8, xor64 );
                num9 = xor64 - num10;
                num12 = XorRotr( x57, 17, xor61 );
                num11 = xor61 - num12;
                num14 = XorRotr( x56, 22, xor62 );
                num13 = xor62 - num14;
                num16 = XorRotr( x58, 37, xor63 );
                num15 = xor63 - num16;
            }
            ulong num33 = num1 - kw[0];
            ulong num34 = num2 - kw[1];
            ulong num35 = num3 - kw[2];
            ulong num36 = num4 - kw[3];
            ulong num37 = num5 - kw[4];
            ulong num38 = num6 - kw[5];
            ulong num39 = num7 - kw[6];
            ulong num40 = num8 - kw[7];
            ulong num41 = num9 - kw[8];
            ulong num42 = num10 - kw[9];
            ulong num43 = num11 - kw[10];
            ulong num44 = num12 - kw[11];
            ulong num45 = num13 - kw[12];
            ulong num46 = num14 - (kw[13] + t[0]);
            ulong num47 = num15 - (kw[14] + t[1]);
            ulong num48 = num16 - kw[15];
            state[0] = num33;
            state[1] = num34;
            state[2] = num35;
            state[3] = num36;
            state[4] = num37;
            state[5] = num38;
            state[6] = num39;
            state[7] = num40;
            state[8] = num41;
            state[9] = num42;
            state[10] = num43;
            state[11] = num44;
            state[12] = num45;
            state[13] = num46;
            state[14] = num47;
            state[15] = num48;
        }
    }
}
[Serializable]
public class DataLengthException : CryptoException
{
    public DataLengthException()
    {
    }

    public DataLengthException( string message )
      : base( message )
    {
    }

    public DataLengthException( string message, Exception exception )
      : base( message, exception )
    {
    }
}
[Serializable]
public class CryptoException : Exception
{
    public CryptoException()
    {
    }

    public CryptoException( string message )
      : base( message )
    {
    }

    public CryptoException( string message, Exception exception )
      : base( message, exception )
    {
    }
}
public class TigerDigest : IDigest, IMemoable
{
    private const int MyByteLength = 64;
    private const int DigestLength = 24;
    private static readonly long[] t1 = new long[256]
    {
      192161084409973854L,
      -6034178070669973268L,
      8272369121297300691L,
      7854730284916899642L,
      -3631738584360316525L,
      8463286011307239906L,
      -5664346993730092093L,
      5082381371487377520L,
      -1536603760329757466L,
      -4232985935611735204L,
      5541490850629862524L,
      766444128913191948L,
      1204553577021685498L,
      -4121719295987045526L,
      1401289229890216703L,
      1893918052108309022L,
      5461170853188208586L,
      2807403890869420487L,
      -8822417684582283338L,
      5699452412975025298L,
      -2914262034798377397L,
      -8199292901130911363L,
      7624427211800470465L,
      -5330070367527189138L,
      9043806901924967914L,
      7231827479902542914L,
      -4667804575905660192L,
      6875646691050945796L,
      -954047427515838778L,
      7786398710221814956L,
      8167597339425066981L,
      1830707105885056415L,
      -192929137551915557L,
      -4000909680243679221L,
      -8790383730744944306L,
      -6559119868654993229L,
      -8046943608939121133L,
      -2635222011098072079L,
      1783120314242633559L,
      248005612187258982L,
      7688500634458409525L,
      -799055769434250085L,
      8591138587399736033L,
      -2813706756098348539L,
      -4803442773389201549L,
      5042603696143252264L,
      2053990370701680515L,
      -8434990628116389527L,
      3741955435321465241L,
      4334407786093429776L,
      -5399798173115342087L,
      1449859124008718907L,
      -259597992345095852L,
      -2299784421946890745L,
      -8624947886301142065L,
      -7850603641235491331L,
      3847074041673952000L,
      4649400157396704725L,
      -4273499526689310132L,
      -3840742565288711634L,
      2909491499011162061L,
      4458122598401901638L,
      7071481730398905774L,
      6725294491764459774L,
      -6201551736110472662L,
      -4372530048007926361L,
      1226483701329067140L,
      -2522035007050864557L,
      -3676115808446124170L,
      -4975751036383735295L,
      -1831728144282101387L,
      -7732658914112356844L,
      479582384021555544L,
      8040612334407127321L,
      -2798227069691230528L,
      -1334228551670664750L,
      8751740296797632830L,
      6603430683508552489L,
      8942924799792477540L,
      3573742753214737511L,
      -2419519573825602302L,
      6349030933445924429L,
      -2501945979441900175L,
      -6177453506703404958L,
      -7885857697280165792L,
      5194369709296555225L,
      7174555471952375656L,
      7982812746821821468L,
      -8707669106532426453L,
      3232013613859041307L,
      -5747376245209101971L,
      -2231459388012946321L,
      3112410413624570453L,
      -2336602742119691332L,
      6658792778814911418L,
      6126246269502162262L,
      -6070952467612144753L,
      4721051187472420532L,
      -5533619424469951182L,
      -4853025588084287359L,
      2663576151211431276L,
      928112258657309258L,
      5664920977038299994L,
      2704699625848084345L,
      2312925355491498803L,
      -528812816973409076L,
      2964761606854114992L,
      4148718494125202372L,
      4082542483235864459L,
      5171535286737311423L,
      2166137813939512309L,
      8844224567096109974L,
      -6373247044080797239L,
      -8133614489572350707L,
      7053919794999990929L,
      5576291611870337032L,
      -1374825740467639573L,
      -734453569254161202L,
      -705972172313107935L,
      -6688726126811769884L,
      -7468621655906046812L,
      -3527580439205474383L,
      -6956282119872554589L,
      -6281089153129775081L,
      853355433004222246L,
      -1924221946255713479L,
      2124075034376372323L,
      5881355904936746717L,
      1033318428544969251L,
      1692585388818821524L,
      -1245985052454466526L,
      1107424405919510210L,
      -9211670503851965599L,
      -5975256720516651978L,
      963191604767572015L,
      4506934758573727688L,
      -6511972687387035778L,
      -6714534832456272315L,
      7421261837586505858L,
      3318186242040429129L,
      -4402061108394378299L,
      1910808081503L,
      4771413979138012118L,
      -3357965141731676491L,
      -6811660122601107496L,
      3247421105326436348L,
      -1009844908434318049L,
      8353265116968520410L,
      -5881406294935394735L,
      -7574869783018555510L,
      6528592316425799439L,
      -3049672598698961616L,
      -3303981966096002009L,
      7320455443630736945L,
      -7351974990356818097L,
      2539802313181221187L,
      -7307523792611951465L,
      6084456898448652712L,
      1615327116689102472L,
      8126548348642832045L,
      -1094214848903295726L,
      6320848846662414801L,
      -1163799684465161365L,
      3439926484095136410L,
      -7218302546559918104L,
      4583261464596863494L,
      5278432013075676693L,
      672210957064462075L,
      -5420889727701263133L,
      -3948047341652367807L,
      3753742208681096767L,
      -5185515461782971584L,
      -460252340867529358L,
      111470777923844445L,
      1951374535466601971L,
      -8875343681432095955L,
      -4493729248843343338L,
      4830799035278983864L,
      -5224728565293047538L,
      6842302225500364445L,
      -7111193868311747516L,
      -2729919277420993032L,
      -5582278241003401657L,
      -126421769187551098L,
      -4035721366655415313L,
      -1986169280154305277L,
      3977519900599801820L,
      9148781857317432677L,
      6468933130968205401L,
      8516219711084257782L,
      1539015908620793624L,
      7527026033758878374L,
      -1647949680688450337L,
      3088835283432281588L,
      3651919061693825289L,
      -8985256062000155568L,
      -423165018983337331L,
      -7032056788937726985L,
      308165109378616703L,
      8884692927086426203L,
      2438838841395254149L,
      -3550173447755953499L,
      2823241734971430590L,
      3896218688877146334L,
      393786506094771122L,
      -3117973570538943511L,
      -7973569017697024389L,
      -8368763565314219996L,
      6934559736714979565L,
      -589348163057397487L,
      -7554853961030558080L,
      -6878676038788161577L,
      -3798065817641571893L,
      -9101961441510934879L,
      -4559443103670756675L,
      -7665374195348870830L,
      -8336074436196531783L,
      4236391428300945648L,
      555138268555536248L,
      5351590591369890935L,
      4306521946498657944L,
      -7151482210676895604L,
      4901816398460471456L,
      -9033789479800328823L,
      7485939926152528684L,
      -5105994143555176462L,
      6245128712556390173L,
      -4718679834244078161L,
      -325273111308121687L,
      7772052866533484500L,
      639373189613950878L,
      2515940555210603828L,
      -2058685867725021174L,
      9187445612742136046L,
      -5771987833248487369L,
      -2125811817212952004L,
      -3204735567712096048L,
      -3393897870002714342L,
      1313621308117380133L,
      3526835097255131285L,
      -4953033604042954265L,
      8704164972314360376L,
      -920137909863202916L,
      5969067443919232116L,
      5791404459833380522L,
      -1682712826007985785L,
      6001456072058810555L,
      -8273861206301250160L,
      2241175407069758350L,
      -2962551490920225208L,
      8359644330926224055L,
      -8523485772611717717L,
      -5183265553382750L,
      -1789270636298447811L,
      -6471656072873752544L,
      -1458735953920612486L
    };
    private static readonly long[] t2 = new long[256]
    {
      -1826563305001377480L,
      -5358963986493047656L,
      6213947727727520144L,
      5496303794639560149L,
      -2795981259149962188L,
      642450021946863605L,
      -2925749420550550287L,
      -4252676236223476327L,
      -2372897249057438062L,
      -2455723000952046826L,
      8011611286970690052L,
      5372247966639775667L,
      -6490268738015937967L,
      -265982677241022690L,
      -1711898199407229911L,
      -2553549223344005918L,
      -3655427155680827379L,
      1788379855404599063L,
      3792259505844355329L,
      857793142685420274L,
      2176386753693503798L,
      -2281187609587102471L,
      -12877901320348396L,
      6070247714570225101L,
      7358743242340641331L,
      -8703516059324417162L,
      1522910625901990663L,
      -2134847759353728262L,
      5235630359010597374L,
      -5774648161970196758L,
      277273466943670671L,
      3580831169916433691L,
      -1032406685548103719L,
      4657750985732713388L,
      1177149711660596421L,
      8685721698255572101L,
      -3227632359902186326L,
      -6349410231276355429L,
      -4809500581665772080L,
      -7923309769729008016L,
      -6726740716384263588L,
      -4587792071496920925L,
      -658271017113840853L,
      3834592178494549117L,
      -3853851402329989932L,
      -8865288174312808228L,
      8774750272303345432L,
      -8428026360225307604L,
      -3404183201405868250L,
      6519077675840655372L,
      1009372798613472243L,
      -4504928615151511518L,
      7670504156571609794L,
      -9068448121725124008L,
      7481699948221361317L,
      2131352009749933493L,
      7854556580946198495L,
      5848046147829288198L,
      6811751916476253359L,
      -635956774299390418L,
      -4737535235939835750L,
      -1614809042241653147L,
      8245611441321613668L,
      8087057586628171618L,
      5058061449640751271L,
      -5151918184365513026L,
      7212395796113148780L,
      8872633840395976086L,
      8602726521519041395L,
      -5885490816789515276L,
      6042660761688602872L,
      1642367900117110883L,
      25924001596622557L,
      7531865058110106323L,
      4223621278438660202L,
      3926684511422013614L,
      -2064363959953346089L,
      5939130201053773422L,
      8312208923375399755L,
      5278156969609628584L,
      -5712322089306707131L,
      3610014133393185213L,
      -8850224129823554669L,
      -7989215126425784091L,
      7953444341930717599L,
      -5072589324995998940L,
      -3677986556148923193L,
      5127306049615917691L,
      9121210965518562125L,
      8462056263389103903L,
      -743704981880018871L,
      5658738406708581754L,
      3084862250275496789L,
      -2839477530259368618L,
      -3966384508771725354L,
      -3487534071112132806L,
      -123994483119243460L,
      -1345606558677941971L,
      -8999779576894164844L,
      -4191785782441631580L,
      1116769798908306816L,
      1871732813531574911L,
      -5639228995346094013L,
      2050857069623328786L,
      942713319182180155L,
      -8555767913901511542L,
      -1938713800388260250L,
      7028952989422544417L,
      9018945159409650955L,
      -9098571702620193189L,
      512456053301416255L,
      -4053543709501018729L,
      -4330900206871259305L,
      -1512795427272957464L,
      -3102984968199159270L,
      -7389706432295929941L,
      -6638196300801425917L,
      -7112719166685012944L,
      4569666897377300404L,
      -7151449437793514816L,
      4462677101358564049L,
      3679240545963649394L,
      -4129112553160565951L,
      776201060342576796L,
      -1202834617519492059L,
      -842133208882402856L,
      -8445297248460022090L,
      3458390008116962295L,
      -8107400727032609416L,
      6618311662604863029L,
      4790267690900900096L,
      1716087693007726108L,
      4148457837926911568L,
      -5418957485852076861L,
      8968309666649857421L,
      -2611360075161572255L,
      6968029403465067289L,
      -3584187592496365262L,
      500987773930853904L,
      -8168172799095912208L,
      2355660670689429871L,
      3178293543037890097L,
      -5583593033549110520L,
      -6297125087914569009L,
      894835714693979080L,
      -5305826774090122525L,
      -348051181029808153L,
      352461093517089771L,
      5441805419015688358L,
      -3049381223523647492L,
      3501129463520285556L,
      -4980126173351398283L,
      -8303518980934164731L,
      -7446347735086057113L,
      2615208954064994172L,
      -522603252265687058L,
      2237558221535645089L,
      -3911919600557704777L,
      -5210711461681408094L,
      7102368496127332321L,
      -7719366717024918019L,
      399232473491847935L,
      7140013836546489399L,
      -8234741283244511424L,
      -2231392863125672626L,
      -7060197492102713059L,
      5038446221635409553L,
      6294769326316815049L,
      -387802090031244907L,
      -3350046130045840024L,
      -2666808022981539793L,
      -6161723600240465717L,
      2783168786742146440L,
      1986639352536355296L,
      -1988727118208302602L,
      8799325730492140254L,
      7305467695957075406L,
      2551364576700533681L,
      -6081001307066006598L,
      -4889804522683628146L,
      -7324859595388608820L,
      -6885748294050442179L,
      5760535140236403614L,
      1501217875009212803L,
      -1291632093432900094L,
      -7706153952057205239L,
      6454505253869455699L,
      4319683495060363885L,
      -6244922308576078969L,
      -6818767823778904188L,
      2960027307368769952L,
      8570410701452901115L,
      160427886842421800L,
      -4969938860820756853L,
      -4627442630994782527L,
      -3285648034072744413L,
      -7606118162332863056L,
      6176075057452006273L,
      7582622308322968760L,
      6649763778434249567L,
      -183456705028906550L,
      2699628156079216836L,
      -1767231947251866451L,
      2945653313023238585L,
      2813841150172635667L,
      8163160757531991904L,
      -7212422464109809801L,
      -5924618728816493121L,
      649720531103423106L,
      6394120152722619742L,
      -934965811117111118L,
      4753049982369101610L,
      2408845162401379802L,
      1253140645631747605L,
      -7799048643966905049L,
      -1584266091164108743L,
      -456002869645138839L,
      8367255505928917714L,
      91400768704631494L,
      -4464375255980341934L,
      1938401838693046941L,
      -7520293791609324052L,
      -8636597607271566304L,
      3990523136699180870L,
      7731749711829208666L,
      4875740361372990282L,
      9173201802070489451L,
      7834799413446679311L,
      -6433392137177717442L,
      3325271250982575439L,
      -8730608807451740020L,
      -2389358865336045484L,
      -9209652622095187875L,
      4359958813756723849L,
      4539467735137059035L,
      -5508531677782308793L,
      1312945880979454078L,
      -947428475416758718L,
      4958176066159770025L,
      1374196081931091686L,
      -6918434684938959032L,
      -1095184559281703237L,
      -1411469442470588444L,
      3145683508650593868L,
      -6039522865352658195L,
      -3804467173852034031L,
      -6563710254104815428L,
      6868326517302426863L,
      6758043032196830276L,
      5827167051130463242L,
      4074828688890126937L,
      3293442170241026694L,
      -8065760984084440343L,
      5618223731912049521L,
      -3014545685365689991L,
      2520538699101199374L
    };
    private static readonly long[] t3 = new long[256]
    {
      -819712100864953445L,
      5224129141031473793L,
      -1683494792012715969L,
      3214246200928423523L,
      -2720183745931134014L,
      3432136347919366758L,
      -6844377996819786796L,
      -4697838837464539535L,
      -3480123136110369641L,
      -5257202687841710057L,
      -3160671586143389472L,
      -8143604544638974599L,
      -7582212342885995579L,
      7399204607179264370L,
      2410740665327626235L,
      -5531319028708868287L,
      -1132011872800708955L,
      -8244108713684067595L,
      -8100030830173699490L,
      -865042824158552761L,
      -1406263208487841571L,
      -743744098937138031L,
      -7255025749313877870L,
      5293216666010209768L,
      -6686350151342941087L,
      505172698323928814L,
      -8504163865352868456L,
      -6039198373597746942L,
      2102395425312436973L,
      -1480681786698906867L,
      6364975572501938982L,
      -7035658141633266754L,
      -8022507636838873565L,
      -4480433668109774745L,
      2328871106231838244L,
      1378680973804076623L,
      -3586772320324138908L,
      -2755027987269747529L,
      7519553577929664460L,
      460638964809724379L,
      -99820877092259348L,
      6562793443469826132L,
      1580997072160885165L,
      859005579845670993L,
      -3058956174016989192L,
      -3379814835910611228L,
      -3936971176641920257L,
      -8723858077265400670L,
      3784640730692549981L,
      -2514946515147142870L,
      -718211188705137671L,
      5877026246039211124L,
      -8623573777109189598L,
      -6383628662057423219L,
      4036482174343220762L,
      -6451625591996463702L,
      -5974472282720051687L,
      -4119613249555124729L,
      -4204805774663870152L,
      1637614953354483776L,
      1768420517056302872L,
      -6063481615036972513L,
      4469119677486524438L,
      6862084742702193339L,
      2666591392741323510L,
      1958911907595193257L,
      2078226524874004819L,
      9182514826368667184L,
      -5667455777910095811L,
      -6961112304229951815L,
      7984583406477441100L,
      5152724216922222472L,
      -2011927023009527807L,
      -212234053999724107L,
      4838452819165657451L,
      -8437636414480207278L,
      -4364095106444861094L,
      -8843563141488759799L,
      -952547977505311611L,
      7192165871822020282L,
      -8957588412064574366L,
      4293149567017494192L,
      6266031685674981260L,
      3297360663327026118L,
      -7424220229153493459L,
      1848411117523063487L,
      4803542876947788811L,
      -6514007507455064743L,
      3918859449562378630L,
      7730455268829558643L,
      2300310138214025757L,
      5073098731442674389L,
      -1867327214174801803L,
      -5119713925479725192L,
      2481833961960165662L,
      3483465760582650171L,
      -3799159280037322961L,
      -2614176868807805682L,
      3683901813415452623L,
      -6586240258798896426L,
      -6280196637815307286L,
      -6878770741467980580L,
      -8649528727307138543L,
      1263269478536931145L,
      -7419991789716909164L,
      -5769815365846261236L,
      7280608515770959015L,
      7790930297845911262L,
      -5059374975740702796L,
      -6705059931318638429L,
      8900403996915095151L,
      8816891275549542045L,
      -476483339080012016L,
      -1232282160203339243L,
      3119849171172694992L,
      7662494604586420558L,
      149203013753700084L,
      5530308158539891708L,
      4143436129840869576L,
      -3411623459852687238L,
      -1026352410626214551L,
      -8324492521276276327L,
      6707891355510602429L,
      5715986277202524800L,
      -393206988093480487L,
      4600951196636466039L,
      -4593511655318796512L,
      9065747437067558111L,
      -8901650410637853864L,
      2592076422926394627L,
      228032410479194937L,
      6667480117540136779L,
      588648581915253038L,
      -2336950474993240516L,
      3634608293302267354L,
      1202024298738736502L,
      6299068367672194603L,
      1932346445954743183L,
      7573861666572117031L,
      -61815566784892605L,
      3549459440654955014L,
      8158286332358861718L,
      -7670372790848096527L,
      -515956617046547146L,
      -3963219078081420846L,
      8464707252757847009L,
      397230465775035974L,
      -4957137534187579283L,
      675316509725923312L,
      2628613740627889320L,
      -2532211618462009391L,
      5345232712238813773L,
      -4776658006885916949L,
      3062009004852183467L,
      -2381228231588757251L,
      74184876899443393L,
      -1882978417976974457L,
      9131956796466541322L,
      8604540880985875509L,
      22099178757704754L,
      -1755823172185693422L,
      -7115222264497037070L,
      2945473010562318822L,
      -3264392033958139096L,
      2789803412788518275L,
      -5023951698716947073L,
      -2879016497062593138L,
      1017933909609308228L,
      -2136777458168640962L,
      8230916861376446652L,
      -4050239832011059757L,
      8983610917420146076L,
      8543542228473779244L,
      1721876046845854392L,
      -2252284190053484385L,
      5559864569757380000L,
      4937681992884682033L,
      -5441254327629638811L,
      -9066842030330493037L,
      5670390740934713304L,
      2219071780988037499L,
      7008521987288882964L,
      6028345117330418825L,
      -7500176903196747008L,
      7071075452076274675L,
      -1604175089662029304L,
      1445978213955986826L,
      -7979034942316814172L,
      951333080223670799L,
      6099155138413436065L,
      -4305900099056973791L,
      -6236769450809946705L,
      -2912898243239114769L,
      -2065740773420267803L,
      -3827177893057145596L,
      1340472571717533606L,
      -3648363291767490877L,
      -5756567784146095673L,
      4461163794677446508L,
      -5848717005041324781L,
      3341940384398866564L,
      -4882598382547103543L,
      3829921822543532494L,
      899996630714791418L,
      6478536468284266291L,
      2994597028103565543L,
      6124895672834828926L,
      -8376542604899771579L,
      -4412652237062246342L,
      -7724700941812371646L,
      728866099714851926L,
      339635816873858970L,
      -1153572816294167456L,
      -592215260546165052L,
      -7150089944179092253L,
      8700134485486622004L,
      -5552633324984327062L,
      -1298517758115136471L,
      8749621007278605595L,
      -6133576477421907076L,
      4199955888901663150L,
      -5341432795218012713L,
      -239890188217778377L,
      8106773277103211697L,
      -2229320058079270256L,
      5930619164422717276L,
      4368075505682949467L,
      4623369983466747106L,
      8403817438537116875L,
      -5327756068839670070L,
      1151085119119418028L,
      6933250016240323664L,
      6814675599201764477L,
      -2995490164984896514L,
      5778917359701360712L,
      -7334472845550608018L,
      -9212347808668562614L,
      -7786744047088363785L,
      4025584697920591189L,
      5446500518121291045L,
      -7866665254384488512L,
      -352887593087136842L,
      8290028954029701554L,
      -9087549732707247512L,
      7234639242841923679L,
      2860911103167493259L,
      -3716770017321781837L,
      7444204691177324181L,
      8012224255291120002L,
      6549509778060988165L,
      -4656265058823564969L,
      -1532696805485516055L,
      4993489137437819341L,
      4727924503904151836L,
      -3180601338503688336L,
      7858325008468642462L
    };
    private static readonly long[] t4 = new long[256]
    {
      6561287832113134677L,
      1893413629145602549L,
      -6205320776685678598L,
      7334764389497132503L,
      421942495471316930L,
      -9085229951450268347L,
      5948965432456907277L,
      -6872877502453521409L,
      4831763938021002582L,
      -4272888574428519313L,
      5678704711006605406L,
      4536654317168965104L,
      802439540090739142L,
      1728614842704535657L,
      7852250862810361152L,
      -2970083550513149273L,
      6999787169451700297L,
      327545298748531618L,
      -2764213178345403342L,
      9213801181845131435L,
      -5950018878971805109L,
      -2186876610533351532L,
      -3100863505161590557L,
      -194921935069456237L,
      2629011484744925146L,
      679658461659738748L,
      -3068808746888436091L,
      2845612796809381245L,
      -7722098226173915145L,
      7273530125705028225L,
      4410076014410041819L,
      -2304212329100317967L,
      -45936371244098582L,
      -5712723046817425393L,
      8922873767131958175L,
      -3382299200423854708L,
      -3236816455951139535L,
      -4036747678298392505L,
      5226125132195873799L,
      2940247444995640068L,
      -4418018165041970817L,
      6671397049608501367L,
      8821388386505911040L,
      -3580187736799586652L,
      -1447046360908978430L,
      2147098610462912262L,
      -1956265881574637814L,
      -2856917834249223582L,
      5141735866072457044L,
      3265027362719053310L,
      -6450920645962515936L,
      6017965846669640613L,
      4287051124723328232L,
      8655371236021312991L,
      -1156847972119148173L,
      2365060307249772354L,
      1630631832073154105L,
      1828719980936758421L,
      2674037562503248056L,
      -7295616781251116690L,
      -1363141094472255887L,
      204405347605452144L,
      5797523068258732423L,
      8122903338174012641L,
      8739821670855295734L,
      961841682317282412L,
      3487881148722869326L,
      -7995384159388863717L,
      7665614591556333409L,
      -7831409025227614873L,
      -822907162794399275L,
      -1691135090558933875L,
      3797048810173566205L,
      -2578904300750297763L,
      -3410711173298709536L,
      577633178325057199L,
      -7379212936790430923L,
      -9035774148364232240L,
      2754939666238358593L,
      8444132705799138470L,
      -7894221632442939675L,
      3065464070595795438L,
      -6610449357786147779L,
      3184382822055416328L,
      5740274767717360273L,
      6179930651821454089L,
      -4826152258144849421L,
      5115645765347262247L,
      4602739923119569497L,
      -3465801151231271281L,
      -6359599548771540712L,
      -1926152657970122275L,
      -8468989295385802946L,
      -6500580506154635033L,
      4125629484990072616L,
      -6834670983768857044L,
      -4845179353893108027L,
      4230689665262407186L,
      -1849684427061896393L,
      9047540561879224854L,
      1112218670439199625L,
      8426162753992594376L,
      -5990769681480860131L,
      -2503790423972405993L,
      4028912247909671416L,
      -409156412951274838L,
      -8377831951645714695L,
      -1152570669068554652L,
      -6327418252815316840L,
      -3725559206061705268L,
      1964465731879646024L,
      -2441760721249263597L,
      6946242362685775318L,
      -3298979752616086841L,
      -7236283555339513389L,
      -1419193050620496778L,
      -93735727476260563L,
      -5905399081030416230L,
      2507248404937789251L,
      7581261321693772141L,
      -8836566033099333598L,
      520172056875071564L,
      3738403388662150470L,
      -2357506837776452040L,
      -5002739851233418934L,
      930169001927683533L,
      6889748805645999668L,
      -1031349426815687751L,
      7941113837267854943L,
      -1243211017071393764L,
      -2154628650105719635L,
      6332043450707792835L,
      3386824618901547762L,
      7130458179308482168L,
      1271522336860346025L,
      -997034324337437613L,
      4823850509807911142L,
      3107332511049695348L,
      5437793788182680416L,
      -8315628002795417155L,
      1494290439970088554L,
      -8609438560643873897L,
      -8207953325454440687L,
      -5432621302919780015L,
      1159256241058966379L,
      1026141471931805870L,
      -8215608786054685932L,
      -609691062749569444L,
      7511556330643118785L,
      -3915792337899679783L,
      3932170512244996561L,
      6834333685245251200L,
      4355290964656419152L,
      6487547078612259600L,
      6267880520331323438L,
      -1545475867304599653L,
      8190919284549556346L,
      3366895789332200348L,
      2444540809879438627L,
      6459524513146455969L,
      4077716903750958194L,
      -6168929569432701476L,
      -6973483665415634802L,
      -5197441416039796052L,
      7734160491610189202L,
      7910254887717195099L,
      3836881802794822270L,
      8311228008842563790L,
      730509642500215940L,
      -650400159804944995L,
      -5124223765383482859L,
      3579688877020158541L,
      8591780283260295173L,
      5028082178778891827L,
      -498814760953987530L,
      -2709709455026140056L,
      5487541034902828271L,
      8530400576707172340L,
      -7604535187505054453L,
      -869656751120750718L,
      4656569414526204412L,
      491061932033469878L,
      8035458231926703496L,
      137019260109594401L,
      7421708309958176805L,
      8223709417363553275L,
      5401705824239018731L,
      -7162608250562934562L,
      5308870500428712900L,
      -5508949737295341638L,
      1376856236535589493L,
      -5655908917112005032L,
      -7100674984259216372L,
      1332977380922036690L,
      3015788518022419172L,
      -6718854486329987908L,
      6396540069380292132L,
      2034188120276215631L,
      -1655134238111203034L,
      -509741179510489141L,
      3623665942510192329L,
      -9164935270648710301L,
      1765784450088366494L,
      5837777785993897047L,
      1564973338399864744L,
      -2605395199060435761L,
      4964475598524693274L,
      -5312043978489901415L,
      6706291041494563888L,
      -789946623649963734L,
      -8091303779971721549L,
      7456716478970921562L,
      -335263357675197259L,
      -8515348892102079999L,
      -7048796562806032069L,
      -233028078259189719L,
      284725780453796946L,
      -3832073186324226638L,
      -4921235094493811069L,
      -5089093504863659344L,
      -5607539644671350465L,
      -8911681616096439592L,
      -4743899514573401058L,
      -7664321526450198170L,
      -4599281686566632149L,
      2560491659082246267L,
      8971180328015050686L,
      2265540171276805379L,
      6093561527083620308L,
      12169565841013306L,
      9128413284208255679L,
      -4178722056535276608L,
      -8960148414521589626L,
      -4216952774774654326L,
      -5374970407177951367L,
      -6668788646589711127L,
      -2946910590031425822L,
      -8674853389405194592L,
      -7535980417822448849L,
      -6115357923114297461L,
      -8065837346967928004L,
      -7487037274649424496L,
      -2061373546992596293L,
      -5783192355322733388L,
      7153300451507295513L,
      -8779488031786375734L,
      2187906506867626476L,
      5612681432830855607L,
      -4653220181978985551L,
      4688837593722596333L,
      -3815667051463559517L,
      -1779743783662362556L,
      -3650491565905270770L,
      -4529053496248414107L,
      -4021111997381021802L,
      -4350414089199835873L
    };
    private long a;
    private long b;
    private long c;
    private long byteCount;
    private byte[] Buffer = new byte[8];
    private int bOff;
    private long[] x = new long[8];
    private int xOff;

    public TigerDigest() => Reset();

    public TigerDigest( TigerDigest t ) => Reset( t );

    public string AlgorithmName => "Tiger";

    public int GetDigestSize() => 24;

    public int GetByteLength() => 64;

    private void ProcessWord( byte[] b, int off )
    {
        x[xOff++] = ((long)(b[off + 7] & byte.MaxValue) << 56) | ((long)(b[off + 6] & byte.MaxValue) << 48) | ((long)(b[off + 5] & byte.MaxValue) << 40) | ((long)(b[off + 4] & byte.MaxValue) << 32) | ((long)(b[off + 3] & byte.MaxValue) << 24) | ((long)(b[off + 2] & byte.MaxValue) << 16) | ((long)(b[off + 1] & byte.MaxValue) << 8) | (b[off] & (uint)byte.MaxValue);
        if (xOff == x.Length)
            ProcessBlock();
        bOff = 0;
    }

    public void Update( byte input )
    {
        Buffer[bOff++] = input;
        if (bOff == Buffer.Length)
            ProcessWord( Buffer, 0 );
        ++byteCount;
    }

    public void BlockUpdate( byte[] input, int inOff, int length )
    {
        for (; bOff != 0 && length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
        while (length > 8)
        {
            ProcessWord( input, inOff );
            inOff += 8;
            length -= 8;
            byteCount += 8L;
        }
        for (; length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
    }

    private void RoundABC( long x, long mul )
    {
        c ^= x;
        a -= t1[(int)c & byte.MaxValue] ^ t2[(int)(c >> 16) & byte.MaxValue] ^ t3[(int)(c >> 32) & byte.MaxValue] ^ t4[(int)(c >> 48) & byte.MaxValue];
        b += t4[(int)(c >> 8) & byte.MaxValue] ^ t3[(int)(c >> 24) & byte.MaxValue] ^ t2[(int)(c >> 40) & byte.MaxValue] ^ t1[(int)(c >> 56) & byte.MaxValue];
        b *= mul;
    }

    private void RoundBCA( long x, long mul )
    {
        a ^= x;
        b -= t1[(int)a & byte.MaxValue] ^ t2[(int)(a >> 16) & byte.MaxValue] ^ t3[(int)(a >> 32) & byte.MaxValue] ^ t4[(int)(a >> 48) & byte.MaxValue];
        c += t4[(int)(a >> 8) & byte.MaxValue] ^ t3[(int)(a >> 24) & byte.MaxValue] ^ t2[(int)(a >> 40) & byte.MaxValue] ^ t1[(int)(a >> 56) & byte.MaxValue];
        c *= mul;
    }

    private void RoundCAB( long x, long mul )
    {
        b ^= x;
        c -= t1[(int)b & byte.MaxValue] ^ t2[(int)(b >> 16) & byte.MaxValue] ^ t3[(int)(b >> 32) & byte.MaxValue] ^ t4[(int)(b >> 48) & byte.MaxValue];
        a += t4[(int)(b >> 8) & byte.MaxValue] ^ t3[(int)(b >> 24) & byte.MaxValue] ^ t2[(int)(b >> 40) & byte.MaxValue] ^ t1[(int)(b >> 56) & byte.MaxValue];
        a *= mul;
    }

    private void KeySchedule()
    {
        long[] x1;
        (x1 = x)[0] = x1[0] - (x[7] ^ -6510615555426900571L);
        long[] x2;
        (x2 = x)[1] = x2[1] ^ x[0];
        long[] x3;
        (x3 = x)[2] = x3[2] + x[1];
        long[] x4;
        (x4 = x)[3] = x4[3] - (x[2] ^ (~x[1] << 19));
        long[] x5;
        (x5 = x)[4] = x5[4] ^ x[3];
        long[] x6;
        (x6 = x)[5] = x6[5] + x[4];
        long[] x7;
        (x7 = x)[6] = x7[6] - (x[5] ^ ~x[4] >>> 23);
        long[] x8;
        (x8 = x)[7] = x8[7] ^ x[6];
        long[] x9;
        (x9 = x)[0] = x9[0] + x[7];
        long[] x10;
        (x10 = x)[1] = x10[1] - (x[0] ^ (~x[7] << 19));
        long[] x11;
        (x11 = x)[2] = x11[2] ^ x[1];
        long[] x12;
        (x12 = x)[3] = x12[3] + x[2];
        long[] x13;
        (x13 = x)[4] = x13[4] - (x[3] ^ ~x[2] >>> 23);
        long[] x14;
        (x14 = x)[5] = x14[5] ^ x[4];
        long[] x15;
        (x15 = x)[6] = x15[6] + x[5];
        long[] x16;
        (x16 = x)[7] = x16[7] - (x[6] ^ 81985529216486895L);
    }

    private void ProcessBlock()
    {
        long a = this.a;
        long b = this.b;
        long c = this.c;
        RoundABC( x[0], 5L );
        RoundBCA( x[1], 5L );
        RoundCAB( x[2], 5L );
        RoundABC( x[3], 5L );
        RoundBCA( x[4], 5L );
        RoundCAB( x[5], 5L );
        RoundABC( x[6], 5L );
        RoundBCA( x[7], 5L );
        KeySchedule();
        RoundCAB( x[0], 7L );
        RoundABC( x[1], 7L );
        RoundBCA( x[2], 7L );
        RoundCAB( x[3], 7L );
        RoundABC( x[4], 7L );
        RoundBCA( x[5], 7L );
        RoundCAB( x[6], 7L );
        RoundABC( x[7], 7L );
        KeySchedule();
        RoundBCA( x[0], 9L );
        RoundCAB( x[1], 9L );
        RoundABC( x[2], 9L );
        RoundBCA( x[3], 9L );
        RoundCAB( x[4], 9L );
        RoundABC( x[5], 9L );
        RoundBCA( x[6], 9L );
        RoundCAB( x[7], 9L );
        this.a ^= a;
        this.b -= b;
        this.c += c;
        xOff = 0;
        for (int index = 0; index != x.Length; ++index)
            x[index] = 0L;
    }

    private void UnpackWord( long r, byte[] output, int outOff )
    {
        output[outOff + 7] = (byte)(r >> 56);
        output[outOff + 6] = (byte)(r >> 48);
        output[outOff + 5] = (byte)(r >> 40);
        output[outOff + 4] = (byte)(r >> 32);
        output[outOff + 3] = (byte)(r >> 24);
        output[outOff + 2] = (byte)(r >> 16);
        output[outOff + 1] = (byte)(r >> 8);
        output[outOff] = (byte)r;
    }

    private void ProcessLength( long bitLength ) => x[7] = bitLength;

    private void Finish()
    {
        long bitLength = byteCount << 3;
        Update( 1 );
        while (bOff != 0)
            Update( 0 );
        ProcessLength( bitLength );
        ProcessBlock();
    }

    public int DoFinal( byte[] output, int outOff )
    {
        Finish();
        UnpackWord( a, output, outOff );
        UnpackWord( b, output, outOff + 8 );
        UnpackWord( c, output, outOff + 16 );
        Reset();
        return 24;
    }

    public void Reset()
    {
        a = 81985529216486895L;
        b = -81985529216486896L;
        c = -1110518062304271993L;
        xOff = 0;
        for (int index = 0; index != x.Length; ++index)
            x[index] = 0L;
        bOff = 0;
        for (int index = 0; index != Buffer.Length; ++index)
            Buffer[index] = 0;
        byteCount = 0L;
    }

    public IMemoable Copy() => new TigerDigest( this );

    public void Reset( IMemoable other )
    {
        TigerDigest tigerDigest = (TigerDigest)other;
        a = tigerDigest.a;
        b = tigerDigest.b;
        c = tigerDigest.c;
        Array.Copy( tigerDigest.x, 0, x, 0, tigerDigest.x.Length );
        xOff = tigerDigest.xOff;
        Array.Copy( tigerDigest.Buffer, 0, Buffer, 0, tigerDigest.Buffer.Length );
        bOff = tigerDigest.bOff;
        byteCount = tigerDigest.byteCount;
    }
}
public sealed class WhirlpoolDigest : IDigest, IMemoable
{
    private const int BYTE_LENGTH = 64;
    private const int DIGEST_LENGTH_BYTES = 64;
    private const int ROUNDS = 10;
    private const int REDUCTION_POLYNOMIAL = 285;
    private const int BITCOUNT_ARRAY_SIZE = 32;
    private static readonly int[] SBOX = new int[256]
    {
      24,
      35,
      198,
      232,
      135,
      184,
      1,
      79,
      54,
      166,
      210,
      245,
      121,
      111,
      145,
      82,
      96,
      188,
      155,
      142,
      163,
      12,
      123,
      53,
      29,
      224,
      215,
      194,
      46,
      75,
      254,
      87,
      21,
      119,
      55,
      229,
      159,
      240,
      74,
      218,
      88,
      201,
      41,
      10,
      177,
      160,
      107,
      133,
      189,
      93,
      16,
      244,
      203,
      62,
      5,
      103,
      228,
      39,
      65,
      139,
      167,
      125,
      149,
      216,
      251,
      238,
      124,
      102,
      221,
      23,
      71,
      158,
      202,
      45,
      191,
      7,
      173,
      90,
      131,
      51,
      99,
      2,
      170,
      113,
      200,
      25,
      73,
      217,
      242,
      227,
      91,
      136,
      154,
      38,
      50,
      176,
      233,
      15,
      213,
      128,
      190,
      205,
      52,
      72,
       byte.MaxValue,
      122,
      144,
      95,
      32,
      104,
      26,
      174,
      180,
      84,
      147,
      34,
      100,
      241,
      115,
      18,
      64,
      8,
      195,
      236,
      219,
      161,
      141,
      61,
      151,
      0,
      207,
      43,
      118,
      130,
      214,
      27,
      181,
      175,
      106,
      80,
      69,
      243,
      48,
      239,
      63,
      85,
      162,
      234,
      101,
      186,
      47,
      192,
      222,
      28,
      253,
      77,
      146,
      117,
      6,
      138,
      178,
      230,
      14,
      31,
      98,
      212,
      168,
      150,
      249,
      197,
      37,
      89,
      132,
      114,
      57,
      76,
      94,
      120,
      56,
      140,
      209,
      165,
      226,
      97,
      179,
      33,
      156,
      30,
      67,
      199,
      252,
      4,
      81,
      153,
      109,
      13,
      250,
      223,
      126,
      36,
      59,
      171,
      206,
      17,
      143,
      78,
      183,
      235,
      60,
      129,
      148,
      247,
      185,
      19,
      44,
      211,
      231,
      110,
      196,
      3,
      86,
      68,
       sbyte.MaxValue,
      169,
      42,
      187,
      193,
      83,
      220,
      11,
      157,
      108,
      49,
      116,
      246,
      70,
      172,
      137,
      20,
      225,
      22,
      58,
      105,
      9,
      112,
      182,
      208,
      237,
      204,
      66,
      152,
      164,
      40,
      92,
      248,
      134
    };
    private static readonly long[] C0 = new long[256];
    private static readonly long[] C1 = new long[256];
    private static readonly long[] C2 = new long[256];
    private static readonly long[] C3 = new long[256];
    private static readonly long[] C4 = new long[256];
    private static readonly long[] C5 = new long[256];
    private static readonly long[] C6 = new long[256];
    private static readonly long[] C7 = new long[256];
    private readonly long[] _rc = new long[11];
    private static readonly short[] EIGHT = new short[32];
    private byte[] _buffer = new byte[64];
    private int _bufferPos;
    private short[] _bitCount = new short[32];
    private long[] _hash = new long[8];
    private long[] _K = new long[8];
    private long[] _L = new long[8];
    private long[] _block = new long[8];
    private long[] _state = new long[8];

    static WhirlpoolDigest()
    {
        EIGHT[31] = 8;
        for (int index = 0; index < 256; ++index)
        {
            int num1 = SBOX[index];
            int num2 = maskWithReductionPolynomial( num1 << 1 );
            int num3 = maskWithReductionPolynomial( num2 << 1 );
            int num4 = num3 ^ num1;
            int num5 = maskWithReductionPolynomial( num3 << 1 );
            int num6 = num5 ^ num1;
            C0[index] = packIntoLong( num1, num1, num3, num1, num5, num4, num2, num6 );
            C1[index] = packIntoLong( num6, num1, num1, num3, num1, num5, num4, num2 );
            C2[index] = packIntoLong( num2, num6, num1, num1, num3, num1, num5, num4 );
            C3[index] = packIntoLong( num4, num2, num6, num1, num1, num3, num1, num5 );
            C4[index] = packIntoLong( num5, num4, num2, num6, num1, num1, num3, num1 );
            C5[index] = packIntoLong( num1, num5, num4, num2, num6, num1, num1, num3 );
            C6[index] = packIntoLong( num3, num1, num5, num4, num2, num6, num1, num1 );
            C7[index] = packIntoLong( num1, num3, num1, num5, num4, num2, num6, num1 );
        }
    }

    public WhirlpoolDigest()
    {
        _rc[0] = 0L;
        for (int index1 = 1; index1 <= 10; ++index1)
        {
            int index2 = 8 * (index1 - 1);
            _rc[index1] = (C0[index2] & -72057594037927936L) ^ (C1[index2 + 1] & 71776119061217280L) ^ (C2[index2 + 2] & 280375465082880L) ^ (C3[index2 + 3] & 1095216660480L) ^ (C4[index2 + 4] & 4278190080L) ^ (C5[index2 + 5] & 16711680L) ^ (C6[index2 + 6] & 65280L) ^ (C7[index2 + 7] & byte.MaxValue);
        }
    }

    private static long packIntoLong(
      int b7,
      int b6,
      int b5,
      int b4,
      int b3,
      int b2,
      int b1,
      int b0 ) => ((long)b7 << 56) ^ ((long)b6 << 48) ^ ((long)b5 << 40) ^ ((long)b4 << 32) ^ ((long)b3 << 24) ^ ((long)b2 << 16) ^ ((long)b1 << 8) ^ b0;

    private static int maskWithReductionPolynomial( int input )
    {
        int num = input;
        if (num >= 256)
            num ^= 285;
        return num;
    }

    public WhirlpoolDigest( WhirlpoolDigest originalDigest ) => Reset( originalDigest );

    public string AlgorithmName => "Whirlpool";

    public int GetDigestSize() => 64;

    public int DoFinal( byte[] output, int outOff )
    {
        finish();
        for (int index = 0; index < 8; ++index)
            convertLongToByteArray( _hash[index], output, outOff + (index * 8) );
        Reset();
        return GetDigestSize();
    }

    public void Reset()
    {
        _bufferPos = 0;
        Array.Clear( _bitCount, 0, _bitCount.Length );
        Array.Clear( _buffer, 0, _buffer.Length );
        Array.Clear( _hash, 0, _hash.Length );
        Array.Clear( _K, 0, _K.Length );
        Array.Clear( _L, 0, _L.Length );
        Array.Clear( _block, 0, _block.Length );
        Array.Clear( _state, 0, _state.Length );
    }

    private void processFilledBuffer()
    {
        for (int index = 0; index < _state.Length; ++index)
            _block[index] = bytesToLongFromBuffer( _buffer, index * 8 );
        processBlock();
        _bufferPos = 0;
        Array.Clear( _buffer, 0, _buffer.Length );
    }

    private static long bytesToLongFromBuffer( byte[] buffer, int startPos ) => ((buffer[startPos] & (long)byte.MaxValue) << 56) | ((buffer[startPos + 1] & (long)byte.MaxValue) << 48) | ((buffer[startPos + 2] & (long)byte.MaxValue) << 40) | ((buffer[startPos + 3] & (long)byte.MaxValue) << 32) | ((buffer[startPos + 4] & (long)byte.MaxValue) << 24) | ((buffer[startPos + 5] & (long)byte.MaxValue) << 16) | ((buffer[startPos + 6] & (long)byte.MaxValue) << 8) | (buffer[startPos + 7] & (long)byte.MaxValue);

    private static void convertLongToByteArray( long inputLong, byte[] outputArray, int offSet )
    {
        for (int index = 0; index < 8; ++index)
            outputArray[offSet + index] = (byte)((ulong)(inputLong >> (56 - (index * 8))) & byte.MaxValue);
    }

    private void processBlock()
    {
        for (int index = 0; index < 8; ++index)
            _state[index] = _block[index] ^ (_K[index] = _hash[index]);
        for (int index1 = 1; index1 <= 10; ++index1)
        {
            for (int index2 = 0; index2 < 8; ++index2)
            {
                _L[index2] = 0L;
                _L[index2] ^= C0[(int)(_K[index2 & 7] >> 56) & byte.MaxValue];
                _L[index2] ^= C1[(int)(_K[(index2 - 1) & 7] >> 48) & byte.MaxValue];
                _L[index2] ^= C2[(int)(_K[(index2 - 2) & 7] >> 40) & byte.MaxValue];
                _L[index2] ^= C3[(int)(_K[(index2 - 3) & 7] >> 32) & byte.MaxValue];
                _L[index2] ^= C4[(int)(_K[(index2 - 4) & 7] >> 24) & byte.MaxValue];
                _L[index2] ^= C5[(int)(_K[(index2 - 5) & 7] >> 16) & byte.MaxValue];
                _L[index2] ^= C6[(int)(_K[(index2 - 6) & 7] >> 8) & byte.MaxValue];
                _L[index2] ^= C7[(int)_K[(index2 - 7) & 7] & byte.MaxValue];
            }
            Array.Copy( _L, 0, _K, 0, _K.Length );
            long[] k;
            (k = _K)[0] = k[0] ^ _rc[index1];
            for (int index3 = 0; index3 < 8; ++index3)
            {
                _L[index3] = _K[index3];
                _L[index3] ^= C0[(int)(_state[index3 & 7] >> 56) & byte.MaxValue];
                _L[index3] ^= C1[(int)(_state[(index3 - 1) & 7] >> 48) & byte.MaxValue];
                _L[index3] ^= C2[(int)(_state[(index3 - 2) & 7] >> 40) & byte.MaxValue];
                _L[index3] ^= C3[(int)(_state[(index3 - 3) & 7] >> 32) & byte.MaxValue];
                _L[index3] ^= C4[(int)(_state[(index3 - 4) & 7] >> 24) & byte.MaxValue];
                _L[index3] ^= C5[(int)(_state[(index3 - 5) & 7] >> 16) & byte.MaxValue];
                _L[index3] ^= C6[(int)(_state[(index3 - 6) & 7] >> 8) & byte.MaxValue];
                _L[index3] ^= C7[(int)_state[(index3 - 7) & 7] & byte.MaxValue];
            }
            Array.Copy( _L, 0, _state, 0, _state.Length );
        }
        for (int index = 0; index < 8; ++index)
            _hash[index] ^= _state[index] ^ _block[index];
    }

    public void Update( byte input )
    {
        _buffer[_bufferPos] = input;
        ++_bufferPos;
        if (_bufferPos == _buffer.Length)
            processFilledBuffer();
        increment();
    }

    private void increment()
    {
        int num1 = 0;
        for (int index = _bitCount.Length - 1; index >= 0; --index)
        {
            int num2 = (_bitCount[index] & byte.MaxValue) + EIGHT[index] + num1;
            num1 = num2 >> 8;
            _bitCount[index] = (short)(num2 & byte.MaxValue);
        }
    }

    public void BlockUpdate( byte[] input, int inOff, int length )
    {
        for (; length > 0; --length)
        {
            Update( input[inOff] );
            ++inOff;
        }
    }

    private void finish()
    {
        byte[] sourceArray = copyBitLength();
        byte[] buffer;
        byte[] numArray = buffer = _buffer;
        int num1 = _bufferPos++;
        int index1;
        int index2 = index1 = num1;
        int num2 = (byte)(numArray[(int)(IntPtr)index2] | 128U);
        buffer[index1] = (byte)num2;
        if (_bufferPos == _buffer.Length)
            processFilledBuffer();
        if (_bufferPos > 32)
        {
            while (_bufferPos != 0)
                Update( 0 );
        }
        while (_bufferPos <= 32)
            Update( 0 );
        Array.Copy( sourceArray, 0, _buffer, 32, sourceArray.Length );
        processFilledBuffer();
    }

    private byte[] copyBitLength()
    {
        byte[] numArray = new byte[32];
        for (int index = 0; index < numArray.Length; ++index)
            numArray[index] = (byte)((uint)_bitCount[index] & byte.MaxValue);
        return numArray;
    }

    public int GetByteLength() => 64;

    public IMemoable Copy() => new WhirlpoolDigest( this );

    public void Reset( IMemoable other )
    {
        WhirlpoolDigest whirlpoolDigest = (WhirlpoolDigest)other;
        Array.Copy( whirlpoolDigest._rc, 0, _rc, 0, _rc.Length );
        Array.Copy( whirlpoolDigest._buffer, 0, _buffer, 0, _buffer.Length );
        _bufferPos = whirlpoolDigest._bufferPos;
        Array.Copy( whirlpoolDigest._bitCount, 0, _bitCount, 0, _bitCount.Length );
        Array.Copy( whirlpoolDigest._hash, 0, _hash, 0, _hash.Length );
        Array.Copy( whirlpoolDigest._K, 0, _K, 0, _K.Length );
        Array.Copy( whirlpoolDigest._L, 0, _L, 0, _L.Length );
        Array.Copy( whirlpoolDigest._block, 0, _block, 0, _block.Length );
        Array.Copy( whirlpoolDigest._state, 0, _state, 0, _state.Length );
    }
}
public class TweakableBlockCipherParameters : ICipherParameters
{
    private readonly byte[] tweak;
    private readonly KeyParameter key;

    public TweakableBlockCipherParameters( KeyParameter key, byte[] tweak )
    {
        this.key = key;
        this.tweak = Arrays.Clone( tweak );
    }

    public KeyParameter Key => this.key;

    public byte[] Tweak => this.tweak;
}
[Serializable]
[ComVisible( true )]
public class InvalidOperationException : SystemException
{
    public InvalidOperationException()
        : base( Environment.GetResourceString( "Arg_InvalidOperationException" ) )
    {
        SetErrorCode( -2146233079 );
    }

    public InvalidOperationException( string message )
        : base( message )
    {
        SetErrorCode( -2146233079 );
    }

    public InvalidOperationException( string message, Exception innerException )
        : base( message, innerException )
    {
        SetErrorCode( -2146233079 );
    }

    protected InvalidOperationException( SerializationInfo info, StreamingContext context )
        : base( info, context )
    {
    }
}
public class KeyParameter : ICipherParameters
{
    private readonly byte[] key;

    public KeyParameter( byte[] key ) => this.key = key != null ? (byte[])key.Clone() : throw new ArgumentNullException( nameof( key ) );

    public KeyParameter( byte[] key, int keyOff, int keyLen )
    {
        if (key == null)
            throw new ArgumentNullException( nameof( key ) );
        if (keyOff < 0 || keyOff > key.Length)
            throw new ArgumentOutOfRangeException( nameof( keyOff ) );
        if (keyLen < 0 || keyOff + keyLen > key.Length)
            throw new ArgumentOutOfRangeException( nameof( keyLen ) );
        this.key = new byte[keyLen];
        Array.Copy( key, keyOff, this.key, 0, keyLen );
    }

    public byte[] GetKey() => (byte[])this.key.Clone();
}
public sealed class NistObjectIdentifiers
{
    public static readonly DerObjectIdentifier NistAlgorithm = new DerObjectIdentifier( "2.16.840.1.101.3.4" );
    public static readonly DerObjectIdentifier HashAlgs = NistAlgorithm.Branch( "2" );
    public static readonly DerObjectIdentifier IdSha256 = HashAlgs.Branch( "1" );
    public static readonly DerObjectIdentifier IdSha384 = HashAlgs.Branch( "2" );
    public static readonly DerObjectIdentifier IdSha512 = HashAlgs.Branch( "3" );
    public static readonly DerObjectIdentifier IdSha224 = HashAlgs.Branch( "4" );
    public static readonly DerObjectIdentifier IdSha512_224 = HashAlgs.Branch( "5" );
    public static readonly DerObjectIdentifier IdSha512_256 = HashAlgs.Branch( "6" );
    public static readonly DerObjectIdentifier IdSha3_224 = HashAlgs.Branch( "7" );
    public static readonly DerObjectIdentifier IdSha3_256 = HashAlgs.Branch( "8" );
    public static readonly DerObjectIdentifier IdSha3_384 = HashAlgs.Branch( "9" );
    public static readonly DerObjectIdentifier IdSha3_512 = HashAlgs.Branch( "10" );
    public static readonly DerObjectIdentifier IdShake128 = HashAlgs.Branch( "11" );
    public static readonly DerObjectIdentifier IdShake256 = HashAlgs.Branch( "12" );
    public static readonly DerObjectIdentifier Aes = new DerObjectIdentifier( NistAlgorithm.ToString() + ".1" );
    public static readonly DerObjectIdentifier IdAes128Ecb = new DerObjectIdentifier( Aes.ToString() + ".1" );
    public static readonly DerObjectIdentifier IdAes128Cbc = new DerObjectIdentifier( Aes.ToString() + ".2" );
    public static readonly DerObjectIdentifier IdAes128Ofb = new DerObjectIdentifier( Aes.ToString() + ".3" );
    public static readonly DerObjectIdentifier IdAes128Cfb = new DerObjectIdentifier( Aes.ToString() + ".4" );
    public static readonly DerObjectIdentifier IdAes128Wrap = new DerObjectIdentifier( Aes.ToString() + ".5" );
    public static readonly DerObjectIdentifier IdAes128Gcm = new DerObjectIdentifier( Aes.ToString() + ".6" );
    public static readonly DerObjectIdentifier IdAes128Ccm = new DerObjectIdentifier( Aes.ToString() + ".7" );
    public static readonly DerObjectIdentifier IdAes192Ecb = new DerObjectIdentifier( Aes.ToString() + ".21" );
    public static readonly DerObjectIdentifier IdAes192Cbc = new DerObjectIdentifier( Aes.ToString() + ".22" );
    public static readonly DerObjectIdentifier IdAes192Ofb = new DerObjectIdentifier( Aes.ToString() + ".23" );
    public static readonly DerObjectIdentifier IdAes192Cfb = new DerObjectIdentifier( Aes.ToString() + ".24" );
    public static readonly DerObjectIdentifier IdAes192Wrap = new DerObjectIdentifier( Aes.ToString() + ".25" );
    public static readonly DerObjectIdentifier IdAes192Gcm = new DerObjectIdentifier( Aes.ToString() + ".26" );
    public static readonly DerObjectIdentifier IdAes192Ccm = new DerObjectIdentifier( Aes.ToString() + ".27" );
    public static readonly DerObjectIdentifier IdAes256Ecb = new DerObjectIdentifier( Aes.ToString() + ".41" );
    public static readonly DerObjectIdentifier IdAes256Cbc = new DerObjectIdentifier( Aes.ToString() + ".42" );
    public static readonly DerObjectIdentifier IdAes256Ofb = new DerObjectIdentifier( Aes.ToString() + ".43" );
    public static readonly DerObjectIdentifier IdAes256Cfb = new DerObjectIdentifier( Aes.ToString() + ".44" );
    public static readonly DerObjectIdentifier IdAes256Wrap = new DerObjectIdentifier( Aes.ToString() + ".45" );
    public static readonly DerObjectIdentifier IdAes256Gcm = new DerObjectIdentifier( Aes.ToString() + ".46" );
    public static readonly DerObjectIdentifier IdAes256Ccm = new DerObjectIdentifier( Aes.ToString() + ".47" );
    public static readonly DerObjectIdentifier IdDsaWithSha2 = new DerObjectIdentifier( NistAlgorithm.ToString() + ".3" );
    public static readonly DerObjectIdentifier DsaWithSha224 = new DerObjectIdentifier( IdDsaWithSha2.ToString() + ".1" );
    public static readonly DerObjectIdentifier DsaWithSha256 = new DerObjectIdentifier( IdDsaWithSha2.ToString() + ".2" );
    public static readonly DerObjectIdentifier DsaWithSha384 = new DerObjectIdentifier( IdDsaWithSha2.ToString() + ".3" );
    public static readonly DerObjectIdentifier DsaWithSha512 = new DerObjectIdentifier( IdDsaWithSha2.ToString() + ".4" );

    private NistObjectIdentifiers()
    {
    }
}
public class DerObjectIdentifier : Asn1Object
{
    private const long LONG_LIMIT = 72057594037927808;
    private readonly string identifier;
    private byte[] body = null;
    private static readonly DerObjectIdentifier[] cache = new DerObjectIdentifier[1024];

    public static DerObjectIdentifier GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case DerObjectIdentifier _:
                return (DerObjectIdentifier)obj;
            case byte[] _:
                return FromOctetString( (byte[])obj );
            default:
                throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }
    }

    public static DerObjectIdentifier GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( obj.GetObject() );

    public DerObjectIdentifier( string identifier )
    {
        if (identifier == null)
            throw new ArgumentNullException( nameof( identifier ) );
        this.identifier = IsValidIdentifier( identifier ) ? identifier : throw new FormatException( "string " + identifier + " not an OID" );
    }

    internal DerObjectIdentifier( DerObjectIdentifier oid, string branchID )
    {
        if (!IsValidBranchID( branchID, 0 ))
            throw new ArgumentException( "string " + branchID + " not a valid OID branch", nameof( branchID ) );
        this.identifier = oid.Id + "." + branchID;
    }

    public string Id => this.identifier;

    public virtual DerObjectIdentifier Branch( string branchID ) => new DerObjectIdentifier( this, branchID );

    public virtual bool On( DerObjectIdentifier stem )
    {
        string id1 = this.Id;
        string id2 = stem.Id;
        return id1.Length > id2.Length && id1[id2.Length] == '.' && Platform.StartsWith( id1, id2 );
    }

    internal DerObjectIdentifier( byte[] bytes )
    {
        this.identifier = MakeOidStringFromBytes( bytes );
        this.body = Arrays.Clone( bytes );
    }

    private void WriteField( Stream outputStream, long fieldValue )
    {
        byte[] buffer = new byte[9];
        int offset = 8;
        buffer[offset] = (byte)((ulong)fieldValue & (ulong)sbyte.MaxValue);
        while (fieldValue >= 128L)
        {
            fieldValue >>= 7;
            buffer[--offset] = (byte)((ulong)(fieldValue & sbyte.MaxValue) | 128UL);
        }
        outputStream.Write( buffer, offset, 9 - offset );
    }

    private void WriteField( Stream outputStream, BigInteger fieldValue )
    {
        int length = (fieldValue.BitLength + 6) / 7;
        if (length == 0)
        {
            outputStream.WriteByte( 0 );
        }
        else
        {
            BigInteger bigInteger = fieldValue;
            byte[] buffer = new byte[length];
            for (int index = length - 1; index >= 0; --index)
            {
                buffer[index] = (byte)((bigInteger.IntValue & sbyte.MaxValue) | 128);
                bigInteger = bigInteger.ShiftRight( 7 );
            }
            byte[] numArray;
            IntPtr index1;
            (numArray = buffer)[(int)(index1 = (IntPtr)(length - 1))] = (byte)(numArray[(int)index1] & (uint)sbyte.MaxValue);
            outputStream.Write( buffer, 0, buffer.Length );
        }
    }

    private void DoOutput( MemoryStream bOut )
    {
        OidTokenizer oidTokenizer = new OidTokenizer( this.identifier );
        int num = int.Parse( oidTokenizer.NextToken() ) * 40;
        string s1 = oidTokenizer.NextToken();
        if (s1.Length <= 18)
            this.WriteField( bOut, num + long.Parse( s1 ) );
        else
            this.WriteField( bOut, new BigInteger( s1 ).Add( BigInteger.ValueOf( num ) ) );
        while (oidTokenizer.HasMoreTokens)
        {
            string s2 = oidTokenizer.NextToken();
            if (s2.Length <= 18)
                this.WriteField( bOut, long.Parse( s2 ) );
            else
                this.WriteField( bOut, new BigInteger( s2 ) );
        }
    }

    internal byte[] GetBody()
    {
        lock (this)
        {
            if (this.body == null)
            {
                MemoryStream bOut = new MemoryStream();
                this.DoOutput( bOut );
                this.body = bOut.ToArray();
            }
        }
        return this.body;
    }

    internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 6, this.GetBody() );

    protected override int Asn1GetHashCode() => this.identifier.GetHashCode();

    protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerObjectIdentifier objectIdentifier && this.identifier.Equals( objectIdentifier.identifier );

    public override string ToString() => this.identifier;

    private static bool IsValidBranchID( string branchID, int start )
    {
        bool flag = false;
        int length = branchID.Length;
        while (--length >= start)
        {
            char ch = branchID[length];
            if ('0' <= ch && ch <= '9')
            {
                flag = true;
            }
            else
            {
                if (ch != '.' || !flag)
                    return false;
                flag = false;
            }
        }
        return flag;
    }

    private static bool IsValidIdentifier( string identifier )
    {
        if (identifier.Length < 3 || identifier[1] != '.')
            return false;
        char ch = identifier[0];
        return ch >= '0' && ch <= '2' && IsValidBranchID( identifier, 2 );
    }

    private static string MakeOidStringFromBytes( byte[] bytes )
    {
        StringBuilder stringBuilder = new StringBuilder();
        long num1 = 0;
        BigInteger bigInteger1 = null;
        bool flag = true;
        for (int index = 0; index != bytes.Length; ++index)
        {
            int num2 = bytes[index];
            if (num1 <= 72057594037927808L)
            {
                long num3 = num1 + (num2 & sbyte.MaxValue);
                if ((num2 & 128) == 0)
                {
                    if (flag)
                    {
                        if (num3 < 40L)
                            stringBuilder.Append( '0' );
                        else if (num3 < 80L)
                        {
                            stringBuilder.Append( '1' );
                            num3 -= 40L;
                        }
                        else
                        {
                            stringBuilder.Append( '2' );
                            num3 -= 80L;
                        }
                        flag = false;
                    }
                    stringBuilder.Append( '.' );
                    stringBuilder.Append( num3 );
                    num1 = 0L;
                }
                else
                    num1 = num3 << 7;
            }
            else
            {
                if (bigInteger1 == null)
                    bigInteger1 = BigInteger.ValueOf( num1 );
                BigInteger bigInteger2 = bigInteger1.Or( BigInteger.ValueOf( num2 & sbyte.MaxValue ) );
                if ((num2 & 128) == 0)
                {
                    if (flag)
                    {
                        stringBuilder.Append( '2' );
                        bigInteger2 = bigInteger2.Subtract( BigInteger.ValueOf( 80L ) );
                        flag = false;
                    }
                    stringBuilder.Append( '.' );
                    stringBuilder.Append( bigInteger2 );
                    bigInteger1 = null;
                    num1 = 0L;
                }
                else
                    bigInteger1 = bigInteger2.ShiftLeft( 7 );
            }
        }
        return stringBuilder.ToString();
    }

    internal static DerObjectIdentifier FromOctetString( byte[] enc )
    {
        int index = Arrays.GetHashCode( enc ) & 1023;
        lock (cache)
        {
            DerObjectIdentifier objectIdentifier = cache[index];
            return objectIdentifier != null && Arrays.AreEqual( enc, objectIdentifier.GetBody() ) ? objectIdentifier : (cache[index] = new DerObjectIdentifier( enc ));
        }
    }
}
public abstract class Asn1Object : Asn1Encodable
{
    public static Asn1Object FromByteArray( byte[] data )
    {
        try
        {
            MemoryStream inputStream = new MemoryStream( data, false );
            Asn1Object asn1Object = new Asn1InputStream( inputStream, data.Length ).ReadObject();
            if (inputStream.Position != inputStream.Length)
                throw new IOException( "extra data found after object" );
            return asn1Object;
        }
        catch (InvalidCastException)
        {
            throw new IOException( "cannot recognise object in byte array" );
        }
    }

    public static Asn1Object FromStream( Stream inStr )
    {
        try
        {
            return new Asn1InputStream( inStr ).ReadObject();
        }
        catch (InvalidCastException)
        {
            throw new IOException( "cannot recognise object in stream" );
        }
    }

    public override sealed Asn1Object ToAsn1Object() => this;

    internal abstract void Encode( DerOutputStream derOut );

    protected abstract bool Asn1Equals( Asn1Object asn1Object );

    protected abstract int Asn1GetHashCode();

    internal bool CallAsn1Equals( Asn1Object obj ) => this.Asn1Equals( obj );

    internal int CallAsn1GetHashCode() => this.Asn1GetHashCode();
}
public abstract class Asn1Encodable : IAsn1Convertible
{
    public const string Der = "DER";
    public const string Ber = "BER";

    public byte[] GetEncoded()
    {
        MemoryStream os = new MemoryStream();
        new Asn1OutputStream( os ).WriteObject( this );
        return os.ToArray();
    }

    public byte[] GetEncoded( string encoding )
    {
        if (!encoding.Equals( "DER" ))
            return this.GetEncoded();
        MemoryStream os = new MemoryStream();
        new DerOutputStream( os ).WriteObject( this );
        return os.ToArray();
    }

    public byte[] GetDerEncoded()
    {
        try
        {
            return this.GetEncoded( "DER" );
        }
        catch (IOException)
        {
            return null;
        }
    }

    public override sealed int GetHashCode() => this.ToAsn1Object().CallAsn1GetHashCode();

    public override sealed bool Equals( object obj )
    {
        if (obj == this)
            return true;
        if (!(obj is IAsn1Convertible asn1Convertible))
            return false;
        Asn1Object asn1Object1 = this.ToAsn1Object();
        Asn1Object asn1Object2 = asn1Convertible.ToAsn1Object();
        return asn1Object1 == asn1Object2 || asn1Object1.CallAsn1Equals( asn1Object2 );
    }

    public abstract Asn1Object ToAsn1Object();
}
public interface IAsn1Convertible
{
    Asn1Object ToAsn1Object();
}
public class OidTokenizer
{
    private string oid;
    private int index;

    public OidTokenizer( string oid ) => this.oid = oid;

    public bool HasMoreTokens => this.index != -1;

    public string NextToken()
    {
        if (this.index == -1)
            return null;
        int num = this.oid.IndexOf( '.', this.index );
        if (num == -1)
        {
            string str = this.oid.Substring( this.index );
            this.index = -1;
            return str;
        }
        string str1 = this.oid.Substring( this.index, num - this.index );
        this.index = num + 1;
        return str1;
    }
}
public class Asn1OutputStream : DerOutputStream
{
    public Asn1OutputStream( Stream os )
      : base( os )
    {
    }

    [Obsolete( "Use version taking an Asn1Encodable arg instead" )]
    public override void WriteObject( object obj )
    {
        switch (obj)
        {
            case null:
                this.WriteNull();
                break;
            case Asn1Object _:
                ((Asn1Object)obj).Encode( this );
                break;
            case Asn1Encodable _:
                ((Asn1Encodable)obj).ToAsn1Object().Encode( this );
                break;
            default:
                throw new IOException( "object not Asn1Encodable" );
        }
    }
}
public class DerOutputStream : FilterStream
{
    public DerOutputStream( Stream os )
      : base( os )
    {
    }

    private void WriteLength( int length )
    {
        if (length > sbyte.MaxValue)
        {
            int num1 = 1;
            uint num2 = (uint)length;
            while ((num2 >>= 8) != 0U)
                ++num1;
            this.WriteByte( (byte)(num1 | 128) );
            for (int index = (num1 - 1) * 8; index >= 0; index -= 8)
                this.WriteByte( (byte)(length >> index) );
        }
        else
            this.WriteByte( (byte)length );
    }

    internal void WriteEncoded( int tag, byte[] bytes )
    {
        this.WriteByte( (byte)tag );
        this.WriteLength( bytes.Length );
        this.Write( bytes, 0, bytes.Length );
    }

    internal void WriteEncoded( int tag, byte first, byte[] bytes )
    {
        this.WriteByte( (byte)tag );
        this.WriteLength( bytes.Length + 1 );
        this.WriteByte( first );
        this.Write( bytes, 0, bytes.Length );
    }

    internal void WriteEncoded( int tag, byte[] bytes, int offset, int length )
    {
        this.WriteByte( (byte)tag );
        this.WriteLength( length );
        this.Write( bytes, offset, length );
    }

    internal void WriteTag( int flags, int tagNo )
    {
        if (tagNo < 31)
        {
            this.WriteByte( (byte)(flags | tagNo) );
        }
        else
        {
            this.WriteByte( (byte)(flags | 31) );
            if (tagNo < 128)
            {
                this.WriteByte( (byte)tagNo );
            }
            else
            {
                byte[] buffer = new byte[5];
                int length = buffer.Length;
                int offset;
                buffer[offset = length - 1] = (byte)(tagNo & sbyte.MaxValue);
                do
                {
                    tagNo >>= 7;
                    buffer[--offset] = (byte)((tagNo & sbyte.MaxValue) | 128);
                }
                while (tagNo > sbyte.MaxValue);
                this.Write( buffer, offset, buffer.Length - offset );
            }
        }
    }

    internal void WriteEncoded( int flags, int tagNo, byte[] bytes )
    {
        this.WriteTag( flags, tagNo );
        this.WriteLength( bytes.Length );
        this.Write( bytes, 0, bytes.Length );
    }

    protected void WriteNull()
    {
        this.WriteByte( 5 );
        this.WriteByte( 0 );
    }

    [Obsolete( "Use version taking an Asn1Encodable arg instead" )]
    public virtual void WriteObject( object obj )
    {
        switch (obj)
        {
            case null:
                this.WriteNull();
                break;
            case Asn1Object _:
                ((Asn1Object)obj).Encode( this );
                break;
            case Asn1Encodable _:
                ((Asn1Encodable)obj).ToAsn1Object().Encode( this );
                break;
            default:
                throw new IOException( "object not Asn1Object" );
        }
    }

    public virtual void WriteObject( Asn1Encodable obj )
    {
        if (obj == null)
            this.WriteNull();
        else
            obj.ToAsn1Object().Encode( this );
    }

    public virtual void WriteObject( Asn1Object obj )
    {
        if (obj == null)
            this.WriteNull();
        else
            obj.Encode( this );
    }
}
public class FilterStream : Stream
{
    protected readonly Stream s;

    public FilterStream( Stream s ) => this.s = s;

    public override bool CanRead => this.s.CanRead;

    public override bool CanSeek => this.s.CanSeek;

    public override bool CanWrite => this.s.CanWrite;

    public override long Length => this.s.Length;

    public override long Position
    {
        get => this.s.Position;
        set => this.s.Position = value;
    }

    public override void Close()
    {
        Platform.Dispose( this.s );
        base.Close();
    }

    public override void Flush() => this.s.Flush();

    public override long Seek( long offset, SeekOrigin origin ) => this.s.Seek( offset, origin );

    public override void SetLength( long value ) => this.s.SetLength( value );

    public override int Read( byte[] buffer, int offset, int count ) => this.s.Read( buffer, offset, count );

    public override int ReadByte() => this.s.ReadByte();

    public override void Write( byte[] buffer, int offset, int count ) => this.s.Write( buffer, offset, count );

    public override void WriteByte( byte value ) => this.s.WriteByte( value );
}
public class Asn1InputStream : FilterStream
{
    private readonly int limit;
    private readonly byte[][] tmpBuffers;

    internal static int FindLimit( Stream input )
    {
        switch (input)
        {
            case LimitedInputStream _:
                return ((LimitedInputStream)input).GetRemaining();
            case MemoryStream _:
                MemoryStream memoryStream = (MemoryStream)input;
                return (int)(memoryStream.Length - memoryStream.Position);
            default:
                return int.MaxValue;
        }
    }

    public Asn1InputStream( Stream inputStream )
      : this( inputStream, FindLimit( inputStream ) )
    {
    }

    public Asn1InputStream( Stream inputStream, int limit )
      : base( inputStream )
    {
        this.limit = limit;
        this.tmpBuffers = new byte[16][];
    }

    public Asn1InputStream( byte[] input )
      : this( new MemoryStream( input, false ), input.Length )
    {
    }

    private Asn1Object BuildObject( int tag, int tagNo, int length )
    {
        bool flag = (tag & 32) != 0;
        DefiniteLengthInputStream lengthInputStream = new DefiniteLengthInputStream( this.s, length );
        if ((tag & 64) != 0)
            return new DerApplicationSpecific( flag, tagNo, lengthInputStream.ToArray() );
        if ((tag & 128) != 0)
            return new Asn1StreamParser( lengthInputStream ).ReadTaggedObject( flag, tagNo );
        if (!flag)
            return CreatePrimitiveDerObject( tagNo, lengthInputStream, this.tmpBuffers );
        switch (tagNo)
        {
            case 4:
                return new BerOctetString( this.BuildDerEncodableVector( lengthInputStream ) );
            case 8:
                return new DerExternal( this.BuildDerEncodableVector( lengthInputStream ) );
            case 16:
                return this.CreateDerSequence( lengthInputStream );
            case 17:
                return this.CreateDerSet( lengthInputStream );
            default:
                throw new IOException( "unknown tag " + tagNo + " encountered" );
        }
    }

    internal Asn1EncodableVector BuildEncodableVector()
    {
        Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector( new Asn1Encodable[0] );
        Asn1Object asn1Object;
        while ((asn1Object = this.ReadObject()) != null)
            asn1EncodableVector.Add( asn1Object );
        return asn1EncodableVector;
    }

    internal virtual Asn1EncodableVector BuildDerEncodableVector( DefiniteLengthInputStream dIn ) => new Asn1InputStream( dIn ).BuildEncodableVector();

    internal virtual DerSequence CreateDerSequence( DefiniteLengthInputStream dIn ) => DerSequence.FromVector( this.BuildDerEncodableVector( dIn ) );

    internal virtual DerSet CreateDerSet( DefiniteLengthInputStream dIn ) => DerSet.FromVector( this.BuildDerEncodableVector( dIn ), false );

    public Asn1Object ReadObject()
    {
        int tag = this.ReadByte();
        if (tag <= 0)
        {
            if (tag == 0)
                throw new IOException( "unexpected end-of-contents marker" );
            return null;
        }
        int num = ReadTagNumber( this.s, tag );
        bool flag = (tag & 32) != 0;
        int length = ReadLength( this.s, this.limit );
        if (length < 0)
        {
            if (!flag)
                throw new IOException( "indefinite length primitive encoding encountered" );
            Asn1StreamParser parser = new Asn1StreamParser( new IndefiniteLengthInputStream( this.s, this.limit ), this.limit );
            if ((tag & 64) != 0)
                return new BerApplicationSpecificParser( num, parser ).ToAsn1Object();
            if ((tag & 128) != 0)
                return new BerTaggedObjectParser( true, num, parser ).ToAsn1Object();
            switch (num)
            {
                case 4:
                    return new BerOctetStringParser( parser ).ToAsn1Object();
                case 8:
                    return new DerExternalParser( parser ).ToAsn1Object();
                case 16:
                    return new BerSequenceParser( parser ).ToAsn1Object();
                case 17:
                    return new BerSetParser( parser ).ToAsn1Object();
                default:
                    throw new IOException( "unknown BER object encountered" );
            }
        }
        else
        {
            try
            {
                return this.BuildObject( tag, num, length );
            }
            catch (ArgumentException ex)
            {
                throw new Asn1Exception( "corrupted stream detected", ex );
            }
        }
    }

    internal static int ReadTagNumber( Stream s, int tag )
    {
        int num1 = tag & 31;
        if (num1 == 31)
        {
            int num2 = 0;
            int num3 = s.ReadByte();
            if ((num3 & sbyte.MaxValue) == 0)
                throw new IOException( "Corrupted stream - invalid high tag number found" );
            for (; num3 >= 0 && (num3 & 128) != 0; num3 = s.ReadByte())
                num2 = (num2 | (num3 & sbyte.MaxValue)) << 7;
            if (num3 < 0)
                throw new EndOfStreamException( "EOF found inside tag value." );
            num1 = num2 | (num3 & sbyte.MaxValue);
        }
        return num1;
    }

    internal static int ReadLength( Stream s, int limit )
    {
        int num1 = s.ReadByte();
        if (num1 < 0)
            throw new EndOfStreamException( "EOF found when length expected" );
        if (num1 == 128)
            return -1;
        if (num1 > sbyte.MaxValue)
        {
            int num2 = num1 & sbyte.MaxValue;
            if (num2 > 4)
                throw new IOException( "DER length more than 4 bytes: " + num2 );
            num1 = 0;
            for (int index = 0; index < num2; ++index)
            {
                int num3 = s.ReadByte();
                if (num3 < 0)
                    throw new EndOfStreamException( "EOF found reading length" );
                num1 = (num1 << 8) + num3;
            }
            if (num1 < 0)
                throw new IOException( "Corrupted stream - negative length found" );
            if (num1 >= limit)
                throw new IOException( "Corrupted stream - out of bounds length found" );
        }
        return num1;
    }

    internal static byte[] GetBuffer( DefiniteLengthInputStream defIn, byte[][] tmpBuffers )
    {
        int remaining = defIn.GetRemaining();
        if (remaining >= tmpBuffers.Length)
            return defIn.ToArray();
        byte[] buf = tmpBuffers[remaining] ?? (tmpBuffers[remaining] = new byte[remaining]);
        defIn.ReadAllIntoByteArray( buf );
        return buf;
    }

    internal static Asn1Object CreatePrimitiveDerObject(
      int tagNo,
      DefiniteLengthInputStream defIn,
      byte[][] tmpBuffers )
    {
        switch (tagNo)
        {
            case 1:
                return DerBoolean.FromOctetString( GetBuffer( defIn, tmpBuffers ) );
            case 6:
                return DerObjectIdentifier.FromOctetString( GetBuffer( defIn, tmpBuffers ) );
            case 10:
                return DerEnumerated.FromOctetString( GetBuffer( defIn, tmpBuffers ) );
            default:
                byte[] array = defIn.ToArray();
                switch (tagNo)
                {
                    case 2:
                        return new DerInteger( array );
                    case 3:
                        return DerBitString.FromAsn1Octets( array );
                    case 4:
                        return new DerOctetString( array );
                    case 5:
                        return DerNull.Instance;
                    case 12:
                        return new DerUtf8String( array );
                    case 18:
                        return new DerNumericString( array );
                    case 19:
                        return new DerPrintableString( array );
                    case 20:
                        return new DerT61String( array );
                    case 21:
                        return new DerVideotexString( array );
                    case 22:
                        return new DerIA5String( array );
                    case 23:
                        return new DerUtcTime( array );
                    case 24:
                        return new DerGeneralizedTime( array );
                    case 25:
                        return new DerGraphicString( array );
                    case 26:
                        return new DerVisibleString( array );
                    case 27:
                        return new DerGeneralString( array );
                    case 28:
                        return new DerUniversalString( array );
                    case 30:
                        return new DerBmpString( array );
                    default:
                        throw new IOException( "unknown tag " + tagNo + " encountered" );
                }
        }
    }
}
internal abstract class LimitedInputStream : BaseInputStream
{
    protected readonly Stream _in;
    private int _limit;

    internal LimitedInputStream( Stream inStream, int limit )
    {
        this._in = inStream;
        this._limit = limit;
    }

    internal virtual int GetRemaining() => this._limit;

    protected virtual void SetParentEofDetect( bool on )
    {
        if (!(this._in is IndefiniteLengthInputStream))
            return;
        ((IndefiniteLengthInputStream)this._in).SetEofOn00( on );
    }
}
internal class IndefiniteLengthInputStream : LimitedInputStream
{
    private int _lookAhead;
    private bool _eofOn00 = true;

    internal IndefiniteLengthInputStream( Stream inStream, int limit )
      : base( inStream, limit )
    {
        this._lookAhead = this.RequireByte();
        this.CheckForEof();
    }

    internal void SetEofOn00( bool eofOn00 )
    {
        this._eofOn00 = eofOn00;
        if (!this._eofOn00)
            return;
        this.CheckForEof();
    }

    private bool CheckForEof()
    {
        if (this._lookAhead != 0)
            return this._lookAhead < 0;
        if (this.RequireByte() != 0)
            throw new IOException( "malformed end-of-contents marker" );
        this._lookAhead = -1;
        this.SetParentEofDetect( true );
        return true;
    }

    public override int Read( byte[] buffer, int offset, int count )
    {
        if (this._eofOn00 || count <= 1)
            return base.Read( buffer, offset, count );
        if (this._lookAhead < 0)
            return 0;
        int num = this._in.Read( buffer, offset + 1, count - 1 );
        if (num <= 0)
            throw new EndOfStreamException();
        buffer[offset] = (byte)this._lookAhead;
        this._lookAhead = this.RequireByte();
        return num + 1;
    }

    public override int ReadByte()
    {
        if (this._eofOn00 && this.CheckForEof())
            return -1;
        int lookAhead = this._lookAhead;
        this._lookAhead = this.RequireByte();
        return lookAhead;
    }

    private int RequireByte()
    {
        int num = this._in.ReadByte();
        return num >= 0 ? num : throw new EndOfStreamException();
    }
}
internal abstract class LimitedInputStream : BaseInputStream
{
    protected readonly Stream _in;
    private int _limit;

    internal LimitedInputStream( Stream inStream, int limit )
    {
        this._in = inStream;
        this._limit = limit;
    }

    internal virtual int GetRemaining() => this._limit;

    protected virtual void SetParentEofDetect( bool on )
    {
        if (!(this._in is IndefiniteLengthInputStream))
            return;
        ((IndefiniteLengthInputStream)this._in).SetEofOn00( on );
    }
}
public abstract class BaseInputStream : Stream
{
    private bool closed;

    public override sealed bool CanRead => !this.closed;

    public override sealed bool CanSeek => false;

    public override sealed bool CanWrite => false;

    public override void Close()
    {
        this.closed = true;
        base.Close();
    }

    public override sealed void Flush()
    {
    }

    public override sealed long Length => throw new NotSupportedException();

    public override sealed long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override int Read( byte[] buffer, int offset, int count )
    {
        int num1 = offset;
        try
        {
            int num2;
            for (int index = offset + count; num1 < index; buffer[num1++] = (byte)num2)
            {
                num2 = this.ReadByte();
                if (num2 == -1)
                    break;
            }
        }
        catch (IOException)
        {
            if (num1 == offset)
                throw;
        }
        return num1 - offset;
    }

    public override sealed long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();

    public override sealed void SetLength( long value ) => throw new NotSupportedException();

    public override sealed void Write( byte[] buffer, int offset, int count ) => throw new NotSupportedException();
}
public abstract class CryptoProObjectIdentifiers
{
    public const string GostID = "1.2.643.2.2";
    public static readonly DerObjectIdentifier GostR3411 = new DerObjectIdentifier( "1.2.643.2.2.9" );
    public static readonly DerObjectIdentifier GostR3411Hmac = new DerObjectIdentifier( "1.2.643.2.2.10" );
    public static readonly DerObjectIdentifier GostR28147Cbc = new DerObjectIdentifier( "1.2.643.2.2.21" );
    public static readonly DerObjectIdentifier ID_Gost28147_89_CryptoPro_A_ParamSet = new DerObjectIdentifier( "1.2.643.2.2.31.1" );
    public static readonly DerObjectIdentifier GostR3410x94 = new DerObjectIdentifier( "1.2.643.2.2.20" );
    public static readonly DerObjectIdentifier GostR3410x2001 = new DerObjectIdentifier( "1.2.643.2.2.19" );
    public static readonly DerObjectIdentifier GostR3411x94WithGostR3410x94 = new DerObjectIdentifier( "1.2.643.2.2.4" );
    public static readonly DerObjectIdentifier GostR3411x94WithGostR3410x2001 = new DerObjectIdentifier( "1.2.643.2.2.3" );
    public static readonly DerObjectIdentifier GostR3411x94CryptoProParamSet = new DerObjectIdentifier( "1.2.643.2.2.30.1" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProA = new DerObjectIdentifier( "1.2.643.2.2.32.2" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProB = new DerObjectIdentifier( "1.2.643.2.2.32.3" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProC = new DerObjectIdentifier( "1.2.643.2.2.32.4" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProD = new DerObjectIdentifier( "1.2.643.2.2.32.5" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProXchA = new DerObjectIdentifier( "1.2.643.2.2.33.1" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProXchB = new DerObjectIdentifier( "1.2.643.2.2.33.2" );
    public static readonly DerObjectIdentifier GostR3410x94CryptoProXchC = new DerObjectIdentifier( "1.2.643.2.2.33.3" );
    public static readonly DerObjectIdentifier GostR3410x2001CryptoProA = new DerObjectIdentifier( "1.2.643.2.2.35.1" );
    public static readonly DerObjectIdentifier GostR3410x2001CryptoProB = new DerObjectIdentifier( "1.2.643.2.2.35.2" );
    public static readonly DerObjectIdentifier GostR3410x2001CryptoProC = new DerObjectIdentifier( "1.2.643.2.2.35.3" );
    public static readonly DerObjectIdentifier GostR3410x2001CryptoProXchA = new DerObjectIdentifier( "1.2.643.2.2.36.0" );
    public static readonly DerObjectIdentifier GostR3410x2001CryptoProXchB = new DerObjectIdentifier( "1.2.643.2.2.36.1" );
    public static readonly DerObjectIdentifier GostElSgDH3410Default = new DerObjectIdentifier( "1.2.643.2.2.36.0" );
    public static readonly DerObjectIdentifier GostElSgDH3410x1 = new DerObjectIdentifier( "1.2.643.2.2.36.1" );
}
public sealed class TeleTrusTObjectIdentifiers
{
    public static readonly DerObjectIdentifier TeleTrusTAlgorithm = new DerObjectIdentifier( "1.3.36.3" );
    public static readonly DerObjectIdentifier RipeMD160 = new DerObjectIdentifier( TeleTrusTAlgorithm.ToString() + ".2.1" );
    public static readonly DerObjectIdentifier RipeMD128 = new DerObjectIdentifier( TeleTrusTAlgorithm.ToString() + ".2.2" );
    public static readonly DerObjectIdentifier RipeMD256 = new DerObjectIdentifier( TeleTrusTAlgorithm.ToString() + ".2.3" );
    public static readonly DerObjectIdentifier TeleTrusTRsaSignatureAlgorithm = new DerObjectIdentifier( TeleTrusTAlgorithm.ToString() + ".3.1" );
    public static readonly DerObjectIdentifier RsaSignatureWithRipeMD160 = new DerObjectIdentifier( TeleTrusTRsaSignatureAlgorithm.ToString() + ".2" );
    public static readonly DerObjectIdentifier RsaSignatureWithRipeMD128 = new DerObjectIdentifier( TeleTrusTRsaSignatureAlgorithm.ToString() + ".3" );
    public static readonly DerObjectIdentifier RsaSignatureWithRipeMD256 = new DerObjectIdentifier( TeleTrusTRsaSignatureAlgorithm.ToString() + ".4" );
    public static readonly DerObjectIdentifier ECSign = new DerObjectIdentifier( TeleTrusTAlgorithm.ToString() + ".3.2" );
    public static readonly DerObjectIdentifier ECSignWithSha1 = new DerObjectIdentifier( ECSign.ToString() + ".1" );
    public static readonly DerObjectIdentifier ECSignWithRipeMD160 = new DerObjectIdentifier( ECSign.ToString() + ".2" );
    public static readonly DerObjectIdentifier EccBrainpool = new DerObjectIdentifier( TeleTrusTAlgorithm.ToString() + ".3.2.8" );
    public static readonly DerObjectIdentifier EllipticCurve = new DerObjectIdentifier( EccBrainpool.ToString() + ".1" );
    public static readonly DerObjectIdentifier VersionOne = new DerObjectIdentifier( EllipticCurve.ToString() + ".1" );
    public static readonly DerObjectIdentifier BrainpoolP160R1 = new DerObjectIdentifier( VersionOne.ToString() + ".1" );
    public static readonly DerObjectIdentifier BrainpoolP160T1 = new DerObjectIdentifier( VersionOne.ToString() + ".2" );
    public static readonly DerObjectIdentifier BrainpoolP192R1 = new DerObjectIdentifier( VersionOne.ToString() + ".3" );
    public static readonly DerObjectIdentifier BrainpoolP192T1 = new DerObjectIdentifier( VersionOne.ToString() + ".4" );
    public static readonly DerObjectIdentifier BrainpoolP224R1 = new DerObjectIdentifier( VersionOne.ToString() + ".5" );
    public static readonly DerObjectIdentifier BrainpoolP224T1 = new DerObjectIdentifier( VersionOne.ToString() + ".6" );
    public static readonly DerObjectIdentifier BrainpoolP256R1 = new DerObjectIdentifier( VersionOne.ToString() + ".7" );
    public static readonly DerObjectIdentifier BrainpoolP256T1 = new DerObjectIdentifier( VersionOne.ToString() + ".8" );
    public static readonly DerObjectIdentifier BrainpoolP320R1 = new DerObjectIdentifier( VersionOne.ToString() + ".9" );
    public static readonly DerObjectIdentifier BrainpoolP320T1 = new DerObjectIdentifier( VersionOne.ToString() + ".10" );
    public static readonly DerObjectIdentifier BrainpoolP384R1 = new DerObjectIdentifier( VersionOne.ToString() + ".11" );
    public static readonly DerObjectIdentifier BrainpoolP384T1 = new DerObjectIdentifier( VersionOne.ToString() + ".12" );
    public static readonly DerObjectIdentifier BrainpoolP512R1 = new DerObjectIdentifier( VersionOne.ToString() + ".13" );
    public static readonly DerObjectIdentifier BrainpoolP512T1 = new DerObjectIdentifier( VersionOne.ToString() + ".14" );

    private TeleTrusTObjectIdentifiers()
    {
    }
}
public abstract class OiwObjectIdentifiers
{
    public static readonly DerObjectIdentifier MD4WithRsa = new DerObjectIdentifier( "1.3.14.3.2.2" );
    public static readonly DerObjectIdentifier MD5WithRsa = new DerObjectIdentifier( "1.3.14.3.2.3" );
    public static readonly DerObjectIdentifier MD4WithRsaEncryption = new DerObjectIdentifier( "1.3.14.3.2.4" );
    public static readonly DerObjectIdentifier DesEcb = new DerObjectIdentifier( "1.3.14.3.2.6" );
    public static readonly DerObjectIdentifier DesCbc = new DerObjectIdentifier( "1.3.14.3.2.7" );
    public static readonly DerObjectIdentifier DesOfb = new DerObjectIdentifier( "1.3.14.3.2.8" );
    public static readonly DerObjectIdentifier DesCfb = new DerObjectIdentifier( "1.3.14.3.2.9" );
    public static readonly DerObjectIdentifier DesEde = new DerObjectIdentifier( "1.3.14.3.2.17" );
    public static readonly DerObjectIdentifier IdSha1 = new DerObjectIdentifier( "1.3.14.3.2.26" );
    public static readonly DerObjectIdentifier DsaWithSha1 = new DerObjectIdentifier( "1.3.14.3.2.27" );
    public static readonly DerObjectIdentifier Sha1WithRsa = new DerObjectIdentifier( "1.3.14.3.2.29" );
    public static readonly DerObjectIdentifier ElGamalAlgorithm = new DerObjectIdentifier( "1.3.14.7.2.1.1" );
}
public abstract class PkcsObjectIdentifiers
{
    public const string Pkcs1 = "1.2.840.113549.1.1";
    public const string Pkcs3 = "1.2.840.113549.1.3";
    public const string Pkcs5 = "1.2.840.113549.1.5";
    public const string EncryptionAlgorithm = "1.2.840.113549.3";
    public const string DigestAlgorithm = "1.2.840.113549.2";
    public const string Pkcs7 = "1.2.840.113549.1.7";
    public const string Pkcs9 = "1.2.840.113549.1.9";
    public const string CertTypes = "1.2.840.113549.1.9.22";
    public const string CrlTypes = "1.2.840.113549.1.9.23";
    public const string IdCT = "1.2.840.113549.1.9.16.1";
    public const string IdCti = "1.2.840.113549.1.9.16.6";
    public const string IdAA = "1.2.840.113549.1.9.16.2";
    public const string IdSpq = "1.2.840.113549.1.9.16.5";
    public const string Pkcs12 = "1.2.840.113549.1.12";
    public const string BagTypes = "1.2.840.113549.1.12.10.1";
    public const string Pkcs12PbeIds = "1.2.840.113549.1.12.1";
    public static readonly DerObjectIdentifier RsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.1" );
    public static readonly DerObjectIdentifier MD2WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.2" );
    public static readonly DerObjectIdentifier MD4WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.3" );
    public static readonly DerObjectIdentifier MD5WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.4" );
    public static readonly DerObjectIdentifier Sha1WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.5" );
    public static readonly DerObjectIdentifier SrsaOaepEncryptionSet = new DerObjectIdentifier( "1.2.840.113549.1.1.6" );
    public static readonly DerObjectIdentifier IdRsaesOaep = new DerObjectIdentifier( "1.2.840.113549.1.1.7" );
    public static readonly DerObjectIdentifier IdMgf1 = new DerObjectIdentifier( "1.2.840.113549.1.1.8" );
    public static readonly DerObjectIdentifier IdPSpecified = new DerObjectIdentifier( "1.2.840.113549.1.1.9" );
    public static readonly DerObjectIdentifier IdRsassaPss = new DerObjectIdentifier( "1.2.840.113549.1.1.10" );
    public static readonly DerObjectIdentifier Sha256WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.11" );
    public static readonly DerObjectIdentifier Sha384WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.12" );
    public static readonly DerObjectIdentifier Sha512WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.13" );
    public static readonly DerObjectIdentifier Sha224WithRsaEncryption = new DerObjectIdentifier( "1.2.840.113549.1.1.14" );
    public static readonly DerObjectIdentifier DhKeyAgreement = new DerObjectIdentifier( "1.2.840.113549.1.3.1" );
    public static readonly DerObjectIdentifier PbeWithMD2AndDesCbc = new DerObjectIdentifier( "1.2.840.113549.1.5.1" );
    public static readonly DerObjectIdentifier PbeWithMD2AndRC2Cbc = new DerObjectIdentifier( "1.2.840.113549.1.5.4" );
    public static readonly DerObjectIdentifier PbeWithMD5AndDesCbc = new DerObjectIdentifier( "1.2.840.113549.1.5.3" );
    public static readonly DerObjectIdentifier PbeWithMD5AndRC2Cbc = new DerObjectIdentifier( "1.2.840.113549.1.5.6" );
    public static readonly DerObjectIdentifier PbeWithSha1AndDesCbc = new DerObjectIdentifier( "1.2.840.113549.1.5.10" );
    public static readonly DerObjectIdentifier PbeWithSha1AndRC2Cbc = new DerObjectIdentifier( "1.2.840.113549.1.5.11" );
    public static readonly DerObjectIdentifier IdPbeS2 = new DerObjectIdentifier( "1.2.840.113549.1.5.13" );
    public static readonly DerObjectIdentifier IdPbkdf2 = new DerObjectIdentifier( "1.2.840.113549.1.5.12" );
    public static readonly DerObjectIdentifier DesEde3Cbc = new DerObjectIdentifier( "1.2.840.113549.3.7" );
    public static readonly DerObjectIdentifier RC2Cbc = new DerObjectIdentifier( "1.2.840.113549.3.2" );
    public static readonly DerObjectIdentifier MD2 = new DerObjectIdentifier( "1.2.840.113549.2.2" );
    public static readonly DerObjectIdentifier MD4 = new DerObjectIdentifier( "1.2.840.113549.2.4" );
    public static readonly DerObjectIdentifier MD5 = new DerObjectIdentifier( "1.2.840.113549.2.5" );
    public static readonly DerObjectIdentifier IdHmacWithSha1 = new DerObjectIdentifier( "1.2.840.113549.2.7" );
    public static readonly DerObjectIdentifier IdHmacWithSha224 = new DerObjectIdentifier( "1.2.840.113549.2.8" );
    public static readonly DerObjectIdentifier IdHmacWithSha256 = new DerObjectIdentifier( "1.2.840.113549.2.9" );
    public static readonly DerObjectIdentifier IdHmacWithSha384 = new DerObjectIdentifier( "1.2.840.113549.2.10" );
    public static readonly DerObjectIdentifier IdHmacWithSha512 = new DerObjectIdentifier( "1.2.840.113549.2.11" );
    public static readonly DerObjectIdentifier Data = new DerObjectIdentifier( "1.2.840.113549.1.7.1" );
    public static readonly DerObjectIdentifier SignedData = new DerObjectIdentifier( "1.2.840.113549.1.7.2" );
    public static readonly DerObjectIdentifier EnvelopedData = new DerObjectIdentifier( "1.2.840.113549.1.7.3" );
    public static readonly DerObjectIdentifier SignedAndEnvelopedData = new DerObjectIdentifier( "1.2.840.113549.1.7.4" );
    public static readonly DerObjectIdentifier DigestedData = new DerObjectIdentifier( "1.2.840.113549.1.7.5" );
    public static readonly DerObjectIdentifier EncryptedData = new DerObjectIdentifier( "1.2.840.113549.1.7.6" );
    public static readonly DerObjectIdentifier Pkcs9AtEmailAddress = new DerObjectIdentifier( "1.2.840.113549.1.9.1" );
    public static readonly DerObjectIdentifier Pkcs9AtUnstructuredName = new DerObjectIdentifier( "1.2.840.113549.1.9.2" );
    public static readonly DerObjectIdentifier Pkcs9AtContentType = new DerObjectIdentifier( "1.2.840.113549.1.9.3" );
    public static readonly DerObjectIdentifier Pkcs9AtMessageDigest = new DerObjectIdentifier( "1.2.840.113549.1.9.4" );
    public static readonly DerObjectIdentifier Pkcs9AtSigningTime = new DerObjectIdentifier( "1.2.840.113549.1.9.5" );
    public static readonly DerObjectIdentifier Pkcs9AtCounterSignature = new DerObjectIdentifier( "1.2.840.113549.1.9.6" );
    public static readonly DerObjectIdentifier Pkcs9AtChallengePassword = new DerObjectIdentifier( "1.2.840.113549.1.9.7" );
    public static readonly DerObjectIdentifier Pkcs9AtUnstructuredAddress = new DerObjectIdentifier( "1.2.840.113549.1.9.8" );
    public static readonly DerObjectIdentifier Pkcs9AtExtendedCertificateAttributes = new DerObjectIdentifier( "1.2.840.113549.1.9.9" );
    public static readonly DerObjectIdentifier Pkcs9AtSigningDescription = new DerObjectIdentifier( "1.2.840.113549.1.9.13" );
    public static readonly DerObjectIdentifier Pkcs9AtExtensionRequest = new DerObjectIdentifier( "1.2.840.113549.1.9.14" );
    public static readonly DerObjectIdentifier Pkcs9AtSmimeCapabilities = new DerObjectIdentifier( "1.2.840.113549.1.9.15" );
    public static readonly DerObjectIdentifier IdSmime = new DerObjectIdentifier( "1.2.840.113549.1.9.16" );
    public static readonly DerObjectIdentifier Pkcs9AtFriendlyName = new DerObjectIdentifier( "1.2.840.113549.1.9.20" );
    public static readonly DerObjectIdentifier Pkcs9AtLocalKeyID = new DerObjectIdentifier( "1.2.840.113549.1.9.21" );
    [Obsolete( "Use X509Certificate instead" )]
    public static readonly DerObjectIdentifier X509CertType = new DerObjectIdentifier( "1.2.840.113549.1.9.22.1" );
    public static readonly DerObjectIdentifier X509Certificate = new DerObjectIdentifier( "1.2.840.113549.1.9.22.1" );
    public static readonly DerObjectIdentifier SdsiCertificate = new DerObjectIdentifier( "1.2.840.113549.1.9.22.2" );
    public static readonly DerObjectIdentifier X509Crl = new DerObjectIdentifier( "1.2.840.113549.1.9.23.1" );
    public static readonly DerObjectIdentifier IdAlg = IdSmime.Branch( "3" );
    public static readonly DerObjectIdentifier IdAlgEsdh = IdAlg.Branch( "5" );
    public static readonly DerObjectIdentifier IdAlgCms3DesWrap = IdAlg.Branch( "6" );
    public static readonly DerObjectIdentifier IdAlgCmsRC2Wrap = IdAlg.Branch( "7" );
    public static readonly DerObjectIdentifier IdAlgPwriKek = IdAlg.Branch( "9" );
    public static readonly DerObjectIdentifier IdAlgSsdh = IdAlg.Branch( "10" );
    public static readonly DerObjectIdentifier IdRsaKem = IdAlg.Branch( "14" );
    public static readonly DerObjectIdentifier PreferSignedData = Pkcs9AtSmimeCapabilities.Branch( "1" );
    public static readonly DerObjectIdentifier CannotDecryptAny = Pkcs9AtSmimeCapabilities.Branch( "2" );
    public static readonly DerObjectIdentifier SmimeCapabilitiesVersions = Pkcs9AtSmimeCapabilities.Branch( "3" );
    public static readonly DerObjectIdentifier IdAAReceiptRequest = IdSmime.Branch( "2.1" );
    public static readonly DerObjectIdentifier IdCTAuthData = new DerObjectIdentifier( "1.2.840.113549.1.9.16.1.2" );
    public static readonly DerObjectIdentifier IdCTTstInfo = new DerObjectIdentifier( "1.2.840.113549.1.9.16.1.4" );
    public static readonly DerObjectIdentifier IdCTCompressedData = new DerObjectIdentifier( "1.2.840.113549.1.9.16.1.9" );
    public static readonly DerObjectIdentifier IdCTAuthEnvelopedData = new DerObjectIdentifier( "1.2.840.113549.1.9.16.1.23" );
    public static readonly DerObjectIdentifier IdCTTimestampedData = new DerObjectIdentifier( "1.2.840.113549.1.9.16.1.31" );
    public static readonly DerObjectIdentifier IdCtiEtsProofOfOrigin = new DerObjectIdentifier( "1.2.840.113549.1.9.16.6.1" );
    public static readonly DerObjectIdentifier IdCtiEtsProofOfReceipt = new DerObjectIdentifier( "1.2.840.113549.1.9.16.6.2" );
    public static readonly DerObjectIdentifier IdCtiEtsProofOfDelivery = new DerObjectIdentifier( "1.2.840.113549.1.9.16.6.3" );
    public static readonly DerObjectIdentifier IdCtiEtsProofOfSender = new DerObjectIdentifier( "1.2.840.113549.1.9.16.6.4" );
    public static readonly DerObjectIdentifier IdCtiEtsProofOfApproval = new DerObjectIdentifier( "1.2.840.113549.1.9.16.6.5" );
    public static readonly DerObjectIdentifier IdCtiEtsProofOfCreation = new DerObjectIdentifier( "1.2.840.113549.1.9.16.6.6" );
    public static readonly DerObjectIdentifier IdAAContentHint = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.4" );
    public static readonly DerObjectIdentifier IdAAMsgSigDigest = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.5" );
    public static readonly DerObjectIdentifier IdAAContentReference = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.10" );
    public static readonly DerObjectIdentifier IdAAEncrypKeyPref = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.11" );
    public static readonly DerObjectIdentifier IdAASigningCertificate = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.12" );
    public static readonly DerObjectIdentifier IdAASigningCertificateV2 = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.47" );
    public static readonly DerObjectIdentifier IdAAContentIdentifier = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.7" );
    public static readonly DerObjectIdentifier IdAASignatureTimeStampToken = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.14" );
    public static readonly DerObjectIdentifier IdAAEtsSigPolicyID = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.15" );
    public static readonly DerObjectIdentifier IdAAEtsCommitmentType = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.16" );
    public static readonly DerObjectIdentifier IdAAEtsSignerLocation = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.17" );
    public static readonly DerObjectIdentifier IdAAEtsSignerAttr = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.18" );
    public static readonly DerObjectIdentifier IdAAEtsOtherSigCert = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.19" );
    public static readonly DerObjectIdentifier IdAAEtsContentTimestamp = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.20" );
    public static readonly DerObjectIdentifier IdAAEtsCertificateRefs = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.21" );
    public static readonly DerObjectIdentifier IdAAEtsRevocationRefs = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.22" );
    public static readonly DerObjectIdentifier IdAAEtsCertValues = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.23" );
    public static readonly DerObjectIdentifier IdAAEtsRevocationValues = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.24" );
    public static readonly DerObjectIdentifier IdAAEtsEscTimeStamp = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.25" );
    public static readonly DerObjectIdentifier IdAAEtsCertCrlTimestamp = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.26" );
    public static readonly DerObjectIdentifier IdAAEtsArchiveTimestamp = new DerObjectIdentifier( "1.2.840.113549.1.9.16.2.27" );
    [Obsolete( "Use 'IdAAEtsSigPolicyID' instead" )]
    public static readonly DerObjectIdentifier IdAASigPolicyID = IdAAEtsSigPolicyID;
    [Obsolete( "Use 'IdAAEtsCommitmentType' instead" )]
    public static readonly DerObjectIdentifier IdAACommitmentType = IdAAEtsCommitmentType;
    [Obsolete( "Use 'IdAAEtsSignerLocation' instead" )]
    public static readonly DerObjectIdentifier IdAASignerLocation = IdAAEtsSignerLocation;
    [Obsolete( "Use 'IdAAEtsOtherSigCert' instead" )]
    public static readonly DerObjectIdentifier IdAAOtherSigCert = IdAAEtsOtherSigCert;
    public static readonly DerObjectIdentifier IdSpqEtsUri = new DerObjectIdentifier( "1.2.840.113549.1.9.16.5.1" );
    public static readonly DerObjectIdentifier IdSpqEtsUNotice = new DerObjectIdentifier( "1.2.840.113549.1.9.16.5.2" );
    public static readonly DerObjectIdentifier KeyBag = new DerObjectIdentifier( "1.2.840.113549.1.12.10.1.1" );
    public static readonly DerObjectIdentifier Pkcs8ShroudedKeyBag = new DerObjectIdentifier( "1.2.840.113549.1.12.10.1.2" );
    public static readonly DerObjectIdentifier CertBag = new DerObjectIdentifier( "1.2.840.113549.1.12.10.1.3" );
    public static readonly DerObjectIdentifier CrlBag = new DerObjectIdentifier( "1.2.840.113549.1.12.10.1.4" );
    public static readonly DerObjectIdentifier SecretBag = new DerObjectIdentifier( "1.2.840.113549.1.12.10.1.5" );
    public static readonly DerObjectIdentifier SafeContentsBag = new DerObjectIdentifier( "1.2.840.113549.1.12.10.1.6" );
    public static readonly DerObjectIdentifier PbeWithShaAnd128BitRC4 = new DerObjectIdentifier( "1.2.840.113549.1.12.1.1" );
    public static readonly DerObjectIdentifier PbeWithShaAnd40BitRC4 = new DerObjectIdentifier( "1.2.840.113549.1.12.1.2" );
    public static readonly DerObjectIdentifier PbeWithShaAnd3KeyTripleDesCbc = new DerObjectIdentifier( "1.2.840.113549.1.12.1.3" );
    public static readonly DerObjectIdentifier PbeWithShaAnd2KeyTripleDesCbc = new DerObjectIdentifier( "1.2.840.113549.1.12.1.4" );
    public static readonly DerObjectIdentifier PbeWithShaAnd128BitRC2Cbc = new DerObjectIdentifier( "1.2.840.113549.1.12.1.5" );
    public static readonly DerObjectIdentifier PbewithShaAnd40BitRC2Cbc = new DerObjectIdentifier( "1.2.840.113549.1.12.1.6" );
}
internal abstract class Enums
{
    internal static Enum GetEnumValue( Type enumType, string s )
    {
        if (!IsEnumType( enumType ))
            throw new ArgumentException( "Not an enumeration type", nameof( enumType ) );
        s = s.Length > 0 && char.IsLetter( s[0] ) && s.IndexOf( ',' ) < 0 ? s.Replace( '-', '_' ) : throw new ArgumentException();
        s = s.Replace( '/', '_' );
        return (Enum)Enum.Parse( enumType, s, false );
    }

    internal static Array GetEnumValues( Type enumType ) => IsEnumType( enumType ) ? Enum.GetValues( enumType ) : throw new ArgumentException( "Not an enumeration type", nameof( enumType ) );

    internal static Enum GetArbitraryValue( Type enumType )
    {
        Array enumValues = GetEnumValues( enumType );
        int index = (int)(DateTimeUtilities.CurrentUnixMs() & int.MaxValue) % enumValues.Length;
        return (Enum)enumValues.GetValue( index );
    }

    internal static bool IsEnumType( Type t ) => t.IsEnum;
}
public class DateTimeUtilities
{
    public static readonly DateTime UnixEpoch = new DateTime( 1970, 1, 1 );

    private DateTimeUtilities()
    {
    }

    public static long DateTimeToUnixMs( DateTime dateTime )
    {
        if (dateTime.CompareTo( (object)UnixEpoch ) < 0)
            throw new ArgumentException( "DateTime value may not be before the epoch", nameof( dateTime ) );
        return (dateTime.Ticks - UnixEpoch.Ticks) / 10000L;
    }

    public static DateTime UnixMsToDateTime( long unixMs ) => new DateTime( (unixMs * 10000L) + UnixEpoch.Ticks );

    public static long CurrentUnixMs() => DateTimeToUnixMs( DateTime.UtcNow );
}
public class DerInteger : Asn1Object
{
    private readonly byte[] bytes;

    public static DerInteger GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case DerInteger _:
                return (DerInteger)obj;
            default:
                throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
        }
    }

    public static DerInteger GetInstance( Asn1TaggedObject obj, bool isExplicit )
    {
        Asn1Object asn1Object = obj != null ? obj.GetObject() : throw new ArgumentNullException( nameof( obj ) );
        return isExplicit || asn1Object is DerInteger ? GetInstance( asn1Object ) : new DerInteger( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
    }

    public DerInteger( int value ) => this.bytes = BigInteger.ValueOf( value ).ToByteArray();

    public DerInteger( BigInteger value ) => this.bytes = value != null ? value.ToByteArray() : throw new ArgumentNullException( nameof( value ) );

    public DerInteger( byte[] bytes ) => this.bytes = bytes;

    public BigInteger Value => new BigInteger( this.bytes );

    public BigInteger PositiveValue => new BigInteger( 1, this.bytes );

    internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 2, this.bytes );

    protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.bytes );

    protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerInteger derInteger && Arrays.AreEqual( this.bytes, derInteger.bytes );

    public override string ToString() => this.Value.ToString();
}
public class DerNumericString : DerStringBase
{
    private readonly string str;

    public static DerNumericString GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case DerNumericString _:
                return (DerNumericString)obj;
            default:
                throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
        }
    }

    public static DerNumericString GetInstance( Asn1TaggedObject obj, bool isExplicit )
    {
        Asn1Object asn1Object = obj.GetObject();
        return isExplicit || asn1Object is DerNumericString ? GetInstance( asn1Object ) : new DerNumericString( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
    }

    public DerNumericString( byte[] str )
      : this( Strings.FromAsciiByteArray( str ), false )
    {
    }

    public DerNumericString( string str )
      : this( str, false )
    {
    }

    public DerNumericString( string str, bool validate )
    {
        if (str == null)
            throw new ArgumentNullException( nameof( str ) );
        this.str = !validate || IsNumericString( str ) ? str : throw new ArgumentException( "string contains illegal characters", nameof( str ) );
    }

    public override string GetString() => this.str;

    public byte[] GetOctets() => Strings.ToAsciiByteArray( this.str );

    internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 18, this.GetOctets() );

    protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerNumericString derNumericString && this.str.Equals( derNumericString.str );

    public static bool IsNumericString( string str )
    {
        foreach (char c in str)
        {
            if (c > '\u007F' || (c != ' ' && !char.IsDigit( c )))
                return false;
        }
        return true;
    }
}
public abstract class Strings
{
    internal static bool IsOneOf( string s, params string[] candidates )
    {
        foreach (string candidate in candidates)
        {
            if (s == candidate)
                return true;
        }
        return false;
    }

    public static string FromByteArray( byte[] bs )
    {
        char[] chArray = new char[bs.Length];
        for (int index = 0; index < chArray.Length; ++index)
            chArray[index] = Convert.ToChar( bs[index] );
        return new string( chArray );
    }

    public static byte[] ToByteArray( char[] cs )
    {
        byte[] byteArray = new byte[cs.Length];
        for (int index = 0; index < byteArray.Length; ++index)
            byteArray[index] = Convert.ToByte( cs[index] );
        return byteArray;
    }

    public static byte[] ToByteArray( string s )
    {
        byte[] byteArray = new byte[s.Length];
        for (int index = 0; index < byteArray.Length; ++index)
            byteArray[index] = Convert.ToByte( s[index] );
        return byteArray;
    }

    public static string FromAsciiByteArray( byte[] bytes ) => Encoding.ASCII.GetString( bytes, 0, bytes.Length );

    public static byte[] ToAsciiByteArray( char[] cs ) => Encoding.ASCII.GetBytes( cs );

    public static byte[] ToAsciiByteArray( string s ) => Encoding.ASCII.GetBytes( s );

    public static string FromUtf8ByteArray( byte[] bytes ) => Encoding.UTF8.GetString( bytes, 0, bytes.Length );

    public static byte[] ToUtf8ByteArray( char[] cs ) => Encoding.UTF8.GetBytes( cs );

    public static byte[] ToUtf8ByteArray( string s ) => Encoding.UTF8.GetBytes( s );
}
public abstract class DerStringBase : Asn1Object, IAsn1String
{
    public abstract string GetString();

    public override string ToString() => this.GetString();

    protected override int Asn1GetHashCode() => this.GetString().GetHashCode();
}
public interface IAsn1String
{
    string GetString();
}
public abstract class Asn1TaggedObject : Asn1Object, Asn1TaggedObjectParser, IAsn1Convertible
{
    internal int tagNo;
    internal bool explicitly = true;
    internal Asn1Encodable obj;

    internal static bool IsConstructed( bool isExplicit, Asn1Object obj )
    {
        if (!isExplicit)
        {
            switch (obj)
            {
                case Asn1Sequence _:
                case Asn1Set _:
                    break;
                case Asn1TaggedObject asn1TaggedObject:
                    return IsConstructed( asn1TaggedObject.IsExplicit(), asn1TaggedObject.GetObject() );
                default:
                    return false;
            }
        }
        return true;
    }

    public static Asn1TaggedObject GetInstance( Asn1TaggedObject obj, bool explicitly )
    {
        if (explicitly)
            return (Asn1TaggedObject)obj.GetObject();
        throw new ArgumentException( "implicitly tagged tagged object" );
    }

    public static Asn1TaggedObject GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case Asn1TaggedObject _:
                return (Asn1TaggedObject)obj;
            default:
                throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }
    }

    protected Asn1TaggedObject( int tagNo, Asn1Encodable obj )
    {
        this.explicitly = true;
        this.tagNo = tagNo;
        this.obj = obj;
    }

    protected Asn1TaggedObject( bool explicitly, int tagNo, Asn1Encodable obj )
    {
        this.explicitly = explicitly || obj is IAsn1Choice;
        this.tagNo = tagNo;
        this.obj = obj;
    }

    protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is Asn1TaggedObject asn1TaggedObject && this.tagNo == asn1TaggedObject.tagNo && this.explicitly == asn1TaggedObject.explicitly && Equals( this.GetObject(), asn1TaggedObject.GetObject() );

    protected override int Asn1GetHashCode()
    {
        int hashCode = this.tagNo.GetHashCode();
        if (this.obj != null)
            hashCode ^= this.obj.GetHashCode();
        return hashCode;
    }

    public int TagNo => this.tagNo;

    public bool IsExplicit() => this.explicitly;

    public bool IsEmpty() => false;

    public Asn1Object GetObject() => this.obj != null ? this.obj.ToAsn1Object() : null;

    public IAsn1Convertible GetObjectParser( int tag, bool isExplicit )
    {
        switch (tag)
        {
            case 4:
                return Asn1OctetString.GetInstance( this, isExplicit ).Parser;
            case 16:
                return Asn1Sequence.GetInstance( this, isExplicit ).Parser;
            case 17:
                return Asn1Set.GetInstance( this, isExplicit ).Parser;
            default:
                if (isExplicit)
                    return this.GetObject();
                throw Platform.CreateNotImplementedException( "implicit tagging for tag: " + tag );
        }
    }

    public override string ToString() => "[" + tagNo + "]" + obj;
}
public interface Asn1TaggedObjectParser : IAsn1Convertible
{
    int TagNo { get; }

    IAsn1Convertible GetObjectParser( int tag, bool isExplicit );
}
public abstract class Asn1OctetString : Asn1Object, Asn1OctetStringParser, IAsn1Convertible
{
    internal byte[] str;

    public static Asn1OctetString GetInstance( Asn1TaggedObject obj, bool isExplicit )
    {
        Asn1Object asn1Object = obj.GetObject();
        return isExplicit || asn1Object is Asn1OctetString ? GetInstance( asn1Object ) : BerOctetString.FromSequence( Asn1Sequence.GetInstance( asn1Object ) );
    }

    public static Asn1OctetString GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case Asn1OctetString _:
                return (Asn1OctetString)obj;
            case Asn1TaggedObject _:
                return GetInstance( ((Asn1TaggedObject)obj).GetObject() );
            default:
                throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
        }
    }

    internal Asn1OctetString( byte[] str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

    internal Asn1OctetString( Asn1Encodable obj )
    {
        try
        {
            this.str = obj.GetEncoded( "DER" );
        }
        catch (IOException ex)
        {
            throw new ArgumentException( "Error processing object : " + ex.ToString() );
        }
    }

    public Stream GetOctetStream() => new MemoryStream( this.str, false );

    public Asn1OctetStringParser Parser => this;

    public virtual byte[] GetOctets() => this.str;

    protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.GetOctets() );

    protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerOctetString derOctetString && Arrays.AreEqual( this.GetOctets(), derOctetString.GetOctets() );

    public override string ToString() => "#" + Hex.ToHexString( this.str );
}
public interface Asn1OctetStringParser : IAsn1Convertible
{
    Stream GetOctetStream();
}
public class DerOctetString : Asn1OctetString
{
    public DerOctetString( byte[] str )
      : base( str )
    {
    }

    public DerOctetString( Asn1Encodable obj )
      : base( obj )
    {
    }

    internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 4, this.str );

    internal static void Encode( DerOutputStream derOut, byte[] bytes, int offset, int length ) => derOut.WriteEncoded( 4, bytes, offset, length );
}
public sealed class Hex
{
    private static readonly IEncoder encoder = new HexEncoder();

    private Hex()
    {
    }

    public static string ToHexString( byte[] data ) => ToHexString( data, 0, data.Length );

    public static string ToHexString( byte[] data, int off, int length ) => Strings.FromAsciiByteArray( Encode( data, off, length ) );

    public static byte[] Encode( byte[] data ) => Encode( data, 0, data.Length );

    public static byte[] Encode( byte[] data, int off, int length )
    {
        MemoryStream outStream = new MemoryStream( length * 2 );
        encoder.Encode( data, off, length, outStream );
        return outStream.ToArray();
    }

    public static int Encode( byte[] data, Stream outStream ) => encoder.Encode( data, 0, data.Length, outStream );

    public static int Encode( byte[] data, int off, int length, Stream outStream ) => encoder.Encode( data, off, length, outStream );

    public static byte[] Decode( byte[] data )
    {
        MemoryStream outStream = new MemoryStream( (data.Length + 1) / 2 );
        encoder.Decode( data, 0, data.Length, outStream );
        return outStream.ToArray();
    }

    public static byte[] Decode( string data )
    {
        MemoryStream outStream = new MemoryStream( (data.Length + 1) / 2 );
        encoder.DecodeString( data, outStream );
        return outStream.ToArray();
    }

    public static int Decode( string data, Stream outStream ) => encoder.DecodeString( data, outStream );
}
public class HexEncoder : IEncoder
{
    protected readonly byte[] encodingTable = new byte[16]
    {
       48,
       49,
       50,
       51,
       52,
       53,
       54,
       55,
       56,
       57,
       97,
       98,
       99,
       100,
       101,
       102
    };
    protected readonly byte[] decodingTable = new byte[128];

    protected void InitialiseDecodingTable()
    {
        Arrays.Fill( this.decodingTable, byte.MaxValue );
        for (int index = 0; index < this.encodingTable.Length; ++index)
            this.decodingTable[this.encodingTable[index]] = (byte)index;
        this.decodingTable[65] = this.decodingTable[97];
        this.decodingTable[66] = this.decodingTable[98];
        this.decodingTable[67] = this.decodingTable[99];
        this.decodingTable[68] = this.decodingTable[100];
        this.decodingTable[69] = this.decodingTable[101];
        this.decodingTable[70] = this.decodingTable[102];
    }

    public HexEncoder() => this.InitialiseDecodingTable();

    public int Encode( byte[] data, int off, int length, Stream outStream )
    {
        for (int index = off; index < off + length; ++index)
        {
            int num = data[index];
            outStream.WriteByte( this.encodingTable[num >> 4] );
            outStream.WriteByte( this.encodingTable[num & 15] );
        }
        return length * 2;
    }

    private static bool Ignore( char c ) => c == '\n' || c == '\r' || c == '\t' || c == ' ';

    public int Decode( byte[] data, int off, int length, Stream outStream )
    {
        int num1 = 0;
        int num2 = off + length;
        while (num2 > off && Ignore( (char)data[num2 - 1] ))
            --num2;
        int index1 = off;
        while (index1 < num2)
        {
            while (index1 < num2 && Ignore( (char)data[index1] ))
                ++index1;
            byte[] decodingTable1 = this.decodingTable;
            byte[] numArray1 = data;
            int index2 = index1;
            int index3 = index2 + 1;
            int index4 = numArray1[index2];
            byte num3 = decodingTable1[index4];
            while (index3 < num2 && Ignore( (char)data[index3] ))
                ++index3;
            byte[] decodingTable2 = this.decodingTable;
            byte[] numArray2 = data;
            int index5 = index3;
            index1 = index5 + 1;
            int index6 = numArray2[index5];
            byte num4 = decodingTable2[index6];
            if ((num3 | num4) >= 128)
                throw new IOException( "invalid characters encountered in Hex data" );
            outStream.WriteByte( (byte)(((uint)num3 << 4) | num4) );
            ++num1;
        }
        return num1;
    }

    public int DecodeString( string data, Stream outStream )
    {
        int num1 = 0;
        int length = data.Length;
        while (length > 0 && Ignore( data[length - 1] ))
            --length;
        int index1 = 0;
        while (index1 < length)
        {
            while (index1 < length && Ignore( data[index1] ))
                ++index1;
            byte[] decodingTable1 = this.decodingTable;
            string str1 = data;
            int index2 = index1;
            int index3 = index2 + 1;
            int index4 = str1[index2];
            byte num2 = decodingTable1[index4];
            while (index3 < length && Ignore( data[index3] ))
                ++index3;
            byte[] decodingTable2 = this.decodingTable;
            string str2 = data;
            int index5 = index3;
            index1 = index5 + 1;
            int index6 = str2[index5];
            byte num3 = decodingTable2[index6];
            if ((num2 | num3) >= 128)
                throw new IOException( "invalid characters encountered in Hex data" );
            outStream.WriteByte( (byte)(((uint)num2 << 4) | num3) );
            ++num1;
        }
        return num1;
    }
}
public class HexTranslator : ITranslator
{
    private static readonly byte[] hexTable = new byte[16]
    {
       48,
       49,
       50,
       51,
       52,
       53,
       54,
       55,
       56,
       57,
       97,
       98,
       99,
       100,
       101,
       102
    };

    public int GetEncodedBlockSize() => 2;

    public int Encode( byte[] input, int inOff, int length, byte[] outBytes, int outOff )
    {
        int num1 = 0;
        int num2 = 0;
        while (num1 < length)
        {
            outBytes[outOff + num2] = hexTable[(input[inOff] >> 4) & 15];
            outBytes[outOff + num2 + 1] = hexTable[input[inOff] & 15];
            ++inOff;
            ++num1;
            num2 += 2;
        }
        return length * 2;
    }

    public int GetDecodedBlockSize() => 1;

    public int Decode( byte[] input, int inOff, int length, byte[] outBytes, int outOff )
    {
        int num1 = length / 2;
        for (int index1 = 0; index1 < num1; ++index1)
        {
            byte num2 = input[inOff + (index1 * 2)];
            byte num3 = input[inOff + (index1 * 2) + 1];
            outBytes[outOff] = num2 >= 97 ? (byte)((num2 - 97 + 10) << 4) : (byte)((num2 - 48) << 4);
            if (num3 < 97)
            {
                byte[] numArray;
                IntPtr index2;
                (numArray = outBytes)[(int)(index2 = (IntPtr)outOff)] = (byte)(numArray[(int)index2] + (uint)(byte)(num3 - 48U));
            }
            else
            {
                byte[] numArray;
                IntPtr index3;
                (numArray = outBytes)[(int)(index3 = (IntPtr)outOff)] = (byte)(numArray[(int)index3] + (uint)(byte)(num3 - 97 + 10));
            }
            ++outOff;
        }
        return num1;
    }
}
public class Gost28147Engine : IBlockCipher
{
    private const int BlockSize = 8;
    private int[] workingKey = null;
    private bool forEncryption;
    private byte[] S = Sbox_Default;
    private static readonly byte[] Sbox_Default = new byte[128]
    {
       4,
       10,
       9,
       2,
       13,
       8,
       0,
       14,
       6,
       11,
       1,
       12,
       7,
       15,
       5,
       3,
       14,
       11,
       4,
       12,
       6,
       13,
       15,
       10,
       2,
       3,
       8,
       1,
       0,
       7,
       5,
       9,
       5,
       8,
       1,
       13,
       10,
       3,
       4,
       2,
       14,
       15,
       12,
       7,
       6,
       0,
       9,
       11,
       7,
       13,
       10,
       1,
       0,
       8,
       9,
       15,
       14,
       4,
       6,
       12,
       11,
       2,
       5,
       3,
       6,
       12,
       7,
       1,
       5,
       15,
       13,
       8,
       4,
       10,
       9,
       14,
       0,
       3,
       11,
       2,
       4,
       11,
       10,
       0,
       7,
       2,
       1,
       13,
       3,
       6,
       8,
       5,
       9,
       12,
       15,
       14,
       13,
       11,
       4,
       1,
       3,
       15,
       5,
       9,
       0,
       10,
       14,
       7,
       6,
       8,
       2,
       12,
       1,
       15,
       13,
       0,
       5,
       7,
       10,
       4,
       9,
       2,
       3,
       14,
       6,
       11,
       8,
       12
    };
    private static readonly byte[] ESbox_Test = new byte[128]
    {
       4,
       2,
       15,
       5,
       9,
       1,
       0,
       8,
       14,
       3,
       11,
       12,
       13,
       7,
       10,
       6,
       12,
       9,
       15,
       14,
       8,
       1,
       3,
       10,
       2,
       7,
       4,
       13,
       6,
       0,
       11,
       5,
       13,
       8,
       14,
       12,
       7,
       3,
       9,
       10,
       1,
       5,
       2,
       4,
       6,
       15,
       0,
       11,
       14,
       9,
       11,
       2,
       5,
       15,
       7,
       1,
       0,
       13,
       12,
       6,
       10,
       4,
       3,
       8,
       3,
       14,
       5,
       9,
       6,
       8,
       0,
       13,
       10,
       11,
       7,
       12,
       2,
       1,
       15,
       4,
       8,
       15,
       6,
       11,
       1,
       9,
       12,
       5,
       13,
       3,
       7,
       10,
       0,
       14,
       2,
       4,
       9,
       11,
       12,
       0,
       3,
       6,
       7,
       5,
       4,
       8,
       14,
       15,
       1,
       10,
       2,
       13,
       12,
       6,
       5,
       2,
       11,
       0,
       9,
       13,
       3,
       14,
       7,
       10,
       15,
       4,
       1,
       8
    };
    private static readonly byte[] ESbox_A = new byte[128]
    {
       9,
       6,
       3,
       2,
       8,
       11,
       1,
       7,
       10,
       4,
       14,
       15,
       12,
       0,
       13,
       5,
       3,
       7,
       14,
       9,
       8,
       10,
       15,
       0,
       5,
       2,
       6,
       12,
       11,
       4,
       13,
       1,
       14,
       4,
       6,
       2,
       11,
       3,
       13,
       8,
       12,
       15,
       5,
       10,
       0,
       7,
       1,
       9,
       14,
       7,
       10,
       12,
       13,
       1,
       3,
       9,
       0,
       2,
       11,
       4,
       15,
       8,
       5,
       6,
       11,
       5,
       1,
       9,
       8,
       13,
       15,
       0,
       14,
       4,
       2,
       3,
       12,
       7,
       10,
       6,
       3,
       10,
       13,
       12,
       1,
       2,
       0,
       11,
       7,
       5,
       9,
       4,
       8,
       15,
       14,
       6,
       1,
       13,
       2,
       9,
       7,
       10,
       6,
       0,
       8,
       12,
       4,
       5,
       15,
       3,
       11,
       14,
       11,
       10,
       15,
       5,
       0,
       12,
       14,
       8,
       6,
       2,
       3,
       9,
       1,
       7,
       13,
       4
    };
    private static readonly byte[] ESbox_B = new byte[128]
    {
       8,
       4,
       11,
       1,
       3,
       5,
       0,
       9,
       2,
       14,
       10,
       12,
       13,
       6,
       7,
       15,
       0,
       1,
       2,
       10,
       4,
       13,
       5,
       12,
       9,
       7,
       3,
       15,
       11,
       8,
       6,
       14,
       14,
       12,
       0,
       10,
       9,
       2,
       13,
       11,
       7,
       5,
       8,
       15,
       3,
       6,
       1,
       4,
       7,
       5,
       0,
       13,
       11,
       6,
       1,
       2,
       3,
       10,
       12,
       15,
       4,
       14,
       9,
       8,
       2,
       7,
       12,
       15,
       9,
       5,
       10,
       11,
       1,
       4,
       0,
       13,
       6,
       8,
       14,
       3,
       8,
       3,
       2,
       6,
       4,
       13,
       14,
       11,
       12,
       1,
       7,
       15,
       10,
       0,
       9,
       5,
       5,
       2,
       10,
       11,
       9,
       1,
       12,
       3,
       7,
       4,
       13,
       0,
       6,
       15,
       8,
       14,
       0,
       4,
       11,
       14,
       8,
       3,
       7,
       1,
       10,
       2,
       9,
       6,
       15,
       13,
       5,
       12
    };
    private static readonly byte[] ESbox_C = new byte[128]
    {
       1,
       11,
       12,
       2,
       9,
       13,
       0,
       15,
       4,
       5,
       8,
       14,
       10,
       7,
       6,
       3,
       0,
       1,
       7,
       13,
       11,
       4,
       5,
       2,
       8,
       14,
       15,
       12,
       9,
       10,
       6,
       3,
       8,
       2,
       5,
       0,
       4,
       9,
       15,
       10,
       3,
       7,
       12,
       13,
       6,
       14,
       1,
       11,
       3,
       6,
       0,
       1,
       5,
       13,
       10,
       8,
       11,
       2,
       9,
       7,
       14,
       15,
       12,
       4,
       8,
       13,
       11,
       0,
       4,
       5,
       1,
       2,
       9,
       3,
       12,
       14,
       6,
       15,
       10,
       7,
       12,
       9,
       11,
       1,
       8,
       14,
       2,
       4,
       7,
       3,
       6,
       5,
       10,
       0,
       15,
       13,
       10,
       9,
       6,
       8,
       13,
       14,
       2,
       0,
       15,
       3,
       5,
       11,
       4,
       1,
       12,
       7,
       7,
       4,
       0,
       5,
       10,
       2,
       15,
       14,
       12,
       6,
       1,
       11,
       13,
       9,
       3,
       8
    };
    private static readonly byte[] ESbox_D = new byte[128]
    {
       15,
       12,
       2,
       10,
       6,
       4,
       5,
       0,
       7,
       9,
       14,
       13,
       1,
       11,
       8,
       3,
       11,
       6,
       3,
       4,
       12,
       15,
       14,
       2,
       7,
       13,
       8,
       0,
       5,
       10,
       9,
       1,
       1,
       12,
       11,
       0,
       15,
       14,
       6,
       5,
       10,
       13,
       4,
       8,
       9,
       3,
       7,
       2,
       1,
       5,
       14,
       12,
       10,
       7,
       0,
       13,
       6,
       2,
       11,
       4,
       9,
       3,
       15,
       8,
       0,
       12,
       8,
       9,
       13,
       2,
       10,
       11,
       7,
       3,
       6,
       5,
       4,
       14,
       15,
       1,
       8,
       0,
       15,
       3,
       2,
       5,
       14,
       11,
       1,
       10,
       4,
       7,
       12,
       9,
       13,
       6,
       3,
       0,
       6,
       15,
       1,
       14,
       9,
       2,
       13,
       8,
       12,
       4,
       11,
       10,
       5,
       7,
       1,
       10,
       6,
       8,
       15,
       11,
       0,
       4,
       12,
       3,
       5,
       9,
       7,
       13,
       2,
       14
    };
    private static readonly byte[] DSbox_Test = new byte[128]
    {
       4,
       10,
       9,
       2,
       13,
       8,
       0,
       14,
       6,
       11,
       1,
       12,
       7,
       15,
       5,
       3,
       14,
       11,
       4,
       12,
       6,
       13,
       15,
       10,
       2,
       3,
       8,
       1,
       0,
       7,
       5,
       9,
       5,
       8,
       1,
       13,
       10,
       3,
       4,
       2,
       14,
       15,
       12,
       7,
       6,
       0,
       9,
       11,
       7,
       13,
       10,
       1,
       0,
       8,
       9,
       15,
       14,
       4,
       6,
       12,
       11,
       2,
       5,
       3,
       6,
       12,
       7,
       1,
       5,
       15,
       13,
       8,
       4,
       10,
       9,
       14,
       0,
       3,
       11,
       2,
       4,
       11,
       10,
       0,
       7,
       2,
       1,
       13,
       3,
       6,
       8,
       5,
       9,
       12,
       15,
       14,
       13,
       11,
       4,
       1,
       3,
       15,
       5,
       9,
       0,
       10,
       14,
       7,
       6,
       8,
       2,
       12,
       1,
       15,
       13,
       0,
       5,
       7,
       10,
       4,
       9,
       2,
       3,
       14,
       6,
       11,
       8,
       12
    };
    private static readonly byte[] DSbox_A = new byte[128]
    {
       10,
       4,
       5,
       6,
       8,
       1,
       3,
       7,
       13,
       12,
       14,
       0,
       9,
       2,
       11,
       15,
       5,
       15,
       4,
       0,
       2,
       13,
       11,
       9,
       1,
       7,
       6,
       3,
       12,
       14,
       10,
       8,
       7,
       15,
       12,
       14,
       9,
       4,
       1,
       0,
       3,
       11,
       5,
       2,
       6,
       10,
       8,
       13,
       4,
       10,
       7,
       12,
       0,
       15,
       2,
       8,
       14,
       1,
       6,
       5,
       13,
       11,
       9,
       3,
       7,
       6,
       4,
       11,
       9,
       12,
       2,
       10,
       1,
       8,
       0,
       14,
       15,
       13,
       3,
       5,
       7,
       6,
       2,
       4,
       13,
       9,
       15,
       0,
       10,
       1,
       5,
       11,
       8,
       14,
       12,
       3,
       13,
       14,
       4,
       1,
       7,
       0,
       5,
       10,
       3,
       12,
       8,
       15,
       6,
       2,
       9,
       11,
       1,
       3,
       10,
       9,
       5,
       11,
       4,
       15,
       8,
       6,
       7,
       14,
       13,
       0,
       2,
       12
    };
    private static readonly IDictionary sBoxes = Platform.CreateHashtable();

    static Gost28147Engine()
    {
        AddSBox( "Default", Sbox_Default );
        AddSBox( "E-TEST", ESbox_Test );
        AddSBox( "E-A", ESbox_A );
        AddSBox( "E-B", ESbox_B );
        AddSBox( "E-C", ESbox_C );
        AddSBox( "E-D", ESbox_D );
        AddSBox( "D-TEST", DSbox_Test );
        AddSBox( "D-A", DSbox_A );
    }

    private static void AddSBox( string sBoxName, byte[] sBox ) => sBoxes.Add( Platform.ToUpperInvariant( sBoxName ), sBox );

    public virtual void Init( bool forEncryption, ICipherParameters parameters )
    {
        switch (parameters)
        {
            case ParametersWithSBox _:
                ParametersWithSBox parametersWithSbox = (ParametersWithSBox)parameters;
                byte[] sbox = parametersWithSbox.GetSBox();
                if (sbox.Length != Sbox_Default.Length)
                    throw new ArgumentException( "invalid S-box passed to GOST28147 init" );
                this.S = Arrays.Clone( sbox );
                if (parametersWithSbox.Parameters == null)
                    break;
                this.workingKey = this.generateWorkingKey( forEncryption, ((KeyParameter)parametersWithSbox.Parameters).GetKey() );
                break;
            case KeyParameter _:
                this.workingKey = this.generateWorkingKey( forEncryption, ((KeyParameter)parameters).GetKey() );
                break;
            case null:
                break;
            default:
                throw new ArgumentException( "invalid parameter passed to Gost28147 init - " + Platform.GetTypeName( parameters ) );
        }
    }

    public virtual string AlgorithmName => "Gost28147";

    public virtual bool IsPartialBlockOkay => false;

    public virtual int GetBlockSize() => 8;

    public virtual int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
    {
        if (this.workingKey == null)
            throw new InvalidOperationException( "Gost28147 engine not initialised" );
        Check.DataLength( input, inOff, 8, "input buffer too short" );
        Check.OutputLength( output, outOff, 8, "output buffer too short" );
        this.Gost28147Func( this.workingKey, input, inOff, output, outOff );
        return 8;
    }

    public virtual void Reset()
    {
    }

    private int[] generateWorkingKey( bool forEncryption, byte[] userKey )
    {
        this.forEncryption = forEncryption;
        if (userKey.Length != 32)
            throw new ArgumentException( "Key length invalid. Key needs to be 32 byte - 256 bit!!!" );
        int[] workingKey = new int[8];
        for (int index = 0; index != 8; ++index)
            workingKey[index] = bytesToint( userKey, index * 4 );
        return workingKey;
    }

    private int Gost28147_mainStep( int n1, int key )
    {
        int num1 = key + n1;
        int num2 = this.S[num1 & 15] + (this.S[16 + ((num1 >> 4) & 15)] << 4) + (this.S[32 + ((num1 >> 8) & 15)] << 8) + (this.S[48 + ((num1 >> 12) & 15)] << 12) + (this.S[64 + ((num1 >> 16) & 15)] << 16) + (this.S[80 + ((num1 >> 20) & 15)] << 20) + (this.S[96 + ((num1 >> 24) & 15)] << 24) + (this.S[112 + ((num1 >> 28) & 15)] << 28);
        return (num2 << 11) | num2 >>> 21;
    }

    private void Gost28147Func(
      int[] workingKey,
      byte[] inBytes,
      int inOff,
      byte[] outBytes,
      int outOff )
    {
        int num1 = bytesToint( inBytes, inOff );
        int num2 = bytesToint( inBytes, inOff + 4 );
        if (this.forEncryption)
        {
            for (int index1 = 0; index1 < 3; ++index1)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    int num3 = num1;
                    int num4 = this.Gost28147_mainStep( num1, workingKey[index2] );
                    num1 = num2 ^ num4;
                    num2 = num3;
                }
            }
            for (int index = 7; index > 0; --index)
            {
                int num5 = num1;
                num1 = num2 ^ this.Gost28147_mainStep( num1, workingKey[index] );
                num2 = num5;
            }
        }
        else
        {
            for (int index = 0; index < 8; ++index)
            {
                int num6 = num1;
                num1 = num2 ^ this.Gost28147_mainStep( num1, workingKey[index] );
                num2 = num6;
            }
            for (int index3 = 0; index3 < 3; ++index3)
            {
                for (int index4 = 7; index4 >= 0 && (index3 != 2 || index4 != 0); --index4)
                {
                    int num7 = num1;
                    num1 = num2 ^ this.Gost28147_mainStep( num1, workingKey[index4] );
                    num2 = num7;
                }
            }
        }
        int num8 = num2 ^ this.Gost28147_mainStep( num1, workingKey[0] );
        intTobytes( num1, outBytes, outOff );
        intTobytes( num8, outBytes, outOff + 4 );
    }

    private static int bytesToint( byte[] inBytes, int inOff ) => (int)(inBytes[inOff + 3] << 24 & 4278190080L) + ((inBytes[inOff + 2] << 16) & 16711680) + ((inBytes[inOff + 1] << 8) & 65280) + (inBytes[inOff] & byte.MaxValue);

    private static void intTobytes( int num, byte[] outBytes, int outOff )
    {
        outBytes[outOff + 3] = (byte)(num >> 24);
        outBytes[outOff + 2] = (byte)(num >> 16);
        outBytes[outOff + 1] = (byte)(num >> 8);
        outBytes[outOff] = (byte)num;
    }

    public static byte[] GetSBox( string sBoxName ) => Arrays.Clone( (byte[])sBoxes[Platform.ToUpperInvariant( sBoxName )] ?? throw new ArgumentException( "Unknown S-Box - possible types: \"Default\", \"E-Test\", \"E-A\", \"E-B\", \"E-C\", \"E-D\", \"D-Test\", \"D-A\"." ) );
}
public interface IBlockCipher
{
    string AlgorithmName { get; }

    void Init( bool forEncryption, ICipherParameters parameters );

    int GetBlockSize();

    bool IsPartialBlockOkay { get; }

    int ProcessBlock( byte[] inBuf, int inOff, byte[] outBuf, int outOff );

    void Reset();
}
public interface IXof : IDigest
{
    int DoFinal( byte[] output, int outOff, int outLen );
}
public class SkeinParameters : ICipherParameters
{
    public const int PARAM_TYPE_KEY = 0;
    public const int PARAM_TYPE_CONFIG = 4;
    public const int PARAM_TYPE_PERSONALISATION = 8;
    public const int PARAM_TYPE_PUBLIC_KEY = 12;
    public const int PARAM_TYPE_KEY_IDENTIFIER = 16;
    public const int PARAM_TYPE_NONCE = 20;
    public const int PARAM_TYPE_MESSAGE = 48;
    public const int PARAM_TYPE_OUTPUT = 63;
    private IDictionary parameters;

    public SkeinParameters()
      : this( Platform.CreateHashtable() )
    {
    }

    private SkeinParameters( IDictionary parameters ) => this.parameters = parameters;

    public IDictionary GetParameters() => this.parameters;

    public byte[] GetKey() => (byte[])this.parameters[0];

    public byte[] GetPersonalisation() => (byte[])this.parameters[8];

    public byte[] GetPublicKey() => (byte[])this.parameters[12];

    public byte[] GetKeyIdentifier() => (byte[])this.parameters[16];

    public byte[] GetNonce() => (byte[])this.parameters[20];

    public class Builder
    {
        private IDictionary parameters = Platform.CreateHashtable();

        public Builder()
        {
        }

        public Builder( IDictionary paramsMap )
        {
            foreach (int key in (IEnumerable)paramsMap.Keys)
                this.parameters.Add( key, paramsMap[key] );
        }

        public Builder( SkeinParameters parameters )
        {
            foreach (int key in (IEnumerable)parameters.parameters.Keys)
                this.parameters.Add( key, parameters.parameters[key] );
        }

        public Builder Set( int type, byte[] value )
        {
            if (value == null)
                throw new ArgumentException( "Parameter value must not be null." );
            if (type != 0 && (type <= 4 || type >= 63 || type == 48))
                throw new ArgumentException( "Parameter types must be in the range 0,5..47,49..62." );
            if (type == 4)
                throw new ArgumentException( "Parameter type " + 4 + " is reserved for internal use." );
            this.parameters.Add( type, value );
            return this;
        }

        public Builder SetKey( byte[] key ) => this.Set( 0, key );

        public Builder SetPersonalisation( byte[] personalisation ) => this.Set( 8, personalisation );

        public Builder SetPersonalisation(
          DateTime date,
          string emailAddress,
          string distinguisher )
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter t = new StreamWriter( memoryStream, Encoding.UTF8 );
                t.Write( date.ToString( "YYYYMMDD", CultureInfo.InvariantCulture ) );
                t.Write( " " );
                t.Write( emailAddress );
                t.Write( " " );
                t.Write( distinguisher );
                Platform.Dispose( t );
                return this.Set( 8, memoryStream.ToArray() );
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException( "Byte I/O failed.", ex );
            }
        }

        public Builder SetPublicKey( byte[] publicKey ) => this.Set( 12, publicKey );

        public Builder SetKeyIdentifier( byte[] keyIdentifier ) => this.Set( 16, keyIdentifier );

        public Builder SetNonce( byte[] nonce ) => this.Set( 20, nonce );

        public SkeinParameters Build() => new SkeinParameters( this.parameters );
    }
}
internal class Check
{
    internal static void DataLength( bool condition, string msg )
    {
        if (condition)
            throw new DataLengthException( msg );
    }

    internal static void DataLength( byte[] buf, int off, int len, string msg )
    {
        if (off + len > buf.Length)
            throw new DataLengthException( msg );
    }

    internal static void OutputLength( byte[] buf, int off, int len, string msg )
    {
        if (off + len > buf.Length)
            throw new OutputLengthException( msg );
    }
}
[Serializable]
public class OutputLengthException : DataLengthException
{
    public OutputLengthException()
    {
    }

    public OutputLengthException( string message )
      : base( message )
    {
    }

    public OutputLengthException( string message, Exception exception )
      : base( message, exception )
    {
    }
}
public abstract class Asn1Sequence : Asn1Object, IEnumerable
{
    private readonly IList seq;

    public static Asn1Sequence GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case Asn1Sequence _:
                return (Asn1Sequence)obj;
            case Asn1SequenceParser _:
                return GetInstance( ((IAsn1Convertible)obj).ToAsn1Object() );
            case byte[] _:
                try
                {
                    return GetInstance( FromByteArray( (byte[])obj ) );
                }
                catch (IOException ex)
                {
                    throw new ArgumentException( "failed to construct sequence from byte[]: " + ex.Message );
                }
            case Asn1Encodable _:
                Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
                if (asn1Object is Asn1Sequence)
                    return (Asn1Sequence)asn1Object;
                break;
        }
        throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
    }

    public static Asn1Sequence GetInstance( Asn1TaggedObject obj, bool explicitly )
    {
        Asn1Object instance = obj.GetObject();
        if (explicitly)
        {
            if (!obj.IsExplicit())
                throw new ArgumentException( "object implicit - explicit expected." );
            return (Asn1Sequence)instance;
        }
        if (obj.IsExplicit())
            return obj is BerTaggedObject ? new BerSequence( instance ) : (Asn1Sequence)new DerSequence( instance );
        return instance is Asn1Sequence ? (Asn1Sequence)instance : throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
    }

    protected internal Asn1Sequence( int capacity ) => this.seq = Platform.CreateArrayList( capacity );

    public virtual IEnumerator GetEnumerator() => this.seq.GetEnumerator();

    [Obsolete( "Use GetEnumerator() instead" )]
    public IEnumerator GetObjects() => this.GetEnumerator();

    public virtual Asn1SequenceParser Parser => new Asn1Sequence.Asn1SequenceParserImpl( this );

    public virtual Asn1Encodable this[int index] => (Asn1Encodable)this.seq[index];

    [Obsolete( "Use 'object[index]' syntax instead" )]
    public Asn1Encodable GetObjectAt( int index ) => this[index];

    [Obsolete( "Use 'Count' property instead" )]
    public int Size => this.Count;

    public virtual int Count => this.seq.Count;

    protected override int Asn1GetHashCode()
    {
        int count = this.Count;
        foreach (object obj in this)
        {
            count *= 17;
            if (obj == null)
                count ^= DerNull.Instance.GetHashCode();
            else
                count ^= obj.GetHashCode();
        }
        return count;
    }

    protected override bool Asn1Equals( Asn1Object asn1Object )
    {
        if (!(asn1Object is Asn1Sequence asn1Sequence) || this.Count != asn1Sequence.Count)
            return false;
        IEnumerator enumerator1 = this.GetEnumerator();
        IEnumerator enumerator2 = asn1Sequence.GetEnumerator();
        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            if (!this.GetCurrent( enumerator1 ).ToAsn1Object().Equals( this.GetCurrent( enumerator2 ).ToAsn1Object() ))
                return false;
        }
        return true;
    }

    private Asn1Encodable GetCurrent( IEnumerator e ) => (Asn1Encodable)e.Current ?? DerNull.Instance;

    protected internal void AddObject( Asn1Encodable obj ) => this.seq.Add( obj );

    public override string ToString() => CollectionUtilities.ToString( seq );

    private class Asn1SequenceParserImpl : Asn1SequenceParser, IAsn1Convertible
    {
        private readonly Asn1Sequence outer;
        private readonly int max;
        private int index;

        public Asn1SequenceParserImpl( Asn1Sequence outer )
        {
            this.outer = outer;
            this.max = outer.Count;
        }

        public IAsn1Convertible ReadObject()
        {
            if (this.index == this.max)
                return null;
            Asn1Encodable asn1Encodable = this.outer[this.index++];
            switch (asn1Encodable)
            {
                case Asn1Sequence _:
                    return ((Asn1Sequence)asn1Encodable).Parser;
                case Asn1Set _:
                    return ((Asn1Set)asn1Encodable).Parser;
                default:
                    return asn1Encodable;
            }
        }

        public Asn1Object ToAsn1Object() => outer;
    }
}
public interface Asn1SequenceParser : IAsn1Convertible
{
    IAsn1Convertible ReadObject();
}
public abstract class Asn1Set : Asn1Object, IEnumerable
{
    private readonly IList _set;

    public static Asn1Set GetInstance( object obj )
    {
        switch (obj)
        {
            case null:
            case Asn1Set _:
                return (Asn1Set)obj;
            case Asn1SetParser _:
                return GetInstance( ((IAsn1Convertible)obj).ToAsn1Object() );
            case byte[] _:
                try
                {
                    return GetInstance( FromByteArray( (byte[])obj ) );
                }
                catch (IOException ex)
                {
                    throw new ArgumentException( "failed to construct set from byte[]: " + ex.Message );
                }
            case Asn1Encodable _:
                Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
                if (asn1Object is Asn1Set)
                    return (Asn1Set)asn1Object;
                break;
        }
        throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
    }

    public static Asn1Set GetInstance( Asn1TaggedObject obj, bool explicitly )
    {
        Asn1Object instance = obj.GetObject();
        if (explicitly)
        {
            if (!obj.IsExplicit())
                throw new ArgumentException( "object implicit - explicit expected." );
            return (Asn1Set)instance;
        }
        if (obj.IsExplicit())
            return new DerSet( instance );
        switch (instance)
        {
            case Asn1Set _:
                return (Asn1Set)instance;
            case Asn1Sequence _:
                Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
                foreach (Asn1Encodable asn1Encodable in (Asn1Sequence)instance)
                    v.Add( asn1Encodable );
                return new DerSet( v, false );
            default:
                throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }
    }

    protected internal Asn1Set( int capacity ) => this._set = Platform.CreateArrayList( capacity );

    public virtual IEnumerator GetEnumerator() => this._set.GetEnumerator();

    [Obsolete( "Use GetEnumerator() instead" )]
    public IEnumerator GetObjects() => this.GetEnumerator();

    public virtual Asn1Encodable this[int index] => (Asn1Encodable)this._set[index];

    [Obsolete( "Use 'object[index]' syntax instead" )]
    public Asn1Encodable GetObjectAt( int index ) => this[index];

    [Obsolete( "Use 'Count' property instead" )]
    public int Size => this.Count;

    public virtual int Count => this._set.Count;

    public virtual Asn1Encodable[] ToArray()
    {
        Asn1Encodable[] array = new Asn1Encodable[this.Count];
        for (int index = 0; index < this.Count; ++index)
            array[index] = this[index];
        return array;
    }

    public Asn1SetParser Parser => new Asn1Set.Asn1SetParserImpl( this );

    protected override int Asn1GetHashCode()
    {
        int count = this.Count;
        foreach (object obj in this)
        {
            count *= 17;
            if (obj == null)
                count ^= DerNull.Instance.GetHashCode();
            else
                count ^= obj.GetHashCode();
        }
        return count;
    }

    protected override bool Asn1Equals( Asn1Object asn1Object )
    {
        if (!(asn1Object is Asn1Set asn1Set) || this.Count != asn1Set.Count)
            return false;
        IEnumerator enumerator1 = this.GetEnumerator();
        IEnumerator enumerator2 = asn1Set.GetEnumerator();
        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            if (!this.GetCurrent( enumerator1 ).ToAsn1Object().Equals( this.GetCurrent( enumerator2 ).ToAsn1Object() ))
                return false;
        }
        return true;
    }

    private Asn1Encodable GetCurrent( IEnumerator e ) => (Asn1Encodable)e.Current ?? DerNull.Instance;

    protected internal void Sort()
    {
        if (this._set.Count < 2)
            return;
        Asn1Encodable[] items = new Asn1Encodable[this._set.Count];
        byte[][] keys = new byte[this._set.Count][];
        for (int index = 0; index < this._set.Count; ++index)
        {
            Asn1Encodable asn1Encodable = (Asn1Encodable)this._set[index];
            items[index] = asn1Encodable;
            keys[index] = asn1Encodable.GetEncoded( "DER" );
        }
        Array.Sort( keys, items, new Asn1Set.DerComparer() );
        for (int index = 0; index < this._set.Count; ++index)
            this._set[index] = items[index];
    }

    protected internal void AddObject( Asn1Encodable obj ) => this._set.Add( obj );

    public override string ToString() => CollectionUtilities.ToString( _set );

    private class Asn1SetParserImpl : Asn1SetParser, IAsn1Convertible
    {
        private readonly Asn1Set outer;
        private readonly int max;
        private int index;

        public Asn1SetParserImpl( Asn1Set outer )
        {
            this.outer = outer;
            this.max = outer.Count;
        }

        public IAsn1Convertible ReadObject()
        {
            if (this.index == this.max)
                return null;
            Asn1Encodable asn1Encodable = this.outer[this.index++];
            switch (asn1Encodable)
            {
                case Asn1Sequence _:
                    return ((Asn1Sequence)asn1Encodable).Parser;
                case Asn1Set _:
                    return ((Asn1Set)asn1Encodable).Parser;
                default:
                    return asn1Encodable;
            }
        }

        public virtual Asn1Object ToAsn1Object() => outer;
    }

    private class DerComparer : IComparer
    {
        public int Compare( object x, object y )
        {
            byte[] bs1 = (byte[])x;
            byte[] bs2 = (byte[])y;
            int pos = System.Math.Min( bs1.Length, bs2.Length );
            for (int index = 0; index != pos; ++index)
            {
                byte num1 = bs1[index];
                byte num2 = bs2[index];
                if (num1 != num2)
                    return num1 >= num2 ? 1 : -1;
            }
            return bs1.Length > bs2.Length ? (!this.AllZeroesFrom( bs1, pos ) ? 1 : 0) : (bs1.Length < bs2.Length && !this.AllZeroesFrom( bs2, pos ) ? -1 : 0);
        }

        private bool AllZeroesFrom( byte[] bs, int pos )
        {
            while (pos < bs.Length)
            {
                if (bs[pos++] != 0)
                    return false;
            }
            return true;
        }
    }
}
public interface Asn1SetParser : IAsn1Convertible
{
    IAsn1Convertible ReadObject();
}
public abstract class Asn1Null : Asn1Object
{
    internal Asn1Null()
    {
    }

    public override string ToString() => "NULL";
}
public abstract class Asn1Encodable : IAsn1Convertible
{
    public const string Der = "DER";
    public const string Ber = "BER";

    public byte[] GetEncoded()
    {
        MemoryStream os = new MemoryStream();
        new Asn1OutputStream( os ).WriteObject( this );
        return os.ToArray();
    }

    public byte[] GetEncoded( string encoding )
    {
        if (!encoding.Equals( "DER" ))
            return this.GetEncoded();
        MemoryStream os = new MemoryStream();
        new DerOutputStream( os ).WriteObject( this );
        return os.ToArray();
    }

    public byte[] GetDerEncoded()
    {
        try
        {
            return this.GetEncoded( "DER" );
        }
        catch (IOException ex)
        {
            return null;
        }
    }

    public override sealed int GetHashCode() => this.ToAsn1Object().CallAsn1GetHashCode();

    public override sealed bool Equals( object obj )
    {
        if (obj == this)
            return true;
        if (!(obj is IAsn1Convertible asn1Convertible))
            return false;
        Asn1Object asn1Object1 = this.ToAsn1Object();
        Asn1Object asn1Object2 = asn1Convertible.ToAsn1Object();
        return asn1Object1 == asn1Object2 || asn1Object1.CallAsn1Equals( asn1Object2 );
    }

    public abstract Asn1Object ToAsn1Object();
}
public class Asn1EncodableVector : IEnumerable
{
    private IList v = Platform.CreateArrayList();

    public static Asn1EncodableVector FromEnumerable( IEnumerable e )
    {
        Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector( new Asn1Encodable[0] );
        foreach (Asn1Encodable asn1Encodable in e)
            asn1EncodableVector.Add( asn1Encodable );
        return asn1EncodableVector;
    }

    public Asn1EncodableVector( params Asn1Encodable[] v ) => this.Add( v );

    public void Add( params Asn1Encodable[] objs )
    {
        foreach (object obj in objs)
            this.v.Add( obj );
    }

    public void AddOptional( params Asn1Encodable[] objs )
    {
        if (objs == null)
            return;
        foreach (Asn1Encodable asn1Encodable in objs)
        {
            if (asn1Encodable != null)
                this.v.Add( asn1Encodable );
        }
    }

    public Asn1Encodable this[int index] => (Asn1Encodable)this.v[index];

    [Obsolete( "Use 'object[index]' syntax instead" )]
    public Asn1Encodable Get( int index ) => this[index];

    [Obsolete( "Use 'Count' property instead" )]
    public int Size => this.v.Count;

    public int Count => this.v.Count;

    public IEnumerator GetEnumerator() => this.v.GetEnumerator();
}
public abstract class CollectionUtilities
{
    public static void AddRange( IList to, IEnumerable range )
    {
        foreach (object obj in range)
            to.Add( obj );
    }

    public static bool CheckElementsAreOfType( IEnumerable e, Type t )
    {
        foreach (object o in e)
        {
            if (!t.IsInstanceOfType( o ))
                return false;
        }
        return true;
    }

    public static IDictionary ReadOnly( IDictionary d ) => new UnmodifiableDictionaryProxy( d );

    public static IList ReadOnly( IList l ) => new UnmodifiableListProxy( l );

    public static ISet ReadOnly( ISet s ) => new UnmodifiableSetProxy( s );

    public static string ToString( IEnumerable c )
    {
        StringBuilder stringBuilder = new StringBuilder( "[" );
        IEnumerator enumerator = c.GetEnumerator();
        if (enumerator.MoveNext())
        {
            stringBuilder.Append( enumerator.Current.ToString() );
            while (enumerator.MoveNext())
            {
                stringBuilder.Append( ", " );
                stringBuilder.Append( enumerator.Current.ToString() );
            }
        }
        stringBuilder.Append( ']' );
        return stringBuilder.ToString();
    }
}
public class UnmodifiableDictionaryProxy : UnmodifiableDictionary
{
    private readonly IDictionary d;

    public UnmodifiableDictionaryProxy( IDictionary d ) => this.d = d;

    public override bool Contains( object k ) => this.d.Contains( k );

    public override void CopyTo( Array array, int index ) => this.d.CopyTo( array, index );

    public override int Count => this.d.Count;

    public override IDictionaryEnumerator GetEnumerator() => this.d.GetEnumerator();

    public override bool IsFixedSize => this.d.IsFixedSize;

    public override bool IsSynchronized => this.d.IsSynchronized;

    public override object SyncRoot => this.d.SyncRoot;

    public override ICollection Keys => this.d.Keys;

    public override ICollection Values => this.d.Values;

    protected override object GetValue( object k ) => this.d[k];
}
public abstract class UnmodifiableDictionary : IDictionary, ICollection, IEnumerable
{
    public virtual void Add( object k, object v ) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public abstract bool Contains( object k );

    public abstract void CopyTo( Array array, int index );

    public abstract int Count { get; }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public abstract IDictionaryEnumerator GetEnumerator();

    public virtual void Remove( object k ) => throw new NotSupportedException();

    public abstract bool IsFixedSize { get; }

    public virtual bool IsReadOnly => true;

    public abstract bool IsSynchronized { get; }

    public abstract object SyncRoot { get; }

    public abstract ICollection Keys { get; }

    public abstract ICollection Values { get; }

    public virtual object this[object k]
    {
        get => this.GetValue( k );
        set => throw new NotSupportedException();
    }

    protected abstract object GetValue( object k );
}
public class UnmodifiableListProxy : UnmodifiableList
{
    private readonly IList l;

    public UnmodifiableListProxy( IList l ) => this.l = l;

    public override bool Contains( object o ) => this.l.Contains( o );

    public override void CopyTo( Array array, int index ) => this.l.CopyTo( array, index );

    public override int Count => this.l.Count;

    public override IEnumerator GetEnumerator() => this.l.GetEnumerator();

    public override int IndexOf( object o ) => this.l.IndexOf( o );

    public override bool IsFixedSize => this.l.IsFixedSize;

    public override bool IsSynchronized => this.l.IsSynchronized;

    public override object SyncRoot => this.l.SyncRoot;

    protected override object GetValue( int i ) => this.l[i];
}
public abstract class UnmodifiableList : IList, ICollection, IEnumerable
{
    public virtual int Add( object o ) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public abstract bool Contains( object o );

    public abstract void CopyTo( Array array, int index );

    public abstract int Count { get; }

    public abstract IEnumerator GetEnumerator();

    public abstract int IndexOf( object o );

    public virtual void Insert( int i, object o ) => throw new NotSupportedException();

    public abstract bool IsFixedSize { get; }

    public virtual bool IsReadOnly => true;

    public abstract bool IsSynchronized { get; }

    public virtual void Remove( object o ) => throw new NotSupportedException();

    public virtual void RemoveAt( int i ) => throw new NotSupportedException();

    public abstract object SyncRoot { get; }

    public virtual object this[int i]
    {
        get => this.GetValue( i );
        set => throw new NotSupportedException();
    }

    protected abstract object GetValue( int i );
}
public class UnmodifiableSetProxy : UnmodifiableSet
{
    private readonly ISet s;

    public UnmodifiableSetProxy( ISet s ) => this.s = s;

    public override bool Contains( object o ) => this.s.Contains( o );

    public override void CopyTo( Array array, int index ) => this.s.CopyTo( array, index );

    public override int Count => this.s.Count;

    public override IEnumerator GetEnumerator() => this.s.GetEnumerator();

    public override bool IsEmpty => this.s.IsEmpty;

    public override bool IsFixedSize => this.s.IsFixedSize;

    public override bool IsSynchronized => this.s.IsSynchronized;

    public override object SyncRoot => this.s.SyncRoot;
}
public abstract class UnmodifiableSet : ISet, ICollection, IEnumerable
{
    public virtual void Add( object o ) => throw new NotSupportedException();

    public virtual void AddAll( IEnumerable e ) => throw new NotSupportedException();

    public virtual void Clear() => throw new NotSupportedException();

    public abstract bool Contains( object o );

    public abstract void CopyTo( Array array, int index );

    public abstract int Count { get; }

    public abstract IEnumerator GetEnumerator();

    public abstract bool IsEmpty { get; }

    public abstract bool IsFixedSize { get; }

    public virtual bool IsReadOnly => true;

    public abstract bool IsSynchronized { get; }

    public abstract object SyncRoot { get; }

    public virtual void Remove( object o ) => throw new NotSupportedException();

    public virtual void RemoveAll( IEnumerable e ) => throw new NotSupportedException();
}