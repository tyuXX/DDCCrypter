// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.RevDetails
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class RevDetails : Asn1Encodable
    {
        private readonly CertTemplate certDetails;
        private readonly X509Extensions crlEntryDetails;

        private RevDetails( Asn1Sequence seq )
        {
            this.certDetails = CertTemplate.GetInstance( seq[0] );
            this.crlEntryDetails = seq.Count <= 1 ? null : X509Extensions.GetInstance( seq[1] );
        }

        public static RevDetails GetInstance( object obj )
        {
            switch (obj)
            {
                case RevDetails _:
                    return (RevDetails)obj;
                case Asn1Sequence _:
                    return new RevDetails( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public RevDetails( CertTemplate certDetails )
          : this( certDetails, null )
        {
        }

        public RevDetails( CertTemplate certDetails, X509Extensions crlEntryDetails )
        {
            this.certDetails = certDetails;
            this.crlEntryDetails = crlEntryDetails;
        }

        public virtual CertTemplate CertDetails => this.certDetails;

        public virtual X509Extensions CrlEntryDetails => this.crlEntryDetails;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         certDetails
            } );
            v.AddOptional( crlEntryDetails );
            return new DerSequence( v );
        }
    }
}
