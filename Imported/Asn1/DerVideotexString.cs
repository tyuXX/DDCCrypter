// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerVideotexString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
    public class DerVideotexString : DerStringBase
    {
        private readonly byte[] mString;

        public static DerVideotexString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerVideotexString _:
                    return (DerVideotexString)obj;
                case byte[] _:
                    try
                    {
                        return (DerVideotexString)FromByteArray( (byte[])obj );
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException( "encoding error in GetInstance: " + ex.ToString(), nameof( obj ) );
                    }
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static DerVideotexString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerVideotexString ? GetInstance( asn1Object ) : new DerVideotexString( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerVideotexString( byte[] encoding ) => this.mString = Arrays.Clone( encoding );

        public override string GetString() => Strings.FromByteArray( this.mString );

        public byte[] GetOctets() => Arrays.Clone( this.mString );

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 21, this.mString );

        protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.mString );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerVideotexString derVideotexString && Arrays.AreEqual( this.mString, derVideotexString.mString );
    }
}
