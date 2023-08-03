// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.ServerNameList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class ServerNameList
    {
        protected readonly IList mServerNameList;

        public ServerNameList( IList serverNameList ) => this.mServerNameList = serverNameList != null && serverNameList.Count >= 1 ? serverNameList : throw new ArgumentException( "must not be null or empty", nameof( serverNameList ) );

        public virtual IList ServerNames => this.mServerNameList;

        public virtual void Encode( Stream output )
        {
            MemoryStream output1 = new();
            foreach (ServerName serverName in (IEnumerable)this.ServerNames)
                serverName.Encode( output1 );
            TlsUtilities.CheckUint16( output1.Length );
            TlsUtilities.WriteUint16( (int)output1.Length, output );
            output1.WriteTo( output );
        }

        public static ServerNameList Parse( Stream input )
        {
            int length = TlsUtilities.ReadUint16( input );
            MemoryStream input1 = length >= 1 ? new MemoryStream( TlsUtilities.ReadFully( length, input ), false ) : throw new TlsFatalAlert( 50 );
            IList arrayList = Platform.CreateArrayList();
            while (input1.Position < input1.Length)
            {
                ServerName serverName = ServerName.Parse( input1 );
                arrayList.Add( serverName );
            }
            return new ServerNameList( arrayList );
        }
    }
}
