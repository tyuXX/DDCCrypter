// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AttCertValidityPeriod
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AttCertValidityPeriod : Asn1Encodable
    {
        private readonly DerGeneralizedTime notBeforeTime;
        private readonly DerGeneralizedTime notAfterTime;

        public static AttCertValidityPeriod GetInstance( object obj )
        {
            switch (obj)
            {
                case AttCertValidityPeriod _:
                case null:
                    return (AttCertValidityPeriod)obj;
                case Asn1Sequence _:
                    return new AttCertValidityPeriod( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static AttCertValidityPeriod GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        private AttCertValidityPeriod( Asn1Sequence seq )
        {
            this.notBeforeTime = seq.Count == 2 ? DerGeneralizedTime.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.notAfterTime = DerGeneralizedTime.GetInstance( seq[1] );
        }

        public AttCertValidityPeriod( DerGeneralizedTime notBeforeTime, DerGeneralizedTime notAfterTime )
        {
            this.notBeforeTime = notBeforeTime;
            this.notAfterTime = notAfterTime;
        }

        public DerGeneralizedTime NotBeforeTime => this.notBeforeTime;

        public DerGeneralizedTime NotAfterTime => this.notAfterTime;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       notBeforeTime,
       notAfterTime
        } );
    }
}
