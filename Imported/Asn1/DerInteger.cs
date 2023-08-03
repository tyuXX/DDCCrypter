// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerInteger
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerInteger : Asn1Object
    {
        private readonly byte[] bytes;

        public static DerInteger GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerInteger _:
                    return (DerInteger)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerInteger GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj != null ? obj.GetObject() : throw new ArgumentNullException( nameof( obj ) );
            return isExplicit || asn1Object is DerInteger ? GetInstance( asn1Object ) : new DerInteger( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerInteger( int value ) => this.bytes = BigInteger.ValueOf( value ).ToByteArray();

        public DerInteger( BigInteger value ) => this.bytes = value != null ? value.ToByteArray() : throw new ArgumentNullException( nameof( value ) );

        public DerInteger( byte[] bytes ) => this.bytes = bytes;

        public BigInteger Value => new BigInteger( this.bytes );

        public BigInteger PositiveValue => new BigInteger( 1, this.bytes );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 2, this.bytes );

        protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.bytes );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerInteger derInteger && Arrays.AreEqual( this.bytes, derInteger.bytes );

        public override string ToString() => this.Value.ToString();
    }
}
