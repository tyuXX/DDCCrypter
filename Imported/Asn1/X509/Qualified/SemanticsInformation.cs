// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.SemanticsInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public class SemanticsInformation : Asn1Encodable
    {
        private readonly DerObjectIdentifier semanticsIdentifier;
        private readonly GeneralName[] nameRegistrationAuthorities;

        public static SemanticsInformation GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SemanticsInformation _:
                    return (SemanticsInformation)obj;
                case Asn1Sequence _:
                    return new SemanticsInformation( Asn1Sequence.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public SemanticsInformation( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.Count >= 1 ? seq.GetEnumerator() : throw new ArgumentException( "no objects in SemanticsInformation" );
            enumerator.MoveNext();
            object current = enumerator.Current;
            if (current is DerObjectIdentifier)
            {
                this.semanticsIdentifier = DerObjectIdentifier.GetInstance( current );
                current = !enumerator.MoveNext() ? null : enumerator.Current;
            }
            if (current == null)
                return;
            Asn1Sequence instance = Asn1Sequence.GetInstance( current );
            this.nameRegistrationAuthorities = new GeneralName[instance.Count];
            for (int index = 0; index < instance.Count; ++index)
                this.nameRegistrationAuthorities[index] = GeneralName.GetInstance( instance[index] );
        }

        public SemanticsInformation( DerObjectIdentifier semanticsIdentifier, GeneralName[] generalNames )
        {
            this.semanticsIdentifier = semanticsIdentifier;
            this.nameRegistrationAuthorities = generalNames;
        }

        public SemanticsInformation( DerObjectIdentifier semanticsIdentifier ) => this.semanticsIdentifier = semanticsIdentifier;

        public SemanticsInformation( GeneralName[] generalNames ) => this.nameRegistrationAuthorities = generalNames;

        public DerObjectIdentifier SemanticsIdentifier => this.semanticsIdentifier;

        public GeneralName[] GetNameRegistrationAuthorities() => this.nameRegistrationAuthorities;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.semanticsIdentifier != null)
                v.Add( semanticsIdentifier );
            if (this.nameRegistrationAuthorities != null)
                v.Add( new DerSequence( nameRegistrationAuthorities ) );
            return new DerSequence( v );
        }
    }
}
