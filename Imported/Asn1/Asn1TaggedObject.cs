// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1TaggedObject
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1TaggedObject : Asn1Object, Asn1TaggedObjectParser, IAsn1Convertible
    {
        internal int tagNo;
        internal bool explicitly = true;
        internal Asn1Encodable obj;

        internal static bool IsConstructed( bool isExplicit, Asn1Object obj )
        {
            if (!isExplicit)
            {
                switch (obj)
                {
                    case Asn1Sequence _:
                    case Asn1Set _:
                        break;
                    case Asn1TaggedObject asn1TaggedObject:
                        return IsConstructed( asn1TaggedObject.IsExplicit(), asn1TaggedObject.GetObject() );
                    default:
                        return false;
                }
            }
            return true;
        }

        public static Asn1TaggedObject GetInstance( Asn1TaggedObject obj, bool explicitly )
        {
            if (explicitly)
                return (Asn1TaggedObject)obj.GetObject();
            throw new ArgumentException( "implicitly tagged tagged object" );
        }

        public static Asn1TaggedObject GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Asn1TaggedObject _:
                    return (Asn1TaggedObject)obj;
                default:
                    throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        protected Asn1TaggedObject( int tagNo, Asn1Encodable obj )
        {
            this.explicitly = true;
            this.tagNo = tagNo;
            this.obj = obj;
        }

        protected Asn1TaggedObject( bool explicitly, int tagNo, Asn1Encodable obj )
        {
            this.explicitly = explicitly || obj is IAsn1Choice;
            this.tagNo = tagNo;
            this.obj = obj;
        }

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is Asn1TaggedObject asn1TaggedObject && this.tagNo == asn1TaggedObject.tagNo && this.explicitly == asn1TaggedObject.explicitly && Equals( this.GetObject(), asn1TaggedObject.GetObject() );

        protected override int Asn1GetHashCode()
        {
            int hashCode = this.tagNo.GetHashCode();
            if (this.obj != null)
                hashCode ^= this.obj.GetHashCode();
            return hashCode;
        }

        public int TagNo => this.tagNo;

        public bool IsExplicit() => this.explicitly;

        public bool IsEmpty() => false;

        public Asn1Object GetObject() => this.obj != null ? this.obj.ToAsn1Object() : null;

        public IAsn1Convertible GetObjectParser( int tag, bool isExplicit )
        {
            switch (tag)
            {
                case 4:
                    return Asn1OctetString.GetInstance( this, isExplicit ).Parser;
                case 16:
                    return Asn1Sequence.GetInstance( this, isExplicit ).Parser;
                case 17:
                    return Asn1Set.GetInstance( this, isExplicit ).Parser;
                default:
                    if (isExplicit)
                        return this.GetObject();
                    throw Platform.CreateNotImplementedException( "implicit tagging for tag: " + tag );
            }
        }

        public override string ToString() => "[" + tagNo + "]" + obj;
    }
}
