// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DeferredHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DeferredHash : TlsHandshakeHash, IDigest
    {
        protected const int BUFFERING_HASH_LIMIT = 4;
        protected TlsContext mContext;
        private DigestInputBuffer mBuf;
        private IDictionary mHashes;
        private int mPrfHashAlgorithm;

        internal DeferredHash()
        {
            this.mBuf = new DigestInputBuffer();
            this.mHashes = Platform.CreateHashtable();
            this.mPrfHashAlgorithm = -1;
        }

        private DeferredHash( byte prfHashAlgorithm, IDigest prfHash )
        {
            this.mBuf = null;
            this.mHashes = Platform.CreateHashtable();
            this.mPrfHashAlgorithm = prfHashAlgorithm;
            this.mHashes[prfHashAlgorithm] = prfHash;
        }

        public virtual void Init( TlsContext context ) => this.mContext = context;

        public virtual TlsHandshakeHash NotifyPrfDetermined()
        {
            int prfAlgorithm = this.mContext.SecurityParameters.PrfAlgorithm;
            if (prfAlgorithm == 0)
            {
                CombinedHash d = new CombinedHash();
                d.Init( this.mContext );
                this.mBuf.UpdateDigest( d );
                return d.NotifyPrfDetermined();
            }
            this.mPrfHashAlgorithm = TlsUtilities.GetHashAlgorithmForPrfAlgorithm( prfAlgorithm );
            this.CheckTrackingHash( (byte)this.mPrfHashAlgorithm );
            return this;
        }

        public virtual void TrackHashAlgorithm( byte hashAlgorithm )
        {
            if (this.mBuf == null)
                throw new InvalidOperationException( "Too late to track more hash algorithms" );
            this.CheckTrackingHash( hashAlgorithm );
        }

        public virtual void SealHashAlgorithms() => this.CheckStopBuffering();

        public virtual TlsHandshakeHash StopTracking()
        {
            byte prfHashAlgorithm = (byte)this.mPrfHashAlgorithm;
            IDigest digest = TlsUtilities.CloneHash( prfHashAlgorithm, (IDigest)this.mHashes[prfHashAlgorithm] );
            if (this.mBuf != null)
                this.mBuf.UpdateDigest( digest );
            DeferredHash deferredHash = new DeferredHash( prfHashAlgorithm, digest );
            deferredHash.Init( this.mContext );
            return deferredHash;
        }

        public virtual IDigest ForkPrfHash()
        {
            this.CheckStopBuffering();
            byte prfHashAlgorithm = (byte)this.mPrfHashAlgorithm;
            if (this.mBuf == null)
                return TlsUtilities.CloneHash( prfHashAlgorithm, (IDigest)this.mHashes[prfHashAlgorithm] );
            IDigest hash = TlsUtilities.CreateHash( prfHashAlgorithm );
            this.mBuf.UpdateDigest( hash );
            return hash;
        }

        public virtual byte[] GetFinalHash( byte hashAlgorithm )
        {
            IDigest digest = TlsUtilities.CloneHash( hashAlgorithm, (IDigest)this.mHashes[hashAlgorithm] ?? throw new InvalidOperationException( "HashAlgorithm." + HashAlgorithm.GetText( hashAlgorithm ) + " is not being tracked" ) );
            if (this.mBuf != null)
                this.mBuf.UpdateDigest( digest );
            return DigestUtilities.DoFinal( digest );
        }

        public virtual string AlgorithmName => throw new InvalidOperationException( "Use Fork() to get a definite IDigest" );

        public virtual int GetByteLength() => throw new InvalidOperationException( "Use Fork() to get a definite IDigest" );

        public virtual int GetDigestSize() => throw new InvalidOperationException( "Use Fork() to get a definite IDigest" );

        public virtual void Update( byte input )
        {
            if (this.mBuf != null)
            {
                this.mBuf.WriteByte( input );
            }
            else
            {
                foreach (IDigest digest in (IEnumerable)this.mHashes.Values)
                    digest.Update( input );
            }
        }

        public virtual void BlockUpdate( byte[] input, int inOff, int len )
        {
            if (this.mBuf != null)
            {
                this.mBuf.Write( input, inOff, len );
            }
            else
            {
                foreach (IDigest digest in (IEnumerable)this.mHashes.Values)
                    digest.BlockUpdate( input, inOff, len );
            }
        }

        public virtual int DoFinal( byte[] output, int outOff ) => throw new InvalidOperationException( "Use Fork() to get a definite IDigest" );

        public virtual void Reset()
        {
            if (this.mBuf != null)
            {
                this.mBuf.SetLength( 0L );
            }
            else
            {
                foreach (IDigest digest in (IEnumerable)this.mHashes.Values)
                    digest.Reset();
            }
        }

        protected virtual void CheckStopBuffering()
        {
            if (this.mBuf == null || this.mHashes.Count > 4)
                return;
            foreach (IDigest d in (IEnumerable)this.mHashes.Values)
                this.mBuf.UpdateDigest( d );
            this.mBuf = null;
        }

        protected virtual void CheckTrackingHash( byte hashAlgorithm )
        {
            if (this.mHashes.Contains( hashAlgorithm ))
                return;
            IDigest hash = TlsUtilities.CreateHash( hashAlgorithm );
            this.mHashes[hashAlgorithm] = hash;
        }
    }
}
