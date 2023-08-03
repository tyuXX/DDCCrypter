// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerTaggedObject
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1
{
    public class DerTaggedObject : Asn1TaggedObject
    {
        public DerTaggedObject( int tagNo, Asn1Encodable obj )
          : base( tagNo, obj )
        {
        }

        public DerTaggedObject( bool explicitly, int tagNo, Asn1Encodable obj )
          : base( explicitly, tagNo, obj )
        {
        }

        public DerTaggedObject( int tagNo )
          : base( false, tagNo, DerSequence.Empty )
        {
        }

        internal override void Encode( DerOutputStream derOut )
        {
            if (!this.IsEmpty())
            {
                byte[] derEncoded = this.obj.GetDerEncoded();
                if (this.explicitly)
                {
                    derOut.WriteEncoded( 160, this.tagNo, derEncoded );
                }
                else
                {
                    int flags = (derEncoded[0] & 32) | 128;
                    derOut.WriteTag( flags, this.tagNo );
                    derOut.Write( derEncoded, 1, derEncoded.Length - 1 );
                }
            }
            else
                derOut.WriteEncoded( 160, this.tagNo, new byte[0] );
        }
    }
}
