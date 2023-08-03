// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SecurityParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class SecurityParameters
    {
        internal int entity = -1;
        internal int cipherSuite = -1;
        internal byte compressionAlgorithm = 0;
        internal int prfAlgorithm = -1;
        internal int verifyDataLength = -1;
        internal byte[] masterSecret = null;
        internal byte[] clientRandom = null;
        internal byte[] serverRandom = null;
        internal byte[] sessionHash = null;
        internal byte[] pskIdentity = null;
        internal byte[] srpIdentity = null;
        internal short maxFragmentLength = -1;
        internal bool truncatedHMac = false;
        internal bool encryptThenMac = false;
        internal bool extendedMasterSecret = false;

        internal virtual void Clear()
        {
            if (this.masterSecret == null)
                return;
            Arrays.Fill( this.masterSecret, 0 );
            this.masterSecret = null;
        }

        public virtual int Entity => this.entity;

        public virtual int CipherSuite => this.cipherSuite;

        public byte CompressionAlgorithm => this.compressionAlgorithm;

        public virtual int PrfAlgorithm => this.prfAlgorithm;

        public virtual int VerifyDataLength => this.verifyDataLength;

        public virtual byte[] MasterSecret => this.masterSecret;

        public virtual byte[] ClientRandom => this.clientRandom;

        public virtual byte[] ServerRandom => this.serverRandom;

        public virtual byte[] SessionHash => this.sessionHash;

        public virtual byte[] PskIdentity => this.pskIdentity;

        public virtual byte[] SrpIdentity => this.srpIdentity;
    }
}
