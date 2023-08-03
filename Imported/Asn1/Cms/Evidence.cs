// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.Evidence
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class Evidence : Asn1Encodable, IAsn1Choice
    {
        private TimeStampTokenEvidence tstEvidence;

        public Evidence( TimeStampTokenEvidence tstEvidence ) => this.tstEvidence = tstEvidence;

        private Evidence( Asn1TaggedObject tagged )
        {
            if (tagged.TagNo != 0)
                return;
            this.tstEvidence = TimeStampTokenEvidence.GetInstance( tagged, false );
        }

        public static Evidence GetInstance( object obj )
        {
            switch (obj)
            {
                case Evidence _:
                    return (Evidence)obj;
                case Asn1TaggedObject _:
                    return new Evidence( Asn1TaggedObject.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual TimeStampTokenEvidence TstEvidence => this.tstEvidence;

        public override Asn1Object ToAsn1Object() => this.tstEvidence != null ? new DerTaggedObject( false, 0, tstEvidence ) : (Asn1Object)null;
    }
}
