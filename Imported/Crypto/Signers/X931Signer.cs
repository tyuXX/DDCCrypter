// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.X931Signer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class X931Signer : ISigner
    {
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_IMPLICIT = 188;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_RIPEMD160 = 12748;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_RIPEMD128 = 13004;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_SHA1 = 13260;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_SHA256 = 13516;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_SHA512 = 13772;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_SHA384 = 14028;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_WHIRLPOOL = 14284;
        [Obsolete( "Use 'IsoTrailers' instead" )]
        public const int TRAILER_SHA224 = 14540;
        private IDigest digest;
        private IAsymmetricBlockCipher cipher;
        private RsaKeyParameters kParam;
        private int trailer;
        private int keyBits;
        private byte[] block;

        public X931Signer( IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit )
        {
            this.cipher = cipher;
            this.digest = digest;
            if (isImplicit)
                this.trailer = 188;
            else
                this.trailer = !IsoTrailers.NoTrailerAvailable( digest ) ? IsoTrailers.GetTrailer( digest ) : throw new ArgumentException( "no valid trailer", nameof( digest ) );
        }

        public virtual string AlgorithmName => this.digest.AlgorithmName + "with" + this.cipher.AlgorithmName + "/X9.31";

        public X931Signer( IAsymmetricBlockCipher cipher, IDigest digest )
          : this( cipher, digest, false )
        {
        }

        public virtual void Init( bool forSigning, ICipherParameters parameters )
        {
            this.kParam = (RsaKeyParameters)parameters;
            this.cipher.Init( forSigning, kParam );
            this.keyBits = this.kParam.Modulus.BitLength;
            this.block = new byte[(this.keyBits + 7) / 8];
            this.Reset();
        }

        private void ClearBlock( byte[] block ) => Array.Clear( block, 0, block.Length );

        public virtual void Update( byte b ) => this.digest.Update( b );

        public virtual void BlockUpdate( byte[] input, int off, int len ) => this.digest.BlockUpdate( input, off, len );

        public virtual void Reset() => this.digest.Reset();

        public virtual byte[] GenerateSignature()
        {
            this.CreateSignatureBlock();
            BigInteger n = new( 1, this.cipher.ProcessBlock( this.block, 0, this.block.Length ) );
            this.ClearBlock( this.block );
            return BigIntegers.AsUnsignedByteArray( (this.kParam.Modulus.BitLength + 7) / 8, n.Min( this.kParam.Modulus.Subtract( n ) ) );
        }

        private void CreateSignatureBlock()
        {
            int digestSize = this.digest.GetDigestSize();
            int outOff;
            if (this.trailer == 188)
            {
                outOff = this.block.Length - digestSize - 1;
                this.digest.DoFinal( this.block, outOff );
                this.block[this.block.Length - 1] = 188;
            }
            else
            {
                outOff = this.block.Length - digestSize - 2;
                this.digest.DoFinal( this.block, outOff );
                this.block[this.block.Length - 2] = (byte)(this.trailer >> 8);
                this.block[this.block.Length - 1] = (byte)this.trailer;
            }
            this.block[0] = 107;
            for (int index = outOff - 2; index != 0; --index)
                this.block[index] = 187;
            this.block[outOff - 1] = 186;
        }

        public virtual bool VerifySignature( byte[] signature )
        {
            try
            {
                this.block = this.cipher.ProcessBlock( signature, 0, signature.Length );
            }
            catch (Exception ex)
            {
                return false;
            }
            BigInteger n1 = new( this.block );
            BigInteger n2;
            if ((n1.IntValue & 15) == 12)
            {
                n2 = n1;
            }
            else
            {
                BigInteger bigInteger = this.kParam.Modulus.Subtract( n1 );
                if ((bigInteger.IntValue & 15) != 12)
                    return false;
                n2 = bigInteger;
            }
            this.CreateSignatureBlock();
            byte[] numArray = BigIntegers.AsUnsignedByteArray( this.block.Length, n2 );
            bool flag = Arrays.ConstantTimeAreEqual( this.block, numArray );
            this.ClearBlock( this.block );
            this.ClearBlock( numArray );
            return flag;
        }
    }
}
