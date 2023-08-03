// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ServerDHParams
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ServerDHParams
    {
        protected readonly DHPublicKeyParameters mPublicKey;

        public ServerDHParams( DHPublicKeyParameters publicKey ) => this.mPublicKey = publicKey != null ? publicKey : throw new ArgumentNullException( nameof( publicKey ) );

        public virtual DHPublicKeyParameters PublicKey => this.mPublicKey;

        public virtual void Encode( Stream output )
        {
            DHParameters parameters = this.mPublicKey.Parameters;
            BigInteger y = this.mPublicKey.Y;
            TlsDHUtilities.WriteDHParameter( parameters.P, output );
            TlsDHUtilities.WriteDHParameter( parameters.G, output );
            TlsDHUtilities.WriteDHParameter( y, output );
        }

        public static ServerDHParams Parse( Stream input )
        {
            BigInteger p = TlsDHUtilities.ReadDHParameter( input );
            BigInteger g = TlsDHUtilities.ReadDHParameter( input );
            return new ServerDHParams( TlsDHUtilities.ValidateDHPublicKey( new DHPublicKeyParameters( TlsDHUtilities.ReadDHParameter( input ), new DHParameters( p, g ) ) ) );
        }
    }
}
