// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Smime.SmimeCapabilitiesAttribute
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Asn1.Smime
{
    public class SmimeCapabilitiesAttribute : AttributeX509
    {
        public SmimeCapabilitiesAttribute( SmimeCapabilityVector capabilities )
          : base( SmimeAttributes.SmimeCapabilities, new DerSet( new DerSequence( capabilities.ToAsn1EncodableVector() ) ) )
        {
        }
    }
}
