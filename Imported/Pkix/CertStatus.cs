// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.CertStatus
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Date;

namespace Org.BouncyCastle.Pkix
{
    public class CertStatus
    {
        public const int Unrevoked = 11;
        public const int Undetermined = 12;
        private int status = 11;
        private DateTimeObject revocationDate = null;

        public DateTimeObject RevocationDate
        {
            get => this.revocationDate;
            set => this.revocationDate = value;
        }

        public int Status
        {
            get => this.status;
            set => this.status = value;
        }
    }
}
