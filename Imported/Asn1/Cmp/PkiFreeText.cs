// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiFreeText
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiFreeText : Asn1Encodable
    {
        internal Asn1Sequence strings;

        public static PkiFreeText GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        public static PkiFreeText GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiFreeText _:
                    return (PkiFreeText)obj;
                case Asn1Sequence _:
                    return new PkiFreeText( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public PkiFreeText( Asn1Sequence seq )
        {
            foreach (object obj in seq)
            {
                if (!(obj is DerUtf8String))
                    throw new ArgumentException( "attempt to insert non UTF8 STRING into PkiFreeText" );
            }
            this.strings = seq;
        }

        public PkiFreeText( DerUtf8String p ) => this.strings = new DerSequence( p );

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.strings.Count;

        public int Count => this.strings.Count;

        public DerUtf8String this[int index] => (DerUtf8String)this.strings[index];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public DerUtf8String GetStringAt( int index ) => this[index];

        public override Asn1Object ToAsn1Object() => strings;
    }
}
