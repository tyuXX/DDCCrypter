// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.KekIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class KekIdentifier : Asn1Encodable
    {
        private Asn1OctetString keyIdentifier;
        private DerGeneralizedTime date;
        private OtherKeyAttribute other;

        public KekIdentifier( byte[] keyIdentifier, DerGeneralizedTime date, OtherKeyAttribute other )
        {
            this.keyIdentifier = new DerOctetString( keyIdentifier );
            this.date = date;
            this.other = other;
        }

        public KekIdentifier( Asn1Sequence seq )
        {
            this.keyIdentifier = (Asn1OctetString)seq[0];
            switch (seq.Count)
            {
                case 1:
                    break;
                case 2:
                    if (seq[1] is DerGeneralizedTime)
                    {
                        this.date = (DerGeneralizedTime)seq[1];
                        break;
                    }
                    this.other = OtherKeyAttribute.GetInstance( seq[2] );
                    break;
                case 3:
                    this.date = (DerGeneralizedTime)seq[1];
                    this.other = OtherKeyAttribute.GetInstance( seq[2] );
                    break;
                default:
                    throw new ArgumentException( "Invalid KekIdentifier" );
            }
        }

        public static KekIdentifier GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static KekIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case KekIdentifier _:
                    return (KekIdentifier)obj;
                case Asn1Sequence _:
                    return new KekIdentifier( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid KekIdentifier: " + Platform.GetTypeName( obj ) );
            }
        }

        public Asn1OctetString KeyIdentifier => this.keyIdentifier;

        public DerGeneralizedTime Date => this.date;

        public OtherKeyAttribute Other => this.other;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         keyIdentifier
            } );
            v.AddOptional( date, other );
            return new DerSequence( v );
        }
    }
}
