// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Macs.Gost28147Mac
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
    public class Gost28147Mac : IMac
    {
        private const int blockSize = 8;
        private const int macSize = 4;
        private int bufOff;
        private byte[] buf;
        private byte[] mac;
        private bool firstStep = true;
        private int[] workingKey;
        private byte[] S = new byte[128]
        {
       9,
       6,
       3,
       2,
       8,
       11,
       1,
       7,
       10,
       4,
       14,
       15,
       12,
       0,
       13,
       5,
       3,
       7,
       14,
       9,
       8,
       10,
       15,
       0,
       5,
       2,
       6,
       12,
       11,
       4,
       13,
       1,
       14,
       4,
       6,
       2,
       11,
       3,
       13,
       8,
       12,
       15,
       5,
       10,
       0,
       7,
       1,
       9,
       14,
       7,
       10,
       12,
       13,
       1,
       3,
       9,
       0,
       2,
       11,
       4,
       15,
       8,
       5,
       6,
       11,
       5,
       1,
       9,
       8,
       13,
       15,
       0,
       14,
       4,
       2,
       3,
       12,
       7,
       10,
       6,
       3,
       10,
       13,
       12,
       1,
       2,
       0,
       11,
       7,
       5,
       9,
       4,
       8,
       15,
       14,
       6,
       1,
       13,
       2,
       9,
       7,
       10,
       6,
       0,
       8,
       12,
       4,
       5,
       15,
       3,
       11,
       14,
       11,
       10,
       15,
       5,
       0,
       12,
       14,
       8,
       6,
       2,
       3,
       9,
       1,
       7,
       13,
       4
        };

        public Gost28147Mac()
        {
            this.mac = new byte[8];
            this.buf = new byte[8];
            this.bufOff = 0;
        }

        private static int[] generateWorkingKey( byte[] userKey )
        {
            if (userKey.Length != 32)
                throw new ArgumentException( "Key length invalid. Key needs to be 32 byte - 256 bit!!!" );
            int[] workingKey = new int[8];
            for (int index = 0; index != 8; ++index)
                workingKey[index] = bytesToint( userKey, index * 4 );
            return workingKey;
        }

        public void Init( ICipherParameters parameters )
        {
            this.Reset();
            this.buf = new byte[8];
            switch (parameters)
            {
                case ParametersWithSBox _:
                    ParametersWithSBox parametersWithSbox = (ParametersWithSBox)parameters;
                    parametersWithSbox.GetSBox().CopyTo( S, 0 );
                    if (parametersWithSbox.Parameters == null)
                        break;
                    this.workingKey = generateWorkingKey( ((KeyParameter)parametersWithSbox.Parameters).GetKey() );
                    break;
                case KeyParameter _:
                    this.workingKey = generateWorkingKey( ((KeyParameter)parameters).GetKey() );
                    break;
                default:
                    throw new ArgumentException( "invalid parameter passed to Gost28147 init - " + Platform.GetTypeName( parameters ) );
            }
        }

        public string AlgorithmName => nameof( Gost28147Mac );

        public int GetMacSize() => 4;

        private int gost28147_mainStep( int n1, int key )
        {
            int num1 = key + n1;
            int num2 = this.S[num1 & 15] + (this.S[16 + ((num1 >> 4) & 15)] << 4) + (this.S[32 + ((num1 >> 8) & 15)] << 8) + (this.S[48 + ((num1 >> 12) & 15)] << 12) + (this.S[64 + ((num1 >> 16) & 15)] << 16) + (this.S[80 + ((num1 >> 20) & 15)] << 20) + (this.S[96 + ((num1 >> 24) & 15)] << 24) + (this.S[112 + ((num1 >> 28) & 15)] << 28);
            return (num2 << 11) | num2 >>> 21;
        }

        private void gost28147MacFunc(
          int[] workingKey,
          byte[] input,
          int inOff,
          byte[] output,
          int outOff )
        {
            int num1 = bytesToint( input, inOff );
            int num2 = bytesToint( input, inOff + 4 );
            for (int index1 = 0; index1 < 2; ++index1)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    int num3 = num1;
                    num1 = num2 ^ this.gost28147_mainStep( num1, workingKey[index2] );
                    num2 = num3;
                }
            }
            intTobytes( num1, output, outOff );
            intTobytes( num2, output, outOff + 4 );
        }

        private static int bytesToint( byte[] input, int inOff ) => (int)(input[inOff + 3] << 24 & 4278190080L) + ((input[inOff + 2] << 16) & 16711680) + ((input[inOff + 1] << 8) & 65280) + (input[inOff] & byte.MaxValue);

        private static void intTobytes( int num, byte[] output, int outOff )
        {
            output[outOff + 3] = (byte)(num >> 24);
            output[outOff + 2] = (byte)(num >> 16);
            output[outOff + 1] = (byte)(num >> 8);
            output[outOff] = (byte)num;
        }

        private static byte[] CM5func( byte[] buf, int bufOff, byte[] mac )
        {
            byte[] destinationArray = new byte[buf.Length - bufOff];
            Array.Copy( buf, bufOff, destinationArray, 0, mac.Length );
            for (int index = 0; index != mac.Length; ++index)
                destinationArray[index] = (byte)(destinationArray[index] ^ (uint)mac[index]);
            return destinationArray;
        }

        public void Update( byte input )
        {
            if (this.bufOff == this.buf.Length)
            {
                byte[] numArray = new byte[this.buf.Length];
                Array.Copy( buf, 0, numArray, 0, this.mac.Length );
                if (this.firstStep)
                    this.firstStep = false;
                else
                    numArray = CM5func( this.buf, 0, this.mac );
                this.gost28147MacFunc( this.workingKey, numArray, 0, this.mac, 0 );
                this.bufOff = 0;
            }
            this.buf[this.bufOff++] = input;
        }

        public void BlockUpdate( byte[] input, int inOff, int len )
        {
            if (len < 0)
                throw new ArgumentException( "Can't have a negative input length!" );
            int length = 8 - this.bufOff;
            if (len > length)
            {
                Array.Copy( input, inOff, buf, this.bufOff, length );
                byte[] numArray = new byte[this.buf.Length];
                Array.Copy( buf, 0, numArray, 0, this.mac.Length );
                if (this.firstStep)
                    this.firstStep = false;
                else
                    numArray = CM5func( this.buf, 0, this.mac );
                this.gost28147MacFunc( this.workingKey, numArray, 0, this.mac, 0 );
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > 8)
                {
                    this.gost28147MacFunc( this.workingKey, CM5func( input, inOff, this.mac ), 0, this.mac, 0 );
                    len -= 8;
                    inOff += 8;
                }
            }
            Array.Copy( input, inOff, buf, this.bufOff, len );
            this.bufOff += len;
        }

        public int DoFinal( byte[] output, int outOff )
        {
            while (this.bufOff < 8)
                this.buf[this.bufOff++] = 0;
            byte[] numArray = new byte[this.buf.Length];
            Array.Copy( buf, 0, numArray, 0, this.mac.Length );
            if (this.firstStep)
                this.firstStep = false;
            else
                numArray = CM5func( this.buf, 0, this.mac );
            this.gost28147MacFunc( this.workingKey, numArray, 0, this.mac, 0 );
            Array.Copy( mac, (this.mac.Length / 2) - 4, output, outOff, 4 );
            this.Reset();
            return 4;
        }

        public void Reset()
        {
            Array.Clear( buf, 0, this.buf.Length );
            this.bufOff = 0;
            this.firstStep = true;
        }
    }
}
