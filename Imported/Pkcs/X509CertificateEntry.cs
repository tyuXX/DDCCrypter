// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.X509CertificateEntry
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections;

namespace Org.BouncyCastle.Pkcs
{
    public class X509CertificateEntry : Pkcs12Entry
    {
        private readonly X509Certificate cert;

        public X509CertificateEntry( X509Certificate cert )
          : base( Platform.CreateHashtable() )
        {
            this.cert = cert;
        }

        [Obsolete]
        public X509CertificateEntry( X509Certificate cert, Hashtable attributes )
          : base( attributes )
        {
            this.cert = cert;
        }

        public X509CertificateEntry( X509Certificate cert, IDictionary attributes )
          : base( attributes )
        {
            this.cert = cert;
        }

        public X509Certificate Certificate => this.cert;

        public override bool Equals( object obj ) => obj is X509CertificateEntry certificateEntry && this.cert.Equals( certificateEntry.cert );

        public override int GetHashCode() => ~this.cert.GetHashCode();
    }
}
