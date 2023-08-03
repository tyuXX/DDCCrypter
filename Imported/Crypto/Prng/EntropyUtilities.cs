// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.EntropyUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Prng
{
    public abstract class EntropyUtilities
    {
        public static byte[] GenerateSeed( IEntropySource entropySource, int numBytes )
        {
            byte[] destinationArray = new byte[numBytes];
            int length;
            for (int destinationIndex = 0; destinationIndex < numBytes; destinationIndex += length)
            {
                byte[] entropy = entropySource.GetEntropy();
                length = System.Math.Min( destinationArray.Length, numBytes - destinationIndex );
                Array.Copy( entropy, 0, destinationArray, destinationIndex, length );
            }
            return destinationArray;
        }
    }
}
