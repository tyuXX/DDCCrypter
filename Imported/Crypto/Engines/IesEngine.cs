// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.IesEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class IesEngine
    {
        private readonly IBasicAgreement agree;
        private readonly IDerivationFunction kdf;
        private readonly IMac mac;
        private readonly BufferedBlockCipher cipher;
        private readonly byte[] macBuf;
        private bool forEncryption;
        private ICipherParameters privParam;
        private ICipherParameters pubParam;
        private IesParameters param;

        public IesEngine( IBasicAgreement agree, IDerivationFunction kdf, IMac mac )
        {
            this.agree = agree;
            this.kdf = kdf;
            this.mac = mac;
            this.macBuf = new byte[mac.GetMacSize()];
        }

        public IesEngine(
          IBasicAgreement agree,
          IDerivationFunction kdf,
          IMac mac,
          BufferedBlockCipher cipher )
        {
            this.agree = agree;
            this.kdf = kdf;
            this.mac = mac;
            this.macBuf = new byte[mac.GetMacSize()];
            this.cipher = cipher;
        }

        public virtual void Init(
          bool forEncryption,
          ICipherParameters privParameters,
          ICipherParameters pubParameters,
          ICipherParameters iesParameters )
        {
            this.forEncryption = forEncryption;
            this.privParam = privParameters;
            this.pubParam = pubParameters;
            this.param = (IesParameters)iesParameters;
        }

        private byte[] DecryptBlock( byte[] in_enc, int inOff, int inLen, byte[] z )
        {
            KdfParameters kdfParameters = new( z, this.param.GetDerivationV() );
            int macKeySize = this.param.MacKeySize;
            this.kdf.Init( kdfParameters );
            if (inLen < this.mac.GetMacSize())
                throw new InvalidCipherTextException( "Length of input must be greater than the MAC" );
            inLen -= this.mac.GetMacSize();
            byte[] numArray;
            KeyParameter parameters;
            if (this.cipher == null)
            {
                byte[] kdfBytes = this.GenerateKdfBytes( kdfParameters, inLen + (macKeySize / 8) );
                numArray = new byte[inLen];
                for (int index = 0; index != inLen; ++index)
                    numArray[index] = (byte)(in_enc[inOff + index] ^ (uint)kdfBytes[index]);
                parameters = new KeyParameter( kdfBytes, inLen, macKeySize / 8 );
            }
            else
            {
                int cipherKeySize = ((IesWithCipherParameters)this.param).CipherKeySize;
                byte[] kdfBytes = this.GenerateKdfBytes( kdfParameters, (cipherKeySize / 8) + (macKeySize / 8) );
                this.cipher.Init( false, new KeyParameter( kdfBytes, 0, cipherKeySize / 8 ) );
                numArray = this.cipher.DoFinal( in_enc, inOff, inLen );
                parameters = new KeyParameter( kdfBytes, cipherKeySize / 8, macKeySize / 8 );
            }
            byte[] encodingV = this.param.GetEncodingV();
            this.mac.Init( parameters );
            this.mac.BlockUpdate( in_enc, inOff, inLen );
            this.mac.BlockUpdate( encodingV, 0, encodingV.Length );
            this.mac.DoFinal( this.macBuf, 0 );
            inOff += inLen;
            if (!Arrays.ConstantTimeAreEqual( Arrays.CopyOfRange( in_enc, inOff, inOff + this.macBuf.Length ), this.macBuf ))
                throw new InvalidCipherTextException( "Invalid MAC." );
            return numArray;
        }

        private byte[] EncryptBlock( byte[] input, int inOff, int inLen, byte[] z )
        {
            KdfParameters kParam = new( z, this.param.GetDerivationV() );
            int macKeySize = this.param.MacKeySize;
            byte[] numArray1;
            int num;
            KeyParameter parameters;
            if (this.cipher == null)
            {
                byte[] kdfBytes = this.GenerateKdfBytes( kParam, inLen + (macKeySize / 8) );
                numArray1 = new byte[inLen + this.mac.GetMacSize()];
                num = inLen;
                for (int index = 0; index != inLen; ++index)
                    numArray1[index] = (byte)(input[inOff + index] ^ (uint)kdfBytes[index]);
                parameters = new KeyParameter( kdfBytes, inLen, macKeySize / 8 );
            }
            else
            {
                int cipherKeySize = ((IesWithCipherParameters)this.param).CipherKeySize;
                byte[] kdfBytes = this.GenerateKdfBytes( kParam, (cipherKeySize / 8) + (macKeySize / 8) );
                this.cipher.Init( true, new KeyParameter( kdfBytes, 0, cipherKeySize / 8 ) );
                byte[] numArray2 = new byte[this.cipher.GetOutputSize( inLen )];
                int outOff = this.cipher.ProcessBytes( input, inOff, inLen, numArray2, 0 );
                int length = outOff + this.cipher.DoFinal( numArray2, outOff );
                numArray1 = new byte[length + this.mac.GetMacSize()];
                num = length;
                Array.Copy( numArray2, 0, numArray1, 0, length );
                parameters = new KeyParameter( kdfBytes, cipherKeySize / 8, macKeySize / 8 );
            }
            byte[] encodingV = this.param.GetEncodingV();
            this.mac.Init( parameters );
            this.mac.BlockUpdate( numArray1, 0, num );
            this.mac.BlockUpdate( encodingV, 0, encodingV.Length );
            this.mac.DoFinal( numArray1, num );
            return numArray1;
        }

        private byte[] GenerateKdfBytes( KdfParameters kParam, int length )
        {
            byte[] output = new byte[length];
            this.kdf.Init( kParam );
            this.kdf.GenerateBytes( output, 0, output.Length );
            return output;
        }

        public virtual byte[] ProcessBlock( byte[] input, int inOff, int inLen )
        {
            this.agree.Init( this.privParam );
            BigInteger agreement = this.agree.CalculateAgreement( this.pubParam );
            byte[] z = BigIntegers.AsUnsignedByteArray( this.agree.GetFieldSize(), agreement );
            try
            {
                return this.forEncryption ? this.EncryptBlock( input, inOff, inLen, z ) : this.DecryptBlock( input, inOff, inLen, z );
            }
            finally
            {
                Array.Clear( z, 0, z.Length );
            }
        }
    }
}
