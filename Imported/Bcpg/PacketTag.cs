// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.PacketTag
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg
{
    public enum PacketTag
    {
        Reserved = 0,
        PublicKeyEncryptedSession = 1,
        Signature = 2,
        SymmetricKeyEncryptedSessionKey = 3,
        OnePassSignature = 4,
        SecretKey = 5,
        PublicKey = 6,
        SecretSubkey = 7,
        CompressedData = 8,
        SymmetricKeyEncrypted = 9,
        Marker = 10, // 0x0000000A
        LiteralData = 11, // 0x0000000B
        Trust = 12, // 0x0000000C
        UserId = 13, // 0x0000000D
        PublicSubkey = 14, // 0x0000000E
        UserAttribute = 17, // 0x00000011
        SymmetricEncryptedIntegrityProtected = 18, // 0x00000012
        ModificationDetectionCode = 19, // 0x00000013
        Experimental1 = 60, // 0x0000003C
        Experimental2 = 61, // 0x0000003D
        Experimental3 = 62, // 0x0000003E
        Experimental4 = 63, // 0x0000003F
    }
}
