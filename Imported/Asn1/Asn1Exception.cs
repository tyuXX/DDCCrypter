// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1Exception
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    [Serializable]
    public class Asn1Exception : IOException
    {
        public Asn1Exception()
        {
        }

        public Asn1Exception( string message )
          : base( message )
        {
        }

        public Asn1Exception( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
