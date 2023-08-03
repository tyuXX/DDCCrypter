// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.KeyAgreeRecipientIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class KeyAgreeRecipientIdentifier : Asn1Encodable, IAsn1Choice
    {
        private readonly IssuerAndSerialNumber issuerSerial;
        private readonly RecipientKeyIdentifier rKeyID;

        public static KeyAgreeRecipientIdentifier GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static KeyAgreeRecipientIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case KeyAgreeRecipientIdentifier _:
                    return (KeyAgreeRecipientIdentifier)obj;
                case Asn1Sequence _:
                    return new KeyAgreeRecipientIdentifier( IssuerAndSerialNumber.GetInstance( obj ) );
                case Asn1TaggedObject _:
                    if (((Asn1TaggedObject)obj).TagNo == 0)
                        return new KeyAgreeRecipientIdentifier( RecipientKeyIdentifier.GetInstance( (Asn1TaggedObject)obj, false ) );
                    break;
            }
            throw new ArgumentException( "Invalid KeyAgreeRecipientIdentifier: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        public KeyAgreeRecipientIdentifier( IssuerAndSerialNumber issuerSerial ) => this.issuerSerial = issuerSerial;

        public KeyAgreeRecipientIdentifier( RecipientKeyIdentifier rKeyID ) => this.rKeyID = rKeyID;

        public IssuerAndSerialNumber IssuerAndSerialNumber => this.issuerSerial;

        public RecipientKeyIdentifier RKeyID => this.rKeyID;

        public override Asn1Object ToAsn1Object() => this.issuerSerial != null ? this.issuerSerial.ToAsn1Object() : new DerTaggedObject( false, 0, rKeyID );
    }
}
