// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Encoders.Base64Encoder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.Encoders
{
    public class Base64Encoder : IEncoder
    {
        protected readonly byte[] encodingTable = new byte[64]
        {
       65,
       66,
       67,
       68,
       69,
       70,
       71,
       72,
       73,
       74,
       75,
       76,
       77,
       78,
       79,
       80,
       81,
       82,
       83,
       84,
       85,
       86,
       87,
       88,
       89,
       90,
       97,
       98,
       99,
       100,
       101,
       102,
       103,
       104,
       105,
       106,
       107,
       108,
       109,
       110,
       111,
       112,
       113,
       114,
       115,
       116,
       117,
       118,
       119,
       120,
       121,
       122,
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
       43,
       47
        };
        protected byte padding = 61;
        protected readonly byte[] decodingTable = new byte[128];

        protected void InitialiseDecodingTable()
        {
            Arrays.Fill( this.decodingTable, byte.MaxValue );
            for (int index = 0; index < this.encodingTable.Length; ++index)
                this.decodingTable[this.encodingTable[index]] = (byte)index;
        }

        public Base64Encoder() => this.InitialiseDecodingTable();

        public int Encode( byte[] data, int off, int length, Stream outStream )
        {
            int num1 = length % 3;
            int num2 = length - num1;
            for (int index = off; index < off + num2; index += 3)
            {
                int num3 = data[index] & byte.MaxValue;
                int num4 = data[index + 1] & byte.MaxValue;
                int num5 = data[index + 2] & byte.MaxValue;
                outStream.WriteByte( this.encodingTable[num3 >>> 2 & 63] );
                outStream.WriteByte( this.encodingTable[((num3 << 4) | num4 >>> 4) & 63] );
                outStream.WriteByte( this.encodingTable[((num4 << 2) | num5 >>> 6) & 63] );
                outStream.WriteByte( this.encodingTable[num5 & 63] );
            }
            switch (num1)
            {
                case 1:
                    int num6 = data[off + num2] & byte.MaxValue;
                    int index1 = (num6 >> 2) & 63;
                    int index2 = (num6 << 4) & 63;
                    outStream.WriteByte( this.encodingTable[index1] );
                    outStream.WriteByte( this.encodingTable[index2] );
                    outStream.WriteByte( this.padding );
                    outStream.WriteByte( this.padding );
                    break;
                case 2:
                    int num7 = data[off + num2] & byte.MaxValue;
                    int num8 = data[off + num2 + 1] & byte.MaxValue;
                    int index3 = (num7 >> 2) & 63;
                    int index4 = ((num7 << 4) | (num8 >> 4)) & 63;
                    int index5 = (num8 << 2) & 63;
                    outStream.WriteByte( this.encodingTable[index3] );
                    outStream.WriteByte( this.encodingTable[index4] );
                    outStream.WriteByte( this.encodingTable[index5] );
                    outStream.WriteByte( this.padding );
                    break;
            }
            return (num2 / 3 * 4) + (num1 == 0 ? 0 : 4);
        }

        private bool ignore( char c ) => c == '\n' || c == '\r' || c == '\t' || c == ' ';

        public int Decode( byte[] data, int off, int length, Stream outStream )
        {
            int num1 = 0;
            int num2 = off + length;
            while (num2 > off && this.ignore( (char)data[num2 - 1] ))
                --num2;
            int i1 = off;
            int finish = num2 - 4;
            int i2;
            for (int index1 = this.nextI( data, i1, finish ); index1 < finish; index1 = this.nextI( data, i2, finish ))
            {
                byte[] decodingTable1 = this.decodingTable;
                byte[] numArray1 = data;
                int index2 = index1;
                int i3 = index2 + 1;
                int index3 = numArray1[index2];
                byte num3 = decodingTable1[index3];
                int num4 = this.nextI( data, i3, finish );
                byte[] decodingTable2 = this.decodingTable;
                byte[] numArray2 = data;
                int index4 = num4;
                int i4 = index4 + 1;
                int index5 = numArray2[index4];
                byte num5 = decodingTable2[index5];
                int num6 = this.nextI( data, i4, finish );
                byte[] decodingTable3 = this.decodingTable;
                byte[] numArray3 = data;
                int index6 = num6;
                int i5 = index6 + 1;
                int index7 = numArray3[index6];
                byte num7 = decodingTable3[index7];
                int num8 = this.nextI( data, i5, finish );
                byte[] decodingTable4 = this.decodingTable;
                byte[] numArray4 = data;
                int index8 = num8;
                i2 = index8 + 1;
                int index9 = numArray4[index8];
                byte num9 = decodingTable4[index9];
                if ((num3 | num5 | num7 | num9) >= 128)
                    throw new IOException( "invalid characters encountered in base64 data" );
                outStream.WriteByte( (byte)((num3 << 2) | (num5 >> 4)) );
                outStream.WriteByte( (byte)((num5 << 4) | (num7 >> 2)) );
                outStream.WriteByte( (byte)(((uint)num7 << 6) | num9) );
                num1 += 3;
            }
            return num1 + this.decodeLastBlock( outStream, (char)data[num2 - 4], (char)data[num2 - 3], (char)data[num2 - 2], (char)data[num2 - 1] );
        }

        private int nextI( byte[] data, int i, int finish )
        {
            while (i < finish && this.ignore( (char)data[i] ))
                ++i;
            return i;
        }

        public int DecodeString( string data, Stream outStream )
        {
            int num1 = 0;
            int length = data.Length;
            while (length > 0 && this.ignore( data[length - 1] ))
                --length;
            int i1 = 0;
            int finish = length - 4;
            int i2;
            for (int index1 = this.nextI( data, i1, finish ); index1 < finish; index1 = this.nextI( data, i2, finish ))
            {
                byte[] decodingTable1 = this.decodingTable;
                string str1 = data;
                int index2 = index1;
                int i3 = index2 + 1;
                int index3 = str1[index2];
                byte num2 = decodingTable1[index3];
                int num3 = this.nextI( data, i3, finish );
                byte[] decodingTable2 = this.decodingTable;
                string str2 = data;
                int index4 = num3;
                int i4 = index4 + 1;
                int index5 = str2[index4];
                byte num4 = decodingTable2[index5];
                int num5 = this.nextI( data, i4, finish );
                byte[] decodingTable3 = this.decodingTable;
                string str3 = data;
                int index6 = num5;
                int i5 = index6 + 1;
                int index7 = str3[index6];
                byte num6 = decodingTable3[index7];
                int num7 = this.nextI( data, i5, finish );
                byte[] decodingTable4 = this.decodingTable;
                string str4 = data;
                int index8 = num7;
                i2 = index8 + 1;
                int index9 = str4[index8];
                byte num8 = decodingTable4[index9];
                if ((num2 | num4 | num6 | num8) >= 128)
                    throw new IOException( "invalid characters encountered in base64 data" );
                outStream.WriteByte( (byte)((num2 << 2) | (num4 >> 4)) );
                outStream.WriteByte( (byte)((num4 << 4) | (num6 >> 2)) );
                outStream.WriteByte( (byte)(((uint)num6 << 6) | num8) );
                num1 += 3;
            }
            return num1 + this.decodeLastBlock( outStream, data[length - 4], data[length - 3], data[length - 2], data[length - 1] );
        }

        private int decodeLastBlock( Stream outStream, char c1, char c2, char c3, char c4 )
        {
            if (c3 == padding)
            {
                byte num1 = this.decodingTable[c1];
                byte num2 = this.decodingTable[c2];
                if ((num1 | num2) >= 128)
                    throw new IOException( "invalid characters encountered at end of base64 data" );
                outStream.WriteByte( (byte)((num1 << 2) | (num2 >> 4)) );
                return 1;
            }
            if (c4 == padding)
            {
                byte num3 = this.decodingTable[c1];
                byte num4 = this.decodingTable[c2];
                byte num5 = this.decodingTable[c3];
                if ((num3 | num4 | num5) >= 128)
                    throw new IOException( "invalid characters encountered at end of base64 data" );
                outStream.WriteByte( (byte)((num3 << 2) | (num4 >> 4)) );
                outStream.WriteByte( (byte)((num4 << 4) | (num5 >> 2)) );
                return 2;
            }
            byte num6 = this.decodingTable[c1];
            byte num7 = this.decodingTable[c2];
            byte num8 = this.decodingTable[c3];
            byte num9 = this.decodingTable[c4];
            if ((num6 | num7 | num8 | num9) >= 128)
                throw new IOException( "invalid characters encountered at end of base64 data" );
            outStream.WriteByte( (byte)((num6 << 2) | (num7 >> 4)) );
            outStream.WriteByte( (byte)((num7 << 4) | (num8 >> 2)) );
            outStream.WriteByte( (byte)(((uint)num8 << 6) | num9) );
            return 3;
        }

        private int nextI( string data, int i, int finish )
        {
            while (i < finish && this.ignore( data[i] ))
                ++i;
            return i;
        }
    }
}
