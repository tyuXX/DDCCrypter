// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.RecipientID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509.Store;

namespace Org.BouncyCastle.Cms
{
    public class RecipientID : X509CertStoreSelector
    {
        private byte[] keyIdentifier;

        public byte[] KeyIdentifier
        {
            get => Arrays.Clone( this.keyIdentifier );
            set => this.keyIdentifier = Arrays.Clone( value );
        }

        public override int GetHashCode()
        {
            int hashCode = Arrays.GetHashCode( this.keyIdentifier ) ^ Arrays.GetHashCode( this.SubjectKeyIdentifier );
            BigInteger serialNumber = this.SerialNumber;
            if (serialNumber != null)
                hashCode ^= serialNumber.GetHashCode();
            X509Name issuer = this.Issuer;
            if (issuer != null)
                hashCode ^= issuer.GetHashCode();
            return hashCode;
        }

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is RecipientID recipientId && Arrays.AreEqual( this.keyIdentifier, recipientId.keyIdentifier ) && Arrays.AreEqual( this.SubjectKeyIdentifier, recipientId.SubjectKeyIdentifier ) && Equals( SerialNumber, recipientId.SerialNumber ) && IssuersMatch( this.Issuer, recipientId.Issuer );
        }
    }
}
