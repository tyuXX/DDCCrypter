// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Target
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
    public class Target : Asn1Encodable, IAsn1Choice
    {
        private readonly GeneralName targetName;
        private readonly GeneralName targetGroup;

        public static Target GetInstance( object obj )
        {
            switch (obj)
            {
                case Target _:
                    return (Target)obj;
                case Asn1TaggedObject _:
                    return new Target( (Asn1TaggedObject)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private Target( Asn1TaggedObject tagObj )
        {
            switch (tagObj.TagNo)
            {
                case 0:
                    this.targetName = GeneralName.GetInstance( tagObj, true );
                    break;
                case 1:
                    this.targetGroup = GeneralName.GetInstance( tagObj, true );
                    break;
                default:
                    throw new ArgumentException( "unknown tag: " + tagObj.TagNo );
            }
        }

        public Target( Target.Choice type, GeneralName name )
          : this( new DerTaggedObject( (int)type, name ) )
        {
        }

        public virtual GeneralName TargetGroup => this.targetGroup;

        public virtual GeneralName TargetName => this.targetName;

        public override Asn1Object ToAsn1Object() => this.targetName != null ? new DerTaggedObject( true, 0, targetName ) : (Asn1Object)new DerTaggedObject( true, 1, targetGroup );

        public enum Choice
        {
            Name,
            Group,
        }
    }
}
