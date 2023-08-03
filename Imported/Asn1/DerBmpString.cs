// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerBmpString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerBmpString : DerStringBase
    {
        private readonly string str;

        public static DerBmpString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerBmpString _:
                    return (DerBmpString)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerBmpString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerBmpString ? GetInstance( asn1Object ) : new DerBmpString( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerBmpString( byte[] str )
        {
            char[] chArray = str != null ? new char[str.Length / 2] : throw new ArgumentNullException( nameof( str ) );
            for (int index = 0; index != chArray.Length; ++index)
                chArray[index] = (char)((str[2 * index] << 8) | (str[(2 * index) + 1] & byte.MaxValue));
            this.str = new string( chArray );
        }

        public DerBmpString( string str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        public override string GetString() => this.str;

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerBmpString derBmpString && this.str.Equals( derBmpString.str );

        internal override void Encode( DerOutputStream derOut )
        {
            char[] charArray = this.str.ToCharArray();
            byte[] bytes = new byte[charArray.Length * 2];
            for (int index = 0; index != charArray.Length; ++index)
            {
                bytes[2 * index] = (byte)((uint)charArray[index] >> 8);
                bytes[(2 * index) + 1] = (byte)charArray[index];
            }
            derOut.WriteEncoded( 30, bytes );
        }
    }
}
