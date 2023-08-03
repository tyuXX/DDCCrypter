// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.RC4Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class RC4Engine : IStreamCipher
    {
        private static readonly int STATE_LENGTH = 256;
        private byte[] engineState;
        private int x;
        private int y;
        private byte[] workingKey;

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.workingKey = parameters is KeyParameter ? ((KeyParameter)parameters).GetKey() : throw new ArgumentException( "invalid parameter passed to RC4 init - " + Platform.GetTypeName( parameters ) );
            this.SetKey( this.workingKey );
        }

        public virtual string AlgorithmName => "RC4";

        public virtual byte ReturnByte( byte input )
        {
            this.x = (this.x + 1) & byte.MaxValue;
            this.y = (this.engineState[this.x] + this.y) & byte.MaxValue;
            byte num = this.engineState[this.x];
            this.engineState[this.x] = this.engineState[this.y];
            this.engineState[this.y] = num;
            return (byte)(input ^ (uint)this.engineState[(this.engineState[this.x] + this.engineState[this.y]) & byte.MaxValue]);
        }

        public virtual void ProcessBytes(
          byte[] input,
          int inOff,
          int length,
          byte[] output,
          int outOff )
        {
            Check.DataLength( input, inOff, length, "input buffer too short" );
            Check.OutputLength( output, outOff, length, "output buffer too short" );
            for (int index = 0; index < length; ++index)
            {
                this.x = (this.x + 1) & byte.MaxValue;
                this.y = (this.engineState[this.x] + this.y) & byte.MaxValue;
                byte num = this.engineState[this.x];
                this.engineState[this.x] = this.engineState[this.y];
                this.engineState[this.y] = num;
                output[index + outOff] = (byte)(input[index + inOff] ^ (uint)this.engineState[(this.engineState[this.x] + this.engineState[this.y]) & byte.MaxValue]);
            }
        }

        public virtual void Reset() => this.SetKey( this.workingKey );

        private void SetKey( byte[] keyBytes )
        {
            this.workingKey = keyBytes;
            this.x = 0;
            this.y = 0;
            if (this.engineState == null)
                this.engineState = new byte[STATE_LENGTH];
            for (int index = 0; index < STATE_LENGTH; ++index)
                this.engineState[index] = (byte)index;
            int index1 = 0;
            int index2 = 0;
            for (int index3 = 0; index3 < STATE_LENGTH; ++index3)
            {
                index2 = ((keyBytes[index1] & byte.MaxValue) + this.engineState[index3] + index2) & byte.MaxValue;
                byte num = this.engineState[index3];
                this.engineState[index3] = this.engineState[index2];
                this.engineState[index2] = num;
                index1 = (index1 + 1) % keyBytes.Length;
            }
        }
    }
}
