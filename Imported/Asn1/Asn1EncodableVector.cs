// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1EncodableVector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1
{
    public class Asn1EncodableVector : IEnumerable
    {
        private IList v = Platform.CreateArrayList();

        public static Asn1EncodableVector FromEnumerable( IEnumerable e )
        {
            Asn1EncodableVector asn1EncodableVector = new( new Asn1Encodable[0] );
            foreach (Asn1Encodable asn1Encodable in e)
                asn1EncodableVector.Add( asn1Encodable );
            return asn1EncodableVector;
        }

        public Asn1EncodableVector( params Asn1Encodable[] v ) => this.Add( v );

        public void Add( params Asn1Encodable[] objs )
        {
            foreach (object obj in objs)
                this.v.Add( obj );
        }

        public void AddOptional( params Asn1Encodable[] objs )
        {
            if (objs == null)
                return;
            foreach (Asn1Encodable asn1Encodable in objs)
            {
                if (asn1Encodable != null)
                    this.v.Add( asn1Encodable );
            }
        }

        public Asn1Encodable this[int index] => (Asn1Encodable)this.v[index];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public Asn1Encodable Get( int index ) => this[index];

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.v.Count;

        public int Count => this.v.Count;

        public IEnumerator GetEnumerator() => this.v.GetEnumerator();
    }
}
