// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.RsaPublicKeyStructure
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class RsaPublicKeyStructure : Asn1Encodable
    {
        private BigInteger modulus;
        private BigInteger publicExponent;

        public static RsaPublicKeyStructure GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static RsaPublicKeyStructure GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RsaPublicKeyStructure _:
                    return (RsaPublicKeyStructure)obj;
                case Asn1Sequence _:
                    return new RsaPublicKeyStructure( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid RsaPublicKeyStructure: " + Platform.GetTypeName( obj ) );
            }
        }

        public RsaPublicKeyStructure( BigInteger modulus, BigInteger publicExponent )
        {
            if (modulus == null)
                throw new ArgumentNullException( nameof( modulus ) );
            if (publicExponent == null)
                throw new ArgumentNullException( nameof( publicExponent ) );
            if (modulus.SignValue <= 0)
                throw new ArgumentException( "Not a valid RSA modulus", nameof( modulus ) );
            if (publicExponent.SignValue <= 0)
                throw new ArgumentException( "Not a valid RSA public exponent", nameof( publicExponent ) );
            this.modulus = modulus;
            this.publicExponent = publicExponent;
        }

        private RsaPublicKeyStructure( Asn1Sequence seq )
        {
            this.modulus = seq.Count == 2 ? DerInteger.GetInstance( seq[0] ).PositiveValue : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.publicExponent = DerInteger.GetInstance( seq[1] ).PositiveValue;
        }

        public BigInteger Modulus => this.modulus;

        public BigInteger PublicExponent => this.publicExponent;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       new DerInteger(this.Modulus),
       new DerInteger(this.PublicExponent)
        } );
    }
}
