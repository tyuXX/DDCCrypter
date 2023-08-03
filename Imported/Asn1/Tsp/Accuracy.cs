// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Tsp.Accuracy
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Tsp
{
    public class Accuracy : Asn1Encodable
    {
        protected const int MinMillis = 1;
        protected const int MaxMillis = 999;
        protected const int MinMicros = 1;
        protected const int MaxMicros = 999;
        private readonly DerInteger seconds;
        private readonly DerInteger millis;
        private readonly DerInteger micros;

        public Accuracy( DerInteger seconds, DerInteger millis, DerInteger micros )
        {
            if (millis != null && (millis.Value.IntValue < 1 || millis.Value.IntValue > 999))
                throw new ArgumentException( "Invalid millis field : not in (1..999)" );
            if (micros != null && (micros.Value.IntValue < 1 || micros.Value.IntValue > 999))
                throw new ArgumentException( "Invalid micros field : not in (1..999)" );
            this.seconds = seconds;
            this.millis = millis;
            this.micros = micros;
        }

        private Accuracy( Asn1Sequence seq )
        {
            for (int index = 0; index < seq.Count; ++index)
            {
                if (seq[index] is DerInteger)
                    this.seconds = (DerInteger)seq[index];
                else if (seq[index] is DerTaggedObject)
                {
                    DerTaggedObject derTaggedObject = (DerTaggedObject)seq[index];
                    switch (derTaggedObject.TagNo)
                    {
                        case 0:
                            this.millis = DerInteger.GetInstance( derTaggedObject, false );
                            if (this.millis.Value.IntValue < 1 || this.millis.Value.IntValue > 999)
                                throw new ArgumentException( "Invalid millis field : not in (1..999)." );
                            continue;
                        case 1:
                            this.micros = DerInteger.GetInstance( derTaggedObject, false );
                            if (this.micros.Value.IntValue < 1 || this.micros.Value.IntValue > 999)
                                throw new ArgumentException( "Invalid micros field : not in (1..999)." );
                            continue;
                        default:
                            throw new ArgumentException( "Invalig tag number" );
                    }
                }
            }
        }

        public static Accuracy GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case Accuracy _:
                    return (Accuracy)o;
                case Asn1Sequence _:
                    return new Accuracy( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "Unknown object in 'Accuracy' factory: " + Platform.GetTypeName( o ) );
            }
        }

        public DerInteger Seconds => this.seconds;

        public DerInteger Millis => this.millis;

        public DerInteger Micros => this.micros;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.seconds != null)
                v.Add( seconds );
            if (this.millis != null)
                v.Add( new DerTaggedObject( false, 0, millis ) );
            if (this.micros != null)
                v.Add( new DerTaggedObject( false, 1, micros ) );
            return new DerSequence( v );
        }
    }
}
