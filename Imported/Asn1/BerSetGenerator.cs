// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerSetGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerSetGenerator : BerGenerator
    {
        public BerSetGenerator( Stream outStream )
          : base( outStream )
        {
            this.WriteBerHeader( 49 );
        }

        public BerSetGenerator( Stream outStream, int tagNo, bool isExplicit )
          : base( outStream, tagNo, isExplicit )
        {
            this.WriteBerHeader( 49 );
        }
    }
}
