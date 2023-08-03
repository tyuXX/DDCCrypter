// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.SCrypt
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class SCrypt
    {
        public static byte[] Generate( byte[] P, byte[] S, int N, int r, int p, int dkLen ) => MFcrypt( P, S, N, r, p, dkLen );

        private static byte[] MFcrypt( byte[] P, byte[] S, int N, int r, int p, int dkLen )
        {
            int num1 = r * 128;
            byte[] numArray1 = SingleIterationPBKDF2( P, S, p * num1 );
            uint[] numArray2 = null;
            try
            {
                int length = numArray1.Length >> 2;
                numArray2 = new uint[length];
                Pack.LE_To_UInt32( numArray1, 0, numArray2 );
                int num2 = num1 >> 2;
                for (int BOff = 0; BOff < length; BOff += num2)
                    SMix( numArray2, BOff, N, r );
                Pack.UInt32_To_LE( numArray2, numArray1, 0 );
                return SingleIterationPBKDF2( P, numArray1, dkLen );
            }
            finally
            {
                ClearAll( numArray1, numArray2 );
            }
        }

        private static byte[] SingleIterationPBKDF2( byte[] P, byte[] S, int dkLen )
        {
            PbeParametersGenerator parametersGenerator = new Pkcs5S2ParametersGenerator( new Sha256Digest() );
            parametersGenerator.Init( P, S, 1 );
            return ((KeyParameter)parametersGenerator.GenerateDerivedMacParameters( dkLen * 8 )).GetKey();
        }

        private static void SMix( uint[] B, int BOff, int N, int r )
        {
            int length = r * 32;
            uint[] X1 = new uint[16];
            uint[] X2 = new uint[16];
            uint[] Y = new uint[length];
            uint[] numArray1 = new uint[length];
            uint[][] numArray2 = new uint[N][];
            try
            {
                Array.Copy( B, BOff, numArray1, 0, length );
                for (int index = 0; index < N; ++index)
                {
                    numArray2[index] = (uint[])numArray1.Clone();
                    BlockMix( numArray1, X1, X2, Y, r );
                }
                uint num = (uint)(N - 1);
                for (int index1 = 0; index1 < N; ++index1)
                {
                    uint index2 = numArray1[length - 16] & num;
                    Xor( numArray1, numArray2[(int)(IntPtr)index2], 0, numArray1 );
                    BlockMix( numArray1, X1, X2, Y, r );
                }
                Array.Copy( numArray1, 0, B, BOff, length );
            }
            finally
            {
                ClearAll( numArray2 );
                ClearAll( numArray1, X1, X2, Y );
            }
        }

        private static void BlockMix( uint[] B, uint[] X1, uint[] X2, uint[] Y, int r )
        {
            Array.Copy( B, B.Length - 16, X1, 0, 16 );
            int bOff = 0;
            int destinationIndex = 0;
            int num = B.Length >> 1;
            for (int index = 2 * r; index > 0; --index)
            {
                Xor( X1, B, bOff, X2 );
                Salsa20Engine.SalsaCore( 8, X2, X1 );
                Array.Copy( X1, 0, Y, destinationIndex, 16 );
                destinationIndex = num + bOff - destinationIndex;
                bOff += 16;
            }
            Array.Copy( Y, 0, B, 0, Y.Length );
        }

        private static void Xor( uint[] a, uint[] b, int bOff, uint[] output )
        {
            for (int index = output.Length - 1; index >= 0; --index)
                output[index] = a[index] ^ b[bOff + index];
        }

        private static void Clear( Array array )
        {
            if (array == null)
                return;
            Array.Clear( array, 0, array.Length );
        }

        private static void ClearAll( params Array[] arrays )
        {
            foreach (Array array in arrays)
                Clear( array );
        }
    }
}
