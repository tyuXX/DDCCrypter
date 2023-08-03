// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SignatureAndHashAlgorithm
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class SignatureAndHashAlgorithm
    {
        protected readonly byte mHash;
        protected readonly byte mSignature;

        public SignatureAndHashAlgorithm( byte hash, byte signature )
        {
            if (!TlsUtilities.IsValidUint8( hash ))
                throw new ArgumentException( "should be a uint8", nameof( hash ) );
            if (!TlsUtilities.IsValidUint8( signature ))
                throw new ArgumentException( "should be a uint8", nameof( signature ) );
            if (signature == 0)
                throw new ArgumentException( "MUST NOT be \"anonymous\"", nameof( signature ) );
            this.mHash = hash;
            this.mSignature = signature;
        }

        public virtual byte Hash => this.mHash;

        public virtual byte Signature => this.mSignature;

        public override bool Equals( object obj )
        {
            if (!(obj is SignatureAndHashAlgorithm))
                return false;
            SignatureAndHashAlgorithm andHashAlgorithm = (SignatureAndHashAlgorithm)obj;
            return andHashAlgorithm.Hash == Hash && andHashAlgorithm.Signature == Signature;
        }

        public override int GetHashCode() => (Hash << 16) | Signature;

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteUint8( this.Hash, output );
            TlsUtilities.WriteUint8( this.Signature, output );
        }

        public static SignatureAndHashAlgorithm Parse( Stream input ) => new SignatureAndHashAlgorithm( TlsUtilities.ReadUint8( input ), TlsUtilities.ReadUint8( input ) );
    }
}
