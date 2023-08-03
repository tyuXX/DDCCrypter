// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.Pbkdf2Params
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class Pbkdf2Params : Asn1Encodable
    {
        private static AlgorithmIdentifier algid_hmacWithSHA1 = new AlgorithmIdentifier( PkcsObjectIdentifiers.IdHmacWithSha1, DerNull.Instance );
        private readonly Asn1OctetString octStr;
        private readonly DerInteger iterationCount;
        private readonly DerInteger keyLength;
        private readonly AlgorithmIdentifier prf;

        public static Pbkdf2Params GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Pbkdf2Params _:
                    return (Pbkdf2Params)obj;
                case Asn1Sequence _:
                    return new Pbkdf2Params( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public Pbkdf2Params( Asn1Sequence seq )
        {
            this.octStr = seq.Count >= 2 && seq.Count <= 4 ? (Asn1OctetString)seq[0] : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.iterationCount = (DerInteger)seq[1];
            Asn1Encodable asn1Encodable1 = null;
            Asn1Encodable asn1Encodable2 = null;
            if (seq.Count > 3)
            {
                asn1Encodable1 = seq[2];
                asn1Encodable2 = seq[3];
            }
            else if (seq.Count > 2)
            {
                if (seq[2] is DerInteger)
                    asn1Encodable1 = seq[2];
                else
                    asn1Encodable2 = seq[2];
            }
            if (asn1Encodable1 != null)
                this.keyLength = (DerInteger)asn1Encodable1;
            if (asn1Encodable2 == null)
                return;
            this.prf = AlgorithmIdentifier.GetInstance( asn1Encodable2 );
        }

        public Pbkdf2Params( byte[] salt, int iterationCount )
        {
            this.octStr = new DerOctetString( salt );
            this.iterationCount = new DerInteger( iterationCount );
        }

        public Pbkdf2Params( byte[] salt, int iterationCount, int keyLength )
          : this( salt, iterationCount )
        {
            this.keyLength = new DerInteger( keyLength );
        }

        public Pbkdf2Params( byte[] salt, int iterationCount, int keyLength, AlgorithmIdentifier prf )
          : this( salt, iterationCount, keyLength )
        {
            this.prf = prf;
        }

        public Pbkdf2Params( byte[] salt, int iterationCount, AlgorithmIdentifier prf )
          : this( salt, iterationCount )
        {
            this.prf = prf;
        }

        public byte[] GetSalt() => this.octStr.GetOctets();

        public BigInteger IterationCount => this.iterationCount.Value;

        public BigInteger KeyLength => this.keyLength != null ? this.keyLength.Value : null;

        public bool IsDefaultPrf => this.prf == null || this.prf.Equals( algid_hmacWithSHA1 );

        public AlgorithmIdentifier Prf => this.prf == null ? algid_hmacWithSHA1 : this.prf;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         octStr,
         iterationCount
            } );
            if (this.keyLength != null)
                v.Add( keyLength );
            if (!this.IsDefaultPrf)
                v.Add( prf );
            return new DerSequence( v );
        }
    }
}
