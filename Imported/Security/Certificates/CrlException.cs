﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.Certificates.CrlException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Security.Certificates
{
    [Serializable]
    public class CrlException : GeneralSecurityException
    {
        public CrlException()
        {
        }

        public CrlException( string msg )
          : base( msg )
        {
        }

        public CrlException( string msg, Exception e )
          : base( msg, e )
        {
        }
    }
}
