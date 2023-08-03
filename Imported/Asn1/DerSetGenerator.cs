// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerSetGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerSetGenerator : DerGenerator
    {
        private readonly MemoryStream _bOut = new MemoryStream();

        public DerSetGenerator( Stream outStream )
          : base( outStream )
        {
        }

        public DerSetGenerator( Stream outStream, int tagNo, bool isExplicit )
          : base( outStream, tagNo, isExplicit )
        {
        }

        public override void AddObject( Asn1Encodable obj ) => new DerOutputStream( _bOut ).WriteObject( obj );

        public override Stream GetRawOutputStream() => _bOut;

        public override void Close() => this.WriteDerEncoded( 49, this._bOut.ToArray() );
    }
}
