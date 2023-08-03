// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.GeneralSubtree
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class GeneralSubtree : Asn1Encodable
    {
        private readonly GeneralName baseName;
        private readonly DerInteger minimum;
        private readonly DerInteger maximum;

        private GeneralSubtree( Asn1Sequence seq )
        {
            this.baseName = GeneralName.GetInstance( seq[0] );
            switch (seq.Count)
            {
                case 1:
                    break;
                case 2:
                    Asn1TaggedObject instance1 = Asn1TaggedObject.GetInstance( seq[1] );
                    switch (instance1.TagNo)
                    {
                        case 0:
                            this.minimum = DerInteger.GetInstance( instance1, false );
                            return;
                        case 1:
                            this.maximum = DerInteger.GetInstance( instance1, false );
                            return;
                        default:
                            throw new ArgumentException( "Bad tag number: " + instance1.TagNo );
                    }
                case 3:
                    Asn1TaggedObject instance2 = Asn1TaggedObject.GetInstance( seq[1] );
                    this.minimum = instance2.TagNo == 0 ? DerInteger.GetInstance( instance2, false ) : throw new ArgumentException( "Bad tag number for 'minimum': " + instance2.TagNo );
                    Asn1TaggedObject instance3 = Asn1TaggedObject.GetInstance( seq[2] );
                    this.maximum = instance3.TagNo == 1 ? DerInteger.GetInstance( instance3, false ) : throw new ArgumentException( "Bad tag number for 'maximum': " + instance3.TagNo );
                    break;
                default:
                    throw new ArgumentException( "Bad sequence size: " + seq.Count );
            }
        }

        public GeneralSubtree( GeneralName baseName, BigInteger minimum, BigInteger maximum )
        {
            this.baseName = baseName;
            if (minimum != null)
                this.minimum = new DerInteger( minimum );
            if (maximum == null)
                return;
            this.maximum = new DerInteger( maximum );
        }

        public GeneralSubtree( GeneralName baseName )
          : this( baseName, null, null )
        {
        }

        public static GeneralSubtree GetInstance( Asn1TaggedObject o, bool isExplicit ) => new GeneralSubtree( Asn1Sequence.GetInstance( o, isExplicit ) );

        public static GeneralSubtree GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is GeneralSubtree ? (GeneralSubtree)obj : new GeneralSubtree( Asn1Sequence.GetInstance( obj ) );
        }

        public GeneralName Base => this.baseName;

        public BigInteger Minimum => this.minimum != null ? this.minimum.Value : BigInteger.Zero;

        public BigInteger Maximum => this.maximum != null ? this.maximum.Value : null;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         baseName
            } );
            if (this.minimum != null && this.minimum.Value.SignValue != 0)
                v.Add( new DerTaggedObject( false, 0, minimum ) );
            if (this.maximum != null)
                v.Add( new DerTaggedObject( false, 1, maximum ) );
            return new DerSequence( v );
        }
    }
}
