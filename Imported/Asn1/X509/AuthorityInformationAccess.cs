// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AuthorityInformationAccess
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AuthorityInformationAccess : Asn1Encodable
    {
        private readonly AccessDescription[] descriptions;

        public static AuthorityInformationAccess GetInstance( object obj )
        {
            if (obj is AuthorityInformationAccess)
                return (AuthorityInformationAccess)obj;
            return obj == null ? null : new AuthorityInformationAccess( Asn1Sequence.GetInstance( obj ) );
        }

        private AuthorityInformationAccess( Asn1Sequence seq )
        {
            this.descriptions = seq.Count >= 1 ? new AccessDescription[seq.Count] : throw new ArgumentException( "sequence may not be empty" );
            for (int index = 0; index < seq.Count; ++index)
                this.descriptions[index] = AccessDescription.GetInstance( seq[index] );
        }

        public AuthorityInformationAccess( AccessDescription description ) => this.descriptions = new AccessDescription[1]
        {
      description
        };

        public AuthorityInformationAccess( DerObjectIdentifier oid, GeneralName location )
          : this( new AccessDescription( oid, location ) )
        {
        }

        public AccessDescription[] GetAccessDescriptions() => (AccessDescription[])this.descriptions.Clone();

        public override Asn1Object ToAsn1Object() => new DerSequence( descriptions );

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string newLine = Platform.NewLine;
            stringBuilder.Append( "AuthorityInformationAccess:" );
            stringBuilder.Append( newLine );
            foreach (AccessDescription description in this.descriptions)
            {
                stringBuilder.Append( "    " );
                stringBuilder.Append( description );
                stringBuilder.Append( newLine );
            }
            return stringBuilder.ToString();
        }
    }
}
