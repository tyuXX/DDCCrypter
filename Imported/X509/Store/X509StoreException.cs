// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Store.X509StoreException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.X509.Store
{
    [Serializable]
    public class X509StoreException : Exception
    {
        public X509StoreException()
        {
        }

        public X509StoreException( string message )
          : base( message )
        {
        }

        public X509StoreException( string message, Exception e )
          : base( message, e )
        {
        }
    }
}
