// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.PreferredAlgorithms
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class PreferredAlgorithms : SignatureSubpacket
    {
        private static byte[] IntToByteArray( int[] v )
        {
            byte[] byteArray = new byte[v.Length];
            for (int index = 0; index != v.Length; ++index)
                byteArray[index] = (byte)v[index];
            return byteArray;
        }

        public PreferredAlgorithms(
          SignatureSubpacketTag type,
          bool critical,
          bool isLongLength,
          byte[] data )
          : base( type, critical, isLongLength, data )
        {
        }

        public PreferredAlgorithms( SignatureSubpacketTag type, bool critical, int[] preferences )
          : base( type, critical, false, IntToByteArray( preferences ) )
        {
        }

        public int[] GetPreferences()
        {
            int[] preferences = new int[this.data.Length];
            for (int index = 0; index != preferences.Length; ++index)
                preferences[index] = this.data[index] & byte.MaxValue;
            return preferences;
        }
    }
}
