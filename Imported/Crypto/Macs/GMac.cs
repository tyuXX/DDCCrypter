// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.GMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class GMac : IMac
    {
        private readonly GcmBlockCipher cipher;
        private readonly int macSizeBits;

        public GMac( GcmBlockCipher cipher )
          : this( cipher, 128 )
        {
        }

        public GMac( GcmBlockCipher cipher, int macSizeBits )
        {
            this.cipher = cipher;
            this.macSizeBits = macSizeBits;
        }

        public void Init( ICipherParameters parameters )
        {
            ParametersWithIV parametersWithIv = parameters is ParametersWithIV ? (ParametersWithIV)parameters : throw new ArgumentException( "GMAC requires ParametersWithIV" );
            byte[] iv = parametersWithIv.GetIV();
            this.cipher.Init( true, new AeadParameters( (KeyParameter)parametersWithIv.Parameters, this.macSizeBits, iv ) );
        }

        public string AlgorithmName => this.cipher.GetUnderlyingCipher().AlgorithmName + "-GMAC";

        public int GetMacSize() => this.macSizeBits / 8;

        public void Update( byte input ) => this.cipher.ProcessAadByte( input );

        public void BlockUpdate( byte[] input, int inOff, int len ) => this.cipher.ProcessAadBytes( input, inOff, len );

        public int DoFinal( byte[] output, int outOff )
        {
            try
            {
                return this.cipher.DoFinal( output, outOff );
            }
            catch (InvalidCipherTextException ex)
            {
                throw new InvalidOperationException( ex.ToString() );
            }
        }

        public void Reset() => this.cipher.Reset();
    }
}
