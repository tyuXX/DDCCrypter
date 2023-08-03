// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.RevokedInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class RevokedInfo : Asn1Encodable
    {
        private readonly DerGeneralizedTime revocationTime;
        private readonly CrlReason revocationReason;

        public static RevokedInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static RevokedInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RevokedInfo _:
                    return (RevokedInfo)obj;
                case Asn1Sequence _:
                    return new RevokedInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public RevokedInfo( DerGeneralizedTime revocationTime )
          : this( revocationTime, null )
        {
        }

        public RevokedInfo( DerGeneralizedTime revocationTime, CrlReason revocationReason )
        {
            this.revocationTime = revocationTime != null ? revocationTime : throw new ArgumentNullException( nameof( revocationTime ) );
            this.revocationReason = revocationReason;
        }

        private RevokedInfo( Asn1Sequence seq )
        {
            this.revocationTime = (DerGeneralizedTime)seq[0];
            if (seq.Count <= 1)
                return;
            this.revocationReason = new CrlReason( DerEnumerated.GetInstance( (Asn1TaggedObject)seq[1], true ) );
        }

        public DerGeneralizedTime RevocationTime => this.revocationTime;

        public CrlReason RevocationReason => this.revocationReason;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         revocationTime
            } );
            if (this.revocationReason != null)
                v.Add( new DerTaggedObject( true, 0, revocationReason ) );
            return new DerSequence( v );
        }
    }
}
