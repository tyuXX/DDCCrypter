﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.Pem.PemGenerationException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
    [Serializable]
    public class PemGenerationException : Exception
    {
        public PemGenerationException()
        {
        }

        public PemGenerationException( string message )
          : base( message )
        {
        }

        public PemGenerationException( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
