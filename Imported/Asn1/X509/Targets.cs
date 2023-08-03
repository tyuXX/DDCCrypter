// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Targets
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class Targets : Asn1Encodable
    {
        private readonly Asn1Sequence targets;

        public static Targets GetInstance( object obj )
        {
            switch (obj)
            {
                case Targets _:
                    return (Targets)obj;
                case Asn1Sequence _:
                    return new Targets( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private Targets( Asn1Sequence targets ) => this.targets = targets;

        public Targets( Target[] targets ) => this.targets = new DerSequence( targets );

        public virtual Target[] GetTargets()
        {
            Target[] targets = new Target[this.targets.Count];
            for (int index = 0; index < this.targets.Count; ++index)
                targets[index] = Target.GetInstance( this.targets[index] );
            return targets;
        }

        public override Asn1Object ToAsn1Object() => targets;
    }
}
