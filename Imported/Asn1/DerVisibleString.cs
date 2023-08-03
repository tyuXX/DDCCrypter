// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerVisibleString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerVisibleString : DerStringBase
    {
        private readonly string str;

        public static DerVisibleString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerVisibleString _:
                    return (DerVisibleString)obj;
                case Asn1OctetString _:
                    return new DerVisibleString( ((Asn1OctetString)obj).GetOctets() );
                case Asn1TaggedObject _:
                    return GetInstance( ((Asn1TaggedObject)obj).GetObject() );
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerVisibleString GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( obj.GetObject() );

        public DerVisibleString( byte[] str )
          : this( Strings.FromAsciiByteArray( str ) )
        {
        }

        public DerVisibleString( string str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        public override string GetString() => this.str;

        public byte[] GetOctets() => Strings.ToAsciiByteArray( this.str );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 26, this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerVisibleString derVisibleString && this.str.Equals( derVisibleString.str );

        protected override int Asn1GetHashCode() => this.str.GetHashCode();
    }
}
