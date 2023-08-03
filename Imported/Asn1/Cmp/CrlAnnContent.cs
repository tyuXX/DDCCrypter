// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CrlAnnContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CrlAnnContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private CrlAnnContent( Asn1Sequence seq ) => this.content = seq;

        public static CrlAnnContent GetInstance( object obj )
        {
            switch (obj)
            {
                case CrlAnnContent _:
                    return (CrlAnnContent)obj;
                case Asn1Sequence _:
                    return new CrlAnnContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual CertificateList[] ToCertificateListArray()
        {
            CertificateList[] certificateListArray = new CertificateList[this.content.Count];
            for (int index = 0; index != certificateListArray.Length; ++index)
                certificateListArray[index] = CertificateList.GetInstance( this.content[index] );
            return certificateListArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
