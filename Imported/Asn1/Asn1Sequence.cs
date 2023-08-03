// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1Sequence
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1Sequence : Asn1Object, IEnumerable
    {
        private readonly IList seq;

        public static Asn1Sequence GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Asn1Sequence _:
                    return (Asn1Sequence)obj;
                case Asn1SequenceParser _:
                    return GetInstance( ((IAsn1Convertible)obj).ToAsn1Object() );
                case byte[] _:
                    try
                    {
                        return GetInstance( FromByteArray( (byte[])obj ) );
                    }
                    catch (IOException ex)
                    {
                        throw new ArgumentException( "failed to construct sequence from byte[]: " + ex.Message );
                    }
                case Asn1Encodable _:
                    Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
                    if (asn1Object is Asn1Sequence)
                        return (Asn1Sequence)asn1Object;
                    break;
            }
            throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        public static Asn1Sequence GetInstance( Asn1TaggedObject obj, bool explicitly )
        {
            Asn1Object instance = obj.GetObject();
            if (explicitly)
            {
                if (!obj.IsExplicit())
                    throw new ArgumentException( "object implicit - explicit expected." );
                return (Asn1Sequence)instance;
            }
            if (obj.IsExplicit())
                return obj is BerTaggedObject ? new BerSequence( instance ) : (Asn1Sequence)new DerSequence( instance );
            return instance is Asn1Sequence ? (Asn1Sequence)instance : throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        protected internal Asn1Sequence( int capacity ) => this.seq = Platform.CreateArrayList( capacity );

        public virtual IEnumerator GetEnumerator() => this.seq.GetEnumerator();

        [Obsolete( "Use GetEnumerator() instead" )]
        public IEnumerator GetObjects() => this.GetEnumerator();

        public virtual Asn1SequenceParser Parser => new Asn1Sequence.Asn1SequenceParserImpl( this );

        public virtual Asn1Encodable this[int index] => (Asn1Encodable)this.seq[index];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public Asn1Encodable GetObjectAt( int index ) => this[index];

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.Count;

        public virtual int Count => this.seq.Count;

        protected override int Asn1GetHashCode()
        {
            int count = this.Count;
            foreach (object obj in this)
            {
                count *= 17;
                if (obj == null)
                    count ^= DerNull.Instance.GetHashCode();
                else
                    count ^= obj.GetHashCode();
            }
            return count;
        }

        protected override bool Asn1Equals( Asn1Object asn1Object )
        {
            if (!(asn1Object is Asn1Sequence asn1Sequence) || this.Count != asn1Sequence.Count)
                return false;
            IEnumerator enumerator1 = this.GetEnumerator();
            IEnumerator enumerator2 = asn1Sequence.GetEnumerator();
            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                if (!this.GetCurrent( enumerator1 ).ToAsn1Object().Equals( this.GetCurrent( enumerator2 ).ToAsn1Object() ))
                    return false;
            }
            return true;
        }

        private Asn1Encodable GetCurrent( IEnumerator e ) => (Asn1Encodable)e.Current ?? DerNull.Instance;

        protected internal void AddObject( Asn1Encodable obj ) => this.seq.Add( obj );

        public override string ToString() => CollectionUtilities.ToString( seq );

        private class Asn1SequenceParserImpl : Asn1SequenceParser, IAsn1Convertible
        {
            private readonly Asn1Sequence outer;
            private readonly int max;
            private int index;

            public Asn1SequenceParserImpl( Asn1Sequence outer )
            {
                this.outer = outer;
                this.max = outer.Count;
            }

            public IAsn1Convertible ReadObject()
            {
                if (this.index == this.max)
                    return null;
                Asn1Encodable asn1Encodable = this.outer[this.index++];
                switch (asn1Encodable)
                {
                    case Asn1Sequence _:
                        return ((Asn1Sequence)asn1Encodable).Parser;
                    case Asn1Set _:
                        return ((Asn1Set)asn1Encodable).Parser;
                    default:
                        return asn1Encodable;
                }
            }

            public Asn1Object ToAsn1Object() => outer;
        }
    }
}
