// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.Ocsp.CertHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.IsisMtt.Ocsp
{
    public class CertHash : Asn1Encodable
    {
        private readonly AlgorithmIdentifier hashAlgorithm;
        private readonly byte[] certificateHash;

        public static CertHash GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CertHash _:
                    return (CertHash)obj;
                case Asn1Sequence _:
                    return new CertHash( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CertHash( Asn1Sequence seq )
        {
            this.hashAlgorithm = seq.Count == 2 ? AlgorithmIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.certificateHash = Asn1OctetString.GetInstance( seq[1] ).GetOctets();
        }

        public CertHash( AlgorithmIdentifier hashAlgorithm, byte[] certificateHash )
        {
            if (hashAlgorithm == null)
                throw new ArgumentNullException( nameof( hashAlgorithm ) );
            if (certificateHash == null)
                throw new ArgumentNullException( nameof( certificateHash ) );
            this.hashAlgorithm = hashAlgorithm;
            this.certificateHash = (byte[])certificateHash.Clone();
        }

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public byte[] CertificateHash => (byte[])this.certificateHash.Clone();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       hashAlgorithm,
       new DerOctetString(this.certificateHash)
        } );
    }
}
