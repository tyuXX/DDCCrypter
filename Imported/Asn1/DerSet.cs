// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerSet
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerSet : Asn1Set
    {
        public static readonly DerSet Empty = new DerSet();

        public static DerSet FromVector( Asn1EncodableVector v ) => v.Count >= 1 ? new DerSet( v ) : Empty;

        internal static DerSet FromVector( Asn1EncodableVector v, bool needsSorting ) => v.Count >= 1 ? new DerSet( v, needsSorting ) : Empty;

        public DerSet()
          : base( 0 )
        {
        }

        public DerSet( Asn1Encodable obj )
          : base( 1 )
        {
            this.AddObject( obj );
        }

        public DerSet( params Asn1Encodable[] v )
          : base( v.Length )
        {
            foreach (Asn1Encodable asn1Encodable in v)
                this.AddObject( asn1Encodable );
            this.Sort();
        }

        public DerSet( Asn1EncodableVector v )
          : this( v, true )
        {
        }

        internal DerSet( Asn1EncodableVector v, bool needsSorting )
          : base( v.Count )
        {
            foreach (Asn1Encodable asn1Encodable in v)
                this.AddObject( asn1Encodable );
            if (!needsSorting)
                return;
            this.Sort();
        }

        internal override void Encode( DerOutputStream derOut )
        {
            MemoryStream os = new MemoryStream();
            DerOutputStream s = new DerOutputStream( os );
            foreach (Asn1Encodable asn1Encodable in (Asn1Set)this)
                s.WriteObject( asn1Encodable );
            Platform.Dispose( s );
            byte[] array = os.ToArray();
            derOut.WriteEncoded( 49, array );
        }
    }
}
