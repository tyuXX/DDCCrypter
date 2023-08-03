// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.ProtectedPart
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class ProtectedPart : Asn1Encodable
    {
        private readonly PkiHeader header;
        private readonly PkiBody body;

        private ProtectedPart( Asn1Sequence seq )
        {
            this.header = PkiHeader.GetInstance( seq[0] );
            this.body = PkiBody.GetInstance( seq[1] );
        }

        public static ProtectedPart GetInstance( object obj )
        {
            switch (obj)
            {
                case ProtectedPart _:
                    return (ProtectedPart)obj;
                case Asn1Sequence _:
                    return new ProtectedPart( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public ProtectedPart( PkiHeader header, PkiBody body )
        {
            this.header = header;
            this.body = body;
        }

        public virtual PkiHeader Header => this.header;

        public virtual PkiBody Body => this.body;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       header,
       body
        } );
    }
}
