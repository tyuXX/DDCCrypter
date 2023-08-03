// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.DHDomainParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X9
{
    public class DHDomainParameters : Asn1Encodable
    {
        private readonly DerInteger p;
        private readonly DerInteger g;
        private readonly DerInteger q;
        private readonly DerInteger j;
        private readonly DHValidationParms validationParms;

        public static DHDomainParameters GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static DHDomainParameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DHDomainParameters _:
                    return (DHDomainParameters)obj;
                case Asn1Sequence _:
                    return new DHDomainParameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid DHDomainParameters: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public DHDomainParameters(
          DerInteger p,
          DerInteger g,
          DerInteger q,
          DerInteger j,
          DHValidationParms validationParms )
        {
            if (p == null)
                throw new ArgumentNullException( nameof( p ) );
            if (g == null)
                throw new ArgumentNullException( nameof( g ) );
            if (q == null)
                throw new ArgumentNullException( nameof( q ) );
            this.p = p;
            this.g = g;
            this.q = q;
            this.j = j;
            this.validationParms = validationParms;
        }

        private DHDomainParameters( Asn1Sequence seq )
        {
            IEnumerator e = seq.Count >= 3 && seq.Count <= 5 ? seq.GetEnumerator() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            this.p = DerInteger.GetInstance( GetNext( e ) );
            this.g = DerInteger.GetInstance( GetNext( e ) );
            this.q = DerInteger.GetInstance( GetNext( e ) );
            Asn1Encodable next = GetNext( e );
            if (next != null && next is DerInteger)
            {
                this.j = DerInteger.GetInstance( next );
                next = GetNext( e );
            }
            if (next == null)
                return;
            this.validationParms = DHValidationParms.GetInstance( next.ToAsn1Object() );
        }

        private static Asn1Encodable GetNext( IEnumerator e ) => !e.MoveNext() ? null : (Asn1Encodable)e.Current;

        public DerInteger P => this.p;

        public DerInteger G => this.g;

        public DerInteger Q => this.q;

        public DerInteger J => this.j;

        public DHValidationParms ValidationParms => this.validationParms;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[3]
            {
         p,
         g,
         q
            } );
            if (this.j != null)
                v.Add( j );
            if (this.validationParms != null)
                v.Add( validationParms );
            return new DerSequence( v );
        }
    }
}
