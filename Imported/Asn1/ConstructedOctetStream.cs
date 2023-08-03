// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.ConstructedOctetStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    internal class ConstructedOctetStream : BaseInputStream
    {
        private readonly Asn1StreamParser _parser;
        private bool _first = true;
        private Stream _currentStream;

        internal ConstructedOctetStream( Asn1StreamParser parser ) => this._parser = parser;

        public override int Read( byte[] buffer, int offset, int count )
        {
            if (this._currentStream == null)
            {
                if (!this._first)
                    return 0;
                Asn1OctetStringParser octetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
                if (octetStringParser == null)
                    return 0;
                this._first = false;
                this._currentStream = octetStringParser.GetOctetStream();
            }
            int num1 = 0;
            while (true)
            {
                do
                {
                    int num2 = this._currentStream.Read( buffer, offset + num1, count - num1 );
                    if (num2 > 0)
                        num1 += num2;
                    else
                        goto label_10;
                }
                while (num1 != count);
                break;
            label_10:
                Asn1OctetStringParser octetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
                if (octetStringParser != null)
                    this._currentStream = octetStringParser.GetOctetStream();
                else
                    goto label_11;
            }
            return num1;
        label_11:
            this._currentStream = null;
            return num1;
        }

        public override int ReadByte()
        {
            if (this._currentStream == null)
            {
                if (!this._first)
                    return 0;
                Asn1OctetStringParser octetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
                if (octetStringParser == null)
                    return 0;
                this._first = false;
                this._currentStream = octetStringParser.GetOctetStream();
            }
            int num;
            while (true)
            {
                num = this._currentStream.ReadByte();
                if (num < 0)
                {
                    Asn1OctetStringParser octetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
                    if (octetStringParser != null)
                        this._currentStream = octetStringParser.GetOctetStream();
                    else
                        goto label_9;
                }
                else
                    break;
            }
            return num;
        label_9:
            this._currentStream = null;
            return -1;
        }
    }
}
