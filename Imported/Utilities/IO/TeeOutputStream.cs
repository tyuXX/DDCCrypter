// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.TeeOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
    public class TeeOutputStream : BaseOutputStream
    {
        private readonly Stream output;
        private readonly Stream tee;

        public TeeOutputStream( Stream output, Stream tee )
        {
            this.output = output;
            this.tee = tee;
        }

        public override void Close()
        {
            Platform.Dispose( this.output );
            Platform.Dispose( this.tee );
            base.Close();
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            this.output.Write( buffer, offset, count );
            this.tee.Write( buffer, offset, count );
        }

        public override void WriteByte( byte b )
        {
            this.output.WriteByte( b );
            this.tee.WriteByte( b );
        }
    }
}
