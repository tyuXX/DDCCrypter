// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.AttCertIssuer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class AttCertIssuer : Asn1Encodable, IAsn1Choice
    {
        internal readonly Asn1Encodable obj;
        internal readonly Asn1Object choiceObj;

        public static AttCertIssuer GetInstance( object obj )
        {
            switch (obj)
            {
                case AttCertIssuer _:
                    return (AttCertIssuer)obj;
                case V2Form _:
                    return new AttCertIssuer( V2Form.GetInstance( obj ) );
                case GeneralNames _:
                    return new AttCertIssuer( (GeneralNames)obj );
                case Asn1TaggedObject _:
                    return new AttCertIssuer( V2Form.GetInstance( (Asn1TaggedObject)obj, false ) );
                case Asn1Sequence _:
                    return new AttCertIssuer( GeneralNames.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static AttCertIssuer GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( obj.GetObject() );

        public AttCertIssuer( GeneralNames names )
        {
            this.obj = names;
            this.choiceObj = this.obj.ToAsn1Object();
        }

        public AttCertIssuer( V2Form v2Form )
        {
            this.obj = v2Form;
            this.choiceObj = new DerTaggedObject( false, 0, this.obj );
        }

        public Asn1Encodable Issuer => this.obj;

        public override Asn1Object ToAsn1Object() => this.choiceObj;
    }
}
