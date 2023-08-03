// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiMessage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiMessage : Asn1Encodable
    {
        private readonly PkiHeader header;
        private readonly PkiBody body;
        private readonly DerBitString protection;
        private readonly Asn1Sequence extraCerts;

        private PkiMessage( Asn1Sequence seq )
        {
            this.header = PkiHeader.GetInstance( seq[0] );
            this.body = PkiBody.GetInstance( seq[1] );
            for (int index = 2; index < seq.Count; ++index)
            {
                Asn1TaggedObject asn1Object = (Asn1TaggedObject)seq[index].ToAsn1Object();
                if (asn1Object.TagNo == 0)
                    this.protection = DerBitString.GetInstance( asn1Object, true );
                else
                    this.extraCerts = Asn1Sequence.GetInstance( asn1Object, true );
            }
        }

        public static PkiMessage GetInstance( object obj )
        {
            if (obj is PkiMessage)
                return (PkiMessage)obj;
            return obj != null ? new PkiMessage( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public PkiMessage(
          PkiHeader header,
          PkiBody body,
          DerBitString protection,
          CmpCertificate[] extraCerts )
        {
            this.header = header;
            this.body = body;
            this.protection = protection;
            if (extraCerts == null)
                return;
            this.extraCerts = new DerSequence( extraCerts );
        }

        public PkiMessage( PkiHeader header, PkiBody body, DerBitString protection )
          : this( header, body, protection, null )
        {
        }

        public PkiMessage( PkiHeader header, PkiBody body )
          : this( header, body, null, null )
        {
        }

        public virtual PkiHeader Header => this.header;

        public virtual PkiBody Body => this.body;

        public virtual DerBitString Protection => this.protection;

        public virtual CmpCertificate[] GetExtraCerts()
        {
            if (this.extraCerts == null)
                return null;
            CmpCertificate[] extraCerts = new CmpCertificate[this.extraCerts.Count];
            for (int index = 0; index < extraCerts.Length; ++index)
                extraCerts[index] = CmpCertificate.GetInstance( this.extraCerts[index] );
            return extraCerts;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[2]
            {
         header,
         body
            } );
            AddOptional( v, 0, protection );
            AddOptional( v, 1, extraCerts );
            return new DerSequence( v );
        }

        private static void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( true, tagNo, obj ) );
        }
    }
}
