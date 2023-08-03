// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpEncryptedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public abstract class PgpEncryptedData
    {
        internal InputStreamPacket encData;
        internal Stream encStream;
        internal PgpEncryptedData.TruncatedStream truncStream;

        internal PgpEncryptedData( InputStreamPacket encData ) => this.encData = encData;

        public virtual Stream GetInputStream() => this.encData.GetInputStream();

        public bool IsIntegrityProtected() => this.encData is SymmetricEncIntegrityPacket;

        public bool Verify()
        {
            if (!this.IsIntegrityProtected())
                throw new PgpException( "data not integrity protected." );
            DigestStream encStream = (DigestStream)this.encStream;
            do
                ;
            while (this.encStream.ReadByte() >= 0);
            byte[] lookAhead = this.truncStream.GetLookAhead();
            IDigest digest = encStream.ReadDigest();
            digest.BlockUpdate( lookAhead, 0, 2 );
            byte[] a = DigestUtilities.DoFinal( digest );
            byte[] numArray = new byte[a.Length];
            Array.Copy( lookAhead, 2, numArray, 0, numArray.Length );
            return Arrays.ConstantTimeAreEqual( a, numArray );
        }

        internal class TruncatedStream : BaseInputStream
        {
            private const int LookAheadSize = 22;
            private const int LookAheadBufSize = 512;
            private const int LookAheadBufLimit = 490;
            private readonly Stream inStr;
            private readonly byte[] lookAhead = new byte[512];
            private int bufStart;
            private int bufEnd;

            internal TruncatedStream( Stream inStr )
            {
                int num = Streams.ReadFully( inStr, this.lookAhead, 0, this.lookAhead.Length );
                if (num < 22)
                    throw new EndOfStreamException();
                this.inStr = inStr;
                this.bufStart = 0;
                this.bufEnd = num - 22;
            }

            private int FillBuffer()
            {
                if (this.bufEnd < 490)
                    return 0;
                Array.Copy( lookAhead, 490, lookAhead, 0, 22 );
                this.bufEnd = Streams.ReadFully( this.inStr, this.lookAhead, 22, 490 );
                this.bufStart = 0;
                return this.bufEnd;
            }

            public override int ReadByte() => this.bufStart < this.bufEnd || this.FillBuffer() >= 1 ? this.lookAhead[this.bufStart++] : -1;

            public override int Read( byte[] buf, int off, int len )
            {
                int length = this.bufEnd - this.bufStart;
                int destinationIndex = off;
                while (len > length)
                {
                    Array.Copy( lookAhead, this.bufStart, buf, destinationIndex, length );
                    this.bufStart += length;
                    destinationIndex += length;
                    len -= length;
                    if ((length = this.FillBuffer()) < 1)
                        return destinationIndex - off;
                }
                Array.Copy( lookAhead, this.bufStart, buf, destinationIndex, len );
                this.bufStart += len;
                return destinationIndex + len - off;
            }

            internal byte[] GetLookAhead()
            {
                byte[] destinationArray = new byte[22];
                Array.Copy( lookAhead, this.bufStart, destinationArray, 0, 22 );
                return destinationArray;
            }
        }
    }
}
