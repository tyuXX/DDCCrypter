// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.EncryptedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class EncryptedData : Asn1Encodable
    {
        private readonly Asn1Sequence data;

        public static EncryptedData GetInstance( object obj )
        {
            switch (obj)
            {
                case EncryptedData _:
                    return (EncryptedData)obj;
                case Asn1Sequence _:
                    return new EncryptedData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private EncryptedData( Asn1Sequence seq )
        {
            if (seq.Count != 2)
                throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            if (((DerInteger)seq[0]).Value.IntValue != 0)
                throw new ArgumentException( "sequence not version 0" );
            this.data = (Asn1Sequence)seq[1];
        }

        public EncryptedData(
          DerObjectIdentifier contentType,
          AlgorithmIdentifier encryptionAlgorithm,
          Asn1Encodable content )
        {
            this.data = new BerSequence( new Asn1Encodable[3]
            {
         contentType,
         encryptionAlgorithm.ToAsn1Object(),
         new BerTaggedObject(false, 0, content)
            } );
        }

        public DerObjectIdentifier ContentType => (DerObjectIdentifier)this.data[0];

        public AlgorithmIdentifier EncryptionAlgorithm => AlgorithmIdentifier.GetInstance( this.data[1] );

        public Asn1OctetString Content => this.data.Count == 3 ? Asn1OctetString.GetInstance( (Asn1TaggedObject)this.data[2], false ) : null;

        public override Asn1Object ToAsn1Object() => new BerSequence( new Asn1Encodable[2]
        {
       new DerInteger(0),
       data
        } );
    }
}
