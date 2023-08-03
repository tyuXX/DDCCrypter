// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.Asn1VerifierFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Operators
{
    public class Asn1VerifierFactory : IVerifierFactory
    {
        private readonly AlgorithmIdentifier algID;
        private readonly AsymmetricKeyParameter publicKey;

        public Asn1VerifierFactory( string algorithm, AsymmetricKeyParameter publicKey )
        {
            DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid( algorithm );
            this.publicKey = publicKey;
            this.algID = X509Utilities.GetSigAlgID( algorithmOid, algorithm );
        }

        public Asn1VerifierFactory( AlgorithmIdentifier algorithm, AsymmetricKeyParameter publicKey )
        {
            this.publicKey = publicKey;
            this.algID = algorithm;
        }

        public object AlgorithmDetails => algID;

        public IStreamCalculator CreateCalculator()
        {
            ISigner signer = SignerUtilities.GetSigner( X509Utilities.GetSignatureName( this.algID ) );
            signer.Init( false, publicKey );
            return new VerifierCalculator( signer );
        }
    }
}
