// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.BufferedIesCipher
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto
{
    public class BufferedIesCipher : BufferedCipherBase
    {
        private readonly IesEngine engine;
        private bool forEncryption;
        private MemoryStream buffer = new MemoryStream();

        public BufferedIesCipher( IesEngine engine ) => this.engine = engine != null ? engine : throw new ArgumentNullException( nameof( engine ) );

        public override string AlgorithmName => "IES";

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.forEncryption = forEncryption;
            throw Platform.CreateNotImplementedException( "IES" );
        }

        public override int GetBlockSize() => 0;

        public override int GetOutputSize( int inputLen )
        {
            if (this.engine == null)
                throw new InvalidOperationException( "cipher not initialised" );
            int num = inputLen + (int)this.buffer.Length;
            return !this.forEncryption ? num - 20 : num + 20;
        }

        public override int GetUpdateOutputSize( int inputLen ) => 0;

        public override byte[] ProcessByte( byte input )
        {
            this.buffer.WriteByte( input );
            return null;
        }

        public override byte[] ProcessBytes( byte[] input, int inOff, int length )
        {
            if (input == null)
                throw new ArgumentNullException( nameof( input ) );
            if (inOff < 0)
                throw new ArgumentException( nameof( inOff ) );
            if (length < 0)
                throw new ArgumentException( nameof( length ) );
            if (inOff + length > input.Length)
                throw new ArgumentException( "invalid offset/length specified for input array" );
            this.buffer.Write( input, inOff, length );
            return null;
        }

        public override byte[] DoFinal()
        {
            byte[] array = this.buffer.ToArray();
            this.Reset();
            return this.engine.ProcessBlock( array, 0, array.Length );
        }

        public override byte[] DoFinal( byte[] input, int inOff, int length )
        {
            this.ProcessBytes( input, inOff, length );
            return this.DoFinal();
        }

        public override void Reset() => this.buffer.SetLength( 0L );
    }
}
