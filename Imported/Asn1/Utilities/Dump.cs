// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Utilities.Dump
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Asn1.Utilities
{
    public sealed class Dump
    {
        private Dump()
        {
        }

        public static void Main( string[] args )
        {
            Asn1InputStream s = new Asn1InputStream( File.OpenRead( args[0] ) );
            Asn1Object asn1Object;
            while ((asn1Object = s.ReadObject()) != null)
                Console.WriteLine( Asn1Dump.DumpAsString( asn1Object ) );
            Platform.Dispose( s );
        }
    }
}
