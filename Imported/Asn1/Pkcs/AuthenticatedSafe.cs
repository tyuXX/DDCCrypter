// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.AuthenticatedSafe
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class AuthenticatedSafe : Asn1Encodable
    {
        private readonly ContentInfo[] info;

        public AuthenticatedSafe( Asn1Sequence seq )
        {
            this.info = new ContentInfo[seq.Count];
            for (int index = 0; index != this.info.Length; ++index)
                this.info[index] = ContentInfo.GetInstance( seq[index] );
        }

        public AuthenticatedSafe( ContentInfo[] info ) => this.info = (ContentInfo[])info.Clone();

        public ContentInfo[] GetContentInfo() => (ContentInfo[])this.info.Clone();

        public override Asn1Object ToAsn1Object() => new BerSequence( info );
    }
}
