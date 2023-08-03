// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ServerSrpParams
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ServerSrpParams
    {
        protected BigInteger m_N;
        protected BigInteger m_g;
        protected BigInteger m_B;
        protected byte[] m_s;

        public ServerSrpParams( BigInteger N, BigInteger g, byte[] s, BigInteger B )
        {
            this.m_N = N;
            this.m_g = g;
            this.m_s = Arrays.Clone( s );
            this.m_B = B;
        }

        public virtual BigInteger B => this.m_B;

        public virtual BigInteger G => this.m_g;

        public virtual BigInteger N => this.m_N;

        public virtual byte[] S => this.m_s;

        public virtual void Encode( Stream output )
        {
            TlsSrpUtilities.WriteSrpParameter( this.m_N, output );
            TlsSrpUtilities.WriteSrpParameter( this.m_g, output );
            TlsUtilities.WriteOpaque8( this.m_s, output );
            TlsSrpUtilities.WriteSrpParameter( this.m_B, output );
        }

        public static ServerSrpParams Parse( Stream input ) => new( TlsSrpUtilities.ReadSrpParameter( input ), TlsSrpUtilities.ReadSrpParameter( input ), TlsUtilities.ReadOpaque8( input ), TlsSrpUtilities.ReadSrpParameter( input ) );
    }
}
