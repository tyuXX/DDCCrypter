// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Crc24
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public class Crc24
    {
        private const int Crc24Init = 11994318;
        private const int Crc24Poly = 25578747;
        private int crc = 11994318;

        public void Update( int b )
        {
            this.crc ^= b << 16;
            for (int index = 0; index < 8; ++index)
            {
                this.crc <<= 1;
                if ((this.crc & 16777216) != 0)
                    this.crc ^= 25578747;
            }
        }

        [Obsolete( "Use 'Value' property instead" )]
        public int GetValue() => this.crc;

        public int Value => this.crc;

        public void Reset() => this.crc = 11994318;
    }
}
