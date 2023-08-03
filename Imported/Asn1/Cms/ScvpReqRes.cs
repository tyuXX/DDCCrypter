// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.ScvpReqRes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cms
{
    public class ScvpReqRes : Asn1Encodable
    {
        private readonly ContentInfo request;
        private readonly ContentInfo response;

        public static ScvpReqRes GetInstance( object obj )
        {
            if (obj is ScvpReqRes)
                return (ScvpReqRes)obj;
            return obj != null ? new ScvpReqRes( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        private ScvpReqRes( Asn1Sequence seq )
        {
            if (seq[0] is Asn1TaggedObject)
            {
                this.request = ContentInfo.GetInstance( Asn1TaggedObject.GetInstance( seq[0] ), true );
                this.response = ContentInfo.GetInstance( seq[1] );
            }
            else
            {
                this.request = null;
                this.response = ContentInfo.GetInstance( seq[0] );
            }
        }

        public ScvpReqRes( ContentInfo response )
          : this( null, response )
        {
        }

        public ScvpReqRes( ContentInfo request, ContentInfo response )
        {
            this.request = request;
            this.response = response;
        }

        public virtual ContentInfo Request => this.request;

        public virtual ContentInfo Response => this.response;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.request != null)
                v.Add( new DerTaggedObject( true, 0, request ) );
            v.Add( response );
            return new DerSequence( v );
        }
    }
}
