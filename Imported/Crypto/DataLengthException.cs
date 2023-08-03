// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.DataLengthException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto
{
    [Serializable]
    public class DataLengthException : CryptoException
    {
        public DataLengthException()
        {
        }

        public DataLengthException( string message )
          : base( message )
        {
        }

        public DataLengthException( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
