﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.MaxFragmentLength
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class MaxFragmentLength
    {
        public const byte pow2_9 = 1;
        public const byte pow2_10 = 2;
        public const byte pow2_11 = 3;
        public const byte pow2_12 = 4;

        public static bool IsValid( byte maxFragmentLength ) => maxFragmentLength >= 1 && maxFragmentLength <= 4;
    }
}
