// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SessionParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public sealed class SessionParameters
    {
        private int mCipherSuite;
        private byte mCompressionAlgorithm;
        private byte[] mMasterSecret;
        private Certificate mPeerCertificate;
        private byte[] mPskIdentity;
        private byte[] mSrpIdentity;
        private byte[] mEncodedServerExtensions;

        private SessionParameters(
          int cipherSuite,
          byte compressionAlgorithm,
          byte[] masterSecret,
          Certificate peerCertificate,
          byte[] pskIdentity,
          byte[] srpIdentity,
          byte[] encodedServerExtensions )
        {
            this.mCipherSuite = cipherSuite;
            this.mCompressionAlgorithm = compressionAlgorithm;
            this.mMasterSecret = Arrays.Clone( masterSecret );
            this.mPeerCertificate = peerCertificate;
            this.mPskIdentity = Arrays.Clone( pskIdentity );
            this.mSrpIdentity = Arrays.Clone( srpIdentity );
            this.mEncodedServerExtensions = encodedServerExtensions;
        }

        public void Clear()
        {
            if (this.mMasterSecret == null)
                return;
            Arrays.Fill( this.mMasterSecret, 0 );
        }

        public SessionParameters Copy() => new( this.mCipherSuite, this.mCompressionAlgorithm, this.mMasterSecret, this.mPeerCertificate, this.mPskIdentity, this.mSrpIdentity, this.mEncodedServerExtensions );

        public int CipherSuite => this.mCipherSuite;

        public byte CompressionAlgorithm => this.mCompressionAlgorithm;

        public byte[] MasterSecret => this.mMasterSecret;

        public Certificate PeerCertificate => this.mPeerCertificate;

        public byte[] PskIdentity => this.mPskIdentity;

        public byte[] SrpIdentity => this.mSrpIdentity;

        public IDictionary ReadServerExtensions() => this.mEncodedServerExtensions == null ? null : TlsProtocol.ReadExtensions( new MemoryStream( this.mEncodedServerExtensions, false ) );

        public sealed class Builder
        {
            private int mCipherSuite = -1;
            private short mCompressionAlgorithm = -1;
            private byte[] mMasterSecret = null;
            private Certificate mPeerCertificate = null;
            private byte[] mPskIdentity = null;
            private byte[] mSrpIdentity = null;
            private byte[] mEncodedServerExtensions = null;

            public SessionParameters Build()
            {
                this.Validate( this.mCipherSuite >= 0, "cipherSuite" );
                this.Validate( this.mCompressionAlgorithm >= 0, "compressionAlgorithm" );
                this.Validate( this.mMasterSecret != null, "masterSecret" );
                return new SessionParameters( this.mCipherSuite, (byte)this.mCompressionAlgorithm, this.mMasterSecret, this.mPeerCertificate, this.mPskIdentity, this.mSrpIdentity, this.mEncodedServerExtensions );
            }

            public SessionParameters.Builder SetCipherSuite( int cipherSuite )
            {
                this.mCipherSuite = cipherSuite;
                return this;
            }

            public SessionParameters.Builder SetCompressionAlgorithm( byte compressionAlgorithm )
            {
                this.mCompressionAlgorithm = compressionAlgorithm;
                return this;
            }

            public SessionParameters.Builder SetMasterSecret( byte[] masterSecret )
            {
                this.mMasterSecret = masterSecret;
                return this;
            }

            public SessionParameters.Builder SetPeerCertificate( Certificate peerCertificate )
            {
                this.mPeerCertificate = peerCertificate;
                return this;
            }

            public SessionParameters.Builder SetPskIdentity( byte[] pskIdentity )
            {
                this.mPskIdentity = pskIdentity;
                return this;
            }

            public SessionParameters.Builder SetSrpIdentity( byte[] srpIdentity )
            {
                this.mSrpIdentity = srpIdentity;
                return this;
            }

            public SessionParameters.Builder SetServerExtensions( IDictionary serverExtensions )
            {
                if (serverExtensions == null)
                {
                    this.mEncodedServerExtensions = null;
                }
                else
                {
                    MemoryStream output = new();
                    TlsProtocol.WriteExtensions( output, serverExtensions );
                    this.mEncodedServerExtensions = output.ToArray();
                }
                return this;
            }

            private void Validate( bool condition, string parameter )
            {
                if (!condition)
                    throw new InvalidOperationException( "Required session parameter '" + parameter + "' not configured" );
            }
        }
    }
}
