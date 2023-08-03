// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.AdditionalInformationSyntax
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class AdditionalInformationSyntax : Asn1Encodable
    {
        private readonly DirectoryString information;

        public static AdditionalInformationSyntax GetInstance( object obj )
        {
            switch (obj)
            {
                case AdditionalInformationSyntax _:
                    return (AdditionalInformationSyntax)obj;
                case IAsn1String _:
                    return new AdditionalInformationSyntax( DirectoryString.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private AdditionalInformationSyntax( DirectoryString information ) => this.information = information;

        public AdditionalInformationSyntax( string information ) => this.information = new DirectoryString( information );

        public virtual DirectoryString Information => this.information;

        public override Asn1Object ToAsn1Object() => this.information.ToAsn1Object();
    }
}
