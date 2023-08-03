// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpEncryptedDataList
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpEncryptedDataList : PgpObject
    {
        private IList list = Platform.CreateArrayList();
        private InputStreamPacket data;

        public PgpEncryptedDataList( BcpgInputStream bcpgInput )
        {
            while (bcpgInput.NextPacketTag() == PacketTag.PublicKeyEncryptedSession || bcpgInput.NextPacketTag() == PacketTag.SymmetricKeyEncryptedSessionKey)
                this.list.Add( bcpgInput.ReadPacket() );
            this.data = (InputStreamPacket)bcpgInput.ReadPacket();
            for (int index = 0; index != this.list.Count; ++index)
                this.list[index] = !(this.list[index] is SymmetricKeyEncSessionPacket) ? new PgpPublicKeyEncryptedData( (PublicKeyEncSessionPacket)this.list[index], this.data ) : (object)new PgpPbeEncryptedData( (SymmetricKeyEncSessionPacket)this.list[index], this.data );
        }

        public PgpEncryptedData this[int index] => (PgpEncryptedData)this.list[index];

        [Obsolete( "Use 'object[index]' syntax instead" )]
        public object Get( int index ) => this[index];

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.list.Count;

        public int Count => this.list.Count;

        public bool IsEmpty => this.list.Count == 0;

        public IEnumerable GetEncryptedDataObjects() => new EnumerableProxy( list );
    }
}
