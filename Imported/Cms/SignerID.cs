// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SignerID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509.Store;

namespace Org.BouncyCastle.Cms
{
    public class SignerID : X509CertStoreSelector
    {
        public override int GetHashCode()
        {
            int hashCode = Arrays.GetHashCode( this.SubjectKeyIdentifier );
            BigInteger serialNumber = this.SerialNumber;
            if (serialNumber != null)
                hashCode ^= serialNumber.GetHashCode();
            X509Name issuer = this.Issuer;
            if (issuer != null)
                hashCode ^= issuer.GetHashCode();
            return hashCode;
        }

        public override bool Equals( object obj ) => obj != this && obj is SignerID signerId && Arrays.AreEqual( this.SubjectKeyIdentifier, signerId.SubjectKeyIdentifier ) && Equals( SerialNumber, signerId.SerialNumber ) && IssuersMatch( this.Issuer, signerId.Issuer );
    }
}
