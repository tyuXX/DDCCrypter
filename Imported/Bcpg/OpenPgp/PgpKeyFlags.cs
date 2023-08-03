// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpKeyFlags
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public abstract class PgpKeyFlags
    {
        public const int CanCertify = 1;
        public const int CanSign = 2;
        public const int CanEncryptCommunications = 4;
        public const int CanEncryptStorage = 8;
        public const int MaybeSplit = 16;
        public const int MaybeShared = 128;
    }
}
