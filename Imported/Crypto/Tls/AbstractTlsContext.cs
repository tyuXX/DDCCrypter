// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.AbstractTlsContext
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Threading;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal abstract class AbstractTlsContext : TlsContext
    {
        private static long counter = Times.NanoTime();
        private readonly IRandomGenerator mNonceRandom;
        private readonly SecureRandom mSecureRandom;
        private readonly SecurityParameters mSecurityParameters;
        private ProtocolVersion mClientVersion = null;
        private ProtocolVersion mServerVersion = null;
        private TlsSession mSession = null;
        private object mUserObject = null;

        private static long NextCounterValue() => Interlocked.Increment( ref counter );

        internal AbstractTlsContext( SecureRandom secureRandom, SecurityParameters securityParameters )
        {
            IDigest hash = TlsUtilities.CreateHash( 4 );
            byte[] numArray = new byte[hash.GetDigestSize()];
            secureRandom.NextBytes( numArray );
            this.mNonceRandom = new DigestRandomGenerator( hash );
            this.mNonceRandom.AddSeedMaterial( NextCounterValue() );
            this.mNonceRandom.AddSeedMaterial( Times.NanoTime() );
            this.mNonceRandom.AddSeedMaterial( numArray );
            this.mSecureRandom = secureRandom;
            this.mSecurityParameters = securityParameters;
        }

        public virtual IRandomGenerator NonceRandomGenerator => this.mNonceRandom;

        public virtual SecureRandom SecureRandom => this.mSecureRandom;

        public virtual SecurityParameters SecurityParameters => this.mSecurityParameters;

        public abstract bool IsServer { get; }

        public virtual ProtocolVersion ClientVersion => this.mClientVersion;

        internal virtual void SetClientVersion( ProtocolVersion clientVersion ) => this.mClientVersion = clientVersion;

        public virtual ProtocolVersion ServerVersion => this.mServerVersion;

        internal virtual void SetServerVersion( ProtocolVersion serverVersion ) => this.mServerVersion = serverVersion;

        public virtual TlsSession ResumableSession => this.mSession;

        internal virtual void SetResumableSession( TlsSession session ) => this.mSession = session;

        public virtual object UserObject
        {
            get => this.mUserObject;
            set => this.mUserObject = value;
        }

        public virtual byte[] ExportKeyingMaterial( string asciiLabel, byte[] context_value, int length )
        {
            if (context_value != null && !TlsUtilities.IsValidUint16( context_value.Length ))
                throw new ArgumentException( "must have length less than 2^16 (or be null)", nameof( context_value ) );
            SecurityParameters securityParameters = this.SecurityParameters;
            byte[] clientRandom = securityParameters.ClientRandom;
            byte[] serverRandom = securityParameters.ServerRandom;
            int length1 = clientRandom.Length + serverRandom.Length;
            if (context_value != null)
                length1 += 2 + context_value.Length;
            byte[] numArray = new byte[length1];
            int destinationIndex1 = 0;
            Array.Copy( clientRandom, 0, numArray, destinationIndex1, clientRandom.Length );
            int destinationIndex2 = destinationIndex1 + clientRandom.Length;
            Array.Copy( serverRandom, 0, numArray, destinationIndex2, serverRandom.Length );
            int offset = destinationIndex2 + serverRandom.Length;
            if (context_value != null)
            {
                TlsUtilities.WriteUint16( context_value.Length, numArray, offset );
                int destinationIndex3 = offset + 2;
                Array.Copy( context_value, 0, numArray, destinationIndex3, context_value.Length );
                offset = destinationIndex3 + context_value.Length;
            }
            if (offset != length1)
                throw new InvalidOperationException( "error in calculation of seed for export" );
            return TlsUtilities.PRF( this, securityParameters.MasterSecret, asciiLabel, numArray, length );
        }
    }
}
