// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.ECMqvWithKdfBasicAgreement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Agreement
{
    public class ECMqvWithKdfBasicAgreement : ECMqvBasicAgreement
    {
        private readonly string algorithm;
        private readonly IDerivationFunction kdf;

        public ECMqvWithKdfBasicAgreement( string algorithm, IDerivationFunction kdf )
        {
            if (algorithm == null)
                throw new ArgumentNullException( nameof( algorithm ) );
            if (kdf == null)
                throw new ArgumentNullException( nameof( kdf ) );
            this.algorithm = algorithm;
            this.kdf = kdf;
        }

        public override BigInteger CalculateAgreement( ICipherParameters pubKey )
        {
            BigInteger agreement = base.CalculateAgreement( pubKey );
            int defaultKeySize = GeneratorUtilities.GetDefaultKeySize( this.algorithm );
            this.kdf.Init( new DHKdfParameters( new DerObjectIdentifier( this.algorithm ), defaultKeySize, this.BigIntToBytes( agreement ) ) );
            byte[] numArray = new byte[defaultKeySize / 8];
            this.kdf.GenerateBytes( numArray, 0, numArray.Length );
            return new BigInteger( 1, numArray );
        }

        private byte[] BigIntToBytes( BigInteger r )
        {
            int byteLength = X9IntegerConverter.GetByteLength( this.privParams.StaticPrivateKey.Parameters.Curve );
            return X9IntegerConverter.IntegerToBytes( r, byteLength );
        }
    }
}
