// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsFatalAlert
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class TlsFatalAlert : IOException
    {
        private readonly byte alertDescription;

        public TlsFatalAlert( byte alertDescription )
          : this( alertDescription, null )
        {
        }

        public TlsFatalAlert( byte alertDescription, Exception alertCause )
          : base( Tls.AlertDescription.GetText( alertDescription ), alertCause )
        {
            this.alertDescription = alertDescription;
        }

        public virtual byte AlertDescription => this.alertDescription;
    }
}
