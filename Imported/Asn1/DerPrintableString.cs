// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerPrintableString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerPrintableString : DerStringBase
    {
        private readonly string str;

        public static DerPrintableString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerPrintableString _:
                    return (DerPrintableString)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerPrintableString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerPrintableString ? GetInstance( asn1Object ) : new DerPrintableString( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerPrintableString( byte[] str )
          : this( Strings.FromAsciiByteArray( str ), false )
        {
        }

        public DerPrintableString( string str )
          : this( str, false )
        {
        }

        public DerPrintableString( string str, bool validate )
        {
            if (str == null)
                throw new ArgumentNullException( nameof( str ) );
            this.str = !validate || IsPrintableString( str ) ? str : throw new ArgumentException( "string contains illegal characters", nameof( str ) );
        }

        public override string GetString() => this.str;

        public byte[] GetOctets() => Strings.ToAsciiByteArray( this.str );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 19, this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerPrintableString derPrintableString && this.str.Equals( derPrintableString.str );

        public static bool IsPrintableString( string str )
        {
            foreach (char c in str)
            {
                if (c > '\u007F')
                    return false;
                if (!char.IsLetterOrDigit( c ))
                {
                    switch (c)
                    {
                        case ' ':
                        case '\'':
                        case '(':
                        case ')':
                        case '+':
                        case ',':
                        case '-':
                        case '.':
                        case '/':
                        case ':':
                        case '=':
                        case '?':
                            continue;
                        default:
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
