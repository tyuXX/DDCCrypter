// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.PkiPublicationInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class PkiPublicationInfo : Asn1Encodable
    {
        private readonly DerInteger action;
        private readonly Asn1Sequence pubInfos;

        private PkiPublicationInfo( Asn1Sequence seq )
        {
            this.action = DerInteger.GetInstance( seq[0] );
            this.pubInfos = Asn1Sequence.GetInstance( seq[1] );
        }

        public static PkiPublicationInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiPublicationInfo _:
                    return (PkiPublicationInfo)obj;
                case Asn1Sequence _:
                    return new PkiPublicationInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual DerInteger Action => this.action;

        public virtual SinglePubInfo[] GetPubInfos()
        {
            if (this.pubInfos == null)
                return null;
            SinglePubInfo[] pubInfos = new SinglePubInfo[this.pubInfos.Count];
            for (int index = 0; index != pubInfos.Length; ++index)
                pubInfos[index] = SinglePubInfo.GetInstance( this.pubInfos[index] );
            return pubInfos;
        }

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       action,
       pubInfos
        } );
    }
}
