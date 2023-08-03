// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.CryptoPro.Gost28147Parameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
    public class Gost28147Parameters : Asn1Encodable
    {
        private readonly Asn1OctetString iv;
        private readonly DerObjectIdentifier paramSet;

        public static Gost28147Parameters GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static Gost28147Parameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case Gost28147Parameters _:
                    return (Gost28147Parameters)obj;
                case Asn1Sequence _:
                    return new Gost28147Parameters( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid GOST3410Parameter: " + Platform.GetTypeName( obj ) );
            }
        }

        private Gost28147Parameters( Asn1Sequence seq )
        {
            this.iv = seq.Count == 2 ? Asn1OctetString.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.paramSet = DerObjectIdentifier.GetInstance( seq[1] );
        }

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       iv,
       paramSet
        } );
    }
}
