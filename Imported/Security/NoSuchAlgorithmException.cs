// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.NoSuchAlgorithmException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Security
{
    [Obsolete( "Never thrown" )]
    [Serializable]
    public class NoSuchAlgorithmException : GeneralSecurityException
    {
        public NoSuchAlgorithmException()
        {
        }

        public NoSuchAlgorithmException( string message )
          : base( message )
        {
        }

        public NoSuchAlgorithmException( string message, Exception exception )
          : base( message, exception )
        {
        }
    }
}
