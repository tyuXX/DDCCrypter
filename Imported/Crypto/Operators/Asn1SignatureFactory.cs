// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.Asn1SignatureFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Operators
{
    public class Asn1SignatureFactory : ISignatureFactory
    {
        private readonly AlgorithmIdentifier algID;
        private readonly string algorithm;
        private readonly AsymmetricKeyParameter privateKey;
        private readonly SecureRandom random;

        public Asn1SignatureFactory( string algorithm, AsymmetricKeyParameter privateKey )
          : this( algorithm, privateKey, null )
        {
        }

        public Asn1SignatureFactory(
          string algorithm,
          AsymmetricKeyParameter privateKey,
          SecureRandom random )
        {
            DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid( algorithm );
            this.algorithm = algorithm;
            this.privateKey = privateKey;
            this.random = random;
            this.algID = X509Utilities.GetSigAlgID( algorithmOid, algorithm );
        }

        public object AlgorithmDetails => algID;

        public IStreamCalculator CreateCalculator()
        {
            ISigner signer = SignerUtilities.GetSigner( this.algorithm );
            if (this.random != null)
                signer.Init( true, new ParametersWithRandom( privateKey, this.random ) );
            else
                signer.Init( true, privateKey );
            return new SigCalculator( signer );
        }

        public static IEnumerable SignatureAlgNames => X509Utilities.GetAlgNames();
    }
}
