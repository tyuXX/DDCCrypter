// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.ErrorMsgContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class ErrorMsgContent : Asn1Encodable
    {
        private readonly PkiStatusInfo pkiStatusInfo;
        private readonly DerInteger errorCode;
        private readonly PkiFreeText errorDetails;

        private ErrorMsgContent( Asn1Sequence seq )
        {
            this.pkiStatusInfo = PkiStatusInfo.GetInstance( seq[0] );
            for (int index = 1; index < seq.Count; ++index)
            {
                Asn1Encodable asn1Encodable = seq[index];
                if (asn1Encodable is DerInteger)
                    this.errorCode = DerInteger.GetInstance( asn1Encodable );
                else
                    this.errorDetails = PkiFreeText.GetInstance( asn1Encodable );
            }
        }

        public static ErrorMsgContent GetInstance( object obj )
        {
            switch (obj)
            {
                case ErrorMsgContent _:
                    return (ErrorMsgContent)obj;
                case Asn1Sequence _:
                    return new ErrorMsgContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public ErrorMsgContent( PkiStatusInfo pkiStatusInfo )
          : this( pkiStatusInfo, null, null )
        {
        }

        public ErrorMsgContent(
          PkiStatusInfo pkiStatusInfo,
          DerInteger errorCode,
          PkiFreeText errorDetails )
        {
            this.pkiStatusInfo = pkiStatusInfo != null ? pkiStatusInfo : throw new ArgumentNullException( nameof( pkiStatusInfo ) );
            this.errorCode = errorCode;
            this.errorDetails = errorDetails;
        }

        public virtual PkiStatusInfo PkiStatusInfo => this.pkiStatusInfo;

        public virtual DerInteger ErrorCode => this.errorCode;

        public virtual PkiFreeText ErrorDetails => this.errorDetails;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         pkiStatusInfo
            } );
            v.AddOptional( errorCode, errorDetails );
            return new DerSequence( v );
        }
    }
}
