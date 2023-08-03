// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.NameConstraints
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class NameConstraints : Asn1Encodable
    {
        private Asn1Sequence permitted;
        private Asn1Sequence excluded;

        public static NameConstraints GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case NameConstraints _:
                    return (NameConstraints)obj;
                case Asn1Sequence _:
                    return new NameConstraints( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public NameConstraints( Asn1Sequence seq )
        {
            foreach (Asn1TaggedObject asn1TaggedObject in seq)
            {
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.permitted = Asn1Sequence.GetInstance( asn1TaggedObject, false );
                        continue;
                    case 1:
                        this.excluded = Asn1Sequence.GetInstance( asn1TaggedObject, false );
                        continue;
                    default:
                        continue;
                }
            }
        }

        public NameConstraints( ArrayList permitted, ArrayList excluded )
          : this( permitted, (IList)excluded )
        {
        }

        public NameConstraints( IList permitted, IList excluded )
        {
            if (permitted != null)
                this.permitted = this.CreateSequence( permitted );
            if (excluded == null)
                return;
            this.excluded = this.CreateSequence( excluded );
        }

        private DerSequence CreateSequence( IList subtrees )
        {
            GeneralSubtree[] generalSubtreeArray = new GeneralSubtree[subtrees.Count];
            for (int index = 0; index < subtrees.Count; ++index)
                generalSubtreeArray[index] = (GeneralSubtree)subtrees[index];
            return new DerSequence( generalSubtreeArray );
        }

        public Asn1Sequence PermittedSubtrees => this.permitted;

        public Asn1Sequence ExcludedSubtrees => this.excluded;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.permitted != null)
                v.Add( new DerTaggedObject( false, 0, permitted ) );
            if (this.excluded != null)
                v.Add( new DerTaggedObject( false, 1, excluded ) );
            return new DerSequence( v );
        }
    }
}
