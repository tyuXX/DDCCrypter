// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.UrlAndHash
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class UrlAndHash
    {
        protected readonly string mUrl;
        protected readonly byte[] mSha1Hash;

        public UrlAndHash( string url, byte[] sha1Hash )
        {
            if (url == null || url.Length < 1 || url.Length >= 65536)
                throw new ArgumentException( "must have length from 1 to (2^16 - 1)", nameof( url ) );
            if (sha1Hash != null && sha1Hash.Length != 20)
                throw new ArgumentException( "must have length == 20, if present", nameof( sha1Hash ) );
            this.mUrl = url;
            this.mSha1Hash = sha1Hash;
        }

        public virtual string Url => this.mUrl;

        public virtual byte[] Sha1Hash => this.mSha1Hash;

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteOpaque16( Strings.ToByteArray( this.mUrl ), output );
            if (this.mSha1Hash == null)
            {
                TlsUtilities.WriteUint8( 0, output );
            }
            else
            {
                TlsUtilities.WriteUint8( 1, output );
                output.Write( this.mSha1Hash, 0, this.mSha1Hash.Length );
            }
        }

        public static UrlAndHash Parse( TlsContext context, Stream input )
        {
            byte[] bs = TlsUtilities.ReadOpaque16( input );
            string url = bs.Length >= 1 ? Strings.FromByteArray( bs ) : throw new TlsFatalAlert( 47 );
            byte[] sha1Hash = null;
            switch (TlsUtilities.ReadUint8( input ))
            {
                case 0:
                    if (TlsUtilities.IsTlsV12( context ))
                        throw new TlsFatalAlert( 47 );
                    break;
                case 1:
                    sha1Hash = TlsUtilities.ReadFully( 20, input );
                    break;
                default:
                    throw new TlsFatalAlert( 47 );
            }
            return new UrlAndHash( url, sha1Hash );
        }
    }
}
