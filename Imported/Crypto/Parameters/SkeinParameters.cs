// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.SkeinParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.Globalization;
using System.IO;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class SkeinParameters : ICipherParameters
    {
        public const int PARAM_TYPE_KEY = 0;
        public const int PARAM_TYPE_CONFIG = 4;
        public const int PARAM_TYPE_PERSONALISATION = 8;
        public const int PARAM_TYPE_PUBLIC_KEY = 12;
        public const int PARAM_TYPE_KEY_IDENTIFIER = 16;
        public const int PARAM_TYPE_NONCE = 20;
        public const int PARAM_TYPE_MESSAGE = 48;
        public const int PARAM_TYPE_OUTPUT = 63;
        private IDictionary parameters;

        public SkeinParameters()
          : this( Platform.CreateHashtable() )
        {
        }

        private SkeinParameters( IDictionary parameters ) => this.parameters = parameters;

        public IDictionary GetParameters() => this.parameters;

        public byte[] GetKey() => (byte[])this.parameters[0];

        public byte[] GetPersonalisation() => (byte[])this.parameters[8];

        public byte[] GetPublicKey() => (byte[])this.parameters[12];

        public byte[] GetKeyIdentifier() => (byte[])this.parameters[16];

        public byte[] GetNonce() => (byte[])this.parameters[20];

        public class Builder
        {
            private IDictionary parameters = Platform.CreateHashtable();

            public Builder()
            {
            }

            public Builder( IDictionary paramsMap )
            {
                foreach (int key in (IEnumerable)paramsMap.Keys)
                    this.parameters.Add( key, paramsMap[key] );
            }

            public Builder( SkeinParameters parameters )
            {
                foreach (int key in (IEnumerable)parameters.parameters.Keys)
                    this.parameters.Add( key, parameters.parameters[key] );
            }

            public SkeinParameters.Builder Set( int type, byte[] value )
            {
                if (value == null)
                    throw new ArgumentException( "Parameter value must not be null." );
                if (type != 0 && (type <= 4 || type >= 63 || type == 48))
                    throw new ArgumentException( "Parameter types must be in the range 0,5..47,49..62." );
                if (type == 4)
                    throw new ArgumentException( "Parameter type " + 4 + " is reserved for internal use." );
                this.parameters.Add( type, value );
                return this;
            }

            public SkeinParameters.Builder SetKey( byte[] key ) => this.Set( 0, key );

            public SkeinParameters.Builder SetPersonalisation( byte[] personalisation ) => this.Set( 8, personalisation );

            public SkeinParameters.Builder SetPersonalisation(
              DateTime date,
              string emailAddress,
              string distinguisher )
            {
                try
                {
                    MemoryStream memoryStream = new();
                    StreamWriter t = new( memoryStream, Encoding.UTF8 );
                    t.Write( date.ToString( "YYYYMMDD", CultureInfo.InvariantCulture ) );
                    t.Write( " " );
                    t.Write( emailAddress );
                    t.Write( " " );
                    t.Write( distinguisher );
                    Platform.Dispose( t );
                    return this.Set( 8, memoryStream.ToArray() );
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException( "Byte I/O failed.", ex );
                }
            }

            public SkeinParameters.Builder SetPublicKey( byte[] publicKey ) => this.Set( 12, publicKey );

            public SkeinParameters.Builder SetKeyIdentifier( byte[] keyIdentifier ) => this.Set( 16, keyIdentifier );

            public SkeinParameters.Builder SetNonce( byte[] nonce ) => this.Set( 20, nonce );

            public SkeinParameters Build() => new( this.parameters );
        }
    }
}
