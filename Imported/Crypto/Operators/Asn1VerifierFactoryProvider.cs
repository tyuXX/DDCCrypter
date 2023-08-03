// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.Asn1VerifierFactoryProvider
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Operators
{
    public class Asn1VerifierFactoryProvider : IVerifierFactoryProvider
    {
        private readonly AsymmetricKeyParameter publicKey;

        public Asn1VerifierFactoryProvider( AsymmetricKeyParameter publicKey ) => this.publicKey = publicKey;

        public IVerifierFactory CreateVerifierFactory( object algorithmDetails ) => new Asn1VerifierFactory( (AlgorithmIdentifier)algorithmDetails, this.publicKey );

        public IEnumerable SignatureAlgNames => X509Utilities.GetAlgNames();
    }
}
