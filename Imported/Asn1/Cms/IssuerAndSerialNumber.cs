// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class IssuerAndSerialNumber : Asn1Encodable
    {
        private X509Name name;
        private DerInteger serialNumber;

        public static IssuerAndSerialNumber GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is IssuerAndSerialNumber issuerAndSerialNumber ? issuerAndSerialNumber : new IssuerAndSerialNumber( Asn1Sequence.GetInstance( obj ) );
        }

        [Obsolete( "Use GetInstance() instead" )]
        public IssuerAndSerialNumber( Asn1Sequence seq )
        {
            this.name = X509Name.GetInstance( seq[0] );
            this.serialNumber = (DerInteger)seq[1];
        }

        public IssuerAndSerialNumber( X509Name name, BigInteger serialNumber )
        {
            this.name = name;
            this.serialNumber = new DerInteger( serialNumber );
        }

        public IssuerAndSerialNumber( X509Name name, DerInteger serialNumber )
        {
            this.name = name;
            this.serialNumber = serialNumber;
        }

        public X509Name Name => this.name;

        public DerInteger SerialNumber => this.serialNumber;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       name,
       serialNumber
        } );
    }
}
