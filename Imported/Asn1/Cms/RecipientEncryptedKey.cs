// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.RecipientEncryptedKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class RecipientEncryptedKey : Asn1Encodable
    {
        private readonly KeyAgreeRecipientIdentifier identifier;
        private readonly Asn1OctetString encryptedKey;

        private RecipientEncryptedKey( Asn1Sequence seq )
        {
            this.identifier = KeyAgreeRecipientIdentifier.GetInstance( seq[0] );
            this.encryptedKey = (Asn1OctetString)seq[1];
        }

        public static RecipientEncryptedKey GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static RecipientEncryptedKey GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RecipientEncryptedKey _:
                    return (RecipientEncryptedKey)obj;
                case Asn1Sequence _:
                    return new RecipientEncryptedKey( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid RecipientEncryptedKey: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public RecipientEncryptedKey( KeyAgreeRecipientIdentifier id, Asn1OctetString encryptedKey )
        {
            this.identifier = id;
            this.encryptedKey = encryptedKey;
        }

        public KeyAgreeRecipientIdentifier Identifier => this.identifier;

        public Asn1OctetString EncryptedKey => this.encryptedKey;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       identifier,
       encryptedKey
        } );
    }
}
