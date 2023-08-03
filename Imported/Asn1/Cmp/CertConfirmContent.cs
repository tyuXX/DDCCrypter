// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CertConfirmContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CertConfirmContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private CertConfirmContent( Asn1Sequence seq ) => this.content = seq;

        public static CertConfirmContent GetInstance( object obj )
        {
            switch (obj)
            {
                case CertConfirmContent _:
                    return (CertConfirmContent)obj;
                case Asn1Sequence _:
                    return new CertConfirmContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual CertStatus[] ToCertStatusArray()
        {
            CertStatus[] certStatusArray = new CertStatus[this.content.Count];
            for (int index = 0; index != certStatusArray.Length; ++index)
                certStatusArray[index] = CertStatus.GetInstance( this.content[index] );
            return certStatusArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
