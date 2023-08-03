// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.IssuingDistributionPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Asn1.X509
{
    public class IssuingDistributionPoint : Asn1Encodable
    {
        private readonly DistributionPointName _distributionPoint;
        private readonly bool _onlyContainsUserCerts;
        private readonly bool _onlyContainsCACerts;
        private readonly ReasonFlags _onlySomeReasons;
        private readonly bool _indirectCRL;
        private readonly bool _onlyContainsAttributeCerts;
        private readonly Asn1Sequence seq;

        public static IssuingDistributionPoint GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static IssuingDistributionPoint GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case IssuingDistributionPoint _:
                    return (IssuingDistributionPoint)obj;
                case Asn1Sequence _:
                    return new IssuingDistributionPoint( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public IssuingDistributionPoint(
          DistributionPointName distributionPoint,
          bool onlyContainsUserCerts,
          bool onlyContainsCACerts,
          ReasonFlags onlySomeReasons,
          bool indirectCRL,
          bool onlyContainsAttributeCerts )
        {
            this._distributionPoint = distributionPoint;
            this._indirectCRL = indirectCRL;
            this._onlyContainsAttributeCerts = onlyContainsAttributeCerts;
            this._onlyContainsCACerts = onlyContainsCACerts;
            this._onlyContainsUserCerts = onlyContainsUserCerts;
            this._onlySomeReasons = onlySomeReasons;
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (distributionPoint != null)
                v.Add( new DerTaggedObject( true, 0, distributionPoint ) );
            if (onlyContainsUserCerts)
                v.Add( new DerTaggedObject( false, 1, DerBoolean.True ) );
            if (onlyContainsCACerts)
                v.Add( new DerTaggedObject( false, 2, DerBoolean.True ) );
            if (onlySomeReasons != null)
                v.Add( new DerTaggedObject( false, 3, onlySomeReasons ) );
            if (indirectCRL)
                v.Add( new DerTaggedObject( false, 4, DerBoolean.True ) );
            if (onlyContainsAttributeCerts)
                v.Add( new DerTaggedObject( false, 5, DerBoolean.True ) );
            this.seq = new DerSequence( v );
        }

        private IssuingDistributionPoint( Asn1Sequence seq )
        {
            this.seq = seq;
            for (int index = 0; index != seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                switch (instance.TagNo)
                {
                    case 0:
                        this._distributionPoint = DistributionPointName.GetInstance( instance, true );
                        break;
                    case 1:
                        this._onlyContainsUserCerts = DerBoolean.GetInstance( instance, false ).IsTrue;
                        break;
                    case 2:
                        this._onlyContainsCACerts = DerBoolean.GetInstance( instance, false ).IsTrue;
                        break;
                    case 3:
                        this._onlySomeReasons = new ReasonFlags( DerBitString.GetInstance( instance, false ) );
                        break;
                    case 4:
                        this._indirectCRL = DerBoolean.GetInstance( instance, false ).IsTrue;
                        break;
                    case 5:
                        this._onlyContainsAttributeCerts = DerBoolean.GetInstance( instance, false ).IsTrue;
                        break;
                    default:
                        throw new ArgumentException( "unknown tag in IssuingDistributionPoint" );
                }
            }
        }

        public bool OnlyContainsUserCerts => this._onlyContainsUserCerts;

        public bool OnlyContainsCACerts => this._onlyContainsCACerts;

        public bool IsIndirectCrl => this._indirectCRL;

        public bool OnlyContainsAttributeCerts => this._onlyContainsAttributeCerts;

        public DistributionPointName DistributionPoint => this._distributionPoint;

        public ReasonFlags OnlySomeReasons => this._onlySomeReasons;

        public override Asn1Object ToAsn1Object() => seq;

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder buf1 = new StringBuilder();
            buf1.Append( "IssuingDistributionPoint: [" );
            buf1.Append( newLine );
            if (this._distributionPoint != null)
                this.appendObject( buf1, newLine, "distributionPoint", this._distributionPoint.ToString() );
            bool flag;
            if (this._onlyContainsUserCerts)
            {
                StringBuilder buf2 = buf1;
                string sep = newLine;
                flag = this._onlyContainsUserCerts;
                string val = flag.ToString();
                this.appendObject( buf2, sep, "onlyContainsUserCerts", val );
            }
            if (this._onlyContainsCACerts)
            {
                StringBuilder buf3 = buf1;
                string sep = newLine;
                flag = this._onlyContainsCACerts;
                string val = flag.ToString();
                this.appendObject( buf3, sep, "onlyContainsCACerts", val );
            }
            if (this._onlySomeReasons != null)
                this.appendObject( buf1, newLine, "onlySomeReasons", this._onlySomeReasons.ToString() );
            if (this._onlyContainsAttributeCerts)
            {
                StringBuilder buf4 = buf1;
                string sep = newLine;
                flag = this._onlyContainsAttributeCerts;
                string val = flag.ToString();
                this.appendObject( buf4, sep, "onlyContainsAttributeCerts", val );
            }
            if (this._indirectCRL)
            {
                StringBuilder buf5 = buf1;
                string sep = newLine;
                flag = this._indirectCRL;
                string val = flag.ToString();
                this.appendObject( buf5, sep, "indirectCRL", val );
            }
            buf1.Append( "]" );
            buf1.Append( newLine );
            return buf1.ToString();
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
