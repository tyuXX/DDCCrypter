// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.BiometricData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public class BiometricData : Asn1Encodable
    {
        private readonly TypeOfBiometricData typeOfBiometricData;
        private readonly AlgorithmIdentifier hashAlgorithm;
        private readonly Asn1OctetString biometricDataHash;
        private readonly DerIA5String sourceDataUri;

        public static BiometricData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case BiometricData _:
                    return (BiometricData)obj;
                case Asn1Sequence _:
                    return new BiometricData( Asn1Sequence.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private BiometricData( Asn1Sequence seq )
        {
            this.typeOfBiometricData = TypeOfBiometricData.GetInstance( seq[0] );
            this.hashAlgorithm = AlgorithmIdentifier.GetInstance( seq[1] );
            this.biometricDataHash = Asn1OctetString.GetInstance( seq[2] );
            if (seq.Count <= 3)
                return;
            this.sourceDataUri = DerIA5String.GetInstance( seq[3] );
        }

        public BiometricData(
          TypeOfBiometricData typeOfBiometricData,
          AlgorithmIdentifier hashAlgorithm,
          Asn1OctetString biometricDataHash,
          DerIA5String sourceDataUri )
        {
            this.typeOfBiometricData = typeOfBiometricData;
            this.hashAlgorithm = hashAlgorithm;
            this.biometricDataHash = biometricDataHash;
            this.sourceDataUri = sourceDataUri;
        }

        public BiometricData(
          TypeOfBiometricData typeOfBiometricData,
          AlgorithmIdentifier hashAlgorithm,
          Asn1OctetString biometricDataHash )
        {
            this.typeOfBiometricData = typeOfBiometricData;
            this.hashAlgorithm = hashAlgorithm;
            this.biometricDataHash = biometricDataHash;
            this.sourceDataUri = null;
        }

        public TypeOfBiometricData TypeOfBiometricData => this.typeOfBiometricData;

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public Asn1OctetString BiometricDataHash => this.biometricDataHash;

        public DerIA5String SourceDataUri => this.sourceDataUri;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         typeOfBiometricData,
         hashAlgorithm,
         biometricDataHash
            } );
            if (this.sourceDataUri != null)
                v.Add( sourceDataUri );
            return new DerSequence( v );
        }
    }
}
