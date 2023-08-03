// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.OtherHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.X509;
using System;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class OtherHash : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1OctetString sha1Hash;
        private readonly OtherHashAlgAndValue otherHash;

        public static OtherHash GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OtherHash _:
                    return (OtherHash)obj;
                case Asn1OctetString _:
                    return new OtherHash( (Asn1OctetString)obj );
                default:
                    return new OtherHash( OtherHashAlgAndValue.GetInstance( obj ) );
            }
        }

        public OtherHash( byte[] sha1Hash ) => this.sha1Hash = sha1Hash != null ? (Asn1OctetString)new DerOctetString( sha1Hash ) : throw new ArgumentNullException( nameof( sha1Hash ) );

        public OtherHash( Asn1OctetString sha1Hash ) => this.sha1Hash = sha1Hash != null ? sha1Hash : throw new ArgumentNullException( nameof( sha1Hash ) );

        public OtherHash( OtherHashAlgAndValue otherHash ) => this.otherHash = otherHash != null ? otherHash : throw new ArgumentNullException( nameof( otherHash ) );

        public AlgorithmIdentifier HashAlgorithm => this.otherHash != null ? this.otherHash.HashAlgorithm : new AlgorithmIdentifier( OiwObjectIdentifiers.IdSha1 );

        public byte[] GetHashValue() => this.otherHash != null ? this.otherHash.GetHashValue() : this.sha1Hash.GetOctets();

        public override Asn1Object ToAsn1Object() => this.otherHash != null ? this.otherHash.ToAsn1Object() : sha1Hash;
    }
}
