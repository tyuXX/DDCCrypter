// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1OctetString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1OctetString : Asn1Object, Asn1OctetStringParser, IAsn1Convertible
    {
        internal byte[] str;

        public static Asn1OctetString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is Asn1OctetString ? GetInstance( asn1Object ) : BerOctetString.FromSequence( Asn1Sequence.GetInstance( asn1Object ) );
        }

        public static Asn1OctetString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Asn1OctetString _:
                    return (Asn1OctetString)obj;
                case Asn1TaggedObject _:
                    return GetInstance( ((Asn1TaggedObject)obj).GetObject() );
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        internal Asn1OctetString( byte[] str ) => this.str = str != null ? str : throw new ArgumentNullException( nameof( str ) );

        internal Asn1OctetString( Asn1Encodable obj )
        {
            try
            {
                this.str = obj.GetEncoded( "DER" );
            }
            catch (IOException ex)
            {
                throw new ArgumentException( "Error processing object : " + ex.ToString() );
            }
        }

        public Stream GetOctetStream() => new MemoryStream( this.str, false );

        public Asn1OctetStringParser Parser => this;

        public virtual byte[] GetOctets() => this.str;

        protected override int Asn1GetHashCode() => Arrays.GetHashCode( this.GetOctets() );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerOctetString derOctetString && Arrays.AreEqual( this.GetOctets(), derOctetString.GetOctets() );

        public override string ToString() => "#" + Hex.ToHexString( this.str );
    }
}
