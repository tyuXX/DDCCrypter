// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.DeclarationOfMajority
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class DeclarationOfMajority : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1TaggedObject declaration;

        public DeclarationOfMajority( int notYoungerThan ) => this.declaration = new DerTaggedObject( false, 0, new DerInteger( notYoungerThan ) );

        public DeclarationOfMajority( bool fullAge, string country )
        {
            DerPrintableString derPrintableString = country.Length <= 2 ? new DerPrintableString( country, true ) : throw new ArgumentException( "country can only be 2 characters" );
            DerSequence derSequence;
            if (fullAge)
                derSequence = new DerSequence( derPrintableString );
            else
                derSequence = new DerSequence( new Asn1Encodable[2]
                {
           DerBoolean.False,
           derPrintableString
                } );
            this.declaration = new DerTaggedObject( false, 1, derSequence );
        }

        public DeclarationOfMajority( DerGeneralizedTime dateOfBirth ) => this.declaration = new DerTaggedObject( false, 2, dateOfBirth );

        public static DeclarationOfMajority GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DeclarationOfMajority _:
                    return (DeclarationOfMajority)obj;
                case Asn1TaggedObject _:
                    return new DeclarationOfMajority( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private DeclarationOfMajority( Asn1TaggedObject o ) => this.declaration = o.TagNo <= 2 ? o : throw new ArgumentException( "Bad tag number: " + o.TagNo );

        public override Asn1Object ToAsn1Object() => declaration;

        public DeclarationOfMajority.Choice Type => (DeclarationOfMajority.Choice)this.declaration.TagNo;

        public virtual int NotYoungerThan => this.declaration.TagNo == 0 ? DerInteger.GetInstance( this.declaration, false ).Value.IntValue : -1;

        public virtual Asn1Sequence FullAgeAtCountry => this.declaration.TagNo == 1 ? Asn1Sequence.GetInstance( this.declaration, false ) : null;

        public virtual DerGeneralizedTime DateOfBirth => this.declaration.TagNo == 2 ? DerGeneralizedTime.GetInstance( this.declaration, false ) : null;

        public enum Choice
        {
            NotYoungerThan,
            FullAgeAtCountry,
            DateOfBirth,
        }
    }
}
