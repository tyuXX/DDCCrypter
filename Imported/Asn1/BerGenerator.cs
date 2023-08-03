// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerGenerator : Asn1Generator
    {
        private bool _tagged = false;
        private bool _isExplicit;
        private int _tagNo;

        protected BerGenerator( Stream outStream )
          : base( outStream )
        {
        }

        public BerGenerator( Stream outStream, int tagNo, bool isExplicit )
          : base( outStream )
        {
            this._tagged = true;
            this._isExplicit = isExplicit;
            this._tagNo = tagNo;
        }

        public override void AddObject( Asn1Encodable obj ) => new BerOutputStream( this.Out ).WriteObject( obj );

        public override Stream GetRawOutputStream() => this.Out;

        public override void Close() => this.WriteBerEnd();

        private void WriteHdr( int tag )
        {
            this.Out.WriteByte( (byte)tag );
            this.Out.WriteByte( 128 );
        }

        protected void WriteBerHeader( int tag )
        {
            if (this._tagged)
            {
                int tag1 = this._tagNo | 128;
                if (this._isExplicit)
                {
                    this.WriteHdr( tag1 | 32 );
                    this.WriteHdr( tag );
                }
                else if ((tag & 32) != 0)
                    this.WriteHdr( tag1 | 32 );
                else
                    this.WriteHdr( tag1 );
            }
            else
                this.WriteHdr( tag );
        }

        protected void WriteBerBody( Stream contentStream ) => Streams.PipeAll( contentStream, this.Out );

        protected void WriteBerEnd()
        {
            this.Out.WriteByte( 0 );
            this.Out.WriteByte( 0 );
            if (!this._tagged || !this._isExplicit)
                return;
            this.Out.WriteByte( 0 );
            this.Out.WriteByte( 0 );
        }
    }
}
