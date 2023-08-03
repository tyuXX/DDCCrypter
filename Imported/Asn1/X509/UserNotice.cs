// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.UserNotice
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class UserNotice : Asn1Encodable
    {
        private readonly NoticeReference noticeRef;
        private readonly DisplayText explicitText;

        public UserNotice( NoticeReference noticeRef, DisplayText explicitText )
        {
            this.noticeRef = noticeRef;
            this.explicitText = explicitText;
        }

        public UserNotice( NoticeReference noticeRef, string str )
          : this( noticeRef, new DisplayText( str ) )
        {
        }

        public UserNotice( Asn1Sequence seq )
        {
            if (seq.Count == 2)
            {
                this.noticeRef = NoticeReference.GetInstance( seq[0] );
                this.explicitText = DisplayText.GetInstance( seq[1] );
            }
            else
            {
                if (seq.Count != 1)
                    throw new ArgumentException( "Bad sequence size: " + seq.Count );
                if (seq[0].ToAsn1Object() is Asn1Sequence)
                    this.noticeRef = NoticeReference.GetInstance( seq[0] );
                else
                    this.explicitText = DisplayText.GetInstance( seq[0] );
            }
        }

        public static UserNotice GetInstance( object obj )
        {
            if (obj is UserNotice)
                return (UserNotice)obj;
            return obj == null ? null : new UserNotice( Asn1Sequence.GetInstance( obj ) );
        }

        public virtual NoticeReference NoticeRef => this.noticeRef;

        public virtual DisplayText ExplicitText => this.explicitText;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.noticeRef != null)
                v.Add( noticeRef );
            if (this.explicitText != null)
                v.Add( explicitText );
            return new DerSequence( v );
        }
    }
}
