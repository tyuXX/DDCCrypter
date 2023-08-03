// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.EncryptedValue
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class EncryptedValue : Asn1Encodable
    {
        private readonly AlgorithmIdentifier intendedAlg;
        private readonly AlgorithmIdentifier symmAlg;
        private readonly DerBitString encSymmKey;
        private readonly AlgorithmIdentifier keyAlg;
        private readonly Asn1OctetString valueHint;
        private readonly DerBitString encValue;

        private EncryptedValue( Asn1Sequence seq )
        {
            int index;
            for (index = 0; seq[index] is Asn1TaggedObject; ++index)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index];
                switch (asn1TaggedObject.TagNo)
                {
                    case 0:
                        this.intendedAlg = AlgorithmIdentifier.GetInstance( asn1TaggedObject, false );
                        break;
                    case 1:
                        this.symmAlg = AlgorithmIdentifier.GetInstance( asn1TaggedObject, false );
                        break;
                    case 2:
                        this.encSymmKey = DerBitString.GetInstance( asn1TaggedObject, false );
                        break;
                    case 3:
                        this.keyAlg = AlgorithmIdentifier.GetInstance( asn1TaggedObject, false );
                        break;
                    case 4:
                        this.valueHint = Asn1OctetString.GetInstance( asn1TaggedObject, false );
                        break;
                }
            }
            this.encValue = DerBitString.GetInstance( seq[index] );
        }

        public static EncryptedValue GetInstance( object obj )
        {
            if (obj is EncryptedValue)
                return (EncryptedValue)obj;
            return obj != null ? new EncryptedValue( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public EncryptedValue(
          AlgorithmIdentifier intendedAlg,
          AlgorithmIdentifier symmAlg,
          DerBitString encSymmKey,
          AlgorithmIdentifier keyAlg,
          Asn1OctetString valueHint,
          DerBitString encValue )
        {
            if (encValue == null)
                throw new ArgumentNullException( nameof( encValue ) );
            this.intendedAlg = intendedAlg;
            this.symmAlg = symmAlg;
            this.encSymmKey = encSymmKey;
            this.keyAlg = keyAlg;
            this.valueHint = valueHint;
            this.encValue = encValue;
        }

        public virtual AlgorithmIdentifier IntendedAlg => this.intendedAlg;

        public virtual AlgorithmIdentifier SymmAlg => this.symmAlg;

        public virtual DerBitString EncSymmKey => this.encSymmKey;

        public virtual AlgorithmIdentifier KeyAlg => this.keyAlg;

        public virtual Asn1OctetString ValueHint => this.valueHint;

        public virtual DerBitString EncValue => this.encValue;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            this.AddOptional( v, 0, intendedAlg );
            this.AddOptional( v, 1, symmAlg );
            this.AddOptional( v, 2, encSymmKey );
            this.AddOptional( v, 3, keyAlg );
            this.AddOptional( v, 4, valueHint );
            v.Add( encValue );
            return new DerSequence( v );
        }

        private void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( false, tagNo, obj ) );
        }
    }
}
