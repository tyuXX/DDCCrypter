// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsSessionImpl
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class TlsSessionImpl : TlsSession
    {
        internal readonly byte[] mSessionID;
        internal SessionParameters mSessionParameters;

        internal TlsSessionImpl( byte[] sessionID, SessionParameters sessionParameters )
        {
            if (sessionID == null)
                throw new ArgumentNullException( nameof( sessionID ) );
            this.mSessionID = sessionID.Length >= 1 && sessionID.Length <= 32 ? Arrays.Clone( sessionID ) : throw new ArgumentException( "must have length between 1 and 32 bytes, inclusive", nameof( sessionID ) );
            this.mSessionParameters = sessionParameters;
        }

        public virtual SessionParameters ExportSessionParameters()
        {
            lock (this)
                return this.mSessionParameters == null ? null : this.mSessionParameters.Copy();
        }

        public virtual byte[] SessionID
        {
            get
            {
                lock (this)
                    return this.mSessionID;
            }
        }

        public virtual void Invalidate()
        {
            lock (this)
            {
                if (this.mSessionParameters == null)
                    return;
                this.mSessionParameters.Clear();
                this.mSessionParameters = null;
            }
        }

        public virtual bool IsResumable
        {
            get
            {
                lock (this)
                    return this.mSessionParameters != null;
            }
        }
    }
}
