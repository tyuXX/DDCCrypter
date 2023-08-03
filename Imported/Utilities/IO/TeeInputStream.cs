// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.TeeInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
    public class TeeInputStream : BaseInputStream
    {
        private readonly Stream input;
        private readonly Stream tee;

        public TeeInputStream( Stream input, Stream tee )
        {
            this.input = input;
            this.tee = tee;
        }

        public override void Close()
        {
            Platform.Dispose( this.input );
            Platform.Dispose( this.tee );
            base.Close();
        }

        public override int Read( byte[] buf, int off, int len )
        {
            int count = this.input.Read( buf, off, len );
            if (count > 0)
                this.tee.Write( buf, off, count );
            return count;
        }

        public override int ReadByte()
        {
            int num = this.input.ReadByte();
            if (num >= 0)
                this.tee.WriteByte( (byte)num );
            return num;
        }
    }
}
