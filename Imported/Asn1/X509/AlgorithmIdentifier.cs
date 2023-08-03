// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AlgorithmIdentifier : Asn1Encodable
    {
        private readonly DerObjectIdentifier algorithm;
        private readonly Asn1Encodable parameters;

        public static AlgorithmIdentifier GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static AlgorithmIdentifier GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is AlgorithmIdentifier ? (AlgorithmIdentifier)obj : new AlgorithmIdentifier( Asn1Sequence.GetInstance( obj ) );
        }

        public AlgorithmIdentifier( DerObjectIdentifier algorithm ) => this.algorithm = algorithm;

        [Obsolete( "Use version taking a DerObjectIdentifier" )]
        public AlgorithmIdentifier( string algorithm ) => this.algorithm = new DerObjectIdentifier( algorithm );

        public AlgorithmIdentifier( DerObjectIdentifier algorithm, Asn1Encodable parameters )
        {
            this.algorithm = algorithm;
            this.parameters = parameters;
        }

        internal AlgorithmIdentifier( Asn1Sequence seq )
        {
            this.algorithm = seq.Count >= 1 && seq.Count <= 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.parameters = seq.Count < 2 ? null : seq[1];
        }

        public virtual DerObjectIdentifier Algorithm => this.algorithm;

        [Obsolete( "Use 'Algorithm' property instead" )]
        public virtual DerObjectIdentifier ObjectID => this.algorithm;

        public virtual Asn1Encodable Parameters => this.parameters;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         algorithm
            } );
            v.AddOptional( this.parameters );
            return new DerSequence( v );
        }
    }
}
