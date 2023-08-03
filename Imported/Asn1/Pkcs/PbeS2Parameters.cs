// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.PbeS2Parameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class PbeS2Parameters : Asn1Encodable
    {
        private readonly KeyDerivationFunc func;
        private readonly EncryptionScheme scheme;

        public static PbeS2Parameters GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is PbeS2Parameters pbeS2Parameters ? pbeS2Parameters : new PbeS2Parameters( Asn1Sequence.GetInstance( obj ) );
        }

        public PbeS2Parameters( KeyDerivationFunc keyDevFunc, EncryptionScheme encScheme )
        {
            this.func = keyDevFunc;
            this.scheme = encScheme;
        }

        [Obsolete( "Use GetInstance() instead" )]
        public PbeS2Parameters( Asn1Sequence seq )
        {
            Asn1Sequence seq1 = seq.Count == 2 ? (Asn1Sequence)seq[0].ToAsn1Object() : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.func = !seq1[0].Equals( PkcsObjectIdentifiers.IdPbkdf2 ) ? new KeyDerivationFunc( seq1 ) : new KeyDerivationFunc( PkcsObjectIdentifiers.IdPbkdf2, Pbkdf2Params.GetInstance( seq1[1] ) );
            this.scheme = EncryptionScheme.GetInstance( seq[1].ToAsn1Object() );
        }

        public KeyDerivationFunc KeyDerivationFunc => this.func;

        public EncryptionScheme EncryptionScheme => this.scheme;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       func,
       scheme
        } );
    }
}
