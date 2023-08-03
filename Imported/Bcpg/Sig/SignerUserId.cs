// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.SignerUserId
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class SignerUserId : SignatureSubpacket
    {
        private static byte[] UserIdToBytes( string id )
        {
            byte[] bytes = new byte[id.Length];
            for (int index = 0; index != id.Length; ++index)
                bytes[index] = (byte)id[index];
            return bytes;
        }

        public SignerUserId( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.SignerUserId, critical, isLongLength, data )
        {
        }

        public SignerUserId( bool critical, string userId )
          : base( SignatureSubpacketTag.SignerUserId, critical, false, UserIdToBytes( userId ) )
        {
        }

        public string GetId()
        {
            char[] chArray = new char[this.data.Length];
            for (int index = 0; index != chArray.Length; ++index)
                chArray[index] = (char)(this.data[index] & (uint)byte.MaxValue);
            return new string( chArray );
        }
    }
}
