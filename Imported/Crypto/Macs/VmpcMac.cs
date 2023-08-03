// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.VmpcMac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class VmpcMac : IMac
    {
        private byte g;
        private byte n = 0;
        private byte[] P = null;
        private byte s = 0;
        private byte[] T;
        private byte[] workingIV;
        private byte[] workingKey;
        private byte x1;
        private byte x2;
        private byte x3;
        private byte x4;

        public virtual int DoFinal( byte[] output, int outOff )
        {
            for (int index = 1; index < 25; ++index)
            {
                this.s = this.P[(s + this.P[n & byte.MaxValue]) & byte.MaxValue];
                this.x4 = this.P[(x4 + x3 + index) & byte.MaxValue];
                this.x3 = this.P[(x3 + x2 + index) & byte.MaxValue];
                this.x2 = this.P[(x2 + x1 + index) & byte.MaxValue];
                this.x1 = this.P[(x1 + s + index) & byte.MaxValue];
                this.T[g & 31] = (byte)(this.T[g & 31] ^ (uint)this.x1);
                this.T[(g + 1) & 31] = (byte)(this.T[(g + 1) & 31] ^ (uint)this.x2);
                this.T[(g + 2) & 31] = (byte)(this.T[(g + 2) & 31] ^ (uint)this.x3);
                this.T[(g + 3) & 31] = (byte)(this.T[(g + 3) & 31] ^ (uint)this.x4);
                this.g = (byte)((g + 4) & 31);
                byte num = this.P[n & byte.MaxValue];
                this.P[n & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num;
                this.n = (byte)((n + 1) & byte.MaxValue);
            }
            for (int index = 0; index < 768; ++index)
            {
                this.s = this.P[(s + this.P[index & byte.MaxValue] + this.T[index & 31]) & byte.MaxValue];
                byte num = this.P[index & byte.MaxValue];
                this.P[index & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num;
            }
            byte[] sourceArray = new byte[20];
            for (int index = 0; index < 20; ++index)
            {
                this.s = this.P[(s + this.P[index & byte.MaxValue]) & byte.MaxValue];
                sourceArray[index] = this.P[(this.P[this.P[s & byte.MaxValue] & byte.MaxValue] + 1) & byte.MaxValue];
                byte num = this.P[index & byte.MaxValue];
                this.P[index & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num;
            }
            Array.Copy( sourceArray, 0, output, outOff, sourceArray.Length );
            this.Reset();
            return sourceArray.Length;
        }

        public virtual string AlgorithmName => "VMPC-MAC";

        public virtual int GetMacSize() => 20;

        public virtual void Init( ICipherParameters parameters )
        {
            ParametersWithIV parametersWithIv = parameters is ParametersWithIV ? (ParametersWithIV)parameters : throw new ArgumentException( "VMPC-MAC Init parameters must include an IV", nameof( parameters ) );
            KeyParameter parameters1 = (KeyParameter)parametersWithIv.Parameters;
            this.workingIV = parametersWithIv.Parameters is KeyParameter ? parametersWithIv.GetIV() : throw new ArgumentException( "VMPC-MAC Init parameters must include a key", nameof( parameters ) );
            if (this.workingIV == null || this.workingIV.Length < 1 || this.workingIV.Length > 768)
                throw new ArgumentException( "VMPC-MAC requires 1 to 768 bytes of IV", nameof( parameters ) );
            this.workingKey = parameters1.GetKey();
            this.Reset();
        }

        private void initKey( byte[] keyBytes, byte[] ivBytes )
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
            this.n = 0;
        }

        public virtual void Reset()
        {
            this.initKey( this.workingKey, this.workingIV );
            this.g = this.x1 = this.x2 = this.x3 = this.x4 = this.n = 0;
            this.T = new byte[32];
            for (int index = 0; index < 32; ++index)
                this.T[index] = 0;
        }

        public virtual void Update( byte input )
        {
            this.s = this.P[(s + this.P[n & byte.MaxValue]) & byte.MaxValue];
            byte num1 = (byte)(input ^ (uint)this.P[(this.P[this.P[s & byte.MaxValue] & byte.MaxValue] + 1) & byte.MaxValue]);
            this.x4 = this.P[(x4 + x3) & byte.MaxValue];
            this.x3 = this.P[(x3 + x2) & byte.MaxValue];
            this.x2 = this.P[(x2 + x1) & byte.MaxValue];
            this.x1 = this.P[(x1 + s + num1) & byte.MaxValue];
            this.T[g & 31] = (byte)(this.T[g & 31] ^ (uint)this.x1);
            this.T[(g + 1) & 31] = (byte)(this.T[(g + 1) & 31] ^ (uint)this.x2);
            this.T[(g + 2) & 31] = (byte)(this.T[(g + 2) & 31] ^ (uint)this.x3);
            this.T[(g + 3) & 31] = (byte)(this.T[(g + 3) & 31] ^ (uint)this.x4);
            this.g = (byte)((g + 4) & 31);
            byte num2 = this.P[n & byte.MaxValue];
            this.P[n & byte.MaxValue] = this.P[s & byte.MaxValue];
            this.P[s & byte.MaxValue] = num2;
            this.n = (byte)((n + 1) & byte.MaxValue);
        }

        public virtual void BlockUpdate( byte[] input, int inOff, int len )
        {
            if (inOff + len > input.Length)
                throw new DataLengthException( "input buffer too short" );
            for (int index = 0; index < len; ++index)
                this.Update( input[inOff + index] );
        }
    }
}
