// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.InvalidCipherTextException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto
{
    [Serializable]
    public class InvalidCipherTextException : CryptoException
    {
        public InvalidCipherTextException()
        {
        }

        public InvalidCipherTextException( string message )
          : base( message )
        {
        }

        public InvalidCipherTextException( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
