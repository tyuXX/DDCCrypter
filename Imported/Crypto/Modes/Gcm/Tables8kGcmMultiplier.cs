// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.Gcm.Tables8kGcmMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    public class Tables8kGcmMultiplier : IGcmMultiplier
    {
        private byte[] H;
        private uint[][][] M;

        public void Init( byte[] H )
        {
            if (this.M == null)
                this.M = new uint[32][][];
            else if (Arrays.AreEqual( this.H, H ))
                return;
            this.H = Arrays.Clone( H );
            this.M[0] = new uint[16][];
            this.M[1] = new uint[16][];
            this.M[0][0] = new uint[4];
            this.M[1][0] = new uint[4];
            this.M[1][8] = GcmUtilities.AsUints( H );
            for (int index = 4; index >= 1; index >>= 1)
            {
                uint[] x = (uint[])this.M[1][index + index].Clone();
                GcmUtilities.MultiplyP( x );
                this.M[1][index] = x;
            }
            uint[] x1 = (uint[])this.M[1][1].Clone();
            GcmUtilities.MultiplyP( x1 );
            this.M[0][8] = x1;
            for (int index = 4; index >= 1; index >>= 1)
            {
                uint[] x2 = (uint[])this.M[0][index + index].Clone();
                GcmUtilities.MultiplyP( x2 );
                this.M[0][index] = x2;
            }
            int index1 = 0;
        label_11:
            do
            {
                for (int index2 = 2; index2 < 16; index2 += index2)
                {
                    for (int index3 = 1; index3 < index2; ++index3)
                    {
                        uint[] x3 = (uint[])this.M[index1][index2].Clone();
                        GcmUtilities.Xor( x3, this.M[index1][index3] );
                        this.M[index1][index2 + index3] = x3;
                    }
                }
                if (++index1 == 32)
                    return;
            }
            while (index1 <= 1);
            this.M[index1] = new uint[16][];
            this.M[index1][0] = new uint[4];
            for (int index4 = 8; index4 > 0; index4 >>= 1)
            {
                uint[] x4 = (uint[])this.M[index1 - 2][index4].Clone();
                GcmUtilities.MultiplyP8( x4 );
                this.M[index1][index4] = x4;
            }
            goto label_11;
        }

        public void MultiplyH( byte[] x )
        {
            uint[] ns = new uint[4];
            for (int index = 15; index >= 0; --index)
            {
                uint[] numArray1 = this.M[index + index][x[index] & 15];
                uint[] numArray2;
                (numArray2 = ns)[0] = numArray2[0] ^ numArray1[0];
                uint[] numArray3;
                (numArray3 = ns)[1] = numArray3[1] ^ numArray1[1];
                uint[] numArray4;
                (numArray4 = ns)[2] = numArray4[2] ^ numArray1[2];
                uint[] numArray5;
                (numArray5 = ns)[3] = numArray5[3] ^ numArray1[3];
                uint[] numArray6 = this.M[index + index + 1][(x[index] & 240) >> 4];
                uint[] numArray7;
                (numArray7 = ns)[0] = numArray7[0] ^ numArray6[0];
                uint[] numArray8;
                (numArray8 = ns)[1] = numArray8[1] ^ numArray6[1];
                uint[] numArray9;
                (numArray9 = ns)[2] = numArray9[2] ^ numArray6[2];
                uint[] numArray10;
                (numArray10 = ns)[3] = numArray10[3] ^ numArray6[3];
            }
            Pack.UInt32_To_BE( ns, x, 0 );
        }
    }
}
