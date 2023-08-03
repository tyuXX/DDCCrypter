// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Icao.DataGroupHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Icao
{
    public class DataGroupHash : Asn1Encodable
    {
        private readonly DerInteger dataGroupNumber;
        private readonly Asn1OctetString dataGroupHashValue;

        public static DataGroupHash GetInstance( object obj )
        {
            if (obj is DataGroupHash)
                return (DataGroupHash)obj;
            return obj != null ? new DataGroupHash( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        private DataGroupHash( Asn1Sequence seq )
        {
            this.dataGroupNumber = seq.Count == 2 ? DerInteger.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.dataGroupHashValue = Asn1OctetString.GetInstance( seq[1] );
        }

        public DataGroupHash( int dataGroupNumber, Asn1OctetString dataGroupHashValue )
        {
            this.dataGroupNumber = new DerInteger( dataGroupNumber );
            this.dataGroupHashValue = dataGroupHashValue;
        }

        public int DataGroupNumber => this.dataGroupNumber.Value.IntValue;

        public Asn1OctetString DataGroupHashValue => this.dataGroupHashValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       dataGroupNumber,
       dataGroupHashValue
        } );
    }
}
