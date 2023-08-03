// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.IX509AttributeCertificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.X509
{
    public interface IX509AttributeCertificate : IX509Extension
    {
        int Version { get; }

        BigInteger SerialNumber { get; }

        DateTime NotBefore { get; }

        DateTime NotAfter { get; }

        AttributeCertificateHolder Holder { get; }

        AttributeCertificateIssuer Issuer { get; }

        X509Attribute[] GetAttributes();

        X509Attribute[] GetAttributes( string oid );

        bool[] GetIssuerUniqueID();

        bool IsValidNow { get; }

        bool IsValid( DateTime date );

        void CheckValidity();

        void CheckValidity( DateTime date );

        byte[] GetSignature();

        void Verify( AsymmetricKeyParameter publicKey );

        byte[] GetEncoded();
    }
}
