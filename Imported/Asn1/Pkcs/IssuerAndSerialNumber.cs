// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.IssuerAndSerialNumber
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class IssuerAndSerialNumber : Asn1Encodable
    {
        private readonly X509Name name;
        private readonly DerInteger certSerialNumber;

        public static IssuerAndSerialNumber GetInstance( object obj )
        {
            switch (obj)
            {
                case IssuerAndSerialNumber _:
                    return (IssuerAndSerialNumber)obj;
                case Asn1Sequence _:
                    return new IssuerAndSerialNumber( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private IssuerAndSerialNumber( Asn1Sequence seq )
        {
            this.name = seq.Count == 2 ? X509Name.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.certSerialNumber = DerInteger.GetInstance( seq[1] );
        }

        public IssuerAndSerialNumber( X509Name name, BigInteger certSerialNumber )
        {
            this.name = name;
            this.certSerialNumber = new DerInteger( certSerialNumber );
        }

        public IssuerAndSerialNumber( X509Name name, DerInteger certSerialNumber )
        {
            this.name = name;
            this.certSerialNumber = certSerialNumber;
        }

        public X509Name Name => this.name;

        public DerInteger CertificateSerialNumber => this.certSerialNumber;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       name,
       certSerialNumber
        } );
    }
}
