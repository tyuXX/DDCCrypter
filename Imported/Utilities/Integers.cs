// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Integers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities
{
    public abstract class Integers
    {
        public static int RotateLeft( int i, int distance ) => (i << distance) ^ i >>> -distance;

        public static int RotateRight( int i, int distance ) => i >>> distance ^ (i << -distance);
    }
}
