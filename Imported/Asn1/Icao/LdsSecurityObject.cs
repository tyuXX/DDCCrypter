// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Icao.LdsSecurityObject
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Icao
{
    public class LdsSecurityObject : Asn1Encodable
    {
        public const int UBDataGroups = 16;
        private DerInteger version = new( 0 );
        private AlgorithmIdentifier digestAlgorithmIdentifier;
        private DataGroupHash[] datagroupHash;
        private LdsVersionInfo versionInfo;

        public static LdsSecurityObject GetInstance( object obj )
        {
            if (obj is LdsSecurityObject)
                return (LdsSecurityObject)obj;
            return obj != null ? new LdsSecurityObject( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        private LdsSecurityObject( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq != null && seq.Count != 0 ? seq.GetEnumerator() : throw new ArgumentException( "null or empty sequence passed." );
            enumerator.MoveNext();
            this.version = DerInteger.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.digestAlgorithmIdentifier = AlgorithmIdentifier.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            Asn1Sequence instance = Asn1Sequence.GetInstance( enumerator.Current );
            if (this.version.Value.Equals( BigInteger.One ))
            {
                enumerator.MoveNext();
                this.versionInfo = LdsVersionInfo.GetInstance( enumerator.Current );
            }
            this.CheckDatagroupHashSeqSize( instance.Count );
            this.datagroupHash = new DataGroupHash[instance.Count];
            for (int index = 0; index < instance.Count; ++index)
                this.datagroupHash[index] = DataGroupHash.GetInstance( instance[index] );
        }

        public LdsSecurityObject(
          AlgorithmIdentifier digestAlgorithmIdentifier,
          DataGroupHash[] datagroupHash )
        {
            this.version = new DerInteger( 0 );
            this.digestAlgorithmIdentifier = digestAlgorithmIdentifier;
            this.datagroupHash = datagroupHash;
            this.CheckDatagroupHashSeqSize( datagroupHash.Length );
        }

        public LdsSecurityObject(
          AlgorithmIdentifier digestAlgorithmIdentifier,
          DataGroupHash[] datagroupHash,
          LdsVersionInfo versionInfo )
        {
            this.version = new DerInteger( 1 );
            this.digestAlgorithmIdentifier = digestAlgorithmIdentifier;
            this.datagroupHash = datagroupHash;
            this.versionInfo = versionInfo;
            this.CheckDatagroupHashSeqSize( datagroupHash.Length );
        }

        private void CheckDatagroupHashSeqSize( int size )
        {
            if (size < 2 || size > 16)
                throw new ArgumentException( "wrong size in DataGroupHashValues : not in (2.." + 16 + ")" );
        }

        public BigInteger Version => this.version.Value;

        public AlgorithmIdentifier DigestAlgorithmIdentifier => this.digestAlgorithmIdentifier;

        public DataGroupHash[] GetDatagroupHash() => this.datagroupHash;

        public LdsVersionInfo VersionInfo => this.versionInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         version,
         digestAlgorithmIdentifier,
         new DerSequence( datagroupHash)
            } );
            if (this.versionInfo != null)
                v.Add( versionInfo );
            return new DerSequence( v );
        }
    }
}
