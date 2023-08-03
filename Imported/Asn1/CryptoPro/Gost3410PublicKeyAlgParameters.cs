// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.CryptoPro.Gost3410PublicKeyAlgParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
    public class Gost3410PublicKeyAlgParameters : Asn1Encodable
    {
        private DerObjectIdentifier publicKeyParamSet;
        private DerObjectIdentifier digestParamSet;
        private DerObjectIdentifier encryptionParamSet;

        public static Gost3410PublicKeyAlgParameters GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static Gost3410PublicKeyAlgParameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Gost3410PublicKeyAlgParameters _:
                    return (Gost3410PublicKeyAlgParameters)obj;
                case Asn1Sequence _:
                    return new Gost3410PublicKeyAlgParameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid GOST3410Parameter: " + Platform.GetTypeName( obj ) );
            }
        }

        public Gost3410PublicKeyAlgParameters(
          DerObjectIdentifier publicKeyParamSet,
          DerObjectIdentifier digestParamSet )
          : this( publicKeyParamSet, digestParamSet, null )
        {
        }

        public Gost3410PublicKeyAlgParameters(
          DerObjectIdentifier publicKeyParamSet,
          DerObjectIdentifier digestParamSet,
          DerObjectIdentifier encryptionParamSet )
        {
            if (publicKeyParamSet == null)
                throw new ArgumentNullException( nameof( publicKeyParamSet ) );
            if (digestParamSet == null)
                throw new ArgumentNullException( nameof( digestParamSet ) );
            this.publicKeyParamSet = publicKeyParamSet;
            this.digestParamSet = digestParamSet;
            this.encryptionParamSet = encryptionParamSet;
        }

        public Gost3410PublicKeyAlgParameters( Asn1Sequence seq )
        {
            this.publicKeyParamSet = (DerObjectIdentifier)seq[0];
            this.digestParamSet = (DerObjectIdentifier)seq[1];
            if (seq.Count <= 2)
                return;
            this.encryptionParamSet = (DerObjectIdentifier)seq[2];
        }

        public DerObjectIdentifier PublicKeyParamSet => this.publicKeyParamSet;

        public DerObjectIdentifier DigestParamSet => this.digestParamSet;

        public DerObjectIdentifier EncryptionParamSet => this.encryptionParamSet;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         publicKeyParamSet,
         digestParamSet
            } );
            if (this.encryptionParamSet != null)
                v.Add( encryptionParamSet );
            return new DerSequence( v );
        }
    }
}
