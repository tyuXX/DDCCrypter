// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerTaggedObject
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1
{
    public class BerTaggedObject : DerTaggedObject
    {
        public BerTaggedObject( int tagNo, Asn1Encodable obj )
          : base( tagNo, obj )
        {
        }

        public BerTaggedObject( bool explicitly, int tagNo, Asn1Encodable obj )
          : base( explicitly, tagNo, obj )
        {
        }

        public BerTaggedObject( int tagNo )
          : base( false, tagNo, BerSequence.Empty )
        {
        }

        internal override void Encode( DerOutputStream derOut )
        {
            switch (derOut)
            {
                case Asn1OutputStream _:
                case BerOutputStream _:
                    derOut.WriteTag( 160, this.tagNo );
                    derOut.WriteByte( 128 );
                    if (!this.IsEmpty())
                    {
                        if (!this.explicitly)
                        {
                            IEnumerable enumerable;
                            if (this.obj is Asn1OctetString)
                                enumerable = !(this.obj is BerOctetString) ? new BerOctetString( ((Asn1OctetString)this.obj).GetOctets() ) : (IEnumerable)this.obj;
                            else if (this.obj is Asn1Sequence)
                                enumerable = (IEnumerable)this.obj;
                            else
                                enumerable = this.obj is Asn1Set ? (IEnumerable)this.obj : throw Platform.CreateNotImplementedException( Platform.GetTypeName( obj ) );
                            foreach (Asn1Encodable asn1Encodable in enumerable)
                                derOut.WriteObject( asn1Encodable );
                        }
                        else
                            derOut.WriteObject( this.obj );
                    }
                    derOut.WriteByte( 0 );
                    derOut.WriteByte( 0 );
                    break;
                default:
                    base.Encode( derOut );
                    break;
            }
        }
    }
}
