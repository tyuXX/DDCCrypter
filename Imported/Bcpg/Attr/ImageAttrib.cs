// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Attr.ImageAttrib
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Bcpg.Attr
{
    public class ImageAttrib : UserAttributeSubpacket
    {
        private static readonly byte[] Zeroes = new byte[12];
        private int hdrLength;
        private int _version;
        private int _encoding;
        private byte[] imageData;

        public ImageAttrib( byte[] data )
          : this( false, data )
        {
        }

        public ImageAttrib( bool forceLongLength, byte[] data )
          : base( UserAttributeSubpacketTag.ImageAttribute, forceLongLength, data )
        {
            this.hdrLength = ((data[1] & byte.MaxValue) << 8) | (data[0] & byte.MaxValue);
            this._version = data[2] & byte.MaxValue;
            this._encoding = data[3] & byte.MaxValue;
            this.imageData = new byte[data.Length - this.hdrLength];
            Array.Copy( data, this.hdrLength, imageData, 0, this.imageData.Length );
        }

        public ImageAttrib( ImageAttrib.Format imageType, byte[] imageData )
          : this( ToByteArray( imageType, imageData ) )
        {
        }

        private static byte[] ToByteArray( ImageAttrib.Format imageType, byte[] imageData )
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte( 16 );
            memoryStream.WriteByte( 0 );
            memoryStream.WriteByte( 1 );
            memoryStream.WriteByte( (byte)imageType );
            memoryStream.Write( Zeroes, 0, Zeroes.Length );
            memoryStream.Write( imageData, 0, imageData.Length );
            return memoryStream.ToArray();
        }

        public virtual int Version => this._version;

        public virtual int Encoding => this._encoding;

        public virtual byte[] GetImageData() => this.imageData;

        public enum Format : byte
        {
            Jpeg = 1,
        }
    }
}
