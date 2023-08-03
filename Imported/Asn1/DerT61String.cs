// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerT61String
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerT61String : DerStringBase
    {
        private readonly string str;

        public static DerT61String GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerT61String _:
                    return (DerT61String)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerT61String GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerT61String ? GetInstance( asn1Object ) : new DerT61String( Asn1OctetString.GetInstance( asn1Object ).GetOctets() );
        }

        public DerT61String( byte[] str )
          : this( Strings.FromByteArray( str ) )
        {
        }

        public DerT61String( string str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        public override string GetString() => this.str;

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 20, this.GetOctets() );

        public byte[] GetOctets() => Strings.ToByteArray( this.str );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerT61String derT61String && this.str.Equals( derT61String.str );
    }
}
