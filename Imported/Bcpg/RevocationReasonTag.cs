// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.RevocationReasonTag
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public enum RevocationReasonTag : byte
    {
        NoReason = 0,
        KeySuperseded = 1,
        KeyCompromised = 2,
        KeyRetired = 3,
        UserNoLongerValid = 32, // 0x20
    }
}
