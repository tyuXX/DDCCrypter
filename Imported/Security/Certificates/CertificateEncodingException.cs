// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.Certificates.CertificateEncodingException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Security.Certificates
{
    [Serializable]
    public class CertificateEncodingException : CertificateException
    {
        public CertificateEncodingException()
        {
        }

        public CertificateEncodingException( string msg )
          : base( msg )
        {
        }

        public CertificateEncodingException( string msg, Exception e )
          : base( msg, e )
        {
        }
    }
}
