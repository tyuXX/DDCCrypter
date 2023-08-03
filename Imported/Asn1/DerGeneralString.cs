// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerGeneralString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerGeneralString : DerStringBase
    {
        private readonly string str;

        public static DerGeneralString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerGeneralString _:
                    return (DerGeneralString)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerGeneralString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerGeneralString ? GetInstance( asn1Object ) : new DerGeneralString( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerGeneralString( byte[] str )
          : this( Strings.FromAsciiByteArray( str ) )
        {
        }

        public DerGeneralString( string str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        public override string GetString() => this.str;

        public byte[] GetOctets() => Strings.ToAsciiByteArray( this.str );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 27, this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerGeneralString derGeneralString && this.str.Equals( derGeneralString.str );
    }
}
