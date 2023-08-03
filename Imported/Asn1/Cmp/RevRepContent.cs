// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.RevRepContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class RevRepContent : Asn1Encodable
    {
        private readonly Asn1Sequence status;
        private readonly Asn1Sequence revCerts;
        private readonly Asn1Sequence crls;

        private RevRepContent( Asn1Sequence seq )
        {
            this.status = Asn1Sequence.GetInstance( seq[0] );
            for (int index = 1; index < seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                if (instance.TagNo == 0)
                    this.revCerts = Asn1Sequence.GetInstance( instance, true );
                else
                    this.crls = Asn1Sequence.GetInstance( instance, true );
            }
        }

        public static RevRepContent GetInstance( object obj )
        {
            switch (obj)
            {
                case RevRepContent _:
                    return (RevRepContent)obj;
                case Asn1Sequence _:
                    return new RevRepContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual PkiStatusInfo[] GetStatus()
        {
            PkiStatusInfo[] status = new PkiStatusInfo[this.status.Count];
            for (int index = 0; index != status.Length; ++index)
                status[index] = PkiStatusInfo.GetInstance( this.status[index] );
            return status;
        }

        public virtual CertId[] GetRevCerts()
        {
            if (this.revCerts == null)
                return null;
            CertId[] revCerts = new CertId[this.revCerts.Count];
            for (int index = 0; index != revCerts.Length; ++index)
                revCerts[index] = CertId.GetInstance( this.revCerts[index] );
            return revCerts;
        }

        public virtual CertificateList[] GetCrls()
        {
            if (this.crls == null)
                return null;
            CertificateList[] crls = new CertificateList[this.crls.Count];
            for (int index = 0; index != crls.Length; ++index)
                crls[index] = CertificateList.GetInstance( this.crls[index] );
            return crls;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         status
            } );
            this.AddOptional( v, 0, revCerts );
            this.AddOptional( v, 1, crls );
            return new DerSequence( v );
        }

        private void AddOptional( Asn1EncodableVector v, int tagNo, Asn1Encodable obj )
        {
            if (obj == null)
                return;
            v.Add( new DerTaggedObject( true, tagNo, obj ) );
        }
    }
}
