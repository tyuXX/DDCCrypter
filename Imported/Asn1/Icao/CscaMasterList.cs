// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Icao.CscaMasterList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Icao
{
    public class CscaMasterList : Asn1Encodable
    {
        private DerInteger version = new( 0 );
        private X509CertificateStructure[] certList;

        public static CscaMasterList GetInstance( object obj )
        {
            if (obj is CscaMasterList)
                return (CscaMasterList)obj;
            return obj != null ? new CscaMasterList( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        private CscaMasterList( Asn1Sequence seq )
        {
            if (seq == null || seq.Count == 0)
                throw new ArgumentException( "null or empty sequence passed." );
            this.version = seq.Count == 2 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Incorrect sequence size: " + seq.Count );
            Asn1Set instance = Asn1Set.GetInstance( seq[1] );
            this.certList = new X509CertificateStructure[instance.Count];
            for (int index = 0; index < this.certList.Length; ++index)
                this.certList[index] = X509CertificateStructure.GetInstance( instance[index] );
        }

        public CscaMasterList( X509CertificateStructure[] certStructs ) => this.certList = CopyCertList( certStructs );

        public virtual int Version => this.version.Value.IntValue;

        public X509CertificateStructure[] GetCertStructs() => CopyCertList( this.certList );

        private static X509CertificateStructure[] CopyCertList( X509CertificateStructure[] orig ) => (X509CertificateStructure[])orig.Clone();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       version,
       new DerSet( certList)
        } );
    }
}
