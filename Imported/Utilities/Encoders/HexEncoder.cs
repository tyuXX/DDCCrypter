// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Encoders.HexEncoder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
    public class HexEncoder : IEncoder
    {
        protected readonly byte[] encodingTable = new byte[16]
        {
       48,
       49,
       50,
       51,
       52,
       53,
       54,
       55,
       56,
       57,
       97,
       98,
       99,
       100,
       101,
       102
        };
        protected readonly byte[] decodingTable = new byte[128];

        protected void InitialiseDecodingTable()
        {
            Arrays.Fill( this.decodingTable, byte.MaxValue );
            for (int index = 0; index < this.encodingTable.Length; ++index)
                this.decodingTable[this.encodingTable[index]] = (byte)index;
            this.decodingTable[65] = this.decodingTable[97];
            this.decodingTable[66] = this.decodingTable[98];
            this.decodingTable[67] = this.decodingTable[99];
            this.decodingTable[68] = this.decodingTable[100];
            this.decodingTable[69] = this.decodingTable[101];
            this.decodingTable[70] = this.decodingTable[102];
        }

        public HexEncoder() => this.InitialiseDecodingTable();

        public int Encode( byte[] data, int off, int length, Stream outStream )
        {
            for (int index = off; index < off + length; ++index)
            {
                int num = data[index];
                outStream.WriteByte( this.encodingTable[num >> 4] );
                outStream.WriteByte( this.encodingTable[num & 15] );
            }
            return length * 2;
        }

        private static bool Ignore( char c ) => c == '\n' || c == '\r' || c == '\t' || c == ' ';

        public int Decode( byte[] data, int off, int length, Stream outStream )
        {
            int num1 = 0;
            int num2 = off + length;
            while (num2 > off && Ignore( (char)data[num2 - 1] ))
                --num2;
            int index1 = off;
            while (index1 < num2)
            {
                while (index1 < num2 && Ignore( (char)data[index1] ))
                    ++index1;
                byte[] decodingTable1 = this.decodingTable;
                byte[] numArray1 = data;
                int index2 = index1;
                int index3 = index2 + 1;
                int index4 = numArray1[index2];
                byte num3 = decodingTable1[index4];
                while (index3 < num2 && Ignore( (char)data[index3] ))
                    ++index3;
                byte[] decodingTable2 = this.decodingTable;
                byte[] numArray2 = data;
                int index5 = index3;
                index1 = index5 + 1;
                int index6 = numArray2[index5];
                byte num4 = decodingTable2[index6];
                if ((num3 | num4) >= 128)
                    throw new IOException( "invalid characters encountered in Hex data" );
                outStream.WriteByte( (byte)(((uint)num3 << 4) | num4) );
                ++num1;
            }
            return num1;
        }

        public int DecodeString( string data, Stream outStream )
        {
            int num1 = 0;
            int length = data.Length;
            while (length > 0 && Ignore( data[length - 1] ))
                --length;
            int index1 = 0;
            while (index1 < length)
            {
                while (index1 < length && Ignore( data[index1] ))
                    ++index1;
                byte[] decodingTable1 = this.decodingTable;
                string str1 = data;
                int index2 = index1;
                int index3 = index2 + 1;
                int index4 = str1[index2];
                byte num2 = decodingTable1[index4];
                while (index3 < length && Ignore( data[index3] ))
                    ++index3;
                byte[] decodingTable2 = this.decodingTable;
                string str2 = data;
                int index5 = index3;
                index1 = index5 + 1;
                int index6 = str2[index5];
                byte num3 = decodingTable2[index6];
                if ((num2 | num3) >= 128)
                    throw new IOException( "invalid characters encountered in Hex data" );
                outStream.WriteByte( (byte)(((uint)num2 << 4) | num3) );
                ++num1;
            }
            return num1;
        }
    }
}
