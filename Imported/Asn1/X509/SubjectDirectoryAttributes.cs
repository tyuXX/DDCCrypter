// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.SubjectDirectoryAttributes
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class SubjectDirectoryAttributes : Asn1Encodable
    {
        private readonly IList attributes;

        public static SubjectDirectoryAttributes GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SubjectDirectoryAttributes _:
                    return (SubjectDirectoryAttributes)obj;
                case Asn1Sequence _:
                    return new SubjectDirectoryAttributes( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private SubjectDirectoryAttributes( Asn1Sequence seq )
        {
            this.attributes = Platform.CreateArrayList();
            foreach (object obj in seq)
                this.attributes.Add( AttributeX509.GetInstance( Asn1Sequence.GetInstance( obj ) ) );
        }

        [Obsolete]
        public SubjectDirectoryAttributes( ArrayList attributes )
          : this( (IList)attributes )
        {
        }

        public SubjectDirectoryAttributes( IList attributes ) => this.attributes = Platform.CreateArrayList( attributes );

        public override Asn1Object ToAsn1Object()
        {
            AttributeX509[] attributeX509Array = new AttributeX509[this.attributes.Count];
            for (int index = 0; index < this.attributes.Count; ++index)
                attributeX509Array[index] = (AttributeX509)this.attributes[index];
            return new DerSequence( attributeX509Array );
        }

        public IEnumerable Attributes => new EnumerableProxy( attributes );
    }
}
