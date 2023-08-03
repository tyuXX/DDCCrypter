// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.EncryptedKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cms;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class EncryptedKey : Asn1Encodable, IAsn1Choice
    {
        private readonly EnvelopedData envelopedData;
        private readonly EncryptedValue encryptedValue;

        public static EncryptedKey GetInstance( object o )
        {
            switch (o)
            {
                case EncryptedKey _:
                    return (EncryptedKey)o;
                case Asn1TaggedObject _:
                    return new EncryptedKey( EnvelopedData.GetInstance( (Asn1TaggedObject)o, false ) );
                case EncryptedValue _:
                    return new EncryptedKey( (EncryptedValue)o );
                default:
                    return new EncryptedKey( EncryptedValue.GetInstance( o ) );
            }
        }

        public EncryptedKey( EnvelopedData envelopedData ) => this.envelopedData = envelopedData;

        public EncryptedKey( EncryptedValue encryptedValue ) => this.encryptedValue = encryptedValue;

        public virtual bool IsEncryptedValue => this.encryptedValue != null;

        public virtual Asn1Encodable Value => this.encryptedValue != null ? encryptedValue : (Asn1Encodable)this.envelopedData;

        public override Asn1Object ToAsn1Object() => this.encryptedValue != null ? this.encryptedValue.ToAsn1Object() : new DerTaggedObject( false, 0, envelopedData );
    }
}
