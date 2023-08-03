// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.PublicKeyAlgorithmTag
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public enum PublicKeyAlgorithmTag
    {
        RsaGeneral = 1,
        RsaEncrypt = 2,
        RsaSign = 3,
        ElGamalEncrypt = 16, // 0x00000010
        Dsa = 17, // 0x00000011
        [Obsolete( "Use 'ECDH' instead" )] EC = 18, // 0x00000012
        ECDH = 18, // 0x00000012
        ECDsa = 19, // 0x00000013
        ElGamalGeneral = 20, // 0x00000014
        DiffieHellman = 21, // 0x00000015
        Experimental_1 = 100, // 0x00000064
        Experimental_2 = 101, // 0x00000065
        Experimental_3 = 102, // 0x00000066
        Experimental_4 = 103, // 0x00000067
        Experimental_5 = 104, // 0x00000068
        Experimental_6 = 105, // 0x00000069
        Experimental_7 = 106, // 0x0000006A
        Experimental_8 = 107, // 0x0000006B
        Experimental_9 = 108, // 0x0000006C
        Experimental_10 = 109, // 0x0000006D
        Experimental_11 = 110, // 0x0000006E
    }
}
