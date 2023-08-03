// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerGraphicString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1
{
    public class DerGraphicString : DerStringBase
    {
        private readonly byte[] mString;

        public static DerGraphicString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerGraphicString _:
                    return (DerGraphicString)obj;
                case byte[] _:
                    try
                    {
                        return (DerGraphicString)FromByteArray( (byte[])obj );
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException( "encoding error in GetInstance: " + ex.ToString(), nameof( obj ) );
                    }
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static DerGraphicString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerGraphicString ? GetInstance( asn1Object ) : new DerGraphicString( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerGraphicString( byte[] encoding ) => this.mString = Arrays.Clone( encoding );

        public override string GetString() => Strings.FromByteArray( this.mString );

        public byte[] GetOctets() => Arrays.Clone( this.mString );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 25, this.mString );

        protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.mString );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerGraphicString derGraphicString && Arrays.AreEqual( this.mString, derGraphicString.mString );
    }
}
