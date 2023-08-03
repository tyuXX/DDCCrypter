// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.MacData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class MacData : Asn1Encodable
    {
        internal DigestInfo digInfo;
        internal byte[] salt;
        internal BigInteger iterationCount;

        public static MacData GetInstance( object obj )
        {
            switch (obj)
            {
                case MacData _:
                    return (MacData)obj;
                case Asn1Sequence _:
                    return new MacData( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private MacData( Asn1Sequence seq )
        {
            this.digInfo = DigestInfo.GetInstance( seq[0] );
            this.salt = ((Asn1OctetString)seq[1]).GetOctets();
            if (seq.Count == 3)
                this.iterationCount = ((DerInteger)seq[2]).Value;
            else
                this.iterationCount = BigInteger.One;
        }

        public MacData( DigestInfo digInfo, byte[] salt, int iterationCount )
        {
            this.digInfo = digInfo;
            this.salt = (byte[])salt.Clone();
            this.iterationCount = BigInteger.ValueOf( iterationCount );
        }

        public DigestInfo Mac => this.digInfo;

        public byte[] GetSalt() => (byte[])this.salt.Clone();

        public BigInteger IterationCount => this.iterationCount;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         digInfo,
         new DerOctetString(this.salt)
            } );
            if (!this.iterationCount.Equals( BigInteger.One ))
                v.Add( new DerInteger( this.iterationCount ) );
            return new DerSequence( v );
        }
    }
}
