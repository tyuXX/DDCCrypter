// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Smime.SmimeCapabilityVector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Smime
{
    public class SmimeCapabilityVector
    {
        private readonly Asn1EncodableVector capabilities = new Asn1EncodableVector( new Asn1Encodable[0] );

        public void AddCapability( DerObjectIdentifier capability ) => this.capabilities.Add( new DerSequence( capability ) );

        public void AddCapability( DerObjectIdentifier capability, int value ) => this.capabilities.Add( new DerSequence( new Asn1Encodable[2]
        {
       capability,
       new DerInteger(value)
        } ) );

        public void AddCapability( DerObjectIdentifier capability, Asn1Encodable parameters ) => this.capabilities.Add( new DerSequence( new Asn1Encodable[2]
        {
       capability,
      parameters
        } ) );

        public Asn1EncodableVector ToAsn1EncodableVector() => this.capabilities;
    }
}
