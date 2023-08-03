﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsSignerCredentials
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class AbstractTlsSignerCredentials :
    AbstractTlsCredentials,
    TlsSignerCredentials,
    TlsCredentials
    {
        public abstract byte[] GenerateCertificateSignature( byte[] hash );

        public virtual SignatureAndHashAlgorithm SignatureAndHashAlgorithm => throw new InvalidOperationException( "TlsSignerCredentials implementation does not support (D)TLS 1.2+" );
    }
}