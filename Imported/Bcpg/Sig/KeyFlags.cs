// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.KeyFlags
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class KeyFlags : SignatureSubpacket
    {
        public const int CertifyOther = 1;
        public const int SignData = 2;
        public const int EncryptComms = 4;
        public const int EncryptStorage = 8;
        public const int Split = 16;
        public const int Authentication = 32;
        public const int Shared = 128;

        private static byte[] IntToByteArray( int v )
        {
            byte[] sourceArray = new byte[4];
            int num = 0;
            for (int index = 0; index != 4; ++index)
            {
                sourceArray[index] = (byte)(v >> (index * 8));
                if (sourceArray[index] != 0)
                    num = index;
            }
            byte[] destinationArray = new byte[num + 1];
            Array.Copy( sourceArray, 0, destinationArray, 0, destinationArray.Length );
            return destinationArray;
        }

        public KeyFlags( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.KeyFlags, critical, isLongLength, data )
        {
        }

        public KeyFlags( bool critical, int flags )
          : base( SignatureSubpacketTag.KeyFlags, critical, false, IntToByteArray( flags ) )
        {
        }

        public int Flags
        {
            get
            {
                int flags = 0;
                for (int index = 0; index != this.data.Length; ++index)
                    flags |= (this.data[index] & byte.MaxValue) << (index * 8);
                return flags;
            }
        }
    }
}
