// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerSequence
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerSequence : Asn1Sequence
    {
        public static readonly DerSequence Empty = new DerSequence();

        public static DerSequence FromVector( Asn1EncodableVector v ) => v.Count >= 1 ? new DerSequence( v ) : Empty;

        public DerSequence()
          : base( 0 )
        {
        }

        public DerSequence( Asn1Encodable obj )
          : base( 1 )
        {
            this.AddObject( obj );
        }

        public DerSequence( params Asn1Encodable[] v )
          : base( v.Length )
        {
            foreach (Asn1Encodable asn1Encodable in v)
                this.AddObject( asn1Encodable );
        }

        public DerSequence( Asn1EncodableVector v )
          : base( v.Count )
        {
            foreach (Asn1Encodable asn1Encodable in v)
                this.AddObject( asn1Encodable );
        }

        internal override void Encode( DerOutputStream derOut )
        {
            MemoryStream os = new MemoryStream();
            DerOutputStream s = new DerOutputStream( os );
            foreach (Asn1Encodable asn1Encodable in (Asn1Sequence)this)
                s.WriteObject( asn1Encodable );
            Platform.Dispose( s );
            byte[] array = os.ToArray();
            derOut.WriteEncoded( 48, array );
        }
    }
}
