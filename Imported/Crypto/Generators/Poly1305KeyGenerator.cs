// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Generators.Poly1305KeyGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Generators
{
    public class Poly1305KeyGenerator : CipherKeyGenerator
    {
        private const byte R_MASK_LOW_2 = 252;
        private const byte R_MASK_HIGH_4 = 15;

        protected override void engineInit( KeyGenerationParameters param )
        {
            this.random = param.Random;
            this.strength = 32;
        }

        protected override byte[] engineGenerateKey()
        {
            byte[] key = base.engineGenerateKey();
            Clamp( key );
            return key;
        }

        public static void Clamp( byte[] key )
        {
            if (key.Length != 32)
                throw new ArgumentException( "Poly1305 key must be 256 bits." );
            byte[] numArray1;
            (numArray1 = key)[19] = (byte)(numArray1[19] & 15U);
            byte[] numArray2;
            (numArray2 = key)[23] = (byte)(numArray2[23] & 15U);
            byte[] numArray3;
            (numArray3 = key)[27] = (byte)(numArray3[27] & 15U);
            byte[] numArray4;
            (numArray4 = key)[31] = (byte)(numArray4[31] & 15U);
            byte[] numArray5;
            (numArray5 = key)[20] = (byte)(numArray5[20] & 252U);
            byte[] numArray6;
            (numArray6 = key)[24] = (byte)(numArray6[24] & 252U);
            byte[] numArray7;
            (numArray7 = key)[28] = (byte)(numArray7[28] & 252U);
        }

        public static void CheckKey( byte[] key )
        {
            if (key.Length != 32)
                throw new ArgumentException( "Poly1305 key must be 256 bits." );
            checkMask( key[19], 15 );
            checkMask( key[23], 15 );
            checkMask( key[27], 15 );
            checkMask( key[31], 15 );
            checkMask( key[20], 252 );
            checkMask( key[24], 252 );
            checkMask( key[28], 252 );
        }

        private static void checkMask( byte b, byte mask )
        {
            if ((b & ~mask) != 0)
                throw new ArgumentException( "Invalid format for r portion of Poly1305 key." );
        }
    }
}
