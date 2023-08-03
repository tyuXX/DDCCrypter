// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Tsp.TimeStampReq
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Tsp
{
    public class TimeStampReq : Asn1Encodable
    {
        private readonly DerInteger version;
        private readonly MessageImprint messageImprint;
        private readonly DerObjectIdentifier tsaPolicy;
        private readonly DerInteger nonce;
        private readonly DerBoolean certReq;
        private readonly X509Extensions extensions;

        public static TimeStampReq GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case TimeStampReq _:
                    return (TimeStampReq)o;
                case Asn1Sequence _:
                    return new TimeStampReq( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "Unknown object in 'TimeStampReq' factory: " + Platform.GetTypeName( o ) );
            }
        }

        private TimeStampReq( Asn1Sequence seq )
        {
            int count = seq.Count;
            int num1 = 0;
            Asn1Sequence asn1Sequence1 = seq;
            int index1 = num1;
            int num2 = index1 + 1;
            this.version = DerInteger.GetInstance( asn1Sequence1[index1] );
            Asn1Sequence asn1Sequence2 = seq;
            int index2 = num2;
            int num3 = index2 + 1;
            this.messageImprint = MessageImprint.GetInstance( asn1Sequence2[index2] );
            for (int index3 = num3; index3 < count; ++index3)
            {
                if (seq[index3] is DerObjectIdentifier)
                    this.tsaPolicy = DerObjectIdentifier.GetInstance( seq[index3] );
                else if (seq[index3] is DerInteger)
                    this.nonce = DerInteger.GetInstance( seq[index3] );
                else if (seq[index3] is DerBoolean)
                    this.certReq = DerBoolean.GetInstance( seq[index3] );
                else if (seq[index3] is Asn1TaggedObject)
                {
                    Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index3];
                    if (asn1TaggedObject.TagNo == 0)
                        this.extensions = X509Extensions.GetInstance( asn1TaggedObject, false );
                }
            }
        }

        public TimeStampReq(
          MessageImprint messageImprint,
          DerObjectIdentifier tsaPolicy,
          DerInteger nonce,
          DerBoolean certReq,
          X509Extensions extensions )
        {
            this.version = new DerInteger( 1 );
            this.messageImprint = messageImprint;
            this.tsaPolicy = tsaPolicy;
            this.nonce = nonce;
            this.certReq = certReq;
            this.extensions = extensions;
        }

        public DerInteger Version => this.version;

        public MessageImprint MessageImprint => this.messageImprint;

        public DerObjectIdentifier ReqPolicy => this.tsaPolicy;

        public DerInteger Nonce => this.nonce;

        public DerBoolean CertReq => this.certReq;

        public X509Extensions Extensions => this.extensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         version,
         messageImprint
            } );
            if (this.tsaPolicy != null)
                v.Add( tsaPolicy );
            if (this.nonce != null)
                v.Add( nonce );
            if (this.certReq != null && this.certReq.IsTrue)
                v.Add( certReq );
            if (this.extensions != null)
                v.Add( new DerTaggedObject( false, 0, extensions ) );
            return new DerSequence( v );
        }
    }
}
