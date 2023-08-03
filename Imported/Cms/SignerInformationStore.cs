// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SignerInformationStore
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Cms
{
    public class SignerInformationStore
    {
        private readonly IList all;
        private readonly IDictionary table = Platform.CreateHashtable();

        public SignerInformationStore( SignerInformation signerInfo )
        {
            this.all = Platform.CreateArrayList( 1 );
            this.all.Add( signerInfo );
            this.table[signerInfo.SignerID] = all;
        }

        public SignerInformationStore( ICollection signerInfos )
        {
            foreach (SignerInformation signerInfo in (IEnumerable)signerInfos)
            {
                SignerID signerId = signerInfo.SignerID;
                IList arrayList = (IList)this.table[signerId];
                if (arrayList == null)
                    this.table[signerId] = arrayList = Platform.CreateArrayList( 1 );
                arrayList.Add( signerInfo );
            }
            this.all = Platform.CreateArrayList( signerInfos );
        }

        public SignerInformation GetFirstSigner( SignerID selector )
        {
            IList list = (IList)this.table[selector];
            return list != null ? (SignerInformation)list[0] : null;
        }

        public int Count => this.all.Count;

        public ICollection GetSigners() => Platform.CreateArrayList( all );

        public ICollection GetSigners( SignerID selector )
        {
            IList list = (IList)this.table[selector];
            return list != null ? Platform.CreateArrayList( list ) : (ICollection)Platform.CreateArrayList();
        }
    }
}
