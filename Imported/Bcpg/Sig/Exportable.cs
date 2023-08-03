// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.Exportable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class Exportable : SignatureSubpacket
    {
        private static byte[] BooleanToByteArray( bool val )
        {
            byte[] byteArray = new byte[1];
            if (!val)
                return byteArray;
            byteArray[0] = 1;
            return byteArray;
        }

        public Exportable( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.Exportable, critical, isLongLength, data )
        {
        }

        public Exportable( bool critical, bool isExportable )
          : base( SignatureSubpacketTag.Exportable, critical, false, BooleanToByteArray( isExportable ) )
        {
        }

        public bool IsExportable() => this.data[0] != 0;
    }
}
