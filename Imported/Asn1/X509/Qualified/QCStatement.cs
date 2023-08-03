// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.QCStatement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public class QCStatement : Asn1Encodable
    {
        private readonly DerObjectIdentifier qcStatementId;
        private readonly Asn1Encodable qcStatementInfo;

        public static QCStatement GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case QCStatement _:
                    return (QCStatement)obj;
                case Asn1Sequence _:
                    return new QCStatement( Asn1Sequence.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private QCStatement( Asn1Sequence seq )
        {
            this.qcStatementId = DerObjectIdentifier.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            this.qcStatementInfo = seq[1];
        }

        public QCStatement( DerObjectIdentifier qcStatementId ) => this.qcStatementId = qcStatementId;

        public QCStatement( DerObjectIdentifier qcStatementId, Asn1Encodable qcStatementInfo )
        {
            this.qcStatementId = qcStatementId;
            this.qcStatementInfo = qcStatementInfo;
        }

        public DerObjectIdentifier StatementId => this.qcStatementId;

        public Asn1Encodable StatementInfo => this.qcStatementInfo;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         qcStatementId
            } );
            if (this.qcStatementInfo != null)
                v.Add( this.qcStatementInfo );
            return new DerSequence( v );
        }
    }
}
