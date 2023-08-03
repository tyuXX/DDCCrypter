// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.HeartbeatExtension
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class HeartbeatExtension
    {
        protected readonly byte mMode;

        public HeartbeatExtension( byte mode ) => this.mMode = HeartbeatMode.IsValid( mode ) ? mode : throw new ArgumentException( "not a valid HeartbeatMode value", nameof( mode ) );

        public virtual byte Mode => this.mMode;

        public virtual void Encode( Stream output ) => TlsUtilities.WriteUint8( this.mMode, output );

        public static HeartbeatExtension Parse( Stream input )
        {
            byte num = TlsUtilities.ReadUint8( input );
            return HeartbeatMode.IsValid( num ) ? new HeartbeatExtension( num ) : throw new TlsFatalAlert( 47 );
        }
    }
}
