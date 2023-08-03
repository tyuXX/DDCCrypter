// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.DesEdeEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class DesEdeEngine : DesEngine
    {
        private int[] workingKey1;
        private int[] workingKey2;
        private int[] workingKey3;
        private bool forEncryption;

        public override void Init( bool forEncryption, ICipherParameters parameters )
        {
            byte[] sourceArray = parameters is KeyParameter ? ((KeyParameter)parameters).GetKey() : throw new ArgumentException( "invalid parameter passed to DESede init - " + Platform.GetTypeName( parameters ) );
            if (sourceArray.Length != 24 && sourceArray.Length != 16)
                throw new ArgumentException( "key size must be 16 or 24 bytes." );
            this.forEncryption = forEncryption;
            byte[] numArray1 = new byte[8];
            Array.Copy( sourceArray, 0, numArray1, 0, numArray1.Length );
            this.workingKey1 = GenerateWorkingKey( forEncryption, numArray1 );
            byte[] numArray2 = new byte[8];
            Array.Copy( sourceArray, 8, numArray2, 0, numArray2.Length );
            this.workingKey2 = GenerateWorkingKey( !forEncryption, numArray2 );
            if (sourceArray.Length == 24)
            {
                byte[] numArray3 = new byte[8];
                Array.Copy( sourceArray, 16, numArray3, 0, numArray3.Length );
                this.workingKey3 = GenerateWorkingKey( forEncryption, numArray3 );
            }
            else
                this.workingKey3 = this.workingKey1;
        }

        public override string AlgorithmName => "DESede";

        public override int GetBlockSize() => 8;

        public override int ProcessBlock( byte[] input, int inOff, byte[] output, int outOff )
        {
            if (this.workingKey1 == null)
                throw new InvalidOperationException( "DESede engine not initialised" );
            Check.DataLength( input, inOff, 8, "input buffer too short" );
            Check.OutputLength( output, outOff, 8, "output buffer too short" );
            byte[] numArray = new byte[8];
            if (this.forEncryption)
            {
                DesFunc( this.workingKey1, input, inOff, numArray, 0 );
                DesFunc( this.workingKey2, numArray, 0, numArray, 0 );
                DesFunc( this.workingKey3, numArray, 0, output, outOff );
            }
            else
            {
                DesFunc( this.workingKey3, input, inOff, numArray, 0 );
                DesFunc( this.workingKey2, numArray, 0, numArray, 0 );
                DesFunc( this.workingKey1, numArray, 0, output, outOff );
            }
            return 8;
        }

        public override void Reset()
        {
        }
    }
}
