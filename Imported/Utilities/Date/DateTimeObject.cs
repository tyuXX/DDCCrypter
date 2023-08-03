// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Date.DateTimeObject
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Utilities.Date
{
    public sealed class DateTimeObject
    {
        private readonly DateTime dt;

        public DateTimeObject( DateTime dt ) => this.dt = dt;

        public DateTime Value => this.dt;

        public override string ToString() => this.dt.ToString();
    }
}
