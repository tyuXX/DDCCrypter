// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ECDHPublicBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math.EC;
using System;

namespace Org.BouncyCastle.Bcpg
{
    public class ECDHPublicBcpgKey : ECPublicBcpgKey
    {
        private byte reserved;
        private HashAlgorithmTag hashFunctionId;
        private SymmetricKeyAlgorithmTag symAlgorithmId;

        public ECDHPublicBcpgKey( BcpgInputStream bcpgIn )
          : base( bcpgIn )
        {
            byte[] buffer = new byte[bcpgIn.ReadByte()];
            if (buffer.Length != 3)
                throw new InvalidOperationException( "kdf parameters size of 3 expected." );
            bcpgIn.ReadFully( buffer );
            this.reserved = buffer[0];
            this.hashFunctionId = (HashAlgorithmTag)buffer[1];
            this.symAlgorithmId = (SymmetricKeyAlgorithmTag)buffer[2];
            this.VerifyHashAlgorithm();
            this.VerifySymmetricKeyAlgorithm();
        }

        public ECDHPublicBcpgKey(
          DerObjectIdentifier oid,
          ECPoint point,
          HashAlgorithmTag hashAlgorithm,
          SymmetricKeyAlgorithmTag symmetricKeyAlgorithm )
          : base( oid, point )
        {
            this.reserved = 1;
            this.hashFunctionId = hashAlgorithm;
            this.symAlgorithmId = symmetricKeyAlgorithm;
            this.VerifyHashAlgorithm();
            this.VerifySymmetricKeyAlgorithm();
        }

        public virtual byte Reserved => this.reserved;

        public virtual HashAlgorithmTag HashAlgorithm => this.hashFunctionId;

        public virtual SymmetricKeyAlgorithmTag SymmetricKeyAlgorithm => this.symAlgorithmId;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            base.Encode( bcpgOut );
            bcpgOut.WriteByte( 3 );
            bcpgOut.WriteByte( this.reserved );
            bcpgOut.WriteByte( (byte)this.hashFunctionId );
            bcpgOut.WriteByte( (byte)this.symAlgorithmId );
        }

        private void VerifyHashAlgorithm()
        {
            switch (this.hashFunctionId)
            {
                case HashAlgorithmTag.Sha256:
                    break;
                case HashAlgorithmTag.Sha384:
                    break;
                case HashAlgorithmTag.Sha512:
                    break;
                default:
                    throw new InvalidOperationException( "Hash algorithm must be SHA-256 or stronger." );
            }
        }

        private void VerifySymmetricKeyAlgorithm()
        {
            switch (this.symAlgorithmId)
            {
                case SymmetricKeyAlgorithmTag.Aes128:
                    break;
                case SymmetricKeyAlgorithmTag.Aes192:
                    break;
                case SymmetricKeyAlgorithmTag.Aes256:
                    break;
                default:
                    throw new InvalidOperationException( "Symmetric key algorithm must be AES-128 or stronger." );
            }
        }
    }
}
