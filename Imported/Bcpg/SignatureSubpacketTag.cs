// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.SignatureSubpacketTag
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public enum SignatureSubpacketTag
    {
        CreationTime = 2,
        ExpireTime = 3,
        Exportable = 4,
        TrustSig = 5,
        RegExp = 6,
        Revocable = 7,
        KeyExpireTime = 9,
        Placeholder = 10, // 0x0000000A
        PreferredSymmetricAlgorithms = 11, // 0x0000000B
        RevocationKey = 12, // 0x0000000C
        IssuerKeyId = 16, // 0x00000010
        NotationData = 20, // 0x00000014
        PreferredHashAlgorithms = 21, // 0x00000015
        PreferredCompressionAlgorithms = 22, // 0x00000016
        KeyServerPreferences = 23, // 0x00000017
        PreferredKeyServer = 24, // 0x00000018
        PrimaryUserId = 25, // 0x00000019
        PolicyUrl = 26, // 0x0000001A
        KeyFlags = 27, // 0x0000001B
        SignerUserId = 28, // 0x0000001C
        RevocationReason = 29, // 0x0000001D
        Features = 30, // 0x0000001E
        SignatureTarget = 31, // 0x0000001F
        EmbeddedSignature = 32, // 0x00000020
    }
}
