// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.Kdf.ECDHKekGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Agreement.Kdf
{
    public class ECDHKekGenerator : IDerivationFunction
    {
        private readonly IDerivationFunction kdf;
        private DerObjectIdentifier algorithm;
        private int keySize;
        private byte[] z;

        public ECDHKekGenerator( IDigest digest ) => this.kdf = new Kdf2BytesGenerator( digest );

        public virtual void Init( IDerivationParameters param )
        {
            DHKdfParameters dhKdfParameters = (DHKdfParameters)param;
            this.algorithm = dhKdfParameters.Algorithm;
            this.keySize = dhKdfParameters.KeySize;
            this.z = dhKdfParameters.GetZ();
        }

        public virtual IDigest Digest => this.kdf.Digest;

        public virtual int GenerateBytes( byte[] outBytes, int outOff, int len )
        {
            this.kdf.Init( new KdfParameters( this.z, new DerSequence( new Asn1Encodable[2]
            {
         new AlgorithmIdentifier(this.algorithm,  DerNull.Instance),
         new DerTaggedObject(true, 2,  new DerOctetString(Pack.UInt32_To_BE((uint) this.keySize)))
            } ).GetDerEncoded() ) );
            return this.kdf.GenerateBytes( outBytes, outOff, len );
        }
    }
}
