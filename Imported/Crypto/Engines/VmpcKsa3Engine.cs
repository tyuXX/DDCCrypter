// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.VmpcKsa3Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Engines
{
    public class VmpcKsa3Engine : VmpcEngine
    {
        public override string AlgorithmName => "VMPC-KSA3";

        protected override void InitKey( byte[] keyBytes, byte[] ivBytes )
        {
            this.s = 0;
            this.P = new byte[256];
            for (int index = 0; index < 256; ++index)
                this.P[index] = (byte)index;
            for (int index = 0; index < 768; ++index)
            {
                this.s = this.P[(s + this.P[index & byte.MaxValue] + keyBytes[index % keyBytes.Length]) & byte.MaxValue];
                byte num = this.P[index & byte.MaxValue];
                this.P[index & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num;
            }
            for (int index = 0; index < 768; ++index)
            {
                this.s = this.P[(s + this.P[index & byte.MaxValue] + ivBytes[index % ivBytes.Length]) & byte.MaxValue];
                byte num = this.P[index & byte.MaxValue];
                this.P[index & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num;
            }
            for (int index = 0; index < 768; ++index)
            {
                this.s = this.P[(s + this.P[index & byte.MaxValue] + keyBytes[index % keyBytes.Length]) & byte.MaxValue];
                byte num = this.P[index & byte.MaxValue];
                this.P[index & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num;
            }
            this.n = 0;
        }
    }
}
