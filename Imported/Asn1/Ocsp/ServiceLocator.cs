// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.ServiceLocator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class ServiceLocator : Asn1Encodable
    {
        private readonly X509Name issuer;
        private readonly Asn1Object locator;

        public static ServiceLocator GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static ServiceLocator GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case ServiceLocator _:
                    return (ServiceLocator)obj;
                case Asn1Sequence _:
                    return new ServiceLocator( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public ServiceLocator( X509Name issuer )
          : this( issuer, null )
        {
        }

        public ServiceLocator( X509Name issuer, Asn1Object locator )
        {
            this.issuer = issuer != null ? issuer : throw new ArgumentNullException( nameof( issuer ) );
            this.locator = locator;
        }

        private ServiceLocator( Asn1Sequence seq )
        {
            this.issuer = X509Name.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            this.locator = seq[1].ToAsn1Object();
        }

        public X509Name Issuer => this.issuer;

        public Asn1Object Locator => this.locator;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         issuer
            } );
            if (this.locator != null)
                v.Add( locator );
            return new DerSequence( v );
        }
    }
}
