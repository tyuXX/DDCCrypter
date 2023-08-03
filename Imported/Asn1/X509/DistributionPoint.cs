// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.DistributionPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class DistributionPoint : Asn1Encodable
    {
        internal readonly DistributionPointName distributionPoint;
        internal readonly ReasonFlags reasons;
        internal readonly GeneralNames cRLIssuer;

        public static DistributionPoint GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static DistributionPoint GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DistributionPoint _:
                    return (DistributionPoint)obj;
                case Asn1Sequence _:
                    return new DistributionPoint( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid DistributionPoint: " + Platform.GetTypeName( obj ) );
            }
        }

        private DistributionPoint( Asn1Sequence seq )
        {
            for (int index = 0; index != seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                switch (instance.TagNo)
                {
                    case 0:
                        this.distributionPoint = DistributionPointName.GetInstance( instance, true );
                        break;
                    case 1:
                        this.reasons = new ReasonFlags( DerBitString.GetInstance( instance, false ) );
                        break;
                    case 2:
                        this.cRLIssuer = GeneralNames.GetInstance( instance, false );
                        break;
                }
            }
        }

        public DistributionPoint(
          DistributionPointName distributionPointName,
          ReasonFlags reasons,
          GeneralNames crlIssuer )
        {
            this.distributionPoint = distributionPointName;
            this.reasons = reasons;
            this.cRLIssuer = crlIssuer;
        }

        public DistributionPointName DistributionPointName => this.distributionPoint;

        public ReasonFlags Reasons => this.reasons;

        public GeneralNames CrlIssuer => this.cRLIssuer;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.distributionPoint != null)
                v.Add( new DerTaggedObject( 0, distributionPoint ) );
            if (this.reasons != null)
                v.Add( new DerTaggedObject( false, 1, reasons ) );
            if (this.cRLIssuer != null)
                v.Add( new DerTaggedObject( false, 2, cRLIssuer ) );
            return new DerSequence( v );
        }

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder buf = new();
            buf.Append( "DistributionPoint: [" );
            buf.Append( newLine );
            if (this.distributionPoint != null)
                this.appendObject( buf, newLine, "distributionPoint", this.distributionPoint.ToString() );
            if (this.reasons != null)
                this.appendObject( buf, newLine, "reasons", this.reasons.ToString() );
            if (this.cRLIssuer != null)
                this.appendObject( buf, newLine, "cRLIssuer", this.cRLIssuer.ToString() );
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
