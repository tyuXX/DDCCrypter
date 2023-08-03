// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SupplementalDataEntry
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public class SupplementalDataEntry
    {
        protected readonly int mDataType;
        protected readonly byte[] mData;

        public SupplementalDataEntry( int dataType, byte[] data )
        {
            this.mDataType = dataType;
            this.mData = data;
        }

        public virtual int DataType => this.mDataType;

        public virtual byte[] Data => this.mData;
    }
}
