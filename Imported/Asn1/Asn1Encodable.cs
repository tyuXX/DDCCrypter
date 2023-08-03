// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1Encodable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1Encodable : IAsn1Convertible
    {
        public const string Der = "DER";
        public const string Ber = "BER";

        public byte[] GetEncoded()
        {
            MemoryStream os = new MemoryStream();
            new Asn1OutputStream( os ).WriteObject( this );
            return os.ToArray();
        }

        public byte[] GetEncoded( string encoding )
        {
            if (!encoding.Equals( "DER" ))
                return this.GetEncoded();
            MemoryStream os = new MemoryStream();
            new DerOutputStream( os ).WriteObject( this );
            return os.ToArray();
        }

        public byte[] GetDerEncoded()
        {
            try
            {
                return this.GetEncoded( "DER" );
            }
            catch (IOException ex)
            {
                return null;
            }
        }

        public override sealed int GetHashCode() => this.ToAsn1Object().CallAsn1GetHashCode();

        public override sealed bool Equals( object obj )
        {
            if (obj == this)
                return true;
            if (!(obj is IAsn1Convertible asn1Convertible))
                return false;
            Asn1Object asn1Object1 = this.ToAsn1Object();
            Asn1Object asn1Object2 = asn1Convertible.ToAsn1Object();
            return asn1Object1 == asn1Object2 || asn1Object1.CallAsn1Equals( asn1Object2 );
        }

        public abstract Asn1Object ToAsn1Object();
    }
}
