// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Esf.CrlListID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Esf
{
    public class CrlListID : Asn1Encodable
    {
        private readonly Asn1Sequence crls;

        public static CrlListID GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case CrlListID _:
                    return (CrlListID)obj;
                case Asn1Sequence _:
                    return new CrlListID( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in 'CrlListID' factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private CrlListID( Asn1Sequence seq )
        {
            if (seq == null)
                throw new ArgumentNullException( nameof( seq ) );
            this.crls = seq.Count == 1 ? (Asn1Sequence)seq[0].ToAsn1Object() : throw new ArgumentException( "Bad sequence size: " + seq.Count, nameof( seq ) );
            foreach (Asn1Encodable crl in this.crls)
                CrlValidatedID.GetInstance( crl.ToAsn1Object() );
        }

        public CrlListID( params CrlValidatedID[] crls ) => this.crls = crls != null ? (Asn1Sequence)new DerSequence( crls ) : throw new ArgumentNullException( nameof( crls ) );

        public CrlListID( IEnumerable crls )
        {
            if (crls == null)
                throw new ArgumentNullException( nameof( crls ) );
            this.crls = CollectionUtilities.CheckElementsAreOfType( crls, typeof( CrlValidatedID ) ) ? (Asn1Sequence)new DerSequence( Asn1EncodableVector.FromEnumerable( crls ) ) : throw new ArgumentException( "Must contain only 'CrlValidatedID' objects", nameof( crls ) );
        }

        public CrlValidatedID[] GetCrls()
        {
            CrlValidatedID[] crls = new CrlValidatedID[this.crls.Count];
            for (int index = 0; index < this.crls.Count; ++index)
                crls[index] = CrlValidatedID.GetInstance( this.crls[index].ToAsn1Object() );
            return crls;
        }

        public override Asn1Object ToAsn1Object() => new DerSequence( crls );
    }
}
