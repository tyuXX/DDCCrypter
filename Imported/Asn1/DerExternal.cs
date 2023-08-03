// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerExternal
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerExternal : Asn1Object
    {
        private DerObjectIdentifier directReference;
        private DerInteger indirectReference;
        private Asn1Object dataValueDescriptor;
        private int encoding;
        private Asn1Object externalContent;

        public DerExternal( Asn1EncodableVector vector )
        {
            int index = 0;
            Asn1Object objFromVector = GetObjFromVector( vector, index );
            if (objFromVector is DerObjectIdentifier)
            {
                this.directReference = (DerObjectIdentifier)objFromVector;
                ++index;
                objFromVector = GetObjFromVector( vector, index );
            }
            if (objFromVector is DerInteger)
            {
                this.indirectReference = (DerInteger)objFromVector;
                ++index;
                objFromVector = GetObjFromVector( vector, index );
            }
            if (!(objFromVector is Asn1TaggedObject))
            {
                this.dataValueDescriptor = objFromVector;
                ++index;
                objFromVector = GetObjFromVector( vector, index );
            }
            if (vector.Count != index + 1)
                throw new ArgumentException( "input vector too large", nameof( vector ) );
            Asn1TaggedObject asn1TaggedObject = objFromVector is Asn1TaggedObject ? (Asn1TaggedObject)objFromVector : throw new ArgumentException( "No tagged object found in vector. Structure doesn't seem to be of type External", nameof( vector ) );
            this.Encoding = asn1TaggedObject.TagNo;
            if (this.encoding < 0 || this.encoding > 2)
                throw new InvalidOperationException( "invalid encoding value" );
            this.externalContent = asn1TaggedObject.GetObject();
        }

        public DerExternal(
          DerObjectIdentifier directReference,
          DerInteger indirectReference,
          Asn1Object dataValueDescriptor,
          DerTaggedObject externalData )
          : this( directReference, indirectReference, dataValueDescriptor, externalData.TagNo, externalData.ToAsn1Object() )
        {
        }

        public DerExternal(
          DerObjectIdentifier directReference,
          DerInteger indirectReference,
          Asn1Object dataValueDescriptor,
          int encoding,
          Asn1Object externalData )
        {
            this.DirectReference = directReference;
            this.IndirectReference = indirectReference;
            this.DataValueDescriptor = dataValueDescriptor;
            this.Encoding = encoding;
            this.ExternalContent = externalData.ToAsn1Object();
        }

        internal override void Encode( DerOutputStream derOut )
        {
            MemoryStream ms = new();
            WriteEncodable( ms, directReference );
            WriteEncodable( ms, indirectReference );
            WriteEncodable( ms, dataValueDescriptor );
            WriteEncodable( ms, new DerTaggedObject( 8, externalContent ) );
            derOut.WriteEncoded( 32, 8, ms.ToArray() );
        }

        protected override int Asn1GetHashCode()
        {
            int hashCode = this.externalContent.GetHashCode();
            if (this.directReference != null)
                hashCode ^= this.directReference.GetHashCode();
            if (this.indirectReference != null)
                hashCode ^= this.indirectReference.GetHashCode();
            if (this.dataValueDescriptor != null)
                hashCode ^= this.dataValueDescriptor.GetHashCode();
            return hashCode;
        }

        protected override bool Asn1Equals( Asn1Object asn1Object )
        {
            if (this == asn1Object)
                return true;
            return asn1Object is DerExternal derExternal && Equals( directReference, derExternal.directReference ) && Equals( indirectReference, derExternal.indirectReference ) && Equals( dataValueDescriptor, derExternal.dataValueDescriptor ) && this.externalContent.Equals( derExternal.externalContent );
        }

        public Asn1Object DataValueDescriptor
        {
            get => this.dataValueDescriptor;
            set => this.dataValueDescriptor = value;
        }

        public DerObjectIdentifier DirectReference
        {
            get => this.directReference;
            set => this.directReference = value;
        }

        public int Encoding
        {
            get => this.encoding;
            set => this.encoding = this.encoding >= 0 && this.encoding <= 2 ? value : throw new InvalidOperationException( "invalid encoding value: " + encoding );
        }

        public Asn1Object ExternalContent
        {
            get => this.externalContent;
            set => this.externalContent = value;
        }

        public DerInteger IndirectReference
        {
            get => this.indirectReference;
            set => this.indirectReference = value;
        }

        private static Asn1Object GetObjFromVector( Asn1EncodableVector v, int index )
        {
            if (v.Count <= index)
                throw new ArgumentException( "too few objects in input vector", nameof( v ) );
            return v[index].ToAsn1Object();
        }

        private static void WriteEncodable( MemoryStream ms, Asn1Encodable e )
        {
            if (e == null)
                return;
            byte[] derEncoded = e.GetDerEncoded();
            ms.Write( derEncoded, 0, derEncoded.Length );
        }
    }
}
