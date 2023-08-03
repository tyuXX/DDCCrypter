// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerOutputStream : DerOutputStream
    {
        public BerOutputStream( Stream os )
          : base( os )
        {
        }

        [Obsolete( "Use version taking an Asn1Encodable arg instead" )]
        public override void WriteObject( object obj )
        {
            switch (obj)
            {
                case null:
                    this.WriteNull();
                    break;
                case Asn1Object _:
                    ((Asn1Object)obj).Encode( this );
                    break;
                case Asn1Encodable _:
                    ((Asn1Encodable)obj).ToAsn1Object().Encode( this );
                    break;
                default:
                    throw new IOException( "object not BerEncodable" );
            }
        }
    }
}
