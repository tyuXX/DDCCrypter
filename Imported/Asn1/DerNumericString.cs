// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerNumericString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerNumericString : DerStringBase
    {
        private readonly string str;

        public static DerNumericString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerNumericString _:
                    return (DerNumericString)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerNumericString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerNumericString ? GetInstance( asn1Object ) : new DerNumericString( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerNumericString( byte[] str )
          : this( Strings.FromAsciiByteArray( str ), false )
        {
        }

        public DerNumericString( string str )
          : this( str, false )
        {
        }

        public DerNumericString( string str, bool validate )
        {
            if (str == null)
                throw new ArgumentNullException( nameof( str ) );
            this.str = !validate || IsNumericString( str ) ? str : throw new ArgumentException( "string contains illegal characters", nameof( str ) );
        }

        public override string GetString() => this.str;

        public byte[] GetOctets() => Strings.ToAsciiByteArray( this.str );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 18, this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerNumericString derNumericString && this.str.Equals( derNumericString.str );

        public static bool IsNumericString( string str )
        {
            foreach (char c in str)
            {
                if (c > '\u007F' || (c != ' ' && !char.IsDigit( c )))
                    return false;
            }
            return true;
        }
    }
}
