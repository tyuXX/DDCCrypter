﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.MaxBytesExceededException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto
{
    [Serializable]
    public class MaxBytesExceededException : CryptoException
    {
        public MaxBytesExceededException()
        {
        }

        public MaxBytesExceededException( string message )
          : base( message )
        {
        }

        public MaxBytesExceededException( string message, Exception e )
          : base( message, e )
        {
        }
    }
}
