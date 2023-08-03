// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.AttributePkcs
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class AttributePkcs : Asn1Encodable
    {
        private readonly DerObjectIdentifier attrType;
        private readonly Asn1Set attrValues;

        public static AttributePkcs GetInstance( object obj )
        {
            AttributePkcs instance = obj as AttributePkcs;
            if (obj == null || instance != null)
                return instance;
            return obj is Asn1Sequence seq ? new AttributePkcs( seq ) : throw new ArgumentException( "Unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        private AttributePkcs( Asn1Sequence seq )
        {
            this.attrType = seq.Count == 2 ? DerObjectIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.attrValues = Asn1Set.GetInstance( seq[1] );
        }

        public AttributePkcs( DerObjectIdentifier attrType, Asn1Set attrValues )
        {
            this.attrType = attrType;
            this.attrValues = attrValues;
        }

        public DerObjectIdentifier AttrType => this.attrType;

        public Asn1Set AttrValues => this.attrValues;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       attrType,
       attrValues
        } );
    }
}
