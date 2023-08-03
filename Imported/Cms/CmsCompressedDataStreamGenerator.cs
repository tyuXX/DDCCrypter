// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsCompressedDataStreamGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.Utilities.Zlib;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsCompressedDataStreamGenerator
    {
        public const string ZLib = "1.2.840.113549.1.9.16.3.8";
        private int _bufferSize;

        public void SetBufferSize( int bufferSize ) => this._bufferSize = bufferSize;

        public Stream Open( Stream outStream, string compressionOID ) => this.Open( outStream, CmsObjectIdentifiers.Data.Id, compressionOID );

        public Stream Open( Stream outStream, string contentOID, string compressionOID )
        {
            BerSequenceGenerator sGen = new BerSequenceGenerator( outStream );
            sGen.AddObject( CmsObjectIdentifiers.CompressedData );
            BerSequenceGenerator cGen = new BerSequenceGenerator( sGen.GetRawOutputStream(), 0, true );
            cGen.AddObject( new DerInteger( 0 ) );
            cGen.AddObject( new AlgorithmIdentifier( new DerObjectIdentifier( "1.2.840.113549.1.9.16.3.8" ) ) );
            BerSequenceGenerator eiGen = new BerSequenceGenerator( cGen.GetRawOutputStream() );
            eiGen.AddObject( new DerObjectIdentifier( contentOID ) );
            return new CmsCompressedDataStreamGenerator.CmsCompressedOutputStream( new ZOutputStream( CmsUtilities.CreateBerOctetOutputStream( eiGen.GetRawOutputStream(), 0, true, this._bufferSize ), -1 ), sGen, cGen, eiGen );
        }

        private class CmsCompressedOutputStream : BaseOutputStream
        {
            private ZOutputStream _out;
            private BerSequenceGenerator _sGen;
            private BerSequenceGenerator _cGen;
            private BerSequenceGenerator _eiGen;

            internal CmsCompressedOutputStream(
              ZOutputStream outStream,
              BerSequenceGenerator sGen,
              BerSequenceGenerator cGen,
              BerSequenceGenerator eiGen )
            {
                this._out = outStream;
                this._sGen = sGen;
                this._cGen = cGen;
                this._eiGen = eiGen;
            }

            public override void WriteByte( byte b ) => this._out.WriteByte( b );

            public override void Write( byte[] bytes, int off, int len ) => this._out.Write( bytes, off, len );

            public override void Close()
            {
                Platform.Dispose( _out );
                this._eiGen.Close();
                this._cGen.Close();
                this._sGen.Close();
                base.Close();
            }
        }
    }
}
