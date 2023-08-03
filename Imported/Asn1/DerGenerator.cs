// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class DerGenerator : Asn1Generator
    {
        private bool _tagged = false;
        private bool _isExplicit;
        private int _tagNo;

        protected DerGenerator( Stream outStream )
          : base( outStream )
        {
        }

        protected DerGenerator( Stream outStream, int tagNo, bool isExplicit )
          : base( outStream )
        {
            this._tagged = true;
            this._isExplicit = isExplicit;
            this._tagNo = tagNo;
        }

        private static void WriteLength( Stream outStr, int length )
        {
            if (length > sbyte.MaxValue)
            {
                int num1 = 1;
                int num2 = length;
                while ((num2 >>= 8) != 0)
                    ++num1;
                outStr.WriteByte( (byte)(num1 | 128) );
                for (int index = (num1 - 1) * 8; index >= 0; index -= 8)
                    outStr.WriteByte( (byte)(length >> index) );
            }
            else
                outStr.WriteByte( (byte)length );
        }

        internal static void WriteDerEncoded( Stream outStream, int tag, byte[] bytes )
        {
            outStream.WriteByte( (byte)tag );
            WriteLength( outStream, bytes.Length );
            outStream.Write( bytes, 0, bytes.Length );
        }

        internal void WriteDerEncoded( int tag, byte[] bytes )
        {
            if (this._tagged)
            {
                int tag1 = this._tagNo | 128;
                if (this._isExplicit)
                {
                    int tag2 = this._tagNo | 32 | 128;
                    MemoryStream outStream = new();
                    WriteDerEncoded( outStream, tag, bytes );
                    WriteDerEncoded( this.Out, tag2, outStream.ToArray() );
                }
                else
                {
                    if ((tag & 32) != 0)
                        tag1 |= 32;
                    WriteDerEncoded( this.Out, tag1, bytes );
                }
            }
            else
                WriteDerEncoded( this.Out, tag, bytes );
        }

        internal static void WriteDerEncoded( Stream outStr, int tag, Stream inStr ) => WriteDerEncoded( outStr, tag, Streams.ReadAll( inStr ) );
    }
}
