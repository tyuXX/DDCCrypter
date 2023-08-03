// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.VmpcEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class VmpcEngine : IStreamCipher
    {
        protected byte n = 0;
        protected byte[] P = null;
        protected byte s = 0;
        protected byte[] workingIV;
        protected byte[] workingKey;

        public virtual string AlgorithmName => "VMPC";

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            ParametersWithIV parametersWithIv = parameters is ParametersWithIV ? (ParametersWithIV)parameters : throw new ArgumentException( "VMPC Init parameters must include an IV" );
            KeyParameter keyParameter = parametersWithIv.Parameters is KeyParameter ? (KeyParameter)parametersWithIv.Parameters : throw new ArgumentException( "VMPC Init parameters must include a key" );
            this.workingIV = parametersWithIv.GetIV();
            if (this.workingIV == null || this.workingIV.Length < 1 || this.workingIV.Length > 768)
                throw new ArgumentException( "VMPC requires 1 to 768 bytes of IV" );
            this.workingKey = keyParameter.GetKey();
            this.InitKey( this.workingKey, this.workingIV );
        }

        protected virtual void InitKey( byte[] keyBytes, byte[] ivBytes )
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

        public virtual void ProcessBytes( byte[] input, int inOff, int len, byte[] output, int outOff )
        {
            Check.DataLength( input, inOff, len, "input buffer too short" );
            Check.OutputLength( output, outOff, len, "output buffer too short" );
            for (int index = 0; index < len; ++index)
            {
                this.s = this.P[(s + this.P[n & byte.MaxValue]) & byte.MaxValue];
                byte num1 = this.P[(this.P[this.P[s & byte.MaxValue] & byte.MaxValue] + 1) & byte.MaxValue];
                byte num2 = this.P[n & byte.MaxValue];
                this.P[n & byte.MaxValue] = this.P[s & byte.MaxValue];
                this.P[s & byte.MaxValue] = num2;
                this.n = (byte)((n + 1) & byte.MaxValue);
                output[index + outOff] = (byte)(input[index + inOff] ^ (uint)num1);
            }
        }

        public virtual void Reset() => this.InitKey( this.workingKey, this.workingIV );

        public virtual byte ReturnByte( byte input )
        {
            this.s = this.P[(s + this.P[n & byte.MaxValue]) & byte.MaxValue];
            byte num1 = this.P[(this.P[this.P[s & byte.MaxValue] & byte.MaxValue] + 1) & byte.MaxValue];
            byte num2 = this.P[n & byte.MaxValue];
            this.P[n & byte.MaxValue] = this.P[s & byte.MaxValue];
            this.P[s & byte.MaxValue] = num2;
            this.n = (byte)((n + 1) & byte.MaxValue);
            return (byte)(input ^ (uint)num1);
        }
    }
}
