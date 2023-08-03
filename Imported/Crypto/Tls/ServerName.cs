// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ServerName
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ServerName
    {
        protected readonly byte mNameType;
        protected readonly object mName;

        public ServerName( byte nameType, object name )
        {
            this.mNameType = IsCorrectType( nameType, name ) ? nameType : throw new ArgumentException( "not an instance of the correct type", nameof( name ) );
            this.mName = name;
        }

        public virtual byte NameType => this.mNameType;

        public virtual object Name => this.mName;

        public virtual string GetHostName() => IsCorrectType( 0, this.mName ) ? (string)this.mName : throw new InvalidOperationException( "'name' is not a HostName string" );

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteUint8( this.mNameType, output );
            if (this.mNameType != 0)
                throw new TlsFatalAlert( 80 );
            byte[] asciiByteArray = Strings.ToAsciiByteArray( (string)this.mName );
            if (asciiByteArray.Length < 1)
                throw new TlsFatalAlert( 80 );
            TlsUtilities.WriteOpaque16( asciiByteArray, output );
        }

        public static ServerName Parse( Stream input )
        {
            byte nameType = TlsUtilities.ReadUint8( input );
            if (nameType != 0)
                throw new TlsFatalAlert( 50 );
            byte[] bytes = TlsUtilities.ReadOpaque16( input );
            object name = bytes.Length >= 1 ? (object)Strings.FromAsciiByteArray( bytes ) : throw new TlsFatalAlert( 50 );
            return new ServerName( nameType, name );
        }

        protected static bool IsCorrectType( byte nameType, object name )
        {
            if (nameType == 0)
                return name is string;
            throw new ArgumentException( "unsupported value", nameof( name ) );
        }
    }
}
