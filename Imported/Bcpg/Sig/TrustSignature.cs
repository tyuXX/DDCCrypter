﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.TrustSignature
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class TrustSignature : SignatureSubpacket
    {
        private static byte[] IntToByteArray( int v1, int v2 ) => new byte[2]
        {
      (byte) v1,
      (byte) v2
        };

        public TrustSignature( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.TrustSig, critical, isLongLength, data )
        {
        }

        public TrustSignature( bool critical, int depth, int trustAmount )
          : base( SignatureSubpacketTag.TrustSig, critical, false, IntToByteArray( depth, trustAmount ) )
        {
        }

        public int Depth => this.data[0] & byte.MaxValue;

        public int TrustAmount => this.data[1] & byte.MaxValue;
    }
}