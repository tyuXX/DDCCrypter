// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.DHParameter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class DHParameter : Asn1Encodable
    {
        internal DerInteger p;
        internal DerInteger g;
        internal DerInteger l;

        public DHParameter( BigInteger p, BigInteger g, int l )
        {
            this.p = new DerInteger( p );
            this.g = new DerInteger( g );
            if (l == 0)
                return;
            this.l = new DerInteger( l );
        }

        public DHParameter( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.p = (DerInteger)enumerator.Current;
            enumerator.MoveNext();
            this.g = (DerInteger)enumerator.Current;
            if (!enumerator.MoveNext())
                return;
            this.l = (DerInteger)enumerator.Current;
        }

        public BigInteger P => this.p.PositiveValue;

        public BigInteger G => this.g.PositiveValue;

        public BigInteger L => this.l != null ? this.l.PositiveValue : null;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         p,
         g
            } );
            if (this.l != null)
                v.Add( l );
            return new DerSequence( v );
        }
    }
}
