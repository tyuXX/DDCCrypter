// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.Challenge
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class Challenge : Asn1Encodable
    {
        private readonly AlgorithmIdentifier owf;
        private readonly Asn1OctetString witness;
        private readonly Asn1OctetString challenge;

        private Challenge( Asn1Sequence seq )
        {
            int num = 0;
            if (seq.Count == 3)
                this.owf = AlgorithmIdentifier.GetInstance( seq[num++] );
            Asn1Sequence asn1Sequence = seq;
            int index1 = num;
            int index2 = index1 + 1;
            this.witness = Asn1OctetString.GetInstance( asn1Sequence[index1] );
            this.challenge = Asn1OctetString.GetInstance( seq[index2] );
        }

        public static Challenge GetInstance( object obj )
        {
            switch (obj)
            {
                case Challenge _:
                    return (Challenge)obj;
                case Asn1Sequence _:
                    return new Challenge( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual AlgorithmIdentifier Owf => this.owf;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            v.AddOptional( owf );
            v.Add( witness );
            v.Add( challenge );
            return new DerSequence( v );
        }
    }
}
