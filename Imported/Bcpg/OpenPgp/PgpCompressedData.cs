// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpCompressedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Apache.Bzip2;
using Org.BouncyCastle.Utilities.Zlib;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpCompressedData : PgpObject
    {
        private readonly CompressedDataPacket data;

        public PgpCompressedData( BcpgInputStream bcpgInput ) => this.data = (CompressedDataPacket)bcpgInput.ReadPacket();

        public CompressionAlgorithmTag Algorithm => this.data.Algorithm;

        public Stream GetInputStream() => this.data.GetInputStream();

        public Stream GetDataStream()
        {
            switch (this.Algorithm)
            {
                case CompressionAlgorithmTag.Uncompressed:
                    return this.GetInputStream();
                case CompressionAlgorithmTag.Zip:
                    return new ZInputStream( this.GetInputStream(), true );
                case CompressionAlgorithmTag.ZLib:
                    return new ZInputStream( this.GetInputStream() );
                case CompressionAlgorithmTag.BZip2:
                    return new CBZip2InputStream( this.GetInputStream() );
                default:
                    throw new PgpException( "can't recognise compression algorithm: " + Algorithm );
            }
        }
    }
}
