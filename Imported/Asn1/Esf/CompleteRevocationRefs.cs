// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CompleteRevocationRefs
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CompleteRevocationRefs : Asn1Encodable
    {
        private readonly Asn1Sequence crlOcspRefs;

        public static CompleteRevocationRefs GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CompleteRevocationRefs _:
                    return (CompleteRevocationRefs)obj;
                case Asn1Sequence _:
                    return new CompleteRevocationRefs( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CompleteRevocationRefs' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CompleteRevocationRefs( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            foreach (Asn1Encodable asn1Encodable in seq)
                CrlOcspRef.GetInstance( asn1Encodable.ToAsn1Object() );
            this.crlOcspRefs = seq;
        }

        public CompleteRevocationRefs( params CrlOcspRef[] crlOcspRefs ) => this.crlOcspRefs = crlOcspRefs != null ? (Asn1Sequence)new DerSequence( crlOcspRefs ) : throw new ArgumentNullException( nameof( crlOcspRefs ) );

        public CompleteRevocationRefs( IEnumerable crlOcspRefs )
        {
            if (crlOcspRefs == null)
                throw new ArgumentNullException( nameof( crlOcspRefs ) );
            this.crlOcspRefs = CollectionUtilities.CheckElementsAreOfType( crlOcspRefs, typeof( CrlOcspRef ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( crlOcspRefs ) ) : throw new ArgumentException( "Must contain only 'CrlOcspRef' objects", nameof( crlOcspRefs ) );
        }

        public CrlOcspRef[] GetCrlOcspRefs()
        {
            CrlOcspRef[] crlOcspRefs = new CrlOcspRef[this.crlOcspRefs.Count];
            for (int index = 0; index < this.crlOcspRefs.Count; ++index)
                crlOcspRefs[index] = CrlOcspRef.GetInstance( this.crlOcspRefs[index].ToAsn1Object() );
            return crlOcspRefs;
        }

        public override Asn1Object ToAsn1Object() => crlOcspRefs;
    }
}
