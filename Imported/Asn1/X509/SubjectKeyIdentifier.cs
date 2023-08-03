// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.SubjectKeyIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class SubjectKeyIdentifier : Asn1Encodable
    {
        private readonly byte[] keyIdentifier;

        public static SubjectKeyIdentifier GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1OctetString.GetInstance( obj, explicitly ) );

        public static SubjectKeyIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case SubjectKeyIdentifier _:
                    return (SubjectKeyIdentifier)obj;
                case SubjectPublicKeyInfo _:
                    return new SubjectKeyIdentifier( (SubjectPublicKeyInfo)obj );
                case Asn1OctetString _:
                    return new SubjectKeyIdentifier( (Asn1OctetString)obj );
                case X509Extension _:
                    return GetInstance( X509Extension.ConvertValueToObject( (X509Extension)obj ) );
                default:
                    throw new ArgumentException( "Invalid SubjectKeyIdentifier: " + Platform.GetTypeName( obj ) );
            }
        }

        public SubjectKeyIdentifier( byte[] keyID ) => this.keyIdentifier = keyID != null ? keyID : throw new ArgumentNullException( nameof( keyID ) );

        public SubjectKeyIdentifier( Asn1OctetString keyID ) => this.keyIdentifier = keyID.GetOctets();

        public SubjectKeyIdentifier( SubjectPublicKeyInfo spki ) => this.keyIdentifier = GetDigest( spki );

        public byte[] GetKeyIdentifier() => this.keyIdentifier;

        public override Asn1Object ToAsn1Object() => new DerOctetString( this.keyIdentifier );

        public static SubjectKeyIdentifier CreateSha1KeyIdentifier( SubjectPublicKeyInfo keyInfo ) => new( keyInfo );

        public static SubjectKeyIdentifier CreateTruncatedSha1KeyIdentifier( SubjectPublicKeyInfo keyInfo )
        {
            byte[] digest = GetDigest( keyInfo );
            byte[] numArray1 = new byte[8];
            Array.Copy( digest, digest.Length - 8, numArray1, 0, numArray1.Length );
            byte[] numArray2;
            (numArray2 = numArray1)[0] = (byte)(numArray2[0] & 15U);
            byte[] numArray3;
            (numArray3 = numArray1)[0] = (byte)(numArray3[0] | 64U);
            return new SubjectKeyIdentifier( numArray1 );
        }

        private static byte[] GetDigest( SubjectPublicKeyInfo spki )
        {
            IDigest digest = new Sha1Digest();
            byte[] output = new byte[digest.GetDigestSize()];
            byte[] bytes = spki.PublicKeyData.GetBytes();
            digest.BlockUpdate( bytes, 0, bytes.Length );
            digest.DoFinal( output, 0 );
            return output;
        }
    }
}
