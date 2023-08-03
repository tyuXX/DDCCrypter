// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1Set
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1Set : Asn1Object, IEnumerable
    {
        private readonly IList _set;

        public static Asn1Set GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Asn1Set _:
                    return (Asn1Set)obj;
                case Asn1SetParser _:
                    return GetInstance( ((IAsn1Convertible)obj).ToAsn1Object() );
                case byte[] _:
                    try
                    {
                        return GetInstance( FromByteArray( (byte[])obj ) );
                    }
                    catch (IOException ex)
                    {
                        throw new ArgumentException( "failed to construct set from byte[]: " + ex.Message );
                    }
                case Asn1Encodable _:
                    Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
                    if (asn1Object is Asn1Set)
                        return (Asn1Set)asn1Object;
                    break;
            }
            throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        public static Asn1Set GetInstance( Asn1TaggedObject obj, bool explicitly )
        {
            Asn1Object instance = obj.GetObject();
            if (explicitly)
            {
                if (!obj.IsExplicit())
                    throw new ArgumentException( "object implicit - explicit expected." );
                return (Asn1Set)instance;
            }
            if (obj.IsExplicit())
                return new DerSet( instance );
            switch (instance)
            {
                case Asn1Set _:
                    return (Asn1Set)instance;
                case Asn1Sequence _:
                    Asn1EncodableVector v = new( new Asn1Encodable[0] );
                    foreach (Asn1Encodable asn1Encodable in (Asn1Sequence)instance)
                        v.Add( asn1Encodable );
                    return new DerSet( v, false );
                default:
                    throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        protected internal Asn1Set( int capacity ) => this._set = Platform.CreateArrayList( capacity );

        public virtual IEnumerator GetEnumerator() => this._set.GetEnumerator();

        [Obsolete( "Use GetEnumerator() instead" )]
        public IEnumerator GetObjects() => this.GetEnumerator();

        public virtual Asn1Encodable this[int index] => (Asn1Encodable)this._set[index];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public Asn1Encodable GetObjectAt( int index ) => this[index];

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.Count;

        public virtual int Count => this._set.Count;

        public virtual Asn1Encodable[] ToArray()
        {
            Asn1Encodable[] array = new Asn1Encodable[this.Count];
            for (int index = 0; index < this.Count; ++index)
                array[index] = this[index];
            return array;
        }

        public Asn1SetParser Parser => new Asn1Set.Asn1SetParserImpl( this );

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
            if (!(asn1Object is Asn1Set asn1Set) || this.Count != asn1Set.Count)
                return false;
            IEnumerator enumerator1 = this.GetEnumerator();
            IEnumerator enumerator2 = asn1Set.GetEnumerator();
            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                if (!this.GetCurrent( enumerator1 ).ToAsn1Object().Equals( this.GetCurrent( enumerator2 ).ToAsn1Object() ))
                    return false;
            }
            return true;
        }

        private Asn1Encodable GetCurrent( IEnumerator e ) => (Asn1Encodable)e.Current ?? DerNull.Instance;

        protected internal void Sort()
        {
            if (this._set.Count < 2)
                return;
            Asn1Encodable[] items = new Asn1Encodable[this._set.Count];
            byte[][] keys = new byte[this._set.Count][];
            for (int index = 0; index < this._set.Count; ++index)
            {
                Asn1Encodable asn1Encodable = (Asn1Encodable)this._set[index];
                items[index] = asn1Encodable;
                keys[index] = asn1Encodable.GetEncoded( "DER" );
            }
            Array.Sort( keys, items, new Asn1Set.DerComparer() );
            for (int index = 0; index < this._set.Count; ++index)
                this._set[index] = items[index];
        }

        protected internal void AddObject( Asn1Encodable obj ) => this._set.Add( obj );

        public override string ToString() => CollectionUtilities.ToString( _set );

        private class Asn1SetParserImpl : Asn1SetParser, IAsn1Convertible
        {
            private readonly Asn1Set outer;
            private readonly int max;
            private int index;

            public Asn1SetParserImpl( Asn1Set outer )
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

            public virtual Asn1Object ToAsn1Object() => outer;
        }

        private class DerComparer : IComparer
        {
            public int Compare( object x, object y )
            {
                byte[] bs1 = (byte[])x;
                byte[] bs2 = (byte[])y;
                int pos = System.Math.Min( bs1.Length, bs2.Length );
                for (int index = 0; index != pos; ++index)
                {
                    byte num1 = bs1[index];
                    byte num2 = bs2[index];
                    if (num1 != num2)
                        return num1 >= num2 ? 1 : -1;
                }
                return bs1.Length > bs2.Length ? (!this.AllZeroesFrom( bs1, pos ) ? 1 : 0) : (bs1.Length < bs2.Length && !this.AllZeroesFrom( bs2, pos ) ? -1 : 0);
            }

            private bool AllZeroesFrom( byte[] bs, int pos )
            {
                while (pos < bs.Length)
                {
                    if (bs[pos++] != 0)
                        return false;
                }
                return true;
            }
        }
    }
}
