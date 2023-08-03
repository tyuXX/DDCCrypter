// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CrlIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CrlIdentifier : Asn1Encodable
    {
        private readonly X509Name crlIssuer;
        private readonly DerUtcTime crlIssuedTime;
        private readonly DerInteger crlNumber;

        public static CrlIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CrlIdentifier _:
                    return (CrlIdentifier)obj;
                case Asn1Sequence _:
                    return new CrlIdentifier( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CrlIdentifier' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CrlIdentifier( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.crlIssuer = seq.Count >= 2 && seq.Count <= 3 ? X509Name.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.crlIssuedTime = DerUtcTime.GetInstance( seq[1] );
            if (seq.Count <= 2)
                return;
            this.crlNumber = DerInteger.GetInstance( seq[2] );
        }

        public CrlIdentifier( X509Name crlIssuer, DateTime crlIssuedTime )
          : this( crlIssuer, crlIssuedTime, null )
        {
        }

        public CrlIdentifier( X509Name crlIssuer, DateTime crlIssuedTime, BigInteger crlNumber )
        {
            this.crlIssuer = crlIssuer != null ? crlIssuer : throw new ArgumentNullException( nameof( crlIssuer ) );
            this.crlIssuedTime = new DerUtcTime( crlIssuedTime );
            if (crlNumber == null)
                return;
            this.crlNumber = new DerInteger( crlNumber );
        }

        public X509Name CrlIssuer => this.crlIssuer;

        public DateTime CrlIssuedTime => this.crlIssuedTime.ToAdjustedDateTime();

        public BigInteger CrlNumber => this.crlNumber != null ? this.crlNumber.Value : null;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         this.crlIssuer.ToAsn1Object(),
         crlIssuedTime
            } );
            if (this.crlNumber != null)
                v.Add( crlNumber );
            return new DerSequence( v );
        }
    }
}
