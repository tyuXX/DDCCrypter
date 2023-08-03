// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1ParsingException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1
{
    [Serializable]
    public class Asn1ParsingException : InvalidOperationException
    {
        public Asn1ParsingException()
        {
        }

        public Asn1ParsingException( string message )
          : base( message )
        {
        }

        public Asn1ParsingException( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
