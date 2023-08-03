// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.Qualified.TypeOfBiometricData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509.Qualified
{
    public class TypeOfBiometricData : Asn1Encodable, IAsn1Choice
    {
        public const int Picture = 0;
        public const int HandwrittenSignature = 1;
        internal Asn1Encodable obj;

        public static TypeOfBiometricData GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case TypeOfBiometricData _:
                    return (TypeOfBiometricData)obj;
                case DerInteger _:
                    return new TypeOfBiometricData( DerInteger.GetInstance( obj ).Value.IntValue );
                case DerObjectIdentifier _:
                    return new TypeOfBiometricData( DerObjectIdentifier.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public TypeOfBiometricData( int predefinedBiometricType ) => this.obj = predefinedBiometricType == 0 || predefinedBiometricType == 1 ? (Asn1Encodable)new DerInteger( predefinedBiometricType ) : throw new ArgumentException( "unknow PredefinedBiometricType : " + predefinedBiometricType );

        public TypeOfBiometricData( DerObjectIdentifier biometricDataOid ) => this.obj = biometricDataOid;

        public bool IsPredefined => this.obj is DerInteger;

        public int PredefinedBiometricType => ((DerInteger)this.obj).Value.IntValue;

        public DerObjectIdentifier BiometricDataOid => (DerObjectIdentifier)this.obj;

        public override Asn1Object ToAsn1Object() => this.obj.ToAsn1Object();
    }
}
