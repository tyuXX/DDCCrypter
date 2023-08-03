// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.Gcm.Tables64kGcmMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    public class Tables64kGcmMultiplier : IGcmMultiplier
    {
        private byte[] H;
        private uint[][][] M;

        public void Init( byte[] H )
        {
            if (this.M == null)
                this.M = new uint[16][][];
            else if (Arrays.AreEqual( this.H, H ))
                return;
            this.H = Arrays.Clone( H );
            this.M[0] = new uint[256][];
            this.M[0][0] = new uint[4];
            this.M[0][128] = GcmUtilities.AsUints( H );
            for (int index = 64; index >= 1; index >>= 1)
            {
                uint[] x = (uint[])this.M[0][index + index].Clone();
                GcmUtilities.MultiplyP( x );
                this.M[0][index] = x;
            }
            int index1 = 0;
        label_8:
            for (int index2 = 2; index2 < 256; index2 += index2)
            {
                for (int index3 = 1; index3 < index2; ++index3)
                {
                    uint[] x = (uint[])this.M[index1][index2].Clone();
                    GcmUtilities.Xor( x, this.M[index1][index3] );
                    this.M[index1][index2 + index3] = x;
                }
            }
            if (++index1 == 16)
                return;
            this.M[index1] = new uint[256][];
            this.M[index1][0] = new uint[4];
            for (int index4 = 128; index4 > 0; index4 >>= 1)
            {
                uint[] x = (uint[])this.M[index1 - 1][index4].Clone();
                GcmUtilities.MultiplyP8( x );
                this.M[index1][index4] = x;
            }
            goto label_8;
        }

        public void MultiplyH( byte[] x )
        {
            uint[] ns = new uint[4];
            for (int index = 0; index != 16; ++index)
            {
                uint[] numArray1 = this.M[index][x[index]];
                uint[] numArray2;
                (numArray2 = ns)[0] = numArray2[0] ^ numArray1[0];
                uint[] numArray3;
                (numArray3 = ns)[1] = numArray3[1] ^ numArray1[1];
                uint[] numArray4;
                (numArray4 = ns)[2] = numArray4[2] ^ numArray1[2];
                uint[] numArray5;
                (numArray5 = ns)[3] = numArray5[3] ^ numArray1[3];
            }
            Pack.UInt32_To_BE( ns, x, 0 );
        }
    }
}
