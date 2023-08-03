// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiStatusInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiStatusInfo : Asn1Encodable
    {
        private DerInteger status;
        private PkiFreeText statusString;
        private DerBitString failInfo;

        public static PkiStatusInfo GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static PkiStatusInfo GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiStatusInfo _:
                    return (PkiStatusInfo)obj;
                case Asn1Sequence _:
                    return new PkiStatusInfo( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public PkiStatusInfo( Asn1Sequence seq )
        {
            this.status = DerInteger.GetInstance( seq[0] );
            this.statusString = null;
            this.failInfo = null;
            if (seq.Count > 2)
            {
                this.statusString = PkiFreeText.GetInstance( seq[1] );
                this.failInfo = DerBitString.GetInstance( seq[2] );
            }
            else
            {
                if (seq.Count <= 1)
                    return;
                object obj = seq[1];
                if (obj is DerBitString)
                    this.failInfo = DerBitString.GetInstance( obj );
                else
                    this.statusString = PkiFreeText.GetInstance( obj );
            }
        }

        public PkiStatusInfo( int status ) => this.status = new DerInteger( status );

        public PkiStatusInfo( int status, PkiFreeText statusString )
        {
            this.status = new DerInteger( status );
            this.statusString = statusString;
        }

        public PkiStatusInfo( int status, PkiFreeText statusString, PkiFailureInfo failInfo )
        {
            this.status = new DerInteger( status );
            this.statusString = statusString;
            this.failInfo = failInfo;
        }

        public BigInteger Status => this.status.Value;

        public PkiFreeText StatusString => this.statusString;

        public DerBitString FailInfo => this.failInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         status
            } );
            if (this.statusString != null)
                v.Add( statusString );
            if (this.failInfo != null)
                v.Add( failInfo );
            return new DerSequence( v );
        }
    }
}
