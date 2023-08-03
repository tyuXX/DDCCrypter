// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerUtf8String
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
    public class DerUtf8String : DerStringBase
    {
        private readonly string str;

        public static DerUtf8String GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerUtf8String _:
                    return (DerUtf8String)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerUtf8String GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerUtf8String ? GetInstance( asn1Object ) : new DerUtf8String( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerUtf8String( byte[] str )
          : this( Encoding.UTF8.GetString( str, 0, str.Length ) )
        {
        }

        public DerUtf8String( string str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        public override string GetString() => this.str;

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerUtf8String derUtf8String && this.str.Equals( derUtf8String.str );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 12, Encoding.UTF8.GetBytes( this.str ) );
    }
}
