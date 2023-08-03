// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.OriginatorInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class OriginatorInfo : Asn1Encodable
    {
        private Asn1Set certs;
        private Asn1Set crls;

        public OriginatorInfo( Asn1Set certs, Asn1Set crls )
        {
            this.certs = certs;
            this.crls = crls;
        }

        public OriginatorInfo( Asn1Sequence seq )
        {
            switch (seq.Count)
            {
                case 0:
                    break;
                case 1:
                    Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[0];
                    switch (asn1TaggedObject.TagNo)
                    {
                        case 0:
                            this.certs = Asn1Set.GetInstance( asn1TaggedObject, false );
                            return;
                        case 1:
                            this.crls = Asn1Set.GetInstance( asn1TaggedObject, false );
                            return;
                        default:
                            throw new ArgumentException( "Bad tag in OriginatorInfo: " + asn1TaggedObject.TagNo );
                    }
                case 2:
                    this.certs = Asn1Set.GetInstance( (Asn1TaggedObject)seq[0], false );
                    this.crls = Asn1Set.GetInstance( (Asn1TaggedObject)seq[1], false );
                    break;
                default:
                    throw new ArgumentException( "OriginatorInfo too big" );
            }
        }

        public static OriginatorInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static OriginatorInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case OriginatorInfo _:
                    return (OriginatorInfo)obj;
                case Asn1Sequence _:
                    return new OriginatorInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid OriginatorInfo: " + Platform.GetTypeName( obj ) );
            }
        }

        public Asn1Set Certificates => this.certs;

        public Asn1Set Crls => this.crls;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.certs != null)
                v.Add( new DerTaggedObject( false, 0, certs ) );
            if (this.crls != null)
                v.Add( new DerTaggedObject( false, 1, crls ) );
            return new DerSequence( v );
        }
    }
}
