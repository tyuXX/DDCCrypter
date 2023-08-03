// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CrlValidatedID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CrlValidatedID : Asn1Encodable
    {
        private readonly OtherHash crlHash;
        private readonly CrlIdentifier crlIdentifier;

        public static CrlValidatedID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CrlValidatedID _:
                    return (CrlValidatedID)obj;
                case Asn1Sequence _:
                    return new CrlValidatedID( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CrlValidatedID' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CrlValidatedID( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.crlHash = seq.Count >= 1 && seq.Count <= 2 ? OtherHash.GetInstance( seq[0].ToAsn1Object() ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            if (seq.Count <= 1)
                return;
            this.crlIdentifier = CrlIdentifier.GetInstance( seq[1].ToAsn1Object() );
        }

        public CrlValidatedID( OtherHash crlHash )
          : this( crlHash, null )
        {
        }

        public CrlValidatedID( OtherHash crlHash, CrlIdentifier crlIdentifier )
        {
            this.crlHash = crlHash != null ? crlHash : throw new ArgumentNullException( nameof( crlHash ) );
            this.crlIdentifier = crlIdentifier;
        }

        public OtherHash CrlHash => this.crlHash;

        public CrlIdentifier CrlIdentifier => this.crlIdentifier;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         this.crlHash.ToAsn1Object()
            } );
            if (this.crlIdentifier != null)
                v.Add( this.crlIdentifier.ToAsn1Object() );
            return new DerSequence( v );
        }
    }
}
