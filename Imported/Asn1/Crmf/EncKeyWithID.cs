// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.EncKeyWithID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class EncKeyWithID : Asn1Encodable
    {
        private readonly PrivateKeyInfo privKeyInfo;
        private readonly Asn1Encodable identifier;

        public static EncKeyWithID GetInstance( object obj )
        {
            if (obj is EncKeyWithID)
                return (EncKeyWithID)obj;
            return obj != null ? new EncKeyWithID( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        private EncKeyWithID( Asn1Sequence seq )
        {
            this.privKeyInfo = PrivateKeyInfo.GetInstance( seq[0] );
            if (seq.Count > 1)
            {
                if (!(seq[1] is DerUtf8String))
                    this.identifier = GeneralName.GetInstance( seq[1] );
                else
                    this.identifier = seq[1];
            }
            else
                this.identifier = null;
        }

        public EncKeyWithID( PrivateKeyInfo privKeyInfo )
        {
            this.privKeyInfo = privKeyInfo;
            this.identifier = null;
        }

        public EncKeyWithID( PrivateKeyInfo privKeyInfo, DerUtf8String str )
        {
            this.privKeyInfo = privKeyInfo;
            this.identifier = str;
        }

        public EncKeyWithID( PrivateKeyInfo privKeyInfo, GeneralName generalName )
        {
            this.privKeyInfo = privKeyInfo;
            this.identifier = generalName;
        }

        public virtual PrivateKeyInfo PrivateKey => this.privKeyInfo;

        public virtual bool HasIdentifier => this.identifier != null;

        public virtual bool IsIdentifierUtf8String => this.identifier is DerUtf8String;

        public virtual Asn1Encodable Identifier => this.identifier;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         privKeyInfo
            } );
            v.AddOptional( this.identifier );
            return new DerSequence( v );
        }
    }
}
