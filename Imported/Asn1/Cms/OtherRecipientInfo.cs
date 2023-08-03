// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.OtherRecipientInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class OtherRecipientInfo : Asn1Encodable
    {
        private readonly DerObjectIdentifier oriType;
        private readonly Asn1Encodable oriValue;

        public OtherRecipientInfo( DerObjectIdentifier oriType, Asn1Encodable oriValue )
        {
            this.oriType = oriType;
            this.oriValue = oriValue;
        }

        [Obsolete( "Use GetInstance() instead" )]
        public OtherRecipientInfo( Asn1Sequence seq )
        {
            this.oriType = DerObjectIdentifier.GetInstance( seq[0] );
            this.oriValue = seq[1];
        }

        public static OtherRecipientInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static OtherRecipientInfo GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is OtherRecipientInfo otherRecipientInfo ? otherRecipientInfo : new OtherRecipientInfo( Asn1Sequence.GetInstance( obj ) );
        }

        public virtual DerObjectIdentifier OriType => this.oriType;

        public virtual Asn1Encodable OriValue => this.oriValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       oriType,
      this.oriValue
        } );
    }
}
