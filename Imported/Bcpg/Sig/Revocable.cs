// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.Revocable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class Revocable : SignatureSubpacket
    {
        private static byte[] BooleanToByteArray( bool value )
        {
            byte[] byteArray = new byte[1];
            if (!value)
                return byteArray;
            byteArray[0] = 1;
            return byteArray;
        }

        public Revocable( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.Revocable, critical, isLongLength, data )
        {
        }

        public Revocable( bool critical, bool isRevocable )
          : base( SignatureSubpacketTag.Revocable, critical, false, BooleanToByteArray( isRevocable ) )
        {
        }

        public bool IsRevocable() => this.data[0] != 0;
    }
}
