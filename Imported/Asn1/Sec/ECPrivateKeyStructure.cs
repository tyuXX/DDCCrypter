// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Sec.ECPrivateKeyStructure
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Sec
{
    public class ECPrivateKeyStructure : Asn1Encodable
    {
        private readonly Asn1Sequence seq;

        public static ECPrivateKeyStructure GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is ECPrivateKeyStructure ? (ECPrivateKeyStructure)obj : new ECPrivateKeyStructure( Asn1Sequence.GetInstance( obj ) );
        }

        [Obsolete( "Use 'GetInstance' instead" )]
        public ECPrivateKeyStructure( Asn1Sequence seq ) => this.seq = seq != null ? seq : throw new ArgumentNullException( nameof( seq ) );

        [Obsolete( "Use constructor which takes 'orderBitLength' instead, to guarantee correct encoding" )]
        public ECPrivateKeyStructure( BigInteger key ) => this.seq = key != null ? (Asn1Sequence)new DerSequence( new Asn1Encodable[2]
        {
       new DerInteger(1),
       new DerOctetString(key.ToByteArrayUnsigned())
        } ) : throw new ArgumentNullException( nameof( key ) );

        public ECPrivateKeyStructure( int orderBitLength, BigInteger key )
        {
            if (key == null)
                throw new ArgumentNullException( nameof( key ) );
            if (orderBitLength < key.BitLength)
                throw new ArgumentException( "must be >= key bitlength", nameof( orderBitLength ) );
            this.seq = new DerSequence( new Asn1Encodable[2]
            {
         new DerInteger(1),
         new DerOctetString(BigIntegers.AsUnsignedByteArray((orderBitLength + 7) / 8, key))
            } );
        }

        [Obsolete( "Use constructor which takes 'orderBitLength' instead, to guarantee correct encoding" )]
        public ECPrivateKeyStructure( BigInteger key, Asn1Encodable parameters )
          : this( key, null, parameters )
        {
        }

        [Obsolete( "Use constructor which takes 'orderBitLength' instead, to guarantee correct encoding" )]
        public ECPrivateKeyStructure( BigInteger key, DerBitString publicKey, Asn1Encodable parameters )
        {
            Asn1EncodableVector v = key != null ? new Asn1EncodableVector( new Asn1Encodable[2]
            {
         new DerInteger(1),
         new DerOctetString(key.ToByteArrayUnsigned())
            } ) : throw new ArgumentNullException( nameof( key ) );
            if (parameters != null)
                v.Add( new DerTaggedObject( true, 0, parameters ) );
            if (publicKey != null)
                v.Add( new DerTaggedObject( true, 1, publicKey ) );
            this.seq = new DerSequence( v );
        }

        public ECPrivateKeyStructure( int orderBitLength, BigInteger key, Asn1Encodable parameters )
          : this( orderBitLength, key, null, parameters )
        {
        }

        public ECPrivateKeyStructure(
          int orderBitLength,
          BigInteger key,
          DerBitString publicKey,
          Asn1Encodable parameters )
        {
            if (key == null)
                throw new ArgumentNullException( nameof( key ) );
            if (orderBitLength < key.BitLength)
                throw new ArgumentException( "must be >= key bitlength", nameof( orderBitLength ) );
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         new DerInteger(1),
         new DerOctetString(BigIntegers.AsUnsignedByteArray((orderBitLength + 7) / 8, key))
            } );
            if (parameters != null)
                v.Add( new DerTaggedObject( true, 0, parameters ) );
            if (publicKey != null)
                v.Add( new DerTaggedObject( true, 1, publicKey ) );
            this.seq = new DerSequence( v );
        }

        public virtual BigInteger GetKey() => new BigInteger( 1, ((Asn1OctetString)this.seq[1]).GetOctets() );

        public virtual DerBitString GetPublicKey() => (DerBitString)this.GetObjectInTag( 1 );

        public virtual Asn1Object GetParameters() => this.GetObjectInTag( 0 );

        private Asn1Object GetObjectInTag( int tagNo )
        {
            foreach (Asn1Encodable asn1Encodable in this.seq)
            {
                Asn1Object asn1Object = asn1Encodable.ToAsn1Object();
                if (asn1Object is Asn1TaggedObject)
                {
                    Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Object;
                    if (asn1TaggedObject.TagNo == tagNo)
                        return asn1TaggedObject.GetObject();
                }
            }
            return null;
        }

        public override Asn1Object ToAsn1Object() => seq;
    }
}
