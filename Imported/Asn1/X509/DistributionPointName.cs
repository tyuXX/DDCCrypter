// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.DistributionPointName
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class DistributionPointName : Asn1Encodable, IAsn1Choice
    {
        public const int FullName = 0;
        public const int NameRelativeToCrlIssuer = 1;
        internal readonly Asn1Encodable name;
        internal readonly int type;

        public static DistributionPointName GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1TaggedObject.GetInstance( obj, true ) );

        public static DistributionPointName GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DistributionPointName _:
                    return (DistributionPointName)obj;
                case Asn1TaggedObject _:
                    return new DistributionPointName( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public DistributionPointName( int type, Asn1Encodable name )
        {
            this.type = type;
            this.name = name;
        }

        public DistributionPointName( GeneralNames name )
          : this( 0, name )
        {
        }

        public int PointType => this.type;

        public Asn1Encodable Name => this.name;

        public DistributionPointName( Asn1TaggedObject obj )
        {
            this.type = obj.TagNo;
            if (this.type == 0)
                this.name = GeneralNames.GetInstance( obj, false );
            else
                this.name = Asn1Set.GetInstance( obj, false );
        }

        public override Asn1Object ToAsn1Object() => new DerTaggedObject( false, this.type, this.name );

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder buf = new();
            buf.Append( "DistributionPointName: [" );
            buf.Append( newLine );
            if (this.type == 0)
                this.appendObject( buf, newLine, "fullName", this.name.ToString() );
            else
                this.appendObject( buf, newLine, "nameRelativeToCRLIssuer", this.name.ToString() );
            buf.Append( "]" );
            buf.Append( newLine );
            return buf.ToString();
        }

        private void appendObject( StringBuilder buf, string sep, string name, string val )
        {
            string str = "    ";
            buf.Append( str );
            buf.Append( name );
            buf.Append( ":" );
            buf.Append( sep );
            buf.Append( str );
            buf.Append( str );
            buf.Append( val );
            buf.Append( sep );
        }
    }
}
