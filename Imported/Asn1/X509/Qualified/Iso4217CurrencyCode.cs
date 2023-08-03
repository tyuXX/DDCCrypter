// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.Iso4217CurrencyCode
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public class Iso4217CurrencyCode : Asn1Encodable, IAsn1Choice
    {
        internal const int AlphabeticMaxSize = 3;
        internal const int NumericMinSize = 1;
        internal const int NumericMaxSize = 999;
        internal Asn1Encodable obj;

        public static Iso4217CurrencyCode GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Iso4217CurrencyCode _:
                    return (Iso4217CurrencyCode)obj;
                case DerInteger _:
                    return new Iso4217CurrencyCode( DerInteger.GetInstance( obj ).Value.IntValue );
                case DerPrintableString _:
                    return new Iso4217CurrencyCode( DerPrintableString.GetInstance( obj ).GetString() );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Iso4217CurrencyCode( int numeric ) => this.obj = numeric <= 999 && numeric >= 1 ? (Asn1Encodable)new DerInteger( numeric ) : throw new ArgumentException( "wrong size in numeric code : not in (" + 1 + ".." + 999 + ")" );

        public Iso4217CurrencyCode( string alphabetic ) => this.obj = alphabetic.Length <= 3 ? (Asn1Encodable)new DerPrintableString( alphabetic ) : throw new ArgumentException( "wrong size in alphabetic code : max size is " + 3 );

        public bool IsAlphabetic => this.obj is DerPrintableString;

        public string Alphabetic => ((DerStringBase)this.obj).GetString();

        public int Numeric => ((DerInteger)this.obj).Value.IntValue;

        public override Asn1Object ToAsn1Object() => this.obj.ToAsn1Object();
    }
}
