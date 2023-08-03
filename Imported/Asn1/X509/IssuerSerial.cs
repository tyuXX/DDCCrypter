// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.IssuerSerial
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class IssuerSerial : Asn1Encodable
    {
        internal readonly GeneralNames issuer;
        internal readonly DerInteger serial;
        internal readonly DerBitString issuerUid;

        public static IssuerSerial GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case IssuerSerial _:
                    return (IssuerSerial)obj;
                case Asn1Sequence _:
                    return new IssuerSerial( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static IssuerSerial GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        private IssuerSerial( Asn1Sequence seq )
        {
            this.issuer = seq.Count == 2 || seq.Count == 3 ? GeneralNames.GetInstance( seq[0] ) : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            this.serial = DerInteger.GetInstance( seq[1] );
            if (seq.Count != 3)
                return;
            this.issuerUid = DerBitString.GetInstance( seq[2] );
        }

        public IssuerSerial( GeneralNames issuer, DerInteger serial )
        {
            this.issuer = issuer;
            this.serial = serial;
        }

        public GeneralNames Issuer => this.issuer;

        public DerInteger Serial => this.serial;

        public DerBitString IssuerUid => this.issuerUid;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[2]
            {
         issuer,
         serial
            } );
            if (this.issuerUid != null)
                v.Add( issuerUid );
            return new DerSequence( v );
        }
    }
}
