// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Smime.SmimeEncryptionKeyPreferenceAttribute
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Smime
{
    public class SmimeEncryptionKeyPreferenceAttribute : AttributeX509
    {
        public SmimeEncryptionKeyPreferenceAttribute( IssuerAndSerialNumber issAndSer )
          : base( SmimeAttributes.EncrypKeyPref, new DerSet( new DerTaggedObject( false, 0, issAndSer ) ) )
        {
        }

        public SmimeEncryptionKeyPreferenceAttribute( RecipientKeyIdentifier rKeyID )
          : base( SmimeAttributes.EncrypKeyPref, new DerSet( new DerTaggedObject( false, 1, rKeyID ) ) )
        {
        }

        public SmimeEncryptionKeyPreferenceAttribute( Asn1OctetString sKeyID )
          : base( SmimeAttributes.EncrypKeyPref, new DerSet( new DerTaggedObject( false, 2, sKeyID ) ) )
        {
        }
    }
}
