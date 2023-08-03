// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ess.EssCertIDv2
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ess
{
    public class EssCertIDv2 : Asn1Encodable
    {
        private readonly AlgorithmIdentifier hashAlgorithm;
        private readonly byte[] certHash;
        private readonly IssuerSerial issuerSerial;
        private static readonly AlgorithmIdentifier DefaultAlgID = new( NistObjectIdentifiers.IdSha256 );

        public static EssCertIDv2 GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is EssCertIDv2 essCertIdv2 ? essCertIdv2 : new EssCertIDv2( Asn1Sequence.GetInstance( obj ) );
        }

        private EssCertIDv2( Asn1Sequence seq )
        {
            if (seq.Count > 3)
                throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            int num = 0;
            this.hashAlgorithm = !(seq[0] is Asn1OctetString) ? AlgorithmIdentifier.GetInstance( seq[num++].ToAsn1Object() ) : DefaultAlgID;
            Asn1Sequence asn1Sequence = seq;
            int index1 = num;
            int index2 = index1 + 1;
            this.certHash = Asn1OctetString.GetInstance( asn1Sequence[index1].ToAsn1Object() ).GetOctets();
            if (seq.Count <= index2)
                return;
            this.issuerSerial = IssuerSerial.GetInstance( Asn1Sequence.GetInstance( seq[index2].ToAsn1Object() ) );
        }

        public EssCertIDv2( byte[] certHash )
          : this( null, certHash, null )
        {
        }

        public EssCertIDv2( AlgorithmIdentifier algId, byte[] certHash )
          : this( algId, certHash, null )
        {
        }

        public EssCertIDv2( byte[] certHash, IssuerSerial issuerSerial )
          : this( null, certHash, issuerSerial )
        {
        }

        public EssCertIDv2( AlgorithmIdentifier algId, byte[] certHash, IssuerSerial issuerSerial )
        {
            this.hashAlgorithm = algId != null ? algId : DefaultAlgID;
            this.certHash = certHash;
            this.issuerSerial = issuerSerial;
        }

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public byte[] GetCertHash() => Arrays.Clone( this.certHash );

        public IssuerSerial IssuerSerial => this.issuerSerial;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (!this.hashAlgorithm.Equals( DefaultAlgID ))
                v.Add( hashAlgorithm );
            v.Add( new DerOctetString( this.certHash ).ToAsn1Object() );
            if (this.issuerSerial != null)
                v.Add( issuerSerial );
            return new DerSequence( v );
        }
    }
}
