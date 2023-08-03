// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CertId
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class CertId : Asn1Encodable
    {
        private readonly GeneralName issuer;
        private readonly DerInteger serialNumber;

        private CertId( Asn1Sequence seq )
        {
            this.issuer = GeneralName.GetInstance( seq[0] );
            this.serialNumber = DerInteger.GetInstance( seq[1] );
        }

        public static CertId GetInstance( object obj )
        {
            switch (obj)
            {
                case CertId _:
                    return (CertId)obj;
                case Asn1Sequence _:
                    return new CertId( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static CertId GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public virtual GeneralName Issuer => this.issuer;

        public virtual DerInteger SerialNumber => this.serialNumber;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       issuer,
       serialNumber
        } );
    }
}
