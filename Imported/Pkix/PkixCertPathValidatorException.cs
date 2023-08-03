// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathValidatorException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Pkix
{
    [Serializable]
    public class PkixCertPathValidatorException : GeneralSecurityException
    {
        private Exception cause;
        private PkixCertPath certPath;
        private int index = -1;

        public PkixCertPathValidatorException()
        {
        }

        public PkixCertPathValidatorException( string message )
          : base( message )
        {
        }

        public PkixCertPathValidatorException( string message, Exception cause )
          : base( message )
        {
            this.cause = cause;
        }

        public PkixCertPathValidatorException(
          string message,
          Exception cause,
          PkixCertPath certPath,
          int index )
          : base( message )
        {
            if (certPath == null && index != -1)
                throw new ArgumentNullException( "certPath = null and index != -1" );
            if (index < -1 || (certPath != null && index >= certPath.Certificates.Count))
                throw new IndexOutOfRangeException( " index < -1 or out of bound of certPath.getCertificates()" );
            this.cause = cause;
            this.certPath = certPath;
            this.index = index;
        }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (message != null)
                    return message;
                return this.cause != null ? this.cause.Message : null;
            }
        }

        public PkixCertPath CertPath => this.certPath;

        public int Index => this.index;
    }
}
