// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.PopoSigningKeyInput
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class PopoSigningKeyInput : Asn1Encodable
    {
        private readonly GeneralName sender;
        private readonly PKMacValue publicKeyMac;
        private readonly SubjectPublicKeyInfo publicKey;

        private PopoSigningKeyInput( Asn1Sequence seq )
        {
            Asn1Encodable asn1Encodable = seq[0];
            if (asn1Encodable is Asn1TaggedObject)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
                this.sender = asn1TaggedObject.TagNo == 0 ? GeneralName.GetInstance( asn1TaggedObject.GetObject() ) : throw new ArgumentException( "Unknown authInfo tag: " + asn1TaggedObject.TagNo, nameof( seq ) );
            }
            else
                this.publicKeyMac = PKMacValue.GetInstance( asn1Encodable );
            this.publicKey = SubjectPublicKeyInfo.GetInstance( seq[1] );
        }

        public static PopoSigningKeyInput GetInstance( object obj )
        {
            switch (obj)
            {
                case PopoSigningKeyInput _:
                    return (PopoSigningKeyInput)obj;
                case Asn1Sequence _:
                    return new PopoSigningKeyInput( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public PopoSigningKeyInput( GeneralName sender, SubjectPublicKeyInfo spki )
        {
            this.sender = sender;
            this.publicKey = spki;
        }

        public PopoSigningKeyInput( PKMacValue pkmac, SubjectPublicKeyInfo spki )
        {
            this.publicKeyMac = pkmac;
            this.publicKey = spki;
        }

        public virtual GeneralName Sender => this.sender;

        public virtual PKMacValue PublicKeyMac => this.publicKeyMac;

        public virtual SubjectPublicKeyInfo PublicKey => this.publicKey;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.sender != null)
                v.Add( new DerTaggedObject( false, 0, sender ) );
            else
                v.Add( publicKeyMac );
            v.Add( publicKey );
            return new DerSequence( v );
        }
    }
}
