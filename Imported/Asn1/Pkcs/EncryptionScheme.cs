// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.EncryptionScheme
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class EncryptionScheme : AlgorithmIdentifier
    {
        public EncryptionScheme( DerObjectIdentifier objectID, Asn1Encodable parameters )
          : base( objectID, parameters )
        {
        }

        internal EncryptionScheme( Asn1Sequence seq )
          : this( (DerObjectIdentifier)seq[0], seq[1] )
        {
        }

        public static EncryptionScheme GetInstance( object obj )
        {
            switch (obj)
            {
                case EncryptionScheme _:
                    return (EncryptionScheme)obj;
                case Asn1Sequence _:
                    return new EncryptionScheme( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Asn1Object Asn1Object => this.Parameters.ToAsn1Object();

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       Algorithm,
      this.Parameters
        } );
    }
}
