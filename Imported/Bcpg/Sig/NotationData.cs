// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.NotationData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;
using System.Text;

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class NotationData : SignatureSubpacket
    {
        public const int HeaderFlagLength = 4;
        public const int HeaderNameLength = 2;
        public const int HeaderValueLength = 2;

        public NotationData( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.NotationData, critical, isLongLength, data )
        {
        }

        public NotationData(
          bool critical,
          bool humanReadable,
          string notationName,
          string notationValue )
          : base( SignatureSubpacketTag.NotationData, critical, false, CreateData( humanReadable, notationName, notationValue ) )
        {
        }

        private static byte[] CreateData( bool humanReadable, string notationName, string notationValue )
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte( humanReadable ? (byte)128 : (byte)0 );
            memoryStream.WriteByte( 0 );
            memoryStream.WriteByte( 0 );
            memoryStream.WriteByte( 0 );
            byte[] bytes1 = Encoding.UTF8.GetBytes( notationName );
            int count1 = System.Math.Min( bytes1.Length, (int)byte.MaxValue );
            byte[] bytes2 = Encoding.UTF8.GetBytes( notationValue );
            int count2 = System.Math.Min( bytes2.Length, (int)byte.MaxValue );
            memoryStream.WriteByte( (byte)(count1 >> 8) );
            memoryStream.WriteByte( (byte)count1 );
            memoryStream.WriteByte( (byte)(count2 >> 8) );
            memoryStream.WriteByte( (byte)count2 );
            memoryStream.Write( bytes1, 0, count1 );
            memoryStream.Write( bytes2, 0, count2 );
            return memoryStream.ToArray();
        }

        public bool IsHumanReadable => this.data[0] == 128;

        public string GetNotationName() => Encoding.UTF8.GetString( this.data, 8, (this.data[4] << 8) + this.data[5] );

        public string GetNotationValue() => Encoding.UTF8.GetString( this.data, 8 + (this.data[4] << 8) + this.data[5], (this.data[6] << 8) + this.data[7] );

        public byte[] GetNotationValueBytes()
        {
            int num = (this.data[4] << 8) + this.data[5];
            int length = (this.data[6] << 8) + this.data[7];
            int sourceIndex = 8 + num;
            byte[] destinationArray = new byte[length];
            Array.Copy( data, sourceIndex, destinationArray, 0, length );
            return destinationArray;
        }
    }
}
