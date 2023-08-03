// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Crmf.CertReqMessages
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Crmf
{
    public class CertReqMessages : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private CertReqMessages( Asn1Sequence seq ) => this.content = seq;

        public static CertReqMessages GetInstance( object obj )
        {
            switch (obj)
            {
                case CertReqMessages _:
                    return (CertReqMessages)obj;
                case Asn1Sequence _:
                    return new CertReqMessages( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public CertReqMessages( params CertReqMsg[] msgs ) => this.content = new DerSequence( msgs );

        public virtual CertReqMsg[] ToCertReqMsgArray()
        {
            CertReqMsg[] certReqMsgArray = new CertReqMsg[this.content.Count];
            for (int index = 0; index != certReqMsgArray.Length; ++index)
                certReqMsgArray[index] = CertReqMsg.GetInstance( this.content[index] );
            return certReqMsgArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
