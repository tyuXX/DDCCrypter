// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.CrlDistPoint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Asn1.X509
{
    public class CrlDistPoint : Asn1Encodable
    {
        internal readonly Asn1Sequence seq;

        public static CrlDistPoint GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static CrlDistPoint GetInstance( object obj )
        {
            switch (obj)
            {
                case CrlDistPoint _:
                case null:
                    return (CrlDistPoint)obj;
                case Asn1Sequence _:
                    return new CrlDistPoint( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CrlDistPoint( Asn1Sequence seq ) => this.seq = seq;

        public CrlDistPoint( DistributionPoint[] points ) => this.seq = new DerSequence( points );

        public DistributionPoint[] GetDistributionPoints()
        {
            DistributionPoint[] distributionPoints = new DistributionPoint[this.seq.Count];
            for (int index = 0; index != this.seq.Count; ++index)
                distributionPoints[index] = DistributionPoint.GetInstance( this.seq[index] );
            return distributionPoints;
        }

        public override Asn1Object ToAsn1Object() => seq;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string newLine = Platform.NewLine;
            stringBuilder.Append( "CRLDistPoint:" );
            stringBuilder.Append( newLine );
            DistributionPoint[] distributionPoints = this.GetDistributionPoints();
            for (int index = 0; index != distributionPoints.Length; ++index)
            {
                stringBuilder.Append( "    " );
                stringBuilder.Append( distributionPoints[index] );
                stringBuilder.Append( newLine );
            }
            return stringBuilder.ToString();
        }
    }
}
