// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpCompressedDataGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Apache.Bzip2;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpCompressedDataGenerator : IStreamGenerator
    {
        private readonly CompressionAlgorithmTag algorithm;
        private readonly int compression;
        private Stream dOut;
        private BcpgOutputStream pkOut;

        public PgpCompressedDataGenerator( CompressionAlgorithmTag algorithm )
          : this( algorithm, -1 )
        {
        }

        public PgpCompressedDataGenerator( CompressionAlgorithmTag algorithm, int compression )
        {
            switch (algorithm)
            {
                case CompressionAlgorithmTag.Uncompressed:
                case CompressionAlgorithmTag.Zip:
                case CompressionAlgorithmTag.ZLib:
                case CompressionAlgorithmTag.BZip2:
                    if (compression != -1 && (compression < 0 || compression > 9))
                        throw new ArgumentException( "unknown compression level: " + compression );
                    this.algorithm = algorithm;
                    this.compression = compression;
                    break;
                default:
                    throw new ArgumentException( "unknown compression algorithm", nameof( algorithm ) );
            }
        }

        public Stream Open( Stream outStr )
        {
            if (this.dOut != null)
                throw new InvalidOperationException( "generator already in open state" );
            this.pkOut = outStr != null ? new BcpgOutputStream( outStr, PacketTag.CompressedData ) : throw new ArgumentNullException( nameof( outStr ) );
            this.doOpen();
            return new WrappedGeneratorStream( this, this.dOut );
        }

        public Stream Open( Stream outStr, byte[] buffer )
        {
            if (this.dOut != null)
                throw new InvalidOperationException( "generator already in open state" );
            if (outStr == null)
                throw new ArgumentNullException( nameof( outStr ) );
            this.pkOut = buffer != null ? new BcpgOutputStream( outStr, PacketTag.CompressedData, buffer ) : throw new ArgumentNullException( nameof( buffer ) );
            this.doOpen();
            return new WrappedGeneratorStream( this, this.dOut );
        }

        private void doOpen()
        {
            this.pkOut.WriteByte( (byte)this.algorithm );
            switch (this.algorithm)
            {
                case CompressionAlgorithmTag.Uncompressed:
                    this.dOut = pkOut;
                    break;
                case CompressionAlgorithmTag.Zip:
                    this.dOut = new PgpCompressedDataGenerator.SafeZOutputStream( pkOut, this.compression, true );
                    break;
                case CompressionAlgorithmTag.ZLib:
                    this.dOut = new PgpCompressedDataGenerator.SafeZOutputStream( pkOut, this.compression, false );
                    break;
                case CompressionAlgorithmTag.BZip2:
                    this.dOut = new PgpCompressedDataGenerator.SafeCBZip2OutputStream( pkOut );
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void Close()
        {
            if (this.dOut == null)
                return;
            if (this.dOut != this.pkOut)
                Platform.Dispose( this.dOut );
            this.dOut = null;
            this.pkOut.Finish();
            this.pkOut.Flush();
            this.pkOut = null;
        }

        private class SafeCBZip2OutputStream : CBZip2OutputStream
        {
            public SafeCBZip2OutputStream( Stream output )
              : base( output )
            {
            }

            public override void Close() => this.Finish();
        }

        private class SafeZOutputStream : ZOutputStream
        {
            public SafeZOutputStream( Stream output, int level, bool nowrap )
              : base( output, level, nowrap )
            {
            }

            public override void Close()
            {
                this.Finish();
                this.End();
            }
        }
    }
}
