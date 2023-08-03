// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.KeyException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Security
{
    [Serializable]
    public class KeyException : GeneralSecurityException
    {
        public KeyException()
        {
        }

        public KeyException( string message )
          : base( message )
        {
        }

        public KeyException( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
