// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.TbsRequest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class TbsRequest : Asn1Encodable
    {
        private static readonly DerInteger V1 = new DerInteger( 0 );
        private readonly DerInteger version;
        private readonly GeneralName requestorName;
        private readonly Asn1Sequence requestList;
        private readonly X509Extensions requestExtensions;
        private bool versionSet;

        public static TbsRequest GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static TbsRequest GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case TbsRequest _:
                    return (TbsRequest)obj;
                case Asn1Sequence _:
                    return new TbsRequest( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public TbsRequest(
          GeneralName requestorName,
          Asn1Sequence requestList,
          X509Extensions requestExtensions )
        {
            this.version = V1;
            this.requestorName = requestorName;
            this.requestList = requestList;
            this.requestExtensions = requestExtensions;
        }

        private TbsRequest( Asn1Sequence seq )
        {
            int index1 = 0;
            Asn1Encodable asn1Encodable = seq[0];
            if (asn1Encodable is Asn1TaggedObject)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
                if (asn1TaggedObject.TagNo == 0)
                {
                    this.versionSet = true;
                    this.version = DerInteger.GetInstance( asn1TaggedObject, true );
                    ++index1;
                }
                else
                    this.version = V1;
            }
            else
                this.version = V1;
            if (seq[index1] is Asn1TaggedObject)
                this.requestorName = GeneralName.GetInstance( (Asn1TaggedObject)seq[index1++], true );
            Asn1Sequence asn1Sequence = seq;
            int index2 = index1;
            int index3 = index2 + 1;
            this.requestList = (Asn1Sequence)asn1Sequence[index2];
            if (seq.Count != index3 + 1)
                return;
            this.requestExtensions = X509Extensions.GetInstance( (Asn1TaggedObject)seq[index3], true );
        }

        public DerInteger Version => this.version;

        public GeneralName RequestorName => this.requestorName;

        public Asn1Sequence RequestList => this.requestList;

        public X509Extensions RequestExtensions => this.requestExtensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (!this.version.Equals( V1 ) || this.versionSet)
                v.Add( new DerTaggedObject( true, 0, version ) );
            if (this.requestorName != null)
                v.Add( new DerTaggedObject( true, 1, requestorName ) );
            v.Add( requestList );
            if (this.requestExtensions != null)
                v.Add( new DerTaggedObject( true, 2, requestExtensions ) );
            return new DerSequence( v );
        }
    }
}
