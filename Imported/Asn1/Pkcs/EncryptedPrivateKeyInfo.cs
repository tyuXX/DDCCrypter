// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.EncryptedPrivateKeyInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class EncryptedPrivateKeyInfo : Asn1Encodable
    {
        private readonly AlgorithmIdentifier algId;
        private readonly Asn1OctetString data;

        private EncryptedPrivateKeyInfo( Asn1Sequence seq )
        {
            this.algId = seq.Count == 2 ? AlgorithmIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.data = Asn1OctetString.GetInstance( seq[1] );
        }

        public EncryptedPrivateKeyInfo( AlgorithmIdentifier algId, byte[] encoding )
        {
            this.algId = algId;
            this.data = new DerOctetString( encoding );
        }

        public static EncryptedPrivateKeyInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case EncryptedPrivateKeyInfo _:
                    return (EncryptedPrivateKeyInfo)obj;
                case Asn1Sequence _:
                    return new EncryptedPrivateKeyInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public AlgorithmIdentifier EncryptionAlgorithm => this.algId;

        public byte[] GetEncryptedData() => this.data.GetOctets();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       algId,
       data
        } );
    }
}
