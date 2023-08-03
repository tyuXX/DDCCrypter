// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerEnumerated
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
    public class DerEnumerated : Asn1Object
    {
        private readonly byte[] bytes;
        private static readonly DerEnumerated[] cache = new DerEnumerated[12];

        public static DerEnumerated GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerEnumerated _:
                    return (DerEnumerated)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerEnumerated GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerEnumerated ? GetInstance( asn1Object ) : FromOctetString( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerEnumerated( int val ) => this.bytes = BigInteger.ValueOf( val ).ToByteArray();

        public DerEnumerated( BigInteger val ) => this.bytes = val.ToByteArray();

        public DerEnumerated( byte[] bytes ) => this.bytes = bytes;

        public BigInteger Value => new( this.bytes );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 10, this.bytes );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerEnumerated derEnumerated && Arrays.AreEqual( this.bytes, derEnumerated.bytes );

        protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.bytes );

        internal static DerEnumerated FromOctetString( byte[] enc )
        {
            if (enc.Length == 0)
                throw new ArgumentException( "ENUMERATED has zero length", nameof( enc ) );
            if (enc.Length == 1)
            {
                int index = enc[0];
                if (index < cache.Length)
                    return cache[index] ?? (cache[index] = new DerEnumerated( Arrays.Clone( enc ) ));
            }
            return new DerEnumerated( Arrays.Clone( enc ) );
        }
    }
}
