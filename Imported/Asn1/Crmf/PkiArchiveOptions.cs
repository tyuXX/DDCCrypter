// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.PkiArchiveOptions
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class PkiArchiveOptions : Asn1Encodable, IAsn1Choice
    {
        public const int encryptedPrivKey = 0;
        public const int keyGenParameters = 1;
        public const int archiveRemGenPrivKey = 2;
        private readonly Asn1Encodable value;

        public static PkiArchiveOptions GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiArchiveOptions _:
                    return (PkiArchiveOptions)obj;
                case Asn1TaggedObject _:
                    return new PkiArchiveOptions( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private PkiArchiveOptions( Asn1TaggedObject tagged )
        {
            switch (tagged.TagNo)
            {
                case 0:
                    this.value = EncryptedKey.GetInstance( tagged.GetObject() );
                    break;
                case 1:
                    this.value = Asn1OctetString.GetInstance( tagged, false );
                    break;
                case 2:
                    this.value = DerBoolean.GetInstance( tagged, false );
                    break;
                default:
                    throw new ArgumentException( "unknown tag number: " + tagged.TagNo, nameof( tagged ) );
            }
        }

        public PkiArchiveOptions( EncryptedKey encKey ) => this.value = encKey;

        public PkiArchiveOptions( Asn1OctetString keyGenParameters ) => this.value = keyGenParameters;

        public PkiArchiveOptions( bool archiveRemGenPrivKey ) => this.value = DerBoolean.GetInstance( archiveRemGenPrivKey );

        public virtual int Type
        {
            get
            {
                if (this.value is EncryptedKey)
                    return 0;
                return this.value is Asn1OctetString ? 1 : 2;
            }
        }

        public virtual Asn1Encodable Value => this.value;

        public override Asn1Object ToAsn1Object()
        {
            if (this.value is EncryptedKey)
                return new DerTaggedObject( true, 0, this.value );
            return this.value is Asn1OctetString ? new DerTaggedObject( false, 1, this.value ) : (Asn1Object)new DerTaggedObject( false, 2, this.value );
        }
    }
}
