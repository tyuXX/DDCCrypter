// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerIA5String
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerIA5String : DerStringBase
    {
        private readonly string str;

        public static DerIA5String GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerIA5String _:
                    return (DerIA5String)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerIA5String GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerIA5String ? GetInstance( asn1Object ) : new DerIA5String( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerIA5String( byte[] str )
          : this( Strings.FromAsciiByteArray( str ), false )
        {
        }

        public DerIA5String( string str )
          : this( str, false )
        {
        }

        public DerIA5String( string str, bool validate )
        {
            if (str == null)
                throw new ArgumentNullException( nameof( str ) );
            this.str = !validate || IsIA5String( str ) ? str : throw new ArgumentException( "string contains illegal characters", nameof( str ) );
        }

        public override string GetString() => this.str;

        public byte[] GetOctets() => Strings.ToAsciiByteArray( this.str );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 22, this.GetOctets() );

        protected override int Asn1GetHashCode() => this.str.GetHashCode();

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerIA5String derIa5String && this.str.Equals( derIa5String.str );

        public static bool IsIA5String( string str )
        {
            foreach (char ch in str)
            {
                if (ch > '\u007F')
                    return false;
            }
            return true;
        }
    }
}
