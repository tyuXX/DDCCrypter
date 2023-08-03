// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RsaEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RsaEngine : IAsymmetricBlockCipher
    {
        private RsaCoreEngine core;

        public virtual string AlgorithmName => "RSA";

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            if (this.core == null)
                this.core = new RsaCoreEngine();
            this.core.Init( forEncryption, parameters );
        }

        public virtual int GetInputBlockSize() => this.core.GetInputBlockSize();

        public virtual int GetOutputBlockSize() => this.core.GetOutputBlockSize();

        public virtual byte[] ProcessBlock( byte[] inBuf, int inOff, int inLen )
        {
            if (this.core == null)
                throw new InvalidOperationException( "RSA engine not initialised" );
            return this.core.ConvertOutput( this.core.ProcessBlock( this.core.ConvertInput( inBuf, inOff, inLen ) ) );
        }
    }
}
