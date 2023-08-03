// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AccessDescription
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AccessDescription : Asn1Encodable
    {
        public static readonly DerObjectIdentifier IdADCAIssuers = new DerObjectIdentifier( "1.3.6.1.5.5.7.48.2" );
        public static readonly DerObjectIdentifier IdADOcsp = new DerObjectIdentifier( "1.3.6.1.5.5.7.48.1" );
        private readonly DerObjectIdentifier accessMethod;
        private readonly GeneralName accessLocation;

        public static AccessDescription GetInstance( object obj )
        {
            switch (obj)
            {
                case AccessDescription _:
                    return (AccessDescription)obj;
                case Asn1Sequence _:
                    return new AccessDescription( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private AccessDescription( Asn1Sequence seq )
        {
            this.accessMethod = seq.Count == 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "wrong number of elements in sequence" );
            this.accessLocation = GeneralName.GetInstance( seq[1] );
        }

        public AccessDescription( DerObjectIdentifier oid, GeneralName location )
        {
            this.accessMethod = oid;
            this.accessLocation = location;
        }

        public DerObjectIdentifier AccessMethod => this.accessMethod;

        public GeneralName AccessLocation => this.accessLocation;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       accessMethod,
       accessLocation
        } );

        public override string ToString() => "AccessDescription: Oid(" + this.accessMethod.Id + ")";
    }
}
