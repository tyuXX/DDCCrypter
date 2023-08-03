// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.TargetInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class TargetInformation : Asn1Encodable
    {
        private readonly Asn1Sequence targets;

        public static TargetInformation GetInstance( object obj )
        {
            switch (obj)
            {
                case TargetInformation _:
                    return (TargetInformation)obj;
                case Asn1Sequence _:
                    return new TargetInformation( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private TargetInformation( Asn1Sequence targets ) => this.targets = targets;

        public virtual Targets[] GetTargetsObjects()
        {
            Targets[] targetsObjects = new Targets[this.targets.Count];
            for (int index = 0; index < this.targets.Count; ++index)
                targetsObjects[index] = Targets.GetInstance( this.targets[index] );
            return targetsObjects;
        }

        public TargetInformation( Targets targets ) => this.targets = new DerSequence( targets );

        public TargetInformation( Target[] targets )
          : this( new Targets( targets ) )
        {
        }

        public override Asn1Object ToAsn1Object() => targets;
    }
}
