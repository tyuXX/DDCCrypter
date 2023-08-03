// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ArmoredInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public class ArmoredInputStream : BaseInputStream
    {
        private static readonly byte[] decodingTable = new byte[128];
        private Stream input;
        private bool start = true;
        private int[] outBuf = new int[3];
        private int bufPtr = 3;
        private Crc24 crc = new();
        private bool crcFound = false;
        private bool hasHeaders = true;
        private string header = null;
        private bool newLineFound = false;
        private bool clearText = false;
        private bool restart = false;
        private IList headerList = Platform.CreateArrayList();
        private int lastC = 0;
        private bool isEndOfStream;

        static ArmoredInputStream()
        {
            for (int index = 65; index <= 90; ++index)
                decodingTable[index] = (byte)(index - 65);
            for (int index = 97; index <= 122; ++index)
                decodingTable[index] = (byte)(index - 97 + 26);
            for (int index = 48; index <= 57; ++index)
                decodingTable[index] = (byte)(index - 48 + 52);
            decodingTable[43] = 62;
            decodingTable[47] = 63;
        }

        private int Decode( int in0, int in1, int in2, int in3, int[] result )
        {
            if (in3 < 0)
                throw new EndOfStreamException( "unexpected end of file in armored stream." );
            if (in2 == 61)
            {
                int num1 = decodingTable[in0] & byte.MaxValue;
                int num2 = decodingTable[in1] & byte.MaxValue;
                result[2] = ((num1 << 2) | (num2 >> 4)) & byte.MaxValue;
                return 2;
            }
            if (in3 == 61)
            {
                int num3 = decodingTable[in0];
                int num4 = decodingTable[in1];
                int num5 = decodingTable[in2];
                result[1] = ((num3 << 2) | (num4 >> 4)) & byte.MaxValue;
                result[2] = ((num4 << 4) | (num5 >> 2)) & byte.MaxValue;
                return 1;
            }
            int num6 = decodingTable[in0];
            int num7 = decodingTable[in1];
            int num8 = decodingTable[in2];
            int num9 = decodingTable[in3];
            result[0] = ((num6 << 2) | (num7 >> 4)) & byte.MaxValue;
            result[1] = ((num7 << 4) | (num8 >> 2)) & byte.MaxValue;
            result[2] = ((num8 << 6) | num9) & byte.MaxValue;
            return 0;
        }

        public ArmoredInputStream( Stream input )
          : this( input, true )
        {
        }

        public ArmoredInputStream( Stream input, bool hasHeaders )
        {
            this.input = input;
            this.hasHeaders = hasHeaders;
            if (hasHeaders)
                this.ParseHeaders();
            this.start = false;
        }

        private bool ParseHeaders()
        {
            this.header = null;
            int num1 = 0;
            bool headers = false;
            this.headerList = Platform.CreateArrayList();
            if (this.restart)
            {
                headers = true;
            }
            else
            {
                int num2;
                while ((num2 = this.input.ReadByte()) >= 0)
                {
                    if (num2 == 45 && (num1 == 0 || num1 == 10 || num1 == 13))
                    {
                        headers = true;
                        break;
                    }
                    num1 = num2;
                }
            }
            if (headers)
            {
                StringBuilder stringBuilder = new( "-" );
                bool flag1 = false;
                bool flag2 = false;
                if (this.restart)
                    stringBuilder.Append( '-' );
                int num3;
                while ((num3 = this.input.ReadByte()) >= 0)
                {
                    if (num1 == 13 && num3 == 10)
                        flag2 = true;
                    if ((!flag1 || num1 == 13 || num3 != 10) && (!flag1 || num3 != 13))
                    {
                        if (num3 == 13 || (num1 != 13 && num3 == 10))
                        {
                            string str = stringBuilder.ToString();
                            if (str.Trim().Length >= 1)
                            {
                                this.headerList.Add( str );
                                stringBuilder.Length = 0;
                            }
                            else
                                break;
                        }
                        if (num3 != 10 && num3 != 13)
                        {
                            stringBuilder.Append( (char)num3 );
                            flag1 = false;
                        }
                        else if (num3 == 13 || (num1 != 13 && num3 == 10))
                            flag1 = true;
                        num1 = num3;
                    }
                    else
                        break;
                }
                if (flag2)
                    this.input.ReadByte();
            }
            if (this.headerList.Count > 0)
                this.header = (string)this.headerList[0];
            this.clearText = "-----BEGIN PGP SIGNED MESSAGE-----".Equals( this.header );
            this.newLineFound = true;
            return headers;
        }

        public bool IsClearText() => this.clearText;

        public bool IsEndOfStream() => this.isEndOfStream;

        public string GetArmorHeaderLine() => this.header;

        public string[] GetArmorHeaders()
        {
            if (this.headerList.Count <= 1)
                return null;
            string[] armorHeaders = new string[this.headerList.Count - 1];
            for (int index = 0; index != armorHeaders.Length; ++index)
                armorHeaders[index] = (string)this.headerList[index + 1];
            return armorHeaders;
        }

        private int ReadIgnoreSpace()
        {
            int num;
            do
            {
                num = this.input.ReadByte();
            }
            while (num == 32 || num == 9);
            return num;
        }

        private int ReadIgnoreWhitespace()
        {
            int num;
            do
            {
                num = this.input.ReadByte();
            }
            while (num == 32 || num == 9 || num == 13 || num == 10);
            return num;
        }

        private int ReadByteClearText()
        {
            int num = this.input.ReadByte();
            switch (num)
            {
                case 10:
                    if (this.lastC == 13)
                        goto default;
                    else
                        goto case 13;
                case 13:
                    this.newLineFound = true;
                    break;
                default:
                    if (this.newLineFound && num == 45)
                    {
                        num = this.input.ReadByte();
                        if (num == 45)
                        {
                            this.clearText = false;
                            this.start = true;
                            this.restart = true;
                        }
                        else
                            num = this.input.ReadByte();
                        this.newLineFound = false;
                        break;
                    }
                    if (num != 10 && this.lastC != 13)
                    {
                        this.newLineFound = false;
                        break;
                    }
                    break;
            }
            this.lastC = num;
            if (num < 0)
                this.isEndOfStream = true;
            return num;
        }

        private int ReadClearText( byte[] buffer, int offset, int count )
        {
            int num1 = offset;
            try
            {
                int num2;
                for (int index = offset + count; num1 < index; buffer[num1++] = (byte)num2)
                {
                    num2 = this.ReadByteClearText();
                    if (num2 == -1)
                        break;
                }
            }
            catch (IOException ex)
            {
                if (num1 == offset)
                    throw ex;
            }
            return num1 - offset;
        }

        private int DoReadByte()
        {
            if (this.bufPtr > 2 || this.crcFound)
            {
                int in0 = this.ReadIgnoreSpace();
                switch (in0)
                {
                    case 10:
                    case 13:
                        in0 = this.ReadIgnoreWhitespace();
                        if (in0 == 61)
                        {
                            this.bufPtr = this.Decode( this.ReadIgnoreSpace(), this.ReadIgnoreSpace(), this.ReadIgnoreSpace(), this.ReadIgnoreSpace(), this.outBuf );
                            if (this.bufPtr != 0)
                                throw new IOException( "no crc found in armored message." );
                            this.crcFound = true;
                            if ((((this.outBuf[0] & byte.MaxValue) << 16) | ((this.outBuf[1] & byte.MaxValue) << 8) | (this.outBuf[2] & byte.MaxValue)) != this.crc.Value)
                                throw new IOException( "crc check failed in armored message." );
                            return this.ReadByte();
                        }
                        if (in0 == 45)
                        {
                            int num;
                            do
                                ;
                            while ((num = this.input.ReadByte()) >= 0 && num != 10 && num != 13);
                            this.crcFound = this.crcFound ? false : throw new IOException( "crc check not found." );
                            this.start = true;
                            this.bufPtr = 3;
                            if (num < 0)
                                this.isEndOfStream = true;
                            return -1;
                        }
                        break;
                }
                if (in0 < 0)
                {
                    this.isEndOfStream = true;
                    return -1;
                }
                this.bufPtr = this.Decode( in0, this.ReadIgnoreSpace(), this.ReadIgnoreSpace(), this.ReadIgnoreSpace(), this.outBuf );
            }
            return this.outBuf[this.bufPtr++];
        }

        public override int ReadByte()
        {
            if (this.start)
            {
                if (this.hasHeaders)
                    this.ParseHeaders();
                this.crc.Reset();
                this.start = false;
            }
            if (this.clearText)
                return this.ReadByteClearText();
            int b = this.DoReadByte();
            this.crc.Update( b );
            return b;
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            if (this.start && count > 0)
            {
                if (this.hasHeaders)
                    this.ParseHeaders();
                this.start = false;
            }
            if (this.clearText)
                return this.ReadClearText( buffer, offset, count );
            int num = offset;
            try
            {
                int b;
                for (int index = offset + count; num < index; buffer[num++] = (byte)b)
                {
                    b = this.DoReadByte();
                    this.crc.Update( b );
                    if (b == -1)
                        break;
                }
            }
            catch (IOException ex)
            {
                if (num == offset)
                    throw ex;
            }
            return num - offset;
        }

        public override void Close()
        {
            Platform.Dispose( this.input );
            base.Close();
        }
    }
}
