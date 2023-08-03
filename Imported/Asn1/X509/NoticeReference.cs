// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.NoticeReference
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class NoticeReference : Asn1Encodable
    {
        private readonly DisplayText organization;
        private readonly Asn1Sequence noticeNumbers;

        private static Asn1EncodableVector ConvertVector( IList numbers )
        {
            Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (object number in (IEnumerable)numbers)
            {
                DerInteger derInteger;
                switch (number)
                {
                    case BigInteger _:
                        derInteger = new DerInteger( (BigInteger)number );
                        break;
                    case int num:
                        derInteger = new DerInteger( num );
                        break;
                    default:
                        throw new ArgumentException();
                }
                asn1EncodableVector.Add( derInteger );
            }
            return asn1EncodableVector;
        }

        public NoticeReference( string organization, IList numbers )
          : this( organization, ConvertVector( numbers ) )
        {
        }

        public NoticeReference( string organization, Asn1EncodableVector noticeNumbers )
          : this( new DisplayText( organization ), noticeNumbers )
        {
        }

        public NoticeReference( DisplayText organization, Asn1EncodableVector noticeNumbers )
        {
            this.organization = organization;
            this.noticeNumbers = new DerSequence( noticeNumbers );
        }

        private NoticeReference( Asn1Sequence seq )
        {
            this.organization = seq.Count == 2 ? DisplayText.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.noticeNumbers = Asn1Sequence.GetInstance( seq[1] );
        }

        public static NoticeReference GetInstance( object obj )
        {
            if (obj is NoticeReference)
                return (NoticeReference)obj;
            return obj == null ? null : new NoticeReference( Asn1Sequence.GetInstance( obj ) );
        }

        public virtual DisplayText Organization => this.organization;

        public virtual DerInteger[] GetNoticeNumbers()
        {
            DerInteger[] noticeNumbers = new DerInteger[this.noticeNumbers.Count];
            for (int index = 0; index != this.noticeNumbers.Count; ++index)
                noticeNumbers[index] = DerInteger.GetInstance( this.noticeNumbers[index] );
            return noticeNumbers;
        }

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       organization,
       noticeNumbers
        } );
    }
}
