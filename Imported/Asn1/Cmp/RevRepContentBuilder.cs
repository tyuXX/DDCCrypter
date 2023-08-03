// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.RevRepContentBuilder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class RevRepContentBuilder
    {
        private readonly Asn1EncodableVector status = new( new Asn1Encodable[0] );
        private readonly Asn1EncodableVector revCerts = new( new Asn1Encodable[0] );
        private readonly Asn1EncodableVector crls = new( new Asn1Encodable[0] );

        public virtual RevRepContentBuilder Add( PkiStatusInfo status )
        {
            this.status.Add( status );
            return this;
        }

        public virtual RevRepContentBuilder Add( PkiStatusInfo status, CertId certId )
        {
            if (this.status.Count != this.revCerts.Count)
                throw new InvalidOperationException( "status and revCerts sequence must be in common order" );
            this.status.Add( status );
            this.revCerts.Add( certId );
            return this;
        }

        public virtual RevRepContentBuilder AddCrl( CertificateList crl )
        {
            this.crls.Add( crl );
            return this;
        }

        public virtual RevRepContent Build()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] )
            {
                new DerSequence( this.status )
            };
            if (this.revCerts.Count != 0)
                v.Add( new DerTaggedObject( true, 0, new DerSequence( this.revCerts ) ) );
            if (this.crls.Count != 0)
                v.Add( new DerTaggedObject( true, 1, new DerSequence( this.crls ) ) );
            return RevRepContent.GetInstance( new DerSequence( v ) );
        }
    }
}
