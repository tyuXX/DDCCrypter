// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.RecipientInformationStore
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class RecipientInformationStore
    {
        private readonly IList all;
        private readonly IDictionary table = Platform.CreateHashtable();

        public RecipientInformationStore( ICollection recipientInfos )
        {
            foreach (RecipientInformation recipientInfo in (IEnumerable)recipientInfos)
            {
                RecipientID recipientId = recipientInfo.RecipientID;
                IList arrayList = (IList)this.table[recipientId];
                if (arrayList == null)
                    this.table[recipientId] = arrayList = Platform.CreateArrayList( 1 );
                arrayList.Add( recipientInfo );
            }
            this.all = Platform.CreateArrayList( recipientInfos );
        }

        public RecipientInformation this[RecipientID selector] => this.GetFirstRecipient( selector );

        public RecipientInformation GetFirstRecipient( RecipientID selector )
        {
            IList list = (IList)this.table[selector];
            return list != null ? (RecipientInformation)list[0] : null;
        }

        public int Count => this.all.Count;

        public ICollection GetRecipients() => Platform.CreateArrayList( all );

        public ICollection GetRecipients( RecipientID selector )
        {
            IList list = (IList)this.table[selector];
            return list != null ? Platform.CreateArrayList( list ) : (ICollection)Platform.CreateArrayList();
        }
    }
}
