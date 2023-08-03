// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.PKMacValue
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class PKMacValue : Asn1Encodable
    {
        private readonly AlgorithmIdentifier algID;
        private readonly DerBitString macValue;

        private PKMacValue( Asn1Sequence seq )
        {
            this.algID = AlgorithmIdentifier.GetInstance( seq[0] );
            this.macValue = DerBitString.GetInstance( seq[1] );
        }

        public static PKMacValue GetInstance( object obj )
        {
            switch (obj)
            {
                case PKMacValue _:
                    return (PKMacValue)obj;
                case Asn1Sequence _:
                    return new PKMacValue( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static PKMacValue GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public PKMacValue( PbmParameter pbmParams, DerBitString macValue )
          : this( new AlgorithmIdentifier( CmpObjectIdentifiers.passwordBasedMac, pbmParams ), macValue )
        {
        }

        public PKMacValue( AlgorithmIdentifier algID, DerBitString macValue )
        {
            this.algID = algID;
            this.macValue = macValue;
        }

        public virtual AlgorithmIdentifier AlgID => this.algID;

        public virtual DerBitString MacValue => this.macValue;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       algID,
       macValue
        } );
    }
}
