// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.PopoSigningKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class PopoSigningKey : Asn1Encodable
    {
        private readonly PopoSigningKeyInput poposkInput;
        private readonly AlgorithmIdentifier algorithmIdentifier;
        private readonly DerBitString signature;

        private PopoSigningKey( Asn1Sequence seq )
        {
            int index1 = 0;
            if (seq[index1] is Asn1TaggedObject)
            {
                Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index1++];
                this.poposkInput = asn1TaggedObject.TagNo == 0 ? PopoSigningKeyInput.GetInstance( asn1TaggedObject.GetObject() ) : throw new ArgumentException( "Unknown PopoSigningKeyInput tag: " + asn1TaggedObject.TagNo, nameof( seq ) );
            }
            Asn1Sequence asn1Sequence = seq;
            int index2 = index1;
            int index3 = index2 + 1;
            this.algorithmIdentifier = AlgorithmIdentifier.GetInstance( asn1Sequence[index2] );
            this.signature = DerBitString.GetInstance( seq[index3] );
        }

        public static PopoSigningKey GetInstance( object obj )
        {
            switch (obj)
            {
                case PopoSigningKey _:
                    return (PopoSigningKey)obj;
                case Asn1Sequence _:
                    return new PopoSigningKey( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static PopoSigningKey GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public PopoSigningKey(
          PopoSigningKeyInput poposkIn,
          AlgorithmIdentifier aid,
          DerBitString signature )
        {
            this.poposkInput = poposkIn;
            this.algorithmIdentifier = aid;
            this.signature = signature;
        }

        public virtual PopoSigningKeyInput PoposkInput => this.poposkInput;

        public virtual AlgorithmIdentifier AlgorithmIdentifier => this.algorithmIdentifier;

        public virtual DerBitString Signature => this.signature;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.poposkInput != null)
                v.Add( new DerTaggedObject( false, 0, poposkInput ) );
            v.Add( algorithmIdentifier );
            v.Add( signature );
            return new DerSequence( v );
        }
    }
}
