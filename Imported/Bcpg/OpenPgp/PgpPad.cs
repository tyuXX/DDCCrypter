// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPad
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public sealed class PgpPad
    {
        private PgpPad()
        {
        }

        public static byte[] PadSessionData( byte[] sessionInfo )
        {
            byte[] destinationArray = new byte[40];
            Array.Copy( sessionInfo, 0, destinationArray, 0, sessionInfo.Length );
            byte num = (byte)(destinationArray.Length - sessionInfo.Length);
            for (int length = sessionInfo.Length; length != destinationArray.Length; ++length)
                destinationArray[length] = num;
            return destinationArray;
        }

        public static byte[] UnpadSessionData( byte[] encoded )
        {
            byte num = encoded[encoded.Length - 1];
            for (int index = encoded.Length - num; index != encoded.Length; ++index)
            {
                if (encoded[index] != num)
                    throw new PgpException( "bad padding found in session data" );
            }
            byte[] destinationArray = new byte[encoded.Length - num];
            Array.Copy( encoded, 0, destinationArray, 0, destinationArray.Length );
            return destinationArray;
        }
    }
}
