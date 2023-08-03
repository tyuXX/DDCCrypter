// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.XSalsa20Engine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class XSalsa20Engine : Salsa20Engine
    {
        public override string AlgorithmName => "XSalsa20";

        protected override int NonceSize => 24;

        protected override void SetKey( byte[] keyBytes, byte[] ivBytes )
        {
            if (keyBytes.Length != 32)
                throw new ArgumentException( this.AlgorithmName + " requires a 256 bit key" );
            base.SetKey( keyBytes, ivBytes );
            this.engineState[8] = Pack.LE_To_UInt32( ivBytes, 8 );
            this.engineState[9] = Pack.LE_To_UInt32( ivBytes, 12 );
            uint[] x = new uint[this.engineState.Length];
            SalsaCore( 20, this.engineState, x );
            this.engineState[1] = x[0] - this.engineState[0];
            this.engineState[2] = x[5] - this.engineState[5];
            this.engineState[3] = x[10] - this.engineState[10];
            this.engineState[4] = x[15] - this.engineState[15];
            this.engineState[11] = x[6] - this.engineState[6];
            this.engineState[12] = x[7] - this.engineState[7];
            this.engineState[13] = x[8] - this.engineState[8];
            this.engineState[14] = x[9] - this.engineState[9];
            this.engineState[6] = Pack.LE_To_UInt32( ivBytes, 16 );
            this.engineState[7] = Pack.LE_To_UInt32( ivBytes, 20 );
            this.ResetCounter();
        }
    }
}
