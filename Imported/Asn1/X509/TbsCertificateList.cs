// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.TbsCertificateList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class TbsCertificateList : Asn1Encodable
    {
        internal Asn1Sequence seq;
        internal DerInteger version;
        internal AlgorithmIdentifier signature;
        internal X509Name issuer;
        internal Time thisUpdate;
        internal Time nextUpdate;
        internal Asn1Sequence revokedCertificates;
        internal X509Extensions crlExtensions;

        public static TbsCertificateList GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static TbsCertificateList GetInstance( object obj )
        {
            TbsCertificateList instance = obj as TbsCertificateList;
            if (obj == null || instance != null)
                return instance;
            return obj is Asn1Sequence ? new TbsCertificateList( (Asn1Sequence)obj ) : throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        internal TbsCertificateList( Asn1Sequence seq )
        {
            if (seq.Count < 3 || seq.Count > 7)
                throw new ArgumentException( "Bad sequence size: " + seq.Count );
            int index1 = 0;
            this.seq = seq;
            this.version = !(seq[index1] is DerInteger) ? new DerInteger( 0 ) : DerInteger.GetInstance( seq[index1++] );
            Asn1Sequence asn1Sequence1 = seq;
            int index2 = index1;
            int num1 = index2 + 1;
            this.signature = AlgorithmIdentifier.GetInstance( asn1Sequence1[index2] );
            Asn1Sequence asn1Sequence2 = seq;
            int index3 = num1;
            int num2 = index3 + 1;
            this.issuer = X509Name.GetInstance( asn1Sequence2[index3] );
            Asn1Sequence asn1Sequence3 = seq;
            int index4 = num2;
            int index5 = index4 + 1;
            this.thisUpdate = Time.GetInstance( asn1Sequence3[index4] );
            if (index5 < seq.Count && (seq[index5] is DerUtcTime || seq[index5] is DerGeneralizedTime || seq[index5] is Time))
                this.nextUpdate = Time.GetInstance( seq[index5++] );
            if (index5 < seq.Count && !(seq[index5] is DerTaggedObject))
                this.revokedCertificates = Asn1Sequence.GetInstance( seq[index5++] );
            if (index5 >= seq.Count || !(seq[index5] is DerTaggedObject))
                return;
            this.crlExtensions = X509Extensions.GetInstance( seq[index5] );
        }

        public int Version => this.version.Value.IntValue + 1;

        public DerInteger VersionNumber => this.version;

        public AlgorithmIdentifier Signature => this.signature;

        public X509Name Issuer => this.issuer;

        public Time ThisUpdate => this.thisUpdate;

        public Time NextUpdate => this.nextUpdate;

        public CrlEntry[] GetRevokedCertificates()
        {
            if (this.revokedCertificates == null)
                return new CrlEntry[0];
            CrlEntry[] revokedCertificates = new CrlEntry[this.revokedCertificates.Count];
            for (int index = 0; index < revokedCertificates.Length; ++index)
                revokedCertificates[index] = new CrlEntry( Asn1Sequence.GetInstance( this.revokedCertificates[index] ) );
            return revokedCertificates;
        }

        public IEnumerable GetRevokedCertificateEnumeration() => this.revokedCertificates == null ? EmptyEnumerable.Instance : new TbsCertificateList.RevokedCertificatesEnumeration( revokedCertificates );

        public X509Extensions Extensions => this.crlExtensions;

        public override Asn1Object ToAsn1Object() => seq;

        private class RevokedCertificatesEnumeration : IEnumerable
        {
            private readonly IEnumerable en;

            internal RevokedCertificatesEnumeration( IEnumerable en ) => this.en = en;

            public IEnumerator GetEnumerator() => new TbsCertificateList.RevokedCertificatesEnumeration.RevokedCertificatesEnumerator( this.en.GetEnumerator() );

            private class RevokedCertificatesEnumerator : IEnumerator
            {
                private readonly IEnumerator e;

                internal RevokedCertificatesEnumerator( IEnumerator e ) => this.e = e;

                public bool MoveNext() => this.e.MoveNext();

                public void Reset() => this.e.Reset();

                public object Current => new CrlEntry( Asn1Sequence.GetInstance( this.e.Current ) );
            }
        }
    }
}
