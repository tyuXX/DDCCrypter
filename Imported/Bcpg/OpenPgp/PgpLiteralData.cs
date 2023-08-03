// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpLiteralData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Date;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpLiteralData : PgpObject
    {
        public const char Binary = 'b';
        public const char Text = 't';
        public const char Utf8 = 'u';
        public const string Console = "_CONSOLE";
        private LiteralDataPacket data;

        public PgpLiteralData( BcpgInputStream bcpgInput ) => this.data = (LiteralDataPacket)bcpgInput.ReadPacket();

        public int Format => this.data.Format;

        public string FileName => this.data.FileName;

        public byte[] GetRawFileName() => this.data.GetRawFileName();

        public DateTime ModificationTime => DateTimeUtilities.UnixMsToDateTime( this.data.ModificationTime );

        public Stream GetInputStream() => this.data.GetInputStream();

        public Stream GetDataStream() => this.GetInputStream();
    }
}
