// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerUniversalString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Asn1
{
    public class DerUniversalString : DerStringBase
    {
        private static readonly char[] table = new char[16]
        {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
        };
        private readonly byte[] str;

        public static DerUniversalString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerUniversalString _:
                    return (DerUniversalString)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerUniversalString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerUniversalString ? GetInstance( asn1Object ) : new DerUniversalString( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerUniversalString( byte[] str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        public override string GetString()
        {
            StringBuilder stringBuilder = new StringBuilder( "#" );
            byte[] derEncoded = this.GetDerEncoded();
            for (int index = 0; index != derEncoded.Length; ++index)
            {
                uint num = derEncoded[index];
                stringBuilder.Append( table[(int)(IntPtr)((num >> 4) & 15U)] );
                stringBuilder.Append( table[derEncoded[index] & 15] );
            }
            return stringBuilder.ToString();
        }

        public byte[] GetOctets() => (byte[])this.str.Clone();

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 28, this.str );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerUniversalString derUniversalString && Arrays.AreEqual( this.str, derUniversalString.str );
    }
}
