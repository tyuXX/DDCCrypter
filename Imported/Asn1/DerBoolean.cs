// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerBoolean
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
    public class DerBoolean : Asn1Object
    {
        private readonly byte value;
        public static readonly DerBoolean False = new( false );
        public static readonly DerBoolean True = new( true );

        public static DerBoolean GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerBoolean _:
                    return (DerBoolean)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerBoolean GetInstance( bool value ) => !value ? False : True;

        public static DerBoolean GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerBoolean ? GetInstance( asn1Object ) : FromOctetString( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerBoolean( byte[] val ) => this.value = val.Length == 1 ? val[0] : throw new ArgumentException( "byte value should have 1 byte in it", nameof( val ) );

        private DerBoolean( bool value ) => this.value = value ? byte.MaxValue : (byte)0;

        public bool IsTrue => this.value != 0;

        internal override void Encode( DerOutputStream derOut ) => derOut.WriteEncoded( 1, new byte[1]
        {
      this.value
        } );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerBoolean derBoolean && this.IsTrue == derBoolean.IsTrue;

        protected override int Asn1GetHashCode() => this.IsTrue.GetHashCode();

        public override string ToString() => !this.IsTrue ? "FALSE" : "TRUE";

        internal static DerBoolean FromOctetString( byte[] value )
        {
            if (value.Length != 1)
                throw new ArgumentException( "BOOLEAN value should have 1 byte in it", nameof( value ) );
            switch (value[0])
            {
                case 0:
                    return False;
                case byte.MaxValue:
                    return True;
                default:
                    return new DerBoolean( value );
            }
        }
    }
}
